Imports Microsoft.VisualBasic
Imports System
Imports System.Collections.Generic
Imports System.Linq
Imports System.Text
Imports System.Runtime.Serialization
Imports Ngl.Core.ChangeTracker

Namespace DataTransferObjects
    <DataContract()> _
    Public Class CarrTarDiscount
        Inherits DTOBaseClass


#Region " Data Members"

        Private _CarrTarDiscountControl As Integer
        <DataMember()> _
        Public Property CarrTarDiscountControl() As Integer
            Get
                Return Me._CarrTarDiscountControl
            End Get
            Set(value As Integer)
                If ((Me._CarrTarDiscountControl = value) _
                            = False) Then
                    Me._CarrTarDiscountControl = value
                    Me.SendPropertyChanged("CarrTarDiscountControl")
                End If
            End Set
        End Property

        Private _CarrTarDiscountCarrTarControl As Integer
        <DataMember()> _
        Public Property CarrTarDiscountCarrTarControl() As Integer
            Get
                Return Me._CarrTarDiscountCarrTarControl
            End Get
            Set(value As Integer)
                If ((Me._CarrTarDiscountCarrTarControl = value) _
                            = False) Then
                    Me._CarrTarDiscountCarrTarControl = value
                    Me.SendPropertyChanged("CarrTarDiscountCarrTarControl")
                End If
            End Set
        End Property

        Private _CarrTarDiscountPointTypeControl As Integer
        <DataMember()> _
        Public Property CarrTarDiscountPointTypeControl() As Integer
            Get
                Return Me._CarrTarDiscountPointTypeControl
            End Get
            Set(value As Integer)
                If ((Me._CarrTarDiscountPointTypeControl = value) _
                            = False) Then
                    Me._CarrTarDiscountPointTypeControl = value
                    Me.SendPropertyChanged("CarrTarDiscountPointTypeControl")
                End If
            End Set
        End Property

        Private _CarrTarDiscountName As String
        <DataMember()> _
        Public Property CarrTarDiscountName() As String
            Get
                Return Left(Me._CarrTarDiscountName, 50)
            End Get
            Set(value As String)
                If (String.Equals(Me._CarrTarDiscountName, value) = False) Then
                    Me._CarrTarDiscountName = Left(value, 50)
                    Me.SendPropertyChanged("CarrTarDiscountName")
                End If
            End Set
        End Property

        Private _CarrTarDiscountEffDateFrom As System.Nullable(Of Date)
        <DataMember()> _
        Public Property CarrTarDiscountEffDateFrom() As System.Nullable(Of Date)
            Get
                Return Me._CarrTarDiscountEffDateFrom
            End Get
            Set(value As System.Nullable(Of Date))
                If (Me._CarrTarDiscountEffDateFrom.Equals(value) = False) Then
                    Me._CarrTarDiscountEffDateFrom = value
                    Me.SendPropertyChanged("CarrTarDiscountEffDateFrom")

                End If
            End Set
        End Property

        Private _CarrTarDiscountEffDateTo As System.Nullable(Of Date)
        <DataMember()> _
        Public Property CarrTarDiscountEffDateTo() As System.Nullable(Of Date)
            Get
                Return Me._CarrTarDiscountEffDateTo
            End Get
            Set(value As System.Nullable(Of Date))
                If (Me._CarrTarDiscountEffDateTo.Equals(value) = False) Then
                    Me._CarrTarDiscountEffDateTo = value
                    Me.SendPropertyChanged("CarrTarDiscountEffDateTo")
                End If
            End Set
        End Property

        Private _CarrTarDiscountCountry As String
        <DataMember()> _
        Public Property CarrTarDiscountCountry() As String
            Get
                Return Left(Me._CarrTarDiscountCountry, 30)
            End Get
            Set(value As String)
                If (String.Equals(Me._CarrTarDiscountCountry, value) = False) Then
                    Me._CarrTarDiscountCountry = Left(value, 30)
                    Me.SendPropertyChanged("CarrTarDiscountCountry")
                End If
            End Set
        End Property

        Private _CarrTarDiscountState As String
        <DataMember()> _
        Public Property CarrTarDiscountState() As String
            Get
                Return Left(Me._CarrTarDiscountState, 2)
            End Get
            Set(value As String)
                If (String.Equals(Me._CarrTarDiscountState, value) = False) Then
                    Me._CarrTarDiscountState = Left(value, 2)
                    Me.SendPropertyChanged("CarrTarDiscountState")
                End If
            End Set
        End Property

        Private _CarrTarDiscountCity As String
        <DataMember()> _
        Public Property CarrTarDiscountCity() As String
            Get
                Return Left(Me._CarrTarDiscountCity, 50)
            End Get
            Set(value As String)
                If (String.Equals(Me._CarrTarDiscountCity, value) = False) Then
                    Me._CarrTarDiscountCity = Left(value, 50)
                    Me.SendPropertyChanged("CarrTarDiscountCity")
                End If
            End Set
        End Property

        Private _CarrTarDiscountZipFrom As String
        <DataMember()> _
        Public Property CarrTarDiscountZipFrom() As String
            Get
                Return Left(Me._CarrTarDiscountZipFrom, 10)
            End Get
            Set(value As String)
                If (String.Equals(Me._CarrTarDiscountZipFrom, value) = False) Then
                    Me._CarrTarDiscountZipFrom = Left(value, 10)
                    Me.SendPropertyChanged("CarrTarDiscountZipFrom")
                End If
            End Set
        End Property

        Private _CarrTarDiscountZipTo As String
        <DataMember()> _
        Public Property CarrTarDiscountZipTo() As String
            Get
                Return Left(Me._CarrTarDiscountZipTo, 10)
            End Get
            Set(value As String)
                If (String.Equals(Me._CarrTarDiscountZipTo, value) = False) Then
                    Me._CarrTarDiscountZipTo = Left(value, 10)
                    Me.SendPropertyChanged("CarrTarDiscountZipTo")
                End If
            End Set
        End Property

        Private _CarrTarDiscountMinValue As Decimal
        <DataMember()> _
        Public Property CarrTarDiscountMinValue() As Decimal
            Get
                Return Me._CarrTarDiscountMinValue
            End Get
            Set(value As Decimal)
                If ((Me._CarrTarDiscountMinValue = value) _
                            = False) Then
                    Me._CarrTarDiscountMinValue = value
                    Me.SendPropertyChanged("CarrTarDiscountMinValue")
                End If
            End Set
        End Property

        Private _CarrTarDiscountRateValue As Decimal
        <DataMember()> _
        Public Property CarrTarDiscountRateValue() As Decimal
            Get
                Return Me._CarrTarDiscountRateValue
            End Get
            Set(value As Decimal)
                If ((Me._CarrTarDiscountRateValue = value) _
                            = False) Then
                    Me._CarrTarDiscountRateValue = value
                    Me.SendPropertyChanged("CarrTarDiscountRateValue")
                End If
            End Set
        End Property

        Private _CarrTarDiscountWgtLimit As Double
        <DataMember()> _
        Public Property CarrTarDiscountWgtLimit() As Double
            Get
                Return Me._CarrTarDiscountWgtLimit
            End Get
            Set(value As Double)
                If ((Me._CarrTarDiscountWgtLimit = value) _
                            = False) Then
                    Me._CarrTarDiscountWgtLimit = value
                    Me.SendPropertyChanged("CarrTarDiscountWgtLimit")
                End If
            End Set
        End Property

        Private _CarrTarDiscountModUser As String = ""
        <DataMember()> _
        Public Property CarrTarDiscountModUser() As String
            Get
                Return Left(_CarrTarDiscountModUser, 100)
            End Get
            Set(ByVal value As String)
                If (String.Equals(Me._CarrTarDiscountModUser, value) = False) Then
                    Me._CarrTarDiscountModUser = Left(value, 100)
                    Me.SendPropertyChanged("CarrTarDiscountModUser")
                End If
            End Set
        End Property

        Private _CarrTarDiscountModDate As Date = DateTime.Now
        <DataMember()> _
        Public Property CarrTarDiscountModDate() As Date
            Get
                Return _CarrTarDiscountModDate
            End Get
            Set(ByVal value As Date)
                If (Me._CarrTarDiscountModDate.Equals(value) = False) Then
                    Me._CarrTarDiscountModDate = value
                    Me.SendPropertyChanged("CarrTarDiscountModDate")
                End If
            End Set
        End Property

        Private _CarrTarDiscountUpdated As Byte()
        <DataMember()> _
        Public Property CarrTarDiscountUpdated() As Byte()
            Get
                Return _CarrTarDiscountUpdated
            End Get
            Set(ByVal value As Byte())
                _CarrTarDiscountUpdated = value
            End Set
        End Property

#End Region

#Region " Public Methods"
        Public Overrides Function Clone() As DTOBaseClass
            Dim instance As New CarrTarDiscount
            instance = DirectCast(MemberwiseClone(), CarrTarDiscount)
            Return instance
        End Function

#End Region

    End Class
End Namespace

