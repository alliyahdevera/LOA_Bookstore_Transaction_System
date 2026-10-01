Imports MySql.Data.MySqlClient

Public Class frmAdminDashboard

    ' NOTE: the FRM_* constants live in RolePermissions.vb, so they are not
    ' redeclared here (duplicates would hide the shared ones).

    Private _currentForm As Form
    Private _isLoggingOut As Boolean = False

    Private ReadOnly _normalColor As Color = Color.FromArgb(1, 21, 78)
    Private ReadOnly _activeColor As Color = Color.FromArgb(25, 55, 140)

    ' Navigation module list
    Private ReadOnly _allModules As String() = {
        FRM_DASHBOARD, FRM_POS, FRM_INVENTORY, FRM_TRANSACTION,
        FRM_REPORTS, FRM_USERMGMT, FRM_STUDENTMGMT, FRM_AUDITLOGS
    }

    ' ==================================================================
    ' Form events
    ' ==================================================================
    Private Sub frmAdminDashboard_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        ApplyRolePermissions()
        OpenModule(RolePermissions.GetDefaultForm())
    End Sub

    Private Sub frmAdminDashboard_FormClosed(sender As Object, e As FormClosedEventArgs) Handles MyBase.FormClosed
        If Not _isLoggingOut Then Application.Exit()
    End Sub

    ' ==================================================================
    ' Role permissions (uses RolePermissions.vb)
    ' ==================================================================
    Private Sub ApplyRolePermissions()
        For Each moduleName As String In _allModules
            Dim btn As Button = GetButton(moduleName)
            If btn IsNot Nothing Then
                btn.Visible = RolePermissions.CanAccess(moduleName)
            End If
        Next
    End Sub

    ' ==================================================================
    ' Module -> Button / Form mapping
    ' ==================================================================
    Private Function GetButton(moduleName As String) As Button
        Select Case moduleName
            Case FRM_DASHBOARD
                Return btnDashboard
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

    ' ==================================================================
    ' Open a module inside pnlContent
    ' ==================================================================
    Private Sub OpenModule(moduleName As String)

        If Not RolePermissions.CanAccess(moduleName) Then
            MsgBox("Access denied. Your role (" & currentuser.Role & ") is not allowed to open this module.",
                   vbExclamation, "Access Denied")
            Exit Sub
        End If

        Try
            Dim frm As Form = CreateForm(moduleName)

            If frm Is Nothing Then
                MsgBox("Module form initialization returned Nothing: " & moduleName,
                       vbCritical, "Module Error")
                Exit Sub
            End If

            If _currentForm IsNot Nothing Then
                _currentForm.Close()
                _currentForm.Dispose()
                _currentForm = Nothing
            End If

            pnlContent.Controls.Clear()

            frm.TopLevel = False
            frm.FormBorderStyle = FormBorderStyle.None
            frm.Dock = DockStyle.Fill

            pnlContent.Controls.Add(frm)
            _currentForm = frm

            frm.Show()
            frm.BringToFront()

            SetActiveButton(moduleName)

        Catch ex As Exception
            MsgBox("Error embedding form into Dashboard Panel: " & ex.Message & vbCrLf & ex.StackTrace,
                   vbCritical, "UI Navigation Error")
        End Try

    End Sub

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

    ' ==================================================================
    ' Navigation buttons
    ' ==================================================================
    Private Sub btnDashboard_Click(sender As Object, e As EventArgs) Handles btnDashboard.Click
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

    ' ==================================================================
    ' Logout
    ' ==================================================================
    Private Sub btnlogout_Click(sender As Object, e As EventArgs) Handles btnlogout.Click

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