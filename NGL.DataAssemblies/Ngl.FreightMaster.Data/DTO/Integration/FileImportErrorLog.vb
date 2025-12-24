Imports Microsoft.VisualBasic
Imports System
Imports System.Collections.Generic
Imports System.Linq
Imports System.Text
Imports System.Runtime.Serialization
Imports Ngl.Core.ChangeTracker

Namespace DataTransferObjects
    <DataContract()> _
    Public Class FileImportErrorLog
        Inherits DTOBaseClass

#Region " Data Members"
         
        Private _ImportRecord As String = ""
        <DataMember()> _
        Public Property ImportRecord() As String
            Get
                Return Me._ImportRecord
            End Get
            Set(ByVal value As String)
                If (String.Equals(Me._ImportRecord, value) = False) Then
                    Me._ImportRecord = value
                    Me.SendPropertyChanged("ImportRecord")
                End If
            End Set
        End Property

        Private _CreateUser As String = ""
        <DataMember()> _
        Public Property CreateUser() As String
            Get
                Return Left(Me._CreateUser, 25)
            End Get
            Set(ByVal value As String)
                If (String.Equals(Me._CreateUser, value) = False) Then
                    Me._CreateUser = Left(value, 25)
                    Me.SendPropertyChanged("CreateUser")
                End If
            End Set
        End Property

        Private _ErrorDate As System.Nullable(Of Date)
        <DataMember()> _
        Public Property ErrorDate() As System.Nullable(Of Date)
            Get
                Return _ErrorDate
            End Get
            Set(ByVal value As System.Nullable(Of Date))
                _ErrorDate = value
            End Set
        End Property

        Private _ErrorMsg As String = ""
        <DataMember()> _
        Public Property ErrorMsg() As String
            Get
                Return Left(Me._ErrorMsg, 500)
            End Get
            Set(ByVal value As String)
                If (String.Equals(Me._ErrorMsg, value) = False) Then
                    Me._ErrorMsg = Left(value, 500)
                    Me.SendPropertyChanged("ErrorMsg")
                End If
            End Set
        End Property


        Private _ImportFileName As String = ""
        <DataMember()> _
        Public Property ImportFileName() As String
            Get
                Return Left(Me._ImportFileName, 255)
            End Get
            Set(ByVal value As String)
                If (String.Equals(Me._ImportFileName, value) = False) Then
                    Me._ImportFileName = Left(value, 255)
                    Me.SendPropertyChanged("ImportFileName")
                End If
            End Set
        End Property

        Private _ImportFileType As Integer = 0
        <DataMember()> _
        Public Property ImportFileType() As Integer
            Get
                Return _ImportFileType
            End Get
            Set(ByVal value As Integer)
                _ImportFileType = value
            End Set
        End Property

        Private _ImportName As String = ""
        <DataMember()> _
        Public Property ImportName() As String
            Get
                Return Left(Me._ImportName, 50)
            End Get
            Set(ByVal value As String)
                If (String.Equals(Me._ImportName, value) = False) Then
                    Me._ImportName = Left(value, 50)
                    Me.SendPropertyChanged("ImportName")
                End If
            End Set
        End Property

#End Region

#Region " Public Methods"
        Public Overrides Function Clone() As DTOBaseClass
            Dim instance As New FileImportErrorLog
            instance = DirectCast(MemberwiseClone(), FileImportErrorLog)
            Return instance
        End Function

#End Region

    End Class
End Namespace
