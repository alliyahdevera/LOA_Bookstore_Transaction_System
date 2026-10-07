<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmInventoryCountReconciliation
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmInventoryCountReconciliation))
        Dim DataGridViewCellStyle1 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Me.Label13 = New System.Windows.Forms.Label()
        Me.Label15 = New System.Windows.Forms.Label()
        Me.Panel1 = New System.Windows.Forms.Panel()
        Me.PictureBox1 = New System.Windows.Forms.PictureBox()
        Me.txtSearch = New System.Windows.Forms.TextBox()
        Me.Panel13 = New System.Windows.Forms.Panel()
        Me.lblname = New System.Windows.Forms.Label()
        Me.Label29 = New System.Windows.Forms.Label()
        Me.Label28 = New System.Windows.Forms.Label()
        Me.lblposition = New System.Windows.Forms.Label()
        Me.Label14 = New System.Windows.Forms.Label()
        Me.Panel5 = New System.Windows.Forms.Panel()
        Me.dgvlistproducts = New System.Windows.Forms.DataGridView()
        Me.Panel6 = New System.Windows.Forms.Panel()
        Me.Label7 = New System.Windows.Forms.Label()
        Me.Label4 = New System.Windows.Forms.Label()
        Me.Label9 = New System.Windows.Forms.Label()
        Me.cbocategory = New System.Windows.Forms.ComboBox()
        Me.btngenerate = New System.Windows.Forms.Button()
        Me.btnreconcile = New System.Windows.Forms.Button()
        Me.btnclear = New System.Windows.Forms.Button()
        Me.btnsave = New System.Windows.Forms.Button()
        Me.btncancel = New System.Windows.Forms.Button()
        Me.txtGrandTotal = New System.Windows.Forms.TextBox()
        Me.Panel11 = New System.Windows.Forms.Panel()
        Me.lbloutofstocks = New System.Windows.Forms.Label()
        Me.Label8 = New System.Windows.Forms.Label()
        Me.Panel7 = New System.Windows.Forms.Panel()
        Me.lblonhand = New System.Windows.Forms.Label()
        Me.Label6 = New System.Windows.Forms.Label()
        Me.Panel8 = New System.Windows.Forms.Panel()
        Me.lbltotalqproducts = New System.Windows.Forms.Label()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.Panel9 = New System.Windows.Forms.Panel()
        Me.lblcriticallvl = New System.Windows.Forms.Label()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.Panel10 = New System.Windows.Forms.Panel()
        Me.lbltotalproducts = New System.Windows.Forms.Label()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.txtCountNo = New System.Windows.Forms.TextBox()
        Me.Label23 = New System.Windows.Forms.Label()
        Me.cboType = New System.Windows.Forms.ComboBox()
        Me.Label5 = New System.Windows.Forms.Label()
        Me.ProductCode = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.ProductName = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.Category = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.TypeofProduct = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.Size = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.SystemQuantity = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.PhysicalQuantity = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.Difference = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.Status = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.Condition = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.Remarks = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.Panel1.SuspendLayout()
        CType(Me.PictureBox1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.Panel13.SuspendLayout()
        Me.Panel5.SuspendLayout()
        CType(Me.dgvlistproducts, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.Panel6.SuspendLayout()
        Me.Panel11.SuspendLayout()
        Me.Panel7.SuspendLayout()
        Me.Panel8.SuspendLayout()
        Me.Panel9.SuspendLayout()
        Me.Panel10.SuspendLayout()
        Me.SuspendLayout()
        '
        'Label13
        '
        Me.Label13.AutoSize = True
        Me.Label13.Font = New System.Drawing.Font("Segoe UI", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label13.ForeColor = System.Drawing.SystemColors.ControlDarkDark
        Me.Label13.Location = New System.Drawing.Point(27, 38)
        Me.Label13.Name = "Label13"
        Me.Label13.Size = New System.Drawing.Size(412, 15)
        Me.Label13.TabIndex = 110
        Me.Label13.Text = "Compare physical counts with recorded quantities and identify discrepancies"
        '
        'Label15
        '
        Me.Label15.AutoSize = True
        Me.Label15.Font = New System.Drawing.Font("Segoe UI", 18.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label15.ForeColor = System.Drawing.Color.FromArgb(CType(CType(1, Byte), Integer), CType(CType(21, Byte), Integer), CType(CType(78, Byte), Integer))
        Me.Label15.Location = New System.Drawing.Point(23, 6)
        Me.Label15.Name = "Label15"
        Me.Label15.Size = New System.Drawing.Size(398, 32)
        Me.Label15.TabIndex = 109
        Me.Label15.Text = "Inventory Count && Reconciliation"
        '
        'Panel1
        '
        Me.Panel1.BackColor = System.Drawing.Color.White
        Me.Panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.Panel1.Controls.Add(Me.PictureBox1)
        Me.Panel1.Controls.Add(Me.txtSearch)
        Me.Panel1.Location = New System.Drawing.Point(355, 181)
        Me.Panel1.Name = "Panel1"
        Me.Panel1.Size = New System.Drawing.Size(216, 26)
        Me.Panel1.TabIndex = 108
        '
        'PictureBox1
        '
        Me.PictureBox1.BackgroundImage = CType(resources.GetObject("PictureBox1.BackgroundImage"), System.Drawing.Image)
        Me.PictureBox1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch
        Me.PictureBox1.Location = New System.Drawing.Point(192, 3)
        Me.PictureBox1.Name = "PictureBox1"
        Me.PictureBox1.Size = New System.Drawing.Size(19, 19)
        Me.PictureBox1.TabIndex = 29
        Me.PictureBox1.TabStop = False
        '
        'txtSearch
        '
        Me.txtSearch.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.txtSearch.Font = New System.Drawing.Font("Segoe UI", 11.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtSearch.Location = New System.Drawing.Point(3, 2)
        Me.txtSearch.Name = "txtSearch"
        Me.txtSearch.Size = New System.Drawing.Size(186, 20)
        Me.txtSearch.TabIndex = 28
        '
        'Panel13
        '
        Me.Panel13.BackColor = System.Drawing.Color.White
        Me.Panel13.Controls.Add(Me.lblname)
        Me.Panel13.Controls.Add(Me.Label29)
        Me.Panel13.Controls.Add(Me.Label28)
        Me.Panel13.Controls.Add(Me.lblposition)
        Me.Panel13.Dock = System.Windows.Forms.DockStyle.Bottom
        Me.Panel13.Location = New System.Drawing.Point(0, 777)
        Me.Panel13.Name = "Panel13"
        Me.Panel13.Size = New System.Drawing.Size(1220, 27)
        Me.Panel13.TabIndex = 107
        '
        'lblname
        '
        Me.lblname.AutoSize = True
        Me.lblname.BackColor = System.Drawing.Color.Transparent
        Me.lblname.Font = New System.Drawing.Font("Segoe UI Semibold", 12.0!, CType((System.Drawing.FontStyle.Bold Or System.Drawing.FontStyle.Italic), System.Drawing.FontStyle), System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblname.ForeColor = System.Drawing.Color.FromArgb(CType(CType(1, Byte), Integer), CType(CType(21, Byte), Integer), CType(CType(78, Byte), Integer))
        Me.lblname.Location = New System.Drawing.Point(56, 3)
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
        'Label14
        '
        Me.Label14.AutoSize = True
        Me.Label14.Font = New System.Drawing.Font("Segoe UI Semibold", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label14.Location = New System.Drawing.Point(301, 185)
        Me.Label14.Name = "Label14"
        Me.Label14.Size = New System.Drawing.Size(48, 17)
        Me.Label14.TabIndex = 106
        Me.Label14.Text = "Search"
        '
        'Panel5
        '
        Me.Panel5.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.Panel5.Controls.Add(Me.dgvlistproducts)
        Me.Panel5.Controls.Add(Me.Panel6)
        Me.Panel5.Location = New System.Drawing.Point(28, 221)
        Me.Panel5.Name = "Panel5"
        Me.Panel5.Size = New System.Drawing.Size(1161, 474)
        Me.Panel5.TabIndex = 100
        '
        'dgvlistproducts
        '
        Me.dgvlistproducts.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill
        Me.dgvlistproducts.AutoSizeRowsMode = System.Windows.Forms.DataGridViewAutoSizeRowsMode.AllCells
        Me.dgvlistproducts.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.dgvlistproducts.Columns.AddRange(New System.Windows.Forms.DataGridViewColumn() {Me.ProductCode, Me.ProductName, Me.Category, Me.TypeofProduct, Me.Size, Me.SystemQuantity, Me.PhysicalQuantity, Me.Difference, Me.Status, Me.Condition, Me.Remarks})
        DataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft
        DataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Window
        DataGridViewCellStyle1.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        DataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.ControlText
        DataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight
        DataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText
        DataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.[True]
        Me.dgvlistproducts.DefaultCellStyle = DataGridViewCellStyle1
        Me.dgvlistproducts.Location = New System.Drawing.Point(-1, 35)
        Me.dgvlistproducts.Name = "dgvlistproducts"
        Me.dgvlistproducts.ReadOnly = True
        Me.dgvlistproducts.Size = New System.Drawing.Size(1161, 538)
        Me.dgvlistproducts.TabIndex = 1
        '
        'Panel6
        '
        Me.Panel6.BackColor = System.Drawing.Color.FromArgb(CType(CType(1, Byte), Integer), CType(CType(21, Byte), Integer), CType(CType(78, Byte), Integer))
        Me.Panel6.Controls.Add(Me.Label7)
        Me.Panel6.Location = New System.Drawing.Point(0, 0)
        Me.Panel6.Name = "Panel6"
        Me.Panel6.Size = New System.Drawing.Size(1166, 35)
        Me.Panel6.TabIndex = 0
        '
        'Label7
        '
        Me.Label7.AutoSize = True
        Me.Label7.Font = New System.Drawing.Font("Segoe UI", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label7.ForeColor = System.Drawing.Color.White
        Me.Label7.Location = New System.Drawing.Point(7, 7)
        Me.Label7.Name = "Label7"
        Me.Label7.Size = New System.Drawing.Size(165, 21)
        Me.Label7.TabIndex = 25
        Me.Label7.Text = "Inventory Count List"
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.Font = New System.Drawing.Font("Segoe UI Semibold", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label4.Location = New System.Drawing.Point(25, 185)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(77, 17)
        Me.Label4.TabIndex = 111
        Me.Label4.Text = "Count Date"
        '
        'Label9
        '
        Me.Label9.AutoSize = True
        Me.Label9.Font = New System.Drawing.Font("Segoe UI Semibold", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label9.Location = New System.Drawing.Point(583, 185)
        Me.Label9.Name = "Label9"
        Me.Label9.Size = New System.Drawing.Size(64, 17)
        Me.Label9.TabIndex = 123
        Me.Label9.Text = "Category"
        '
        'cbocategory
        '
        Me.cbocategory.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.cbocategory.Font = New System.Drawing.Font("Segoe UI", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cbocategory.FormattingEnabled = True
        Me.cbocategory.Location = New System.Drawing.Point(653, 182)
        Me.cbocategory.Name = "cbocategory"
        Me.cbocategory.Size = New System.Drawing.Size(158, 25)
        Me.cbocategory.TabIndex = 124
        '
        'btngenerate
        '
        Me.btngenerate.BackColor = System.Drawing.Color.FromArgb(CType(CType(1, Byte), Integer), CType(CType(21, Byte), Integer), CType(CType(78, Byte), Integer))
        Me.btngenerate.FlatAppearance.BorderSize = 0
        Me.btngenerate.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btngenerate.ForeColor = System.Drawing.Color.White
        Me.btngenerate.Location = New System.Drawing.Point(1072, 182)
        Me.btngenerate.Name = "btngenerate"
        Me.btngenerate.Size = New System.Drawing.Size(117, 28)
        Me.btngenerate.TabIndex = 164
        Me.btngenerate.Text = "Load Products"
        Me.btngenerate.UseVisualStyleBackColor = False
        '
        'btnreconcile
        '
        Me.btnreconcile.BackColor = System.Drawing.Color.FromArgb(CType(CType(1, Byte), Integer), CType(CType(21, Byte), Integer), CType(CType(78, Byte), Integer))
        Me.btnreconcile.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnreconcile.ForeColor = System.Drawing.Color.White
        Me.btnreconcile.Location = New System.Drawing.Point(662, 713)
        Me.btnreconcile.Name = "btnreconcile"
        Me.btnreconcile.Size = New System.Drawing.Size(170, 40)
        Me.btnreconcile.TabIndex = 166
        Me.btnreconcile.Text = "Adjust Inventory"
        Me.btnreconcile.UseVisualStyleBackColor = False
        '
        'btnclear
        '
        Me.btnclear.BackColor = System.Drawing.Color.LightSlateGray
        Me.btnclear.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnclear.ForeColor = System.Drawing.Color.White
        Me.btnclear.Location = New System.Drawing.Point(840, 713)
        Me.btnclear.Name = "btnclear"
        Me.btnclear.Size = New System.Drawing.Size(170, 40)
        Me.btnclear.TabIndex = 167
        Me.btnclear.Text = "Clear"
        Me.btnclear.UseVisualStyleBackColor = False
        '
        'btnsave
        '
        Me.btnsave.BackColor = System.Drawing.Color.Green
        Me.btnsave.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnsave.ForeColor = System.Drawing.Color.White
        Me.btnsave.Location = New System.Drawing.Point(483, 713)
        Me.btnsave.Name = "btnsave"
        Me.btnsave.Size = New System.Drawing.Size(170, 40)
        Me.btnsave.TabIndex = 168
        Me.btnsave.Text = "Save Count"
        Me.btnsave.UseVisualStyleBackColor = False
        '
        'btncancel
        '
        Me.btncancel.BackColor = System.Drawing.Color.IndianRed
        Me.btncancel.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btncancel.ForeColor = System.Drawing.Color.White
        Me.btncancel.Location = New System.Drawing.Point(1019, 713)
        Me.btncancel.Name = "btncancel"
        Me.btncancel.Size = New System.Drawing.Size(170, 40)
        Me.btncancel.TabIndex = 169
        Me.btncancel.Text = "Cancel"
        Me.btncancel.UseVisualStyleBackColor = False
        '
        'txtGrandTotal
        '
        Me.txtGrandTotal.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.txtGrandTotal.Font = New System.Drawing.Font("Segoe UI", 11.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtGrandTotal.Location = New System.Drawing.Point(108, 180)
        Me.txtGrandTotal.Name = "txtGrandTotal"
        Me.txtGrandTotal.ReadOnly = True
        Me.txtGrandTotal.Size = New System.Drawing.Size(177, 27)
        Me.txtGrandTotal.TabIndex = 67
        '
        'Panel11
        '
        Me.Panel11.BackColor = System.Drawing.Color.White
        Me.Panel11.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.Panel11.Controls.Add(Me.lbloutofstocks)
        Me.Panel11.Controls.Add(Me.Label8)
        Me.Panel11.Location = New System.Drawing.Point(992, 71)
        Me.Panel11.Name = "Panel11"
        Me.Panel11.Size = New System.Drawing.Size(197, 87)
        Me.Panel11.TabIndex = 174
        '
        'lbloutofstocks
        '
        Me.lbloutofstocks.AutoSize = True
        Me.lbloutofstocks.Font = New System.Drawing.Font("Segoe UI", 24.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lbloutofstocks.Location = New System.Drawing.Point(71, 24)
        Me.lbloutofstocks.Name = "lbloutofstocks"
        Me.lbloutofstocks.Size = New System.Drawing.Size(56, 45)
        Me.lbloutofstocks.TabIndex = 91
        Me.lbloutofstocks.Text = "00"
        Me.lbloutofstocks.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'Label8
        '
        Me.Label8.AutoSize = True
        Me.Label8.Font = New System.Drawing.Font("Segoe UI", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label8.Location = New System.Drawing.Point(76, 5)
        Me.Label8.Name = "Label8"
        Me.Label8.Size = New System.Drawing.Size(43, 15)
        Me.Label8.TabIndex = 3
        Me.Label8.Text = "Excess"
        '
        'Panel7
        '
        Me.Panel7.BackColor = System.Drawing.Color.White
        Me.Panel7.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.Panel7.Controls.Add(Me.lblonhand)
        Me.Panel7.Controls.Add(Me.Label6)
        Me.Panel7.Location = New System.Drawing.Point(511, 71)
        Me.Panel7.Name = "Panel7"
        Me.Panel7.Size = New System.Drawing.Size(197, 87)
        Me.Panel7.TabIndex = 173
        '
        'lblonhand
        '
        Me.lblonhand.AutoSize = True
        Me.lblonhand.Font = New System.Drawing.Font("Segoe UI", 24.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblonhand.Location = New System.Drawing.Point(71, 24)
        Me.lblonhand.Name = "lblonhand"
        Me.lblonhand.Size = New System.Drawing.Size(56, 45)
        Me.lblonhand.TabIndex = 89
        Me.lblonhand.Text = "00"
        Me.lblonhand.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'Label6
        '
        Me.Label6.AutoSize = True
        Me.Label6.Font = New System.Drawing.Font("Segoe UI", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label6.Location = New System.Drawing.Point(43, 7)
        Me.Label6.Name = "Label6"
        Me.Label6.Size = New System.Drawing.Size(113, 15)
        Me.Label6.TabIndex = 3
        Me.Label6.Text = "With Discrepancies"
        '
        'Panel8
        '
        Me.Panel8.BackColor = System.Drawing.Color.White
        Me.Panel8.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.Panel8.Controls.Add(Me.lbltotalqproducts)
        Me.Panel8.Controls.Add(Me.Label2)
        Me.Panel8.Location = New System.Drawing.Point(271, 71)
        Me.Panel8.Name = "Panel8"
        Me.Panel8.Size = New System.Drawing.Size(197, 87)
        Me.Panel8.TabIndex = 171
        '
        'lbltotalqproducts
        '
        Me.lbltotalqproducts.AutoSize = True
        Me.lbltotalqproducts.Font = New System.Drawing.Font("Segoe UI", 24.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lbltotalqproducts.Location = New System.Drawing.Point(68, 25)
        Me.lbltotalqproducts.Name = "lbltotalqproducts"
        Me.lbltotalqproducts.Size = New System.Drawing.Size(56, 45)
        Me.lbltotalqproducts.TabIndex = 87
        Me.lbltotalqproducts.Text = "00"
        Me.lbltotalqproducts.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Font = New System.Drawing.Font("Segoe UI", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label2.Location = New System.Drawing.Point(48, 7)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(91, 15)
        Me.Label2.TabIndex = 1
        Me.Label2.Text = "Matched Items"
        '
        'Panel9
        '
        Me.Panel9.BackColor = System.Drawing.Color.White
        Me.Panel9.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.Panel9.Controls.Add(Me.lblcriticallvl)
        Me.Panel9.Controls.Add(Me.Label3)
        Me.Panel9.Location = New System.Drawing.Point(752, 71)
        Me.Panel9.Name = "Panel9"
        Me.Panel9.Size = New System.Drawing.Size(197, 87)
        Me.Panel9.TabIndex = 172
        '
        'lblcriticallvl
        '
        Me.lblcriticallvl.AutoSize = True
        Me.lblcriticallvl.Font = New System.Drawing.Font("Segoe UI", 24.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblcriticallvl.Location = New System.Drawing.Point(68, 24)
        Me.lblcriticallvl.Name = "lblcriticallvl"
        Me.lblcriticallvl.Size = New System.Drawing.Size(56, 45)
        Me.lblcriticallvl.TabIndex = 90
        Me.lblcriticallvl.Text = "00"
        Me.lblcriticallvl.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Font = New System.Drawing.Font("Segoe UI", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label3.Location = New System.Drawing.Point(52, 8)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(90, 15)
        Me.Label3.TabIndex = 2
        Me.Label3.Text = "Short / Missing"
        '
        'Panel10
        '
        Me.Panel10.BackColor = System.Drawing.Color.White
        Me.Panel10.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.Panel10.Controls.Add(Me.lbltotalproducts)
        Me.Panel10.Controls.Add(Me.Label1)
        Me.Panel10.Location = New System.Drawing.Point(30, 71)
        Me.Panel10.Name = "Panel10"
        Me.Panel10.Size = New System.Drawing.Size(197, 87)
        Me.Panel10.TabIndex = 170
        '
        'lbltotalproducts
        '
        Me.lbltotalproducts.AutoSize = True
        Me.lbltotalproducts.Font = New System.Drawing.Font("Segoe UI", 24.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lbltotalproducts.Location = New System.Drawing.Point(68, 24)
        Me.lbltotalproducts.Name = "lbltotalproducts"
        Me.lbltotalproducts.Size = New System.Drawing.Size(56, 45)
        Me.lbltotalproducts.TabIndex = 86
        Me.lbltotalproducts.Text = "00"
        Me.lbltotalproducts.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Font = New System.Drawing.Font("Segoe UI", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label1.Location = New System.Drawing.Point(37, 8)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(119, 15)
        Me.Label1.TabIndex = 0
        Me.Label1.Text = "Total Items Counted"
        '
        'txtCountNo
        '
        Me.txtCountNo.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.txtCountNo.Font = New System.Drawing.Font("Segoe UI", 11.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtCountNo.Location = New System.Drawing.Point(948, 26)
        Me.txtCountNo.Name = "txtCountNo"
        Me.txtCountNo.ReadOnly = True
        Me.txtCountNo.Size = New System.Drawing.Size(241, 27)
        Me.txtCountNo.TabIndex = 175
        '
        'Label23
        '
        Me.Label23.AutoSize = True
        Me.Label23.Font = New System.Drawing.Font("Segoe UI Semibold", 11.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label23.Location = New System.Drawing.Point(853, 28)
        Me.Label23.Name = "Label23"
        Me.Label23.Size = New System.Drawing.Size(79, 20)
        Me.Label23.TabIndex = 176
        Me.Label23.Text = "Count No."
        '
        'cboType
        '
        Me.cboType.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.cboType.Font = New System.Drawing.Font("Segoe UI", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cboType.FormattingEnabled = True
        Me.cboType.Location = New System.Drawing.Point(879, 183)
        Me.cboType.Name = "cboType"
        Me.cboType.Size = New System.Drawing.Size(178, 25)
        Me.cboType.TabIndex = 178
        '
        'Label5
        '
        Me.Label5.AutoSize = True
        Me.Label5.Font = New System.Drawing.Font("Segoe UI Semibold", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label5.Location = New System.Drawing.Point(837, 187)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(36, 17)
        Me.Label5.TabIndex = 177
        Me.Label5.Text = "Type"
        '
        'ProductCode
        '
        Me.ProductCode.HeaderText = "Product Code"
        Me.ProductCode.Name = "ProductCode"
        Me.ProductCode.ReadOnly = True
        '
        'ProductName
        '
        Me.ProductName.HeaderText = "Product Name"
        Me.ProductName.Name = "ProductName"
        Me.ProductName.ReadOnly = True
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
        'Size
        '
        Me.Size.HeaderText = "Size"
        Me.Size.Name = "Size"
        Me.Size.ReadOnly = True
        '
        'SystemQuantity
        '
        Me.SystemQuantity.HeaderText = "System Quantity"
        Me.SystemQuantity.Name = "SystemQuantity"
        Me.SystemQuantity.ReadOnly = True
        '
        'PhysicalQuantity
        '
        Me.PhysicalQuantity.HeaderText = "Physical Quantity"
        Me.PhysicalQuantity.Name = "PhysicalQuantity"
        Me.PhysicalQuantity.ReadOnly = True
        '
        'Difference
        '
        Me.Difference.HeaderText = "Difference"
        Me.Difference.Name = "Difference"
        Me.Difference.ReadOnly = True
        '
        'Status
        '
        Me.Status.HeaderText = "Status"
        Me.Status.Name = "Status"
        Me.Status.ReadOnly = True
        '
        'Condition
        '
        Me.Condition.HeaderText = "Condition"
        Me.Condition.Name = "Condition"
        Me.Condition.ReadOnly = True
        '
        'Remarks
        '
        Me.Remarks.HeaderText = "Remarks"
        Me.Remarks.Name = "Remarks"
        Me.Remarks.ReadOnly = True
        '
        'frmInventoryCountReconciliation
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(1220, 804)
        Me.Controls.Add(Me.cboType)
        Me.Controls.Add(Me.Label5)
        Me.Controls.Add(Me.txtCountNo)
        Me.Controls.Add(Me.Label23)
        Me.Controls.Add(Me.Panel11)
        Me.Controls.Add(Me.Panel7)
        Me.Controls.Add(Me.Panel8)
        Me.Controls.Add(Me.Panel9)
        Me.Controls.Add(Me.Panel10)
        Me.Controls.Add(Me.txtGrandTotal)
        Me.Controls.Add(Me.btncancel)
        Me.Controls.Add(Me.btnsave)
        Me.Controls.Add(Me.btnclear)
        Me.Controls.Add(Me.btnreconcile)
        Me.Controls.Add(Me.btngenerate)
        Me.Controls.Add(Me.cbocategory)
        Me.Controls.Add(Me.Label9)
        Me.Controls.Add(Me.Label4)
        Me.Controls.Add(Me.Label13)
        Me.Controls.Add(Me.Label15)
        Me.Controls.Add(Me.Panel1)
        Me.Controls.Add(Me.Panel13)
        Me.Controls.Add(Me.Label14)
        Me.Controls.Add(Me.Panel5)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None
        Me.Name = "frmInventoryCountReconciliation"
        Me.Text = "frmInventoryCountReconciliation"
        Me.Panel1.ResumeLayout(False)
        Me.Panel1.PerformLayout()
        CType(Me.PictureBox1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.Panel13.ResumeLayout(False)
        Me.Panel13.PerformLayout()
        Me.Panel5.ResumeLayout(False)
        CType(Me.dgvlistproducts, System.ComponentModel.ISupportInitialize).EndInit()
        Me.Panel6.ResumeLayout(False)
        Me.Panel6.PerformLayout()
        Me.Panel11.ResumeLayout(False)
        Me.Panel11.PerformLayout()
        Me.Panel7.ResumeLayout(False)
        Me.Panel7.PerformLayout()
        Me.Panel8.ResumeLayout(False)
        Me.Panel8.PerformLayout()
        Me.Panel9.ResumeLayout(False)
        Me.Panel9.PerformLayout()
        Me.Panel10.ResumeLayout(False)
        Me.Panel10.PerformLayout()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

    Friend WithEvents Label13 As Label
    Friend WithEvents Label15 As Label
    Friend WithEvents Panel1 As Panel
    Friend WithEvents PictureBox1 As PictureBox
    Friend WithEvents txtSearch As TextBox
    Friend WithEvents Panel13 As Panel
    Friend WithEvents lblname As Label
    Friend WithEvents Label29 As Label
    Friend WithEvents Label28 As Label
    Friend WithEvents lblposition As Label
    Friend WithEvents Label14 As Label
    Friend WithEvents Panel5 As Panel
    Friend WithEvents dgvlistproducts As DataGridView
    Friend WithEvents Panel6 As Panel
    Friend WithEvents Label7 As Label
    Friend WithEvents Label4 As Label
    Friend WithEvents Label9 As Label
    Friend WithEvents cbocategory As ComboBox
    Friend WithEvents btngenerate As Button
    Friend WithEvents btnreconcile As Button
    Friend WithEvents btnclear As Button
    Friend WithEvents btnsave As Button
    Friend WithEvents btncancel As Button
    Friend WithEvents txtGrandTotal As TextBox
    Friend WithEvents Panel11 As Panel
    Friend WithEvents lbloutofstocks As Label
    Friend WithEvents Label8 As Label
    Friend WithEvents Panel7 As Panel
    Friend WithEvents lblonhand As Label
    Friend WithEvents Label6 As Label
    Friend WithEvents Panel8 As Panel
    Friend WithEvents lbltotalqproducts As Label
    Friend WithEvents Label2 As Label
    Friend WithEvents Panel9 As Panel
    Friend WithEvents lblcriticallvl As Label
    Friend WithEvents Label3 As Label
    Friend WithEvents Panel10 As Panel
    Friend WithEvents lbltotalproducts As Label
    Friend WithEvents Label1 As Label
    Friend WithEvents txtCountNo As TextBox
    Friend WithEvents Label23 As Label
    Friend WithEvents cboType As ComboBox
    Friend WithEvents Label5 As Label
    Friend WithEvents ProductCode As DataGridViewTextBoxColumn
    Friend WithEvents ProductName As DataGridViewTextBoxColumn
    Friend WithEvents Category As DataGridViewTextBoxColumn
    Friend WithEvents TypeofProduct As DataGridViewTextBoxColumn
    Friend WithEvents Size As DataGridViewTextBoxColumn
    Friend WithEvents SystemQuantity As DataGridViewTextBoxColumn
    Friend WithEvents PhysicalQuantity As DataGridViewTextBoxColumn
    Friend WithEvents Difference As DataGridViewTextBoxColumn
    Friend WithEvents Status As DataGridViewTextBoxColumn
    Friend WithEvents Condition As DataGridViewTextBoxColumn
    Friend WithEvents Remarks As DataGridViewTextBoxColumn
End Class
