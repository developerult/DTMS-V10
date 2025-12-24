Imports Microsoft.VisualBasic
Imports System
Imports System.Collections.Generic
Imports System.Linq
Imports System.Text
Imports System.Runtime.Serialization
Imports Ngl.Core.ChangeTracker

Namespace DataTransferObjects
    <DataContract()> _
    Public Class Currency
        Inherits DTOBaseClass


#Region " Data Members"
        Private _ID As Integer = 0
        <DataMember()> _
        Public Property ID() As Integer
            Get
                Return _ID
            End Get
            Set(ByVal value As Integer)
                _ID = value
            End Set
        End Property

        Private _CurrencyType As String = ""
        <DataMember()> _
        Public Property CurrencyType() As String
            Get
                Return Left(_CurrencyType, 3)
            End Get
            Set(ByVal value As String)
                _CurrencyType = Left(value, 3)
            End Set
        End Property

        Private _CurrencyName As String = ""
        <DataMember()> _
        Public Property CurrencyName() As String
            Get
                Return Left(_CurrencyName, 50)
            End Get
            Set(ByVal value As String)
                _CurrencyName = Left(value, 50)
            End Set
        End Property

        Private _CurrencyCountry As String = ""
        <DataMember()> _
        Public Property CurrencyCountry() As String
            Get
                Return Left(_CurrencyCountry, 50)
            End Get
            Set(ByVal value As String)
                _CurrencyCountry = Left(value, 50)
            End Set
        End Property

        Private _CurrencyUpdated As Byte()
        <DataMember()> _
        Public Property CurrencyUpdated() As Byte()
            Get
                Return _CurrencyUpdated
            End Get
            Set(ByVal value As Byte())
                _CurrencyUpdated = value
            End Set
        End Property


#End Region

#Region " Public Methods"
        Public Overrides Function Clone() As DTOBaseClass
            Dim instance As New Currency
            instance = DirectCast(MemberwiseClone(), Currency)
            Return instance
        End Function

#End Region

    End Class
End Namespace
