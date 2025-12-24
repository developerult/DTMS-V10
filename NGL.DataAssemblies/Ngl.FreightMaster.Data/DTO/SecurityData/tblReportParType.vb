Imports Microsoft.VisualBasic
Imports System
Imports System.Collections.Generic
Imports System.Linq
Imports System.Text
Imports System.Runtime.Serialization
Imports Ngl.Core.ChangeTracker

Namespace DataTransferObjects
    <DataContract()> _
    Public Class tblReportParType
        Inherits DTOBaseClass

#Region " Data Members"

        Private _ReportParTypeControl As Integer = 0
        <DataMember()> _
        Public Property ReportParTypeControl() As Integer
            Get
                Return _ReportParTypeControl
            End Get
            Set(ByVal value As Integer)
                _ReportParTypeControl = value
            End Set
        End Property

        Private _ReportParTypeName As String = ""
        <DataMember()> _
        Public Property ReportParTypeName() As String
            Get
                Return Left(_ReportParTypeName, 50)
            End Get
            Set(ByVal value As String)
                _ReportParTypeName = Left(value, 50)
            End Set
        End Property

        Private _ReportParTypeDesc As String = ""
        <DataMember()> _
        Public Property ReportParTypeDesc() As String
            Get
                Return Left(_ReportParTypeDesc, 255)
            End Get
            Set(ByVal value As String)
                _ReportParTypeDesc = Left(value, 255)
            End Set
        End Property

        Private _ReportParTypeUpdated As Byte()
        <DataMember()> _
        Public Property ReportParTypeUpdated() As Byte()
            Get
                Return _ReportParTypeUpdated
            End Get
            Set(ByVal value As Byte())
                _ReportParTypeUpdated = value
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

#End Region

#Region " Public Methods"
        Public Overrides Function Clone() As DTOBaseClass
            Dim instance As New tblReportParType
            instance = DirectCast(MemberwiseClone(), tblReportParType)
            instance.tblReportPars = Nothing
            For Each item In tblReportPars
                instance.tblReportPars.Add(DirectCast(item.Clone, tblReportPar))
            Next
            Return instance
        End Function

#End Region
    End Class
End Namespace