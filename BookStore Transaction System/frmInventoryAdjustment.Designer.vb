<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmInventoryAdjustment
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
        Me.txtsize = New System.Windows.Forms.ComboBox()
        Me.txtProduct = New System.Windows.Forms.ComboBox()
        Me.Label9 = New System.Windows.Forms.Label()
        Me.Panel5 = New System.Windows.Forms.Panel()
        Me.Label4 = New System.Windows.Forms.Label()
        Me.Label13 = New System.Windows.Forms.Label()
        Me.Label16 = New System.Windows.Forms.Label()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.lblsysq = New System.Windows.Forms.Label()
        Me.lblphyq = New System.Windows.Forms.Label()
        Me.lblDifference = New System.Windows.Forms.Label()
        Me.reason = New System.Windows.Forms.Label()
        Me.txtReason = New System.Windows.Forms.TextBox()
        Me.Panel1 = New System.Windows.Forms.Panel()
        Me.btncancel = New System.Windows.Forms.Button()
        Me.btnconfirm = New System.Windows.Forms.Button()
        Me.Label5 = New System.Windows.Forms.Label()
        Me.Label6 = New System.Windows.Forms.Label()
        Me.PictureBox1 = New System.Windows.Forms.PictureBox()
        Me.txtStatus = New System.Windows.Forms.TextBox()
        Me.Panel5.SuspendLayout()
        Me.Panel1.SuspendLayout()
        CType(Me.PictureBox1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'txtsize
        '
        Me.txtsize.Font = New System.Drawing.Font("Segoe UI", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtsize.FormattingEnabled = True
        Me.txtsize.Location = New System.Drawing.Point(145, 195)
        Me.txtsize.Name = "txtsize"
        Me.txtsize.Size = New System.Drawing.Size(193, 25)
        Me.txtsize.TabIndex = 131
        '
        'txtProduct
        '
        Me.txtProduct.Font = New System.Drawing.Font("Segoe UI", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtProduct.FormattingEnabled = True
        Me.txtProduct.Location = New System.Drawing.Point(145, 157)
        Me.txtProduct.Name = "txtProduct"
        Me.txtProduct.Size = New System.Drawing.Size(193, 25)
        Me.txtProduct.TabIndex = 130
        '
        'Label9
        '
        Me.Label9.AutoSize = True
        Me.Label9.Font = New System.Drawing.Font("Segoe UI Semibold", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label9.Location = New System.Drawing.Point(25, 239)
        Me.Label9.Name = "Label9"
        Me.Label9.Size = New System.Drawing.Size(99, 17)
        Me.Label9.TabIndex = 127
        Me.Label9.Text = "Available Stock"
        '
        'Panel5
        '
        Me.Panel5.BackColor = System.Drawing.Color.FromArgb(CType(CType(1, Byte), Integer), CType(CType(21, Byte), Integer), CType(CType(78, Byte), Integer))
        Me.Panel5.Controls.Add(Me.Label4)
        Me.Panel5.Location = New System.Drawing.Point(1, 0)
        Me.Panel5.Name = "Panel5"
        Me.Panel5.Size = New System.Drawing.Size(799, 35)
        Me.Panel5.TabIndex = 11
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.Font = New System.Drawing.Font("Segoe UI Semibold", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label4.ForeColor = System.Drawing.Color.White
        Me.Label4.Location = New System.Drawing.Point(7, 7)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(170, 21)
        Me.Label4.TabIndex = 10
        Me.Label4.Text = "Inventory Adjustment"
        '
        'Label13
        '
        Me.Label13.AutoSize = True
        Me.Label13.Font = New System.Drawing.Font("Segoe UI Semibold", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label13.Location = New System.Drawing.Point(25, 198)
        Me.Label13.Name = "Label13"
        Me.Label13.Size = New System.Drawing.Size(31, 17)
        Me.Label13.TabIndex = 126
        Me.Label13.Text = "Size"
        '
        'Label16
        '
        Me.Label16.AutoSize = True
        Me.Label16.Font = New System.Drawing.Font("Segoe UI Semibold", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label16.Location = New System.Drawing.Point(25, 160)
        Me.Label16.Name = "Label16"
        Me.Label16.Size = New System.Drawing.Size(56, 17)
        Me.Label16.TabIndex = 125
        Me.Label16.Text = "Product"
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Font = New System.Drawing.Font("Segoe UI Semibold", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label1.Location = New System.Drawing.Point(374, 160)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(109, 17)
        Me.Label1.TabIndex = 132
        Me.Label1.Text = "System Quantity"
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Font = New System.Drawing.Font("Segoe UI Semibold", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label2.Location = New System.Drawing.Point(374, 198)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(113, 17)
        Me.Label2.TabIndex = 133
        Me.Label2.Text = "Physical Quantity"
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Font = New System.Drawing.Font("Segoe UI Semibold", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label3.Location = New System.Drawing.Point(374, 239)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(68, 17)
        Me.Label3.TabIndex = 134
        Me.Label3.Text = "Difference"
        '
        'lblsysq
        '
        Me.lblsysq.AutoSize = True
        Me.lblsysq.Font = New System.Drawing.Font("Segoe UI Semibold", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblsysq.Location = New System.Drawing.Point(500, 160)
        Me.lblsysq.Name = "lblsysq"
        Me.lblsysq.Size = New System.Drawing.Size(13, 17)
        Me.lblsysq.TabIndex = 135
        Me.lblsysq.Text = "-"
        '
        'lblphyq
        '
        Me.lblphyq.AutoSize = True
        Me.lblphyq.Font = New System.Drawing.Font("Segoe UI Semibold", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblphyq.Location = New System.Drawing.Point(500, 198)
        Me.lblphyq.Name = "lblphyq"
        Me.lblphyq.Size = New System.Drawing.Size(13, 17)
        Me.lblphyq.TabIndex = 136
        Me.lblphyq.Text = "-"
        '
        'lblDifference
        '
        Me.lblDifference.AutoSize = True
        Me.lblDifference.Font = New System.Drawing.Font("Segoe UI Semibold", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblDifference.ForeColor = System.Drawing.Color.Maroon
        Me.lblDifference.Location = New System.Drawing.Point(500, 239)
        Me.lblDifference.Name = "lblDifference"
        Me.lblDifference.Size = New System.Drawing.Size(13, 17)
        Me.lblDifference.TabIndex = 137
        Me.lblDifference.Text = "-"
        '
        'reason
        '
        Me.reason.AutoSize = True
        Me.reason.Font = New System.Drawing.Font("Segoe UI Semibold", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.reason.Location = New System.Drawing.Point(25, 283)
        Me.reason.Name = "reason"
        Me.reason.Size = New System.Drawing.Size(79, 34)
        Me.reason.TabIndex = 139
        Me.reason.Text = "Reason for" & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "Adjustment" & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10)
        '
        'txtReason
        '
        Me.txtReason.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.txtReason.Font = New System.Drawing.Font("Segoe UI", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtReason.Location = New System.Drawing.Point(145, 283)
        Me.txtReason.Multiline = True
        Me.txtReason.Name = "txtReason"
        Me.txtReason.Size = New System.Drawing.Size(489, 79)
        Me.txtReason.TabIndex = 138
        '
        'Panel1
        '
        Me.Panel1.BackColor = System.Drawing.Color.LemonChiffon
        Me.Panel1.Controls.Add(Me.PictureBox1)
        Me.Panel1.Controls.Add(Me.Label6)
        Me.Panel1.Controls.Add(Me.Label5)
        Me.Panel1.Location = New System.Drawing.Point(28, 57)
        Me.Panel1.Name = "Panel1"
        Me.Panel1.Size = New System.Drawing.Size(606, 81)
        Me.Panel1.TabIndex = 140
        '
        'btncancel
        '
        Me.btncancel.BackColor = System.Drawing.Color.Gray
        Me.btncancel.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btncancel.ForeColor = System.Drawing.Color.White
        Me.btncancel.Location = New System.Drawing.Point(484, 378)
        Me.btncancel.Name = "btncancel"
        Me.btncancel.Size = New System.Drawing.Size(150, 35)
        Me.btncancel.TabIndex = 142
        Me.btncancel.Text = "Cancel"
        Me.btncancel.UseVisualStyleBackColor = False
        '
        'btnconfirm
        '
        Me.btnconfirm.BackColor = System.Drawing.Color.Navy
        Me.btnconfirm.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnconfirm.ForeColor = System.Drawing.Color.White
        Me.btnconfirm.Location = New System.Drawing.Point(320, 378)
        Me.btnconfirm.Name = "btnconfirm"
        Me.btnconfirm.Size = New System.Drawing.Size(150, 35)
        Me.btnconfirm.TabIndex = 141
        Me.btnconfirm.Text = "Confirm Adjustment"
        Me.btnconfirm.UseVisualStyleBackColor = False
        '
        'Label5
        '
        Me.Label5.AutoSize = True
        Me.Label5.Font = New System.Drawing.Font("Segoe UI", 11.0!, System.Drawing.FontStyle.Bold)
        Me.Label5.ForeColor = System.Drawing.Color.FromArgb(CType(CType(1, Byte), Integer), CType(CType(21, Byte), Integer), CType(CType(78, Byte), Integer))
        Me.Label5.Location = New System.Drawing.Point(71, 21)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(518, 20)
        Me.Label5.TabIndex = 143
        Me.Label5.Text = "You are about to adjust the system quantity based on the physical count." & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10)
        '
        'Label6
        '
        Me.Label6.AutoSize = True
        Me.Label6.Font = New System.Drawing.Font("Segoe UI", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label6.ForeColor = System.Drawing.Color.FromArgb(CType(CType(1, Byte), Integer), CType(CType(21, Byte), Integer), CType(CType(78, Byte), Integer))
        Me.Label6.Location = New System.Drawing.Point(72, 42)
        Me.Label6.Name = "Label6"
        Me.Label6.Size = New System.Drawing.Size(360, 17)
        Me.Label6.TabIndex = 144
        Me.Label6.Text = "This action will be recorded in the stock entry and audit logs" & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10)
        '
        'PictureBox1
        '
        Me.PictureBox1.Location = New System.Drawing.Point(15, 15)
        Me.PictureBox1.Name = "PictureBox1"
        Me.PictureBox1.Size = New System.Drawing.Size(50, 50)
        Me.PictureBox1.TabIndex = 145
        Me.PictureBox1.TabStop = False
        '
        'txtStatus
        '
        Me.txtStatus.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.txtStatus.Font = New System.Drawing.Font("Segoe UI", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtStatus.Location = New System.Drawing.Point(145, 237)
        Me.txtStatus.Name = "txtStatus"
        Me.txtStatus.Size = New System.Drawing.Size(193, 25)
        Me.txtStatus.TabIndex = 143
        '
        'frmInventoryAdjustment
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(660, 436)
        Me.Controls.Add(Me.txtStatus)
        Me.Controls.Add(Me.btncancel)
        Me.Controls.Add(Me.btnconfirm)
        Me.Controls.Add(Me.Panel1)
        Me.Controls.Add(Me.reason)
        Me.Controls.Add(Me.txtReason)
        Me.Controls.Add(Me.lblsysq)
        Me.Controls.Add(Me.lblphyq)
        Me.Controls.Add(Me.lblDifference)
        Me.Controls.Add(Me.Label1)
        Me.Controls.Add(Me.Label2)
        Me.Controls.Add(Me.Label3)
        Me.Controls.Add(Me.txtsize)
        Me.Controls.Add(Me.Panel5)
        Me.Controls.Add(Me.txtProduct)
        Me.Controls.Add(Me.Label16)
        Me.Controls.Add(Me.Label13)
        Me.Controls.Add(Me.Label9)
        Me.Name = "frmInventoryAdjustment"
        Me.Text = "frmInventoryAdjustment"
        Me.Panel5.ResumeLayout(False)
        Me.Panel5.PerformLayout()
        Me.Panel1.ResumeLayout(False)
        Me.Panel1.PerformLayout()
        CType(Me.PictureBox1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents txtsize As ComboBox
    Friend WithEvents txtProduct As ComboBox
    Friend WithEvents Label9 As Label
    Friend WithEvents Panel5 As Panel
    Friend WithEvents Label4 As Label
    Friend WithEvents Label13 As Label
    Friend WithEvents Label16 As Label
    Friend WithEvents Label1 As Label
    Friend WithEvents Label2 As Label
    Friend WithEvents Label3 As Label
    Friend WithEvents lblsysq As Label
    Friend WithEvents lblphyq As Label
    Friend WithEvents lblDifference As Label
    Friend WithEvents reason As Label
    Friend WithEvents txtReason As TextBox
    Friend WithEvents Panel1 As Panel
    Friend WithEvents btncancel As Button
    Friend WithEvents btnconfirm As Button
    Friend WithEvents PictureBox1 As PictureBox
    Friend WithEvents Label6 As Label
    Friend WithEvents Label5 As Label
    Friend WithEvents txtStatus As TextBox
End Class
