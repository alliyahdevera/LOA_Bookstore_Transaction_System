<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class frmInventory
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
        Me.pnlinventory = New System.Windows.Forms.Panel()
        Me.Label6 = New System.Windows.Forms.Label()
        Me.cboInventory = New System.Windows.Forms.ComboBox()
        Me.Panel1 = New System.Windows.Forms.Panel()
        Me.Panel1.SuspendLayout()
        Me.SuspendLayout()
        '
        'pnlinventory
        '
        Me.pnlinventory.Dock = System.Windows.Forms.DockStyle.Bottom
        Me.pnlinventory.Location = New System.Drawing.Point(0, 46)
        Me.pnlinventory.Name = "pnlinventory"
        Me.pnlinventory.Size = New System.Drawing.Size(1220, 804)
        Me.pnlinventory.TabIndex = 6
        '
        'Label6
        '
        Me.Label6.AutoSize = True
        Me.Label6.Font = New System.Drawing.Font("Segoe UI", 18.0!, System.Drawing.FontStyle.Bold)
        Me.Label6.ForeColor = System.Drawing.Color.FromArgb(CType(CType(1, Byte), Integer), CType(CType(21, Byte), Integer), CType(CType(78, Byte), Integer))
        Me.Label6.Location = New System.Drawing.Point(6, 7)
        Me.Label6.Name = "Label6"
        Me.Label6.Size = New System.Drawing.Size(126, 32)
        Me.Label6.TabIndex = 149
        Me.Label6.Text = "Inventory"
        '
        'cboInventory
        '
        Me.cboInventory.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cboInventory.Font = New System.Drawing.Font("Segoe UI Semibold", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cboInventory.FormattingEnabled = True
        Me.cboInventory.Items.AddRange(New Object() {"Product List", "Manage Products", "Stock In", "Stock In History", "Low Level Stocks", "Inventory Count & Reconciliation"})
        Me.cboInventory.Location = New System.Drawing.Point(140, 10)
        Me.cboInventory.Name = "cboInventory"
        Me.cboInventory.Size = New System.Drawing.Size(298, 29)
        Me.cboInventory.TabIndex = 148
        '
        'Panel1
        '
        Me.Panel1.Controls.Add(Me.Label6)
        Me.Panel1.Controls.Add(Me.cboInventory)
        Me.Panel1.Dock = System.Windows.Forms.DockStyle.Top
        Me.Panel1.Location = New System.Drawing.Point(0, 0)
        Me.Panel1.Name = "Panel1"
        Me.Panel1.Size = New System.Drawing.Size(1220, 45)
        Me.Panel1.TabIndex = 150
        '
        'frmInventory
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(1220, 850)
        Me.Controls.Add(Me.Panel1)
        Me.Controls.Add(Me.pnlinventory)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None
        Me.Name = "frmInventory"
        Me.Text = "frmInventory"
        Me.Panel1.ResumeLayout(False)
        Me.Panel1.PerformLayout()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents pnlinventory As Panel
    Friend WithEvents Label6 As Label
    Friend WithEvents cboInventory As ComboBox
    Friend WithEvents Panel1 As Panel
End Class
