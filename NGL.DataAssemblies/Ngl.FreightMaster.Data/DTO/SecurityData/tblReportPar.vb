Imports Microsoft.VisualBasic
Imports System
Imports System.Collections.Generic
Imports System.Linq
Imports System.Text
Imports System.Runtime.Serialization
Imports Ngl.Core.ChangeTracker

Namespace DataTransferObjects
    <DataContract()> _
    Public Class tblReportPar
        Inherits DTOBaseClass

#Region " Data Members"

        Private _ReportParControl As Integer = 0
        <DataMember()> _
        Public Property ReportParControl() As Integer
            Get
                Return _ReportParControl
            End Get
            Set(ByVal value As Integer)
                _ReportParControl = value
            End Set
        End Property

        Private _ReportParReportControl As Integer = 0
        <DataMember()> _
        Public Property ReportParReportControl() As Integer
            Get
                Return _ReportParReportControl
            End Get
            Set(ByVal value As Integer)
                _ReportParReportControl = value
            End Set
        End Property

        Private _ReportParReportParTypeControl As Integer = 0
        <DataMember()> _
        Public Property ReportParReportParTypeControl() As Integer
            Get
                Return _ReportParReportParTypeControl
            End Get
            Set(ByVal value As Integer)
                _ReportParReportParTypeControl = value
            End Set
        End Property

        Private _ReportParName As String = ""
        <DataMember()> _
        Public Property ReportParName() As String
            Get
                Return Left(_ReportParName, 50)
            End Get
            Set(ByVal value As String)
                _ReportParName = Left(value, 50)
            End Set
        End Property

        Private _ReportParText As String = ""
        <DataMember()> _
        Public Property ReportParText() As String
            Get
                Return Left(_ReportParText, 50)
            End Get
            Set(ByVal value As String)
                _ReportParText = Left(value, 50)
            End Set
        End Property

        Private _ReportParSource As String = ""
        <DataMember()> _
        Public Property ReportParSource() As String
            Get
                Return Left(_ReportParSource, 255)
            End Get
            Set(ByVal value As String)
                _ReportParSource = Left(value, 255)
            End Set
        End Property

        Private _ReportParValueField As String = ""
        <DataMember()> _
        Public Property ReportParValueField() As String
            Get
                Return Left(_ReportParValueField, 50)
            End Get
            Set(ByVal value As String)
                _ReportParValueField = Left(value, 50)
            End Set
        End Property

        Private _ReportParTextField As String = ""
        <DataMember()> _
        Public Property ReportParTextField() As String
            Get
                Return Left(_ReportParTextField, 50)
            End Get
            Set(ByVal value As String)
                _ReportParTextField = Left(value, 50)
            End Set
        End Property

        Private _ReportParSortOrder As Integer = 0
        <DataMember()> _
        Public Property ReportParSortOrder() As Integer
            Get
                Return _ReportParSortOrder
            End Get
            Set(ByVal value As Integer)
                _ReportParSortOrder = value
            End Set
        End Property

        Private _ReportParApplyUserName As boolean = true
        <DataMember()> _
        Public Property ReportParApplyUserName() As Boolean
            Get
                Return _ReportParApplyUserName
            End Get
            Set(ByVal value As Boolean)
                _ReportParApplyUserName = value
            End Set
        End Property

        Private _ReportParDefaultValue As String = ""
        <DataMember()> _
        Public Property ReportParDefaultValue() As String
            Get
                Return Left(_ReportParDefaultValue, 50)
            End Get
            Set(ByVal value As String)
                _ReportParDefaultValue = Left(value, 50)
            End Set
        End Property

        Private _ReportParReportName As String = ""
        <DataMember()> _
        Public Property ReportParReportName() As String
            Get
                Return _ReportParReportName
            End Get
            Set(ByVal value As String)
                _ReportParReportName = value
            End Set
        End Property

        Private _ReportParReportParTypeName As String = ""
        <DataMember()> _
        Public Property ReportParReportParTypeName() As String
            Get
                Return _ReportParReportParTypeName
            End Get
            Set(ByVal value As String)
                _ReportParReportParTypeName = value
            End Set
        End Property

        Private _ReportParUpdated As Byte()
        <DataMember()> _
        Public Property ReportParUpdated() As Byte()
            Get
                Return _ReportParUpdated
            End Get
            Set(ByVal value As Byte())
                _ReportParUpdated = value
            End Set
        End Property


#End Region

#Region " Public Methods"
        Public Overrides Function Clone() As DTOBaseClass
            Dim instance As New tblReportPar
            instance = DirectCast(MemberwiseClone(), tblReportPar)
            Return instance
        End Function

#End Region
    End Class
End Namespace
