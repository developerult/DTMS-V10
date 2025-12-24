Namespace Models

    Public Class LoadsDeliveredSummary

        Private _iMonth As Integer = 0
        Public Property iMonth() As Integer
            Get
                Return _iMonth
            End Get
            Set(ByVal value As Integer)
                _iMonth = value
            End Set
        End Property

        Private _iLoads As Integer = 0
        Public Property Loads() As Integer
            Get
                Return _iLoads
            End Get
            Set(ByVal value As Integer)
                _iLoads = value
            End Set
        End Property

        Public ReadOnly Property Month() As String
            Get
                Return System.Globalization.DateTimeFormatInfo.CurrentInfo.GetAbbreviatedMonthName(iMonth)
            End Get

        End Property
    End Class

End Namespace