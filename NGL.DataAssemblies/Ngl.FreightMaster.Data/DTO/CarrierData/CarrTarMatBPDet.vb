
Imports Microsoft.VisualBasic
Imports System
Imports System.Collections.Generic
Imports System.Linq
Imports System.Text
Imports System.Runtime.Serialization
Imports Ngl.Core.ChangeTracker


Namespace DataTransferObjects
    <DataContract()> _
    Public Class CarrTarMatBPDet
        Inherits DTOBaseClass

#Region " Data Members"

        Private _CarrTarMatBPDetControl As Integer = 0
        <DataMember()> _
        Public Property CarrTarMatBPDetControl() As Integer
            Get
                Return _CarrTarMatBPDetControl
            End Get
            Set(ByVal value As Integer)
                If (Me._CarrTarMatBPDetControl <> value) Then
                    Me._CarrTarMatBPDetControl = value
                    Me.SendPropertyChanged("CarrTarMatBPDetControl")
                End If
            End Set
        End Property

        Private _CarrTarMatBPDetCarrTarMatBPControl As Integer = 0
        <DataMember()> _
        Public Property CarrTarMatBPDetCarrTarMatBPControl() As Integer
            Get
                Return _CarrTarMatBPDetCarrTarMatBPControl
            End Get
            Set(ByVal value As Integer)
                If (Me._CarrTarMatBPDetCarrTarMatBPControl <> value) Then
                    Me._CarrTarMatBPDetCarrTarMatBPControl = value
                    Me.SendPropertyChanged("CarrTarMatBPDetCarrTarMatBPControl")
                End If
            End Set
        End Property

        Private _CarrTarMatBPDetName As String = ""
        <DataMember()> _
        Public Property CarrTarMatBPDetName() As String
            Get
                Return Left(_CarrTarMatBPDetName, 50)
            End Get
            Set(ByVal value As String)
                If (String.Equals(Me._CarrTarMatBPDetName, value) = False) Then
                    Me._CarrTarMatBPDetName = Left(value, 50)
                    Me.SendPropertyChanged("CarrTarMatBPDetName")
                End If
            End Set
        End Property

        Private _CarrTarMatBPDetDesc As String = ""
        <DataMember()> _
        Public Property CarrTarMatBPDetDesc() As String
            Get
                Return Left(_CarrTarMatBPDetDesc, 100)
            End Get
            Set(ByVal value As String)
                If (String.Equals(Me._CarrTarMatBPDetDesc, value) = False) Then
                    Me._CarrTarMatBPDetDesc = Left(value, 100)
                    Me.SendPropertyChanged("CarrTarMatBPDetDesc")
                End If
            End Set
        End Property

        Private _CarrTarMatBPDetID As Integer = 0
        <DataMember()> _
        Public Property CarrTarMatBPDetID() As Integer
            Get
                Return _CarrTarMatBPDetID
            End Get
            Set(ByVal value As Integer)
                If (Me._CarrTarMatBPDetID <> value) Then
                    Me._CarrTarMatBPDetID = value
                    Me.SendPropertyChanged("CarrTarMatBPDetID")
                End If
            End Set
        End Property

        Private _CarrTarMatBPDetValue As System.Nullable(Of Decimal)
        <DataMember()> _
        Public Property CarrTarMatBPDetValue() As System.Nullable(Of Decimal)
            Get
                Return _CarrTarMatBPDetValue
            End Get
            Set(ByVal value As System.Nullable(Of Decimal))
                If (Me._CarrTarMatBPDetValue.Equals(value) = False) Then
                    Me._CarrTarMatBPDetValue = value
                    Me.SendPropertyChanged("CarrTarMatBPDetValue")
                End If
            End Set
        End Property

        Private _CarrTarMatBPDetModUser As String = ""
        <DataMember()> _
        Public Property CarrTarMatBPDetModUser() As String
            Get
                Return Left(_CarrTarMatBPDetModUser, 100)
            End Get
            Set(ByVal value As String)
                If (String.Equals(Me._CarrTarMatBPDetModUser, value) = False) Then
                    Me._CarrTarMatBPDetModUser = Left(value, 100)
                    Me.SendPropertyChanged("CarrTarMatBPDetModUser")
                End If
            End Set
        End Property

        Private _CarrTarMatBPDetModDate As Date = DateTime.Now
        <DataMember()> _
        Public Property CarrTarMatBPDetModDate() As Date
            Get
                Return _CarrTarMatBPDetModDate
            End Get
            Set(ByVal value As Date)
                If (Me._CarrTarMatBPDetModDate.Equals(value) = False) Then
                    Me._CarrTarMatBPDetModDate = value
                    Me.SendPropertyChanged("CarrTarMatBPDetModDate")
                End If
            End Set
        End Property

        Private _CarrTarMatBPDetUpdated As Byte()
        <DataMember()> _
        Public Property CarrTarMatBPDetUpdated() As Byte()
            Get
                Return _CarrTarMatBPDetUpdated
            End Get
            Set(ByVal value As Byte())
                _CarrTarMatBPDetUpdated = value
            End Set
        End Property

#End Region

#Region " Public Methods"
        Public Overrides Function Clone() As DTOBaseClass
            Dim instance As New CarrTarMatBP
            instance = DirectCast(MemberwiseClone(), CarrTarMatBP)
            Return instance
        End Function

#End Region

    End Class
End Namespace