<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmRemittance
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
        Dim DataGridViewCellStyle3 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Me.btngenerate = New System.Windows.Forms.Button()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.dtfrom = New System.Windows.Forms.DateTimePicker()
        Me.Label5 = New System.Windows.Forms.Label()
        Me.Label6 = New System.Windows.Forms.Label()
        Me.btnexportexcel = New System.Windows.Forms.Button()
        Me.Panel13 = New System.Windows.Forms.Panel()
        Me.lblname = New System.Windows.Forms.Label()
        Me.lbldatetime = New System.Windows.Forms.Label()
        Me.Label29 = New System.Windows.Forms.Label()
        Me.Label27 = New System.Windows.Forms.Label()
        Me.Label28 = New System.Windows.Forms.Label()
        Me.lblposition = New System.Windows.Forms.Label()
        Me.Panel5 = New System.Windows.Forms.Panel()
        Me.dgvsalesreport = New System.Windows.Forms.DataGridView()
        Me.Panel6 = New System.Windows.Forms.Panel()
        Me.Label7 = New System.Windows.Forms.Label()
        Me.ComboBox1 = New System.Windows.Forms.ComboBox()
        Me.Label11 = New System.Windows.Forms.Label()
        Me.cbocashier = New System.Windows.Forms.ComboBox()
        Me.Label14 = New System.Windows.Forms.Label()
        Me.RemittanceNo = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.nDate = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.CashierStaff = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.SalesPeriod = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.TotalSales = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.CashCollected = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.Difference = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.TotalRemittance = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.ORARRange = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.AmtAccounting = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.DateTimeRemitted = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.ReceivedBy = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.Remarks = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.Signature = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.dtto = New System.Windows.Forms.DateTimePicker()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.Panel13.SuspendLayout()
        Me.Panel5.SuspendLayout()
        CType(Me.dgvsalesreport, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.Panel6.SuspendLayout()
        Me.SuspendLayout()
        '
        'btngenerate
        '
        Me.btngenerate.BackColor = System.Drawing.Color.FromArgb(CType(CType(1, Byte), Integer), CType(CType(21, Byte), Integer), CType(CType(78, Byte), Integer))
        Me.btngenerate.FlatAppearance.BorderSize = 0
        Me.btngenerate.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btngenerate.ForeColor = System.Drawing.Color.White
        Me.btngenerate.Location = New System.Drawing.Point(1062, 96)
        Me.btngenerate.Name = "btngenerate"
        Me.btngenerate.Size = New System.Drawing.Size(130, 31)
        Me.btngenerate.TabIndex = 152
        Me.btngenerate.Text = "Generate"
        Me.btngenerate.UseVisualStyleBackColor = False
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Font = New System.Drawing.Font("Segoe UI", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label1.Location = New System.Drawing.Point(20, 102)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(67, 17)
        Me.Label1.TabIndex = 149
        Me.Label1.Text = "Date from"
        '
        'dtfrom
        '
        Me.dtfrom.Font = New System.Drawing.Font("Segoe UI", 11.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.dtfrom.Location = New System.Drawing.Point(88, 96)
        Me.dtfrom.Name = "dtfrom"
        Me.dtfrom.Size = New System.Drawing.Size(251, 27)
        Me.dtfrom.TabIndex = 148
        '
        'Label5
        '
        Me.Label5.AutoSize = True
        Me.Label5.Font = New System.Drawing.Font("Segoe UI", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label5.ForeColor = System.Drawing.SystemColors.ControlDarkDark
        Me.Label5.Location = New System.Drawing.Point(20, 61)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(244, 15)
        Me.Label5.TabIndex = 147
        Me.Label5.Text = "View and print the official remittance records"
        '
        'Label6
        '
        Me.Label6.AutoSize = True
        Me.Label6.Font = New System.Drawing.Font("Segoe UI", 18.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label6.ForeColor = System.Drawing.Color.FromArgb(CType(CType(1, Byte), Integer), CType(CType(21, Byte), Integer), CType(CType(78, Byte), Integer))
        Me.Label6.Location = New System.Drawing.Point(16, 24)
        Me.Label6.Name = "Label6"
        Me.Label6.Size = New System.Drawing.Size(227, 32)
        Me.Label6.TabIndex = 146
        Me.Label6.Text = "Remittance Report"
        '
        'btnexportexcel
        '
        Me.btnexportexcel.BackColor = System.Drawing.Color.FromArgb(CType(CType(1, Byte), Integer), CType(CType(21, Byte), Integer), CType(CType(78, Byte), Integer))
        Me.btnexportexcel.FlatAppearance.BorderSize = 0
        Me.btnexportexcel.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnexportexcel.ForeColor = System.Drawing.Color.White
        Me.btnexportexcel.Location = New System.Drawing.Point(1062, 732)
        Me.btnexportexcel.Name = "btnexportexcel"
        Me.btnexportexcel.Size = New System.Drawing.Size(130, 31)
        Me.btnexportexcel.TabIndex = 139
        Me.btnexportexcel.Text = "Print"
        Me.btnexportexcel.UseVisualStyleBackColor = False
        '
        'Panel13
        '
        Me.Panel13.BackColor = System.Drawing.Color.White
        Me.Panel13.Controls.Add(Me.lblname)
        Me.Panel13.Controls.Add(Me.lbldatetime)
        Me.Panel13.Controls.Add(Me.Label29)
        Me.Panel13.Controls.Add(Me.Label27)
        Me.Panel13.Controls.Add(Me.Label28)
        Me.Panel13.Controls.Add(Me.lblposition)
        Me.Panel13.Dock = System.Windows.Forms.DockStyle.Bottom
        Me.Panel13.Location = New System.Drawing.Point(0, 781)
        Me.Panel13.Name = "Panel13"
        Me.Panel13.Size = New System.Drawing.Size(1220, 27)
        Me.Panel13.TabIndex = 141
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
        'lbldatetime
        '
        Me.lbldatetime.AutoSize = True
        Me.lbldatetime.BackColor = System.Drawing.Color.Transparent
        Me.lbldatetime.Font = New System.Drawing.Font("Segoe UI Semibold", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lbldatetime.ForeColor = System.Drawing.Color.FromArgb(CType(CType(1, Byte), Integer), CType(CType(21, Byte), Integer), CType(CType(78, Byte), Integer))
        Me.lbldatetime.Location = New System.Drawing.Point(575, 3)
        Me.lbldatetime.Name = "lbldatetime"
        Me.lbldatetime.Size = New System.Drawing.Size(16, 21)
        Me.lbldatetime.TabIndex = 68
        Me.lbldatetime.Text = "-"
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
        'Label27
        '
        Me.Label27.AutoSize = True
        Me.Label27.BackColor = System.Drawing.Color.Transparent
        Me.Label27.Font = New System.Drawing.Font("Segoe UI", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label27.ForeColor = System.Drawing.Color.FromArgb(CType(CType(1, Byte), Integer), CType(CType(21, Byte), Integer), CType(CType(78, Byte), Integer))
        Me.Label27.Location = New System.Drawing.Point(504, 3)
        Me.Label27.Name = "Label27"
        Me.Label27.Size = New System.Drawing.Size(65, 21)
        Me.Label27.TabIndex = 67
        Me.Label27.Text = "Today is"
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
        'Panel5
        '
        Me.Panel5.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.Panel5.Controls.Add(Me.dgvsalesreport)
        Me.Panel5.Controls.Add(Me.Panel6)
        Me.Panel5.Location = New System.Drawing.Point(22, 141)
        Me.Panel5.Name = "Panel5"
        Me.Panel5.Size = New System.Drawing.Size(1170, 575)
        Me.Panel5.TabIndex = 140
        '
        'dgvsalesreport
        '
        Me.dgvsalesreport.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill
        Me.dgvsalesreport.AutoSizeRowsMode = System.Windows.Forms.DataGridViewAutoSizeRowsMode.AllCells
        Me.dgvsalesreport.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.dgvsalesreport.Columns.AddRange(New System.Windows.Forms.DataGridViewColumn() {Me.RemittanceNo, Me.nDate, Me.CashierStaff, Me.SalesPeriod, Me.TotalSales, Me.CashCollected, Me.Difference, Me.TotalRemittance, Me.ORARRange, Me.AmtAccounting, Me.DateTimeRemitted, Me.ReceivedBy, Me.Remarks, Me.Signature})
        DataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft
        DataGridViewCellStyle3.BackColor = System.Drawing.SystemColors.Window
        DataGridViewCellStyle3.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        DataGridViewCellStyle3.ForeColor = System.Drawing.SystemColors.ControlText
        DataGridViewCellStyle3.SelectionBackColor = System.Drawing.SystemColors.Highlight
        DataGridViewCellStyle3.SelectionForeColor = System.Drawing.SystemColors.HighlightText
        DataGridViewCellStyle3.WrapMode = System.Windows.Forms.DataGridViewTriState.[True]
        Me.dgvsalesreport.DefaultCellStyle = DataGridViewCellStyle3
        Me.dgvsalesreport.Location = New System.Drawing.Point(-1, 34)
        Me.dgvsalesreport.Name = "dgvsalesreport"
        Me.dgvsalesreport.ReadOnly = True
        Me.dgvsalesreport.Size = New System.Drawing.Size(1170, 539)
        Me.dgvsalesreport.TabIndex = 1
        '
        'Panel6
        '
        Me.Panel6.BackColor = System.Drawing.Color.FromArgb(CType(CType(1, Byte), Integer), CType(CType(21, Byte), Integer), CType(CType(78, Byte), Integer))
        Me.Panel6.Controls.Add(Me.Label7)
        Me.Panel6.Location = New System.Drawing.Point(0, 0)
        Me.Panel6.Name = "Panel6"
        Me.Panel6.Size = New System.Drawing.Size(1169, 35)
        Me.Panel6.TabIndex = 0
        '
        'Label7
        '
        Me.Label7.AutoSize = True
        Me.Label7.Font = New System.Drawing.Font("Segoe UI", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label7.ForeColor = System.Drawing.Color.White
        Me.Label7.Location = New System.Drawing.Point(7, 7)
        Me.Label7.Name = "Label7"
        Me.Label7.Size = New System.Drawing.Size(161, 21)
        Me.Label7.TabIndex = 25
        Me.Label7.Text = "Remittance Records"
        '
        'ComboBox1
        '
        Me.ComboBox1.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.ComboBox1.Font = New System.Drawing.Font("Segoe UI", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ComboBox1.FormattingEnabled = True
        Me.ComboBox1.Items.AddRange(New Object() {"Morning", "Afternoon", "Night"})
        Me.ComboBox1.Location = New System.Drawing.Point(909, 99)
        Me.ComboBox1.Name = "ComboBox1"
        Me.ComboBox1.Size = New System.Drawing.Size(120, 25)
        Me.ComboBox1.TabIndex = 148
        '
        'Label11
        '
        Me.Label11.AutoSize = True
        Me.Label11.Font = New System.Drawing.Font("Segoe UI Semibold", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label11.Location = New System.Drawing.Point(858, 102)
        Me.Label11.Name = "Label11"
        Me.Label11.Size = New System.Drawing.Size(46, 17)
        Me.Label11.TabIndex = 147
        Me.Label11.Text = "Status"
        '
        'cbocashier
        '
        Me.cbocashier.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.cbocashier.Font = New System.Drawing.Font("Segoe UI", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cbocashier.FormattingEnabled = True
        Me.cbocashier.Location = New System.Drawing.Point(702, 98)
        Me.cbocashier.Name = "cbocashier"
        Me.cbocashier.Size = New System.Drawing.Size(142, 25)
        Me.cbocashier.TabIndex = 146
        '
        'Label14
        '
        Me.Label14.AutoSize = True
        Me.Label14.Font = New System.Drawing.Font("Segoe UI Semibold", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label14.Location = New System.Drawing.Point(644, 102)
        Me.Label14.Name = "Label14"
        Me.Label14.Size = New System.Drawing.Size(52, 17)
        Me.Label14.TabIndex = 144
        Me.Label14.Text = "Cashier"
        '
        'RemittanceNo
        '
        Me.RemittanceNo.HeaderText = "Remittance No."
        Me.RemittanceNo.Name = "RemittanceNo"
        Me.RemittanceNo.ReadOnly = True
        '
        'nDate
        '
        Me.nDate.HeaderText = "Date"
        Me.nDate.Name = "nDate"
        Me.nDate.ReadOnly = True
        '
        'CashierStaff
        '
        Me.CashierStaff.HeaderText = "Cashier/Staff"
        Me.CashierStaff.Name = "CashierStaff"
        Me.CashierStaff.ReadOnly = True
        '
        'SalesPeriod
        '
        Me.SalesPeriod.HeaderText = "Sales Period"
        Me.SalesPeriod.Name = "SalesPeriod"
        Me.SalesPeriod.ReadOnly = True
        '
        'TotalSales
        '
        Me.TotalSales.HeaderText = "Total Sales"
        Me.TotalSales.Name = "TotalSales"
        Me.TotalSales.ReadOnly = True
        '
        'CashCollected
        '
        Me.CashCollected.HeaderText = "Cash Collected"
        Me.CashCollected.Name = "CashCollected"
        Me.CashCollected.ReadOnly = True
        '
        'Difference
        '
        Me.Difference.HeaderText = "GcashPayment"
        Me.Difference.Name = "Difference"
        Me.Difference.ReadOnly = True
        '
        'TotalRemittance
        '
        Me.TotalRemittance.HeaderText = "Total Remittance"
        Me.TotalRemittance.Name = "TotalRemittance"
        Me.TotalRemittance.ReadOnly = True
        '
        'ORARRange
        '
        Me.ORARRange.HeaderText = "OR/AR Range"
        Me.ORARRange.Name = "ORARRange"
        Me.ORARRange.ReadOnly = True
        '
        'AmtAccounting
        '
        Me.AmtAccounting.HeaderText = "Amount Remitted to Accounting"
        Me.AmtAccounting.Name = "AmtAccounting"
        Me.AmtAccounting.ReadOnly = True
        '
        'DateTimeRemitted
        '
        Me.DateTimeRemitted.HeaderText = "Date/Time Remitted"
        Me.DateTimeRemitted.Name = "DateTimeRemitted"
        Me.DateTimeRemitted.ReadOnly = True
        '
        'ReceivedBy
        '
        Me.ReceivedBy.HeaderText = "Received By"
        Me.ReceivedBy.Name = "ReceivedBy"
        Me.ReceivedBy.ReadOnly = True
        '
        'Remarks
        '
        Me.Remarks.HeaderText = "Remarks"
        Me.Remarks.Name = "Remarks"
        Me.Remarks.ReadOnly = True
        '
        'Signature
        '
        Me.Signature.HeaderText = "Signature"
        Me.Signature.Name = "Signature"
        Me.Signature.ReadOnly = True
        '
        'dtto
        '
        Me.dtto.Font = New System.Drawing.Font("Segoe UI", 11.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.dtto.Location = New System.Drawing.Point(377, 96)
        Me.dtto.Name = "dtto"
        Me.dtto.Size = New System.Drawing.Size(251, 27)
        Me.dtto.TabIndex = 150
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Font = New System.Drawing.Font("Segoe UI", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label2.Location = New System.Drawing.Point(348, 101)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(22, 17)
        Me.Label2.TabIndex = 151
        Me.Label2.Text = "To"
        '
        'frmRemittance
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(1220, 808)
        Me.Controls.Add(Me.ComboBox1)
        Me.Controls.Add(Me.btngenerate)
        Me.Controls.Add(Me.Label11)
        Me.Controls.Add(Me.Label2)
        Me.Controls.Add(Me.cbocashier)
        Me.Controls.Add(Me.dtto)
        Me.Controls.Add(Me.Label14)
        Me.Controls.Add(Me.Label1)
        Me.Controls.Add(Me.dtfrom)
        Me.Controls.Add(Me.Label5)
        Me.Controls.Add(Me.Label6)
        Me.Controls.Add(Me.btnexportexcel)
        Me.Controls.Add(Me.Panel13)
        Me.Controls.Add(Me.Panel5)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None
        Me.Name = "frmRemittance"
        Me.Text = "frmRemittance"
        Me.Panel13.ResumeLayout(False)
        Me.Panel13.PerformLayout()
        Me.Panel5.ResumeLayout(False)
        CType(Me.dgvsalesreport, System.ComponentModel.ISupportInitialize).EndInit()
        Me.Panel6.ResumeLayout(False)
        Me.Panel6.PerformLayout()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

    Friend WithEvents btngenerate As Button
    Friend WithEvents Label1 As Label
    Friend WithEvents dtfrom As DateTimePicker
    Friend WithEvents Label5 As Label
    Friend WithEvents Label6 As Label
    Friend WithEvents btnexportexcel As Button
    Friend WithEvents Panel13 As Panel
    Friend WithEvents lblname As Label
    Friend WithEvents lbldatetime As Label
    Friend WithEvents Label29 As Label
    Friend WithEvents Label27 As Label
    Friend WithEvents Label28 As Label
    Friend WithEvents lblposition As Label
    Friend WithEvents Panel5 As Panel
    Friend WithEvents dgvsalesreport As DataGridView
    Friend WithEvents Panel6 As Panel
    Friend WithEvents Label7 As Label
    Friend WithEvents ComboBox1 As ComboBox
    Friend WithEvents Label11 As Label
    Friend WithEvents cbocashier As ComboBox
    Friend WithEvents Label14 As Label
    Friend WithEvents RemittanceNo As DataGridViewTextBoxColumn
    Friend WithEvents nDate As DataGridViewTextBoxColumn
    Friend WithEvents CashierStaff As DataGridViewTextBoxColumn
    Friend WithEvents SalesPeriod As DataGridViewTextBoxColumn
    Friend WithEvents TotalSales As DataGridViewTextBoxColumn
    Friend WithEvents CashCollected As DataGridViewTextBoxColumn
    Friend WithEvents Difference As DataGridViewTextBoxColumn
    Friend WithEvents TotalRemittance As DataGridViewTextBoxColumn
    Friend WithEvents ORARRange As DataGridViewTextBoxColumn
    Friend WithEvents AmtAccounting As DataGridViewTextBoxColumn
    Friend WithEvents DateTimeRemitted As DataGridViewTextBoxColumn
    Friend WithEvents ReceivedBy As DataGridViewTextBoxColumn
    Friend WithEvents Remarks As DataGridViewTextBoxColumn
    Friend WithEvents Signature As DataGridViewTextBoxColumn
    Friend WithEvents dtto As DateTimePicker
    Friend WithEvents Label2 As Label
End Class
