Module modAppData
    Private _ERPFrieghtAccountIndex As String = "520"
    Public Property ERPFrieghtAccountIndex() As String
        Get
            Return _ERPFrieghtAccountIndex
        End Get
        Set(ByVal value As String)
            _ERPFrieghtAccountIndex = value
        End Set
    End Property


End Module
