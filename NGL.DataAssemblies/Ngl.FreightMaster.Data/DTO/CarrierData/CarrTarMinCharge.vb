Imports Microsoft.VisualBasic
Imports System
Imports System.Collections.Generic
Imports System.Linq
Imports System.Text
Imports System.Runtime.Serialization
Imports Ngl.Core.ChangeTracker

Namespace DataTransferObjects
    <DataContract()> _
    Public Class CarrTarMinCharge
        Inherits DTOBaseClass


#Region " Data Members"

        Private _CarrTarMinChargeControl As Integer
        <DataMember()> _
        Public Property CarrTarMinChargeControl() As Integer
            Get
                Return Me._CarrTarMinChargeControl
            End Get
            Set(value As Integer)
                If ((Me._CarrTarMinChargeControl = value) _
                            = False) Then
                    Me._CarrTarMinChargeControl = value
                    Me.SendPropertyChanged("CarrTarMinChargeControl")
                End If
            End Set
        End Property

        Private _CarrTarMinChargeCarrTarControl As Integer
        <DataMember()> _
        Public Property CarrTarMinChargeCarrTarControl() As Integer
            Get
                Return Me._CarrTarMinChargeCarrTarControl
            End Get
            Set(value As Integer)
                If ((Me._CarrTarMinChargeCarrTarControl = value) _
                            = False) Then
                    Me._CarrTarMinChargeCarrTarControl = value
                    Me.SendPropertyChanged("CarrTarMinChargeCarrTarControl")
                End If
            End Set
        End Property

        Private _CarrTarMinChargePointTypeControl As Integer
        <DataMember()> _
        Public Property CarrTarMinChargePointTypeControl() As Integer
            Get
                Return Me._CarrTarMinChargePointTypeControl
            End Get
            Set(value As Integer)
                If ((Me._CarrTarMinChargePointTypeControl = value) _
                            = False) Then
                    Me._CarrTarMinChargePointTypeControl = value
                    Me.SendPropertyChanged("CarrTarMinChargePointTypeControl")
                End If
            End Set
        End Property

        Private _CarrTarMinChargeName As String
        <DataMember()> _
        Public Property CarrTarMinChargeName() As String
            Get
                Return Left(Me._CarrTarMinChargeName, 50)
            End Get
            Set(value As String)
                If (String.Equals(Me._CarrTarMinChargeName, value) = False) Then
                    Me._CarrTarMinChargeName = Left(value, 50)
                    Me.SendPropertyChanged("CarrTarMinChargeName")
                End If
            End Set
        End Property

        Private _CarrTarMinChargeEffDateFrom As System.Nullable(Of Date)
        <DataMember()> _
        Public Property CarrTarMinChargeEffDateFrom() As System.Nullable(Of Date)
            Get
                Return Me._CarrTarMinChargeEffDateFrom
            End Get
            Set(value As System.Nullable(Of Date))
                If (Me._CarrTarMinChargeEffDateFrom.Equals(value) = False) Then
                    Me._CarrTarMinChargeEffDateFrom = value
                    Me.SendPropertyChanged("CarrTarMinChargeEffDateFrom")

                End If
            End Set
        End Property

        Private _CarrTarMinChargeEffDateTo As System.Nullable(Of Date)
        <DataMember()> _
        Public Property CarrTarMinChargeEffDateTo() As System.Nullable(Of Date)
            Get
                Return Me._CarrTarMinChargeEffDateTo
            End Get
            Set(value As System.Nullable(Of Date))
                If (Me._CarrTarMinChargeEffDateTo.Equals(value) = False) Then
                    Me._CarrTarMinChargeEffDateTo = value
                    Me.SendPropertyChanged("CarrTarMinChargeEffDateTo")
                End If
            End Set
        End Property

        Private _CarrTarMinChargeCountry As String
        <DataMember()> _
        Public Property CarrTarMinChargeCountry() As String
            Get
                Return Left(Me._CarrTarMinChargeCountry, 30)
            End Get
            Set(value As String)
                If (String.Equals(Me._CarrTarMinChargeCountry, value) = False) Then
                    Me._CarrTarMinChargeCountry = Left(value, 30)
                    Me.SendPropertyChanged("CarrTarMinChargeCountry")
                End If
            End Set
        End Property

        Private _CarrTarMinChargeState As String
        <DataMember()> _
        Public Property CarrTarMinChargeState() As String
            Get
                Return Left(Me._CarrTarMinChargeState, 2)
            End Get
            Set(value As String)
                If (String.Equals(Me._CarrTarMinChargeState, value) = False) Then
                    Me._CarrTarMinChargeState = Left(value, 2)
                    Me.SendPropertyChanged("CarrTarMinChargeState")
                End If
            End Set
        End Property

        Private _CarrTarMinChargeCity As String
        <DataMember()> _
        Public Property CarrTarMinChargeCity() As String
            Get
                Return Left(Me._CarrTarMinChargeCity, 50)
            End Get
            Set(value As String)
                If (String.Equals(Me._CarrTarMinChargeCity, value) = False) Then
                    Me._CarrTarMinChargeCity = Left(value, 50)
                    Me.SendPropertyChanged("CarrTarMinChargeCity")
                End If
            End Set
        End Property

        Private _CarrTarMinChargeZipFrom As String
        <DataMember()> _
        Public Property CarrTarMinChargeZipFrom() As String
            Get
                Return Left(Me._CarrTarMinChargeZipFrom, 10)
            End Get
            Set(value As String)
                If (String.Equals(Me._CarrTarMinChargeZipFrom, value) = False) Then
                    Me._CarrTarMinChargeZipFrom = Left(value, 10)
                    Me.SendPropertyChanged("CarrTarMinChargeZipFrom")
                End If
            End Set
        End Property

        Private _CarrTarMinChargeZipTo As String
        <DataMember()> _
        Public Property CarrTarMinChargeZipTo() As String
            Get
                Return Left(Me._CarrTarMinChargeZipTo, 10)
            End Get
            Set(value As String)
                If (String.Equals(Me._CarrTarMinChargeZipTo, value) = False) Then
                    Me._CarrTarMinChargeZipTo = Left(value, 10)
                    Me.SendPropertyChanged("CarrTarMinChargeZipTo")
                End If
            End Set
        End Property

        Private _CarrTarMinChargeValue As Decimal
        <DataMember()> _
        Public Property CarrTarMinChargeValue() As Decimal
            Get
                Return Me._CarrTarMinChargeValue
            End Get
            Set(value As Decimal)
                If ((Me._CarrTarMinChargeValue = value) _
                            = False) Then
                    Me._CarrTarMinChargeValue = value
                    Me.SendPropertyChanged("CarrTarMinChargeValue")
                End If
            End Set
        End Property

        Private _CarrTarMinChargeModUser As String = ""
        <DataMember()> _
        Public Property CarrTarMinChargeModUser() As String
            Get
                Return Left(_CarrTarMinChargeModUser, 100)
            End Get
            Set(ByVal value As String)
                If (String.Equals(Me._CarrTarMinChargeModUser, value) = False) Then
                    Me._CarrTarMinChargeModUser = Left(value, 100)
                    Me.SendPropertyChanged("CarrTarMinChargeModUser")
                End If
            End Set
        End Property

        Private _CarrTarMinChargeModDate As Date = DateTime.Now
        <DataMember()> _
        Public Property CarrTarMinChargeModDate() As Date
            Get
                Return _CarrTarMinChargeModDate
            End Get
            Set(ByVal value As Date)
                If (Me._CarrTarMinChargeModDate.Equals(value) = False) Then
                    Me._CarrTarMinChargeModDate = value
                    Me.SendPropertyChanged("CarrTarMinChargeModDate")
                End If
            End Set
        End Property

        Private _CarrTarMinChargeUpdated As Byte()
        <DataMember()> _
        Public Property CarrTarMinChargeUpdated() As Byte()
            Get
                Return _CarrTarMinChargeUpdated
            End Get
            Set(ByVal value As Byte())
                _CarrTarMinChargeUpdated = value
            End Set
        End Property

#End Region

#Region " Public Methods"
        Public Overrides Function Clone() As DTOBaseClass
            Dim instance As New CarrTarMinCharge
            instance = DirectCast(MemberwiseClone(), CarrTarMinCharge)
            Return instance
        End Function

#End Region

    End Class
End Namespace