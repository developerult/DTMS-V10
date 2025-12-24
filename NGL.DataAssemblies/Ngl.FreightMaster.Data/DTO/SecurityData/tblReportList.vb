Imports Microsoft.VisualBasic
Imports System
Imports System.Collections.Generic
Imports System.Linq
Imports System.Text
Imports System.Runtime.Serialization
Imports Ngl.Core.ChangeTracker

Namespace DataTransferObjects
    <DataContract()> _
    Public Class tblReportList
        Inherits DTOBaseClass

#Region " Data Members"

        Private _ReportControl As Integer = 0
        <DataMember()> _
        Public Property ReportControl() As Integer
            Get
                Return _ReportControl
            End Get
            Set(ByVal value As Integer)
                _ReportControl = value
            End Set
        End Property

        Private _ReportName As String = ""
        <DataMember()> _
        Public Property ReportName() As String
            Get
                Return Left(_ReportName, 50)
            End Get
            Set(ByVal value As String)
                _ReportName = Left(value, 50)
            End Set
        End Property

        Private _ReportDescription As String = ""
        <DataMember()> _
        Public Property ReportDescription() As String
            Get
                Return Left(_ReportDescription, 50)
            End Get
            Set(ByVal value As String)
                _ReportDescription = Left(value, 50)
            End Set
        End Property

        Private _ReportServerURL As String = ""
        <DataMember()> _
        Public Property ReportServerURL() As String
            Get
                Return Left(_ReportServerURL, 500)
            End Get
            Set(ByVal value As String)
                _ReportServerURL = Left(value, 500)
            End Set
        End Property

        Private _ReportURL As String = ""
        <DataMember()> _
        Public Property ReportURL() As String
            Get
                Return Left(_ReportURL, 500)
            End Get
            Set(ByVal value As String)
                _ReportURL = Left(value, 500)
            End Set
        End Property

        Private _ReportPrinterName As String = ""
        <DataMember()> _
        Public Property ReportPrinterName() As String
            Get
                Return Left(_ReportPrinterName, 255)
            End Get
            Set(ByVal value As String)
                _ReportPrinterName = Left(value, 255)
            End Set
        End Property

        Private _ReportDataSource As String = ""
        <DataMember()> _
        Public Property ReportDataSource() As String
            Get
                Return Left(_ReportDataSource, 255)
            End Get
            Set(ByVal value As String)
                _ReportDataSource = Left(value, 255)
            End Set
        End Property

        Private _AMSActive As Boolean = False
        <DataMember()> _
        Public Property AMSActive() As Boolean
            Get
                Return _AMSActive
            End Get
            Set(ByVal value As Boolean)
                _AMSActive = value
            End Set
        End Property

        Private _ReportMenu As String = ""
        <DataMember()> _
        Public Property ReportMenu() As String
            Get
                Return _ReportMenu
            End Get
            Set(ByVal value As String)
                _ReportMenu = value
            End Set
        End Property

        Private _ReportUpdated As Byte()
        <DataMember()> _
        Public Property ReportUpdated() As Byte()
            Get
                Return _ReportUpdated
            End Get
            Set(ByVal value As Byte())
                _ReportUpdated = value
            End Set
        End Property

        Private _ReportReportMenuControl As Integer = 0
        <DataMember()> _
        Public Property ReportReportMenuControl() As Integer
            Get
                Return _ReportReportMenuControl
            End Get
            Set(ByVal value As Integer)
                _ReportReportMenuControl = value
            End Set
        End Property

        Private _ReportMenuSequence As Integer = 0
        <DataMember()> _
        Public Property ReportMenuSequence() As Integer
            Get
                Return _ReportMenuSequence
            End Get
            Set(ByVal value As Integer)
                _ReportMenuSequence = value
            End Set
        End Property

        Private _ReportSecurityXrefControl As Integer = 0
        <DataMember()> _
        Public Property ReportSecurityXrefControl() As Integer
            Get
                Return _ReportSecurityXrefControl
            End Get
            Set(ByVal value As Integer)
                _ReportSecurityXrefControl = value
            End Set
        End Property

        Private _ReportSecurityGroupXrefControl As Integer = 0
        <DataMember()> _
        Public Property ReportSecurityGroupXrefControl() As Integer
            Get
                Return _ReportSecurityGroupXrefControl
            End Get
            Set(ByVal value As Integer)
                _ReportSecurityGroupXrefControl = value
            End Set
        End Property

        Private _tblReportPars As List(Of tblReportPar)
        <DataMember()> _
        Public Property tblReportPars() As List(Of tblReportPar)
            Get
                Return _tblReportPars
            End Get
            Set(ByVal value As List(Of tblReportPar))
                _tblReportPars = value
            End Set
        End Property

        Private _ReportUserOverrideGroup As Boolean = False
        <DataMember()> _
        Public Property ReportUserOverrideGroup() As Boolean
            Get
                Return _ReportUserOverrideGroup
            End Get
            Set(ByVal value As Boolean)
                _ReportUserOverrideGroup = value
            End Set
        End Property

        Private _ReportNEXTrackActive As Boolean = True
        <DataMember()> _
        Public Property ReportNEXTrackActive() As Boolean
            Get
                Return _ReportNEXTrackActive
            End Get
            Set(ByVal value As Boolean)
                _ReportNEXTrackActive = value
            End Set
        End Property

        Private _ReportShowInTree As Boolean = True
        <DataMember()> _
        Public Property ReportShowInTree() As Boolean
            Get
                Return _ReportShowInTree
            End Get
            Set(ByVal value As Boolean)
                _ReportShowInTree = value
            End Set
        End Property



#End Region

#Region " Public Methods"
        Public Overrides Function Clone() As DTOBaseClass
            Dim instance As New tblReportList
            instance = DirectCast(MemberwiseClone(), tblReportList)
            instance.tblReportPars = Nothing
            For Each item In tblReportPars
                instance.tblReportPars.Add(DirectCast(item.Clone, tblReportPar))
            Next
            Return instance
        End Function

#End Region
    End Class
End Namespace