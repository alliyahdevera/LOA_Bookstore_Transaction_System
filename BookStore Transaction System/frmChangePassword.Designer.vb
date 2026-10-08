<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmChangePassword
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmChangePassword))
        Me.Panel3 = New System.Windows.Forms.Panel()
        Me.PictureBox2 = New System.Windows.Forms.PictureBox()
        Me.txtold = New System.Windows.Forms.TextBox()
        Me.Panel2 = New System.Windows.Forms.Panel()
        Me.btnnewp = New System.Windows.Forms.Button()
        Me.PictureBox1 = New System.Windows.Forms.PictureBox()
        Me.txtnew = New System.Windows.Forms.TextBox()
        Me.Label4 = New System.Windows.Forms.Label()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.PWPanel = New System.Windows.Forms.Panel()
        Me.btncshow = New System.Windows.Forms.Button()
        Me.PW = New System.Windows.Forms.PictureBox()
        Me.txtconfirm = New System.Windows.Forms.TextBox()
        Me.btnLogin = New System.Windows.Forms.Button()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.Panel1 = New System.Windows.Forms.Panel()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.Panel3.SuspendLayout()
        CType(Me.PictureBox2, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.Panel2.SuspendLayout()
        CType(Me.PictureBox1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.PWPanel.SuspendLayout()
        CType(Me.PW, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.Panel1.SuspendLayout()
        Me.SuspendLayout()
        '
        'Panel3
        '
        Me.Panel3.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.Panel3.Controls.Add(Me.PictureBox2)
        Me.Panel3.Controls.Add(Me.txtold)
        Me.Panel3.Location = New System.Drawing.Point(22, 82)
        Me.Panel3.Name = "Panel3"
        Me.Panel3.Size = New System.Drawing.Size(257, 35)
        Me.Panel3.TabIndex = 60
        '
        'PictureBox2
        '
        Me.PictureBox2.BackgroundImage = CType(resources.GetObject("PictureBox2.BackgroundImage"), System.Drawing.Image)
        Me.PictureBox2.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch
        Me.PictureBox2.Location = New System.Drawing.Point(5, 6)
        Me.PictureBox2.Name = "PictureBox2"
        Me.PictureBox2.Size = New System.Drawing.Size(20, 20)
        Me.PictureBox2.TabIndex = 11
        Me.PictureBox2.TabStop = False
        '
        'txtold
        '
        Me.txtold.BackColor = System.Drawing.SystemColors.Control
        Me.txtold.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.txtold.Font = New System.Drawing.Font("Segoe UI", 12.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtold.Location = New System.Drawing.Point(28, 5)
        Me.txtold.Name = "txtold"
        Me.txtold.PasswordChar = Global.Microsoft.VisualBasic.ChrW(42)
        Me.txtold.Size = New System.Drawing.Size(224, 23)
        Me.txtold.TabIndex = 10
        '
        'Panel2
        '
        Me.Panel2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.Panel2.Controls.Add(Me.btnnewp)
        Me.Panel2.Controls.Add(Me.PictureBox1)
        Me.Panel2.Controls.Add(Me.txtnew)
        Me.Panel2.Location = New System.Drawing.Point(22, 158)
        Me.Panel2.Name = "Panel2"
        Me.Panel2.Size = New System.Drawing.Size(257, 35)
        Me.Panel2.TabIndex = 57
        '
        'btnnewp
        '
        Me.btnnewp.BackgroundImage = CType(resources.GetObject("btnnewp.BackgroundImage"), System.Drawing.Image)
        Me.btnnewp.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch
        Me.btnnewp.FlatAppearance.BorderSize = 0
        Me.btnnewp.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnnewp.Font = New System.Drawing.Font("Segoe UI", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnnewp.Location = New System.Drawing.Point(229, 10)
        Me.btnnewp.Name = "btnnewp"
        Me.btnnewp.Size = New System.Drawing.Size(20, 12)
        Me.btnnewp.TabIndex = 12
        Me.btnnewp.TextAlign = System.Drawing.ContentAlignment.TopCenter
        Me.btnnewp.UseVisualStyleBackColor = True
        '
        'PictureBox1
        '
        Me.PictureBox1.BackgroundImage = CType(resources.GetObject("PictureBox1.BackgroundImage"), System.Drawing.Image)
        Me.PictureBox1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch
        Me.PictureBox1.Location = New System.Drawing.Point(5, 6)
        Me.PictureBox1.Name = "PictureBox1"
        Me.PictureBox1.Size = New System.Drawing.Size(20, 20)
        Me.PictureBox1.TabIndex = 11
        Me.PictureBox1.TabStop = False
        '
        'txtnew
        '
        Me.txtnew.BackColor = System.Drawing.SystemColors.Control
        Me.txtnew.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.txtnew.Font = New System.Drawing.Font("Segoe UI", 12.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtnew.Location = New System.Drawing.Point(28, 5)
        Me.txtnew.Name = "txtnew"
        Me.txtnew.PasswordChar = Global.Microsoft.VisualBasic.ChrW(42)
        Me.txtnew.Size = New System.Drawing.Size(195, 23)
        Me.txtnew.TabIndex = 10
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.Font = New System.Drawing.Font("Segoe UI", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label4.Location = New System.Drawing.Point(19, 58)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(89, 17)
        Me.Label4.TabIndex = 59
        Me.Label4.Text = "Old Password"
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Font = New System.Drawing.Font("Segoe UI", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label3.Location = New System.Drawing.Point(19, 212)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(114, 17)
        Me.Label3.TabIndex = 58
        Me.Label3.Text = "Confirm Password"
        '
        'PWPanel
        '
        Me.PWPanel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.PWPanel.Controls.Add(Me.btncshow)
        Me.PWPanel.Controls.Add(Me.PW)
        Me.PWPanel.Controls.Add(Me.txtconfirm)
        Me.PWPanel.Location = New System.Drawing.Point(22, 236)
        Me.PWPanel.Name = "PWPanel"
        Me.PWPanel.Size = New System.Drawing.Size(257, 35)
        Me.PWPanel.TabIndex = 56
        '
        'btncshow
        '
        Me.btncshow.BackgroundImage = CType(resources.GetObject("btncshow.BackgroundImage"), System.Drawing.Image)
        Me.btncshow.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch
        Me.btncshow.FlatAppearance.BorderSize = 0
        Me.btncshow.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btncshow.Font = New System.Drawing.Font("Segoe UI", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btncshow.Location = New System.Drawing.Point(229, 10)
        Me.btncshow.Name = "btncshow"
        Me.btncshow.Size = New System.Drawing.Size(20, 12)
        Me.btncshow.TabIndex = 12
        Me.btncshow.TextAlign = System.Drawing.ContentAlignment.TopCenter
        Me.btncshow.UseVisualStyleBackColor = True
        '
        'PW
        '
        Me.PW.BackgroundImage = CType(resources.GetObject("PW.BackgroundImage"), System.Drawing.Image)
        Me.PW.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch
        Me.PW.Location = New System.Drawing.Point(5, 6)
        Me.PW.Name = "PW"
        Me.PW.Size = New System.Drawing.Size(20, 20)
        Me.PW.TabIndex = 11
        Me.PW.TabStop = False
        '
        'txtconfirm
        '
        Me.txtconfirm.BackColor = System.Drawing.SystemColors.Control
        Me.txtconfirm.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.txtconfirm.Font = New System.Drawing.Font("Segoe UI", 12.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtconfirm.Location = New System.Drawing.Point(28, 5)
        Me.txtconfirm.Name = "txtconfirm"
        Me.txtconfirm.PasswordChar = Global.Microsoft.VisualBasic.ChrW(42)
        Me.txtconfirm.Size = New System.Drawing.Size(195, 23)
        Me.txtconfirm.TabIndex = 10
        '
        'btnLogin
        '
        Me.btnLogin.BackColor = System.Drawing.Color.FromArgb(CType(CType(1, Byte), Integer), CType(CType(21, Byte), Integer), CType(CType(78, Byte), Integer))
        Me.btnLogin.FlatAppearance.BorderSize = 0
        Me.btnLogin.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnLogin.Font = New System.Drawing.Font("Segoe UI Semibold", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnLogin.ForeColor = System.Drawing.Color.White
        Me.btnLogin.Location = New System.Drawing.Point(189, 288)
        Me.btnLogin.Name = "btnLogin"
        Me.btnLogin.Size = New System.Drawing.Size(90, 26)
        Me.btnLogin.TabIndex = 55
        Me.btnLogin.Text = "Update"
        Me.btnLogin.UseVisualStyleBackColor = False
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Font = New System.Drawing.Font("Segoe UI", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label2.Location = New System.Drawing.Point(19, 134)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(94, 17)
        Me.Label2.TabIndex = 54
        Me.Label2.Text = "New Password"
        '
        'Panel1
        '
        Me.Panel1.BackColor = System.Drawing.Color.FromArgb(CType(CType(1, Byte), Integer), CType(CType(21, Byte), Integer), CType(CType(78, Byte), Integer))
        Me.Panel1.Controls.Add(Me.Label1)
        Me.Panel1.Dock = System.Windows.Forms.DockStyle.Top
        Me.Panel1.Location = New System.Drawing.Point(0, 0)
        Me.Panel1.Name = "Panel1"
        Me.Panel1.Size = New System.Drawing.Size(301, 42)
        Me.Panel1.TabIndex = 53
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Font = New System.Drawing.Font("Segoe UI", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label1.ForeColor = System.Drawing.Color.White
        Me.Label1.Location = New System.Drawing.Point(9, 11)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(168, 21)
        Me.Label1.TabIndex = 0
        Me.Label1.Text = "CHANGE PASSWORD"
        '
        'frmChangePassword
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(301, 339)
        Me.Controls.Add(Me.Panel3)
        Me.Controls.Add(Me.Panel2)
        Me.Controls.Add(Me.Label4)
        Me.Controls.Add(Me.Label3)
        Me.Controls.Add(Me.PWPanel)
        Me.Controls.Add(Me.btnLogin)
        Me.Controls.Add(Me.Label2)
        Me.Controls.Add(Me.Panel1)
        Me.Name = "frmChangePassword"
        Me.Text = "frmChangePassword"
        Me.Panel3.ResumeLayout(False)
        Me.Panel3.PerformLayout()
        CType(Me.PictureBox2, System.ComponentModel.ISupportInitialize).EndInit()
        Me.Panel2.ResumeLayout(False)
        Me.Panel2.PerformLayout()
        CType(Me.PictureBox1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.PWPanel.ResumeLayout(False)
        Me.PWPanel.PerformLayout()
        CType(Me.PW, System.ComponentModel.ISupportInitialize).EndInit()
        Me.Panel1.ResumeLayout(False)
        Me.Panel1.PerformLayout()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

    Friend WithEvents Panel3 As Panel
    Friend WithEvents PictureBox2 As PictureBox
    Friend WithEvents txtold As TextBox
    Friend WithEvents Panel2 As Panel
    Friend WithEvents btnnewp As Button
    Friend WithEvents PictureBox1 As PictureBox
    Friend WithEvents txtnew As TextBox
    Friend WithEvents Label4 As Label
    Friend WithEvents Label3 As Label
    Friend WithEvents PWPanel As Panel
    Friend WithEvents btncshow As Button
    Friend WithEvents PW As PictureBox
    Friend WithEvents txtconfirm As TextBox
    Friend WithEvents btnLogin As Button
    Friend WithEvents Label2 As Label
    Friend WithEvents Panel1 As Panel
    Friend WithEvents Label1 As Label
End Class
