<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class frmAdminDashboard
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmAdminDashboard))
        Me.Panel1 = New System.Windows.Forms.Panel()
        Me.btnlogout = New System.Windows.Forms.Button()
        Me.btnAuditLogs = New System.Windows.Forms.Button()
        Me.btnStudentManagement = New System.Windows.Forms.Button()
        Me.btnUserManagement = New System.Windows.Forms.Button()
        Me.btnReports = New System.Windows.Forms.Button()
        Me.btnTransaction = New System.Windows.Forms.Button()
        Me.btnInventory = New System.Windows.Forms.Button()
        Me.btnPOS = New System.Windows.Forms.Button()
        Me.btnDashboard = New System.Windows.Forms.Button()
        Me.Panel4 = New System.Windows.Forms.Panel()
        Me.Panel3 = New System.Windows.Forms.Panel()
        Me.Panel2 = New System.Windows.Forms.Panel()
        Me.Label5 = New System.Windows.Forms.Label()
        Me.Label4 = New System.Windows.Forms.Label()
        Me.Logo = New System.Windows.Forms.PictureBox()
        Me.PictureBox1 = New System.Windows.Forms.PictureBox()
        Me.pnlContent = New System.Windows.Forms.Panel()
        Me.Panel1.SuspendLayout()
        Me.Panel2.SuspendLayout()
        CType(Me.Logo, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.PictureBox1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'Panel1
        '
        Me.Panel1.BackColor = System.Drawing.Color.FromArgb(CType(CType(1, Byte), Integer), CType(CType(21, Byte), Integer), CType(CType(78, Byte), Integer))
        Me.Panel1.Controls.Add(Me.btnlogout)
        Me.Panel1.Controls.Add(Me.btnAuditLogs)
        Me.Panel1.Controls.Add(Me.btnStudentManagement)
        Me.Panel1.Controls.Add(Me.btnUserManagement)
        Me.Panel1.Controls.Add(Me.btnReports)
        Me.Panel1.Controls.Add(Me.btnTransaction)
        Me.Panel1.Controls.Add(Me.btnInventory)
        Me.Panel1.Controls.Add(Me.btnPOS)
        Me.Panel1.Controls.Add(Me.btnDashboard)
        Me.Panel1.Controls.Add(Me.Panel4)
        Me.Panel1.Controls.Add(Me.Panel3)
        Me.Panel1.Controls.Add(Me.Panel2)
        Me.Panel1.Controls.Add(Me.PictureBox1)
        Me.Panel1.Dock = System.Windows.Forms.DockStyle.Left
        Me.Panel1.Location = New System.Drawing.Point(0, 0)
        Me.Panel1.Name = "Panel1"
        Me.Panel1.Size = New System.Drawing.Size(241, 851)
        Me.Panel1.TabIndex = 0
        '
        'btnlogout
        '
        Me.btnlogout.BackColor = System.Drawing.Color.Brown
        Me.btnlogout.Dock = System.Windows.Forms.DockStyle.Bottom
        Me.btnlogout.FlatAppearance.BorderSize = 0
        Me.btnlogout.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnlogout.Font = New System.Drawing.Font("Segoe UI", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnlogout.ForeColor = System.Drawing.Color.White
        Me.btnlogout.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btnlogout.Location = New System.Drawing.Point(10, 802)
        Me.btnlogout.Name = "btnlogout"
        Me.btnlogout.Size = New System.Drawing.Size(221, 49)
        Me.btnlogout.TabIndex = 16
        Me.btnlogout.Text = "Logout"
        Me.btnlogout.UseVisualStyleBackColor = False
        '
        'btnAuditLogs
        '
        Me.btnAuditLogs.BackColor = System.Drawing.Color.FromArgb(CType(CType(1, Byte), Integer), CType(CType(21, Byte), Integer), CType(CType(78, Byte), Integer))
        Me.btnAuditLogs.Dock = System.Windows.Forms.DockStyle.Top
        Me.btnAuditLogs.FlatAppearance.BorderSize = 0
        Me.btnAuditLogs.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnAuditLogs.Font = New System.Drawing.Font("Segoe UI", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnAuditLogs.ForeColor = System.Drawing.Color.White
        Me.btnAuditLogs.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btnAuditLogs.Location = New System.Drawing.Point(10, 443)
        Me.btnAuditLogs.Name = "btnAuditLogs"
        Me.btnAuditLogs.Size = New System.Drawing.Size(221, 49)
        Me.btnAuditLogs.TabIndex = 15
        Me.btnAuditLogs.Text = "Audit Logs"
        Me.btnAuditLogs.UseVisualStyleBackColor = False
        '
        'btnStudentManagement
        '
        Me.btnStudentManagement.BackColor = System.Drawing.Color.FromArgb(CType(CType(1, Byte), Integer), CType(CType(21, Byte), Integer), CType(CType(78, Byte), Integer))
        Me.btnStudentManagement.Dock = System.Windows.Forms.DockStyle.Top
        Me.btnStudentManagement.FlatAppearance.BorderSize = 0
        Me.btnStudentManagement.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnStudentManagement.Font = New System.Drawing.Font("Segoe UI", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnStudentManagement.ForeColor = System.Drawing.Color.White
        Me.btnStudentManagement.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btnStudentManagement.Location = New System.Drawing.Point(10, 394)
        Me.btnStudentManagement.Name = "btnStudentManagement"
        Me.btnStudentManagement.Size = New System.Drawing.Size(221, 49)
        Me.btnStudentManagement.TabIndex = 14
        Me.btnStudentManagement.Text = "Student Management"
        Me.btnStudentManagement.UseVisualStyleBackColor = False
        '
        'btnUserManagement
        '
        Me.btnUserManagement.BackColor = System.Drawing.Color.FromArgb(CType(CType(1, Byte), Integer), CType(CType(21, Byte), Integer), CType(CType(78, Byte), Integer))
        Me.btnUserManagement.Dock = System.Windows.Forms.DockStyle.Top
        Me.btnUserManagement.FlatAppearance.BorderSize = 0
        Me.btnUserManagement.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnUserManagement.Font = New System.Drawing.Font("Segoe UI", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnUserManagement.ForeColor = System.Drawing.Color.White
        Me.btnUserManagement.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btnUserManagement.Location = New System.Drawing.Point(10, 345)
        Me.btnUserManagement.Name = "btnUserManagement"
        Me.btnUserManagement.Size = New System.Drawing.Size(221, 49)
        Me.btnUserManagement.TabIndex = 13
        Me.btnUserManagement.Text = "User Management"
        Me.btnUserManagement.UseVisualStyleBackColor = False
        '
        'btnReports
        '
        Me.btnReports.BackColor = System.Drawing.Color.FromArgb(CType(CType(1, Byte), Integer), CType(CType(21, Byte), Integer), CType(CType(78, Byte), Integer))
        Me.btnReports.Dock = System.Windows.Forms.DockStyle.Top
        Me.btnReports.FlatAppearance.BorderSize = 0
        Me.btnReports.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnReports.Font = New System.Drawing.Font("Segoe UI", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnReports.ForeColor = System.Drawing.Color.White
        Me.btnReports.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btnReports.Location = New System.Drawing.Point(10, 296)
        Me.btnReports.Name = "btnReports"
        Me.btnReports.Size = New System.Drawing.Size(221, 49)
        Me.btnReports.TabIndex = 12
        Me.btnReports.Text = "Reports"
        Me.btnReports.UseVisualStyleBackColor = False
        '
        'btnTransaction
        '
        Me.btnTransaction.BackColor = System.Drawing.Color.FromArgb(CType(CType(1, Byte), Integer), CType(CType(21, Byte), Integer), CType(CType(78, Byte), Integer))
        Me.btnTransaction.Dock = System.Windows.Forms.DockStyle.Top
        Me.btnTransaction.FlatAppearance.BorderSize = 0
        Me.btnTransaction.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnTransaction.Font = New System.Drawing.Font("Segoe UI", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnTransaction.ForeColor = System.Drawing.Color.White
        Me.btnTransaction.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btnTransaction.Location = New System.Drawing.Point(10, 247)
        Me.btnTransaction.Name = "btnTransaction"
        Me.btnTransaction.Size = New System.Drawing.Size(221, 49)
        Me.btnTransaction.TabIndex = 9
        Me.btnTransaction.Text = "Transaction"
        Me.btnTransaction.UseVisualStyleBackColor = False
        '
        'btnInventory
        '
        Me.btnInventory.BackColor = System.Drawing.Color.FromArgb(CType(CType(1, Byte), Integer), CType(CType(21, Byte), Integer), CType(CType(78, Byte), Integer))
        Me.btnInventory.Dock = System.Windows.Forms.DockStyle.Top
        Me.btnInventory.FlatAppearance.BorderSize = 0
        Me.btnInventory.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnInventory.Font = New System.Drawing.Font("Segoe UI", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnInventory.ForeColor = System.Drawing.Color.White
        Me.btnInventory.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btnInventory.Location = New System.Drawing.Point(10, 198)
        Me.btnInventory.Name = "btnInventory"
        Me.btnInventory.Size = New System.Drawing.Size(221, 49)
        Me.btnInventory.TabIndex = 8
        Me.btnInventory.Text = "Inventory"
        Me.btnInventory.UseVisualStyleBackColor = False
        '
        'btnPOS
        '
        Me.btnPOS.BackColor = System.Drawing.Color.FromArgb(CType(CType(1, Byte), Integer), CType(CType(21, Byte), Integer), CType(CType(78, Byte), Integer))
        Me.btnPOS.Dock = System.Windows.Forms.DockStyle.Top
        Me.btnPOS.FlatAppearance.BorderSize = 0
        Me.btnPOS.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnPOS.Font = New System.Drawing.Font("Segoe UI", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnPOS.ForeColor = System.Drawing.Color.White
        Me.btnPOS.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btnPOS.Location = New System.Drawing.Point(10, 149)
        Me.btnPOS.Name = "btnPOS"
        Me.btnPOS.Size = New System.Drawing.Size(221, 49)
        Me.btnPOS.TabIndex = 4
        Me.btnPOS.Text = "POS"
        Me.btnPOS.UseVisualStyleBackColor = False
        '
        'btnDashboard
        '
        Me.btnDashboard.BackColor = System.Drawing.Color.FromArgb(CType(CType(1, Byte), Integer), CType(CType(21, Byte), Integer), CType(CType(78, Byte), Integer))
        Me.btnDashboard.Dock = System.Windows.Forms.DockStyle.Top
        Me.btnDashboard.FlatAppearance.BorderSize = 0
        Me.btnDashboard.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnDashboard.Font = New System.Drawing.Font("Segoe UI", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnDashboard.ForeColor = System.Drawing.Color.White
        Me.btnDashboard.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btnDashboard.Location = New System.Drawing.Point(10, 100)
        Me.btnDashboard.Name = "btnDashboard"
        Me.btnDashboard.Size = New System.Drawing.Size(221, 49)
        Me.btnDashboard.TabIndex = 3
        Me.btnDashboard.Text = "Dashboard"
        Me.btnDashboard.UseVisualStyleBackColor = False
        '
        'Panel4
        '
        Me.Panel4.BackColor = System.Drawing.Color.Transparent
        Me.Panel4.Dock = System.Windows.Forms.DockStyle.Right
        Me.Panel4.Location = New System.Drawing.Point(231, 100)
        Me.Panel4.Name = "Panel4"
        Me.Panel4.Size = New System.Drawing.Size(10, 751)
        Me.Panel4.TabIndex = 2
        '
        'Panel3
        '
        Me.Panel3.BackColor = System.Drawing.Color.Transparent
        Me.Panel3.Dock = System.Windows.Forms.DockStyle.Left
        Me.Panel3.Location = New System.Drawing.Point(0, 100)
        Me.Panel3.Name = "Panel3"
        Me.Panel3.Size = New System.Drawing.Size(10, 751)
        Me.Panel3.TabIndex = 1
        '
        'Panel2
        '
        Me.Panel2.BackColor = System.Drawing.Color.FromArgb(CType(CType(1, Byte), Integer), CType(CType(21, Byte), Integer), CType(CType(78, Byte), Integer))
        Me.Panel2.Controls.Add(Me.Label5)
        Me.Panel2.Controls.Add(Me.Label4)
        Me.Panel2.Controls.Add(Me.Logo)
        Me.Panel2.Dock = System.Windows.Forms.DockStyle.Top
        Me.Panel2.Location = New System.Drawing.Point(0, 0)
        Me.Panel2.Name = "Panel2"
        Me.Panel2.Size = New System.Drawing.Size(241, 100)
        Me.Panel2.TabIndex = 0
        '
        'Label5
        '
        Me.Label5.AutoSize = True
        Me.Label5.Font = New System.Drawing.Font("Segoe UI", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label5.ForeColor = System.Drawing.Color.White
        Me.Label5.Location = New System.Drawing.Point(81, 55)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(138, 15)
        Me.Label5.TabIndex = 24
        Me.Label5.Text = "TRANSACTION SYSTEM"
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.Font = New System.Drawing.Font("Segoe UI", 17.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label4.ForeColor = System.Drawing.Color.White
        Me.Label4.Location = New System.Drawing.Point(78, 23)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(147, 31)
        Me.Label4.TabIndex = 23
        Me.Label4.Text = "BOOKSTORE"
        '
        'Logo
        '
        Me.Logo.BackgroundImage = CType(resources.GetObject("Logo.BackgroundImage"), System.Drawing.Image)
        Me.Logo.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch
        Me.Logo.Location = New System.Drawing.Point(15, 19)
        Me.Logo.Name = "Logo"
        Me.Logo.Size = New System.Drawing.Size(60, 60)
        Me.Logo.TabIndex = 22
        Me.Logo.TabStop = False
        '
        'PictureBox1
        '
        Me.PictureBox1.BackgroundImage = CType(resources.GetObject("PictureBox1.BackgroundImage"), System.Drawing.Image)
        Me.PictureBox1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch
        Me.PictureBox1.Location = New System.Drawing.Point(0, 498)
        Me.PictureBox1.Name = "PictureBox1"
        Me.PictureBox1.Size = New System.Drawing.Size(241, 353)
        Me.PictureBox1.TabIndex = 0
        Me.PictureBox1.TabStop = False
        '
        'pnlContent
        '
        Me.pnlContent.Dock = System.Windows.Forms.DockStyle.Fill
        Me.pnlContent.Location = New System.Drawing.Point(241, 0)
        Me.pnlContent.Name = "pnlContent"
        Me.pnlContent.Size = New System.Drawing.Size(1223, 851)
        Me.pnlContent.TabIndex = 72
        '
        'frmAdminDashboard
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(1464, 851)
        Me.Controls.Add(Me.pnlContent)
        Me.Controls.Add(Me.Panel1)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Fixed3D
        Me.Name = "frmAdminDashboard"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Panel1.ResumeLayout(False)
        Me.Panel2.ResumeLayout(False)
        Me.Panel2.PerformLayout()
        CType(Me.Logo, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.PictureBox1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub

    Friend WithEvents Panel1 As Panel
    Friend WithEvents Panel2 As Panel
    Friend WithEvents btnPOS As Button
    Friend WithEvents btnDashboard As Button
    Friend WithEvents btnTransaction As Button
    Friend WithEvents btnInventory As Button
    Friend WithEvents Logo As PictureBox
    Friend WithEvents Label5 As Label
    Friend WithEvents Label4 As Label
    Friend WithEvents btnReports As Button
    Friend WithEvents btnUserManagement As Button
    Friend WithEvents pnlContent As Panel
    Friend WithEvents btnStudentManagement As Button
    Friend WithEvents btnAuditLogs As Button
    Friend WithEvents PictureBox1 As PictureBox
    Friend WithEvents btnlogout As Button
    Friend WithEvents Panel4 As Panel
    Friend WithEvents Panel3 As Panel
End Class
