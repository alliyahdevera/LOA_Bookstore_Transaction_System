Public Module RolePermissions

    Public Const FRM_DASHBOARD As String = "FRM_DASHBOARD"
    Public Const FRM_POS As String = "FRM_POS"
    Public Const FRM_INVENTORY As String = "FRM_INVENTORY"
    Public Const FRM_TRANSACTION As String = "FRM_TRANSACTION"
    Public Const FRM_REPORTS As String = "FRM_REPORTS"
    Public Const FRM_USERMGMT As String = "FRM_USERMGMT"
    Public Const FRM_STUDENTMGMT As String = "FRM_STUDENTMGMT"
    Public Const FRM_AUDITLOGS As String = "FRM_AUDITLOGS"
    Public Const ROLE_SUPERVISOR As String = "Bookstore Supervisor"
    Public Const ROLE_CASHIER As String = "Cashier"
    Public Const ROLE_INVENTORY_STAFF As String = "Inventory Staff"
    Public Const ROLE_STAFF As String = "Inventory Staff"          ' alias
    Public Const ROLE_MANAGEMENT As String = "Management"
    Public Const ROLE_ADMIN As String = "Bookstore Supervisor"     ' alias

    Private ReadOnly AllModules As String() = New String() {
        FRM_DASHBOARD, FRM_POS, FRM_INVENTORY, FRM_TRANSACTION,
        FRM_REPORTS, FRM_USERMGMT, FRM_STUDENTMGMT, FRM_AUDITLOGS
    }

    Private Function IsRole(roleName As String, expected As String) As Boolean
        Return If(roleName, "").Trim().Equals(expected, StringComparison.OrdinalIgnoreCase)
    End Function

    ''' <summary>True for Management: they may look at everything they can open, but never save/edit.</summary>
    Public Function IsViewOnly() As Boolean
        Return IsRole(currentuser.Role, ROLE_MANAGEMENT)
    End Function

    ''' <summary>Returns True if the logged-in user's role may open the module.</summary>
    Public Function CanAccess(moduleName As String) As Boolean
        Return CanAccessAs(currentuser.Role, moduleName)
    End Function

    Private Function CanAccessAs(roleName As String, moduleName As String) As Boolean
        ' Dashboard is for everyone
        If moduleName = FRM_DASHBOARD Then Return True

        ' Supervisor: everything
        If IsRole(roleName, ROLE_SUPERVISOR) OrElse IsRole(roleName, "Admin") Then Return True

        ' Management: monitoring only (all screens are read-only, see IsViewOnly)
        If IsRole(roleName, ROLE_MANAGEMENT) Then
            Select Case moduleName
                Case FRM_TRANSACTION, FRM_INVENTORY, FRM_REPORTS, FRM_AUDITLOGS
                    Return True
                Case Else
                    Return False
            End Select
        End If

        ' Cashier: sales, payments, student lookup, daily cash report
        If IsRole(roleName, ROLE_CASHIER) Then
            Select Case moduleName
                Case FRM_POS, FRM_TRANSACTION, FRM_STUDENTMGMT, FRM_REPORTS
                    Return True
                Case Else
                    Return False
            End Select
        End If

        ' Inventory Staff: receiving, stock records, counts, low-stock monitoring
        If IsRole(roleName, ROLE_INVENTORY_STAFF) Then
            Select Case moduleName
                Case FRM_INVENTORY, FRM_STUDENTMGMT
                    Return True
                Case Else
                    Return False
            End Select
        End If

        ' Unknown / missing role: blocked
        Return False
    End Function

    ''' <summary>Modules available to a given role (same order as the menu).</summary>
    Public Function GetAllowedForms(role As String) As String()
        Dim result As New List(Of String)
        For Each m As String In AllModules
            If CanAccessAs(role, m) Then result.Add(m)
        Next
        If result.Count = 0 Then result.Add(FRM_DASHBOARD)
        Return result.ToArray()
    End Function

    ''' <summary>Default starting module after login.</summary>
    Public Function GetDefaultForm() As String
        Dim allowed() As String = GetAllowedForms(currentuser.Role)
        If allowed.Length > 0 Then Return allowed(0)
        Return FRM_DASHBOARD
    End Function

End Module