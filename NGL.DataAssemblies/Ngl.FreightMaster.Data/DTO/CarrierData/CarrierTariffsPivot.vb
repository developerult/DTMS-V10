
Imports Microsoft.VisualBasic
Imports System
Imports System.Collections.Generic
Imports System.Linq
Imports System.Text
Imports System.Runtime.Serialization
Imports Ngl.Core.ChangeTracker


Namespace DataTransferObjects
    <DataContract()> _
    Public Class CarrierTariffsPivot
        Inherits DTOBaseClass


#Region " Data Members"

        Private _CarrierControl As Integer = 0
        <DataMember()> _
        Public Property CarrierControl() As Integer
            Get
                Return _CarrierControl
            End Get
            Set(ByVal value As Integer)
                If (Me._CarrierControl <> value) Then
                    Me._CarrierControl = value
                    Me.SendPropertyChanged("CarrierControl")
                End If
            End Set
        End Property

        Private _CarrierName As String = ""
        <DataMember()> _
        Public Property CarrierName() As String
            Get
                Return Left(_CarrierName, 40)
            End Get
            Set(ByVal value As String)
                If (String.Equals(Me._CarrierName, value) = False) Then
                    Me._CarrierName = Left(value, 40)
                    Me.SendPropertyChanged("CarrierName")
                End If
            End Set
        End Property

        Private _CarrierNumber As Integer = 0
        <DataMember()> _
        Public Property CarrierNumber() As Integer
            Get
                Return _CarrierNumber
            End Get
            Set(ByVal value As Integer)
                If (Me._CarrierNumber <> value) Then
                    Me._CarrierNumber = value
                    Me.SendPropertyChanged("CarrierNumber")
                End If
            End Set
        End Property

        Private _CarrTarControl As Integer = 0
        <DataMember()> _
        Public Property CarrTarControl() As Integer
            Get
                Return _CarrTarControl
            End Get
            Set(ByVal value As Integer)
                If (Me._CarrTarControl <> value) Then
                    Me._CarrTarControl = value
                    Me.SendPropertyChanged("CarrTarControl")
                End If
            End Set
        End Property

        Private _CarrTarName As String = ""
        <DataMember()> _
        Public Property CarrTarName() As String
            Get
                Return Left(_CarrTarName, 50)
            End Get
            Set(ByVal value As String)
                If (String.Equals(Me._CarrTarName, value) = False) Then
                    Me._CarrTarName = Left(value, 50)
                    Me.SendPropertyChanged("CarrTarName")
                End If
            End Set
        End Property

        Private _CarrTarID As String = ""
        <DataMember()> _
        Public Property CarrTarID() As String
            Get
                Return Left(_CarrTarID, 100)
            End Get
            Set(ByVal value As String)
                If (String.Equals(Me._CarrTarID, value) = False) Then
                    Me._CarrTarID = Left(value, 100)
                    Me.SendPropertyChanged("CarrTarID")
                End If
            End Set
        End Property

        Private _CarrTarTempType As Integer = 0
        <DataMember()> _
        Public Property CarrTarTempType() As Integer
            Get
                Return _CarrTarTempType
            End Get
            Set(ByVal value As Integer)
                If (Me._CarrTarTempType <> value) Then
                    Me._CarrTarTempType = value
                    Me.SendPropertyChanged("CarrTarTempType")
                End If
            End Set
        End Property

        Private _CarrTarDefWgt As Boolean = True
        <DataMember()> _
        Public Property CarrTarDefWgt() As Boolean
            Get
                Return _CarrTarDefWgt
            End Get
            Set(ByVal value As Boolean)
                If (Me._CarrTarDefWgt <> value) Then
                    Me._CarrTarDefWgt = value
                    Me.SendPropertyChanged("CarrTarDefWgt")
                End If
            End Set
        End Property

        Private _CarrTarEffDateFrom As System.Nullable(Of Date)
        <DataMember()> _
        Public Property CarrTarEffDateFrom() As System.Nullable(Of Date)
            Get
                Return _CarrTarEffDateFrom
            End Get
            Set(ByVal value As System.Nullable(Of Date))
                If (Me._CarrTarEffDateFrom.Equals(value) = False) Then
                    Me._CarrTarEffDateFrom = value
                    Me.SendPropertyChanged("CarrTarEffDateFrom")
                End If
            End Set
        End Property

        Private _CarrTarEffDateTo As System.Nullable(Of Date)
        <DataMember()> _
        Public Property CarrTarEffDateTo() As System.Nullable(Of Date)
            Get
                Return _CarrTarEffDateTo
            End Get
            Set(ByVal value As System.Nullable(Of Date))
                If (Me._CarrTarEffDateTo.Equals(value) = False) Then
                    Me._CarrTarEffDateTo = value
                    Me.SendPropertyChanged("CarrTarEffDateTo")
                End If
            End Set
        End Property

        Private _CarrTarAutoAssignPro As Boolean = False
        <DataMember()> _
        Public Property CarrTarAutoAssignPro() As Boolean
            Get
                Return _CarrTarAutoAssignPro
            End Get
            Set(ByVal value As Boolean)
                If (Me._CarrTarAutoAssignPro <> value) Then
                    Me._CarrTarAutoAssignPro = value
                    Me.SendPropertyChanged("CarrTarAutoAssignPro")
                End If
            End Set
        End Property

        Private _CarrTarTariffTypeControl As Integer = 0
        <DataMember()> _
        Public Property CarrTarTariffTypeControl() As Integer
            Get
                Return _CarrTarTariffTypeControl
            End Get
            Set(ByVal value As Integer)
                If (Me._CarrTarTariffTypeControl <> value) Then
                    Me._CarrTarTariffTypeControl = value
                    Me.SendPropertyChanged("CarrTarTariffTypeControl")
                End If
            End Set
        End Property

        Private _CarrTarTariffModeTypeControl As Integer = 0
        <DataMember()> _
        Public Property CarrTarTariffModeTypeControl() As Integer
            Get
                Return _CarrTarTariffModeTypeControl
            End Get
            Set(ByVal value As Integer)
                If (Me._CarrTarTariffModeTypeControl <> value) Then
                    Me._CarrTarTariffModeTypeControl = value
                    Me.SendPropertyChanged("CarrTarTariffModeTypeControl")
                End If
            End Set
        End Property

        Private _CarrTarRevisionNumber As Integer = 1
        <DataMember()> _
        Public Property CarrTarRevisionNumber() As Integer
            Get
                Return _CarrTarRevisionNumber
            End Get
            Set(ByVal value As Integer)
                If (Me._CarrTarRevisionNumber <> value) Then
                    Me._CarrTarRevisionNumber = value
                    Me.SendPropertyChanged("CarrTarRevisionNumber")
                End If
            End Set
        End Property

        Private _CarrTarOutbound As Boolean = False
        <DataMember()> _
        Public Property CarrTarOutbound() As Boolean
            Get
                Return _CarrTarOutbound
            End Get
            Set(ByVal value As Boolean)
                If (Me._CarrTarOutbound <> value) Then
                    Me._CarrTarOutbound = value
                    Me.SendPropertyChanged("CarrTarOutbound")
                End If
            End Set
        End Property

        Private _CarrTarWillDriveSunday As Boolean = False
        <DataMember()> _
        Public Property CarrTarWillDriveSunday() As Boolean
            Get
                Return _CarrTarWillDriveSunday
            End Get
            Set(ByVal value As Boolean)
                If (Me._CarrTarWillDriveSunday <> value) Then
                    Me._CarrTarWillDriveSunday = value
                    Me.SendPropertyChanged("CarrTarWillDriveSunday")
                End If
            End Set
        End Property

        Private _CarrTarWillDriveSaturday As Boolean = False
        <DataMember()> _
        Public Property CarrTarWillDriveSaturday() As Boolean
            Get
                Return _CarrTarWillDriveSaturday
            End Get
            Set(ByVal value As Boolean)
                If (Me._CarrTarWillDriveSaturday <> value) Then
                    Me._CarrTarWillDriveSaturday = value
                    Me.SendPropertyChanged("CarrTarWillDriveSaturday")
                End If
            End Set
        End Property

        Private _CarrTarAgentControl As Integer = 0
        <DataMember()> _
        Public Property CarrTarAgentControl() As Integer
            Get
                Return _CarrTarAgentControl
            End Get
            Set(ByVal value As Integer)
                If (Me._CarrTarAgentControl <> value) Then
                    Me._CarrTarAgentControl = value
                    Me.SendPropertyChanged("CarrTarAgentControl")
                End If
            End Set
        End Property

        Private _CarrTarEquipControl As Integer = 0
        <DataMember()> _
        Public Property CarrTarEquipControl() As Integer
            Get
                Return _CarrTarEquipControl
            End Get
            Set(ByVal value As Integer)
                If (Me._CarrTarEquipControl <> value) Then
                    Me._CarrTarEquipControl = value
                    Me.SendPropertyChanged("CarrTarEquipControl")
                End If
            End Set
        End Property

        Private _CarrTarEquipCarrierEquipControl As Integer = 0
        <DataMember()> _
        Public Property CarrTarEquipCarrierEquipControl() As Integer
            Get
                Return _CarrTarEquipCarrierEquipControl
            End Get
            Set(ByVal value As Integer)
                If (Me._CarrTarEquipCarrierEquipControl <> value) Then
                    Me._CarrTarEquipCarrierEquipControl = value
                    Me.SendPropertyChanged("CarrTarEquipCarrierEquipControl")
                End If
            End Set
        End Property

        Private _CarrTarEquipName As String = ""
        <DataMember()> _
        Public Property CarrTarEquipName() As String
            Get
                Return Left(_CarrTarEquipName, 50)
            End Get
            Set(ByVal value As String)
                If (String.Equals(Me._CarrTarEquipName, value) = False) Then
                    Me._CarrTarEquipName = Left(value, 50)
                    Me.SendPropertyChanged("CarrTarEquipName")
                End If
            End Set
        End Property

        Private _CarrTarEquipDescription As String = ""
        <DataMember()> _
        Public Property CarrTarEquipDescription() As String
            Get
                Return Left(_CarrTarEquipDescription, 255)
            End Get
            Set(ByVal value As String)
                If (String.Equals(Me._CarrTarEquipDescription, value) = False) Then
                    Me._CarrTarEquipDescription = Left(value, 255)
                    Me.SendPropertyChanged("CarrTarEquipDescription")
                End If
            End Set
        End Property

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

        Private _CarrTarEquipMatFromZip As String = ""
        <DataMember()> _
        Public Property CarrTarEquipMatFromZip() As String
            Get
                Return Left(_CarrTarEquipMatFromZip, 20) 'Modified by RHR for v-8.4.003 on 06/25/2021
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

        Private _Val1 As System.Nullable(Of Decimal)
        <DataMember()> _
        Public Property Val1() As System.Nullable(Of Decimal)
            Get
                Return _Val1
            End Get
            Set(ByVal value As System.Nullable(Of Decimal))
                If (Me._Val1.Equals(value) = False) Then
                    Me._Val1 = value
                    Me.SendPropertyChanged("Val1")
                End If
            End Set
        End Property

        Private _Val2 As System.Nullable(Of Decimal)
        <DataMember()> _
        Public Property Val2() As System.Nullable(Of Decimal)
            Get
                Return _Val2
            End Get
            Set(ByVal value As System.Nullable(Of Decimal))
                If (Me._Val2.Equals(value) = False) Then
                    Me._Val2 = value
                    Me.SendPropertyChanged("Val2")
                End If
            End Set
        End Property

        Private _Val3 As System.Nullable(Of Decimal)
        <DataMember()> _
        Public Property Val3() As System.Nullable(Of Decimal)
            Get
                Return _Val3
            End Get
            Set(ByVal value As System.Nullable(Of Decimal))
                If (Me._Val3.Equals(value) = False) Then
                    Me._Val3 = value
                    Me.SendPropertyChanged("Val3")
                End If
            End Set
        End Property

        Private _Val4 As System.Nullable(Of Decimal)
        <DataMember()> _
        Public Property Val4() As System.Nullable(Of Decimal)
            Get
                Return _Val4
            End Get
            Set(ByVal value As System.Nullable(Of Decimal))
                If (Me._Val4.Equals(value) = False) Then
                    Me._Val4 = value
                    Me.SendPropertyChanged("Val4")
                End If
            End Set
        End Property

        Private _Val5 As System.Nullable(Of Decimal)
        <DataMember()> _
        Public Property Val5() As System.Nullable(Of Decimal)
            Get
                Return _Val5
            End Get
            Set(ByVal value As System.Nullable(Of Decimal))
                If (Me._Val5.Equals(value) = False) Then
                    Me._Val5 = value
                    Me.SendPropertyChanged("Val5")
                End If
            End Set
        End Property

        Private _Val6 As System.Nullable(Of Decimal)
        <DataMember()> _
        Public Property Val6() As System.Nullable(Of Decimal)
            Get
                Return _Val6
            End Get
            Set(ByVal value As System.Nullable(Of Decimal))
                If (Me._Val6.Equals(value) = False) Then
                    Me._Val6 = value
                    Me.SendPropertyChanged("Val6")
                End If
            End Set
        End Property

        Private _Val7 As System.Nullable(Of Decimal)
        <DataMember()> _
        Public Property Val7() As System.Nullable(Of Decimal)
            Get
                Return _Val7
            End Get
            Set(ByVal value As System.Nullable(Of Decimal))
                If (Me._Val7.Equals(value) = False) Then
                    Me._Val7 = value
                    Me.SendPropertyChanged("Val7")
                End If
            End Set
        End Property

        Private _Val8 As System.Nullable(Of Decimal)
        <DataMember()> _
        Public Property Val8() As System.Nullable(Of Decimal)
            Get
                Return _Val8
            End Get
            Set(ByVal value As System.Nullable(Of Decimal))
                If (Me._Val8.Equals(value) = False) Then
                    Me._Val8 = value
                    Me.SendPropertyChanged("Val8")
                End If
            End Set
        End Property

        Private _Val9 As System.Nullable(Of Decimal)
        <DataMember()> _
        Public Property Val9() As System.Nullable(Of Decimal)
            Get
                Return _Val9
            End Get
            Set(ByVal value As System.Nullable(Of Decimal))
                If (Me._Val9.Equals(value) = False) Then
                    Me._Val9 = value
                    Me.SendPropertyChanged("Val9")
                End If
            End Set
        End Property

        Private _Val10 As System.Nullable(Of Decimal)
        <DataMember()> _
        Public Property Val10() As System.Nullable(Of Decimal)
            Get
                Return _Val10
            End Get
            Set(ByVal value As System.Nullable(Of Decimal))
                If (Me._Val10.Equals(value) = False) Then
                    Me._Val10 = value
                    Me.SendPropertyChanged("Val10")
                End If
            End Set
        End Property

        Private _RatedBookControl As Integer
        <DataMember()> _
        Public Property RatedBookControl() As Integer
            Get
                Return _RatedBookControl
            End Get
            Set(ByVal value As Integer)
                _RatedBookControl = value
            End Set
        End Property

        Private _MixedCount As Integer
        <DataMember()> _
        Public Property MixedCount() As Integer
            Get
                Return _MixedCount
            End Get
            Set(ByVal value As Integer)
                _MixedCount = value
            End Set
        End Property

        Private _AllowFlatRate As Boolean
        <DataMember()> _
        Public Property AllowFlatRate() As Boolean
            Get
                Return _AllowFlatRate
            End Get
            Set(ByVal value As Boolean)
                _AllowFlatRate = value
            End Set
        End Property

        Private _AllowDistanceRate As Boolean
        <DataMember()> _
        Public Property AllowDistanceRate() As Boolean
            Get
                Return _AllowDistanceRate
            End Get
            Set(ByVal value As Boolean)
                _AllowDistanceRate = value
            End Set
        End Property

        Private _AllowLTLRate As Boolean
        <DataMember()> _
        Public Property AllowLTLRate() As Boolean
            Get
                Return _AllowLTLRate
            End Get
            Set(ByVal value As Boolean)
                _AllowLTLRate = value
            End Set
        End Property

        Private _AllowUOMRate As Boolean
        <DataMember()> _
        Public Property AllowUOMRate() As Boolean
            Get
                Return _AllowUOMRate
            End Get
            Set(ByVal value As Boolean)
                _AllowUOMRate = value
            End Set
        End Property


        Private _Warning As String = ""
        <DataMember()> _
        Public Property Warning() As String
            Get
                Return _Warning
            End Get
            Set(ByVal value As String)
                _Warning = value
            End Set
        End Property

        Private _BPPivot As CarrTarMatBPPivot
        <DataMember()>
        Public Property BPPivot() As CarrTarMatBPPivot
            Get
                Return _BPPivot
            End Get
            Set(ByVal value As CarrTarMatBPPivot)
                _BPPivot = value
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

        Private _CarrTarEquipMultiOrigRating As Boolean? = False
        ''' <summary>
        ''' Multi Origin Rating Flag
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks>
        ''' Modified by RHR for v-8.5.4.001 on 06/14/2023 new Origin Rate shop logic
        ''' </remarks>
        <DataMember()>
        Public Property CarrTarEquipMultiOrigRating() As Boolean?
            Get
                Return _CarrTarEquipMultiOrigRating
            End Get
            Set(ByVal value As Boolean?)
                If (Me._CarrTarEquipMultiOrigRating <> value) Then
                    Me._CarrTarEquipMultiOrigRating = value
                    Me.SendPropertyChanged("CarrTarEquipMultiOrigRating")
                End If
            End Set
        End Property

#End Region

#Region " Public Methods"
        Public Overrides Function Clone() As DTOBaseClass
            Dim instance As New CarrierTariffsPivot
            instance = DirectCast(MemberwiseClone(), CarrierTariffsPivot)
            instance.BPPivot = instance.BPPivot.Clone()
            Return instance
        End Function

#End Region

    End Class
End Namespace