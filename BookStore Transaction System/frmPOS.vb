Imports MySql.Data.MySqlClient

Public Class frmPOS

    Private selectedVariantId As Integer = 0
    Private selectedUnitPrice As Decimal = 0
    Private availableStock As Integer = 0
    Private foundStudentId As Integer = 0

    Private ReadOnly gradeLevels As String() = {"Kinder", "Grade 1", "Grade 2", "Grade 3", "Grade 4", "Grade 5", "Grade 6",
        "Grade 7", "Grade 8", "Grade 9", "Grade 10", "Grade 11", "Grade 12",
        "1st Year College", "2nd Year College", "3rd Year College", "4th Year College"}

    Private Sub frmPOS_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        TextBox4.Text = currentuser.FullName
        TextBox4.ReadOnly = True

        ComboBox6.Items.Clear()
        ComboBox6.Items.AddRange(gradeLevels)

        ComboBox4.Items.Clear()
        ComboBox4.Items.AddRange(New String() {"Cash", "Salary Deduction"})
        ComboBox4.Enabled = False   ' Unlocked when Settle Payment completes

        TextBox7.ReadOnly = True    ' Reference / OR No.
        TextBox8.ReadOnly = True    ' Amount Received
        TextBox9.ReadOnly = True    ' Amount Change

        DateTimePicker1.Value = Today

        LoadCategoryCombo()
        ResetProductInfo()
    End Sub

    ' ---------------- Student lookup ----------------
    Private Sub PictureBox1_Click(sender As Object, e As EventArgs) Handles PictureBox1.Click
        LookupStudent()
    End Sub

    Private Sub txtSearch_KeyDown(sender As Object, e As KeyEventArgs) Handles txtSearch.KeyDown
        If e.KeyCode = Keys.Enter Then
            LookupStudent()
            e.SuppressKeyPress = True
        End If
    End Sub

    Private Sub LookupStudent()
        If String.IsNullOrWhiteSpace(txtSearch.Text) Then Exit Sub
        Dim dt As DataTable = GetDataTable("SELECT student_id, first_name, last_name, grade_level, section FROM TBL_STUDENTS WHERE student_no = @n",
                                            New String() {"@n"}, New Object() {txtSearch.Text.Trim()})
        If dt.Rows.Count > 0 Then
            Dim r As DataRow = dt.Rows(0)
            foundStudentId = Convert.ToInt32(r("student_id"))
            TextBox2.Text = r("first_name").ToString() & " " & r("last_name").ToString()
            Dim grade As String = r("grade_level").ToString()
            If Not ComboBox6.Items.Contains(grade) Then ComboBox6.Items.Add(grade)
            ComboBox6.Text = grade
            TextBox3.Text = If(IsDBNull(r("section")), "", r("section").ToString())
        Else
            foundStudentId = 0
            TextBox2.Clear()
            ComboBox6.Text = ""
            TextBox3.Clear()
            MsgBox("Student number not found. You can type the buyer's name manually for a walk-in / employee sale.", vbInformation, "Point of Sale")
            TextBox2.Focus()
        End If
    End Sub

    ' ---------------- Product cascading combos ----------------
    Private Sub LoadCategoryCombo()
        Dim dt As DataTable = GetDataTable("SELECT category_id, category_name FROM TBL_CATEGORIES ORDER BY category_name")
        FillCombo(ComboBox1, dt, "category_name", "category_id")
    End Sub

    Private Sub ComboBox1_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ComboBox1.SelectedIndexChanged
        If ComboBox1.SelectedValue Is Nothing OrElse Not IsNumeric(ComboBox1.SelectedValue) Then Exit Sub
        Dim dt As DataTable = GetDataTable("SELECT category_type_id, type_name FROM TBL_CATEGORY_TYPES WHERE category_id = @c ORDER BY type_name",
                                            New String() {"@c"}, New Object() {ComboBox1.SelectedValue})
        FillCombo(ComboBox5, dt, "type_name", "category_type_id")
    End Sub

    Private Sub ComboBox5_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ComboBox5.SelectedIndexChanged
        If ComboBox5.SelectedValue Is Nothing OrElse Not IsNumeric(ComboBox5.SelectedValue) Then Exit Sub
        Dim dt As DataTable = GetDataTable("SELECT product_id, product_name FROM TBL_PRODUCTS WHERE category_type_id = @t AND status = 'Active' ORDER BY product_name",
                                            New String() {"@t"}, New Object() {ComboBox5.SelectedValue})
        FillCombo(ComboBox2, dt, "product_name", "product_id")
    End Sub

    Private Sub ComboBox2_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ComboBox2.SelectedIndexChanged
        If ComboBox2.SelectedValue Is Nothing OrElse Not IsNumeric(ComboBox2.SelectedValue) Then Exit Sub
        selectedUnitPrice = Convert.ToDecimal(If(ExecScalar("SELECT unit_price FROM TBL_PRODUCTS WHERE product_id = @p", New String() {"@p"}, New Object() {ComboBox2.SelectedValue}), 0))
        TextBox10.Text = selectedUnitPrice.ToString("N2")

        Dim dt As DataTable = GetDataTable("SELECT variant_id, size FROM TBL_PRODUCT_VARIANTS WHERE product_id = @p ORDER BY size",
                                            New String() {"@p"}, New Object() {ComboBox2.SelectedValue})
        FillCombo(ComboBox3, dt, "size", "variant_id")
    End Sub

    Private Sub ComboBox3_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ComboBox3.SelectedIndexChanged
        If ComboBox3.SelectedValue Is Nothing OrElse Not IsNumeric(ComboBox3.SelectedValue) Then Exit Sub
        selectedVariantId = Convert.ToInt32(ComboBox3.SelectedValue)
        availableStock = Convert.ToInt32(If(ExecScalar("SELECT quantity_on_hand FROM TBL_PRODUCT_VARIANTS WHERE variant_id = @v", New String() {"@v"}, New Object() {selectedVariantId}), 0))
        TextBox5.Text = availableStock.ToString()
    End Sub

    Private Sub TextBox6_TextChanged(sender As Object, e As EventArgs) Handles TextBox6.TextChanged   ' Quantity
        If IsNumeric(TextBox6.Text) AndAlso selectedUnitPrice > 0 Then
            TextBox11.Text = (Convert.ToDecimal(TextBox6.Text) * selectedUnitPrice).ToString("N2")
        Else
            TextBox11.Text = ""
        End If
    End Sub

    ' ---------------- 1. btnAddToCart ----------------
    ' Puts selected product information into DataGridView1 (dgv)
    Private Sub btnAddToCart_Click(sender As Object, e As EventArgs) Handles btnAddToCart.Click
        If selectedVariantId = 0 Then
            MsgBox("Select a Category, Type, Product, and Size first.", vbExclamation, "Point of Sale")
            Exit Sub
        End If
        If Not IsNumeric(TextBox6.Text) OrElse Convert.ToInt32(TextBox6.Text) <= 0 Then
            MsgBox("Enter a valid quantity.", vbExclamation, "Point of Sale")
            Exit Sub
        End If

        Dim qty As Integer = Convert.ToInt32(TextBox6.Text)
        If qty > availableStock Then
            MsgBox("Only " & availableStock & " left in stock.", vbExclamation, "Point of Sale")
            Exit Sub
        End If

        ' Add to dgv
        Dim idx As Integer = DataGridView1.Rows.Add(
            ComboBox2.Text, ComboBox3.Text, qty, selectedUnitPrice.ToString("N2"), (qty * selectedUnitPrice).ToString("N2"))
        DataGridView1.Rows(idx).Tag = selectedVariantId

        ' Clear product fields after adding to cart
        ResetProductInfo()
        RecalculateTotal()
    End Sub

    ' ---------------- 2. btnRemoveItem ----------------
    ' Removes only selected item(s) from DataGridView1 (dgv)
    Private Sub btnRemoveItem_Click(sender As Object, e As EventArgs) Handles btnRemoveItem.Click
        If DataGridView1.SelectedRows.Count > 0 Then
            For Each row As DataGridViewRow In DataGridView1.SelectedRows
                If Not row.IsNewRow Then
                    DataGridView1.Rows.Remove(row)
                End If
            Next
            RecalculateTotal()
        Else
            MsgBox("Select an item row in the list to remove.", vbExclamation, "Point of Sale")
        End If
    End Sub

    ' ---------------- 3. btnClear ----------------
    ' Removes only Customer Information and Product Information
    Private Sub btnClear_Click(sender As Object, e As EventArgs) Handles btnClear.Click
        ResetCustomerInfo()
        ResetProductInfo()
    End Sub

    ' ---------------- 4. btnCancelTransaction ----------------
    ' Removes all (Customer Info, Product Info, DataGridView Cart, and Payment Info)
    Private Sub btnCancelTransaction_Click(sender As Object, e As EventArgs) Handles btnCancelTransaction.Click
        If MsgBox("Cancel this transaction? All entered details and cart items will be cleared.", vbYesNo + vbQuestion, "Point of Sale") = MsgBoxResult.Yes Then
            ResetAll()
        End If
    End Sub

    ' ---------------- 5. btnSaveTransaction ----------------
    ' Saves transaction and item details to the database
    Private Sub btnSaveTransaction_Click(sender As Object, e As EventArgs) Handles btnSaveTransaction.Click
        If DataGridView1.Rows.Count = 0 Then
            MsgBox("Add at least one item to the cart first.", vbExclamation, "Point of Sale")
            Exit Sub
        End If
        If String.IsNullOrWhiteSpace(TextBox7.Text) Then
            MsgBox("Click 'Settle Payment' first.", vbExclamation, "Point of Sale")
            Exit Sub
        End If
        If String.IsNullOrWhiteSpace(TextBox2.Text) Then
            MsgBox("Enter the buyer's name (or look up a Student No.).", vbExclamation, "Point of Sale")
            Exit Sub
        End If

        Dim buyerType As String = If(foundStudentId > 0, "Student", "Walk-in")

        Try
            If Not connection() Then Exit Sub
            Dim trans As MySqlTransaction = cn.BeginTransaction()
            Try
                Dim transactionId As Long = 0
                Dim insTxn As String = "INSERT INTO TBL_TRANSACTIONS " &
                    "(transaction_no, buyer_type, student_id, buyer_name, or_no, or_date, payment_method, total_amount, amount_paid, amount_change, created_by, status) " &
                    "VALUES (@tno, @bt, @sid, @bn, @orno, @ord, @pm, @tot, @paid, @chg, @by, 'Completed')"
                Using c1 As New MySqlCommand(insTxn, cn, trans)
                    c1.Parameters.AddWithValue("@tno", NewTransactionNo())
                    c1.Parameters.AddWithValue("@bt", buyerType)
                    c1.Parameters.AddWithValue("@sid", If(foundStudentId > 0, CType(foundStudentId, Object), DBNull.Value))
                    c1.Parameters.AddWithValue("@bn", TextBox2.Text.Trim())
                    c1.Parameters.AddWithValue("@orno", TextBox7.Text.Trim())
                    c1.Parameters.AddWithValue("@ord", DateTimePicker1.Value.Date)
                    c1.Parameters.AddWithValue("@pm", ComboBox4.Text)
                    c1.Parameters.AddWithValue("@tot", Convert.ToDecimal(TextBox1.Text))
                    c1.Parameters.AddWithValue("@paid", Convert.ToDecimal(TextBox8.Text))
                    c1.Parameters.AddWithValue("@chg", Convert.ToDecimal(TextBox9.Text))
                    c1.Parameters.AddWithValue("@by", currentuser.UserID)
                    c1.ExecuteNonQuery()
                    transactionId = c1.LastInsertedId
                End Using

                For Each row As DataGridViewRow In DataGridView1.Rows
                    If row.IsNewRow Then Continue For
                    Dim variantId As Integer = Convert.ToInt32(row.Tag)
                    Dim subtotal As Decimal = Convert.ToDecimal(row.Cells("SubTotal").Value)
                    Dim qty As Integer = Convert.ToInt32(row.Cells("Quantity").Value)

                    Using c2 As New MySqlCommand("INSERT INTO TBL_TRANSACTION_ITEMS (transaction_id, variant_id, subtotal) VALUES (@t, @v, @s)", cn, trans)
                        c2.Parameters.AddWithValue("@t", transactionId)
                        c2.Parameters.AddWithValue("@v", variantId)
                        c2.Parameters.AddWithValue("@s", subtotal)
                        c2.ExecuteNonQuery()
                    End Using

                    Using c3 As New MySqlCommand("UPDATE TBL_PRODUCT_VARIANTS SET quantity_on_hand = quantity_on_hand - @q WHERE variant_id = @v", cn, trans)
                        c3.Parameters.AddWithValue("@q", qty)
                        c3.Parameters.AddWithValue("@v", variantId)
                        c3.ExecuteNonQuery()
                    End Using
                Next

                trans.Commit()
                MsgBox("Transaction saved successfully.", vbInformation, "Point of Sale")
                ResetAll()
            Catch exInner As Exception
                trans.Rollback()
                MsgBox("Transaction failed and was rolled back: " & exInner.Message, vbCritical, "Point of Sale")
            End Try
            cn.Close()
        Catch ex As Exception
            If cn.State = ConnectionState.Open Then cn.Close()
            MsgBox("Database error: " & ex.Message, vbCritical, "Error")
        End Try
    End Sub

    ' ---------------- Settle Payment ----------------
    Private Sub btnSettlePayment_Click(sender As Object, e As EventArgs) Handles btnSettlePayment.Click
        If DataGridView1.Rows.Count = 0 Then
            MsgBox("Add at least one item to the cart first.", vbExclamation, "Point of Sale")
            Exit Sub
        End If

        Dim grandTotal As Decimal = If(IsNumeric(TextBox1.Text), Convert.ToDecimal(TextBox1.Text), 0)
        Dim frm As New frmPayment()
        frm.GrandTotal = grandTotal
        If frm.ShowDialog() = DialogResult.OK Then
            TextBox7.Text = frm.ResultORNo
            DateTimePicker1.Value = frm.ResultDate
            ComboBox4.Text = frm.ResultMethod
            TextBox8.Text = frm.ResultReceived.ToString("N2")
            TextBox9.Text = frm.ResultChange.ToString("N2")
        End If
    End Sub

    ' ---------------- Helper Helper Methods ----------------
    Private Sub RecalculateTotal()
        Dim total As Decimal = 0
        For Each row As DataGridViewRow In DataGridView1.Rows
            If Not row.IsNewRow AndAlso row.Cells("SubTotal").Value IsNot Nothing Then
                total += Convert.ToDecimal(row.Cells("SubTotal").Value)
            End If
        Next
        TextBox1.Text = total.ToString("N2")
    End Sub

    Private Sub ResetCustomerInfo()
        txtSearch.Clear()
        TextBox2.Clear()
        ComboBox6.Text = ""
        TextBox3.Clear()
        foundStudentId = 0
    End Sub

    Private Sub ResetProductInfo()
        ComboBox1.SelectedIndex = -1
        ComboBox5.DataSource = Nothing
        ComboBox2.DataSource = Nothing
        ComboBox3.DataSource = Nothing
        TextBox6.Clear()
        TextBox10.Clear()
        TextBox11.Clear()
        TextBox5.Clear()
        selectedVariantId = 0
        selectedUnitPrice = 0
        availableStock = 0
    End Sub

    Private Sub ResetPaymentInfo()
        TextBox7.Clear()
        TextBox8.Clear()
        TextBox9.Clear()
        ComboBox4.Text = ""
        DateTimePicker1.Value = Today
    End Sub

    Private Sub ResetAll()
        ResetCustomerInfo()
        ResetProductInfo()
        ResetPaymentInfo()
        DataGridView1.Rows.Clear()
        TextBox1.Text = "0.00"
    End Sub

End Class