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
        Me.pnlreports = New System.Windows.Forms.Panel()
        Me.cboReportType = New System.Windows.Forms.ComboBox()
        Me.Label6 = New System.Windows.Forms.Label()
        Me.SuspendLayout()
        '
        'pnlreports
        '
        Me.pnlreports.Dock = System.Windows.Forms.DockStyle.Bottom
        Me.pnlreports.Location = New System.Drawing.Point(0, 42)
        Me.pnlreports.Name = "pnlreports"
        Me.pnlreports.Size = New System.Drawing.Size(1220, 808)
        Me.pnlreports.TabIndex = 12
        '
        'cboReportType
        '
        Me.cboReportType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cboReportType.Font = New System.Drawing.Font("Segoe UI Semibold", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cboReportType.FormattingEnabled = True
        Me.cboReportType.Items.AddRange(New Object() {"Sales By Item", "Sales by Date Range", "Cash Denomination", "Remittance Report", "Inventory Discrepancy"})
        Me.cboReportType.Location = New System.Drawing.Point(117, 7)
        Me.cboReportType.Name = "cboReportType"
        Me.cboReportType.Size = New System.Drawing.Size(197, 29)
        Me.cboReportType.TabIndex = 17
        '
        'Label6
        '
        Me.Label6.AutoSize = True
        Me.Label6.Font = New System.Drawing.Font("Segoe UI", 18.0!, System.Drawing.FontStyle.Bold)
        Me.Label6.ForeColor = System.Drawing.Color.FromArgb(CType(CType(1, Byte), Integer), CType(CType(21, Byte), Integer), CType(CType(78, Byte), Integer))
        Me.Label6.Location = New System.Drawing.Point(8, 5)
        Me.Label6.Name = "Label6"
        Me.Label6.Size = New System.Drawing.Size(103, 32)
        Me.Label6.TabIndex = 147
        Me.Label6.Text = "Reports"
        '
        'frmReports
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(1220, 850)
        Me.Controls.Add(Me.Label6)
        Me.Controls.Add(Me.cboReportType)
        Me.Controls.Add(Me.pnlreports)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None
        Me.Name = "frmReports"
        Me.Text = "frmReports"
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents pnlreports As Panel
    Friend WithEvents cboReportType As ComboBox
    Friend WithEvents Label6 As Label
End Class
