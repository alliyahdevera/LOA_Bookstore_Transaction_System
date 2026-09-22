<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class Login
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(Login))
        Me.Label5 = New System.Windows.Forms.Label()
        Me.Label4 = New System.Windows.Forms.Label()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.txtUsername = New System.Windows.Forms.TextBox()
        Me.btnLogin = New System.Windows.Forms.Button()
        Me.UserPanel = New System.Windows.Forms.Panel()
        Me.User = New System.Windows.Forms.PictureBox()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.PWPanel = New System.Windows.Forms.Panel()
        Me.PW = New System.Windows.Forms.PictureBox()
        Me.txtPassword = New System.Windows.Forms.TextBox()
        Me.cboShowPW = New System.Windows.Forms.CheckBox()
        Me.Logo = New System.Windows.Forms.PictureBox()
        Me.btnExit = New System.Windows.Forms.Button()
        Me.LoginPanel = New System.Windows.Forms.Panel()
        Me.Label8 = New System.Windows.Forms.Label()
        Me.Label7 = New System.Windows.Forms.Label()
        Me.Label6 = New System.Windows.Forms.Label()
        Me.UserPanel.SuspendLayout()
        CType(Me.User, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.PWPanel.SuspendLayout()
        CType(Me.PW, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.Logo, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.LoginPanel.SuspendLayout()
        Me.SuspendLayout()
        '
        'Label5
        '
        Me.Label5.AutoSize = True
        Me.Label5.Font = New System.Drawing.Font("Segoe UI", 11.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label5.ForeColor = System.Drawing.Color.FromArgb(CType(CType(1, Byte), Integer), CType(CType(21, Byte), Integer), CType(CType(78, Byte), Integer))
        Me.Label5.Location = New System.Drawing.Point(167, 279)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(176, 20)
        Me.Label5.TabIndex = 15
        Me.Label5.Text = "TRANSACTION SYSTEM"
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.Font = New System.Drawing.Font("Segoe UI", 26.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label4.ForeColor = System.Drawing.Color.FromArgb(CType(CType(1, Byte), Integer), CType(CType(21, Byte), Integer), CType(CType(78, Byte), Integer))
        Me.Label4.Location = New System.Drawing.Point(143, 232)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(228, 47)
        Me.Label4.TabIndex = 14
        Me.Label4.Text = "BOOKSTORE"
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Font = New System.Drawing.Font("Segoe UI", 11.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label3.ForeColor = System.Drawing.Color.Silver
        Me.Label3.Location = New System.Drawing.Point(167, 314)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(177, 20)
        Me.Label3.TabIndex = 13
        Me.Label3.Text = "Please sign in to continue"
        '
        'txtUsername
        '
        Me.txtUsername.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.txtUsername.Font = New System.Drawing.Font("Segoe UI", 12.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtUsername.Location = New System.Drawing.Point(37, 6)
        Me.txtUsername.Name = "txtUsername"
        Me.txtUsername.Size = New System.Drawing.Size(252, 23)
        Me.txtUsername.TabIndex = 10
        '
        'btnLogin
        '
        Me.btnLogin.BackColor = System.Drawing.Color.FromArgb(CType(CType(1, Byte), Integer), CType(CType(21, Byte), Integer), CType(CType(78, Byte), Integer))
        Me.btnLogin.FlatAppearance.BorderSize = 0
        Me.btnLogin.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnLogin.Font = New System.Drawing.Font("Segoe UI", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnLogin.ForeColor = System.Drawing.Color.White
        Me.btnLogin.Location = New System.Drawing.Point(106, 528)
        Me.btnLogin.Name = "btnLogin"
        Me.btnLogin.Size = New System.Drawing.Size(296, 39)
        Me.btnLogin.TabIndex = 8
        Me.btnLogin.Text = "Login"
        Me.btnLogin.UseVisualStyleBackColor = False
        '
        'UserPanel
        '
        Me.UserPanel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.UserPanel.Controls.Add(Me.User)
        Me.UserPanel.Controls.Add(Me.txtUsername)
        Me.UserPanel.Location = New System.Drawing.Point(106, 365)
        Me.UserPanel.Name = "UserPanel"
        Me.UserPanel.Size = New System.Drawing.Size(296, 37)
        Me.UserPanel.TabIndex = 16
        '
        'User
        '
        Me.User.BackgroundImage = CType(resources.GetObject("User.BackgroundImage"), System.Drawing.Image)
        Me.User.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch
        Me.User.Location = New System.Drawing.Point(8, 8)
        Me.User.Name = "User"
        Me.User.Size = New System.Drawing.Size(20, 20)
        Me.User.TabIndex = 11
        Me.User.TabStop = False
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Font = New System.Drawing.Font("Segoe UI", 11.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label1.ForeColor = System.Drawing.Color.FromArgb(CType(CType(1, Byte), Integer), CType(CType(21, Byte), Integer), CType(CType(78, Byte), Integer))
        Me.Label1.Location = New System.Drawing.Point(102, 343)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(75, 20)
        Me.Label1.TabIndex = 11
        Me.Label1.Text = "Username"
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Font = New System.Drawing.Font("Segoe UI", 11.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label2.ForeColor = System.Drawing.Color.FromArgb(CType(CType(1, Byte), Integer), CType(CType(21, Byte), Integer), CType(CType(78, Byte), Integer))
        Me.Label2.Location = New System.Drawing.Point(102, 412)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(70, 20)
        Me.Label2.TabIndex = 17
        Me.Label2.Text = "Password"
        '
        'PWPanel
        '
        Me.PWPanel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.PWPanel.Controls.Add(Me.PW)
        Me.PWPanel.Controls.Add(Me.txtPassword)
        Me.PWPanel.Location = New System.Drawing.Point(106, 434)
        Me.PWPanel.Name = "PWPanel"
        Me.PWPanel.Size = New System.Drawing.Size(296, 37)
        Me.PWPanel.TabIndex = 18
        '
        'PW
        '
        Me.PW.BackgroundImage = CType(resources.GetObject("PW.BackgroundImage"), System.Drawing.Image)
        Me.PW.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch
        Me.PW.Location = New System.Drawing.Point(8, 8)
        Me.PW.Name = "PW"
        Me.PW.Size = New System.Drawing.Size(20, 20)
        Me.PW.TabIndex = 11
        Me.PW.TabStop = False
        '
        'txtPassword
        '
        Me.txtPassword.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.txtPassword.Font = New System.Drawing.Font("Segoe UI", 12.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtPassword.Location = New System.Drawing.Point(37, 6)
        Me.txtPassword.Name = "txtPassword"
        Me.txtPassword.PasswordChar = Global.Microsoft.VisualBasic.ChrW(42)
        Me.txtPassword.Size = New System.Drawing.Size(252, 23)
        Me.txtPassword.TabIndex = 10
        '
        'cboShowPW
        '
        Me.cboShowPW.AutoSize = True
        Me.cboShowPW.Font = New System.Drawing.Font("Segoe UI", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cboShowPW.ForeColor = System.Drawing.Color.FromArgb(CType(CType(1, Byte), Integer), CType(CType(21, Byte), Integer), CType(CType(78, Byte), Integer))
        Me.cboShowPW.Location = New System.Drawing.Point(301, 480)
        Me.cboShowPW.Name = "cboShowPW"
        Me.cboShowPW.Size = New System.Drawing.Size(107, 17)
        Me.cboShowPW.TabIndex = 20
        Me.cboShowPW.Text = "Show Password"
        Me.cboShowPW.UseVisualStyleBackColor = True
        '
        'Logo
        '
        Me.Logo.BackgroundImage = CType(resources.GetObject("Logo.BackgroundImage"), System.Drawing.Image)
        Me.Logo.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch
        Me.Logo.Location = New System.Drawing.Point(188, 36)
        Me.Logo.Name = "Logo"
        Me.Logo.Size = New System.Drawing.Size(125, 125)
        Me.Logo.TabIndex = 21
        Me.Logo.TabStop = False
        '
        'btnExit
        '
        Me.btnExit.BackColor = System.Drawing.Color.Transparent
        Me.btnExit.FlatAppearance.BorderSize = 0
        Me.btnExit.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnExit.Font = New System.Drawing.Font("Segoe UI", 14.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnExit.ForeColor = System.Drawing.Color.White
        Me.btnExit.Location = New System.Drawing.Point(1169, 2)
        Me.btnExit.Name = "btnExit"
        Me.btnExit.Size = New System.Drawing.Size(30, 32)
        Me.btnExit.TabIndex = 22
        Me.btnExit.Text = "X"
        Me.btnExit.UseVisualStyleBackColor = False
        '
        'LoginPanel
        '
        Me.LoginPanel.BackColor = System.Drawing.Color.White
        Me.LoginPanel.Controls.Add(Me.Label8)
        Me.LoginPanel.Controls.Add(Me.Label7)
        Me.LoginPanel.Controls.Add(Me.Logo)
        Me.LoginPanel.Controls.Add(Me.cboShowPW)
        Me.LoginPanel.Controls.Add(Me.Label2)
        Me.LoginPanel.Controls.Add(Me.PWPanel)
        Me.LoginPanel.Controls.Add(Me.Label1)
        Me.LoginPanel.Controls.Add(Me.UserPanel)
        Me.LoginPanel.Controls.Add(Me.Label5)
        Me.LoginPanel.Controls.Add(Me.Label4)
        Me.LoginPanel.Controls.Add(Me.Label3)
        Me.LoginPanel.Controls.Add(Me.btnLogin)
        Me.LoginPanel.Controls.Add(Me.Label6)
        Me.LoginPanel.Location = New System.Drawing.Point(352, 35)
        Me.LoginPanel.Name = "LoginPanel"
        Me.LoginPanel.Size = New System.Drawing.Size(497, 660)
        Me.LoginPanel.TabIndex = 23
        '
        'Label8
        '
        Me.Label8.AutoSize = True
        Me.Label8.Font = New System.Drawing.Font("Times New Roman", 12.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label8.ForeColor = System.Drawing.Color.FromArgb(CType(CType(1, Byte), Integer), CType(CType(21, Byte), Integer), CType(CType(78, Byte), Integer))
        Me.Label8.Location = New System.Drawing.Point(173, 200)
        Me.Label8.Name = "Label8"
        Me.Label8.Size = New System.Drawing.Size(155, 19)
        Me.Label8.TabIndex = 25
        Me.Label8.Text = "OF ALABANG INC."
        '
        'Label7
        '
        Me.Label7.AutoSize = True
        Me.Label7.Font = New System.Drawing.Font("Times New Roman", 26.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label7.ForeColor = System.Drawing.Color.FromArgb(CType(CType(1, Byte), Integer), CType(CType(21, Byte), Integer), CType(CType(78, Byte), Integer))
        Me.Label7.Location = New System.Drawing.Point(169, 162)
        Me.Label7.Name = "Label7"
        Me.Label7.Size = New System.Drawing.Size(170, 40)
        Me.Label7.TabIndex = 24
        Me.Label7.Text = "LYCEUM"
        '
        'Label6
        '
        Me.Label6.AutoSize = True
        Me.Label6.Font = New System.Drawing.Font("Arial Black", 11.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label6.ForeColor = System.Drawing.Color.FromArgb(CType(CType(253, Byte), Integer), CType(CType(202, Byte), Integer), CType(CType(3, Byte), Integer))
        Me.Label6.Location = New System.Drawing.Point(107, 278)
        Me.Label6.Name = "Label6"
        Me.Label6.Size = New System.Drawing.Size(296, 22)
        Me.Label6.TabIndex = 23
        Me.Label6.Text = "──────────────────────────"
        '
        'Login
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.Color.FromArgb(CType(CType(1, Byte), Integer), CType(CType(21, Byte), Integer), CType(CType(78, Byte), Integer))
        Me.ClientSize = New System.Drawing.Size(1201, 731)
        Me.Controls.Add(Me.LoginPanel)
        Me.Controls.Add(Me.btnExit)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None
        Me.Name = "Login"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Login"
        Me.UserPanel.ResumeLayout(False)
        Me.UserPanel.PerformLayout()
        CType(Me.User, System.ComponentModel.ISupportInitialize).EndInit()
        Me.PWPanel.ResumeLayout(False)
        Me.PWPanel.PerformLayout()
        CType(Me.PW, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.Logo, System.ComponentModel.ISupportInitialize).EndInit()
        Me.LoginPanel.ResumeLayout(False)
        Me.LoginPanel.PerformLayout()
        Me.ResumeLayout(False)

    End Sub

    Friend WithEvents Label5 As Label
    Friend WithEvents Label4 As Label
    Friend WithEvents Label3 As Label
    Friend WithEvents txtUsername As TextBox
    Friend WithEvents btnLogin As Button
    Friend WithEvents UserPanel As Panel
    Friend WithEvents User As PictureBox
    Friend WithEvents Label1 As Label
    Friend WithEvents Label2 As Label
    Friend WithEvents PWPanel As Panel
    Friend WithEvents PW As PictureBox
    Friend WithEvents txtPassword As TextBox
    Friend WithEvents cboShowPW As CheckBox
    Friend WithEvents Logo As PictureBox
    Friend WithEvents btnExit As Button
    Friend WithEvents LoginPanel As Panel
    Friend WithEvents Label6 As Label
    Friend WithEvents Label7 As Label
    Friend WithEvents Label8 As Label
End Class
