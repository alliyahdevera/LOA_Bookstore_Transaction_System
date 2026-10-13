Imports System.Data
Imports System.Drawing
Imports System.Drawing.Printing
Imports System.Windows.Forms
Imports MySql.Data.MySqlClient

Public Module ModuleOrders

    Public Const PAYMENT_METHOD As String = "School Cashier"
    Public Const WAITING_STATUS As String = "Waiting for OR No."

    Public Function IsModuleVariant(variantId As Integer) As Boolean
        Dim result As Object = ExecScalar(
            "SELECT c.category_name " &
            "FROM tbl_product_variants v " &
            "INNER JOIN tbl_products p ON v.product_id = p.product_id " &
            "INNER JOIN tbl_category_types ct " &
            "ON p.category_type_id = ct.category_type_id " &
            "INNER JOIN tbl_categories c ON ct.category_id = c.category_id " &
            "WHERE v.variant_id = @variantId",
            New String() {"@variantId"},
            New Object() {variantId})

        If result Is Nothing OrElse Convert.IsDBNull(result) Then
            Throw New InvalidOperationException(
                "The product category could not be verified.")
        End If

        Return String.Equals(
            Convert.ToString(result),
            "Modules",
            StringComparison.OrdinalIgnoreCase)
    End Function

    Public Function SaveOrder(
        buyer As IBuyerInfo,
        cart As DataGridView,
        transactionNo As String
    ) As Long

        If RolePermissions.IsViewOnly() OrElse
            Not RolePermissions.CanAccess(RolePermissions.FRM_POS) Then

            Throw New InvalidOperationException(
                "Your role cannot save module orders.")
        End If

        If buyer Is Nothing Then
            Throw New InvalidOperationException("Select a buyer first.")
        End If

        Dim validationMessage As String = ""

        If Not buyer.ValidateBuyer(validationMessage) Then
            Throw New InvalidOperationException(validationMessage)
        End If

        Dim quantities As New SortedDictionary(Of Integer, Integer)
        Dim displayedTotals As New Dictionary(Of Integer, Decimal)

        For Each row As DataGridViewRow In cart.Rows
            If row.IsNewRow Then Continue For

            Dim variantId As Integer = Convert.ToInt32(row.Tag)
            Dim quantity As Integer =
                Convert.ToInt32(row.Cells("Quantity").Value)

            If quantity <= 0 Then
                Throw New InvalidOperationException(
                    "Every module quantity must be greater than zero.")
            End If

            If quantities.ContainsKey(variantId) Then
                Throw New InvalidOperationException(
                    "The cart contains a duplicate product row.")
            End If

            quantities.Add(variantId, quantity)
            displayedTotals.Add(
                variantId,
                Convert.ToDecimal(row.Cells("SubTotal").Value))
        Next

        If quantities.Count = 0 Then
            Throw New InvalidOperationException("The cart is empty.")
        End If

        Dim transactionId As Long
        Dim verifiedTotals As New Dictionary(Of Integer, Decimal)
        Dim grandTotal As Decimal = 0D

        Using connection As MySqlConnection = NewConnection()
            connection.Open()

            Using transaction As MySqlTransaction =
                connection.BeginTransaction()

                For Each item As KeyValuePair(Of Integer, Integer) _
                    In quantities

                    Dim product As New DataTable()

                    Using command As New MySqlCommand(
                        "SELECT p.unit_price, p.status, c.category_name " &
                        "FROM tbl_product_variants v " &
                        "INNER JOIN tbl_products p " &
                        "ON v.product_id = p.product_id " &
                        "INNER JOIN tbl_category_types ct " &
                        "ON p.category_type_id = ct.category_type_id " &
                        "INNER JOIN tbl_categories c " &
                        "ON ct.category_id = c.category_id " &
                        "WHERE v.variant_id = @variantId FOR UPDATE",
                        connection,
                        transaction)

                        command.Parameters.AddWithValue(
                            "@variantId", item.Key)

                        Using reader As MySqlDataReader =
                            command.ExecuteReader()

                            product.Load(reader)
                        End Using
                    End Using

                    If product.Rows.Count <> 1 Then
                        Throw New InvalidOperationException(
                            "A module product could not be found.")
                    End If

                    Dim record As DataRow = product.Rows(0)

                    If Not String.Equals(
                        record("category_name").ToString(),
                        "Modules",
                        StringComparison.OrdinalIgnoreCase) OrElse
                        record("status").ToString() <> "Active" Then

                        Throw New InvalidOperationException(
                            "Only active modules can be saved in this order.")
                    End If

                    Dim subtotal As Decimal =
                        Decimal.Round(
                            Convert.ToDecimal(record("unit_price")) *
                            item.Value,
                            2,
                            MidpointRounding.AwayFromZero)

                    If subtotal <> displayedTotals(item.Key) Then
                        Throw New InvalidOperationException(
                            "A module price has changed. Remove the item " &
                            "and add it again before saving.")
                    End If

                    verifiedTotals.Add(item.Key, subtotal)
                    grandTotal += subtotal
                Next

                Using command As New MySqlCommand(
                    "INSERT INTO tbl_transactions " &
                    "(transaction_no, buyer_type, student_id, id_number, " &
                    "buyer_name, or_no, or_date, payment_method, " &
                    "total_amount, amount_paid, amount_change, " &
                    "created_by, status) " &
                    "VALUES (@number, @type, @studentId, @idNumber, " &
                    "@buyerName, NULL, CURDATE(), @method, @total, " &
                    "0, 0, @userId, @status)",
                    connection,
                    transaction)

                    command.Parameters.AddWithValue(
                        "@number", transactionNo)
                    command.Parameters.AddWithValue(
                        "@type", buyer.BuyerType)
                    command.Parameters.AddWithValue(
                        "@studentId",
                        If(buyer.StudentId > 0,
                           CType(buyer.StudentId, Object),
                           DBNull.Value))
                    command.Parameters.AddWithValue(
                        "@idNumber",
                        If(String.IsNullOrWhiteSpace(buyer.IdNumber),
                           CType(DBNull.Value, Object),
                           buyer.IdNumber.Trim()))
                    command.Parameters.AddWithValue(
                        "@buyerName", buyer.BuyerName)
                    command.Parameters.AddWithValue(
                        "@method", PAYMENT_METHOD)
                    command.Parameters.AddWithValue(
                        "@total", grandTotal)
                    command.Parameters.AddWithValue(
                        "@userId", currentuser.UserID)
                    command.Parameters.AddWithValue(
                        "@status", WAITING_STATUS)

                    command.ExecuteNonQuery()
                    transactionId = command.LastInsertedId
                End Using

                For Each item As KeyValuePair(Of Integer, Integer) _
                    In quantities

                    Using command As New MySqlCommand(
                        "INSERT INTO tbl_transaction_items " &
                        "(transaction_id, variant_id, quantity, subtotal, " &
                        "is_backorder, pickup_date, released_at, " &
                        "released_by, received_by) " &
                        "VALUES (@transactionId, @variantId, @quantity, " &
                        "@subtotal, 1, NULL, NULL, NULL, NULL)",
                        connection,
                        transaction)

                        command.Parameters.AddWithValue(
                            "@transactionId", transactionId)
                        command.Parameters.AddWithValue(
                            "@variantId", item.Key)
                        command.Parameters.AddWithValue(
                            "@quantity", item.Value)
                        command.Parameters.AddWithValue(
                            "@subtotal", verifiedTotals(item.Key))

                        command.ExecuteNonQuery()
                    End Using
                Next

                transaction.Commit()
            End Using
        End Using

        LogActivity(
            "Module Order",
            transactionNo,
            "Module order saved for " & buyer.BuyerName &
            ". Payment is handled by the school cashier.")

        Return transactionId
    End Function

    Public Sub PrintPaymentSlip(
        transactionId As Long,
        owner As IWin32Window)

        Dim result As DataTable = GetDataTable(
            "SELECT t.transaction_no, t.buyer_name, t.id_number, " &
            "t.total_amount, t.status, ti.quantity, ti.subtotal, " &
            "p.product_name, v.size " &
            "FROM tbl_transactions t " &
            "INNER JOIN tbl_transaction_items ti " &
            "ON ti.transaction_id = t.transaction_id " &
            "INNER JOIN tbl_product_variants v " &
            "ON ti.variant_id = v.variant_id " &
            "INNER JOIN tbl_products p ON v.product_id = p.product_id " &
            "WHERE t.transaction_id = @transactionId " &
            "AND t.payment_method = @method " &
            "ORDER BY ti.transaction_item_id",
            New String() {"@transactionId", "@method"},
            New Object() {transactionId, PAYMENT_METHOD})

        If result Is Nothing OrElse result.Rows.Count = 0 Then
            Throw New InvalidOperationException(
                "The module payment slip could not be loaded.")
        End If

        If result.Rows(0)("status").ToString() = "Cancelled" Then
            Throw New InvalidOperationException(
                "A cancelled order cannot have a payment slip.")
        End If

        Dim lines As New List(Of String) From {
            "LOA BOOKSTORE",
            "MODULE PAYMENT SLIP",
            "",
            "Transaction: " &
                result.Rows(0)("transaction_no").ToString(),
            "Buyer: " & result.Rows(0)("buyer_name").ToString(),
            "ID No.: " &
                If(String.IsNullOrWhiteSpace(
                       result.Rows(0)("id_number").ToString()),
                   "N/A",
                   result.Rows(0)("id_number").ToString()),
            "",
            "Present this slip to the school cashier.",
            "This slip is not proof of payment or release.",
            ""
        }

        For Each row As DataRow In result.Rows
            Dim itemName As String = row("product_name").ToString()
            Dim itemSize As String = row("size").ToString()

            If itemSize <> "" AndAlso itemSize <> "N/A" Then
                itemName &= " (" & itemSize & ")"
            End If

            lines.Add(row("quantity").ToString() & " x " & itemName)
            lines.Add(
                "    Amount: PHP " &
                Convert.ToDecimal(row("subtotal")).ToString("N2"))
        Next

        lines.Add("")
        lines.Add(
            "TOTAL: PHP " &
            Convert.ToDecimal(
                result.Rows(0)("total_amount")).ToString("N2"))
        lines.Add("")
        lines.Add("Return with the school cashier's receipt or OR No.")
        lines.Add("Modules are released after the OR is recorded.")

        Using document As New PrintDocument(),
            bodyFont As New Font("Consolas", 10)

            document.DefaultPageSettings.Margins =
                New Margins(40, 40, 40, 40)

            Dim printableLines As New List(Of String)
            Dim nextLine As Integer = 0

            AddHandler document.BeginPrint,
                Sub(sender, args)
                    nextLine = 0
                    printableLines.Clear()
                End Sub

            AddHandler document.PrintPage,
                Sub(sender, args)
                    ' Wrap long lines to fit the actual printer page.
                    If printableLines.Count = 0 Then
                        For Each originalLine As String In lines
                            If originalLine = "" Then
                                printableLines.Add("")
                                Continue For
                            End If

                            Dim remaining As String = originalLine

                            While remaining.Length > 0
                                Dim length As Integer = remaining.Length

                                While length > 1 AndAlso
                                    args.Graphics.MeasureString(
                                        remaining.Substring(0, length),
                                        bodyFont).Width >
                                    args.MarginBounds.Width

                                    length -= 1
                                End While

                                If length < remaining.Length Then
                                    Dim space As Integer =
                                        remaining.LastIndexOf(
                                            " "c, length - 1)

                                    If space > 0 Then length = space
                                End If

                                printableLines.Add(
                                    remaining.Substring(0, length).
                                        TrimEnd())

                                remaining =
                                    remaining.Substring(length).
                                        TrimStart()
                            End While
                        Next
                    End If

                    Dim lineHeight As Single =
                        bodyFont.GetHeight(args.Graphics) + 4

                    Dim y As Single = args.MarginBounds.Top

                    While nextLine < printableLines.Count AndAlso
                        y + lineHeight <= args.MarginBounds.Bottom

                        args.Graphics.DrawString(
                            printableLines(nextLine),
                            bodyFont,
                            Brushes.Black,
                            args.MarginBounds.Left,
                            y)

                        y += lineHeight
                        nextLine += 1
                    End While

                    args.HasMorePages =
                        nextLine < printableLines.Count
                End Sub

            Using preview As New PrintPreviewDialog()
                preview.Document = document
                preview.WindowState = FormWindowState.Maximized
                preview.ShowDialog(owner)
            End Using
        End Using
    End Sub

End Module