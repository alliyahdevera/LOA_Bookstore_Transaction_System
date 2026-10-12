Imports System.Data
Imports System.Drawing
Imports System.Drawing.Printing
Imports System.Windows.Forms
Imports MySql.Data.MySqlClient

Public Module SalaryDeductions

    Public Const METHOD As String = "Salary Deduction"
    Public Const PENDING_STATUS As String = "Pending Finance"

    Public Function VerifyEmployeeAndCart(
        connection As MySqlConnection,
        transaction As MySqlTransaction,
        buyer As IBuyerInfo,
        cart As DataGridView
    ) As Integer

        If buyer Is Nothing OrElse
            buyer.BuyerType <> "Employee" OrElse
            String.IsNullOrWhiteSpace(buyer.IdNumber) Then

            Throw New InvalidOperationException(
                "Select an employee buyer before using salary deduction.")
        End If

        Dim employeeId As Integer

        Using command As New MySqlCommand(
            "SELECT employee_id FROM tbl_employees " &
            "WHERE employee_no = @number AND status = 'Active' " &
            "FOR UPDATE",
            connection,
            transaction)

            command.Parameters.AddWithValue(
                "@number", buyer.IdNumber.Trim())

            Dim result As Object = command.ExecuteScalar()

            If result Is Nothing OrElse IsDBNull(result) Then
                Throw New InvalidOperationException(
                    "The selected employee is missing or inactive.")
            End If

            employeeId = Convert.ToInt32(result)
        End Using

        Dim itemCount As Integer = 0

        For Each row As DataGridViewRow In cart.Rows
            If row.IsNewRow Then Continue For

            itemCount += 1

            Dim category As Object

            Using command As New MySqlCommand(
                "SELECT c.category_name " &
                "FROM tbl_product_variants v " &
                "INNER JOIN tbl_products p " &
                "ON v.product_id = p.product_id " &
                "INNER JOIN tbl_category_types ct " &
                "ON p.category_type_id = ct.category_type_id " &
                "INNER JOIN tbl_categories c " &
                "ON ct.category_id = c.category_id " &
                "WHERE v.variant_id = @variantId " &
                "AND p.status = 'Active' FOR UPDATE",
                connection,
                transaction)

                command.Parameters.AddWithValue(
                    "@variantId", Convert.ToInt32(row.Tag))

                category = command.ExecuteScalar()
            End Using

            Dim categoryName As String = Convert.ToString(category)

            If categoryName <> "Uniforms" AndAlso
                categoryName <> "Books" Then

                Throw New InvalidOperationException(
                    "Salary deduction is allowed only for uniforms " &
                    "and books. Remove the other products first.")
            End If
        Next

        If itemCount = 0 Then
            Throw New InvalidOperationException("The cart is empty.")
        End If

        Return employeeId
    End Function

    Public Function CanUseForCart(
        buyer As IBuyerInfo,
        cart As DataGridView
    ) As Boolean

        If buyer Is Nothing OrElse buyer.BuyerType <> "Employee" Then
            Return False
        End If

        Using connection As MySqlConnection = NewConnection()
            connection.Open()

            Using transaction As MySqlTransaction =
                connection.BeginTransaction()

                Try
                    VerifyEmployeeAndCart(
                        connection, transaction, buyer, cart)

                    transaction.Rollback()
                    Return True
                Catch ex As InvalidOperationException
                    transaction.Rollback()
                    Return False
                End Try
            End Using
        End Using
    End Function

    Public Sub RecordFinanceApproval(
        transactionId As Integer,
        approvalReference As String)

        approvalReference = If(approvalReference, "").Trim()

        If approvalReference.Length = 0 OrElse
            approvalReference.Length > 100 Then

            Throw New InvalidOperationException(
                "Enter a finance approval reference of 1 to 100 characters.")
        End If

        Using connection As MySqlConnection = NewConnection()
            connection.Open()

            Using transaction As MySqlTransaction =
                connection.BeginTransaction()

                ' Verify the logged-in role against the database.
                Using command As New MySqlCommand(
                    "SELECT r.role_name " &
                    "FROM tbl_users u " &
                    "INNER JOIN tbl_roles r ON u.role_id = r.role_id " &
                    "WHERE u.user_id = @userId AND u.status = 'Active' " &
                    "FOR UPDATE",
                    connection,
                    transaction)

                    command.Parameters.AddWithValue(
                        "@userId", currentuser.UserID)

                    Dim roleName As String =
                        Convert.ToString(command.ExecuteScalar())

                    If roleName <> RolePermissions.ROLE_SUPERVISOR AndAlso
                        roleName <> "Admin" Then

                        Throw New InvalidOperationException(
                            "Only the supervisor can record finance approval.")
                    End If
                End Using

                Dim header As New DataTable()

                Using command As New MySqlCommand(
                    "SELECT transaction_no, payment_method, status, " &
                    "salary_employee_id " &
                    "FROM tbl_transactions " &
                    "WHERE transaction_id = @id FOR UPDATE",
                    connection,
                    transaction)

                    command.Parameters.AddWithValue("@id", transactionId)

                    Using reader As MySqlDataReader =
                        command.ExecuteReader()

                        header.Load(reader)
                    End Using
                End Using

                If header.Rows.Count <> 1 Then
                    Throw New InvalidOperationException(
                        "The transaction could not be found.")
                End If

                Dim order As DataRow = header.Rows(0)

                If order("payment_method").ToString() <> METHOD OrElse
                    order("status").ToString() <> PENDING_STATUS OrElse
                    IsDBNull(order("salary_employee_id")) Then

                    Throw New InvalidOperationException(
                        "Only a pending finance deduction request " &
                        "can be approved.")
                End If

                Using command As New MySqlCommand(
                    "UPDATE tbl_transactions " &
                    "SET finance_approval_reference = @reference, " &
                    "finance_approved_by = @userId, " &
                    "finance_approved_at = NOW(), " &
                    "amount_paid = total_amount, amount_change = 0, " &
                    "status = 'Completed' " &
                    "WHERE transaction_id = @id " &
                    "AND status = @pending",
                    connection,
                    transaction)

                    command.Parameters.AddWithValue(
                        "@reference", approvalReference)
                    command.Parameters.AddWithValue(
                        "@userId", currentuser.UserID)
                    command.Parameters.AddWithValue("@id", transactionId)
                    command.Parameters.AddWithValue(
                        "@pending", PENDING_STATUS)

                    If command.ExecuteNonQuery() <> 1 Then
                        Throw New InvalidOperationException(
                            "The request has already changed. Refresh it.")
                    End If
                End Using

                Using command As New MySqlCommand(
                    "INSERT INTO tbl_audit_logs " &
                    "(user_id, log_type, action_type, reference_no, " &
                    "details, status, created_at) " &
                    "VALUES (@userId, 'Activity', " &
                    "'Salary Deduction Finance Approval', " &
                    "@transactionNo, @details, 'Success', NOW())",
                    connection,
                    transaction)

                    command.Parameters.AddWithValue(
                        "@userId", currentuser.UserID)
                    command.Parameters.AddWithValue(
                        "@transactionNo",
                        order("transaction_no").ToString())
                    command.Parameters.AddWithValue(
                        "@details",
                        "Returned finance approval recorded. Reference: " &
                        approvalReference &
                        ". No cash received by the bookstore.")

                    command.ExecuteNonQuery()
                End Using

                transaction.Commit()
            End Using
        End Using
    End Sub

    Public Sub PrintSlip(
        transactionId As Integer,
        owner As IWin32Window)

        Dim records As DataTable = GetDataTable(
            "SELECT t.transaction_no, t.buyer_name, t.id_number, " &
            "t.total_amount, t.status, t.finance_approval_reference, " &
            "ti.quantity, ti.subtotal, p.product_name, v.size " &
            "FROM tbl_transactions t " &
            "INNER JOIN tbl_transaction_items ti " &
            "ON ti.transaction_id = t.transaction_id " &
            "INNER JOIN tbl_product_variants v " &
            "ON ti.variant_id = v.variant_id " &
            "INNER JOIN tbl_products p ON v.product_id = p.product_id " &
            "WHERE t.transaction_id = @id AND t.payment_method = @method " &
            "ORDER BY ti.transaction_item_id",
            New String() {"@id", "@method"},
            New Object() {transactionId, METHOD})

        If records Is Nothing OrElse records.Rows.Count = 0 Then
            Throw New InvalidOperationException(
                "The deduction slip could not be loaded.")
        End If

        Dim first As DataRow = records.Rows(0)

        If first("status").ToString() = "Cancelled" Then
            Throw New InvalidOperationException(
                "This deduction request was cancelled.")
        End If

        Dim lines As New List(Of String) From {
            "LOA BOOKSTORE",
            "SALARY DEDUCTION SLIP",
            "",
            "Transaction: " & first("transaction_no").ToString(),
            "Employee: " & first("buyer_name").ToString(),
            "Employee ID No.: " & first("id_number").ToString(),
            "Status: " & first("status").ToString(),
            ""
        }

        For Each item As DataRow In records.Rows
            Dim itemName As String = item("product_name").ToString()
            Dim itemSize As String = item("size").ToString()

            If itemSize <> "" AndAlso itemSize <> "N/A" Then
                itemName &= " (" & itemSize & ")"
            End If

            lines.Add(item("quantity").ToString() & " x " & itemName)
            lines.Add(
                "    PHP " &
                Convert.ToDecimal(item("subtotal")).ToString("N2"))
        Next

        lines.Add("")
        lines.Add(
            "Deduction amount: PHP " &
            Convert.ToDecimal(first("total_amount")).ToString("N2"))
        lines.Add("Cash collected by bookstore: PHP 0.00")

        If first("status").ToString() = PENDING_STATUS Then
            lines.Add("")
            lines.Add("PENDING FINANCE APPROVAL - NOT PAID")
            lines.Add("Submit this slip to the Finance Department.")
            lines.Add("Return the approved form to the supervisor.")
        Else
            lines.Add(
                "Finance reference: " &
                first("finance_approval_reference").ToString())
        End If

        Using document As New PrintDocument(),
            font As New Font("Consolas", 10)

            document.DefaultPageSettings.Margins =
                New Margins(40, 40, 40, 40)

            Dim wrappedLines As New List(Of String)
            Dim nextLine As Integer = 0

            AddHandler document.BeginPrint,
                Sub(sender, args)
                    nextLine = 0
                    wrappedLines.Clear()
                End Sub

            AddHandler document.PrintPage,
                Sub(sender, args)
                    If wrappedLines.Count = 0 Then
                        For Each line As String In lines
                            Dim remaining As String = line

                            If remaining = "" Then
                                wrappedLines.Add("")
                                Continue For
                            End If

                            While remaining.Length > 0
                                Dim length As Integer = remaining.Length

                                While length > 1 AndAlso
                                    args.Graphics.MeasureString(
                                        remaining.Substring(0, length),
                                        font).Width >
                                    args.MarginBounds.Width

                                    length -= 1
                                End While

                                If length < remaining.Length Then
                                    Dim space As Integer =
                                        remaining.LastIndexOf(
                                            " "c, length - 1)

                                    If space > 0 Then length = space
                                End If

                                wrappedLines.Add(
                                    remaining.Substring(0, length).
                                        TrimEnd())

                                remaining =
                                    remaining.Substring(length).TrimStart()
                            End While
                        Next
                    End If

                    Dim height As Single =
                        font.GetHeight(args.Graphics) + 4

                    Dim y As Single = args.MarginBounds.Top

                    While nextLine < wrappedLines.Count AndAlso
                        y + height <= args.MarginBounds.Bottom

                        args.Graphics.DrawString(
                            wrappedLines(nextLine),
                            font,
                            Brushes.Black,
                            args.MarginBounds.Left,
                            y)

                        nextLine += 1
                        y += height
                    End While

                    args.HasMorePages = nextLine < wrappedLines.Count
                End Sub

            Using preview As New PrintPreviewDialog()
                preview.Document = document
                preview.WindowState = FormWindowState.Maximized
                preview.ShowDialog(owner)
            End Using
        End Using
    End Sub

End Module