Public Class frmUnreleasedItems
    Inherits frmReleasing

    Public Sub New()
        MyBase.New(False)       ' read-only list of every pending pick-up item
    End Sub
End Class