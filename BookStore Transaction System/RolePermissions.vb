Module RolePermissions

    ' Role names - must match TBL_ROLES.role_name exactly
    Public Const ROLE_SUPERVISOR As String = "Bookstore Supervisor"
    Public Const ROLE_CASHIER As String = "Cashier"
    Public Const ROLE_INVENTORY As String = "Inventory Staff"
    Public Const ROLE_MANAGEMENT As String = "Management"

    ' Module (form) names - must match the form class names
    Public Const FRM_DASHBOARD As String = "frmDashboard"
    Public Const FRM_POS As String = "frmPOS"
    Public Const FRM_INVENTORY As String = "frmInventory"
    Public Const FRM_TRANSACTION As String = "frmTransactionHistory"
    Public Const FRM_REPORTS As String = "frmReports"
    Public Const FRM_USERMGMT As String = "frmUserManagement"

    Private Function GetAllowedForms(role As String) As String()
        Select Case If(role, "").Trim()
            Case ROLE_SUPERVISOR
                ' Q38: all inventory, sales, reports and user-management functions
                Return New String() {FRM_DASHBOARD, FRM_POS, FRM_INVENTORY, FRM_TRANSACTION, FRM_REPORTS, FRM_USERMGMT}
            Case ROLE_CASHIER
                ' Q38/Q40: sales transactions, payments, change, daily cash reports
                Return New String() {FRM_POS, FRM_TRANSACTION, FRM_REPORTS}
            Case ROLE_INVENTORY
                ' Q38: receiving, stock-in/out, inventory counts, low-stock monitoring
                Return New String() {FRM_INVENTORY, FRM_REPORTS}
            Case ROLE_MANAGEMENT
                ' Q38/Q43: reports, summaries and monitoring only
                Return New String() {FRM_DASHBOARD, FRM_REPORTS}
            Case Else
                Return New String() {}
        End Select
    End Function

    ' True if the logged-in user's role may open the given form
    Public Function CanAccess(formName As String) As Boolean
        Return Array.IndexOf(GetAllowedForms(currentuser.Role), formName) >= 0
    End Function

    ' The form opened right after login (first allowed form for the role)
    Public Function GetDefaultForm() As String
        Dim allowed As String() = GetAllowedForms(currentuser.Role)
        If allowed.Length > 0 Then Return allowed(0)
        Return ""
    End Function

End Module