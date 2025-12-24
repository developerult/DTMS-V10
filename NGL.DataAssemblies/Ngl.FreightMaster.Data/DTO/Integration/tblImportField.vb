Imports Microsoft.VisualBasic
Imports System
Imports System.Collections.Generic
Imports System.Linq
Imports System.Text
Imports System.Runtime.Serialization
Imports Ngl.Core.ChangeTracker

Namespace DataTransferObjects
    <DataContract()> _
    Public Class tblImportField
        Inherits DTOBaseClass

#Region " Data Members"

        Private _ImportControl As Integer = 0
        <DataMember()> _
        Public Property ImportControl() As Integer
            Get
                Return Me._ImportControl
            End Get
            Set(ByVal value As Integer)
                If ((Me._ImportControl = value) _
                   = False) Then
                    Me._ImportControl = value
                    Me.SendPropertyChanged("ImportControl")
                End If
            End Set
        End Property

        Private _ImportFieldName As String = ""
        <DataMember()> _
        Public Property ImportFieldName() As String
            Get
                Return Left(Me._ImportFieldName, 100)
            End Get
            Set(ByVal value As String)
                If (String.Equals(Me._ImportFieldName, value) = False) Then
                    Me._ImportFieldName = Left(value, 100)
                    Me.SendPropertyChanged("ImportFieldName")
                End If
            End Set
        End Property


        Private _ImportFileType As Integer = 0
        <DataMember()> _
        Public Property ImportFileType() As Integer
            Get
                Return Me._ImportFileType
            End Get
            Set(ByVal value As Integer)
                If ((Me._ImportFileType = value) _
                   = False) Then
                    Me._ImportFileType = value
                    Me.SendPropertyChanged("ImportFileType")
                End If
            End Set
        End Property


        Private _ImportFlag As Boolean = True
        <DataMember()> _
        Public Property ImportFlag() As Boolean
            Get
                Return Me._ImportFlag
            End Get
            Set(ByVal value As Boolean)
                If ((Me._ImportFlag = value) _
                   = False) Then
                    Me._ImportFlag = value
                    Me.SendPropertyChanged("ImportFlag")
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


        Private _ImportUpdated As Byte()
        <DataMember()> _
        Public Property ImportUpdated() As Byte()
            Get
                Return _ImportUpdated
            End Get
            Set(ByVal value As Byte())
                _ImportUpdated = value
            End Set
        End Property

#End Region

#Region " Public Methods"
        Public Overrides Function Clone() As DTOBaseClass
            Dim instance As New tblImportField
            instance = DirectCast(MemberwiseClone(), tblImportField)
            Return instance
        End Function

#End Region

    End Class
End Namespace
