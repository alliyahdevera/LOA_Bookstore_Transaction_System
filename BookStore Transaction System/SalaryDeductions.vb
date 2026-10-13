Imports System.Data
Imports System.Drawing
Imports System.Drawing.Printing
Imports System.Windows.Forms
Imports System.Text.RegularExpressions
Imports MySql.Data.MySqlClient

Public Module SalaryDeductions

    Public Const METHOD As String = "Salary Deduction"
    Public Const PENDING_STATUS As String = "Pending Finance"
    Public Const REFERENCE_FORMAT_HINT As String = "FA-YYYY-NNNN (example: FA-2026-0001)"
    ' change this one line if you want a different format
    Private Const REFERENCE_PATTERN As String = "^FA-\d{4}-\d{4}$"
    Public Function NormalizeReference(reference As String) As String
        Return If(reference, "").Trim().ToUpperInvariant()
    End Function

    Public Function IsValidReference(reference As String) As Boolean
        Return Regex.IsMatch(NormalizeReference(reference), REFERENCE_PATTERN)
    End Function

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

        approvalReference = NormalizeReference(approvalReference)

        If Not IsValidReference(approvalReference) Then
            Throw New InvalidOperationException(
        "The reference must follow the format " & REFERENCE_FORMAT_HINT & ".")
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
                Using dupCommand As New MySqlCommand(
                "SELECT COUNT(*) FROM tbl_transactions " &
                "WHERE finance_approval_reference = @reference",
                connection,
                transaction)

                    dupCommand.Parameters.AddWithValue("@reference", approvalReference)

                    If Convert.ToInt32(dupCommand.ExecuteScalar()) > 0 Then
                        Throw New InvalidOperationException(
                        "This Finance reference was already used for another transaction.")
                    End If
                End Using
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

        Dim isPending As Boolean = first("status").ToString() = PENDING_STATUS

        Dim itemNames As New List(Of String)
        Dim itemAmounts As New List(Of Decimal)

        For Each item As DataRow In records.Rows
            Dim itemName As String =
                item("quantity").ToString() & " x " & item("product_name").ToString()
            Dim itemSize As String = item("size").ToString()

            If itemSize <> "" AndAlso itemSize <> "N/A" Then
                itemName &= " (" & itemSize & ")"
            End If

            itemNames.Add(itemName)
            itemAmounts.Add(Convert.ToDecimal(item("subtotal")))
        Next

        Dim txnNo As String = first("transaction_no").ToString()
        Dim employeeName As String = first("buyer_name").ToString()
        Dim employeeId As String = first("id_number").ToString()
        Dim total As Decimal = Convert.ToDecimal(first("total_amount"))
        Dim reference As String = first("finance_approval_reference").ToString()
        Dim logo As Image = LoadSlipLogo()   ' do not dispose: shared with the Login form

        ' 80 mm roll slip. Paper size is in hundredths of an inch (315 = 3.15 in).
        Const SLIP_WIDTH As Integer = 315
        Dim slipHeight As Integer = 540 + (itemNames.Count * 34)

        Using document As New PrintDocument(),
              fTitle As New Font("Segoe UI", 11, FontStyle.Bold),
              fBold As New Font("Segoe UI", 8, FontStyle.Bold),
              fBody As New Font("Segoe UI", 8),
              fSmall As New Font("Segoe UI", 7),
              dashPen As New Pen(Color.Black) With {
                  .DashStyle = System.Drawing.Drawing2D.DashStyle.Dash
              }

            document.DefaultPageSettings.PaperSize =
                New PaperSize("Salary Deduction Slip", SLIP_WIDTH, slipHeight)
            document.DefaultPageSettings.Margins = New Margins(12, 12, 12, 12)

            AddHandler document.PrintPage,
                Sub(sender, args)
                    Dim g As Graphics = args.Graphics
                    Dim x0 As Single = 12
                    Dim x1 As Single = SLIP_WIDTH - 12
                    Dim w As Single = x1 - x0
                    Dim y As Single = 12

                    Dim centered As New StringFormat With {.Alignment = StringAlignment.Center}
                    Dim farRight As New StringFormat With {.Alignment = StringAlignment.Far}

                    Dim separator As Action =
                        Sub()
                            y += 3
                            g.DrawLine(dashPen, x0, y, x1, y)
                            y += 6
                        End Sub

                    Dim row As Action(Of String, String) =
                        Sub(label As String, value As String)
                            g.DrawString(label, fBody, Brushes.Black, x0, y)
                            g.DrawString(value, fBold, Brushes.Black,
                                         New RectangleF(x0 + 70, y, w - 70, 16), farRight)
                            y += 15
                        End Sub

                    ' ---- header ----
                    If logo IsNot Nothing Then
                        Dim logoSize As Single = 58
                        g.DrawImage(logo, x0 + (w - logoSize) / 2, y, logoSize, logoSize)
                        y += logoSize + 4
                    End If

                    g.DrawString("LYCEUM OF ALABANG", fTitle, Brushes.Black,
                                 New RectangleF(x0, y, w, 20), centered)
                    y += 19
                    g.DrawString("BOOKSTORE", fBold, Brushes.Black,
                                 New RectangleF(x0, y, w, 14), centered)
                    y += 15
                    g.DrawString("SALARY DEDUCTION SLIP", fBold, Brushes.Black,
                                 New RectangleF(x0, y, w, 14), centered)
                    y += 16
                    separator()

                    ' ---- details ----
                    row("Transaction:", txnNo)
                    row("Employee:", employeeName)
                    row("Employee ID No.:", employeeId)
                    row("Date:", Date.Now.ToString("MMM dd, yyyy  h:mm tt"))
                    separator()

                    ' ---- items ----
                    For i As Integer = 0 To itemNames.Count - 1
                        Dim nameBox As New RectangleF(x0, y, w - 62, 60)
                        Dim size As SizeF = g.MeasureString(itemNames(i), fBody, CInt(nameBox.Width))

                        g.DrawString(itemNames(i), fBody, Brushes.Black, nameBox)
                        g.DrawString("PHP " & itemAmounts(i).ToString("N2"), fBody, Brushes.Black,
                                     New RectangleF(x1 - 62, y, 62, 16), farRight)
                        y += Math.Max(size.Height, 14) + 3
                    Next
                    separator()

                    ' ---- totals ----
                    g.DrawString("DEDUCTION AMOUNT", fBold, Brushes.Black, x0, y)
                    g.DrawString("PHP " & total.ToString("N2"), fBold, Brushes.Black,
                                 New RectangleF(x0 + 70, y, w - 70, 16), farRight)
                    y += 16
                    g.DrawString("Cash collected by bookstore", fSmall, Brushes.Black, x0, y)
                    g.DrawString("PHP 0.00", fSmall, Brushes.Black,
                                 New RectangleF(x0 + 70, y, w - 70, 14), farRight)
                    y += 18

                    ' ---- status box ----
                    Dim boxHeight As Single = 34
                    g.DrawRectangle(Pens.Black, x0, y, w, boxHeight)

                    If isPending Then
                        g.DrawString("PENDING FINANCE APPROVAL", fBold, Brushes.Black,
                                     New RectangleF(x0, y + 5, w, 14), centered)
                        g.DrawString("NOT YET PAID", fBody, Brushes.Black,
                                     New RectangleF(x0, y + 18, w, 14), centered)
                    Else
                        g.DrawString("APPROVED BY FINANCE", fBold, Brushes.Black,
                                     New RectangleF(x0, y + 5, w, 14), centered)
                        g.DrawString("Ref: " & reference, fBody, Brushes.Black,
                                     New RectangleF(x0, y + 18, w, 14), centered)
                    End If
                    y += boxHeight + 10

                    ' ---- finance section (blank lines to be filled in by Finance) ----
                    If isPending Then
                        g.DrawString("FOR FINANCE USE", fBold, Brushes.Black, x0, y)
                        y += 20

                        g.DrawString("Control No.:  FA-____-______", fBody, Brushes.Black, x0, y)
                        y += 22

                        g.DrawString("Approved by: ", fBody, Brushes.Black, x0, y)
                        g.DrawLine(Pens.Black, x0 + 62, y + 12, x1, y + 12)
                        y += 22

                        g.DrawString("Date: ", fBody, Brushes.Black, x0, y)
                        g.DrawLine(Pens.Black, x0 + 30, y + 12, x1, y + 12)
                        y += 20
                        separator()

                        g.DrawString(
                            "1. Submit this slip to the Finance Department." & vbCrLf &
                            "2. Return the approved form to the bookstore supervisor." & vbCrLf &
                            "3. Items are released only after approval.",
                            fSmall, Brushes.Black, New RectangleF(x0, y, w, 50))
                        y += 46
                    End If

                    g.DrawString("Thank you!", fSmall, Brushes.Black,
                                 New RectangleF(x0, y, w, 12), centered)

                    args.HasMorePages = False
                End Sub

            Using preview As New PrintPreviewDialog()
                preview.Document = document
                preview.UseAntiAlias = True
                preview.StartPosition = FormStartPosition.CenterParent
                preview.Width = 520
                preview.Height = 760
                preview.PrintPreviewControl.Zoom = 1.4
                preview.ShowDialog(owner)
            End Using
        End Using
    End Sub

    ' loads the school logo from the picture on the Login form
    Public Function LoadSlipLogo() As Image
        Try
            Dim resources As New System.ComponentModel.ComponentResourceManager(GetType(Login))
            Dim picture As Image = TryCast(resources.GetObject("Logo.BackgroundImage"), Image)
            If picture IsNot Nothing Then Return picture
        Catch
        End Try

        ' fallback: a logo.png placed next to the .exe
        Try
            Dim path As String = System.IO.Path.Combine(Application.StartupPath, "logo.png")
            If System.IO.File.Exists(path) Then Return Image.FromFile(path)
        Catch
        End Try

        Return Nothing
    End Function

End Module