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

        Dim itemNames As New List(Of String)
        Dim itemAmounts As New List(Of Decimal)

        For Each row As DataRow In result.Rows
            Dim itemName As String =
                row("quantity").ToString() & " x " & row("product_name").ToString()
            Dim itemSize As String = row("size").ToString()

            If itemSize <> "" AndAlso itemSize <> "N/A" Then
                itemName &= " (" & itemSize & ")"
            End If

            itemNames.Add(itemName)
            itemAmounts.Add(Convert.ToDecimal(row("subtotal")))
        Next

        Dim txnNo As String = result.Rows(0)("transaction_no").ToString()
        Dim buyerName As String = result.Rows(0)("buyer_name").ToString()
        Dim idNo As String =
            If(String.IsNullOrWhiteSpace(result.Rows(0)("id_number").ToString()),
               "N/A",
               result.Rows(0)("id_number").ToString())
        Dim total As Decimal = Convert.ToDecimal(result.Rows(0)("total_amount"))
        Dim logo As Image = SalaryDeductions.LoadSlipLogo()   ' do not dispose: shared with the Login form

        ' 80 mm roll slip. Paper size is in hundredths of an inch (315 = 3.15 in).
        Const SLIP_WIDTH As Integer = 315
        Dim slipHeight As Integer = 500 + (itemNames.Count * 34)

        Using document As New PrintDocument(),
              fTitle As New Font("Segoe UI", 11, FontStyle.Bold),
              fBold As New Font("Segoe UI", 8, FontStyle.Bold),
              fBody As New Font("Segoe UI", 8),
              fSmall As New Font("Segoe UI", 7),
              dashPen As New Pen(Color.Black) With {
                  .DashStyle = System.Drawing.Drawing2D.DashStyle.Dash
              }

            document.DefaultPageSettings.PaperSize =
                New PaperSize("Module Payment Slip", SLIP_WIDTH, slipHeight)
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

                    Dim detail As Action(Of String, String) =
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
                    g.DrawString("MODULE PAYMENT SLIP", fBold, Brushes.Black,
                                 New RectangleF(x0, y, w, 14), centered)
                    y += 16
                    separator()

                    ' ---- details ----
                    detail("Transaction:", txnNo)
                    detail("Buyer:", buyerName)
                    detail("ID No.:", idNo)
                    detail("Date:", Date.Now.ToString("MMM dd, yyyy  h:mm tt"))
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

                    ' ---- total ----
                    g.DrawString("TOTAL TO PAY", fBold, Brushes.Black, x0, y)
                    g.DrawString("PHP " & total.ToString("N2"), fBold, Brushes.Black,
                                 New RectangleF(x0 + 70, y, w - 70, 16), farRight)
                    y += 16
                    g.DrawString("Please pay at the school cashier", fSmall, Brushes.Black, x0, y)
                    y += 18

                    ' ---- notice box ----
                    Dim boxHeight As Single = 34
                    g.DrawRectangle(Pens.Black, x0, y, w, boxHeight)
                    g.DrawString("PRESENT TO THE SCHOOL CASHIER", fBold, Brushes.Black,
                                 New RectangleF(x0, y + 5, w, 14), centered)
                    g.DrawString("Not proof of payment or release", fBody, Brushes.Black,
                                 New RectangleF(x0, y + 18, w, 14), centered)
                    y += boxHeight + 10

                    ' ---- school cashier section ----
                    g.DrawString("FOR SCHOOL CASHIER USE", fBold, Brushes.Black, x0, y)
                    y += 20

                    g.DrawString("OR No.:", fBody, Brushes.Black, x0, y)
                    g.DrawLine(Pens.Black, x0 + 38, y + 12, x1, y + 12)
                    y += 22

                    g.DrawString("Date paid:", fBody, Brushes.Black, x0, y)
                    g.DrawLine(Pens.Black, x0 + 52, y + 12, x1, y + 12)
                    y += 20
                    separator()

                    g.DrawString(
                        "1. Pay the total at the school cashier." & vbCrLf &
                        "2. Return to the bookstore with the receipt or OR No." & vbCrLf &
                        "3. Modules are released after the OR is recorded.",
                        fSmall, Brushes.Black, New RectangleF(x0, y, w, 50))
                    y += 46

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

End Module