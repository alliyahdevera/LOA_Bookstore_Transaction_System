<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class frmReports
    Inherits System.Windows.Forms.Form

    'Form overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()>
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
    <System.Diagnostics.DebuggerStepThrough()>
    Private Sub InitializeComponent()
        Me.Panel1 = New System.Windows.Forms.Panel()
        Me.btnsalesrange = New System.Windows.Forms.Button()
        Me.btnsalesitem = New System.Windows.Forms.Button()
        Me.btnendofday = New System.Windows.Forms.Button()
        Me.btninvdiscrepancy = New System.Windows.Forms.Button()
        Me.btnremittance = New System.Windows.Forms.Button()
        Me.SuspendLayout()
        '
        'Panel1
        '
        Me.Panel1.Location = New System.Drawing.Point(0, 33)
        Me.Panel1.Name = "Panel1"
        Me.Panel1.Size = New System.Drawing.Size(1220, 817)
        Me.Panel1.TabIndex = 12
        '
        'btnsalesrange
        '
        Me.btnsalesrange.BackColor = System.Drawing.Color.FromArgb(CType(CType(1, Byte), Integer), CType(CType(21, Byte), Integer), CType(CType(78, Byte), Integer))
        Me.btnsalesrange.FlatAppearance.BorderSize = 0
        Me.btnsalesrange.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnsalesrange.ForeColor = System.Drawing.Color.White
        Me.btnsalesrange.Location = New System.Drawing.Point(199, 0)
        Me.btnsalesrange.Name = "btnsalesrange"
        Me.btnsalesrange.Size = New System.Drawing.Size(198, 33)
        Me.btnsalesrange.TabIndex = 8
        Me.btnsalesrange.Text = "Sales by Date Range"
        Me.btnsalesrange.UseVisualStyleBackColor = False
        '
        'btnsalesitem
        '
        Me.btnsalesitem.BackColor = System.Drawing.Color.FromArgb(CType(CType(1, Byte), Integer), CType(CType(21, Byte), Integer), CType(CType(78, Byte), Integer))
        Me.btnsalesitem.FlatAppearance.BorderSize = 0
        Me.btnsalesitem.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnsalesitem.ForeColor = System.Drawing.Color.White
        Me.btnsalesitem.Location = New System.Drawing.Point(0, 0)
        Me.btnsalesitem.Name = "btnsalesitem"
        Me.btnsalesitem.Size = New System.Drawing.Size(198, 33)
        Me.btnsalesitem.TabIndex = 13
        Me.btnsalesitem.Text = "Sales by Item"
        Me.btnsalesitem.UseVisualStyleBackColor = False
        '
        'btnendofday
        '
        Me.btnendofday.BackColor = System.Drawing.Color.FromArgb(CType(CType(1, Byte), Integer), CType(CType(21, Byte), Integer), CType(CType(78, Byte), Integer))
        Me.btnendofday.FlatAppearance.BorderSize = 0
        Me.btnendofday.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnendofday.ForeColor = System.Drawing.Color.White
        Me.btnendofday.Location = New System.Drawing.Point(398, 0)
        Me.btnendofday.Name = "btnendofday"
        Me.btnendofday.Size = New System.Drawing.Size(198, 33)
        Me.btnendofday.TabIndex = 14
        Me.btnendofday.Text = "Cash Denomination"
        Me.btnendofday.UseVisualStyleBackColor = False
        '
        'btninvdiscrepancy
        '
        Me.btninvdiscrepancy.BackColor = System.Drawing.Color.FromArgb(CType(CType(1, Byte), Integer), CType(CType(21, Byte), Integer), CType(CType(78, Byte), Integer))
        Me.btninvdiscrepancy.FlatAppearance.BorderSize = 0
        Me.btninvdiscrepancy.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btninvdiscrepancy.ForeColor = System.Drawing.Color.White
        Me.btninvdiscrepancy.Location = New System.Drawing.Point(796, 0)
        Me.btninvdiscrepancy.Name = "btninvdiscrepancy"
        Me.btninvdiscrepancy.Size = New System.Drawing.Size(198, 33)
        Me.btninvdiscrepancy.TabIndex = 16
        Me.btninvdiscrepancy.Text = "Inventory Discrepancy"
        Me.btninvdiscrepancy.UseVisualStyleBackColor = False
        '
        'btnremittance
        '
        Me.btnremittance.BackColor = System.Drawing.Color.FromArgb(CType(CType(1, Byte), Integer), CType(CType(21, Byte), Integer), CType(CType(78, Byte), Integer))
        Me.btnremittance.FlatAppearance.BorderSize = 0
        Me.btnremittance.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnremittance.ForeColor = System.Drawing.Color.White
        Me.btnremittance.Location = New System.Drawing.Point(597, 0)
        Me.btnremittance.Name = "btnremittance"
        Me.btnremittance.Size = New System.Drawing.Size(198, 33)
        Me.btnremittance.TabIndex = 15
        Me.btnremittance.Text = "Remittance Report"
        Me.btnremittance.UseVisualStyleBackColor = False
        '
        'frmReports
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(1220, 850)
        Me.Controls.Add(Me.btninvdiscrepancy)
        Me.Controls.Add(Me.btnremittance)
        Me.Controls.Add(Me.btnendofday)
        Me.Controls.Add(Me.btnsalesitem)
        Me.Controls.Add(Me.Panel1)
        Me.Controls.Add(Me.btnsalesrange)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None
        Me.Name = "frmReports"
        Me.Text = "frmReports"
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents Panel1 As Panel
    Friend WithEvents btnsalesrange As Button
    Friend WithEvents btnsalesitem As Button
    Friend WithEvents btnendofday As Button
    Friend WithEvents btninvdiscrepancy As Button
    Friend WithEvents btnremittance As Button
End Class
