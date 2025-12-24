Imports Microsoft.VisualBasic
Imports System
Imports System.Collections.Generic
Imports System.Linq
Imports System.Text
Imports System.Runtime.Serialization
Imports Ngl.Core.ChangeTracker

Namespace DataTransferObjects
    <DataContract()> _
    Public Class BatchProcessingResults
        Inherits DTOBaseClass

#Region " Data Members"

        Private _Key As Integer = 0
        <DataMember()> _
        Public Property Key() As Integer
            Get
                Return _Key
            End Get
            Set(ByVal value As Integer)
                _Key = value
            End Set
        End Property

        Private _KeyFields As Dictionary(Of String, String)
        <DataMember()> _
        Public Property KeyFields() As Dictionary(Of String, String)
            Get
                If _KeyFields Is Nothing Then _KeyFields = New Dictionary(Of String, String)
                Return _KeyFields
            End Get
            Set(ByVal value As Dictionary(Of String, String))
                _KeyFields = value
            End Set
        End Property

        Private _AlphaCode As String
        <DataMember()> _
        Public Property AlphaCode() As String
            Get
                Return _AlphaCode
            End Get
            Set(ByVal value As String)
                _AlphaCode = value
            End Set
        End Property

        Private _Success As Boolean = False
        <DataMember()> _
        Public Property Success() As Boolean
            Get
                Return _Success
            End Get
            Set(ByVal value As Boolean)
                _Success = value
            End Set
        End Property

        Private _intValues As New List(Of Integer)
        <DataMember()> _
        Public Property intValues() As List(Of Integer)
            Get
                Return _intValues
            End Get
            Set(ByVal value As List(Of Integer))
                _intValues = value
            End Set
        End Property

        Private _dblValues As New List(Of Double)
        <DataMember()> _
        Public Property dblValues() As List(Of Double)
            Get
                Return _dblValues
            End Get
            Set(ByVal value As List(Of Double))
                _dblValues = value
            End Set
        End Property

        Private _dtValues As New List(Of Date)
        <DataMember()> _
        Public Property dtValues() As List(Of Date)
            Get
                Return _dtValues
            End Get
            Set(ByVal value As List(Of Date))
                _dtValues = value
            End Set
        End Property

        Private _strValues As New List(Of String)
        <DataMember()> _
        Public Property strValues() As List(Of String)
            Get
                Return _strValues
            End Get
            Set(ByVal value As List(Of String))
                _strValues = value
            End Set
        End Property

        Private _Errors As New List(Of String)
        <DataMember()> _
        Public Property Errors() As List(Of String)
            Get
                Return _Errors
            End Get
            Set(ByVal value As List(Of String))
                _Errors = value
            End Set
        End Property

        Private _Warnings As New List(Of String)
        <DataMember()> _
        Public Property Warnings() As List(Of String)
            Get
                Return _Warnings
            End Get
            Set(ByVal value As List(Of String))
                _Warnings = value
            End Set
        End Property

        Private _Messages As New List(Of String)
        <DataMember()> _
        Public Property Messages() As List(Of String)
            Get
                Return _Messages
            End Get
            Set(ByVal value As List(Of String))
                _Messages = value
            End Set
        End Property

        Private _DTOData As DTOBaseClass()
        <DataMember()> _
        Public Property DTOData() As DTOBaseClass()
            Get
                Return _DTOData
            End Get
            Set(ByVal value As DTOBaseClass())
                _DTOData = value
            End Set
        End Property


#End Region

#Region " Public Methods"
        Public Overrides Function Clone() As DTOBaseClass
            Dim instance As New BatchProcessingResults
            instance = DirectCast(MemberwiseClone(), DTOBaseClass)
            instance.DTOData = Nothing
            If Not DTOData Is Nothing Then
                instance.DTOData = DTOData.Clone()
            End If
            Return instance
        End Function

        Public Function getErrorsAsSingleStr(ByVal delimiter As String)
            Dim result As String = ""
            For i As Integer = 0 To Me.Errors.Count - 1
                If i = 0 Then
                    result = result & Me.Errors(i)
                Else
                    result = result & " " & delimiter & " " & vbCrLf & Me.Errors(i)
                End If
            Next
            Return result
        End Function
#End Region

    End Class

End Namespace
