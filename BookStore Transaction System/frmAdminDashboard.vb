Public Class frmAdminDashboard

    ' Declare Form Identifier Constants
    Public Const FRM_DASHBOARD As String = "FRM_DASHBOARD"
    Public Const FRM_POS As String = "FRM_POS"
    Public Const FRM_INVENTORY As String = "FRM_INVENTORY"
    Public Const FRM_TRANSACTION As String = "FRM_TRANSACTION"
    Public Const FRM_REPORTS As String = "FRM_REPORTS"
    Public Const FRM_USERMGMT As String = "FRM_USERMGMT"
    Public Const FRM_STUDENTMGMT As String = "FRM_STUDENTMGMT"
    Public Const FRM_AUDITLOGS As String = "FRM_AUDITLOGS"

    Private _currentForm As Form
    Private _isLoggingOut As Boolean = False

    Private ReadOnly _normalColor As Color = Color.FromArgb(1, 21, 78)
    Private ReadOnly _activeColor As Color = Color.FromArgb(25, 55, 140)

    ' Include all modules in the navigation array
    Private ReadOnly _allModules As String() = {
        FRM_DASHBOARD, FRM_POS, FRM_INVENTORY, FRM_TRANSACTION,
        FRM_REPORTS, FRM_USERMGMT, FRM_STUDENTMGMT, FRM_AUDITLOGS
    }

    Private Sub frmAdminDashboard_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        ApplyRolePermissions()
        OpenModule(GetDefaultForm())
    End Sub

    ' If the window is closed with the X (not through Logout), close the whole app
    Private Sub frmAdminDashboard_FormClosed(sender As Object, e As FormClosedEventArgs) Handles MyBase.FormClosed
        If Not _isLoggingOut Then Application.Exit()
    End Sub

    ' ------------------------------------------------------------------
    ' Role restrictions
    ' ------------------------------------------------------------------
    Private Sub ApplyRolePermissions()
        For Each moduleName As String In _allModules
            Dim btn As Button = GetButton(moduleName)
            If btn IsNot Nothing Then
                btn.Visible = CanAccess(moduleName)
            End If
        Next
    End Sub

    Private Function GetButton(moduleName As String) As Button
        Select Case moduleName
            Case FRM_DASHBOARD
                Return Button1          ' the "Dashboard" button
            Case FRM_POS
                Return btnPOS
            Case FRM_INVENTORY
                Return btnInventory
            Case FRM_TRANSACTION
                Return btnTransaction
            Case FRM_REPORTS
                Return btnReports
            Case FRM_USERMGMT
                Return btnUserManagement
            Case FRM_STUDENTMGMT
                Return btnStudentManagement
            Case FRM_AUDITLOGS
                Return btnAuditLogs
            Case Else
                Return Nothing
        End Select
    End Function

    Private Function CreateForm(moduleName As String) As Form
        Select Case moduleName
            Case FRM_DASHBOARD
                Return New frmDashboard()
            Case FRM_POS
                Return New frmPOS()
            Case FRM_INVENTORY
                Return New frmInventory()
            Case FRM_TRANSACTION
                Return New frmTransactionHistory()
            Case FRM_REPORTS
                Return New frmReports()
            Case FRM_USERMGMT
                Return New frmUserManagement()
            Case FRM_STUDENTMGMT
                Return New frmStudentManagement()
            Case FRM_AUDITLOGS
                Return New frmAuditLogs()
            Case Else
                Return Nothing
        End Select
    End Function

    ' ------------------------------------------------------------------
    ' Loads a form inside pnlContent (after checking the role again)
    ' ------------------------------------------------------------------
    Private Sub OpenModule(moduleName As String)
        If Not CanAccess(moduleName) Then
            MsgBox("Access denied. Your role (" & currentuser.Role & ") is not allowed to open this module.", vbExclamation, "Access Denied")
            Exit Sub
        End If

        Dim frm As Form = CreateForm(moduleName)
        If frm Is Nothing Then Exit Sub

        If _currentForm IsNot Nothing Then
            _currentForm.Close()
            _currentForm = Nothing
        End If
        pnlContent.Controls.Clear()

        frm.TopLevel = False
        frm.FormBorderStyle = FormBorderStyle.None
        frm.Dock = DockStyle.Fill
        pnlContent.Controls.Add(frm)
        _currentForm = frm
        frm.Show()

        SetActiveButton(moduleName)
    End Sub

    ' Highlights the button of the module that is currently open
    Private Sub SetActiveButton(moduleName As String)
        For Each m As String In _allModules
            Dim btn As Button = GetButton(m)
            If btn IsNot Nothing Then
                btn.BackColor = _normalColor
            End If
        Next

        Dim active As Button = GetButton(moduleName)
        If active IsNot Nothing Then active.BackColor = _activeColor
    End Sub

    ' ------------------------------------------------------------------
    ' Navigation Button Click Handlers
    ' ------------------------------------------------------------------
    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        OpenModule(FRM_DASHBOARD)
    End Sub

    Private Sub btnPOS_Click(sender As Object, e As EventArgs) Handles btnPOS.Click
        OpenModule(FRM_POS)
    End Sub

    Private Sub btnInventory_Click(sender As Object, e As EventArgs) Handles btnInventory.Click
        OpenModule(FRM_INVENTORY)
    End Sub

    Private Sub btnTransaction_Click(sender As Object, e As EventArgs) Handles btnTransaction.Click
        OpenModule(FRM_TRANSACTION)
    End Sub

    Private Sub btnReports_Click(sender As Object, e As EventArgs) Handles btnReports.Click
        OpenModule(FRM_REPORTS)
    End Sub

    Private Sub btnUserManagement_Click(sender As Object, e As EventArgs) Handles btnUserManagement.Click
        OpenModule(FRM_USERMGMT)
    End Sub

    Private Sub btnStudentManagement_Click(sender As Object, e As EventArgs) Handles btnStudentManagement.Click
        OpenModule(FRM_STUDENTMGMT)
    End Sub

    Private Sub btnAuditLogs_Click(sender As Object, e As EventArgs) Handles btnAuditLogs.Click
        OpenModule(FRM_AUDITLOGS)
    End Sub

    Private Sub btnLogout_Click(sender As Object, e As EventArgs) Handles btnLogout.Click
        If MsgBox("Are you sure you want to logout?", vbYesNo + vbQuestion, "Confirm Logout") = MsgBoxResult.Yes Then
            currentuser.UserID = 0
            currentuser.FullName = ""
            currentuser.Role = ""
            _isLoggingOut = True
            Login.Show()
            Me.Close()
        End If
    End Sub

End Class