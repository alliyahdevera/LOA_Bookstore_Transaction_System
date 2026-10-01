<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmInventoryDiscrepancies
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmInventoryDiscrepancies))
        Dim DataGridViewCellStyle6 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Me.btngenerate = New System.Windows.Forms.Button()
        Me.cbocategory = New System.Windows.Forms.ComboBox()
        Me.Label9 = New System.Windows.Forms.Label()
        Me.Label13 = New System.Windows.Forms.Label()
        Me.Label15 = New System.Windows.Forms.Label()
        Me.Panel1 = New System.Windows.Forms.Panel()
        Me.PictureBox1 = New System.Windows.Forms.PictureBox()
        Me.txtSearch = New System.Windows.Forms.TextBox()
        Me.Label14 = New System.Windows.Forms.Label()
        Me.Panel5 = New System.Windows.Forms.Panel()
        Me.dgvlistproducts = New System.Windows.Forms.DataGridView()
        Me.Panel6 = New System.Windows.Forms.Panel()
        Me.Label7 = New System.Windows.Forms.Label()
        Me.CountNo = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.CountDate = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.ProductName = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.Size = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.SystemQuantity = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.PhysicalQty = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.Difference = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.Adjusted = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.Status = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.Remarks = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.PreparedBy = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.Panel11 = New System.Windows.Forms.Panel()
        Me.lbloutofstocks = New System.Windows.Forms.Label()
        Me.Label8 = New System.Windows.Forms.Label()
        Me.Panel9 = New System.Windows.Forms.Panel()
        Me.lblcriticallvl = New System.Windows.Forms.Label()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.Panel7 = New System.Windows.Forms.Panel()
        Me.lblonhand = New System.Windows.Forms.Label()
        Me.Label6 = New System.Windows.Forms.Label()
        Me.Panel8 = New System.Windows.Forms.Panel()
        Me.lbltotalqproducts = New System.Windows.Forms.Label()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.Panel10 = New System.Windows.Forms.Panel()
        Me.lbltotalproducts = New System.Windows.Forms.Label()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.Panel1.SuspendLayout()
        CType(Me.PictureBox1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.Panel5.SuspendLayout()
        CType(Me.dgvlistproducts, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.Panel6.SuspendLayout()
        Me.Panel11.SuspendLayout()
        Me.Panel9.SuspendLayout()
        Me.Panel7.SuspendLayout()
        Me.Panel8.SuspendLayout()
        Me.Panel10.SuspendLayout()
        Me.SuspendLayout()
        '
        'btngenerate
        '
        Me.btngenerate.BackColor = System.Drawing.Color.FromArgb(CType(CType(1, Byte), Integer), CType(CType(21, Byte), Integer), CType(CType(78, Byte), Integer))
        Me.btngenerate.FlatAppearance.BorderSize = 0
        Me.btngenerate.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btngenerate.ForeColor = System.Drawing.Color.White
        Me.btngenerate.Location = New System.Drawing.Point(996, 190)
        Me.btngenerate.Name = "btngenerate"
        Me.btngenerate.Size = New System.Drawing.Size(197, 28)
        Me.btngenerate.TabIndex = 174
        Me.btngenerate.Text = "Load Products"
        Me.btngenerate.UseVisualStyleBackColor = False
        '
        'cbocategory
        '
        Me.cbocategory.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.cbocategory.Font = New System.Drawing.Font("Segoe UI", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cbocategory.FormattingEnabled = True
        Me.cbocategory.Location = New System.Drawing.Point(821, 192)
        Me.cbocategory.Name = "cbocategory"
        Me.cbocategory.Size = New System.Drawing.Size(158, 25)
        Me.cbocategory.TabIndex = 173
        '
        'Label9
        '
        Me.Label9.AutoSize = True
        Me.Label9.Font = New System.Drawing.Font("Segoe UI Semibold", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label9.Location = New System.Drawing.Point(751, 195)
        Me.Label9.Name = "Label9"
        Me.Label9.Size = New System.Drawing.Size(64, 17)
        Me.Label9.TabIndex = 172
        Me.Label9.Text = "Category"
        '
        'Label13
        '
        Me.Label13.AutoSize = True
        Me.Label13.Font = New System.Drawing.Font("Segoe UI", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label13.ForeColor = System.Drawing.SystemColors.ControlDarkDark
        Me.Label13.Location = New System.Drawing.Point(28, 50)
        Me.Label13.Name = "Label13"
        Me.Label13.Size = New System.Drawing.Size(249, 15)
        Me.Label13.TabIndex = 170
        Me.Label13.Text = "History of the Inventory Count & Reconciliation" & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10)
        '
        'Label15
        '
        Me.Label15.AutoSize = True
        Me.Label15.Font = New System.Drawing.Font("Segoe UI", 18.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label15.ForeColor = System.Drawing.Color.FromArgb(CType(CType(1, Byte), Integer), CType(CType(21, Byte), Integer), CType(CType(78, Byte), Integer))
        Me.Label15.Location = New System.Drawing.Point(24, 14)
        Me.Label15.Name = "Label15"
        Me.Label15.Size = New System.Drawing.Size(483, 32)
        Me.Label15.TabIndex = 169
        Me.Label15.Text = "Inventory Count && Reconciliation Report" & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10)
        '
        'Panel1
        '
        Me.Panel1.BackColor = System.Drawing.Color.White
        Me.Panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.Panel1.Controls.Add(Me.PictureBox1)
        Me.Panel1.Controls.Add(Me.txtSearch)
        Me.Panel1.Location = New System.Drawing.Point(246, 191)
        Me.Panel1.Name = "Panel1"
        Me.Panel1.Size = New System.Drawing.Size(216, 26)
        Me.Panel1.TabIndex = 168
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
        'Label14
        '
        Me.Label14.AutoSize = True
        Me.Label14.Font = New System.Drawing.Font("Segoe UI Semibold", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label14.Location = New System.Drawing.Point(30, 195)
        Me.Label14.Name = "Label14"
        Me.Label14.Size = New System.Drawing.Size(211, 17)
        Me.Label14.TabIndex = 167
        Me.Label14.Text = "Search by Product Code or Name"
        '
        'Panel5
        '
        Me.Panel5.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.Panel5.Controls.Add(Me.dgvlistproducts)
        Me.Panel5.Controls.Add(Me.Panel6)
        Me.Panel5.Location = New System.Drawing.Point(32, 234)
        Me.Panel5.Name = "Panel5"
        Me.Panel5.Size = New System.Drawing.Size(1161, 510)
        Me.Panel5.TabIndex = 166
        '
        'dgvlistproducts
        '
        Me.dgvlistproducts.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill
        Me.dgvlistproducts.AutoSizeRowsMode = System.Windows.Forms.DataGridViewAutoSizeRowsMode.AllCells
        Me.dgvlistproducts.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.dgvlistproducts.Columns.AddRange(New System.Windows.Forms.DataGridViewColumn() {Me.CountNo, Me.CountDate, Me.ProductName, Me.Size, Me.SystemQuantity, Me.PhysicalQty, Me.Difference, Me.Adjusted, Me.Status, Me.Remarks, Me.PreparedBy})
        DataGridViewCellStyle6.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft
        DataGridViewCellStyle6.BackColor = System.Drawing.SystemColors.Window
        DataGridViewCellStyle6.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        DataGridViewCellStyle6.ForeColor = System.Drawing.SystemColors.ControlText
        DataGridViewCellStyle6.SelectionBackColor = System.Drawing.SystemColors.Highlight
        DataGridViewCellStyle6.SelectionForeColor = System.Drawing.SystemColors.HighlightText
        DataGridViewCellStyle6.WrapMode = System.Windows.Forms.DataGridViewTriState.[True]
        Me.dgvlistproducts.DefaultCellStyle = DataGridViewCellStyle6
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
        'CountNo
        '
        Me.CountNo.HeaderText = "Count No."
        Me.CountNo.Name = "CountNo"
        Me.CountNo.ReadOnly = True
        '
        'CountDate
        '
        Me.CountDate.HeaderText = "Count Date"
        Me.CountDate.Name = "CountDate"
        Me.CountDate.ReadOnly = True
        '
        'ProductName
        '
        Me.ProductName.HeaderText = "Product Name"
        Me.ProductName.Name = "ProductName"
        Me.ProductName.ReadOnly = True
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
        'PhysicalQty
        '
        Me.PhysicalQty.HeaderText = "Physical Quantity"
        Me.PhysicalQty.Name = "PhysicalQty"
        Me.PhysicalQty.ReadOnly = True
        '
        'Difference
        '
        Me.Difference.HeaderText = "Difference"
        Me.Difference.Name = "Difference"
        Me.Difference.ReadOnly = True
        '
        'Adjusted
        '
        Me.Adjusted.HeaderText = "Adjusted"
        Me.Adjusted.Name = "Adjusted"
        Me.Adjusted.ReadOnly = True
        '
        'Status
        '
        Me.Status.HeaderText = "Status"
        Me.Status.Name = "Status"
        Me.Status.ReadOnly = True
        '
        'Remarks
        '
        Me.Remarks.HeaderText = "Remarks"
        Me.Remarks.Name = "Remarks"
        Me.Remarks.ReadOnly = True
        '
        'PreparedBy
        '
        Me.PreparedBy.HeaderText = "Prepared By"
        Me.PreparedBy.Name = "PreparedBy"
        Me.PreparedBy.ReadOnly = True
        '
        'Panel11
        '
        Me.Panel11.BackColor = System.Drawing.Color.FromArgb(CType(CType(255, Byte), Integer), CType(CType(192, Byte), Integer), CType(CType(192, Byte), Integer))
        Me.Panel11.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.Panel11.Controls.Add(Me.lbloutofstocks)
        Me.Panel11.Controls.Add(Me.Label8)
        Me.Panel11.Location = New System.Drawing.Point(996, 84)
        Me.Panel11.Name = "Panel11"
        Me.Panel11.Size = New System.Drawing.Size(197, 87)
        Me.Panel11.TabIndex = 176
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
        Me.Label8.Location = New System.Drawing.Point(45, 7)
        Me.Label8.Name = "Label8"
        Me.Label8.Size = New System.Drawing.Size(108, 15)
        Me.Label8.TabIndex = 3
        Me.Label8.Text = "Total Excess Items"
        '
        'Panel9
        '
        Me.Panel9.BackColor = System.Drawing.Color.FromArgb(CType(CType(192, Byte), Integer), CType(CType(192, Byte), Integer), CType(CType(255, Byte), Integer))
        Me.Panel9.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.Panel9.Controls.Add(Me.lblcriticallvl)
        Me.Panel9.Controls.Add(Me.Label3)
        Me.Panel9.Location = New System.Drawing.Point(756, 84)
        Me.Panel9.Name = "Panel9"
        Me.Panel9.Size = New System.Drawing.Size(197, 87)
        Me.Panel9.TabIndex = 175
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
        Me.Label3.Location = New System.Drawing.Point(40, 8)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(113, 15)
        Me.Label3.TabIndex = 2
        Me.Label3.Text = "Total Missing Items"
        '
        'Panel7
        '
        Me.Panel7.BackColor = System.Drawing.Color.FromArgb(CType(CType(255, Byte), Integer), CType(CType(255, Byte), Integer), CType(CType(192, Byte), Integer))
        Me.Panel7.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.Panel7.Controls.Add(Me.lblonhand)
        Me.Panel7.Controls.Add(Me.Label6)
        Me.Panel7.Location = New System.Drawing.Point(513, 84)
        Me.Panel7.Name = "Panel7"
        Me.Panel7.Size = New System.Drawing.Size(197, 87)
        Me.Panel7.TabIndex = 179
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
        Me.Panel8.BackColor = System.Drawing.Color.FromArgb(CType(CType(192, Byte), Integer), CType(CType(255, Byte), Integer), CType(CType(255, Byte), Integer))
        Me.Panel8.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.Panel8.Controls.Add(Me.lbltotalqproducts)
        Me.Panel8.Controls.Add(Me.Label2)
        Me.Panel8.Location = New System.Drawing.Point(273, 84)
        Me.Panel8.Name = "Panel8"
        Me.Panel8.Size = New System.Drawing.Size(197, 87)
        Me.Panel8.TabIndex = 178
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
        'Panel10
        '
        Me.Panel10.BackColor = System.Drawing.Color.FromArgb(CType(CType(192, Byte), Integer), CType(CType(255, Byte), Integer), CType(CType(192, Byte), Integer))
        Me.Panel10.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.Panel10.Controls.Add(Me.lbltotalproducts)
        Me.Panel10.Controls.Add(Me.Label1)
        Me.Panel10.Location = New System.Drawing.Point(32, 84)
        Me.Panel10.Name = "Panel10"
        Me.Panel10.Size = New System.Drawing.Size(197, 87)
        Me.Panel10.TabIndex = 177
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
        'frmInventoryDiscrepancies
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(1220, 808)
        Me.Controls.Add(Me.Panel7)
        Me.Controls.Add(Me.Panel8)
        Me.Controls.Add(Me.Panel10)
        Me.Controls.Add(Me.Panel11)
        Me.Controls.Add(Me.Panel9)
        Me.Controls.Add(Me.btngenerate)
        Me.Controls.Add(Me.cbocategory)
        Me.Controls.Add(Me.Label9)
        Me.Controls.Add(Me.Label13)
        Me.Controls.Add(Me.Label15)
        Me.Controls.Add(Me.Panel1)
        Me.Controls.Add(Me.Label14)
        Me.Controls.Add(Me.Panel5)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None
        Me.Name = "frmInventoryDiscrepancies"
        Me.Text = "frmInventoryDiscrepancies"
        Me.Panel1.ResumeLayout(False)
        Me.Panel1.PerformLayout()
        CType(Me.PictureBox1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.Panel5.ResumeLayout(False)
        CType(Me.dgvlistproducts, System.ComponentModel.ISupportInitialize).EndInit()
        Me.Panel6.ResumeLayout(False)
        Me.Panel6.PerformLayout()
        Me.Panel11.ResumeLayout(False)
        Me.Panel11.PerformLayout()
        Me.Panel9.ResumeLayout(False)
        Me.Panel9.PerformLayout()
        Me.Panel7.ResumeLayout(False)
        Me.Panel7.PerformLayout()
        Me.Panel8.ResumeLayout(False)
        Me.Panel8.PerformLayout()
        Me.Panel10.ResumeLayout(False)
        Me.Panel10.PerformLayout()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents btngenerate As Button
    Friend WithEvents cbocategory As ComboBox
    Friend WithEvents Label9 As Label
    Friend WithEvents Label13 As Label
    Friend WithEvents Label15 As Label
    Friend WithEvents Panel1 As Panel
    Friend WithEvents PictureBox1 As PictureBox
    Friend WithEvents txtSearch As TextBox
    Friend WithEvents Label14 As Label
    Friend WithEvents Panel5 As Panel
    Friend WithEvents dgvlistproducts As DataGridView
    Friend WithEvents Panel6 As Panel
    Friend WithEvents Label7 As Label
    Friend WithEvents CountNo As DataGridViewTextBoxColumn
    Friend WithEvents CountDate As DataGridViewTextBoxColumn
    Friend WithEvents ProductName As DataGridViewTextBoxColumn
    Friend WithEvents Size As DataGridViewTextBoxColumn
    Friend WithEvents SystemQuantity As DataGridViewTextBoxColumn
    Friend WithEvents PhysicalQty As DataGridViewTextBoxColumn
    Friend WithEvents Difference As DataGridViewTextBoxColumn
    Friend WithEvents Adjusted As DataGridViewTextBoxColumn
    Friend WithEvents Status As DataGridViewTextBoxColumn
    Friend WithEvents Remarks As DataGridViewTextBoxColumn
    Friend WithEvents PreparedBy As DataGridViewTextBoxColumn
    Friend WithEvents Panel11 As Panel
    Friend WithEvents lbloutofstocks As Label
    Friend WithEvents Label8 As Label
    Friend WithEvents Panel9 As Panel
    Friend WithEvents lblcriticallvl As Label
    Friend WithEvents Label3 As Label
    Friend WithEvents Panel7 As Panel
    Friend WithEvents lblonhand As Label
    Friend WithEvents Label6 As Label
    Friend WithEvents Panel8 As Panel
    Friend WithEvents lbltotalqproducts As Label
    Friend WithEvents Label2 As Label
    Friend WithEvents Panel10 As Panel
    Friend WithEvents lbltotalproducts As Label
    Friend WithEvents Label1 As Label
End Class
