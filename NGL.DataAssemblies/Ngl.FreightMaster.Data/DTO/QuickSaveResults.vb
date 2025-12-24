Imports Microsoft.VisualBasic
Imports System
Imports System.Collections.Generic
Imports System.Linq
Imports System.Text
Imports System.Runtime.Serialization
Imports Ngl.Core.ChangeTracker

Namespace DataTransferObjects
    <DataContract()> _
    Public Class QuickSaveResults

#Region " Data Members"
        Private _Control As Integer = 0
        <DataMember()> _
        Public Property Control() As Integer
            Get
                Return _Control
            End Get
            Set(ByVal value As Integer)
                _Control = value
            End Set
        End Property

        Private _AltKey As String = ""
        <DataMember()> _
        Public Property AltKey() As String
            Get
                Return _AltKey
            End Get
            Set(ByVal value As String)
                _AltKey = value
            End Set
        End Property

        Private _ModDate As Date
        <DataMember()> _
        Public Property ModDate() As Date
            Get
                Return _ModDate
            End Get
            Set(ByVal value As Date)
                _ModDate = value
            End Set
        End Property

        Private _ModUser As String = ""
        <DataMember()> _
        Public Property ModUser() As String
            Get
                Return Left(_ModUser, 100)
            End Get
            Set(ByVal value As String)
                _ModUser = Left(value, 100)
            End Set
        End Property

        Private _Updated As Byte()
        <DataMember()> _
        Public Property Updated() As Byte()
            Get
                Return _Updated
            End Get
            Set(ByVal value As Byte())
                _Updated = value
            End Set
        End Property

#End Region

    End Class

End Namespace
