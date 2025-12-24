
Imports Microsoft.VisualBasic
Imports System
Imports System.Collections.Generic
Imports System.Linq
Imports System.Text
Imports System.Runtime.Serialization
Imports Ngl.Core.ChangeTracker


Namespace DataTransferObjects
    <DataContract()> _
    Public Class CarrTarEquipMat
        Inherits DTOBaseClass


#Region " Data Members"
        Private _CarrTarEquipMatControl As Integer = 0
        <DataMember()> _
        Public Property CarrTarEquipMatControl() As Integer
            Get
                Return _CarrTarEquipMatControl
            End Get
            Set(ByVal value As Integer)
                If (Me._CarrTarEquipMatControl <> value) Then
                    Me._CarrTarEquipMatControl = value
                    Me.SendPropertyChanged("CarrTarEquipMatControl")
                End If
            End Set
        End Property

        Private _CarrTarEquipMatCarrTarEquipControl As Integer = 0
        <DataMember()> _
        Public Property CarrTarEquipMatCarrTarEquipControl() As Integer
            Get
                Return _CarrTarEquipMatCarrTarEquipControl
            End Get
            Set(ByVal value As Integer)
                If (Me._CarrTarEquipMatCarrTarEquipControl <> value) Then
                    Me._CarrTarEquipMatCarrTarEquipControl = value
                    Me.SendPropertyChanged("CarrTarEquipMatCarrTarEquipControl")
                End If
            End Set
        End Property

        Private _CarrTarEquipMatCarrTarControl As Integer = 0
        <DataMember()> _
        Public Property CarrTarEquipMatCarrTarControl() As Integer
            Get
                Return _CarrTarEquipMatCarrTarControl
            End Get
            Set(ByVal value As Integer)
                If (Me._CarrTarEquipMatCarrTarControl <> value) Then
                    Me._CarrTarEquipMatCarrTarControl = value
                    Me.SendPropertyChanged("CarrTarEquipMatCarrTarControl")
                End If
            End Set
        End Property

        Private _CarrTarEquipMatCarrTarMatControl As System.Nullable(Of Integer)
        <DataMember()> _
        Public Property CarrTarEquipMatCarrTarMatControl() As System.Nullable(Of Integer)
            Get
                Return _CarrTarEquipMatCarrTarMatControl
            End Get
            Set(ByVal value As System.Nullable(Of Integer))
                If (Me._CarrTarEquipMatCarrTarMatControl.Equals(value) = False) Then
                    Me._CarrTarEquipMatCarrTarMatControl = value
                    Me.SendPropertyChanged("CarrTarEquipMatCarrTarMatControl")
                End If
            End Set
        End Property

        Private _CarrTarEquipMatCarrTarMatBPControl As Integer = 0
        <DataMember()> _
        Public Property CarrTarEquipMatCarrTarMatBPControl() As Integer
            Get
                Return _CarrTarEquipMatCarrTarMatBPControl
            End Get
            Set(ByVal value As Integer)
                If (Me._CarrTarEquipMatCarrTarMatBPControl <> value) Then
                    Me._CarrTarEquipMatCarrTarMatBPControl = value
                    Me.SendPropertyChanged("CarrTarEquipMatCarrTarMatBPControl")
                End If
            End Set
        End Property

        Private _CarrTarEquipMatFromZip As String = ""
        <DataMember()> _
        Public Property CarrTarEquipMatFromZip() As String
            Get
                Return Left(_CarrTarEquipMatFromZip, 20)  'Modified by RHR for v-8.4.003 on 06/25/2021
            End Get
            Set(ByVal value As String)
                If (String.Equals(Me._CarrTarEquipMatFromZip, value) = False) Then
                    Me._CarrTarEquipMatFromZip = Left(value, 20) 'Modified by RHR for v-8.4.003 on 06/25/2021
                    Me.SendPropertyChanged("CarrTarEquipMatFromZip")
                End If
            End Set
        End Property


        Private _CarrTarEquipMatToZip As String = ""
        <DataMember()> _
        Public Property CarrTarEquipMatToZip() As String
            Get
                Return Left(_CarrTarEquipMatToZip, 20) 'Modified by RHR for v-8.4.003 on 06/25/2021
            End Get
            Set(ByVal value As String)
                If (String.Equals(Me._CarrTarEquipMatToZip, value) = False) Then
                    Me._CarrTarEquipMatToZip = Left(value, 20) 'Modified by RHR for v-8.4.003 on 06/25/2021
                    Me.SendPropertyChanged("CarrTarEquipMatToZip")
                End If
            End Set
        End Property


        Private _CarrTarEquipMatExptFlag As Boolean = False
        <DataMember()> _
        Public Property CarrTarEquipMatExptFlag() As Boolean
            Get
                Return _CarrTarEquipMatExptFlag
            End Get
            Set(ByVal value As Boolean)
                If (Me._CarrTarEquipMatExptFlag <> value) Then
                    Me._CarrTarEquipMatExptFlag = value
                    Me.SendPropertyChanged("CarrTarEquipMatExptFlag")
                End If
            End Set
        End Property

        Private _CarrTarEquipMatMin As System.Nullable(Of Decimal)
        <DataMember()> _
        Public Property CarrTarEquipMatMin() As System.Nullable(Of Decimal)
            Get
                Return _CarrTarEquipMatMin
            End Get
            Set(ByVal value As System.Nullable(Of Decimal))
                If (Me._CarrTarEquipMatMin.Equals(value) = False) Then
                    Me._CarrTarEquipMatMin = value
                    Me.SendPropertyChanged("CarrTarEquipMatMin")
                End If
            End Set
        End Property

        Private _CarrTarEquipMatMaxDays As Integer = 0
        <DataMember()> _
        Public Property CarrTarEquipMatMaxDays() As Integer
            Get
                Return _CarrTarEquipMatMaxDays
            End Get
            Set(ByVal value As Integer)
                If (Me._CarrTarEquipMatMaxDays <> value) Then
                    Me._CarrTarEquipMatMaxDays = value
                    Me.SendPropertyChanged("CarrTarEquipMatMaxDays")
                End If
            End Set
        End Property

        Private _CarrTarEquipMatModUser As String = ""
        <DataMember()> _
        Public Property CarrTarEquipMatModUser() As String
            Get
                Return Left(_CarrTarEquipMatModUser, 100)
            End Get
            Set(ByVal value As String)
                If (String.Equals(Me._CarrTarEquipMatModUser, value) = False) Then
                    Me._CarrTarEquipMatModUser = Left(value, 100)
                    Me.SendPropertyChanged("CarrTarEquipMatModUser")
                End If
            End Set
        End Property


        Private _CarrTarEquipMatModDate As Date = DateTime.Now
        <DataMember()> _
        Public Property CarrTarEquipMatModDate() As Date
            Get
                Return _CarrTarEquipMatModDate
            End Get
            Set(ByVal value As Date)
                If (Me._CarrTarEquipMatModDate.Equals(value) = False) Then
                    Me._CarrTarEquipMatModDate = value
                    Me.SendPropertyChanged("CarrTarEquipMatModDate")
                End If
            End Set
        End Property

        Private _CarrTarEquipMatUpdated As Byte()
        <DataMember()> _
        Public Property CarrTarEquipMatUpdated() As Byte()
            Get
                Return _CarrTarEquipMatUpdated
            End Get
            Set(ByVal value As Byte())
                _CarrTarEquipMatUpdated = value
            End Set
        End Property

        Private _CarrTarEquipMatName As String = ""
        <DataMember()> _
        Public Property CarrTarEquipMatName() As String
            Get
                Return Left(_CarrTarEquipMatName, 50)
            End Get
            Set(ByVal value As String)
                If (String.Equals(Me._CarrTarEquipMatName, value) = False) Then
                    Me._CarrTarEquipMatName = Left(value, 50)
                    Me.SendPropertyChanged("CarrTarEquipMatName")
                End If
            End Set
        End Property

        Private _CarrTarEquipMatClass As String = ""
        <DataMember()> _
        Public Property CarrTarEquipMatClass() As String
            Get
                Return Left(_CarrTarEquipMatClass, 50)
            End Get
            Set(ByVal value As String)
                If (String.Equals(Me._CarrTarEquipMatClass, value) = False) Then
                    Me._CarrTarEquipMatClass = Left(value, 50)
                    Me.SendPropertyChanged("CarrTarEquipMatClass")
                End If
            End Set
        End Property

        Private _CarrTarEquipMatCountry As String = ""
        <DataMember()> _
        Public Property CarrTarEquipMatCountry() As String
            Get
                Return Left(_CarrTarEquipMatCountry, 30)
            End Get
            Set(ByVal value As String)
                If (String.Equals(Me._CarrTarEquipMatCountry, value) = False) Then
                    Me._CarrTarEquipMatCountry = Left(value, 30)
                    Me.SendPropertyChanged("CarrTarEquipMatCountry")
                End If
            End Set
        End Property

        Private _CarrTarEquipMatState As String = ""
        <DataMember()> _
        Public Property CarrTarEquipMatState() As String
            Get
                Return Left(_CarrTarEquipMatState, 2)
            End Get
            Set(ByVal value As String)
                If (String.Equals(Me._CarrTarEquipMatState, value) = False) Then
                    Me._CarrTarEquipMatState = Left(value, 2)
                    Me.SendPropertyChanged("CarrTarEquipMatState")
                End If
            End Set
        End Property

        Private _CarrTarEquipMatCity As String = ""
        <DataMember()> _
        Public Property CarrTarEquipMatCity() As String
            Get
                Return Left(_CarrTarEquipMatCity, 50)
            End Get
            Set(ByVal value As String)
                If (String.Equals(Me._CarrTarEquipMatCity, value) = False) Then
                    Me._CarrTarEquipMatCity = Left(value, 50)
                    Me.SendPropertyChanged("CarrTarEquipMatCity")
                End If
            End Set
        End Property

        Private _CarrTarEquipMatClassTypeControl As Integer = 0
        <DataMember()> _
        Public Property CarrTarEquipMatClassTypeControl() As Integer
            Get
                Return _CarrTarEquipMatClassTypeControl
            End Get
            Set(ByVal value As Integer)
                If (Me._CarrTarEquipMatClassTypeControl <> value) Then
                    Me._CarrTarEquipMatClassTypeControl = value
                    Me.SendPropertyChanged("CarrTarEquipMatClassTypeControl")
                End If
            End Set
        End Property

        Private _CarrTarEquipMatTarRateTypeControl As Integer = 0
        <DataMember()> _
        Public Property CarrTarEquipMatTarRateTypeControl() As Integer
            Get
                Return _CarrTarEquipMatTarRateTypeControl
            End Get
            Set(ByVal value As Integer)
                If (Me._CarrTarEquipMatTarRateTypeControl <> value) Then
                    Me._CarrTarEquipMatTarRateTypeControl = value
                    Me.SendPropertyChanged("CarrTarEquipMatTarRateTypeControl")
                End If
            End Set
        End Property

        Private _CarrTarEquipMatLaneControl As Integer = 0
        <DataMember()> _
        Public Property CarrTarEquipMatLaneControl() As Integer
            Get
                Return _CarrTarEquipMatLaneControl
            End Get
            Set(ByVal value As Integer)
                If (Me._CarrTarEquipMatLaneControl <> value) Then
                    Me._CarrTarEquipMatLaneControl = value
                    Me.SendPropertyChanged("CarrTarEquipMatLaneControl")
                End If
            End Set
        End Property

        Private _CarrTarEquipMatTarBracketTypeControl As Integer = 0
        <DataMember()> _
        Public Property CarrTarEquipMatTarBracketTypeControl() As Integer
            Get
                Return _CarrTarEquipMatTarBracketTypeControl
            End Get
            Set(ByVal value As Integer)
                If (Me._CarrTarEquipMatTarBracketTypeControl <> value) Then
                    Me._CarrTarEquipMatTarBracketTypeControl = value
                    Me.SendPropertyChanged("CarrTarEquipMatTarBracketTypeControl")
                End If
            End Set
        End Property

        Private _CarrierTariffEquipMatrixDetails As List(Of CarrTarEquipMatDet)
        <DataMember()>
        Public Property CarrierTariffEquipMatrixDetails() As List(Of CarrTarEquipMatDet)
            Get
                Return _CarrierTariffEquipMatrixDetails
            End Get
            Set(ByVal value As List(Of CarrTarEquipMatDet))
                _CarrierTariffEquipMatrixDetails = value
            End Set
        End Property

        Private _CarrTarEquipMatOrigZip As String = ""
        ''' <summary>
        ''' Optional Origin Zip Code for Rate Shop
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks>
        ''' Modified by RHR for v-8.5.4.001 on 06/14/2023 new Origin Rate shop logic
        ''' </remarks>
        <DataMember()>
        Public Property CarrTarEquipMatOrigZip() As String
            Get
                Return Left(_CarrTarEquipMatOrigZip, 20)
            End Get
            Set(ByVal value As String)
                If (String.Equals(Me._CarrTarEquipMatOrigZip, value) = False) Then
                    Me._CarrTarEquipMatOrigZip = Left(value, 20)
                    Me.SendPropertyChanged("CarrTarEquipMatOrigZip")
                End If
            End Set
        End Property

#End Region

#Region " Public Methods"
        Public Overrides Function Clone() As DTOBaseClass
            Dim instance As New CarrTarEquipMat
            instance = DirectCast(MemberwiseClone(), CarrTarEquipMat)
            instance.CarrierTariffEquipMatrixDetails = Nothing
            For Each item In CarrierTariffEquipMatrixDetails
                instance.CarrierTariffEquipMatrixDetails.Add(DirectCast(item.Clone, CarrTarEquipMatDet))
            Next
            Return instance
        End Function

#End Region

    End Class
End Namespace
