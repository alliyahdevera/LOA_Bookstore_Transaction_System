<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmStudentManagement
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmStudentManagement))
        Me.Label15 = New System.Windows.Forms.Label()
        Me.btnclear = New System.Windows.Forms.Button()
        Me.Panel1 = New System.Windows.Forms.Panel()
        Me.PictureBox1 = New System.Windows.Forms.PictureBox()
        Me.txtSearch = New System.Windows.Forms.TextBox()
        Me.Panel13 = New System.Windows.Forms.Panel()
        Me.lblname = New System.Windows.Forms.Label()
        Me.lbldatetime = New System.Windows.Forms.Label()
        Me.Label29 = New System.Windows.Forms.Label()
        Me.Label27 = New System.Windows.Forms.Label()
        Me.Label28 = New System.Windows.Forms.Label()
        Me.lblposition = New System.Windows.Forms.Label()
        Me.btnremove = New System.Windows.Forms.Button()
        Me.btnupdate = New System.Windows.Forms.Button()
        Me.Label14 = New System.Windows.Forms.Label()
        Me.Panel6 = New System.Windows.Forms.Panel()
        Me.Label10 = New System.Windows.Forms.Label()
        Me.Panel7 = New System.Windows.Forms.Panel()
        Me.DataGridView1 = New System.Windows.Forms.DataGridView()
        Me.StudentNo = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.LastName = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.FirstName = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.GradeLevel = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.Section = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.Panel11 = New System.Windows.Forms.Panel()
        Me.Label9 = New System.Windows.Forms.Label()
        Me.Panel5 = New System.Windows.Forms.Panel()
        Me.cboGradeLevel = New System.Windows.Forms.ComboBox()
        Me.cboSection = New System.Windows.Forms.ComboBox()
        Me.txtfirstname = New System.Windows.Forms.TextBox()
        Me.Label12 = New System.Windows.Forms.Label()
        Me.btnadd = New System.Windows.Forms.Button()
        Me.Label11 = New System.Windows.Forms.Label()
        Me.txtstudentno = New System.Windows.Forms.TextBox()
        Me.txtlastname = New System.Windows.Forms.TextBox()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.Label8 = New System.Windows.Forms.Label()
        Me.Label6 = New System.Windows.Forms.Label()
        Me.Panel1.SuspendLayout()
        CType(Me.PictureBox1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.Panel13.SuspendLayout()
        Me.Panel6.SuspendLayout()
        Me.Panel7.SuspendLayout()
        CType(Me.DataGridView1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.Panel11.SuspendLayout()
        Me.Panel5.SuspendLayout()
        Me.SuspendLayout()
        '
        'Label15
        '
        Me.Label15.AutoSize = True
        Me.Label15.Font = New System.Drawing.Font("Segoe UI", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label15.ForeColor = System.Drawing.SystemColors.ControlDarkDark
        Me.Label15.Location = New System.Drawing.Point(27, 55)
        Me.Label15.Name = "Label15"
        Me.Label15.Size = New System.Drawing.Size(167, 15)
        Me.Label15.TabIndex = 109
        Me.Label15.Text = "View and Manage All Students"
        '
        'btnclear
        '
        Me.btnclear.BackColor = System.Drawing.Color.Gray
        Me.btnclear.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnclear.ForeColor = System.Drawing.Color.White
        Me.btnclear.Location = New System.Drawing.Point(965, 226)
        Me.btnclear.Name = "btnclear"
        Me.btnclear.Size = New System.Drawing.Size(172, 33)
        Me.btnclear.TabIndex = 106
        Me.btnclear.Text = "Clear"
        Me.btnclear.UseVisualStyleBackColor = False
        '
        'Panel1
        '
        Me.Panel1.BackColor = System.Drawing.Color.White
        Me.Panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.Panel1.Controls.Add(Me.PictureBox1)
        Me.Panel1.Controls.Add(Me.txtSearch)
        Me.Panel1.Location = New System.Drawing.Point(267, 293)
        Me.Panel1.Name = "Panel1"
        Me.Panel1.Size = New System.Drawing.Size(263, 26)
        Me.Panel1.TabIndex = 108
        '
        'PictureBox1
        '
        Me.PictureBox1.BackgroundImage = CType(resources.GetObject("PictureBox1.BackgroundImage"), System.Drawing.Image)
        Me.PictureBox1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch
        Me.PictureBox1.Location = New System.Drawing.Point(239, 3)
        Me.PictureBox1.Name = "PictureBox1"
        Me.PictureBox1.Size = New System.Drawing.Size(19, 19)
        Me.PictureBox1.TabIndex = 29
        Me.PictureBox1.TabStop = False
        '
        'txtSearch
        '
        Me.txtSearch.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.txtSearch.Font = New System.Drawing.Font("Segoe UI", 11.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtSearch.Location = New System.Drawing.Point(3, 3)
        Me.txtSearch.Name = "txtSearch"
        Me.txtSearch.Size = New System.Drawing.Size(230, 20)
        Me.txtSearch.TabIndex = 28
        '
        'Panel13
        '
        Me.Panel13.BackColor = System.Drawing.Color.White
        Me.Panel13.Controls.Add(Me.lblname)
        Me.Panel13.Controls.Add(Me.lbldatetime)
        Me.Panel13.Controls.Add(Me.Label29)
        Me.Panel13.Controls.Add(Me.Label27)
        Me.Panel13.Controls.Add(Me.Label28)
        Me.Panel13.Controls.Add(Me.lblposition)
        Me.Panel13.Location = New System.Drawing.Point(-1, 823)
        Me.Panel13.Name = "Panel13"
        Me.Panel13.Size = New System.Drawing.Size(1228, 27)
        Me.Panel13.TabIndex = 107
        '
        'lblname
        '
        Me.lblname.AutoSize = True
        Me.lblname.BackColor = System.Drawing.Color.Transparent
        Me.lblname.Font = New System.Drawing.Font("Segoe UI Semibold", 12.0!, CType((System.Drawing.FontStyle.Bold Or System.Drawing.FontStyle.Italic), System.Drawing.FontStyle), System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblname.ForeColor = System.Drawing.Color.FromArgb(CType(CType(1, Byte), Integer), CType(CType(21, Byte), Integer), CType(CType(78, Byte), Integer))
        Me.lblname.Location = New System.Drawing.Point(54, 3)
        Me.lblname.Name = "lblname"
        Me.lblname.Size = New System.Drawing.Size(53, 21)
        Me.lblname.TabIndex = 64
        Me.lblname.Text = "Name"
        '
        'lbldatetime
        '
        Me.lbldatetime.AutoSize = True
        Me.lbldatetime.BackColor = System.Drawing.Color.Transparent
        Me.lbldatetime.Font = New System.Drawing.Font("Segoe UI Semibold", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lbldatetime.ForeColor = System.Drawing.Color.FromArgb(CType(CType(1, Byte), Integer), CType(CType(21, Byte), Integer), CType(CType(78, Byte), Integer))
        Me.lbldatetime.Location = New System.Drawing.Point(576, 3)
        Me.lbldatetime.Name = "lbldatetime"
        Me.lbldatetime.Size = New System.Drawing.Size(16, 21)
        Me.lbldatetime.TabIndex = 68
        Me.lbldatetime.Text = "-"
        '
        'Label29
        '
        Me.Label29.AutoSize = True
        Me.Label29.BackColor = System.Drawing.Color.Transparent
        Me.Label29.Font = New System.Drawing.Font("Segoe UI", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label29.ForeColor = System.Drawing.Color.FromArgb(CType(CType(1, Byte), Integer), CType(CType(21, Byte), Integer), CType(CType(78, Byte), Integer))
        Me.Label29.Location = New System.Drawing.Point(4, 3)
        Me.Label29.Name = "Label29"
        Me.Label29.Size = New System.Drawing.Size(55, 21)
        Me.Label29.TabIndex = 63
        Me.Label29.Text = "Name:"
        '
        'Label27
        '
        Me.Label27.AutoSize = True
        Me.Label27.BackColor = System.Drawing.Color.Transparent
        Me.Label27.Font = New System.Drawing.Font("Segoe UI", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label27.ForeColor = System.Drawing.Color.FromArgb(CType(CType(1, Byte), Integer), CType(CType(21, Byte), Integer), CType(CType(78, Byte), Integer))
        Me.Label27.Location = New System.Drawing.Point(504, 3)
        Me.Label27.Name = "Label27"
        Me.Label27.Size = New System.Drawing.Size(65, 21)
        Me.Label27.TabIndex = 67
        Me.Label27.Text = "Today is"
        '
        'Label28
        '
        Me.Label28.AutoSize = True
        Me.Label28.BackColor = System.Drawing.Color.Transparent
        Me.Label28.Font = New System.Drawing.Font("Segoe UI", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label28.ForeColor = System.Drawing.Color.FromArgb(CType(CType(1, Byte), Integer), CType(CType(21, Byte), Integer), CType(CType(78, Byte), Integer))
        Me.Label28.Location = New System.Drawing.Point(249, 3)
        Me.Label28.Name = "Label28"
        Me.Label28.Size = New System.Drawing.Size(68, 21)
        Me.Label28.TabIndex = 65
        Me.Label28.Text = "Position:"
        '
        'lblposition
        '
        Me.lblposition.AutoSize = True
        Me.lblposition.BackColor = System.Drawing.Color.Transparent
        Me.lblposition.Font = New System.Drawing.Font("Segoe UI Semibold", 12.0!, CType((System.Drawing.FontStyle.Bold Or System.Drawing.FontStyle.Italic), System.Drawing.FontStyle), System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblposition.ForeColor = System.Drawing.Color.FromArgb(CType(CType(1, Byte), Integer), CType(CType(21, Byte), Integer), CType(CType(78, Byte), Integer))
        Me.lblposition.Location = New System.Drawing.Point(315, 3)
        Me.lblposition.Name = "lblposition"
        Me.lblposition.Size = New System.Drawing.Size(82, 21)
        Me.lblposition.TabIndex = 66
        Me.lblposition.Text = "NPosition"
        '
        'btnremove
        '
        Me.btnremove.BackColor = System.Drawing.Color.Maroon
        Me.btnremove.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnremove.ForeColor = System.Drawing.Color.White
        Me.btnremove.Location = New System.Drawing.Point(965, 179)
        Me.btnremove.Name = "btnremove"
        Me.btnremove.Size = New System.Drawing.Size(172, 33)
        Me.btnremove.TabIndex = 105
        Me.btnremove.Text = "Remove"
        Me.btnremove.UseVisualStyleBackColor = False
        '
        'btnupdate
        '
        Me.btnupdate.BackColor = System.Drawing.Color.Navy
        Me.btnupdate.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnupdate.ForeColor = System.Drawing.Color.White
        Me.btnupdate.Location = New System.Drawing.Point(965, 132)
        Me.btnupdate.Name = "btnupdate"
        Me.btnupdate.Size = New System.Drawing.Size(172, 33)
        Me.btnupdate.TabIndex = 104
        Me.btnupdate.Text = "Update"
        Me.btnupdate.UseVisualStyleBackColor = False
        '
        'Label14
        '
        Me.Label14.AutoSize = True
        Me.Label14.Font = New System.Drawing.Font("Segoe UI Semibold", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label14.Location = New System.Drawing.Point(28, 296)
        Me.Label14.Name = "Label14"
        Me.Label14.Size = New System.Drawing.Size(233, 17)
        Me.Label14.TabIndex = 103
        Me.Label14.Text = "Search by  Student No. or Last Name"
        '
        'Panel6
        '
        Me.Panel6.BackColor = System.Drawing.Color.FromArgb(CType(CType(1, Byte), Integer), CType(CType(21, Byte), Integer), CType(CType(78, Byte), Integer))
        Me.Panel6.Controls.Add(Me.Label10)
        Me.Panel6.Location = New System.Drawing.Point(30, 327)
        Me.Panel6.Name = "Panel6"
        Me.Panel6.Size = New System.Drawing.Size(1157, 35)
        Me.Panel6.TabIndex = 101
        '
        'Label10
        '
        Me.Label10.AutoSize = True
        Me.Label10.Font = New System.Drawing.Font("Segoe UI", 11.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label10.ForeColor = System.Drawing.Color.White
        Me.Label10.Location = New System.Drawing.Point(7, 8)
        Me.Label10.Name = "Label10"
        Me.Label10.Size = New System.Drawing.Size(96, 20)
        Me.Label10.TabIndex = 10
        Me.Label10.Text = "List of Users"
        '
        'Panel7
        '
        Me.Panel7.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.Panel7.Controls.Add(Me.DataGridView1)
        Me.Panel7.Location = New System.Drawing.Point(30, 327)
        Me.Panel7.Name = "Panel7"
        Me.Panel7.Size = New System.Drawing.Size(1157, 468)
        Me.Panel7.TabIndex = 102
        '
        'DataGridView1
        '
        Me.DataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.DataGridView1.Columns.AddRange(New System.Windows.Forms.DataGridViewColumn() {Me.StudentNo, Me.LastName, Me.FirstName, Me.GradeLevel, Me.Section})
        Me.DataGridView1.Location = New System.Drawing.Point(-1, 34)
        Me.DataGridView1.Name = "DataGridView1"
        Me.DataGridView1.ReadOnly = True
        Me.DataGridView1.Size = New System.Drawing.Size(1157, 433)
        Me.DataGridView1.TabIndex = 2
        '
        'StudentNo
        '
        Me.StudentNo.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill
        Me.StudentNo.HeaderText = "Student Number"
        Me.StudentNo.Name = "StudentNo"
        Me.StudentNo.ReadOnly = True
        '
        'LastName
        '
        Me.LastName.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill
        Me.LastName.HeaderText = "Last Name"
        Me.LastName.Name = "LastName"
        Me.LastName.ReadOnly = True
        '
        'FirstName
        '
        Me.FirstName.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill
        Me.FirstName.HeaderText = "First Name"
        Me.FirstName.Name = "FirstName"
        Me.FirstName.ReadOnly = True
        '
        'GradeLevel
        '
        Me.GradeLevel.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill
        Me.GradeLevel.HeaderText = "Grade Level"
        Me.GradeLevel.Name = "GradeLevel"
        Me.GradeLevel.ReadOnly = True
        '
        'Section
        '
        Me.Section.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill
        Me.Section.HeaderText = "Section"
        Me.Section.Name = "Section"
        Me.Section.ReadOnly = True
        '
        'Panel11
        '
        Me.Panel11.BackColor = System.Drawing.Color.FromArgb(CType(CType(1, Byte), Integer), CType(CType(21, Byte), Integer), CType(CType(78, Byte), Integer))
        Me.Panel11.Controls.Add(Me.Label9)
        Me.Panel11.Location = New System.Drawing.Point(30, 91)
        Me.Panel11.Name = "Panel11"
        Me.Panel11.Size = New System.Drawing.Size(860, 35)
        Me.Panel11.TabIndex = 99
        '
        'Label9
        '
        Me.Label9.AutoSize = True
        Me.Label9.Font = New System.Drawing.Font("Segoe UI", 11.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label9.ForeColor = System.Drawing.Color.White
        Me.Label9.Location = New System.Drawing.Point(8, 8)
        Me.Label9.Name = "Label9"
        Me.Label9.Size = New System.Drawing.Size(153, 20)
        Me.Label9.TabIndex = 10
        Me.Label9.Text = "Student Information"
        '
        'Panel5
        '
        Me.Panel5.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.Panel5.Controls.Add(Me.cboGradeLevel)
        Me.Panel5.Controls.Add(Me.cboSection)
        Me.Panel5.Controls.Add(Me.txtfirstname)
        Me.Panel5.Controls.Add(Me.Label12)
        Me.Panel5.Controls.Add(Me.btnadd)
        Me.Panel5.Controls.Add(Me.Label11)
        Me.Panel5.Controls.Add(Me.txtstudentno)
        Me.Panel5.Controls.Add(Me.txtlastname)
        Me.Panel5.Controls.Add(Me.Label2)
        Me.Panel5.Controls.Add(Me.Label1)
        Me.Panel5.Controls.Add(Me.Label8)
        Me.Panel5.Location = New System.Drawing.Point(30, 91)
        Me.Panel5.Name = "Panel5"
        Me.Panel5.Size = New System.Drawing.Size(860, 182)
        Me.Panel5.TabIndex = 100
        '
        'cboGradeLevel
        '
        Me.cboGradeLevel.Font = New System.Drawing.Font("Segoe UI", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cboGradeLevel.FormattingEnabled = True
        Me.cboGradeLevel.Items.AddRange(New Object() {"Grade 1", "Grade 2", "Grade 3", "Grade 4", "Grade 5", "Grade 6", "Grade 7", "Grade 8", "Grade 9", "Grade 10", "Grade 11", "Grade 12", "1st Year College", "2nd Year College", "3rd Year College", "4th Year College"})
        Me.cboGradeLevel.Location = New System.Drawing.Point(556, 55)
        Me.cboGradeLevel.Name = "cboGradeLevel"
        Me.cboGradeLevel.Size = New System.Drawing.Size(270, 25)
        Me.cboGradeLevel.TabIndex = 67
        '
        'cboSection
        '
        Me.cboSection.Font = New System.Drawing.Font("Segoe UI", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cboSection.FormattingEnabled = True
        Me.cboSection.Location = New System.Drawing.Point(556, 95)
        Me.cboSection.Name = "cboSection"
        Me.cboSection.Size = New System.Drawing.Size(270, 25)
        Me.cboSection.TabIndex = 66
        '
        'txtfirstname
        '
        Me.txtfirstname.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.txtfirstname.Font = New System.Drawing.Font("Segoe UI", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtfirstname.Location = New System.Drawing.Point(160, 131)
        Me.txtfirstname.Name = "txtfirstname"
        Me.txtfirstname.Size = New System.Drawing.Size(274, 25)
        Me.txtfirstname.TabIndex = 54
        '
        'Label12
        '
        Me.Label12.AutoSize = True
        Me.Label12.Font = New System.Drawing.Font("Segoe UI Semibold", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label12.Location = New System.Drawing.Point(29, 133)
        Me.Label12.Name = "Label12"
        Me.Label12.Size = New System.Drawing.Size(74, 17)
        Me.Label12.TabIndex = 55
        Me.Label12.Text = "First Name"
        '
        'btnadd
        '
        Me.btnadd.BackColor = System.Drawing.Color.Green
        Me.btnadd.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnadd.ForeColor = System.Drawing.Color.White
        Me.btnadd.Location = New System.Drawing.Point(556, 133)
        Me.btnadd.Name = "btnadd"
        Me.btnadd.Size = New System.Drawing.Size(270, 26)
        Me.btnadd.TabIndex = 64
        Me.btnadd.Text = "Add"
        Me.btnadd.UseVisualStyleBackColor = False
        '
        'Label11
        '
        Me.Label11.AutoSize = True
        Me.Label11.Font = New System.Drawing.Font("Segoe UI Semibold", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label11.Location = New System.Drawing.Point(465, 57)
        Me.Label11.Name = "Label11"
        Me.Label11.Size = New System.Drawing.Size(78, 17)
        Me.Label11.TabIndex = 53
        Me.Label11.Text = "Grade Level"
        '
        'txtstudentno
        '
        Me.txtstudentno.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.txtstudentno.Font = New System.Drawing.Font("Segoe UI", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtstudentno.Location = New System.Drawing.Point(160, 55)
        Me.txtstudentno.Name = "txtstudentno"
        Me.txtstudentno.Size = New System.Drawing.Size(274, 25)
        Me.txtstudentno.TabIndex = 42
        '
        'txtlastname
        '
        Me.txtlastname.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.txtlastname.Font = New System.Drawing.Font("Segoe UI", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtlastname.Location = New System.Drawing.Point(160, 93)
        Me.txtlastname.Name = "txtlastname"
        Me.txtlastname.Size = New System.Drawing.Size(274, 25)
        Me.txtlastname.TabIndex = 43
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Font = New System.Drawing.Font("Segoe UI Semibold", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label2.Location = New System.Drawing.Point(465, 95)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(52, 17)
        Me.Label2.TabIndex = 49
        Me.Label2.Text = "Section"
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Font = New System.Drawing.Font("Segoe UI Semibold", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label1.Location = New System.Drawing.Point(29, 57)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(81, 17)
        Me.Label1.TabIndex = 45
        Me.Label1.Text = "Student No."
        '
        'Label8
        '
        Me.Label8.AutoSize = True
        Me.Label8.Font = New System.Drawing.Font("Segoe UI Semibold", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label8.Location = New System.Drawing.Point(29, 95)
        Me.Label8.Name = "Label8"
        Me.Label8.Size = New System.Drawing.Size(72, 17)
        Me.Label8.TabIndex = 46
        Me.Label8.Text = "Last Name"
        '
        'Label6
        '
        Me.Label6.AutoSize = True
        Me.Label6.Font = New System.Drawing.Font("Segoe UI", 18.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label6.ForeColor = System.Drawing.Color.FromArgb(CType(CType(1, Byte), Integer), CType(CType(21, Byte), Integer), CType(CType(78, Byte), Integer))
        Me.Label6.Location = New System.Drawing.Point(24, 20)
        Me.Label6.Name = "Label6"
        Me.Label6.Size = New System.Drawing.Size(260, 32)
        Me.Label6.TabIndex = 98
        Me.Label6.Text = "Student Management"
        '
        'frmStudentManagement
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(1220, 850)
        Me.Controls.Add(Me.Label15)
        Me.Controls.Add(Me.btnclear)
        Me.Controls.Add(Me.Panel1)
        Me.Controls.Add(Me.Panel13)
        Me.Controls.Add(Me.btnremove)
        Me.Controls.Add(Me.btnupdate)
        Me.Controls.Add(Me.Label14)
        Me.Controls.Add(Me.Panel6)
        Me.Controls.Add(Me.Panel7)
        Me.Controls.Add(Me.Panel11)
        Me.Controls.Add(Me.Panel5)
        Me.Controls.Add(Me.Label6)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None
        Me.Name = "frmStudentManagement"
        Me.Text = "frmStudentManagement"
        Me.Panel1.ResumeLayout(False)
        Me.Panel1.PerformLayout()
        CType(Me.PictureBox1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.Panel13.ResumeLayout(False)
        Me.Panel13.PerformLayout()
        Me.Panel6.ResumeLayout(False)
        Me.Panel6.PerformLayout()
        Me.Panel7.ResumeLayout(False)
        CType(Me.DataGridView1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.Panel11.ResumeLayout(False)
        Me.Panel11.PerformLayout()
        Me.Panel5.ResumeLayout(False)
        Me.Panel5.PerformLayout()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

    Friend WithEvents Label15 As Label
    Friend WithEvents btnclear As Button
    Friend WithEvents Panel1 As Panel
    Friend WithEvents PictureBox1 As PictureBox
    Friend WithEvents txtSearch As TextBox
    Friend WithEvents Panel13 As Panel
    Friend WithEvents lblname As Label
    Friend WithEvents lbldatetime As Label
    Friend WithEvents Label29 As Label
    Friend WithEvents Label27 As Label
    Friend WithEvents Label28 As Label
    Friend WithEvents lblposition As Label
    Friend WithEvents btnremove As Button
    Friend WithEvents btnupdate As Button
    Friend WithEvents Label14 As Label
    Friend WithEvents Panel6 As Panel
    Friend WithEvents Label10 As Label
    Friend WithEvents Panel7 As Panel
    Friend WithEvents DataGridView1 As DataGridView
    Friend WithEvents Panel11 As Panel
    Friend WithEvents Label9 As Label
    Friend WithEvents Panel5 As Panel
    Friend WithEvents cboSection As ComboBox
    Friend WithEvents txtfirstname As TextBox
    Friend WithEvents Label12 As Label
    Friend WithEvents btnadd As Button
    Friend WithEvents Label11 As Label
    Friend WithEvents txtstudentno As TextBox
    Friend WithEvents txtlastname As TextBox
    Friend WithEvents Label2 As Label
    Friend WithEvents Label1 As Label
    Friend WithEvents Label8 As Label
    Friend WithEvents Label6 As Label
    Friend WithEvents cboGradeLevel As ComboBox
    Friend WithEvents StudentNo As DataGridViewTextBoxColumn
    Friend WithEvents LastName As DataGridViewTextBoxColumn
    Friend WithEvents FirstName As DataGridViewTextBoxColumn
    Friend WithEvents GradeLevel As DataGridViewTextBoxColumn
    Friend WithEvents Section As DataGridViewTextBoxColumn
End Class
