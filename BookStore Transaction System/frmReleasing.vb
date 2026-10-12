Imports MySql.Data.MySqlClient
Imports System.Data
Imports System.Drawing.Printing
Imports System.Linq

Public Class frmReleasing
    Private Const ST_RELEASED As String = "Completed"
    Private Const ST_READY As String = "Ready for release"

    Private pg As GridPager
    Private isLoading As Boolean = True
    Private WithEvents tmrSearch As New System.Windows.Forms.Timer With {.Interval = 350}
    ' ==================== LOAD ====================
    Private Sub frmReleasing_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        ApplySearchPlaceholders(Me)
        SetupFooter(Me, lblname, lblposition, lbldatetime)

        With dgvstockinhistory
            .AutoGenerateColumns = False
            .AllowUserToAddRows = False
            .AllowUserToDeleteRows = False
            .ReadOnly = True
            .MultiSelect = True
            .SelectionMode = DataGridViewSelectionMode.FullRowSelect
        End With
        BindCol("TransactionNo", "transaction_no")
        BindCol("ORNo", "or_no")
        BindCol("BuyerName", "buyer_name")
        BindCol("IDNumber", "id_number")
        BindCol("ProductCode", "product_code")
        BindCol("ProductName", "item_name")
        BindCol("Quantity", "qty_n")
        BindCol("PickUpdate", "pickup_date")
        BindCol("InStock", "in_stock")
        BindCol("Status", "rel_status")
        BindCol("ReleasedBy", "released_by_name")
        BindCol("ReleasedTo", "received_by")
        dgvstockinhistory.Columns("PickUpdate").DefaultCellStyle.Format = "MMM d, yyyy"

        cbocategory.DropDownStyle = ComboBoxStyle.DropDownList
        cbocategory.Items.Clear()
        cbocategory.Items.Add("Modules")
        cbocategory.SelectedIndex = 0
        cbocategory.Enabled = False

        Button1.Text = "Print Payment Slip"
        btnexportexcel.Text = "Release Modules"
        Label7.Text = "MODULES FOR RELEASE"

        DateTimePicker1.Value = Date.Today.AddYears(-1)
        DateTimePicker2.Value = Date.Today
        ApplySchoolYearToPickers(DateTimePicker1, DateTimePicker2)

        btnexportexcel.Enabled = Not RolePermissions.IsViewOnly()      ' "Release Item/s"

        pg = New GridPager(dgvstockinhistory, 20)
        AddHandler pg.PageChanged, Sub() LoadRows()

        isLoading = False
        LoadRows()
    End Sub

    Private Sub frmReleasing_FormClosed(sender As Object, e As FormClosedEventArgs) Handles MyBase.FormClosed
        tmrSearch.Stop()
    End Sub

    Private Sub BindCol(colName As String, field As String)
        dgvstockinhistory.Columns(colName).DataPropertyName = field
    End Sub

    ' ==================== FILTERS ====================
    Private Sub txtsearch_TextChanged(sender As Object, e As EventArgs) Handles txtsearch.TextChanged
        If isLoading Then Exit Sub
        tmrSearch.Stop()
        tmrSearch.Start()
    End Sub

    Private Sub tmrSearch_Tick(sender As Object, e As EventArgs) Handles tmrSearch.Tick
        tmrSearch.Stop()
        pg.Reset()
        LoadRows()
    End Sub

    Private Sub cbocategory_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cbocategory.SelectedIndexChanged
        If isLoading Then Exit Sub
        pg.Reset()
        LoadRows()
    End Sub

    Private Sub btngenerate_Click(sender As Object, e As EventArgs) Handles btngenerate.Click       ' "Load Data"
        If DateTimePicker1.Value.Date > DateTimePicker2.Value.Date Then
            MsgBox("'Date from' cannot be later than 'To'.", vbExclamation, "Releasing")
            Exit Sub
        End If
        pg.Reset()
        LoadRows()
    End Sub
    Private Sub LoadRows()
        If isLoading OrElse pg Is Nothing Then Exit Sub

        If DateTimePicker1.Value.Date >
        DateTimePicker2.Value.Date Then
            Exit Sub
        End If

        Dim query As String =
        "SELECT * FROM (" &
        "SELECT t.transaction_id, ti.transaction_item_id, " &
        "ti.variant_id, ti.quantity AS qty_n, " &
        "t.transaction_no, t.or_no, t.buyer_name, t.id_number, " &
        "v.product_code, c.category_name, " &
        "CONCAT(p.product_name, " &
        "IF(v.size IS NULL OR v.size IN ('', 'N/A'), '', " &
        "CONCAT(' (', v.size, ')'))) AS item_name, " &
        "ti.pickup_date, v.quantity_on_hand AS in_stock, " &
        "CASE " &
        "WHEN ti.released_at IS NOT NULL THEN 'Completed' " &
        "WHEN NULLIF(TRIM(t.or_no), '') IS NULL " &
        "THEN 'Waiting for OR No.' " &
        "WHEN v.quantity_on_hand < ti.quantity " &
        "THEN 'Waiting for stock' " &
        "ELSE 'Ready for release' END AS rel_status, " &
        "CONCAT(u.first_name, ' ', u.last_name) " &
        "AS released_by_name, ti.received_by, ti.released_at " &
        "FROM tbl_transaction_items ti " &
        "INNER JOIN tbl_transactions t " &
        "ON ti.transaction_id = t.transaction_id " &
        "INNER JOIN tbl_product_variants v " &
        "ON ti.variant_id = v.variant_id " &
        "INNER JOIN tbl_products p ON v.product_id = p.product_id " &
        "INNER JOIN tbl_category_types ct " &
        "ON p.category_type_id = ct.category_type_id " &
        "INNER JOIN tbl_categories c " &
        "ON ct.category_id = c.category_id " &
        "LEFT JOIN tbl_users u ON ti.released_by = u.user_id " &
        "WHERE c.category_name = 'Modules' " &
        "AND t.payment_method = @method " &
        "AND t.status <> 'Cancelled' " &
        "AND t.created_at >= @dateFrom " &
        "AND t.created_at < @dateTo" &
        ") x " &
        "WHERE (x.transaction_no LIKE @search " &
        "OR x.or_no LIKE @search " &
        "OR x.buyer_name LIKE @search " &
        "OR x.item_name LIKE @search " &
        "OR x.product_code LIKE @search " &
        "OR x.rel_status LIKE @search) " &
        "ORDER BY (x.released_at IS NOT NULL), " &
        "x.transaction_id DESC, x.transaction_item_id"

        dgvstockinhistory.DataSource = pg.LoadPage(
        query,
        New String() {
            "@method", "@dateFrom", "@dateTo", "@search"
        },
        New Object() {
            ModuleOrders.PAYMENT_METHOD,
            DateTimePicker1.Value.Date,
            DateTimePicker2.Value.Date.AddDays(1),
            "%" & txtSearch.Text.Trim() & "%"
        })

        Label3.Text = pg.TotalRows.ToString("N0")
    End Sub

    ' overdue pick-up = red, already released = grey
    Private Sub dgvstockinhistory_DataBindingComplete(sender As Object, e As DataGridViewBindingCompleteEventArgs) Handles dgvstockinhistory.DataBindingComplete
        For Each row As DataGridViewRow In dgvstockinhistory.Rows
            Dim d As DataRowView = TryCast(row.DataBoundItem, DataRowView)
            If d Is Nothing Then Continue For
            Dim st As String = d("rel_status").ToString()
            Dim overdue As Boolean = st <> ST_RELEASED AndAlso Not IsDBNull(d("pickup_date")) AndAlso
                                     Convert.ToDateTime(d("pickup_date")).Date < Date.Today
            row.DefaultCellStyle.ForeColor = If(st = ST_RELEASED, Color.Gray, If(overdue, Color.Firebrick, Color.Black))
        Next
        dgvstockinhistory.ClearSelection()
    End Sub

    Private Function SelectedItems() As List(Of DataRowView)
        Dim list As New List(Of DataRowView)
        For Each r As DataGridViewRow In dgvstockinhistory.SelectedRows
            Dim d As DataRowView = TryCast(r.DataBoundItem, DataRowView)
            If d IsNot Nothing Then list.Add(d)
        Next
        Return list
    End Function

    Private Sub btnexportexcel_Click(
    sender As Object,
    e As EventArgs
) Handles btnexportexcel.Click

        If RolePermissions.IsViewOnly() OrElse
        Not (
            RolePermissions.CanAccess(RolePermissions.FRM_POS) OrElse
            RolePermissions.CanAccess(RolePermissions.FRM_INVENTORY)
        ) Then

            MsgBox(
            "Your role cannot release modules.",
            vbExclamation,
            "Module Release")
            Exit Sub
        End If

        Dim items As List(Of DataRowView) = SelectedItems()

        If items.Count = 0 Then
            MsgBox(
            "Select a module order first.",
            vbExclamation,
            "Module Release")
            Exit Sub
        End If

        Dim transactionId As Integer =
        Convert.ToInt32(items(0)("transaction_id"))

        For Each item As DataRowView In items
            If Convert.ToInt32(item("transaction_id")) <>
            transactionId Then

                MsgBox(
                "Select items from one transaction only.",
                vbExclamation,
                "Module Release")
                Exit Sub
            End If
        Next

        Dim orNumber As String = InputBox(
        "Enter the OR number from the school cashier." &
        vbCrLf &
        "Check it against the customer's receipt.",
        "School Cashier OR",
        items(0)("or_no").ToString()).Trim()

        If orNumber = "" Then Exit Sub

        If orNumber.Length > 30 OrElse
        orNumber.Any(Function(character) Char.IsControl(character)) Then

            MsgBox(
            "The OR number must contain 1 to 30 printable characters.",
            vbExclamation,
            "Module Release")
            Exit Sub
        End If

        Dim receiver As String = InputBox(
        "Who is receiving the modules?",
        "Module Receiver",
        items(0)("buyer_name").ToString()).Trim()

        If receiver = "" Then Exit Sub

        If receiver.Length > 100 Then
            MsgBox(
            "Receiver name must not exceed 100 characters.",
            vbExclamation,
            "Module Release")
            Exit Sub
        End If

        If MsgBox(
        "Record OR No. " & orNumber &
        " and release ALL remaining modules in transaction " &
        items(0)("transaction_no").ToString() &
        " to " & receiver & "?" &
        vbCrLf & vbCrLf &
        "Stock will decrease. No bookstore cash payment " &
        "will be recorded.",
        vbYesNo + vbQuestion + vbDefaultButton2,
        "Confirm Module Release"
    ) <> MsgBoxResult.Yes Then
            Exit Sub
        End If

        Try
            ReleaseModuleOrder(transactionId, orNumber, receiver)

            MsgBox(
            "Modules released. Transaction status: Completed.",
            vbInformation,
            "Module Release")

            LoadRows()
        Catch ex As Exception
            MsgBox(
            "Modules were not released: " & ex.Message,
            vbCritical,
            "Module Release")
        End Try
    End Sub
    Private Sub ReleaseModuleOrder(
    transactionId As Integer,
    orNumber As String,
    receiver As String)

        Dim transactionNo As String

        Using connection As MySqlConnection = NewConnection()
            connection.Open()

            Using transaction As MySqlTransaction =
            connection.BeginTransaction()

                Dim header As New DataTable()

                Using command As New MySqlCommand(
                "SELECT transaction_no, payment_method, status, or_no " &
                "FROM tbl_transactions " &
                "WHERE transaction_id = @id FOR UPDATE",
                connection,
                transaction)

                    command.Parameters.AddWithValue("@id", transactionId)

                    Using reader As MySqlDataReader = command.ExecuteReader()
                        header.Load(reader)
                    End Using
                End Using

                If header.Rows.Count <> 1 Then
                    Throw New InvalidOperationException(
                    "The module order could not be found.")
                End If

                Dim order As DataRow = header.Rows(0)
                transactionNo = order("transaction_no").ToString()

                If order("payment_method").ToString() <>
                ModuleOrders.PAYMENT_METHOD OrElse
                order("status").ToString() = "Cancelled" Then

                    Throw New InvalidOperationException(
                    "This transaction cannot be released as a module order.")
                End If

                Dim existingOR As String = order("or_no").ToString().Trim()

                If existingOR <> "" AndAlso
                Not String.Equals(
                    existingOR,
                    orNumber,
                    StringComparison.OrdinalIgnoreCase) Then

                    Throw New InvalidOperationException(
                    "This order already has a different OR number.")
                End If

                Dim details As New DataTable()

                Using command As New MySqlCommand(
                "SELECT ti.transaction_item_id, ti.variant_id, " &
                "ti.quantity, ti.released_at, c.category_name " &
                "FROM tbl_transaction_items ti " &
                "INNER JOIN tbl_product_variants v " &
                "ON ti.variant_id = v.variant_id " &
                "INNER JOIN tbl_products p " &
                "ON v.product_id = p.product_id " &
                "INNER JOIN tbl_category_types ct " &
                "ON p.category_type_id = ct.category_type_id " &
                "INNER JOIN tbl_categories c " &
                "ON ct.category_id = c.category_id " &
                "WHERE ti.transaction_id = @id " &
                "ORDER BY ti.variant_id, ti.transaction_item_id FOR UPDATE",
                connection,
                transaction)

                    command.Parameters.AddWithValue("@id", transactionId)

                    Using reader As MySqlDataReader = command.ExecuteReader()
                        details.Load(reader)
                    End Using
                End Using

                If details.Rows.Count = 0 Then
                    Throw New InvalidOperationException(
                    "The order has no module items.")
                End If

                Dim releaseCount As Integer = 0

                For Each item As DataRow In details.Rows
                    If item("category_name").ToString() <> "Modules" Then
                        Throw New InvalidOperationException(
                        "This order contains a non-module product.")
                    End If

                    If Not IsDBNull(item("released_at")) Then Continue For

                    Dim variantId As Integer =
                    Convert.ToInt32(item("variant_id"))

                    Dim quantity As Integer =
                    Convert.ToInt32(item("quantity"))

                    If quantity <= 0 Then
                        Throw New InvalidOperationException(
                        "An order item has an invalid quantity.")
                    End If

                    Dim previousQuantity As Integer

                    Using command As New MySqlCommand(
                    "SELECT quantity_on_hand " &
                    "FROM tbl_product_variants " &
                    "WHERE variant_id = @id FOR UPDATE",
                    connection,
                    transaction)

                        command.Parameters.AddWithValue("@id", variantId)

                        Dim value As Object = command.ExecuteScalar()

                        If value Is Nothing OrElse IsDBNull(value) Then
                            Throw New InvalidOperationException(
                            "The module stock record is missing.")
                        End If

                        previousQuantity = Convert.ToInt32(value)
                    End Using

                    If previousQuantity < quantity Then
                        Throw New InvalidOperationException(
                        "Insufficient stock for module variant " &
                        variantId.ToString() &
                        ". Available: " & previousQuantity.ToString() &
                        "; required: " & quantity.ToString() & ".")
                    End If

                    Using command As New MySqlCommand(
                    "UPDATE tbl_product_variants " &
                    "SET quantity_on_hand = quantity_on_hand - @quantity " &
                    "WHERE variant_id = @id",
                    connection,
                    transaction)

                        command.Parameters.AddWithValue("@quantity", quantity)
                        command.Parameters.AddWithValue("@id", variantId)
                        command.ExecuteNonQuery()
                    End Using

                    Using command As New MySqlCommand(
                    "UPDATE tbl_transaction_items " &
                    "SET released_at = NOW(), released_by = @userId, " &
                    "received_by = @receiver " &
                    "WHERE transaction_item_id = @itemId " &
                    "AND released_at IS NULL",
                    connection,
                    transaction)

                        command.Parameters.AddWithValue(
                        "@userId", currentuser.UserID)
                        command.Parameters.AddWithValue("@receiver", receiver)
                        command.Parameters.AddWithValue(
                        "@itemId", item("transaction_item_id"))

                        If command.ExecuteNonQuery() <> 1 Then
                            Throw New InvalidOperationException(
                            "An item was already released. Refresh the list.")
                        End If
                    End Using

                    Using command As New MySqlCommand(
                    "INSERT INTO tbl_stock_movements " &
                    "(variant_id, movement_type, quantity, " &
                    "previous_quantity, new_quantity, reference_no, " &
                    "remarks, created_by) " &
                    "VALUES (@variantId, 'Stock Out', @quantity, " &
                    "@previous, @new, @reference, @remarks, @userId)",
                    connection,
                    transaction)

                        command.Parameters.AddWithValue("@variantId", variantId)
                        command.Parameters.AddWithValue("@quantity", quantity)
                        command.Parameters.AddWithValue(
                        "@previous", previousQuantity)
                        command.Parameters.AddWithValue(
                        "@new", previousQuantity - quantity)
                        command.Parameters.AddWithValue(
                        "@reference", transactionNo)
                        command.Parameters.AddWithValue(
                        "@remarks",
                        "Module released to " & receiver &
                        "; school cashier OR " & orNumber)
                        command.Parameters.AddWithValue(
                        "@userId", currentuser.UserID)

                        command.ExecuteNonQuery()
                    End Using

                    releaseCount += 1
                Next

                If releaseCount = 0 Then
                    Throw New InvalidOperationException(
                    "All modules in this order were already released.")
                End If

                Using command As New MySqlCommand(
                "UPDATE tbl_transactions " &
                "SET or_no = @orNumber, status = 'Completed' " &
                "WHERE transaction_id = @id",
                connection,
                transaction)

                    command.Parameters.AddWithValue("@orNumber", orNumber)
                    command.Parameters.AddWithValue("@id", transactionId)
                    command.ExecuteNonQuery()
                End Using

                transaction.Commit()
            End Using
        End Using

        LogActivity(
        "Module Release",
        transactionNo,
        "Modules released to " & receiver &
        ". School cashier OR: " & orNumber &
        ". No bookstore cash collected.")
    End Sub

    ' ==================== PRINT SLIP ====================
    Private Sub Button1_Click(
    sender As Object,
    e As EventArgs
) Handles Button1.Click

        Dim items As List(Of DataRowView) = SelectedItems()

        If items.Count = 0 Then
            MsgBox(
            "Select a module order to print its payment slip.",
            vbExclamation,
            "Module Payment Slip")
            Exit Sub
        End If

        Dim transactionId As Integer =
        Convert.ToInt32(items(0)("transaction_id"))

        For Each item As DataRowView In items
            If Convert.ToInt32(item("transaction_id")) <>
            transactionId Then

                MsgBox(
                "Select items from one transaction only.",
                vbExclamation,
                "Module Payment Slip")
                Exit Sub
            End If
        Next

        Try
            ModuleOrders.PrintPaymentSlip(transactionId, Me)
        Catch ex As Exception
            MsgBox(
            "Could not open the payment slip: " & ex.Message,
            vbCritical,
            "Module Payment Slip")
        End Try
    End Sub
End Class