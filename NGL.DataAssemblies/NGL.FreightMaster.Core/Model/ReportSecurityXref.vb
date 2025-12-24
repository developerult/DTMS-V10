Imports NGL.FreightMaster.Core.UserConfiguration
Imports NGL.FreightMaster.Data.SystemSecurityDataTableAdapters
Imports Ngl.Core.Utility.DataTransformation
Namespace Model
    <Serializable()> _
    <System.ComponentModel.DataObject()> _
    Public Class ReportSecurityXref : Inherits Ngl.Core.DirectDataObject

#Region "Class Variables and Properties"

        Private _oReportSecurityXrefTable As SystemSecurityData.tblReportSecurityXrefDataTable = Nothing
        Public ReadOnly Property oReportSecurityXrefTable() As SystemSecurityData.tblReportSecurityXrefDataTable
            Get
                If _oReportSecurityXrefTable Is Nothing Then
                    _oReportSecurityXrefTable = Me.GetData
                End If
                Return _oReportSecurityXrefTable
            End Get
        End Property

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

        Private _strName As String = "Report Security Cross Reference"
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

        Private _Adapter As tblReportSecurityXrefTableAdapter
        Protected ReadOnly Property Adapter() As tblReportSecurityXrefTableAdapter
            Get
                If _Adapter Is Nothing Then
                    _Adapter = New tblReportSecurityXrefTableAdapter
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
        Public Function GetData() As SystemSecurityData.tblReportSecurityXrefDataTable
            Adapter.SetCommandTimeOut(Me.CommandTimeOut)
            Return (Adapter.GetData())
        End Function

        <System.ComponentModel.DataObjectMethodAttribute(System.ComponentModel.DataObjectMethodType.Select, True)> _
        Public Function GetDataByID(ByVal ID As Integer) As SystemSecurityData.tblReportSecurityXrefDataTable
            Adapter.SetCommandTimeOut(Me.CommandTimeOut)
            Return (Adapter.GetDataByID(ID))
        End Function

        <System.ComponentModel.DataObjectMethodAttribute(System.ComponentModel.DataObjectMethodType.Select, True)> _
        Public Function GetDataByReportID(ByVal ReportID As Integer) As SystemSecurityData.tblReportSecurityXrefDataTable
            Adapter.SetCommandTimeOut(Me.CommandTimeOut)
            Return (Adapter.GetDataByReportID(ReportID))
        End Function

        <System.ComponentModel.DataObjectMethodAttribute(System.ComponentModel.DataObjectMethodType.Select, True)> _
        Public Function GetDataByUserID(ByVal UserID As Integer) As SystemSecurityData.tblReportSecurityXrefDataTable
            Adapter.SetCommandTimeOut(Me.CommandTimeOut)
            Return (Adapter.GetDataByUserID(UserID))
        End Function

        <System.ComponentModel.DataObjectMethodAttribute(System.ComponentModel.DataObjectMethodType.Select, True)> _
        Public Function GetDataByUserIDandReportID(ByVal UserID As Integer, ByVal ReportID As Integer) As SystemSecurityData.tblReportSecurityXrefDataTable
            Adapter.SetCommandTimeOut(Me.CommandTimeOut)
            Return (Adapter.GetDataByUserIDandReportID(UserID, ReportID))
        End Function
#End Region


    End Class

End Namespace


