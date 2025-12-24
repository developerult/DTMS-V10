Imports NGL.FreightMaster.Core.UserConfiguration
Imports NGL.FreightMaster.Data.ApplicationDataTableAdapters
Imports Ngl.Core.Utility.DataTransformation

Namespace Model

    <Serializable()> _
    <System.ComponentModel.DataObject()> _
    Public Class PickListOrder : Inherits Ngl.Core.DirectDataObject

#Region "Class Variables and Properties"


        Private _oUserConfiguration As UserConfiguration = Nothing
        Public Property oUserConfiguration() As UserConfiguration
            Get
                If _oUserConfiguration Is Nothing Then
                    _oUserConfiguration = New UserConfiguration
                End If
                Return _oUserConfiguration
            End Get
            Set(ByVal value As UserConfiguration)
                _oUserConfiguration = value
            End Set
        End Property

        Private _strUnmatched As String = ""
        Public ReadOnly Property Unmatched() As String
            Get
                Return _strUnmatched

            End Get
        End Property

        Private _intRowsAffected As Integer = 0
        Public ReadOnly Property RowsAffected() As Integer
            Get
                Return _intRowsAffected
            End Get
        End Property

        Private _strName As String = "GeneratePickListOrder"
        Private _strKey As String = ""
        Private _intTimeOut As Integer = 0

        Protected Property CommandTimeOut() As Integer
            Get
                If _intTimeOut < 100 Then
                    _intTimeOut = Me.oUserConfiguration.ShortTimeOut
                End If
                Return _intTimeOut
            End Get
            Set(ByVal value As Integer)
                _intTimeOut = value
            End Set
        End Property


        Private _Adapter As spGeneratePickListOrderTableAdapter
        Protected ReadOnly Property Adapter() As spGeneratePickListOrderTableAdapter
            Get
                If _Adapter Is Nothing Then
                    _Adapter = New spGeneratePickListOrderTableAdapter
                    _Adapter.SetConnectionString(ConnectionString)
                End If

                Return _Adapter
            End Get
        End Property

#End Region

#Region "Constructors"

        Public Sub New()
            MyBase.new()
        End Sub

        Public Sub New(ByVal oConfig As UserConfiguration)
            MyBase.new()
            Me.oUserConfiguration = oConfig
            With oConfig
                Me.Debug = .Debug
                Me.Database = .Database
                Me.Server = .DBServer
                Me.Source = .Source
                Me.ConnectionString = .ConnectionString
            End With
        End Sub


#End Region

#Region "Functions"

        <System.ComponentModel.DataObjectMethodAttribute(System.ComponentModel.DataObjectMethodType.Select, True)> _
            Public Function GetData(ByVal BookControl As Global.System.Nullable(Of Integer), ByVal BookConsPrefix As String) As ApplicationData.spGeneratePickListOrderDataTable
            Adapter.SetCommandTimeOut(Me.CommandTimeOut)
            Return (Adapter.GetData(BookControl, BookConsPrefix))

        End Function

#End Region

    End Class

End Namespace