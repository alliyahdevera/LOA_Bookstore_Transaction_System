<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmPayment
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmPayment))
        Me.Label17 = New System.Windows.Forms.Label()
        Me.Panel12 = New System.Windows.Forms.Panel()
        Me.Label21 = New System.Windows.Forms.Label()
        Me.Label18 = New System.Windows.Forms.Label()
        Me.Label20 = New System.Windows.Forms.Label()
        Me.cboPaymentMethod = New System.Windows.Forms.ComboBox()
        Me.txtChange = New System.Windows.Forms.TextBox()
        Me.txtAmountReceived = New System.Windows.Forms.TextBox()
        Me.txtGrandTotal = New System.Windows.Forms.TextBox()
        Me.Label19 = New System.Windows.Forms.Label()
        Me.txtORNo = New System.Windows.Forms.TextBox()
        Me.Label23 = New System.Windows.Forms.Label()
        Me.btnSave = New System.Windows.Forms.Button()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.dtpORDate = New System.Windows.Forms.DateTimePicker()
        Me.PrintPreviewDialog1 = New System.Windows.Forms.PrintPreviewDialog()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.txtEmployeeName = New System.Windows.Forms.TextBox()
        Me.btnSearchEmployee = New System.Windows.Forms.Button()
        Me.Panel12.SuspendLayout()
        Me.SuspendLayout()
        '
        'Label17
        '
        Me.Label17.AutoSize = True
        Me.Label17.Font = New System.Drawing.Font("Segoe UI Semibold", 11.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label17.Location = New System.Drawing.Point(35, 392)
        Me.Label17.Name = "Label17"
        Me.Label17.Size = New System.Drawing.Size(61, 20)
        Me.Label17.TabIndex = 25
        Me.Label17.Text = "Change"
        '
        'Panel12
        '
        Me.Panel12.BackColor = System.Drawing.Color.FromArgb(CType(CType(1, Byte), Integer), CType(CType(21, Byte), Integer), CType(CType(78, Byte), Integer))
        Me.Panel12.Controls.Add(Me.Label21)
        Me.Panel12.Dock = System.Windows.Forms.DockStyle.Top
        Me.Panel12.Location = New System.Drawing.Point(0, 0)
        Me.Panel12.Name = "Panel12"
        Me.Panel12.Size = New System.Drawing.Size(451, 54)
        Me.Panel12.TabIndex = 26
        '
        'Label21
        '
        Me.Label21.AutoSize = True
        Me.Label21.Font = New System.Drawing.Font("Segoe UI", 14.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label21.ForeColor = System.Drawing.Color.White
        Me.Label21.Location = New System.Drawing.Point(14, 14)
        Me.Label21.Name = "Label21"
        Me.Label21.Size = New System.Drawing.Size(169, 25)
        Me.Label21.TabIndex = 9
        Me.Label21.Text = "SETTLE PAYMENT"
        '
        'Label18
        '
        Me.Label18.AutoSize = True
        Me.Label18.Font = New System.Drawing.Font("Segoe UI Semibold", 11.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label18.Location = New System.Drawing.Point(35, 337)
        Me.Label18.Name = "Label18"
        Me.Label18.Size = New System.Drawing.Size(129, 20)
        Me.Label18.TabIndex = 24
        Me.Label18.Text = "Amount Received"
        '
        'Label20
        '
        Me.Label20.AutoSize = True
        Me.Label20.Font = New System.Drawing.Font("Segoe UI Semibold", 11.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label20.Location = New System.Drawing.Point(35, 174)
        Me.Label20.Name = "Label20"
        Me.Label20.Size = New System.Drawing.Size(127, 20)
        Me.Label20.TabIndex = 22
        Me.Label20.Text = "Payment Method"
        '
        'cboPaymentMethod
        '
        Me.cboPaymentMethod.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.cboPaymentMethod.Font = New System.Drawing.Font("Segoe UI", 11.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cboPaymentMethod.FormattingEnabled = True
        Me.cboPaymentMethod.Items.AddRange(New Object() {"Cash", "Employee's Salary"})
        Me.cboPaymentMethod.Location = New System.Drawing.Point(177, 172)
        Me.cboPaymentMethod.Name = "cboPaymentMethod"
        Me.cboPaymentMethod.Size = New System.Drawing.Size(241, 28)
        Me.cboPaymentMethod.TabIndex = 20
        '
        'txtChange
        '
        Me.txtChange.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.txtChange.Font = New System.Drawing.Font("Segoe UI", 11.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtChange.Location = New System.Drawing.Point(177, 389)
        Me.txtChange.Name = "txtChange"
        Me.txtChange.ReadOnly = True
        Me.txtChange.Size = New System.Drawing.Size(241, 27)
        Me.txtChange.TabIndex = 17
        '
        'txtAmountReceived
        '
        Me.txtAmountReceived.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.txtAmountReceived.Font = New System.Drawing.Font("Segoe UI", 11.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtAmountReceived.Location = New System.Drawing.Point(177, 334)
        Me.txtAmountReceived.Name = "txtAmountReceived"
        Me.txtAmountReceived.Size = New System.Drawing.Size(241, 27)
        Me.txtAmountReceived.TabIndex = 16
        '
        'txtGrandTotal
        '
        Me.txtGrandTotal.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.txtGrandTotal.Font = New System.Drawing.Font("Segoe UI", 11.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtGrandTotal.Location = New System.Drawing.Point(177, 280)
        Me.txtGrandTotal.Name = "txtGrandTotal"
        Me.txtGrandTotal.ReadOnly = True
        Me.txtGrandTotal.Size = New System.Drawing.Size(241, 27)
        Me.txtGrandTotal.TabIndex = 15
        '
        'Label19
        '
        Me.Label19.AutoSize = True
        Me.Label19.Font = New System.Drawing.Font("Segoe UI Semibold", 11.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label19.Location = New System.Drawing.Point(35, 284)
        Me.Label19.Name = "Label19"
        Me.Label19.Size = New System.Drawing.Size(88, 20)
        Me.Label19.TabIndex = 23
        Me.Label19.Text = "Grand Total"
        '
        'txtORNo
        '
        Me.txtORNo.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.txtORNo.Font = New System.Drawing.Font("Segoe UI", 11.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtORNo.Location = New System.Drawing.Point(177, 79)
        Me.txtORNo.Name = "txtORNo"
        Me.txtORNo.ReadOnly = True
        Me.txtORNo.Size = New System.Drawing.Size(241, 27)
        Me.txtORNo.TabIndex = 32
        '
        'Label23
        '
        Me.Label23.AutoSize = True
        Me.Label23.Font = New System.Drawing.Font("Segoe UI Semibold", 11.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label23.Location = New System.Drawing.Point(36, 80)
        Me.Label23.Name = "Label23"
        Me.Label23.Size = New System.Drawing.Size(58, 20)
        Me.Label23.TabIndex = 33
        Me.Label23.Text = "OR No."
        '
        'btnSave
        '
        Me.btnSave.BackColor = System.Drawing.Color.FromArgb(CType(CType(1, Byte), Integer), CType(CType(21, Byte), Integer), CType(CType(78, Byte), Integer))
        Me.btnSave.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnSave.ForeColor = System.Drawing.Color.White
        Me.btnSave.Location = New System.Drawing.Point(39, 448)
        Me.btnSave.Name = "btnSave"
        Me.btnSave.Size = New System.Drawing.Size(379, 37)
        Me.btnSave.TabIndex = 34
        Me.btnSave.Text = "Add Payment"
        Me.btnSave.UseVisualStyleBackColor = False
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Font = New System.Drawing.Font("Segoe UI Semibold", 11.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label1.Location = New System.Drawing.Point(35, 127)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(65, 20)
        Me.Label1.TabIndex = 35
        Me.Label1.Text = "OR Date"
        '
        'dtpORDate
        '
        Me.dtpORDate.Font = New System.Drawing.Font("Segoe UI", 11.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.dtpORDate.Location = New System.Drawing.Point(176, 127)
        Me.dtpORDate.Name = "dtpORDate"
        Me.dtpORDate.Size = New System.Drawing.Size(242, 27)
        Me.dtpORDate.TabIndex = 36
        '
        'PrintPreviewDialog1
        '
        Me.PrintPreviewDialog1.AutoScrollMargin = New System.Drawing.Size(0, 0)
        Me.PrintPreviewDialog1.AutoScrollMinSize = New System.Drawing.Size(0, 0)
        Me.PrintPreviewDialog1.ClientSize = New System.Drawing.Size(400, 300)
        Me.PrintPreviewDialog1.Enabled = True
        Me.PrintPreviewDialog1.Icon = CType(resources.GetObject("PrintPreviewDialog1.Icon"), System.Drawing.Icon)
        Me.PrintPreviewDialog1.Name = "PrintPreviewDialog1"
        Me.PrintPreviewDialog1.Visible = False
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Font = New System.Drawing.Font("Segoe UI Semibold", 11.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label2.Location = New System.Drawing.Point(35, 228)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(121, 20)
        Me.Label2.TabIndex = 38
        Me.Label2.Text = "Employee Name"
        '
        'txtEmployeeName
        '
        Me.txtEmployeeName.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.txtEmployeeName.Font = New System.Drawing.Font("Segoe UI", 11.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtEmployeeName.Location = New System.Drawing.Point(177, 225)
        Me.txtEmployeeName.Name = "txtEmployeeName"
        Me.txtEmployeeName.ReadOnly = True
        Me.txtEmployeeName.Size = New System.Drawing.Size(155, 27)
        Me.txtEmployeeName.TabIndex = 37
        '
        'btnSearchEmployee
        '
        Me.btnSearchEmployee.BackColor = System.Drawing.Color.FromArgb(CType(CType(1, Byte), Integer), CType(CType(21, Byte), Integer), CType(CType(78, Byte), Integer))
        Me.btnSearchEmployee.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnSearchEmployee.Font = New System.Drawing.Font("Segoe UI Semibold", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnSearchEmployee.ForeColor = System.Drawing.Color.White
        Me.btnSearchEmployee.Location = New System.Drawing.Point(338, 223)
        Me.btnSearchEmployee.Name = "btnSearchEmployee"
        Me.btnSearchEmployee.Size = New System.Drawing.Size(80, 29)
        Me.btnSearchEmployee.TabIndex = 120
        Me.btnSearchEmployee.Text = "Search"
        Me.btnSearchEmployee.UseVisualStyleBackColor = False
        '
        'frmPayment
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(451, 511)
        Me.Controls.Add(Me.btnSearchEmployee)
        Me.Controls.Add(Me.Label2)
        Me.Controls.Add(Me.txtEmployeeName)
        Me.Controls.Add(Me.dtpORDate)
        Me.Controls.Add(Me.Label1)
        Me.Controls.Add(Me.btnSave)
        Me.Controls.Add(Me.txtORNo)
        Me.Controls.Add(Me.Label23)
        Me.Controls.Add(Me.Panel12)
        Me.Controls.Add(Me.txtChange)
        Me.Controls.Add(Me.Label17)
        Me.Controls.Add(Me.Label19)
        Me.Controls.Add(Me.txtGrandTotal)
        Me.Controls.Add(Me.Label18)
        Me.Controls.Add(Me.txtAmountReceived)
        Me.Controls.Add(Me.Label20)
        Me.Controls.Add(Me.cboPaymentMethod)
        Me.Name = "frmPayment"
        Me.Text = "frmPayment"
        Me.Panel12.ResumeLayout(False)
        Me.Panel12.PerformLayout()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents Label17 As Label
    Friend WithEvents Panel12 As Panel
    Friend WithEvents Label21 As Label
    Friend WithEvents Label18 As Label
    Friend WithEvents Label20 As Label
    Friend WithEvents cboPaymentMethod As ComboBox
    Friend WithEvents txtChange As TextBox
    Friend WithEvents txtAmountReceived As TextBox
    Friend WithEvents txtGrandTotal As TextBox
    Friend WithEvents Label19 As Label
    Friend WithEvents txtORNo As TextBox
    Friend WithEvents Label23 As Label
    Friend WithEvents btnSave As Button
    Friend WithEvents Label1 As Label
    Friend WithEvents dtpORDate As DateTimePicker
    Friend WithEvents PrintPreviewDialog1 As PrintPreviewDialog
    Friend WithEvents Label2 As Label
    Friend WithEvents txtEmployeeName As TextBox
    Friend WithEvents btnSearchEmployee As Button
End Class
