<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmReturnExchange
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
        Me.btncancel = New System.Windows.Forms.Button()
        Me.btnreturnexc = New System.Windows.Forms.Button()
        Me.Label10 = New System.Windows.Forms.Label()
        Me.Label11 = New System.Windows.Forms.Label()
        Me.Panel7 = New System.Windows.Forms.Panel()
        Me.dgvCart = New System.Windows.Forms.DataGridView()
        Me.ProductName = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.Category = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.Size = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.Quantity = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.UnitPrice = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.SubTotal = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.Panel8 = New System.Windows.Forms.Panel()
        Me.Label15 = New System.Windows.Forms.Label()
        Me.Panel3 = New System.Windows.Forms.Panel()
        Me.dtpORDate = New System.Windows.Forms.DateTimePicker()
        Me.Label19 = New System.Windows.Forms.Label()
        Me.txtsize = New System.Windows.Forms.ComboBox()
        Me.txtProduct = New System.Windows.Forms.ComboBox()
        Me.txtAvailstock = New System.Windows.Forms.ComboBox()
        Me.Label18 = New System.Windows.Forms.Label()
        Me.numupqty = New System.Windows.Forms.NumericUpDown()
        Me.Label9 = New System.Windows.Forms.Label()
        Me.Panel5 = New System.Windows.Forms.Panel()
        Me.Label4 = New System.Windows.Forms.Label()
        Me.Label13 = New System.Windows.Forms.Label()
        Me.Label16 = New System.Windows.Forms.Label()
        Me.txtReason = New System.Windows.Forms.TextBox()
        Me.Panel1 = New System.Windows.Forms.Panel()
        Me.reason = New System.Windows.Forms.Label()
        Me.lblcondition = New System.Windows.Forms.Label()
        Me.txtcreatedby = New System.Windows.Forms.TextBox()
        Me.Label12 = New System.Windows.Forms.Label()
        Me.txtcondition = New System.Windows.Forms.TextBox()
        Me.lblquantity = New System.Windows.Forms.Label()
        Me.nudQuantity = New System.Windows.Forms.NumericUpDown()
        Me.Label14 = New System.Windows.Forms.Label()
        Me.Label5 = New System.Windows.Forms.Label()
        Me.rbtnexchange = New System.Windows.Forms.RadioButton()
        Me.rbtnReturn = New System.Windows.Forms.RadioButton()
        Me.Panel2 = New System.Windows.Forms.Panel()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.Panel4 = New System.Windows.Forms.Panel()
        Me.txtpaymentmethod = New System.Windows.Forms.TextBox()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.txtstatus = New System.Windows.Forms.TextBox()
        Me.dtpTransactionDate = New System.Windows.Forms.DateTimePicker()
        Me.Label21 = New System.Windows.Forms.Label()
        Me.txtorno = New System.Windows.Forms.TextBox()
        Me.Label17 = New System.Windows.Forms.Label()
        Me.Label24 = New System.Windows.Forms.Label()
        Me.txtgrade = New System.Windows.Forms.TextBox()
        Me.btnStudentno = New System.Windows.Forms.TextBox()
        Me.Label7 = New System.Windows.Forms.Label()
        Me.Panel9 = New System.Windows.Forms.Panel()
        Me.Label20 = New System.Windows.Forms.Label()
        Me.Label8 = New System.Windows.Forms.Label()
        Me.Label6 = New System.Windows.Forms.Label()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.txtProgramStrand = New System.Windows.Forms.TextBox()
        Me.txtStudentName = New System.Windows.Forms.TextBox()
        Me.Panel7.SuspendLayout()
        CType(Me.dgvCart, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.Panel8.SuspendLayout()
        Me.Panel3.SuspendLayout()
        CType(Me.numupqty, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.Panel5.SuspendLayout()
        Me.Panel1.SuspendLayout()
        CType(Me.nudQuantity, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.Panel2.SuspendLayout()
        Me.Panel4.SuspendLayout()
        Me.Panel9.SuspendLayout()
        Me.SuspendLayout()
        '
        'btncancel
        '
        Me.btncancel.BackColor = System.Drawing.Color.SlateGray
        Me.btncancel.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btncancel.ForeColor = System.Drawing.Color.White
        Me.btncancel.Location = New System.Drawing.Point(552, 765)
        Me.btncancel.Name = "btncancel"
        Me.btncancel.Size = New System.Drawing.Size(170, 40)
        Me.btncancel.TabIndex = 129
        Me.btncancel.Text = "Cancel"
        Me.btncancel.UseVisualStyleBackColor = False
        '
        'btnreturnexc
        '
        Me.btnreturnexc.BackColor = System.Drawing.Color.FromArgb(CType(CType(1, Byte), Integer), CType(CType(21, Byte), Integer), CType(CType(78, Byte), Integer))
        Me.btnreturnexc.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnreturnexc.ForeColor = System.Drawing.Color.White
        Me.btnreturnexc.Location = New System.Drawing.Point(378, 765)
        Me.btnreturnexc.Name = "btnreturnexc"
        Me.btnreturnexc.Size = New System.Drawing.Size(160, 40)
        Me.btnreturnexc.TabIndex = 128
        Me.btnreturnexc.Text = "Return / Exchange"
        Me.btnreturnexc.UseVisualStyleBackColor = False
        '
        'Label10
        '
        Me.Label10.AutoSize = True
        Me.Label10.Font = New System.Drawing.Font("Segoe UI", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label10.ForeColor = System.Drawing.SystemColors.ControlDarkDark
        Me.Label10.Location = New System.Drawing.Point(18, 50)
        Me.Label10.Name = "Label10"
        Me.Label10.Size = New System.Drawing.Size(318, 15)
        Me.Label10.TabIndex = 126
        Me.Label10.Text = "Process a return or exchange from a completed transaction"
        '
        'Label11
        '
        Me.Label11.AutoSize = True
        Me.Label11.Font = New System.Drawing.Font("Segoe UI", 18.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label11.ForeColor = System.Drawing.Color.FromArgb(CType(CType(1, Byte), Integer), CType(CType(21, Byte), Integer), CType(CType(78, Byte), Integer))
        Me.Label11.Location = New System.Drawing.Point(15, 17)
        Me.Label11.Name = "Label11"
        Me.Label11.Size = New System.Drawing.Size(235, 32)
        Me.Label11.TabIndex = 125
        Me.Label11.Text = "Returns / Exchange"
        '
        'Panel7
        '
        Me.Panel7.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.Panel7.Controls.Add(Me.dgvCart)
        Me.Panel7.Controls.Add(Me.Panel8)
        Me.Panel7.Location = New System.Drawing.Point(21, 315)
        Me.Panel7.Name = "Panel7"
        Me.Panel7.Size = New System.Drawing.Size(701, 155)
        Me.Panel7.TabIndex = 124
        '
        'dgvCart
        '
        Me.dgvCart.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.dgvCart.Columns.AddRange(New System.Windows.Forms.DataGridViewColumn() {Me.ProductName, Me.Category, Me.Size, Me.Quantity, Me.UnitPrice, Me.SubTotal})
        Me.dgvCart.Location = New System.Drawing.Point(-2, 35)
        Me.dgvCart.Name = "dgvCart"
        Me.dgvCart.ReadOnly = True
        Me.dgvCart.Size = New System.Drawing.Size(712, 119)
        Me.dgvCart.TabIndex = 10
        '
        'ProductName
        '
        Me.ProductName.HeaderText = "Product Name"
        Me.ProductName.Name = "ProductName"
        Me.ProductName.ReadOnly = True
        Me.ProductName.Width = 180
        '
        'Category
        '
        Me.Category.HeaderText = "Category"
        Me.Category.Name = "Category"
        Me.Category.ReadOnly = True
        Me.Category.Width = 150
        '
        'Size
        '
        Me.Size.HeaderText = "Size"
        Me.Size.Name = "Size"
        Me.Size.ReadOnly = True
        Me.Size.Width = 65
        '
        'Quantity
        '
        Me.Quantity.HeaderText = "Quantity Purchased"
        Me.Quantity.Name = "Quantity"
        Me.Quantity.ReadOnly = True
        Me.Quantity.Width = 90
        '
        'UnitPrice
        '
        Me.UnitPrice.HeaderText = "Unit Price"
        Me.UnitPrice.Name = "UnitPrice"
        Me.UnitPrice.ReadOnly = True
        Me.UnitPrice.Width = 90
        '
        'SubTotal
        '
        Me.SubTotal.HeaderText = "SubTotal"
        Me.SubTotal.Name = "SubTotal"
        Me.SubTotal.ReadOnly = True
        Me.SubTotal.Width = 85
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
        Me.Label15.Size = New System.Drawing.Size(239, 21)
        Me.Label15.TabIndex = 9
        Me.Label15.Text = "Items from Original Transaction"
        '
        'Panel3
        '
        Me.Panel3.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.Panel3.Controls.Add(Me.dtpORDate)
        Me.Panel3.Controls.Add(Me.Label19)
        Me.Panel3.Controls.Add(Me.txtsize)
        Me.Panel3.Controls.Add(Me.txtProduct)
        Me.Panel3.Controls.Add(Me.txtAvailstock)
        Me.Panel3.Controls.Add(Me.Label18)
        Me.Panel3.Controls.Add(Me.numupqty)
        Me.Panel3.Controls.Add(Me.Label9)
        Me.Panel3.Controls.Add(Me.Panel5)
        Me.Panel3.Controls.Add(Me.Label13)
        Me.Panel3.Controls.Add(Me.Label16)
        Me.Panel3.Location = New System.Drawing.Point(378, 486)
        Me.Panel3.Name = "Panel3"
        Me.Panel3.Size = New System.Drawing.Size(345, 260)
        Me.Panel3.TabIndex = 121
        '
        'dtpORDate
        '
        Me.dtpORDate.Font = New System.Drawing.Font("Segoe UI", 11.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.dtpORDate.Location = New System.Drawing.Point(135, 215)
        Me.dtpORDate.Name = "dtpORDate"
        Me.dtpORDate.Size = New System.Drawing.Size(193, 27)
        Me.dtpORDate.TabIndex = 133
        '
        'Label19
        '
        Me.Label19.AutoSize = True
        Me.Label19.Font = New System.Drawing.Font("Segoe UI", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label19.Location = New System.Drawing.Point(15, 222)
        Me.Label19.Name = "Label19"
        Me.Label19.Size = New System.Drawing.Size(35, 17)
        Me.Label19.TabIndex = 132
        Me.Label19.Text = "Date"
        '
        'txtsize
        '
        Me.txtsize.Font = New System.Drawing.Font("Segoe UI", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtsize.FormattingEnabled = True
        Me.txtsize.Location = New System.Drawing.Point(135, 88)
        Me.txtsize.Name = "txtsize"
        Me.txtsize.Size = New System.Drawing.Size(193, 25)
        Me.txtsize.TabIndex = 131
        '
        'txtProduct
        '
        Me.txtProduct.Font = New System.Drawing.Font("Segoe UI", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtProduct.FormattingEnabled = True
        Me.txtProduct.Location = New System.Drawing.Point(135, 50)
        Me.txtProduct.Name = "txtProduct"
        Me.txtProduct.Size = New System.Drawing.Size(193, 25)
        Me.txtProduct.TabIndex = 130
        '
        'txtAvailstock
        '
        Me.txtAvailstock.Font = New System.Drawing.Font("Segoe UI", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtAvailstock.FormattingEnabled = True
        Me.txtAvailstock.Location = New System.Drawing.Point(135, 129)
        Me.txtAvailstock.Name = "txtAvailstock"
        Me.txtAvailstock.Size = New System.Drawing.Size(193, 25)
        Me.txtAvailstock.TabIndex = 129
        '
        'Label18
        '
        Me.Label18.AutoSize = True
        Me.Label18.Font = New System.Drawing.Font("Segoe UI", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label18.Location = New System.Drawing.Point(15, 173)
        Me.Label18.Name = "Label18"
        Me.Label18.Size = New System.Drawing.Size(107, 17)
        Me.Label18.TabIndex = 128
        Me.Label18.Text = "Replacement Qty"
        '
        'numupqty
        '
        Me.numupqty.Font = New System.Drawing.Font("Segoe UI", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.numupqty.Location = New System.Drawing.Point(135, 169)
        Me.numupqty.Name = "numupqty"
        Me.numupqty.Size = New System.Drawing.Size(193, 25)
        Me.numupqty.TabIndex = 125
        '
        'Label9
        '
        Me.Label9.AutoSize = True
        Me.Label9.Font = New System.Drawing.Font("Segoe UI", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label9.Location = New System.Drawing.Point(15, 132)
        Me.Label9.Name = "Label9"
        Me.Label9.Size = New System.Drawing.Size(95, 17)
        Me.Label9.TabIndex = 127
        Me.Label9.Text = "Available Stock"
        '
        'Panel5
        '
        Me.Panel5.BackColor = System.Drawing.Color.FromArgb(CType(CType(1, Byte), Integer), CType(CType(21, Byte), Integer), CType(CType(78, Byte), Integer))
        Me.Panel5.Controls.Add(Me.Label4)
        Me.Panel5.Location = New System.Drawing.Point(0, 0)
        Me.Panel5.Name = "Panel5"
        Me.Panel5.Size = New System.Drawing.Size(355, 35)
        Me.Panel5.TabIndex = 11
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.Font = New System.Drawing.Font("Segoe UI Semibold", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label4.ForeColor = System.Drawing.Color.White
        Me.Label4.Location = New System.Drawing.Point(7, 7)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(312, 21)
        Me.Label4.TabIndex = 10
        Me.Label4.Text = "Replacement Item (Required if Exchange)"
        '
        'Label13
        '
        Me.Label13.AutoSize = True
        Me.Label13.Font = New System.Drawing.Font("Segoe UI", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label13.Location = New System.Drawing.Point(15, 91)
        Me.Label13.Name = "Label13"
        Me.Label13.Size = New System.Drawing.Size(31, 17)
        Me.Label13.TabIndex = 126
        Me.Label13.Text = "Size"
        '
        'Label16
        '
        Me.Label16.AutoSize = True
        Me.Label16.Font = New System.Drawing.Font("Segoe UI", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label16.Location = New System.Drawing.Point(15, 53)
        Me.Label16.Name = "Label16"
        Me.Label16.Size = New System.Drawing.Size(53, 17)
        Me.Label16.TabIndex = 125
        Me.Label16.Text = "Product"
        '
        'txtReason
        '
        Me.txtReason.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.txtReason.Font = New System.Drawing.Font("Segoe UI", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtReason.Location = New System.Drawing.Point(116, 108)
        Me.txtReason.Multiline = True
        Me.txtReason.Name = "txtReason"
        Me.txtReason.ReadOnly = True
        Me.txtReason.Size = New System.Drawing.Size(196, 63)
        Me.txtReason.TabIndex = 107
        '
        'Panel1
        '
        Me.Panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.Panel1.Controls.Add(Me.reason)
        Me.Panel1.Controls.Add(Me.lblcondition)
        Me.Panel1.Controls.Add(Me.txtcreatedby)
        Me.Panel1.Controls.Add(Me.txtReason)
        Me.Panel1.Controls.Add(Me.Label12)
        Me.Panel1.Controls.Add(Me.txtcondition)
        Me.Panel1.Controls.Add(Me.lblquantity)
        Me.Panel1.Controls.Add(Me.nudQuantity)
        Me.Panel1.Controls.Add(Me.Label14)
        Me.Panel1.Controls.Add(Me.Label5)
        Me.Panel1.Controls.Add(Me.rbtnexchange)
        Me.Panel1.Controls.Add(Me.rbtnReturn)
        Me.Panel1.Controls.Add(Me.Panel2)
        Me.Panel1.Location = New System.Drawing.Point(22, 486)
        Me.Panel1.Name = "Panel1"
        Me.Panel1.Size = New System.Drawing.Size(326, 260)
        Me.Panel1.TabIndex = 120
        '
        'reason
        '
        Me.reason.AutoSize = True
        Me.reason.Font = New System.Drawing.Font("Segoe UI", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.reason.Location = New System.Drawing.Point(16, 108)
        Me.reason.Name = "reason"
        Me.reason.Size = New System.Drawing.Size(51, 17)
        Me.reason.TabIndex = 124
        Me.reason.Text = "Reason"
        '
        'lblcondition
        '
        Me.lblcondition.AutoSize = True
        Me.lblcondition.Font = New System.Drawing.Font("Segoe UI", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblcondition.Location = New System.Drawing.Point(15, 189)
        Me.lblcondition.Name = "lblcondition"
        Me.lblcondition.Size = New System.Drawing.Size(64, 17)
        Me.lblcondition.TabIndex = 121
        Me.lblcondition.Text = "Condition"
        '
        'txtcreatedby
        '
        Me.txtcreatedby.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.txtcreatedby.Font = New System.Drawing.Font("Segoe UI", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtcreatedby.Location = New System.Drawing.Point(116, 221)
        Me.txtcreatedby.Name = "txtcreatedby"
        Me.txtcreatedby.ReadOnly = True
        Me.txtcreatedby.Size = New System.Drawing.Size(196, 25)
        Me.txtcreatedby.TabIndex = 120
        '
        'Label12
        '
        Me.Label12.AutoSize = True
        Me.Label12.Font = New System.Drawing.Font("Segoe UI", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label12.Location = New System.Drawing.Point(15, 223)
        Me.Label12.Name = "Label12"
        Me.Label12.Size = New System.Drawing.Size(85, 17)
        Me.Label12.TabIndex = 121
        Me.Label12.Text = "Processed By"
        '
        'txtcondition
        '
        Me.txtcondition.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.txtcondition.Font = New System.Drawing.Font("Segoe UI", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtcondition.Location = New System.Drawing.Point(116, 184)
        Me.txtcondition.Name = "txtcondition"
        Me.txtcondition.ReadOnly = True
        Me.txtcondition.Size = New System.Drawing.Size(197, 25)
        Me.txtcondition.TabIndex = 120
        '
        'lblquantity
        '
        Me.lblquantity.AutoSize = True
        Me.lblquantity.Font = New System.Drawing.Font("Segoe UI", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblquantity.Location = New System.Drawing.Point(186, 76)
        Me.lblquantity.Name = "lblquantity"
        Me.lblquantity.Size = New System.Drawing.Size(94, 17)
        Me.lblquantity.TabIndex = 123
        Me.lblquantity.Text = "of - purchased"
        '
        'nudQuantity
        '
        Me.nudQuantity.Font = New System.Drawing.Font("Segoe UI", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.nudQuantity.Location = New System.Drawing.Point(116, 73)
        Me.nudQuantity.Name = "nudQuantity"
        Me.nudQuantity.Size = New System.Drawing.Size(64, 25)
        Me.nudQuantity.TabIndex = 122
        '
        'Label14
        '
        Me.Label14.AutoSize = True
        Me.Label14.Font = New System.Drawing.Font("Segoe UI", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label14.Location = New System.Drawing.Point(16, 76)
        Me.Label14.Name = "Label14"
        Me.Label14.Size = New System.Drawing.Size(56, 17)
        Me.Label14.TabIndex = 121
        Me.Label14.Text = "Quantity"
        '
        'Label5
        '
        Me.Label5.AutoSize = True
        Me.Label5.Font = New System.Drawing.Font("Segoe UI", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label5.Location = New System.Drawing.Point(16, 47)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(75, 17)
        Me.Label5.TabIndex = 120
        Me.Label5.Text = "Action Type"
        '
        'rbtnexchange
        '
        Me.rbtnexchange.AutoSize = True
        Me.rbtnexchange.Location = New System.Drawing.Point(229, 47)
        Me.rbtnexchange.Name = "rbtnexchange"
        Me.rbtnexchange.Size = New System.Drawing.Size(73, 17)
        Me.rbtnexchange.TabIndex = 104
        Me.rbtnexchange.TabStop = True
        Me.rbtnexchange.Text = "Exchange"
        Me.rbtnexchange.UseVisualStyleBackColor = True
        '
        'rbtnReturn
        '
        Me.rbtnReturn.AutoSize = True
        Me.rbtnReturn.Location = New System.Drawing.Point(116, 47)
        Me.rbtnReturn.Name = "rbtnReturn"
        Me.rbtnReturn.Size = New System.Drawing.Size(57, 17)
        Me.rbtnReturn.TabIndex = 103
        Me.rbtnReturn.TabStop = True
        Me.rbtnReturn.Text = "Return"
        Me.rbtnReturn.UseVisualStyleBackColor = True
        '
        'Panel2
        '
        Me.Panel2.BackColor = System.Drawing.Color.FromArgb(CType(CType(1, Byte), Integer), CType(CType(21, Byte), Integer), CType(CType(78, Byte), Integer))
        Me.Panel2.Controls.Add(Me.Label2)
        Me.Panel2.Location = New System.Drawing.Point(0, 0)
        Me.Panel2.Name = "Panel2"
        Me.Panel2.Size = New System.Drawing.Size(355, 35)
        Me.Panel2.TabIndex = 11
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Font = New System.Drawing.Font("Segoe UI Semibold", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label2.ForeColor = System.Drawing.Color.White
        Me.Label2.Location = New System.Drawing.Point(7, 7)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(196, 21)
        Me.Label2.TabIndex = 10
        Me.Label2.Text = "Return / Exchange Details"
        '
        'Panel4
        '
        Me.Panel4.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.Panel4.Controls.Add(Me.txtpaymentmethod)
        Me.Panel4.Controls.Add(Me.Label1)
        Me.Panel4.Controls.Add(Me.txtstatus)
        Me.Panel4.Controls.Add(Me.dtpTransactionDate)
        Me.Panel4.Controls.Add(Me.Label21)
        Me.Panel4.Controls.Add(Me.txtorno)
        Me.Panel4.Controls.Add(Me.Label17)
        Me.Panel4.Controls.Add(Me.Label24)
        Me.Panel4.Controls.Add(Me.txtgrade)
        Me.Panel4.Controls.Add(Me.btnStudentno)
        Me.Panel4.Controls.Add(Me.Label7)
        Me.Panel4.Controls.Add(Me.Panel9)
        Me.Panel4.Controls.Add(Me.Label8)
        Me.Panel4.Controls.Add(Me.Label6)
        Me.Panel4.Controls.Add(Me.Label3)
        Me.Panel4.Controls.Add(Me.txtProgramStrand)
        Me.Panel4.Controls.Add(Me.txtStudentName)
        Me.Panel4.Location = New System.Drawing.Point(21, 82)
        Me.Panel4.Name = "Panel4"
        Me.Panel4.Size = New System.Drawing.Size(701, 215)
        Me.Panel4.TabIndex = 119
        '
        'txtpaymentmethod
        '
        Me.txtpaymentmethod.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.txtpaymentmethod.Font = New System.Drawing.Font("Segoe UI", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtpaymentmethod.Location = New System.Drawing.Point(469, 129)
        Me.txtpaymentmethod.Name = "txtpaymentmethod"
        Me.txtpaymentmethod.ReadOnly = True
        Me.txtpaymentmethod.Size = New System.Drawing.Size(216, 25)
        Me.txtpaymentmethod.TabIndex = 119
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Font = New System.Drawing.Font("Segoe UI", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label1.Location = New System.Drawing.Point(351, 177)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(43, 17)
        Me.Label1.TabIndex = 118
        Me.Label1.Text = "Status"
        '
        'txtstatus
        '
        Me.txtstatus.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.txtstatus.Font = New System.Drawing.Font("Segoe UI", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtstatus.Location = New System.Drawing.Point(469, 173)
        Me.txtstatus.Name = "txtstatus"
        Me.txtstatus.ReadOnly = True
        Me.txtstatus.Size = New System.Drawing.Size(216, 25)
        Me.txtstatus.TabIndex = 117
        '
        'dtpTransactionDate
        '
        Me.dtpTransactionDate.Font = New System.Drawing.Font("Segoe UI", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.dtpTransactionDate.Location = New System.Drawing.Point(469, 87)
        Me.dtpTransactionDate.Name = "dtpTransactionDate"
        Me.dtpTransactionDate.Size = New System.Drawing.Size(216, 25)
        Me.dtpTransactionDate.TabIndex = 116
        '
        'Label21
        '
        Me.Label21.AutoSize = True
        Me.Label21.Font = New System.Drawing.Font("Segoe UI", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label21.Location = New System.Drawing.Point(351, 132)
        Me.Label21.Name = "Label21"
        Me.Label21.Size = New System.Drawing.Size(107, 17)
        Me.Label21.TabIndex = 115
        Me.Label21.Text = "Payment Method"
        '
        'txtorno
        '
        Me.txtorno.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.txtorno.Font = New System.Drawing.Font("Segoe UI", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtorno.Location = New System.Drawing.Point(469, 48)
        Me.txtorno.Name = "txtorno"
        Me.txtorno.ReadOnly = True
        Me.txtorno.Size = New System.Drawing.Size(216, 25)
        Me.txtorno.TabIndex = 113
        '
        'Label17
        '
        Me.Label17.AutoSize = True
        Me.Label17.Font = New System.Drawing.Font("Segoe UI", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label17.Location = New System.Drawing.Point(351, 52)
        Me.Label17.Name = "Label17"
        Me.Label17.Size = New System.Drawing.Size(51, 17)
        Me.Label17.TabIndex = 114
        Me.Label17.Text = "OR No."
        '
        'Label24
        '
        Me.Label24.AutoSize = True
        Me.Label24.Font = New System.Drawing.Font("Segoe UI", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label24.Location = New System.Drawing.Point(351, 90)
        Me.Label24.Name = "Label24"
        Me.Label24.Size = New System.Drawing.Size(105, 17)
        Me.Label24.TabIndex = 112
        Me.Label24.Text = "Transaction Date"
        '
        'txtgrade
        '
        Me.txtgrade.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.txtgrade.Font = New System.Drawing.Font("Segoe UI", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtgrade.Location = New System.Drawing.Point(140, 129)
        Me.txtgrade.Name = "txtgrade"
        Me.txtgrade.ReadOnly = True
        Me.txtgrade.Size = New System.Drawing.Size(186, 25)
        Me.txtgrade.TabIndex = 110
        '
        'btnStudentno
        '
        Me.btnStudentno.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.btnStudentno.Font = New System.Drawing.Font("Segoe UI", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnStudentno.Location = New System.Drawing.Point(140, 49)
        Me.btnStudentno.Name = "btnStudentno"
        Me.btnStudentno.ReadOnly = True
        Me.btnStudentno.Size = New System.Drawing.Size(186, 25)
        Me.btnStudentno.TabIndex = 107
        '
        'Label7
        '
        Me.Label7.AutoSize = True
        Me.Label7.Font = New System.Drawing.Font("Segoe UI", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label7.Location = New System.Drawing.Point(14, 177)
        Me.Label7.Name = "Label7"
        Me.Label7.Size = New System.Drawing.Size(110, 17)
        Me.Label7.TabIndex = 14
        Me.Label7.Text = "Program / Strand"
        '
        'Panel9
        '
        Me.Panel9.BackColor = System.Drawing.Color.FromArgb(CType(CType(1, Byte), Integer), CType(CType(21, Byte), Integer), CType(CType(78, Byte), Integer))
        Me.Panel9.Controls.Add(Me.Label20)
        Me.Panel9.Location = New System.Drawing.Point(0, 0)
        Me.Panel9.Name = "Panel9"
        Me.Panel9.Size = New System.Drawing.Size(724, 35)
        Me.Panel9.TabIndex = 11
        '
        'Label20
        '
        Me.Label20.AutoSize = True
        Me.Label20.Font = New System.Drawing.Font("Segoe UI Semibold", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label20.ForeColor = System.Drawing.Color.White
        Me.Label20.Location = New System.Drawing.Point(7, 7)
        Me.Label20.Name = "Label20"
        Me.Label20.Size = New System.Drawing.Size(184, 21)
        Me.Label20.TabIndex = 10
        Me.Label20.Text = "Transaction Information"
        '
        'Label8
        '
        Me.Label8.AutoSize = True
        Me.Label8.Font = New System.Drawing.Font("Segoe UI", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label8.Location = New System.Drawing.Point(14, 134)
        Me.Label8.Name = "Label8"
        Me.Label8.Size = New System.Drawing.Size(77, 17)
        Me.Label8.TabIndex = 13
        Me.Label8.Text = "Grade Level"
        '
        'Label6
        '
        Me.Label6.AutoSize = True
        Me.Label6.Font = New System.Drawing.Font("Segoe UI", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label6.Location = New System.Drawing.Point(14, 92)
        Me.Label6.Name = "Label6"
        Me.Label6.Size = New System.Drawing.Size(91, 17)
        Me.Label6.TabIndex = 12
        Me.Label6.Text = "Student Name"
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Font = New System.Drawing.Font("Segoe UI", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label3.Location = New System.Drawing.Point(14, 53)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(77, 17)
        Me.Label3.TabIndex = 11
        Me.Label3.Text = "Student No."
        '
        'txtProgramStrand
        '
        Me.txtProgramStrand.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.txtProgramStrand.Font = New System.Drawing.Font("Segoe UI", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtProgramStrand.Location = New System.Drawing.Point(140, 173)
        Me.txtProgramStrand.Name = "txtProgramStrand"
        Me.txtProgramStrand.ReadOnly = True
        Me.txtProgramStrand.Size = New System.Drawing.Size(186, 25)
        Me.txtProgramStrand.TabIndex = 3
        '
        'txtStudentName
        '
        Me.txtStudentName.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.txtStudentName.Font = New System.Drawing.Font("Segoe UI", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtStudentName.Location = New System.Drawing.Point(140, 88)
        Me.txtStudentName.Name = "txtStudentName"
        Me.txtStudentName.ReadOnly = True
        Me.txtStudentName.Size = New System.Drawing.Size(186, 25)
        Me.txtStudentName.TabIndex = 1
        '
        'frmReturnExchange
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(747, 818)
        Me.Controls.Add(Me.btncancel)
        Me.Controls.Add(Me.btnreturnexc)
        Me.Controls.Add(Me.Label10)
        Me.Controls.Add(Me.Label11)
        Me.Controls.Add(Me.Panel7)
        Me.Controls.Add(Me.Panel3)
        Me.Controls.Add(Me.Panel1)
        Me.Controls.Add(Me.Panel4)
        Me.Name = "frmReturnExchange"
        Me.Text = "frmReturnExchange"
        Me.Panel7.ResumeLayout(False)
        CType(Me.dgvCart, System.ComponentModel.ISupportInitialize).EndInit()
        Me.Panel8.ResumeLayout(False)
        Me.Panel8.PerformLayout()
        Me.Panel3.ResumeLayout(False)
        Me.Panel3.PerformLayout()
        CType(Me.numupqty, System.ComponentModel.ISupportInitialize).EndInit()
        Me.Panel5.ResumeLayout(False)
        Me.Panel5.PerformLayout()
        Me.Panel1.ResumeLayout(False)
        Me.Panel1.PerformLayout()
        CType(Me.nudQuantity, System.ComponentModel.ISupportInitialize).EndInit()
        Me.Panel2.ResumeLayout(False)
        Me.Panel2.PerformLayout()
        Me.Panel4.ResumeLayout(False)
        Me.Panel4.PerformLayout()
        Me.Panel9.ResumeLayout(False)
        Me.Panel9.PerformLayout()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

    Friend WithEvents btncancel As Button
    Friend WithEvents btnreturnexc As Button
    Friend WithEvents Label10 As Label
    Friend WithEvents Label11 As Label
    Friend WithEvents Panel7 As Panel
    Friend WithEvents Panel8 As Panel
    Friend WithEvents Label15 As Label
    Friend WithEvents Panel3 As Panel
    Friend WithEvents txtReason As TextBox
    Friend WithEvents Panel5 As Panel
    Friend WithEvents Label4 As Label
    Friend WithEvents Panel1 As Panel
    Friend WithEvents Panel2 As Panel
    Friend WithEvents Label2 As Label
    Friend WithEvents Panel4 As Panel
    Friend WithEvents txtpaymentmethod As TextBox
    Friend WithEvents Label1 As Label
    Friend WithEvents txtstatus As TextBox
    Friend WithEvents dtpTransactionDate As DateTimePicker
    Friend WithEvents Label21 As Label
    Friend WithEvents txtorno As TextBox
    Friend WithEvents Label17 As Label
    Friend WithEvents Label24 As Label
    Friend WithEvents txtgrade As TextBox
    Friend WithEvents btnStudentno As TextBox
    Friend WithEvents Label7 As Label
    Friend WithEvents Panel9 As Panel
    Friend WithEvents Label20 As Label
    Friend WithEvents Label8 As Label
    Friend WithEvents Label6 As Label
    Friend WithEvents Label3 As Label
    Friend WithEvents txtProgramStrand As TextBox
    Friend WithEvents txtStudentName As TextBox
    Friend WithEvents txtcreatedby As TextBox
    Friend WithEvents Label12 As Label
    Friend WithEvents dgvCart As DataGridView
    Friend WithEvents ProductName As DataGridViewTextBoxColumn
    Friend WithEvents Category As DataGridViewTextBoxColumn
    Friend WithEvents Size As DataGridViewTextBoxColumn
    Friend WithEvents Quantity As DataGridViewTextBoxColumn
    Friend WithEvents UnitPrice As DataGridViewTextBoxColumn
    Friend WithEvents SubTotal As DataGridViewTextBoxColumn
    Friend WithEvents rbtnexchange As RadioButton
    Friend WithEvents rbtnReturn As RadioButton
    Friend WithEvents Label5 As Label
    Friend WithEvents reason As Label
    Friend WithEvents lblcondition As Label
    Friend WithEvents txtcondition As TextBox
    Friend WithEvents lblquantity As Label
    Friend WithEvents nudQuantity As NumericUpDown
    Friend WithEvents Label14 As Label
    Friend WithEvents txtAvailstock As ComboBox
    Friend WithEvents Label18 As Label
    Friend WithEvents numupqty As NumericUpDown
    Friend WithEvents Label9 As Label
    Friend WithEvents Label13 As Label
    Friend WithEvents Label16 As Label
    Friend WithEvents txtsize As ComboBox
    Friend WithEvents txtProduct As ComboBox
    Friend WithEvents Label19 As Label
    Friend WithEvents dtpORDate As DateTimePicker
End Class
