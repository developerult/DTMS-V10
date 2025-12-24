Public Class clsHazmatObject

    Private _HazRegulation As String = ""
    Public Property HazRegulation() As String
        Get
            Return Left(Me._HazRegulation, 50)
        End Get
        Set(value As String)
            If (String.Equals(Me._HazRegulation, value) = False) Then
                Me._HazRegulation = Left(value, 50)
            End If
        End Set
    End Property

    Private _HazItem As String = ""
    Public Property HazItem() As String
        Get
            Return Left(Me._HazItem, 50)
        End Get
        Set(value As String)
            If (String.Equals(Me._HazItem, value) = False) Then
                Me._HazItem = Left(value, 50)
            End If
        End Set
    End Property

    Private _HazClass As String = ""
    Public Property HazClass() As String
        Get
            Return Left(Me._HazClass, 10)
        End Get
        Set(value As String)
            If (String.Equals(Me._HazClass, value) = False) Then
                Me._HazClass = Left(value, 10)
            End If
        End Set
    End Property

    Private _HazID As String = ""
    Public Property HazID() As String
        Get
            Return Left(Me._HazID, 20)
        End Get
        Set(value As String)
            If (String.Equals(Me._HazID, value) = False) Then
                Me._HazID = Left(value, 20)
            End If
        End Set
    End Property

    Private _HazDesc01 As String = ""
    Public Property HazDesc01() As String
        Get
            Return Left(Me._HazDesc01, 255)
        End Get
        Set(value As String)
            If (String.Equals(Me._HazDesc01, value) = False) Then
                Me._HazDesc01 = Left(value, 255)
            End If
        End Set
    End Property

    Private _HazDesc02 As String = ""
    Public Property HazDesc02() As String
        Get
            Return Left(Me._HazDesc02, 255)
        End Get
        Set(value As String)
            If (String.Equals(Me._HazDesc02, value) = False) Then
                Me._HazDesc02 = Left(value, 255)
            End If
        End Set
    End Property

    Private _HazDesc03 As String = ""
    Public Property HazDesc03() As String
        Get
            Return Left(Me._HazDesc03, 255)
        End Get
        Set(value As String)
            If (String.Equals(Me._HazDesc03, value) = False) Then
                Me._HazDesc03 = Left(value, 255)
            End If
        End Set
    End Property

    Private _HazUnit As String = ""
    Public Property HazUnit() As String
        Get
            Return Left(Me._HazUnit, 100)
        End Get
        Set(value As String)
            If (String.Equals(Me._HazUnit, value) = False) Then
                Me._HazUnit = Left(value, 100)
            End If
        End Set
    End Property

    Private _HazPackingGroup As String = ""
    Public Property HazPackingGroup() As String
        Get
            Return Left(Me._HazPackingGroup, 10)
        End Get
        Set(value As String)
            If (String.Equals(Me._HazPackingGroup, value) = False) Then
                Me._HazPackingGroup = Left(value, 10)
            End If
        End Set
    End Property

    Private _HazPackingDesc As String = ""
    Public Property HazPackingDesc() As String
        Get
            Return Left(Me._HazPackingDesc, 1000)
        End Get
        Set(value As String)
            If (String.Equals(Me._HazPackingDesc, value) = False) Then
                Me._HazPackingDesc = Left(value, 1000)
            End If
        End Set
    End Property

    Private _HazShipInst As String = ""
    Public Property HazShipInst() As String
        Get
            Return Left(Me._HazShipInst, 1000)
        End Get
        Set(value As String)
            If (String.Equals(Me._HazShipInst, value) = False) Then
                Me._HazShipInst = Left(value, 1000)
            End If
        End Set
    End Property

    Private _HazLtdQ As Boolean = False
    Public Property HazLtdQ() As Boolean
        Get
            Return Me._HazLtdQ
        End Get
        Set(value As Boolean)
            If ((Me._HazLtdQ = value) _
               = False) Then
                Me._HazLtdQ = value
            End If
        End Set
    End Property

    Private _HazMarPoll As Boolean = False
    Public Property HazMarPoll() As Boolean
        Get
            Return Me._HazMarPoll
        End Get
        Set(value As Boolean)
            If ((Me._HazMarPoll = value) _
               = False) Then
                Me._HazMarPoll = value
            End If
        End Set
    End Property

    Private _HazMarStorCat As String = ""
    Public Property HazMarStorCat() As String
        Get
            Return Left(Me._HazMarStorCat, 50)
        End Get
        Set(value As String)
            If (String.Equals(Me._HazMarStorCat, value) = False) Then
                Me._HazMarStorCat = Left(value, 50)
            End If
        End Set
    End Property

    Private _HazNMFCSub As Integer = 0
    Public Property HazNMFCSub() As Integer
        Get
            Return Me._HazNMFCSub
        End Get
        Set(value As Integer)
            If ((Me._HazNMFCSub = value) _
               = False) Then
                Me._HazNMFCSub = value
            End If
        End Set
    End Property

    Private _HazNMFC As Integer = 0
    Public Property HazNMFC() As Integer
        Get
            Return Me._HazNMFC
        End Get
        Set(value As Integer)
            If ((Me._HazNMFC = value) _
               = False) Then
                Me._HazNMFC = value
            End If
        End Set
    End Property

    Private _HazFrtClass As Integer = 0
    Public Property HazFrtClass() As Integer
        Get
            Return Me._HazFrtClass
        End Get
        Set(value As Integer)
            If ((Me._HazFrtClass = value) _
               = False) Then
                Me._HazFrtClass = value
            End If
        End Set
    End Property

    Private _HazFdxGndOK As Boolean = False
    Public Property HazFdxGndOK() As Boolean
        Get
            Return Me._HazFdxGndOK
        End Get
        Set(value As Boolean)
            If ((Me._HazFdxGndOK = value) _
               = False) Then
                Me._HazFdxGndOK = value
            End If
        End Set
    End Property

    Private _HazFdxAirOK As Boolean = False
    Public Property HazFdxAirOK() As Boolean
        Get
            Return Me._HazFdxAirOK
        End Get
        Set(value As Boolean)
            If ((Me._HazFdxAirOK = value) _
               = False) Then
                Me._HazFdxAirOK = value
            End If
        End Set
    End Property

    Private _HazUPSgndOK As Boolean = False
    Public Property HazUPSgndOK() As Boolean
        Get
            Return Me._HazUPSgndOK
        End Get
        Set(value As Boolean)
            If ((Me._HazUPSgndOK = value) _
               = False) Then
                Me._HazUPSgndOK = value
            End If
        End Set
    End Property

    Private _HazUPSAirOK As Boolean = False
    Public Property HazUPSAirOK() As Boolean
        Get
            Return Me._HazUPSAirOK
        End Get
        Set(value As Boolean)
            If ((Me._HazUPSAirOK = value) _
               = False) Then
                Me._HazUPSAirOK = value
            End If
        End Set
    End Property

    Public Shared Function GenerateSampleObject() As clsHazmatObject

        Return New clsHazmatObject With { _
            .HazRegulation = "Regulation Text",
            .HazItem = "HAZ TEST Item",
            .HazClass = "Test Class",
            .HazID = "TEST",
            .HazDesc01 = "Desc 1",
            .HazDesc02 = "Desc 2",
            .HazDesc03 = "Desc 3",
            .HazUnit = "Unit Text",
            .HazPackingGroup = "Packing Group",
            .HazPackingDesc = "Packing Desc",
            .HazShipInst = "Shiping Instructions",
            .HazLtdQ = False,
            .HazMarPoll = False,
            .HazMarStorCat = "Store Cat Text",
            .HazNMFCSub = 1,
            .HazNMFC = 150,
            .HazFrtClass = 100,
            .HazFdxGndOK = True,
            .HazFdxAirOK = True,
            .HazUPSgndOK = True,
            .HazUPSAirOK = True}

    End Function

End Class
