Public Module RolePermissions

    ' ==================================================================
    ' Form / Module Navigation Constants
    ' ==================================================================
    Public Const FRM_DASHBOARD As String = "FRM_DASHBOARD"
    Public Const FRM_POS As String = "FRM_POS"
    Public Const FRM_INVENTORY As String = "FRM_INVENTORY"
    Public Const FRM_TRANSACTION As String = "FRM_TRANSACTION"
    Public Const FRM_REPORTS As String = "FRM_REPORTS"
    Public Const FRM_USERMGMT As String = "FRM_USERMGMT"
    Public Const FRM_STUDENTMGMT As String = "FRM_STUDENTMGMT"
    Public Const FRM_AUDITLOGS As String = "FRM_AUDITLOGS"

    ' ==================================================================
    ' Role Constants (Matches tbl_roles in MySQL database)
    ' ==================================================================
    Public Const ROLE_SUPERVISOR As String = "Bookstore Supervisor"
    Public Const ROLE_CASHIER As String = "Cashier"
    Public Const ROLE_INVENTORY_STAFF As String = "Inventory Staff"
    Public Const ROLE_STAFF As String = "Inventory Staff" ' Alias for forms referencing ROLE_STAFF
    Public Const ROLE_MANAGEMENT As String = "Management"
    Public Const ROLE_ADMIN As String = "Bookstore Supervisor" ' Alias for forms referencing Admin

    ''' <summary>
    ''' Returns True if the logged-in user's role has access to the specified module.
    ''' </summary>
    Public Function CanAccess(moduleName As String) As Boolean
        ' 1. Dashboard is accessible to everyone
        If moduleName = FRM_DASHBOARD Then Return True

        ' 2. Retrieve user role safely
        Dim userRole As String = If(currentuser.Role, "").Trim()

        ' 3. Bookstore Supervisor & Management have access to all modules
        If userRole.Equals(ROLE_SUPERVISOR, StringComparison.OrdinalIgnoreCase) OrElse
           userRole.Equals(ROLE_MANAGEMENT, StringComparison.OrdinalIgnoreCase) OrElse
           userRole.Equals("Admin", StringComparison.OrdinalIgnoreCase) Then
            Return True
        End If

        ' 4. Role-specific restrictions
        Select Case userRole
            Case ROLE_CASHIER
                ' Cashiers can access POS, Transactions, and Student Lookup
                Select Case moduleName
                    Case FRM_POS, FRM_TRANSACTION, FRM_STUDENTMGMT
                        Return True
                    Case Else
                        Return False
                End Select

            Case ROLE_INVENTORY_STAFF
                ' Inventory Staff can access Inventory and Student Management
                Select Case moduleName
                    Case FRM_INVENTORY, FRM_STUDENTMGMT
                        Return True
                    Case Else
                        Return False
                End Select

            Case Else
                ' Unknown roles or missing roles are blocked by default
                Return False
        End Select
    End Function

    ''' <summary>
    ''' Returns an array of module constants available to a given role.
    ''' </summary>
    Public Function GetAllowedForms(role As String) As String()
        Dim roleName As String = If(role, "").Trim()

        If roleName.Equals(ROLE_SUPERVISOR, StringComparison.OrdinalIgnoreCase) OrElse
           roleName.Equals(ROLE_MANAGEMENT, StringComparison.OrdinalIgnoreCase) OrElse
           roleName.Equals("Admin", StringComparison.OrdinalIgnoreCase) Then

            Return New String() {
                FRM_DASHBOARD, FRM_POS, FRM_INVENTORY, FRM_TRANSACTION,
                FRM_REPORTS, FRM_USERMGMT, FRM_STUDENTMGMT, FRM_AUDITLOGS
            }

        ElseIf roleName.Equals(ROLE_CASHIER, StringComparison.OrdinalIgnoreCase) Then
            Return New String() {
                FRM_DASHBOARD, FRM_POS, FRM_TRANSACTION, FRM_STUDENTMGMT
            }

        ElseIf roleName.Equals(ROLE_INVENTORY_STAFF, StringComparison.OrdinalIgnoreCase) Then
            Return New String() {
                FRM_DASHBOARD, FRM_INVENTORY, FRM_STUDENTMGMT
            }

        Else
            Return New String() {FRM_DASHBOARD}
        End If
    End Function

    ''' <summary>
    ''' Returns the default starting module after login.
    ''' </summary>
    Public Function GetDefaultForm() As String
        Dim allowed() As String = GetAllowedForms(currentuser.Role)
        If allowed.Length > 0 Then Return allowed(0)
        Return FRM_DASHBOARD
    End Function

End Module