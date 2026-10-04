<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmPOS
    Inherits System.Windows.Forms.Form

    'Form overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()> _
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        Try
            If disposing AndAlso components IsNot Nothing Then
                components.Dispose()
            End If
        Finally
            MyBase.Dispose(disposing)
        End Try
    End Sub

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container()
        Dim DataGridViewCellStyle5 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Dim DataGridViewCellStyle6 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmPOS))
        Me.Panel5 = New System.Windows.Forms.Panel()
        Me.pnlbuyertype = New System.Windows.Forms.Panel()
        Me.Label16 = New System.Windows.Forms.Label()
        Me.cbobuyertype = New System.Windows.Forms.ComboBox()
        Me.Label13 = New System.Windows.Forms.Label()
        Me.Panel11 = New System.Windows.Forms.Panel()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.btnAddToCart = New System.Windows.Forms.Button()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.Panel7 = New System.Windows.Forms.Panel()
        Me.dgvCart = New System.Windows.Forms.DataGridView()
        Me.ProductName = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.Size = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.Quantity = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.UnitPrice = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.SubTotal = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.Panel8 = New System.Windows.Forms.Panel()
        Me.Label15 = New System.Windows.Forms.Label()
        Me.btnSettlePayment = New System.Windows.Forms.Button()
        Me.btnCancelTransaction = New System.Windows.Forms.Button()
        Me.lblposition = New System.Windows.Forms.Label()
        Me.Label28 = New System.Windows.Forms.Label()
        Me.lblname = New System.Windows.Forms.Label()
        Me.Label29 = New System.Windows.Forms.Label()
        Me.Panel13 = New System.Windows.Forms.Panel()
        Me.Label4 = New System.Windows.Forms.Label()
        Me.Label24 = New System.Windows.Forms.Label()
        Me.txtTotalAMount = New System.Windows.Forms.TextBox()
        Me.btnClear = New System.Windows.Forms.Button()
        Me.btnSaveTransaction = New System.Windows.Forms.Button()
        Me.txtReferenceNo = New System.Windows.Forms.TextBox()
        Me.Label17 = New System.Windows.Forms.Label()
        Me.txtAmountReceived = New System.Windows.Forms.TextBox()
        Me.Label18 = New System.Windows.Forms.Label()
        Me.txtAmountChange = New System.Windows.Forms.TextBox()
        Me.Label19 = New System.Windows.Forms.Label()
        Me.Panel4 = New System.Windows.Forms.Panel()
        Me.Label11 = New System.Windows.Forms.Label()
        Me.txtPaymentMethod = New System.Windows.Forms.TextBox()
        Me.txttransactdate = New System.Windows.Forms.TextBox()
        Me.Panel9 = New System.Windows.Forms.Panel()
        Me.Label20 = New System.Windows.Forms.Label()
        Me.Label21 = New System.Windows.Forms.Label()
        Me.btnRemoveItem = New System.Windows.Forms.Button()
        Me.Timer1 = New System.Windows.Forms.Timer(Me.components)
        Me.DateTimePicker2 = New System.Windows.Forms.DateTimePicker()
        Me.Label14 = New System.Windows.Forms.Label()
        Me.Panel6 = New System.Windows.Forms.Panel()
        Me.Panel10 = New System.Windows.Forms.Panel()
        Me.Label9 = New System.Windows.Forms.Label()
        Me.dgvlistproducts = New System.Windows.Forms.DataGridView()
        Me.ProductCode = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.DataGridViewTextBoxColumn1 = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.Category = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.TypeofProduct = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.DataGridViewTextBoxColumn2 = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.DataGridViewTextBoxColumn3 = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.DataGridViewTextBoxColumn4 = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.Status = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.nudQuantity = New System.Windows.Forms.NumericUpDown()
        Me.Panel12 = New System.Windows.Forms.Panel()
        Me.picSearchProduct = New System.Windows.Forms.PictureBox()
        Me.txtProductSearch = New System.Windows.Forms.TextBox()
        Me.Label10 = New System.Windows.Forms.Label()
        Me.Label5 = New System.Windows.Forms.Label()
        Me.cbocategory = New System.Windows.Forms.ComboBox()
        Me.Label12 = New System.Windows.Forms.Label()
        Me.txtTransactionNo = New System.Windows.Forms.TextBox()
        Me.cboType = New System.Windows.Forms.ComboBox()
        Me.Type = New System.Windows.Forms.Label()
        Me.Panel5.SuspendLayout()
        Me.Panel11.SuspendLayout()
        Me.Panel7.SuspendLayout()
        CType(Me.dgvCart, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.Panel8.SuspendLayout()
        Me.Panel13.SuspendLayout()
        Me.Panel4.SuspendLayout()
        Me.Panel9.SuspendLayout()
        Me.Panel6.SuspendLayout()
        Me.Panel10.SuspendLayout()
        CType(Me.dgvlistproducts, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.nudQuantity, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.Panel12.SuspendLayout()
        CType(Me.picSearchProduct, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'Panel5
        '
        Me.Panel5.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.Panel5.Controls.Add(Me.pnlbuyertype)
        Me.Panel5.Controls.Add(Me.Label16)
        Me.Panel5.Controls.Add(Me.cbobuyertype)
        Me.Panel5.Controls.Add(Me.Label13)
        Me.Panel5.Controls.Add(Me.Panel11)
        Me.Panel5.Location = New System.Drawing.Point(844, 115)
        Me.Panel5.Name = "Panel5"
        Me.Panel5.Size = New System.Drawing.Size(345, 298)
        Me.Panel5.TabIndex = 9
        '
        'pnlbuyertype
        '
        Me.pnlbuyertype.Dock = System.Windows.Forms.DockStyle.Bottom
        Me.pnlbuyertype.Location = New System.Drawing.Point(0, 107)
        Me.pnlbuyertype.Name = "pnlbuyertype"
        Me.pnlbuyertype.Size = New System.Drawing.Size(343, 189)
        Me.pnlbuyertype.TabIndex = 126
        '
        'Label16
        '
        Me.Label16.AutoSize = True
        Me.Label16.Font = New System.Drawing.Font("Segoe UI", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label16.ForeColor = System.Drawing.Color.Silver
        Me.Label16.Location = New System.Drawing.Point(16, 87)
        Me.Label16.Name = "Label16"
        Me.Label16.Size = New System.Drawing.Size(307, 17)
        Me.Label16.TabIndex = 125
        Me.Label16.Text = "———————————————————————" & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10)
        '
        'cbobuyertype
        '
        Me.cbobuyertype.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.cbobuyertype.Font = New System.Drawing.Font("Segoe UI", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cbobuyertype.FormattingEnabled = True
        Me.cbobuyertype.Items.AddRange(New Object() {"Student", "Employee", "Guest"})
        Me.cbobuyertype.Location = New System.Drawing.Point(134, 51)
        Me.cbobuyertype.Name = "cbobuyertype"
        Me.cbobuyertype.Size = New System.Drawing.Size(189, 25)
        Me.cbobuyertype.TabIndex = 119
        '
        'Label13
        '
        Me.Label13.AutoSize = True
        Me.Label13.Font = New System.Drawing.Font("Segoe UI", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label13.Location = New System.Drawing.Point(17, 54)
        Me.Label13.Name = "Label13"
        Me.Label13.Size = New System.Drawing.Size(71, 17)
        Me.Label13.TabIndex = 120
        Me.Label13.Text = "Buyer Type"
        '
        'Panel11
        '
        Me.Panel11.BackColor = System.Drawing.Color.FromArgb(CType(CType(1, Byte), Integer), CType(CType(21, Byte), Integer), CType(CType(78, Byte), Integer))
        Me.Panel11.Controls.Add(Me.Label2)
        Me.Panel11.Location = New System.Drawing.Point(0, 0)
        Me.Panel11.Name = "Panel11"
        Me.Panel11.Size = New System.Drawing.Size(384, 35)
        Me.Panel11.TabIndex = 11
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Font = New System.Drawing.Font("Segoe UI Semibold", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label2.ForeColor = System.Drawing.Color.White
        Me.Label2.Location = New System.Drawing.Point(8, 7)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(172, 21)
        Me.Label2.TabIndex = 10
        Me.Label2.Text = "Customer Information"
        '
        'btnAddToCart
        '
        Me.btnAddToCart.BackColor = System.Drawing.Color.FromArgb(CType(CType(1, Byte), Integer), CType(CType(21, Byte), Integer), CType(CType(78, Byte), Integer))
        Me.btnAddToCart.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnAddToCart.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnAddToCart.ForeColor = System.Drawing.Color.White
        Me.btnAddToCart.Location = New System.Drawing.Point(647, 441)
        Me.btnAddToCart.Name = "btnAddToCart"
        Me.btnAddToCart.Size = New System.Drawing.Size(172, 28)
        Me.btnAddToCart.TabIndex = 18
        Me.btnAddToCart.Text = "Add to Cart"
        Me.btnAddToCart.UseVisualStyleBackColor = False
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Font = New System.Drawing.Font("Segoe UI", 18.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label1.ForeColor = System.Drawing.Color.FromArgb(CType(CType(1, Byte), Integer), CType(CType(21, Byte), Integer), CType(CType(78, Byte), Integer))
        Me.Label1.Location = New System.Drawing.Point(28, 14)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(158, 32)
        Me.Label1.TabIndex = 8
        Me.Label1.Text = "Point of Sale"
        '
        'Panel7
        '
        Me.Panel7.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.Panel7.Controls.Add(Me.dgvCart)
        Me.Panel7.Controls.Add(Me.Panel8)
        Me.Panel7.Location = New System.Drawing.Point(33, 478)
        Me.Panel7.Name = "Panel7"
        Me.Panel7.Size = New System.Drawing.Size(786, 256)
        Me.Panel7.TabIndex = 16
        '
        'dgvCart
        '
        Me.dgvCart.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill
        Me.dgvCart.AutoSizeRowsMode = System.Windows.Forms.DataGridViewAutoSizeRowsMode.AllCells
        Me.dgvCart.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.dgvCart.Columns.AddRange(New System.Windows.Forms.DataGridViewColumn() {Me.ProductName, Me.Size, Me.Quantity, Me.UnitPrice, Me.SubTotal})
        DataGridViewCellStyle5.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft
        DataGridViewCellStyle5.BackColor = System.Drawing.SystemColors.Window
        DataGridViewCellStyle5.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        DataGridViewCellStyle5.ForeColor = System.Drawing.SystemColors.ControlText
        DataGridViewCellStyle5.SelectionBackColor = System.Drawing.SystemColors.Highlight
        DataGridViewCellStyle5.SelectionForeColor = System.Drawing.SystemColors.HighlightText
        DataGridViewCellStyle5.WrapMode = System.Windows.Forms.DataGridViewTriState.[True]
        Me.dgvCart.DefaultCellStyle = DataGridViewCellStyle5
        Me.dgvCart.Location = New System.Drawing.Point(-1, 35)
        Me.dgvCart.Name = "dgvCart"
        Me.dgvCart.ReadOnly = True
        Me.dgvCart.Size = New System.Drawing.Size(786, 220)
        Me.dgvCart.TabIndex = 1
        '
        'ProductName
        '
        Me.ProductName.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill
        Me.ProductName.HeaderText = "Product Name"
        Me.ProductName.Name = "ProductName"
        Me.ProductName.ReadOnly = True
        '
        'Size
        '
        Me.Size.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill
        Me.Size.HeaderText = "Size"
        Me.Size.Name = "Size"
        Me.Size.ReadOnly = True
        '
        'Quantity
        '
        Me.Quantity.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill
        Me.Quantity.HeaderText = "Quantity"
        Me.Quantity.Name = "Quantity"
        Me.Quantity.ReadOnly = True
        '
        'UnitPrice
        '
        Me.UnitPrice.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill
        Me.UnitPrice.HeaderText = "Unit Price"
        Me.UnitPrice.Name = "UnitPrice"
        Me.UnitPrice.ReadOnly = True
        '
        'SubTotal
        '
        Me.SubTotal.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill
        Me.SubTotal.HeaderText = "SubTotal"
        Me.SubTotal.Name = "SubTotal"
        Me.SubTotal.ReadOnly = True
        '
        'Panel8
        '
        Me.Panel8.BackColor = System.Drawing.Color.FromArgb(CType(CType(1, Byte), Integer), CType(CType(21, Byte), Integer), CType(CType(78, Byte), Integer))
        Me.Panel8.Controls.Add(Me.Label15)
        Me.Panel8.Location = New System.Drawing.Point(0, 0)
        Me.Panel8.Name = "Panel8"
        Me.Panel8.Size = New System.Drawing.Size(785, 35)
        Me.Panel8.TabIndex = 0
        '
        'Label15
        '
        Me.Label15.AutoSize = True
        Me.Label15.Font = New System.Drawing.Font("Segoe UI Semibold", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label15.ForeColor = System.Drawing.Color.White
        Me.Label15.Location = New System.Drawing.Point(7, 6)
        Me.Label15.Name = "Label15"
        Me.Label15.Size = New System.Drawing.Size(151, 21)
        Me.Label15.TabIndex = 9
        Me.Label15.Text = "List of Items in Cart"
        '
        'btnSettlePayment
        '
        Me.btnSettlePayment.BackColor = System.Drawing.Color.Green
        Me.btnSettlePayment.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnSettlePayment.ForeColor = System.Drawing.Color.White
        Me.btnSettlePayment.Location = New System.Drawing.Point(516, 759)
        Me.btnSettlePayment.Name = "btnSettlePayment"
        Me.btnSettlePayment.Size = New System.Drawing.Size(200, 40)
        Me.btnSettlePayment.TabIndex = 28
        Me.btnSettlePayment.Text = "Settle Payment"
        Me.btnSettlePayment.UseVisualStyleBackColor = False
        '
        'btnCancelTransaction
        '
        Me.btnCancelTransaction.BackColor = System.Drawing.Color.Maroon
        Me.btnCancelTransaction.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnCancelTransaction.ForeColor = System.Drawing.Color.White
        Me.btnCancelTransaction.Location = New System.Drawing.Point(988, 759)
        Me.btnCancelTransaction.Name = "btnCancelTransaction"
        Me.btnCancelTransaction.Size = New System.Drawing.Size(200, 40)
        Me.btnCancelTransaction.TabIndex = 29
        Me.btnCancelTransaction.Text = "Cancel Transaction"
        Me.btnCancelTransaction.UseVisualStyleBackColor = False
        '
        'lblposition
        '
        Me.lblposition.AutoSize = True
        Me.lblposition.BackColor = System.Drawing.Color.Transparent
        Me.lblposition.Font = New System.Drawing.Font("Segoe UI Semibold", 12.0!, CType((System.Drawing.FontStyle.Bold Or System.Drawing.FontStyle.Italic), System.Drawing.FontStyle), System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblposition.ForeColor = System.Drawing.Color.FromArgb(CType(CType(1, Byte), Integer), CType(CType(21, Byte), Integer), CType(CType(78, Byte), Integer))
        Me.lblposition.Location = New System.Drawing.Point(315, 3)
        Me.lblposition.Name = "lblposition"
        Me.lblposition.Size = New System.Drawing.Size(82, 21)
        Me.lblposition.TabIndex = 66
        Me.lblposition.Text = "NPosition"
        '
        'Label28
        '
        Me.Label28.AutoSize = True
        Me.Label28.BackColor = System.Drawing.Color.Transparent
        Me.Label28.Font = New System.Drawing.Font("Segoe UI", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label28.ForeColor = System.Drawing.Color.FromArgb(CType(CType(1, Byte), Integer), CType(CType(21, Byte), Integer), CType(CType(78, Byte), Integer))
        Me.Label28.Location = New System.Drawing.Point(249, 3)
        Me.Label28.Name = "Label28"
        Me.Label28.Size = New System.Drawing.Size(68, 21)
        Me.Label28.TabIndex = 65
        Me.Label28.Text = "Position:"
        '
        'lblname
        '
        Me.lblname.AutoSize = True
        Me.lblname.BackColor = System.Drawing.Color.Transparent
        Me.lblname.Font = New System.Drawing.Font("Segoe UI Semibold", 12.0!, CType((System.Drawing.FontStyle.Bold Or System.Drawing.FontStyle.Italic), System.Drawing.FontStyle), System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblname.ForeColor = System.Drawing.Color.FromArgb(CType(CType(1, Byte), Integer), CType(CType(21, Byte), Integer), CType(CType(78, Byte), Integer))
        Me.lblname.Location = New System.Drawing.Point(54, 3)
        Me.lblname.Name = "lblname"
        Me.lblname.Size = New System.Drawing.Size(53, 21)
        Me.lblname.TabIndex = 64
        Me.lblname.Text = "Name"
        '
        'Label29
        '
        Me.Label29.AutoSize = True
        Me.Label29.BackColor = System.Drawing.Color.Transparent
        Me.Label29.Font = New System.Drawing.Font("Segoe UI", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label29.ForeColor = System.Drawing.Color.FromArgb(CType(CType(1, Byte), Integer), CType(CType(21, Byte), Integer), CType(CType(78, Byte), Integer))
        Me.Label29.Location = New System.Drawing.Point(4, 3)
        Me.Label29.Name = "Label29"
        Me.Label29.Size = New System.Drawing.Size(55, 21)
        Me.Label29.TabIndex = 63
        Me.Label29.Text = "Name:"
        '
        'Panel13
        '
        Me.Panel13.BackColor = System.Drawing.Color.White
        Me.Panel13.Controls.Add(Me.lblname)
        Me.Panel13.Controls.Add(Me.Label29)
        Me.Panel13.Controls.Add(Me.Label28)
        Me.Panel13.Controls.Add(Me.lblposition)
        Me.Panel13.Dock = System.Windows.Forms.DockStyle.Bottom
        Me.Panel13.Location = New System.Drawing.Point(0, 823)
        Me.Panel13.Name = "Panel13"
        Me.Panel13.Size = New System.Drawing.Size(1220, 27)
        Me.Panel13.TabIndex = 69
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.Font = New System.Drawing.Font("Segoe UI", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label4.ForeColor = System.Drawing.SystemColors.ControlDarkDark
        Me.Label4.Location = New System.Drawing.Point(31, 47)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(177, 15)
        Me.Label4.TabIndex = 88
        Me.Label4.Text = "Create Transaction for a Product"
        '
        'Label24
        '
        Me.Label24.AutoSize = True
        Me.Label24.Font = New System.Drawing.Font("Segoe UI", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label24.Location = New System.Drawing.Point(17, 95)
        Me.Label24.Name = "Label24"
        Me.Label24.Size = New System.Drawing.Size(105, 17)
        Me.Label24.TabIndex = 92
        Me.Label24.Text = "Transaction Date"
        '
        'txtTotalAMount
        '
        Me.txtTotalAMount.BackColor = System.Drawing.Color.White
        Me.txtTotalAMount.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.txtTotalAMount.Font = New System.Drawing.Font("Segoe UI", 15.0!)
        Me.txtTotalAMount.Location = New System.Drawing.Point(135, 254)
        Me.txtTotalAMount.Name = "txtTotalAMount"
        Me.txtTotalAMount.ReadOnly = True
        Me.txtTotalAMount.Size = New System.Drawing.Size(190, 34)
        Me.txtTotalAMount.TabIndex = 94
        Me.txtTotalAMount.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        'btnClear
        '
        Me.btnClear.BackColor = System.Drawing.Color.Gray
        Me.btnClear.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnClear.ForeColor = System.Drawing.Color.White
        Me.btnClear.Location = New System.Drawing.Point(275, 759)
        Me.btnClear.Name = "btnClear"
        Me.btnClear.Size = New System.Drawing.Size(200, 40)
        Me.btnClear.TabIndex = 31
        Me.btnClear.Text = "Clear Fields"
        Me.btnClear.UseVisualStyleBackColor = False
        '
        'btnSaveTransaction
        '
        Me.btnSaveTransaction.BackColor = System.Drawing.Color.Navy
        Me.btnSaveTransaction.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnSaveTransaction.ForeColor = System.Drawing.Color.White
        Me.btnSaveTransaction.Location = New System.Drawing.Point(756, 759)
        Me.btnSaveTransaction.Name = "btnSaveTransaction"
        Me.btnSaveTransaction.Size = New System.Drawing.Size(200, 40)
        Me.btnSaveTransaction.TabIndex = 30
        Me.btnSaveTransaction.Text = "Save Transaction"
        Me.btnSaveTransaction.UseVisualStyleBackColor = False
        '
        'txtReferenceNo
        '
        Me.txtReferenceNo.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.txtReferenceNo.Font = New System.Drawing.Font("Segoe UI", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtReferenceNo.Location = New System.Drawing.Point(135, 51)
        Me.txtReferenceNo.Name = "txtReferenceNo"
        Me.txtReferenceNo.ReadOnly = True
        Me.txtReferenceNo.Size = New System.Drawing.Size(190, 25)
        Me.txtReferenceNo.TabIndex = 97
        '
        'Label17
        '
        Me.Label17.AutoSize = True
        Me.Label17.Font = New System.Drawing.Font("Segoe UI", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label17.Location = New System.Drawing.Point(17, 53)
        Me.Label17.Name = "Label17"
        Me.Label17.Size = New System.Drawing.Size(91, 17)
        Me.Label17.TabIndex = 98
        Me.Label17.Text = "Reference No."
        '
        'txtAmountReceived
        '
        Me.txtAmountReceived.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.txtAmountReceived.Font = New System.Drawing.Font("Segoe UI", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtAmountReceived.Location = New System.Drawing.Point(135, 171)
        Me.txtAmountReceived.Name = "txtAmountReceived"
        Me.txtAmountReceived.ReadOnly = True
        Me.txtAmountReceived.Size = New System.Drawing.Size(190, 25)
        Me.txtAmountReceived.TabIndex = 101
        '
        'Label18
        '
        Me.Label18.AutoSize = True
        Me.Label18.Font = New System.Drawing.Font("Segoe UI", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label18.Location = New System.Drawing.Point(17, 173)
        Me.Label18.Name = "Label18"
        Me.Label18.Size = New System.Drawing.Size(109, 17)
        Me.Label18.TabIndex = 102
        Me.Label18.Text = "Amount Received"
        '
        'txtAmountChange
        '
        Me.txtAmountChange.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.txtAmountChange.Font = New System.Drawing.Font("Segoe UI", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtAmountChange.Location = New System.Drawing.Point(135, 214)
        Me.txtAmountChange.Name = "txtAmountChange"
        Me.txtAmountChange.ReadOnly = True
        Me.txtAmountChange.Size = New System.Drawing.Size(190, 25)
        Me.txtAmountChange.TabIndex = 99
        '
        'Label19
        '
        Me.Label19.AutoSize = True
        Me.Label19.Font = New System.Drawing.Font("Segoe UI", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label19.Location = New System.Drawing.Point(17, 216)
        Me.Label19.Name = "Label19"
        Me.Label19.Size = New System.Drawing.Size(101, 17)
        Me.Label19.TabIndex = 100
        Me.Label19.Text = "Amount Change"
        '
        'Panel4
        '
        Me.Panel4.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.Panel4.Controls.Add(Me.Label11)
        Me.Panel4.Controls.Add(Me.txtPaymentMethod)
        Me.Panel4.Controls.Add(Me.txtTotalAMount)
        Me.Panel4.Controls.Add(Me.txttransactdate)
        Me.Panel4.Controls.Add(Me.Panel9)
        Me.Panel4.Controls.Add(Me.Label21)
        Me.Panel4.Controls.Add(Me.txtAmountReceived)
        Me.Panel4.Controls.Add(Me.Label18)
        Me.Panel4.Controls.Add(Me.txtAmountChange)
        Me.Panel4.Controls.Add(Me.Label19)
        Me.Panel4.Controls.Add(Me.txtReferenceNo)
        Me.Panel4.Controls.Add(Me.Label17)
        Me.Panel4.Controls.Add(Me.Label24)
        Me.Panel4.Location = New System.Drawing.Point(844, 430)
        Me.Panel4.Name = "Panel4"
        Me.Panel4.Size = New System.Drawing.Size(344, 304)
        Me.Panel4.TabIndex = 103
        '
        'Label11
        '
        Me.Label11.AutoSize = True
        Me.Label11.Font = New System.Drawing.Font("Segoe UI", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label11.Location = New System.Drawing.Point(18, 265)
        Me.Label11.Name = "Label11"
        Me.Label11.Size = New System.Drawing.Size(85, 17)
        Me.Label11.TabIndex = 107
        Me.Label11.Text = "Total Amount"
        '
        'txtPaymentMethod
        '
        Me.txtPaymentMethod.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.txtPaymentMethod.Font = New System.Drawing.Font("Segoe UI", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtPaymentMethod.Location = New System.Drawing.Point(135, 132)
        Me.txtPaymentMethod.Name = "txtPaymentMethod"
        Me.txtPaymentMethod.ReadOnly = True
        Me.txtPaymentMethod.Size = New System.Drawing.Size(190, 25)
        Me.txtPaymentMethod.TabIndex = 106
        '
        'txttransactdate
        '
        Me.txttransactdate.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.txttransactdate.Font = New System.Drawing.Font("Segoe UI", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txttransactdate.Location = New System.Drawing.Point(135, 93)
        Me.txttransactdate.Name = "txttransactdate"
        Me.txttransactdate.ReadOnly = True
        Me.txttransactdate.Size = New System.Drawing.Size(190, 25)
        Me.txttransactdate.TabIndex = 105
        '
        'Panel9
        '
        Me.Panel9.BackColor = System.Drawing.Color.FromArgb(CType(CType(1, Byte), Integer), CType(CType(21, Byte), Integer), CType(CType(78, Byte), Integer))
        Me.Panel9.Controls.Add(Me.Label20)
        Me.Panel9.Location = New System.Drawing.Point(0, 0)
        Me.Panel9.Name = "Panel9"
        Me.Panel9.Size = New System.Drawing.Size(343, 35)
        Me.Panel9.TabIndex = 11
        '
        'Label20
        '
        Me.Label20.AutoSize = True
        Me.Label20.Font = New System.Drawing.Font("Segoe UI Semibold", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label20.ForeColor = System.Drawing.Color.White
        Me.Label20.Location = New System.Drawing.Point(7, 7)
        Me.Label20.Name = "Label20"
        Me.Label20.Size = New System.Drawing.Size(164, 21)
        Me.Label20.TabIndex = 10
        Me.Label20.Text = "Payment Information"
        '
        'Label21
        '
        Me.Label21.AutoSize = True
        Me.Label21.Font = New System.Drawing.Font("Segoe UI", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label21.Location = New System.Drawing.Point(17, 134)
        Me.Label21.Name = "Label21"
        Me.Label21.Size = New System.Drawing.Size(107, 17)
        Me.Label21.TabIndex = 104
        Me.Label21.Text = "Payment Method"
        '
        'btnRemoveItem
        '
        Me.btnRemoveItem.BackColor = System.Drawing.Color.IndianRed
        Me.btnRemoveItem.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnRemoveItem.ForeColor = System.Drawing.Color.White
        Me.btnRemoveItem.Location = New System.Drawing.Point(35, 759)
        Me.btnRemoveItem.Name = "btnRemoveItem"
        Me.btnRemoveItem.Size = New System.Drawing.Size(200, 40)
        Me.btnRemoveItem.TabIndex = 104
        Me.btnRemoveItem.Text = "Remove Item from Cart"
        Me.btnRemoveItem.UseVisualStyleBackColor = False
        '
        'DateTimePicker2
        '
        Me.DateTimePicker2.Location = New System.Drawing.Point(824, 661)
        Me.DateTimePicker2.Name = "DateTimePicker2"
        Me.DateTimePicker2.Size = New System.Drawing.Size(200, 20)
        Me.DateTimePicker2.TabIndex = 105
        '
        'Label14
        '
        Me.Label14.AutoSize = True
        Me.Label14.Font = New System.Drawing.Font("Segoe UI Semibold", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label14.Location = New System.Drawing.Point(473, 445)
        Me.Label14.Name = "Label14"
        Me.Label14.Size = New System.Drawing.Size(61, 17)
        Me.Label14.TabIndex = 106
        Me.Label14.Text = "Quantity"
        '
        'Panel6
        '
        Me.Panel6.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.Panel6.Controls.Add(Me.Panel10)
        Me.Panel6.Location = New System.Drawing.Point(33, 115)
        Me.Panel6.Name = "Panel6"
        Me.Panel6.Size = New System.Drawing.Size(786, 313)
        Me.Panel6.TabIndex = 17
        '
        'Panel10
        '
        Me.Panel10.BackColor = System.Drawing.Color.FromArgb(CType(CType(1, Byte), Integer), CType(CType(21, Byte), Integer), CType(CType(78, Byte), Integer))
        Me.Panel10.Controls.Add(Me.Label9)
        Me.Panel10.Location = New System.Drawing.Point(0, 0)
        Me.Panel10.Name = "Panel10"
        Me.Panel10.Size = New System.Drawing.Size(785, 35)
        Me.Panel10.TabIndex = 0
        '
        'Label9
        '
        Me.Label9.AutoSize = True
        Me.Label9.Font = New System.Drawing.Font("Segoe UI Semibold", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label9.ForeColor = System.Drawing.Color.White
        Me.Label9.Location = New System.Drawing.Point(7, 6)
        Me.Label9.Name = "Label9"
        Me.Label9.Size = New System.Drawing.Size(75, 21)
        Me.Label9.TabIndex = 9
        Me.Label9.Text = "Products"
        '
        'dgvlistproducts
        '
        Me.dgvlistproducts.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill
        Me.dgvlistproducts.AutoSizeRowsMode = System.Windows.Forms.DataGridViewAutoSizeRowsMode.AllCells
        Me.dgvlistproducts.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.dgvlistproducts.Columns.AddRange(New System.Windows.Forms.DataGridViewColumn() {Me.ProductCode, Me.DataGridViewTextBoxColumn1, Me.Category, Me.TypeofProduct, Me.DataGridViewTextBoxColumn2, Me.DataGridViewTextBoxColumn3, Me.DataGridViewTextBoxColumn4, Me.Status})
        DataGridViewCellStyle6.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft
        DataGridViewCellStyle6.BackColor = System.Drawing.SystemColors.Window
        DataGridViewCellStyle6.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        DataGridViewCellStyle6.ForeColor = System.Drawing.SystemColors.ControlText
        DataGridViewCellStyle6.SelectionBackColor = System.Drawing.SystemColors.Highlight
        DataGridViewCellStyle6.SelectionForeColor = System.Drawing.SystemColors.HighlightText
        DataGridViewCellStyle6.WrapMode = System.Windows.Forms.DataGridViewTriState.[True]
        Me.dgvlistproducts.DefaultCellStyle = DataGridViewCellStyle6
        Me.dgvlistproducts.Location = New System.Drawing.Point(34, 149)
        Me.dgvlistproducts.Name = "dgvlistproducts"
        Me.dgvlistproducts.ReadOnly = True
        Me.dgvlistproducts.Size = New System.Drawing.Size(785, 282)
        Me.dgvlistproducts.TabIndex = 2
        '
        'ProductCode
        '
        Me.ProductCode.HeaderText = "Product Code"
        Me.ProductCode.Name = "ProductCode"
        Me.ProductCode.ReadOnly = True
        '
        'DataGridViewTextBoxColumn1
        '
        Me.DataGridViewTextBoxColumn1.HeaderText = "Product Name"
        Me.DataGridViewTextBoxColumn1.Name = "DataGridViewTextBoxColumn1"
        Me.DataGridViewTextBoxColumn1.ReadOnly = True
        '
        'Category
        '
        Me.Category.HeaderText = "Category"
        Me.Category.Name = "Category"
        Me.Category.ReadOnly = True
        '
        'TypeofProduct
        '
        Me.TypeofProduct.HeaderText = "Type of Product"
        Me.TypeofProduct.Name = "TypeofProduct"
        Me.TypeofProduct.ReadOnly = True
        '
        'DataGridViewTextBoxColumn2
        '
        Me.DataGridViewTextBoxColumn2.HeaderText = "Size"
        Me.DataGridViewTextBoxColumn2.Name = "DataGridViewTextBoxColumn2"
        Me.DataGridViewTextBoxColumn2.ReadOnly = True
        '
        'DataGridViewTextBoxColumn3
        '
        Me.DataGridViewTextBoxColumn3.HeaderText = "Unit Price"
        Me.DataGridViewTextBoxColumn3.Name = "DataGridViewTextBoxColumn3"
        Me.DataGridViewTextBoxColumn3.ReadOnly = True
        '
        'DataGridViewTextBoxColumn4
        '
        Me.DataGridViewTextBoxColumn4.HeaderText = "Quantity"
        Me.DataGridViewTextBoxColumn4.Name = "DataGridViewTextBoxColumn4"
        Me.DataGridViewTextBoxColumn4.ReadOnly = True
        '
        'Status
        '
        Me.Status.HeaderText = "Status"
        Me.Status.Name = "Status"
        Me.Status.ReadOnly = True
        '
        'nudQuantity
        '
        Me.nudQuantity.Font = New System.Drawing.Font("Segoe UI", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.nudQuantity.Location = New System.Drawing.Point(535, 442)
        Me.nudQuantity.Name = "nudQuantity"
        Me.nudQuantity.Size = New System.Drawing.Size(105, 25)
        Me.nudQuantity.TabIndex = 113
        '
        'Panel12
        '
        Me.Panel12.BackColor = System.Drawing.Color.White
        Me.Panel12.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.Panel12.Controls.Add(Me.picSearchProduct)
        Me.Panel12.Controls.Add(Me.txtProductSearch)
        Me.Panel12.Location = New System.Drawing.Point(555, 78)
        Me.Panel12.Name = "Panel12"
        Me.Panel12.Size = New System.Drawing.Size(263, 25)
        Me.Panel12.TabIndex = 115
        '
        'picSearchProduct
        '
        Me.picSearchProduct.BackgroundImage = CType(resources.GetObject("picSearchProduct.BackgroundImage"), System.Drawing.Image)
        Me.picSearchProduct.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch
        Me.picSearchProduct.Location = New System.Drawing.Point(239, 2)
        Me.picSearchProduct.Name = "picSearchProduct"
        Me.picSearchProduct.Size = New System.Drawing.Size(19, 19)
        Me.picSearchProduct.TabIndex = 29
        Me.picSearchProduct.TabStop = False
        '
        'txtProductSearch
        '
        Me.txtProductSearch.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.txtProductSearch.Font = New System.Drawing.Font("Segoe UI", 11.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtProductSearch.Location = New System.Drawing.Point(3, 2)
        Me.txtProductSearch.Name = "txtProductSearch"
        Me.txtProductSearch.Size = New System.Drawing.Size(230, 20)
        Me.txtProductSearch.TabIndex = 28
        '
        'Label10
        '
        Me.Label10.AutoSize = True
        Me.Label10.Font = New System.Drawing.Font("Segoe UI", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label10.Location = New System.Drawing.Point(502, 83)
        Me.Label10.Name = "Label10"
        Me.Label10.Size = New System.Drawing.Size(47, 17)
        Me.Label10.TabIndex = 114
        Me.Label10.Text = "Search"
        '
        'Label5
        '
        Me.Label5.AutoSize = True
        Me.Label5.Font = New System.Drawing.Font("Segoe UI", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label5.Location = New System.Drawing.Point(31, 82)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(61, 17)
        Me.Label5.TabIndex = 116
        Me.Label5.Text = "Category"
        '
        'cbocategory
        '
        Me.cbocategory.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.cbocategory.Font = New System.Drawing.Font("Segoe UI", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cbocategory.FormattingEnabled = True
        Me.cbocategory.Items.AddRange(New Object() {"Uniforms", "Books", "Modules", "School Supplies", "Office Supplies", "All Items"})
        Me.cbocategory.Location = New System.Drawing.Point(98, 78)
        Me.cbocategory.Name = "cbocategory"
        Me.cbocategory.Size = New System.Drawing.Size(178, 25)
        Me.cbocategory.TabIndex = 97
        '
        'Label12
        '
        Me.Label12.AutoSize = True
        Me.Label12.Font = New System.Drawing.Font("Segoe UI", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label12.Location = New System.Drawing.Point(862, 78)
        Me.Label12.Name = "Label12"
        Me.Label12.Size = New System.Drawing.Size(99, 17)
        Me.Label12.TabIndex = 98
        Me.Label12.Text = "Transaction No."
        '
        'txtTransactionNo
        '
        Me.txtTransactionNo.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.txtTransactionNo.Font = New System.Drawing.Font("Segoe UI", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtTransactionNo.Location = New System.Drawing.Point(979, 76)
        Me.txtTransactionNo.Name = "txtTransactionNo"
        Me.txtTransactionNo.ReadOnly = True
        Me.txtTransactionNo.Size = New System.Drawing.Size(190, 25)
        Me.txtTransactionNo.TabIndex = 97
        '
        'cboType
        '
        Me.cboType.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.cboType.Font = New System.Drawing.Font("Segoe UI", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cboType.FormattingEnabled = True
        Me.cboType.Items.AddRange(New Object() {"Uniforms", "Books", "Modules", "School Supplies", "Office Supplies", "All Items"})
        Me.cboType.Location = New System.Drawing.Point(326, 78)
        Me.cboType.Name = "cboType"
        Me.cboType.Size = New System.Drawing.Size(166, 25)
        Me.cboType.TabIndex = 117
        '
        'Type
        '
        Me.Type.AutoSize = True
        Me.Type.Font = New System.Drawing.Font("Segoe UI", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Type.Location = New System.Drawing.Point(289, 82)
        Me.Type.Name = "Type"
        Me.Type.Size = New System.Drawing.Size(35, 17)
        Me.Type.TabIndex = 118
        Me.Type.Text = "Type"
        '
        'frmPOS
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(1220, 850)
        Me.Controls.Add(Me.cboType)
        Me.Controls.Add(Me.Type)
        Me.Controls.Add(Me.Label12)
        Me.Controls.Add(Me.txtTransactionNo)
        Me.Controls.Add(Me.cbocategory)
        Me.Controls.Add(Me.Label5)
        Me.Controls.Add(Me.Panel12)
        Me.Controls.Add(Me.Label10)
        Me.Controls.Add(Me.nudQuantity)
        Me.Controls.Add(Me.dgvlistproducts)
        Me.Controls.Add(Me.Panel6)
        Me.Controls.Add(Me.Label14)
        Me.Controls.Add(Me.btnRemoveItem)
        Me.Controls.Add(Me.btnClear)
        Me.Controls.Add(Me.btnSaveTransaction)
        Me.Controls.Add(Me.Panel4)
        Me.Controls.Add(Me.btnSettlePayment)
        Me.Controls.Add(Me.btnCancelTransaction)
        Me.Controls.Add(Me.btnAddToCart)
        Me.Controls.Add(Me.Label4)
        Me.Controls.Add(Me.Panel13)
        Me.Controls.Add(Me.Panel7)
        Me.Controls.Add(Me.Label1)
        Me.Controls.Add(Me.Panel5)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None
        Me.Name = "frmPOS"
        Me.Text = "frmPOS"
        Me.Panel5.ResumeLayout(False)
        Me.Panel5.PerformLayout()
        Me.Panel11.ResumeLayout(False)
        Me.Panel11.PerformLayout()
        Me.Panel7.ResumeLayout(False)
        CType(Me.dgvCart, System.ComponentModel.ISupportInitialize).EndInit()
        Me.Panel8.ResumeLayout(False)
        Me.Panel8.PerformLayout()
        Me.Panel13.ResumeLayout(False)
        Me.Panel13.PerformLayout()
        Me.Panel4.ResumeLayout(False)
        Me.Panel4.PerformLayout()
        Me.Panel9.ResumeLayout(False)
        Me.Panel9.PerformLayout()
        Me.Panel6.ResumeLayout(False)
        Me.Panel10.ResumeLayout(False)
        Me.Panel10.PerformLayout()
        CType(Me.dgvlistproducts, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.nudQuantity, System.ComponentModel.ISupportInitialize).EndInit()
        Me.Panel12.ResumeLayout(False)
        Me.Panel12.PerformLayout()
        CType(Me.picSearchProduct, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents Panel5 As Panel
    Friend WithEvents Label2 As Label
    Friend WithEvents Label1 As Label
    Friend WithEvents Panel7 As Panel
    Friend WithEvents Panel8 As Panel
    Friend WithEvents Label15 As Label
    Friend WithEvents btnAddToCart As Button
    Friend WithEvents btnSettlePayment As Button
    Friend WithEvents btnCancelTransaction As Button
    Friend WithEvents Panel11 As Panel
    Friend WithEvents dgvCart As DataGridView
    Friend WithEvents lblposition As Label
    Friend WithEvents Label28 As Label
    Friend WithEvents lblname As Label
    Friend WithEvents Label29 As Label
    Friend WithEvents Panel13 As Panel
    Friend WithEvents Label4 As Label
    Friend WithEvents Label24 As Label
    Friend WithEvents txtTotalAMount As TextBox
    Friend WithEvents btnClear As Button
    Friend WithEvents btnSaveTransaction As Button
    Friend WithEvents txtReferenceNo As TextBox
    Friend WithEvents Label17 As Label
    Friend WithEvents txtAmountReceived As TextBox
    Friend WithEvents Label18 As Label
    Friend WithEvents txtAmountChange As TextBox
    Friend WithEvents Label19 As Label
    Friend WithEvents Panel4 As Panel
    Friend WithEvents Panel9 As Panel
    Friend WithEvents Label20 As Label
    Friend WithEvents btnRemoveItem As Button
    Friend WithEvents Label21 As Label
    Friend WithEvents Timer1 As Timer
    Friend WithEvents DateTimePicker2 As DateTimePicker
    Friend WithEvents Label14 As Label
    Friend WithEvents Panel6 As Panel
    Friend WithEvents Panel10 As Panel
    Friend WithEvents Label9 As Label
    Friend WithEvents dgvlistproducts As DataGridView
    Friend WithEvents nudQuantity As NumericUpDown
    Friend WithEvents Panel12 As Panel
    Friend WithEvents picSearchProduct As PictureBox
    Friend WithEvents txtProductSearch As TextBox
    Friend WithEvents Label10 As Label
    Friend WithEvents Label5 As Label
    Friend WithEvents cbocategory As ComboBox
    Friend WithEvents txtPaymentMethod As TextBox
    Friend WithEvents txttransactdate As TextBox
    Friend WithEvents Label12 As Label
    Friend WithEvents txtTransactionNo As TextBox
    Friend WithEvents ProductName As DataGridViewTextBoxColumn
    Friend WithEvents Size As DataGridViewTextBoxColumn
    Friend WithEvents Quantity As DataGridViewTextBoxColumn
    Friend WithEvents UnitPrice As DataGridViewTextBoxColumn
    Friend WithEvents SubTotal As DataGridViewTextBoxColumn
    Friend WithEvents ProductCode As DataGridViewTextBoxColumn
    Friend WithEvents DataGridViewTextBoxColumn1 As DataGridViewTextBoxColumn
    Friend WithEvents Category As DataGridViewTextBoxColumn
    Friend WithEvents TypeofProduct As DataGridViewTextBoxColumn
    Friend WithEvents DataGridViewTextBoxColumn2 As DataGridViewTextBoxColumn
    Friend WithEvents DataGridViewTextBoxColumn3 As DataGridViewTextBoxColumn
    Friend WithEvents DataGridViewTextBoxColumn4 As DataGridViewTextBoxColumn
    Friend WithEvents Status As DataGridViewTextBoxColumn
    Friend WithEvents cboType As ComboBox
    Friend WithEvents Type As Label
    Friend WithEvents Label11 As Label
    Friend WithEvents cbobuyertype As ComboBox
    Friend WithEvents Label13 As Label
    Friend WithEvents Label16 As Label
    Friend WithEvents pnlbuyertype As Panel
End Class
