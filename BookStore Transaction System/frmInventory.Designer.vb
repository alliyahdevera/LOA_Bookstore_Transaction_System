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
        Me.btnProductList = New System.Windows.Forms.Button()
        Me.btnManageProducts = New System.Windows.Forms.Button()
        Me.btnStockInHistory = New System.Windows.Forms.Button()
        Me.btnStockEntry = New System.Windows.Forms.Button()
        Me.Panel1 = New System.Windows.Forms.Panel()
        Me.btnLowLevelStocks = New System.Windows.Forms.Button()
        Me.Button1 = New System.Windows.Forms.Button()
        Me.SuspendLayout()
        '
        'btnProductList
        '
        Me.btnProductList.BackColor = System.Drawing.Color.FromArgb(CType(CType(1, Byte), Integer), CType(CType(21, Byte), Integer), CType(CType(78, Byte), Integer))
        Me.btnProductList.FlatAppearance.BorderSize = 0
        Me.btnProductList.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnProductList.ForeColor = System.Drawing.Color.White
        Me.btnProductList.Location = New System.Drawing.Point(0, 0)
        Me.btnProductList.Name = "btnProductList"
        Me.btnProductList.Size = New System.Drawing.Size(198, 33)
        Me.btnProductList.TabIndex = 2
        Me.btnProductList.Text = "Product List"
        Me.btnProductList.UseVisualStyleBackColor = False
        '
        'btnManageProducts
        '
        Me.btnManageProducts.BackColor = System.Drawing.Color.FromArgb(CType(CType(1, Byte), Integer), CType(CType(21, Byte), Integer), CType(CType(78, Byte), Integer))
        Me.btnManageProducts.FlatAppearance.BorderSize = 0
        Me.btnManageProducts.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnManageProducts.ForeColor = System.Drawing.Color.White
        Me.btnManageProducts.Location = New System.Drawing.Point(199, 0)
        Me.btnManageProducts.Name = "btnManageProducts"
        Me.btnManageProducts.Size = New System.Drawing.Size(198, 33)
        Me.btnManageProducts.TabIndex = 3
        Me.btnManageProducts.Text = "Manage Products"
        Me.btnManageProducts.UseVisualStyleBackColor = False
        '
        'btnStockInHistory
        '
        Me.btnStockInHistory.BackColor = System.Drawing.Color.FromArgb(CType(CType(1, Byte), Integer), CType(CType(21, Byte), Integer), CType(CType(78, Byte), Integer))
        Me.btnStockInHistory.FlatAppearance.BorderSize = 0
        Me.btnStockInHistory.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnStockInHistory.ForeColor = System.Drawing.Color.White
        Me.btnStockInHistory.Location = New System.Drawing.Point(597, 0)
        Me.btnStockInHistory.Name = "btnStockInHistory"
        Me.btnStockInHistory.Size = New System.Drawing.Size(198, 33)
        Me.btnStockInHistory.TabIndex = 5
        Me.btnStockInHistory.Text = "Stock In History"
        Me.btnStockInHistory.UseVisualStyleBackColor = False
        '
        'btnStockEntry
        '
        Me.btnStockEntry.BackColor = System.Drawing.Color.FromArgb(CType(CType(1, Byte), Integer), CType(CType(21, Byte), Integer), CType(CType(78, Byte), Integer))
        Me.btnStockEntry.FlatAppearance.BorderSize = 0
        Me.btnStockEntry.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnStockEntry.ForeColor = System.Drawing.Color.White
        Me.btnStockEntry.Location = New System.Drawing.Point(398, 0)
        Me.btnStockEntry.Name = "btnStockEntry"
        Me.btnStockEntry.Size = New System.Drawing.Size(198, 33)
        Me.btnStockEntry.TabIndex = 4
        Me.btnStockEntry.Text = "Stock Entry"
        Me.btnStockEntry.UseVisualStyleBackColor = False
        '
        'Panel1
        '
        Me.Panel1.Dock = System.Windows.Forms.DockStyle.Bottom
        Me.Panel1.Location = New System.Drawing.Point(0, 32)
        Me.Panel1.Name = "Panel1"
        Me.Panel1.Size = New System.Drawing.Size(1220, 818)
        Me.Panel1.TabIndex = 6
        '
        'btnLowLevelStocks
        '
        Me.btnLowLevelStocks.BackColor = System.Drawing.Color.FromArgb(CType(CType(1, Byte), Integer), CType(CType(21, Byte), Integer), CType(CType(78, Byte), Integer))
        Me.btnLowLevelStocks.FlatAppearance.BorderSize = 0
        Me.btnLowLevelStocks.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnLowLevelStocks.ForeColor = System.Drawing.Color.White
        Me.btnLowLevelStocks.Location = New System.Drawing.Point(796, 0)
        Me.btnLowLevelStocks.Name = "btnLowLevelStocks"
        Me.btnLowLevelStocks.Size = New System.Drawing.Size(198, 33)
        Me.btnLowLevelStocks.TabIndex = 10
        Me.btnLowLevelStocks.Text = "Low Level Stocks"
        Me.btnLowLevelStocks.UseVisualStyleBackColor = False
        '
        'Button1
        '
        Me.Button1.BackColor = System.Drawing.Color.FromArgb(CType(CType(1, Byte), Integer), CType(CType(21, Byte), Integer), CType(CType(78, Byte), Integer))
        Me.Button1.FlatAppearance.BorderSize = 0
        Me.Button1.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.Button1.ForeColor = System.Drawing.Color.White
        Me.Button1.Location = New System.Drawing.Point(995, 0)
        Me.Button1.Name = "Button1"
        Me.Button1.Size = New System.Drawing.Size(198, 33)
        Me.Button1.TabIndex = 11
        Me.Button1.Text = "Inventory Count && Reconciliation"
        Me.Button1.UseVisualStyleBackColor = False
        '
        'frmInventory
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(1220, 850)
        Me.Controls.Add(Me.Button1)
        Me.Controls.Add(Me.btnLowLevelStocks)
        Me.Controls.Add(Me.Panel1)
        Me.Controls.Add(Me.btnStockInHistory)
        Me.Controls.Add(Me.btnStockEntry)
        Me.Controls.Add(Me.btnManageProducts)
        Me.Controls.Add(Me.btnProductList)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None
        Me.Name = "frmInventory"
        Me.Text = "frmInventory"
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents btnProductList As Button
    Friend WithEvents btnManageProducts As Button
    Friend WithEvents btnStockInHistory As Button
    Friend WithEvents btnStockEntry As Button
    Friend WithEvents Panel1 As Panel
    Friend WithEvents btnLowLevelStocks As Button
    Friend WithEvents Button1 As Button
End Class
