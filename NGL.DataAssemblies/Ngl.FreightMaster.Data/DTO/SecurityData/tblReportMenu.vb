Imports Microsoft.VisualBasic
Imports System
Imports System.Collections.Generic
Imports System.Linq
Imports System.Text
Imports System.Runtime.Serialization
Imports Ngl.Core.ChangeTracker

Namespace DataTransferObjects
    <DataContract()> _
    Public Class tblReportMenu
        Inherits DTOBaseClass

#Region " Data Members"

        Private _ReportMenuControl As Integer = 0
        <DataMember()> _
        Public Property ReportMenuControl() As Integer
            Get
                Return _ReportMenuControl
            End Get
            Set(ByVal value As Integer)
                _ReportMenuControl = value
            End Set
        End Property

        Private _ReportMenuName As String = ""
        <DataMember()> _
        Public Property ReportMenuName() As String
            Get
                Return Left(_ReportMenuName, 100)
            End Get
            Set(ByVal value As String)
                _ReportMenuName = Left(value, 100)
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

        Private _ReportMenuDescription As String = ""
        <DataMember()> _
        Public Property ReportMenuDescription() As String
            Get
                Return Left(_ReportMenuDescription, 50)
            End Get
            Set(ByVal value As String)
                _ReportMenuDescription = Left(value, 50)
            End Set
        End Property

        Private _ReportMenuUpdated As Byte()
        <DataMember()> _
        Public Property ReportMenuUpdated() As Byte()
            Get
                Return _ReportMenuUpdated
            End Get
            Set(ByVal value As Byte())
                _ReportMenuUpdated = value
            End Set
        End Property

#End Region

#Region " Public Methods"
        Public Overrides Function Clone() As DTOBaseClass
            Dim instance As New tblReportMenu
            instance = DirectCast(MemberwiseClone(), tblReportMenu)
            Return instance
        End Function

#End Region
    End Class
End Namespace

