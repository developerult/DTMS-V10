Imports Microsoft.VisualBasic
Imports System
Imports System.Collections.Generic
Imports System.Linq
Imports System.Text
Imports System.Runtime.Serialization
Imports Ngl.Core.ChangeTracker

Namespace DataTransferObjects
    <DataContract()> _
    Public Class BookReferenceData
        Inherits DTOBaseClass


#Region " Data Members"

        Private _BookControl As Integer
        <DataMember()> _
        Public Property BookControl() As Integer
            Get
                Return _BookControl
            End Get
            Set(ByVal value As Integer)
                _BookControl = value
            End Set
        End Property

        Private _BookProNumber As String
        <DataMember()> _
        Public Property BookProNumber() As String
            Get
                Return _BookProNumber
            End Get
            Set(ByVal value As String)
                _BookProNumber = value
            End Set
        End Property

        Private _BookCarrOrderNumber As String
        Public Property BookCarrOrderNumber() As String
            Get
                Return _BookCarrOrderNumber
            End Get
            Set(ByVal value As String)
                _BookCarrOrderNumber = value
            End Set
        End Property

        Private _BookOrderSequence As Integer
        <DataMember()> _
        Public Property BookOrderSequence() As Integer
            Get
                Return _BookOrderSequence
            End Get
            Set(ByVal value As Integer)
                _BookOrderSequence = value
            End Set
        End Property

        Private _BookConsPrefix As String = ""
        <DataMember()> _
        Public Property BookConsPrefix() As String
            Get
                Return Left(_BookConsPrefix, 20)
            End Get
            Set(ByVal value As String)
                _BookConsPrefix = Left(value, 20)
            End Set
        End Property

        Private _BookRouteConsFlag As Boolean = False
        <DataMember()> _
        Public Property BookRouteConsFlag() As Boolean
            Get
                Return _BookRouteConsFlag
            End Get
            Set(ByVal value As Boolean)
                _BookRouteConsFlag = value
            End Set
        End Property

        Private _BookCustCompControl As Integer = 0
        <DataMember()> _
        Public Property BookCustCompControl() As Integer
            Get
                Return _BookCustCompControl
            End Get
            Set(ByVal value As Integer)
                _BookCustCompControl = value
            End Set
        End Property

        Private _Results As WCFResults
        Public Property Results() As WCFResults
            Get
                If _Results Is Nothing Then _Results = New WCFResults()
                Return _Results
            End Get
            Set(ByVal value As WCFResults)
                _Results = value
            End Set
        End Property


        'Private _Errors As New List(Of String)
        '<DataMember()> _
        'Public Property Errors() As List(Of String)
        '    Get
        '        Return _Errors
        '    End Get
        '    Set(ByVal value As List(Of String))
        '        _Errors = value
        '    End Set
        'End Property

        'Private _Warnings As New List(Of String)
        '<DataMember()> _
        'Public Property Warnings() As List(Of String)
        '    Get
        '        Return _Warnings
        '    End Get
        '    Set(ByVal value As List(Of String))
        '        _Warnings = value
        '    End Set
        'End Property

        'Private _Messages As New List(Of String)
        '<DataMember()> _
        'Public Property Messages() As List(Of String)
        '    Get
        '        Return _Messages
        '    End Get
        '    Set(ByVal value As List(Of String))
        '        _Messages = value
        '    End Set
        'End Property

#End Region

#Region " Public Methods"

        Public Overrides Function Clone() As DTOBaseClass
            Dim instance As New BookReferenceData
            instance = DirectCast(MemberwiseClone(), BookReferenceData)
            Return instance
        End Function

        'Public Function getErrorsAsSingleStr(ByVal delimiter As String)
        '    Dim result As String = ""
        '    For i As Integer = 0 To Me.Errors.Count - 1
        '        If i = 0 Then
        '            result = result & Me.Errors(i)
        '        Else
        '            result = result & " " & delimiter & " " & vbCrLf & Me.Errors(i)
        '        End If
        '    Next
        '    Return result
        'End Function

#End Region

    End Class
End Namespace
