<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmAuditLogs
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
        Me.btnLoginHistory = New System.Windows.Forms.Button()
        Me.btnActivityHistory = New System.Windows.Forms.Button()
        Me.Panel1 = New System.Windows.Forms.Panel()
        Me.btnPriceChangeHistory = New System.Windows.Forms.Button()
        Me.SuspendLayout()
        '
        'btnLoginHistory
        '
        Me.btnLoginHistory.BackColor = System.Drawing.Color.FromArgb(CType(CType(1, Byte), Integer), CType(CType(21, Byte), Integer), CType(CType(78, Byte), Integer))
        Me.btnLoginHistory.FlatAppearance.BorderSize = 0
        Me.btnLoginHistory.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnLoginHistory.ForeColor = System.Drawing.Color.White
        Me.btnLoginHistory.Location = New System.Drawing.Point(402, 0)
        Me.btnLoginHistory.Name = "btnLoginHistory"
        Me.btnLoginHistory.Size = New System.Drawing.Size(198, 33)
        Me.btnLoginHistory.TabIndex = 18
        Me.btnLoginHistory.Text = "Login History"
        Me.btnLoginHistory.UseVisualStyleBackColor = False
        '
        'btnActivityHistory
        '
        Me.btnActivityHistory.BackColor = System.Drawing.Color.FromArgb(CType(CType(1, Byte), Integer), CType(CType(21, Byte), Integer), CType(CType(78, Byte), Integer))
        Me.btnActivityHistory.FlatAppearance.BorderSize = 0
        Me.btnActivityHistory.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnActivityHistory.ForeColor = System.Drawing.Color.White
        Me.btnActivityHistory.Location = New System.Drawing.Point(2, 0)
        Me.btnActivityHistory.Name = "btnActivityHistory"
        Me.btnActivityHistory.Size = New System.Drawing.Size(198, 33)
        Me.btnActivityHistory.TabIndex = 17
        Me.btnActivityHistory.Text = "Activity History"
        Me.btnActivityHistory.UseVisualStyleBackColor = False
        '
        'Panel1
        '
        Me.Panel1.Dock = System.Windows.Forms.DockStyle.Bottom
        Me.Panel1.Location = New System.Drawing.Point(0, 33)
        Me.Panel1.Name = "Panel1"
        Me.Panel1.Size = New System.Drawing.Size(1220, 817)
        Me.Panel1.TabIndex = 16
        '
        'btnPriceChangeHistory
        '
        Me.btnPriceChangeHistory.BackColor = System.Drawing.Color.FromArgb(CType(CType(1, Byte), Integer), CType(CType(21, Byte), Integer), CType(CType(78, Byte), Integer))
        Me.btnPriceChangeHistory.FlatAppearance.BorderSize = 0
        Me.btnPriceChangeHistory.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnPriceChangeHistory.ForeColor = System.Drawing.Color.White
        Me.btnPriceChangeHistory.Location = New System.Drawing.Point(202, 0)
        Me.btnPriceChangeHistory.Name = "btnPriceChangeHistory"
        Me.btnPriceChangeHistory.Size = New System.Drawing.Size(198, 33)
        Me.btnPriceChangeHistory.TabIndex = 15
        Me.btnPriceChangeHistory.Text = "Price Change History"
        Me.btnPriceChangeHistory.UseVisualStyleBackColor = False
        '
        'frmAuditLogs
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(1220, 850)
        Me.Controls.Add(Me.btnLoginHistory)
        Me.Controls.Add(Me.btnActivityHistory)
        Me.Controls.Add(Me.Panel1)
        Me.Controls.Add(Me.btnPriceChangeHistory)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None
        Me.Name = "frmAuditLogs"
        Me.Text = "frmAuditLogs"
        Me.ResumeLayout(False)

    End Sub

    Friend WithEvents btnLoginHistory As Button
    Friend WithEvents btnActivityHistory As Button
    Friend WithEvents Panel1 As Panel
    Friend WithEvents btnPriceChangeHistory As Button
End Class
