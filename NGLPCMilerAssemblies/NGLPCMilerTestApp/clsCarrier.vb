Public Class clsCarrier

    Private _Name As String
    Public Property Name() As String
        Get
            Return _Name
        End Get
        Set(ByVal value As String)
            _Name = value
        End Set
    End Property

    Private _Total_Cost As String
    Public Property Total_Cost() As String
        Get
            Return _Total_Cost
        End Get
        Set(ByVal value As String)
            _Total_Cost = value
        End Set
    End Property

    Private _Fuel As String
    Public Property Fuel() As String
        Get
            Return _Fuel
        End Get
        Set(ByVal value As String)
            _Fuel = value
        End Set
    End Property

    Sub New()

    End Sub

    Sub New(sName As String, sCost As String, sFuel As String)
        Me.Name = sName
        Me.Total_Cost = sCost
        Me.Fuel = sFuel
    End Sub




End Class
