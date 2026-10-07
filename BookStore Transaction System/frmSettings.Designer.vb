<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmSettings
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
        Me.btngenerate = New System.Windows.Forms.Button()
        Me.SuspendLayout()
        '
        'btngenerate
        '
        Me.btngenerate.BackColor = System.Drawing.Color.FromArgb(CType(CType(1, Byte), Integer), CType(CType(21, Byte), Integer), CType(CType(78, Byte), Integer))
        Me.btngenerate.FlatAppearance.BorderSize = 0
        Me.btngenerate.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btngenerate.ForeColor = System.Drawing.Color.White
        Me.btngenerate.Location = New System.Drawing.Point(12, 12)
        Me.btngenerate.Name = "btngenerate"
        Me.btngenerate.Size = New System.Drawing.Size(136, 38)
        Me.btngenerate.TabIndex = 164
        Me.btngenerate.Text = "Manage School Year"
        Me.btngenerate.UseVisualStyleBackColor = False
        '
        'frmSettings
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(1223, 851)
        Me.Controls.Add(Me.btngenerate)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None
        Me.Name = "frmSettings"
        Me.Text = "frmSettings"
        Me.ResumeLayout(False)

    End Sub

    Friend WithEvents btngenerate As Button
End Class
