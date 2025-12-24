<Serializable()> _
Public Class clsIntegrationUpdateResults 'Does it need to inherit anything?

    Private _ReturnValue As Ngl.FreightMaster.Integration.Configuration.ProcessDataReturnValues = Configuration.ProcessDataReturnValues.nglDataIntegrationComplete
    Public Property ReturnValue() As Ngl.FreightMaster.Integration.Configuration.ProcessDataReturnValues
        Get
            Return _ReturnValue
        End Get
        Set(ByVal value As Ngl.FreightMaster.Integration.Configuration.ProcessDataReturnValues)
            _ReturnValue = value
        End Set
    End Property

    Private _ControlNumbers As New List(Of Integer)
    Public Property ControlNumbers As List(Of Integer)
        Get
            If _ControlNumbers Is Nothing Then _ControlNumbers = New List(Of Integer)
            Return _ControlNumbers
        End Get
        Set(ByVal value As List(Of Integer))
            _ControlNumbers = value
        End Set
    End Property

End Class
