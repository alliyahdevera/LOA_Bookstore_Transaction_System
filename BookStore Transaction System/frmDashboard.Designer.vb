<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class frmDashboard
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
        Dim ChartArea1 As System.Windows.Forms.DataVisualization.Charting.ChartArea = New System.Windows.Forms.DataVisualization.Charting.ChartArea()
        Dim Legend1 As System.Windows.Forms.DataVisualization.Charting.Legend = New System.Windows.Forms.DataVisualization.Charting.Legend()
        Dim Series1 As System.Windows.Forms.DataVisualization.Charting.Series = New System.Windows.Forms.DataVisualization.Charting.Series()
        Dim ChartArea2 As System.Windows.Forms.DataVisualization.Charting.ChartArea = New System.Windows.Forms.DataVisualization.Charting.ChartArea()
        Dim Legend2 As System.Windows.Forms.DataVisualization.Charting.Legend = New System.Windows.Forms.DataVisualization.Charting.Legend()
        Dim Series2 As System.Windows.Forms.DataVisualization.Charting.Series = New System.Windows.Forms.DataVisualization.Charting.Series()
        Dim ChartArea3 As System.Windows.Forms.DataVisualization.Charting.ChartArea = New System.Windows.Forms.DataVisualization.Charting.ChartArea()
        Dim Legend3 As System.Windows.Forms.DataVisualization.Charting.Legend = New System.Windows.Forms.DataVisualization.Charting.Legend()
        Dim Series3 As System.Windows.Forms.DataVisualization.Charting.Series = New System.Windows.Forms.DataVisualization.Charting.Series()
        Dim ChartArea4 As System.Windows.Forms.DataVisualization.Charting.ChartArea = New System.Windows.Forms.DataVisualization.Charting.ChartArea()
        Dim Legend4 As System.Windows.Forms.DataVisualization.Charting.Legend = New System.Windows.Forms.DataVisualization.Charting.Legend()
        Dim Series4 As System.Windows.Forms.DataVisualization.Charting.Series = New System.Windows.Forms.DataVisualization.Charting.Series()
        Me.pnllowprod = New System.Windows.Forms.Panel()
        Me.Label14 = New System.Windows.Forms.Label()
        Me.chrtlowlevlprod = New System.Windows.Forms.DataVisualization.Charting.Chart()
        Me.pnlmostpurchasedprod = New System.Windows.Forms.Panel()
        Me.Label12 = New System.Windows.Forms.Label()
        Me.chtmostpurchased = New System.Windows.Forms.DataVisualization.Charting.Chart()
        Me.pnlsalespmonth = New System.Windows.Forms.Panel()
        Me.Label15 = New System.Windows.Forms.Label()
        Me.chrtsalespermonth = New System.Windows.Forms.DataVisualization.Charting.Chart()
        Me.pnldistrsale = New System.Windows.Forms.Panel()
        Me.Label13 = New System.Windows.Forms.Label()
        Me.chtdistsales = New System.Windows.Forms.DataVisualization.Charting.Chart()
        Me.Label7 = New System.Windows.Forms.Label()
        Me.pnllowstock = New System.Windows.Forms.Panel()
        Me.Label11 = New System.Windows.Forms.Label()
        Me.Label6 = New System.Windows.Forms.Label()
        Me.pnltotqprod = New System.Windows.Forms.Panel()
        Me.Label9 = New System.Windows.Forms.Label()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.pnlsalestoday = New System.Windows.Forms.Panel()
        Me.Label10 = New System.Windows.Forms.Label()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.pnltotprod = New System.Windows.Forms.Panel()
        Me.Label8 = New System.Windows.Forms.Label()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.Panel13 = New System.Windows.Forms.Panel()
        Me.lblname = New System.Windows.Forms.Label()
        Me.lbldatetime = New System.Windows.Forms.Label()
        Me.Label29 = New System.Windows.Forms.Label()
        Me.Label27 = New System.Windows.Forms.Label()
        Me.Label28 = New System.Windows.Forms.Label()
        Me.lblposition = New System.Windows.Forms.Label()
        Me.Label4 = New System.Windows.Forms.Label()
        Me.pnllowprod.SuspendLayout()
        CType(Me.chrtlowlevlprod, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.pnlmostpurchasedprod.SuspendLayout()
        CType(Me.chtmostpurchased, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.pnlsalespmonth.SuspendLayout()
        CType(Me.chrtsalespermonth, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.pnldistrsale.SuspendLayout()
        CType(Me.chtdistsales, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.pnllowstock.SuspendLayout()
        Me.pnltotqprod.SuspendLayout()
        Me.pnlsalestoday.SuspendLayout()
        Me.pnltotprod.SuspendLayout()
        Me.Panel13.SuspendLayout()
        Me.SuspendLayout()
        '
        'pnllowprod
        '
        Me.pnllowprod.BackColor = System.Drawing.Color.White
        Me.pnllowprod.Controls.Add(Me.Label14)
        Me.pnllowprod.Controls.Add(Me.chrtlowlevlprod)
        Me.pnllowprod.Location = New System.Drawing.Point(29, 522)
        Me.pnllowprod.Name = "pnllowprod"
        Me.pnllowprod.Size = New System.Drawing.Size(570, 275)
        Me.pnllowprod.TabIndex = 85
        '
        'Label14
        '
        Me.Label14.AutoSize = True
        Me.Label14.Font = New System.Drawing.Font("Segoe UI Semibold", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label14.Location = New System.Drawing.Point(13, 14)
        Me.Label14.Name = "Label14"
        Me.Label14.Size = New System.Drawing.Size(176, 21)
        Me.Label14.TabIndex = 36
        Me.Label14.Text = "LOW LEVEL PRODUCTS"
        '
        'chrtlowlevlprod
        '
        ChartArea1.Name = "ChartArea1"
        Me.chrtlowlevlprod.ChartAreas.Add(ChartArea1)
        Legend1.Name = "Legend1"
        Me.chrtlowlevlprod.Legends.Add(Legend1)
        Me.chrtlowlevlprod.Location = New System.Drawing.Point(23, 49)
        Me.chrtlowlevlprod.Name = "chrtlowlevlprod"
        Series1.ChartArea = "ChartArea1"
        Series1.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Pie
        Series1.Legend = "Legend1"
        Series1.Name = "Series1"
        Me.chrtlowlevlprod.Series.Add(Series1)
        Me.chrtlowlevlprod.Size = New System.Drawing.Size(520, 200)
        Me.chrtlowlevlprod.TabIndex = 35
        Me.chrtlowlevlprod.Text = "Chart3"
        '
        'pnlmostpurchasedprod
        '
        Me.pnlmostpurchasedprod.BackColor = System.Drawing.Color.White
        Me.pnlmostpurchasedprod.Controls.Add(Me.Label12)
        Me.pnlmostpurchasedprod.Controls.Add(Me.chtmostpurchased)
        Me.pnlmostpurchasedprod.Location = New System.Drawing.Point(29, 224)
        Me.pnlmostpurchasedprod.Name = "pnlmostpurchasedprod"
        Me.pnlmostpurchasedprod.Size = New System.Drawing.Size(570, 275)
        Me.pnlmostpurchasedprod.TabIndex = 83
        '
        'Label12
        '
        Me.Label12.AutoSize = True
        Me.Label12.Font = New System.Drawing.Font("Segoe UI Semibold", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label12.Location = New System.Drawing.Point(13, 13)
        Me.Label12.Name = "Label12"
        Me.Label12.Size = New System.Drawing.Size(226, 21)
        Me.Label12.TabIndex = 36
        Me.Label12.Text = "MOST PURCHASED PRODUCT"
        '
        'chtmostpurchased
        '
        ChartArea2.Name = "ChartArea1"
        Me.chtmostpurchased.ChartAreas.Add(ChartArea2)
        Legend2.Name = "Legend1"
        Me.chtmostpurchased.Legends.Add(Legend2)
        Me.chtmostpurchased.Location = New System.Drawing.Point(17, 46)
        Me.chtmostpurchased.Name = "chtmostpurchased"
        Series2.ChartArea = "ChartArea1"
        Series2.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Bar
        Series2.Legend = "Legend1"
        Series2.Name = "Series1"
        Me.chtmostpurchased.Series.Add(Series2)
        Me.chtmostpurchased.Size = New System.Drawing.Size(520, 200)
        Me.chtmostpurchased.TabIndex = 35
        Me.chtmostpurchased.Text = "Chart3"
        '
        'pnlsalespmonth
        '
        Me.pnlsalespmonth.BackColor = System.Drawing.Color.White
        Me.pnlsalespmonth.Controls.Add(Me.Label15)
        Me.pnlsalespmonth.Controls.Add(Me.chrtsalespermonth)
        Me.pnlsalespmonth.Location = New System.Drawing.Point(625, 522)
        Me.pnlsalespmonth.Name = "pnlsalespmonth"
        Me.pnlsalespmonth.Size = New System.Drawing.Size(570, 275)
        Me.pnlsalespmonth.TabIndex = 84
        '
        'Label15
        '
        Me.Label15.AutoSize = True
        Me.Label15.Font = New System.Drawing.Font("Segoe UI Semibold", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label15.Location = New System.Drawing.Point(14, 14)
        Me.Label15.Name = "Label15"
        Me.Label15.Size = New System.Drawing.Size(150, 21)
        Me.Label15.TabIndex = 38
        Me.Label15.Text = "SALES PER MONTH"
        '
        'chrtsalespermonth
        '
        ChartArea3.Name = "ChartArea1"
        Me.chrtsalespermonth.ChartAreas.Add(ChartArea3)
        Legend3.Name = "Legend1"
        Me.chrtsalespermonth.Legends.Add(Legend3)
        Me.chrtsalespermonth.Location = New System.Drawing.Point(27, 49)
        Me.chrtsalespermonth.Name = "chrtsalespermonth"
        Series3.ChartArea = "ChartArea1"
        Series3.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Line
        Series3.Legend = "Legend1"
        Series3.Name = "Series1"
        Me.chrtsalespermonth.Series.Add(Series3)
        Me.chrtsalespermonth.Size = New System.Drawing.Size(520, 200)
        Me.chrtsalespermonth.TabIndex = 37
        Me.chrtsalespermonth.Text = "Chart3"
        '
        'pnldistrsale
        '
        Me.pnldistrsale.BackColor = System.Drawing.Color.White
        Me.pnldistrsale.Controls.Add(Me.Label13)
        Me.pnldistrsale.Controls.Add(Me.chtdistsales)
        Me.pnldistrsale.Location = New System.Drawing.Point(625, 224)
        Me.pnldistrsale.Name = "pnldistrsale"
        Me.pnldistrsale.Size = New System.Drawing.Size(570, 275)
        Me.pnldistrsale.TabIndex = 82
        '
        'Label13
        '
        Me.Label13.AutoSize = True
        Me.Label13.Font = New System.Drawing.Font("Segoe UI Semibold", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label13.Location = New System.Drawing.Point(14, 13)
        Me.Label13.Name = "Label13"
        Me.Label13.Size = New System.Drawing.Size(167, 21)
        Me.Label13.TabIndex = 36
        Me.Label13.Text = "DISTRIBUTION SALES"
        '
        'chtdistsales
        '
        ChartArea4.Name = "ChartArea1"
        Me.chtdistsales.ChartAreas.Add(ChartArea4)
        Legend4.Name = "Legend1"
        Me.chtdistsales.Legends.Add(Legend4)
        Me.chtdistsales.Location = New System.Drawing.Point(27, 46)
        Me.chtdistsales.Name = "chtdistsales"
        Series4.ChartArea = "ChartArea1"
        Series4.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Pie
        Series4.Legend = "Legend1"
        Series4.Name = "Series1"
        Me.chtdistsales.Series.Add(Series4)
        Me.chtdistsales.Size = New System.Drawing.Size(520, 200)
        Me.chtdistsales.TabIndex = 35
        Me.chtdistsales.Text = "Chart1"
        '
        'Label7
        '
        Me.Label7.AutoSize = True
        Me.Label7.Font = New System.Drawing.Font("Segoe UI", 18.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label7.ForeColor = System.Drawing.Color.FromArgb(CType(CType(1, Byte), Integer), CType(CType(21, Byte), Integer), CType(CType(78, Byte), Integer))
        Me.Label7.Location = New System.Drawing.Point(23, 12)
        Me.Label7.Name = "Label7"
        Me.Label7.Size = New System.Drawing.Size(138, 32)
        Me.Label7.TabIndex = 81
        Me.Label7.Text = "Dashboard"
        '
        'pnllowstock
        '
        Me.pnllowstock.BackColor = System.Drawing.Color.White
        Me.pnllowstock.Controls.Add(Me.Label11)
        Me.pnllowstock.Controls.Add(Me.Label6)
        Me.pnllowstock.Location = New System.Drawing.Point(925, 77)
        Me.pnllowstock.Name = "pnllowstock"
        Me.pnllowstock.Size = New System.Drawing.Size(270, 125)
        Me.pnllowstock.TabIndex = 80
        '
        'Label11
        '
        Me.Label11.AutoSize = True
        Me.Label11.Font = New System.Drawing.Font("Segoe UI", 24.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label11.Location = New System.Drawing.Point(108, 52)
        Me.Label11.Name = "Label11"
        Me.Label11.Size = New System.Drawing.Size(56, 45)
        Me.Label11.TabIndex = 4
        Me.Label11.Text = "00"
        Me.Label11.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'Label6
        '
        Me.Label6.AutoSize = True
        Me.Label6.Font = New System.Drawing.Font("Segoe UI", 14.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label6.Location = New System.Drawing.Point(56, 8)
        Me.Label6.Name = "Label6"
        Me.Label6.Size = New System.Drawing.Size(157, 25)
        Me.Label6.TabIndex = 3
        Me.Label6.Text = "Low Stock Items"
        '
        'pnltotqprod
        '
        Me.pnltotqprod.BackColor = System.Drawing.Color.White
        Me.pnltotqprod.Controls.Add(Me.Label9)
        Me.pnltotqprod.Controls.Add(Me.Label2)
        Me.pnltotqprod.Location = New System.Drawing.Point(329, 77)
        Me.pnltotqprod.Name = "pnltotqprod"
        Me.pnltotqprod.Size = New System.Drawing.Size(270, 125)
        Me.pnltotqprod.TabIndex = 78
        '
        'Label9
        '
        Me.Label9.AutoSize = True
        Me.Label9.Font = New System.Drawing.Font("Segoe UI", 24.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label9.Location = New System.Drawing.Point(101, 52)
        Me.Label9.Name = "Label9"
        Me.Label9.Size = New System.Drawing.Size(56, 45)
        Me.Label9.TabIndex = 2
        Me.Label9.Text = "00"
        Me.Label9.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Font = New System.Drawing.Font("Segoe UI", 14.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label2.Location = New System.Drawing.Point(11, 8)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(246, 25)
        Me.Label2.TabIndex = 1
        Me.Label2.Text = "Total Quantity of Products"
        '
        'pnlsalestoday
        '
        Me.pnlsalestoday.BackColor = System.Drawing.Color.White
        Me.pnlsalestoday.Controls.Add(Me.Label10)
        Me.pnlsalestoday.Controls.Add(Me.Label3)
        Me.pnlsalestoday.Location = New System.Drawing.Point(625, 77)
        Me.pnlsalestoday.Name = "pnlsalestoday"
        Me.pnlsalestoday.Size = New System.Drawing.Size(270, 125)
        Me.pnlsalestoday.TabIndex = 79
        '
        'Label10
        '
        Me.Label10.AutoSize = True
        Me.Label10.Font = New System.Drawing.Font("Segoe UI", 24.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label10.Location = New System.Drawing.Point(108, 52)
        Me.Label10.Name = "Label10"
        Me.Label10.Size = New System.Drawing.Size(56, 45)
        Me.Label10.TabIndex = 3
        Me.Label10.Text = "00"
        Me.Label10.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Font = New System.Drawing.Font("Segoe UI", 14.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label3.Location = New System.Drawing.Point(53, 8)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(162, 25)
        Me.Label3.TabIndex = 2
        Me.Label3.Text = "Total Sales Today"
        '
        'pnltotprod
        '
        Me.pnltotprod.BackColor = System.Drawing.Color.White
        Me.pnltotprod.Controls.Add(Me.Label8)
        Me.pnltotprod.Controls.Add(Me.Label1)
        Me.pnltotprod.Location = New System.Drawing.Point(29, 77)
        Me.pnltotprod.Name = "pnltotprod"
        Me.pnltotprod.Size = New System.Drawing.Size(270, 125)
        Me.pnltotprod.TabIndex = 77
        '
        'Label8
        '
        Me.Label8.AutoSize = True
        Me.Label8.Font = New System.Drawing.Font("Segoe UI", 24.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label8.Location = New System.Drawing.Point(97, 52)
        Me.Label8.Name = "Label8"
        Me.Label8.Size = New System.Drawing.Size(56, 45)
        Me.Label8.TabIndex = 1
        Me.Label8.Text = "00"
        Me.Label8.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Font = New System.Drawing.Font("Segoe UI", 14.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label1.Location = New System.Drawing.Point(60, 8)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(140, 25)
        Me.Label1.TabIndex = 0
        Me.Label1.Text = "Total Products"
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
        Me.Panel13.Dock = System.Windows.Forms.DockStyle.Bottom
        Me.Panel13.Location = New System.Drawing.Point(0, 823)
        Me.Panel13.Name = "Panel13"
        Me.Panel13.Size = New System.Drawing.Size(1220, 27)
        Me.Panel13.TabIndex = 86
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
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.Font = New System.Drawing.Font("Segoe UI", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label4.ForeColor = System.Drawing.SystemColors.ControlDarkDark
        Me.Label4.Location = New System.Drawing.Point(27, 45)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(178, 15)
        Me.Label4.TabIndex = 87
        Me.Label4.Text = "View Performance of the System"
        '
        'frmDashboard
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(1220, 850)
        Me.Controls.Add(Me.Label4)
        Me.Controls.Add(Me.Panel13)
        Me.Controls.Add(Me.pnllowprod)
        Me.Controls.Add(Me.pnlmostpurchasedprod)
        Me.Controls.Add(Me.pnlsalespmonth)
        Me.Controls.Add(Me.pnldistrsale)
        Me.Controls.Add(Me.Label7)
        Me.Controls.Add(Me.pnllowstock)
        Me.Controls.Add(Me.pnltotqprod)
        Me.Controls.Add(Me.pnlsalestoday)
        Me.Controls.Add(Me.pnltotprod)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None
        Me.Name = "frmDashboard"
        Me.Text = "frmDashboard"
        Me.pnllowprod.ResumeLayout(False)
        Me.pnllowprod.PerformLayout()
        CType(Me.chrtlowlevlprod, System.ComponentModel.ISupportInitialize).EndInit()
        Me.pnlmostpurchasedprod.ResumeLayout(False)
        Me.pnlmostpurchasedprod.PerformLayout()
        CType(Me.chtmostpurchased, System.ComponentModel.ISupportInitialize).EndInit()
        Me.pnlsalespmonth.ResumeLayout(False)
        Me.pnlsalespmonth.PerformLayout()
        CType(Me.chrtsalespermonth, System.ComponentModel.ISupportInitialize).EndInit()
        Me.pnldistrsale.ResumeLayout(False)
        Me.pnldistrsale.PerformLayout()
        CType(Me.chtdistsales, System.ComponentModel.ISupportInitialize).EndInit()
        Me.pnllowstock.ResumeLayout(False)
        Me.pnllowstock.PerformLayout()
        Me.pnltotqprod.ResumeLayout(False)
        Me.pnltotqprod.PerformLayout()
        Me.pnlsalestoday.ResumeLayout(False)
        Me.pnlsalestoday.PerformLayout()
        Me.pnltotprod.ResumeLayout(False)
        Me.pnltotprod.PerformLayout()
        Me.Panel13.ResumeLayout(False)
        Me.Panel13.PerformLayout()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

    Friend WithEvents pnllowprod As Panel
    Friend WithEvents Label14 As Label
    Friend WithEvents chrtlowlevlprod As DataVisualization.Charting.Chart
    Friend WithEvents pnlmostpurchasedprod As Panel
    Friend WithEvents Label12 As Label
    Friend WithEvents chtmostpurchased As DataVisualization.Charting.Chart
    Friend WithEvents pnlsalespmonth As Panel
    Friend WithEvents Label15 As Label
    Friend WithEvents chrtsalespermonth As DataVisualization.Charting.Chart
    Friend WithEvents pnldistrsale As Panel
    Friend WithEvents Label13 As Label
    Friend WithEvents chtdistsales As DataVisualization.Charting.Chart
    Friend WithEvents Label7 As Label
    Friend WithEvents pnllowstock As Panel
    Friend WithEvents Label11 As Label
    Friend WithEvents Label6 As Label
    Friend WithEvents pnltotqprod As Panel
    Friend WithEvents Label9 As Label
    Friend WithEvents Label2 As Label
    Friend WithEvents pnlsalestoday As Panel
    Friend WithEvents Label10 As Label
    Friend WithEvents Label3 As Label
    Friend WithEvents pnltotprod As Panel
    Friend WithEvents Label8 As Label
    Friend WithEvents Label1 As Label
    Friend WithEvents Panel13 As Panel
    Friend WithEvents lblname As Label
    Friend WithEvents lbldatetime As Label
    Friend WithEvents Label29 As Label
    Friend WithEvents Label27 As Label
    Friend WithEvents Label28 As Label
    Friend WithEvents lblposition As Label
    Friend WithEvents Label4 As Label
End Class
