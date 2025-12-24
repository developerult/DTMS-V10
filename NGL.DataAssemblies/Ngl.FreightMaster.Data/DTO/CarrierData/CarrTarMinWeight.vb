Imports Microsoft.VisualBasic
Imports System
Imports System.Collections.Generic
Imports System.Linq
Imports System.Text
Imports System.Runtime.Serialization
Imports Ngl.Core.ChangeTracker

Namespace DataTransferObjects
    <DataContract()> _
    Public Class CarrTarMinWeight
        Inherits DTOBaseClass


#Region " Data Members"

        Private _CarrTarMinWeightControl As Integer
        <DataMember()> _
        Public Property CarrTarMinWeightControl() As Integer
            Get
                Return Me._CarrTarMinWeightControl
            End Get
            Set(value As Integer)
                If ((Me._CarrTarMinWeightControl = value) _
                            = False) Then
                    Me._CarrTarMinWeightControl = value
                    Me.SendPropertyChanged("CarrTarMinWeightControl")
                End If
            End Set
        End Property

        Private _CarrTarMinWeightCarrTarControl As Integer
        <DataMember()> _
        Public Property CarrTarMinWeightCarrTarControl() As Integer
            Get
                Return Me._CarrTarMinWeightCarrTarControl
            End Get
            Set(value As Integer)
                If ((Me._CarrTarMinWeightCarrTarControl = value) _
                            = False) Then
                    Me._CarrTarMinWeightCarrTarControl = value
                    Me.SendPropertyChanged("CarrTarMinWeightCarrTarControl")
                End If
            End Set
        End Property

        Private _CarrTarMinWeightPointTypeControl As Integer
        <DataMember()> _
        Public Property CarrTarMinWeightPointTypeControl() As Integer
            Get
                Return Me._CarrTarMinWeightPointTypeControl
            End Get
            Set(value As Integer)
                If ((Me._CarrTarMinWeightPointTypeControl = value) _
                            = False) Then
                    Me._CarrTarMinWeightPointTypeControl = value
                    Me.SendPropertyChanged("CarrTarMinWeightPointTypeControl")
                End If
            End Set
        End Property


        Private _CarrTarMinWeightClassTypeControl As Integer
        <DataMember()> _
        Public Property CarrTarMinWeightClassTypeControl() As Integer
            Get
                Return Me._CarrTarMinWeightClassTypeControl
            End Get
            Set(value As Integer)
                If ((Me._CarrTarMinWeightClassTypeControl = value) _
                            = False) Then
                    Me._CarrTarMinWeightClassTypeControl = value
                    Me.SendPropertyChanged("CarrTarMinWeightClassTypeControl")
                End If
            End Set
        End Property

        Private _CarrTarMinWeightName As String
        <DataMember()> _
        Public Property CarrTarMinWeightName() As String
            Get
                Return Left(Me._CarrTarMinWeightName, 50)
            End Get
            Set(value As String)
                If (String.Equals(Me._CarrTarMinWeightName, value) = False) Then
                    Me._CarrTarMinWeightName = Left(value, 50)
                    Me.SendPropertyChanged("CarrTarMinWeightName")
                End If
            End Set
        End Property

        Private _CarrTarMinWeightEffDateFrom As System.Nullable(Of Date)
        <DataMember()> _
        Public Property CarrTarMinWeightEffDateFrom() As System.Nullable(Of Date)
            Get
                Return Me._CarrTarMinWeightEffDateFrom
            End Get
            Set(value As System.Nullable(Of Date))
                If (Me._CarrTarMinWeightEffDateFrom.Equals(value) = False) Then
                    Me._CarrTarMinWeightEffDateFrom = value
                    Me.SendPropertyChanged("CarrTarMinWeightEffDateFrom")

                End If
            End Set
        End Property

        Private _CarrTarMinWeightEffDateTo As System.Nullable(Of Date)
        <DataMember()> _
        Public Property CarrTarMinWeightEffDateTo() As System.Nullable(Of Date)
            Get
                Return Me._CarrTarMinWeightEffDateTo
            End Get
            Set(value As System.Nullable(Of Date))
                If (Me._CarrTarMinWeightEffDateTo.Equals(value) = False) Then
                    Me._CarrTarMinWeightEffDateTo = value
                    Me.SendPropertyChanged("CarrTarMinWeightEffDateTo")
                End If
            End Set
        End Property

        Private _CarrTarMinWeightCountry As String
        <DataMember()> _
        Public Property CarrTarMinWeightCountry() As String
            Get
                Return Left(Me._CarrTarMinWeightCountry, 30)
            End Get
            Set(value As String)
                If (String.Equals(Me._CarrTarMinWeightCountry, value) = False) Then
                    Me._CarrTarMinWeightCountry = Left(value, 30)
                    Me.SendPropertyChanged("CarrTarMinWeightCountry")
                End If
            End Set
        End Property

        Private _CarrTarMinWeightState As String
        <DataMember()> _
        Public Property CarrTarMinWeightState() As String
            Get
                Return Left(Me._CarrTarMinWeightState, 2)
            End Get
            Set(value As String)
                If (String.Equals(Me._CarrTarMinWeightState, value) = False) Then
                    Me._CarrTarMinWeightState = Left(value, 2)
                    Me.SendPropertyChanged("CarrTarMinWeightState")
                End If
            End Set
        End Property

        Private _CarrTarMinWeightCity As String
        <DataMember()> _
        Public Property CarrTarMinWeightCity() As String
            Get
                Return Left(Me._CarrTarMinWeightCity, 50)
            End Get
            Set(value As String)
                If (String.Equals(Me._CarrTarMinWeightCity, value) = False) Then
                    Me._CarrTarMinWeightCity = Left(value, 50)
                    Me.SendPropertyChanged("CarrTarMinWeightCity")
                End If
            End Set
        End Property

        Private _CarrTarMinWeightZipFrom As String
        <DataMember()> _
        Public Property CarrTarMinWeightZipFrom() As String
            Get
                Return Left(Me._CarrTarMinWeightZipFrom, 10)
            End Get
            Set(value As String)
                If (String.Equals(Me._CarrTarMinWeightZipFrom, value) = False) Then
                    Me._CarrTarMinWeightZipFrom = Left(value, 10)
                    Me.SendPropertyChanged("CarrTarMinWeightZipFrom")
                End If
            End Set
        End Property

        Private _CarrTarMinWeightZipTo As String
        <DataMember()> _
        Public Property CarrTarMinWeightZipTo() As String
            Get
                Return Left(Me._CarrTarMinWeightZipTo, 10)
            End Get
            Set(value As String)
                If (String.Equals(Me._CarrTarMinWeightZipTo, value) = False) Then
                    Me._CarrTarMinWeightZipTo = Left(value, 10)
                    Me.SendPropertyChanged("CarrTarMinWeightZipTo")
                End If
            End Set
        End Property

        Private _CarrTarMinWeightLaneControl As Integer
        <DataMember()> _
        Public Property CarrTarMinWeightLaneControl() As Integer
            Get
                Return Me._CarrTarMinWeightLaneControl
            End Get
            Set(value As Integer)
                If ((Me._CarrTarMinWeightLaneControl = value) _
                            = False) Then
                    Me._CarrTarMinWeightLaneControl = value
                    Me.SendPropertyChanged("CarrTarMinWeightLaneControl")
                End If
            End Set
        End Property

      

        Private _CarrTarMinWeightClassFrom As String = ""
        <DataMember()> _
        Public Property CarrTarMinWeightClassFrom() As String
            Get
                Return Left(_CarrTarMinWeightClassFrom, 50)
            End Get
            Set(ByVal value As String)
                If (String.Equals(Me._CarrTarMinWeightClassFrom, value) = False) Then
                    Me._CarrTarMinWeightClassFrom = Left(value, 50)
                    Me.SendPropertyChanged("CarrTarMinWeightClassFrom")
                End If
            End Set
        End Property

        Private _CarrTarMinWeightClassTo As String = ""
        <DataMember()> _
        Public Property CarrTarMinWeightClassTo() As String
            Get
                Return Left(_CarrTarMinWeightClassTo, 50)
            End Get
            Set(ByVal value As String)
                If (String.Equals(Me._CarrTarMinWeightClassTo, value) = False) Then
                    Me._CarrTarMinWeightClassTo = Left(value, 50)
                    Me.SendPropertyChanged("CarrTarMinWeightClassTo")
                End If
            End Set
        End Property

        Private _CarrTarMinWeightPerLoad As Decimal
        <DataMember()> _
        Public Property CarrTarMinWeightPerLoad() As Decimal
            Get
                Return Me._CarrTarMinWeightPerLoad
            End Get
            Set(value As Decimal)
                If ((Me._CarrTarMinWeightPerLoad = value) _
                            = False) Then
                    Me._CarrTarMinWeightPerLoad = value
                    Me.SendPropertyChanged("CarrTarMinWeightPerLoad")
                End If
            End Set
        End Property

        Private _CarrTarMinWeightPerPallet As Decimal
        <DataMember()> _
        Public Property CarrTarMinWeightPerPallet() As Decimal
            Get
                Return Me._CarrTarMinWeightPerPallet
            End Get
            Set(value As Decimal)
                If ((Me._CarrTarMinWeightPerPallet = value) _
                            = False) Then
                    Me._CarrTarMinWeightPerPallet = value
                    Me.SendPropertyChanged("CarrTarMinWeightPerPallet")
                End If
            End Set
        End Property

        Private _CarrTarMinWeightModUser As String = ""
        <DataMember()> _
        Public Property CarrTarMinWeightModUser() As String
            Get
                Return Left(_CarrTarMinWeightModUser, 100)
            End Get
            Set(ByVal value As String)
                If (String.Equals(Me._CarrTarMinWeightModUser, value) = False) Then
                    Me._CarrTarMinWeightModUser = Left(value, 100)
                    Me.SendPropertyChanged("CarrTarMinWeightModUser")
                End If
            End Set
        End Property

        Private _CarrTarMinWeightModDate As Date = DateTime.Now
        <DataMember()> _
        Public Property CarrTarMinWeightModDate() As Date
            Get
                Return _CarrTarMinWeightModDate
            End Get
            Set(ByVal value As Date)
                If (Me._CarrTarMinWeightModDate.Equals(value) = False) Then
                    Me._CarrTarMinWeightModDate = value
                    Me.SendPropertyChanged("CarrTarMinWeightModDate")
                End If
            End Set
        End Property

        Private _CarrTarMinWeightUpdated As Byte()
        <DataMember()> _
        Public Property CarrTarMinWeightUpdated() As Byte()
            Get
                Return _CarrTarMinWeightUpdated
            End Get
            Set(ByVal value As Byte())
                _CarrTarMinWeightUpdated = value
            End Set
        End Property

#End Region

#Region " Public Methods"
        Public Overrides Function Clone() As DTOBaseClass
            Dim instance As New CarrTarMinWeight
            instance = DirectCast(MemberwiseClone(), CarrTarMinWeight)
            Return instance
        End Function

#End Region

    End Class
End Namespace