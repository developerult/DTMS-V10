Imports Microsoft.VisualBasic
Imports System
Imports System.Collections.Generic
Imports System.Linq
Imports System.Text
Imports System.Runtime.Serialization
Imports Ngl.Core.ChangeTracker

'Added By LVV on 10/26/16 for v-7.0.5.110 Lane Default Carrier Enhancements 

Namespace DataTransferObjects
    ''' <summary>
    ''' The LimitLaneToCarrier DTO object is used to filter available carriers based on lane preferred settings
    ''' </summary>
    ''' <remarks>
    ''' Added By LVV on 10/26/16 for v-7.0.5.110 Lane Default Carrier Enhancements 
    ''' Modified by RHR for v-7.0.6.0 on 11/04/2016
    '''   added additional values to improve performance.
    '''     CompRestrictCarrierSelection 
    '''     CompWarnOnRestrictedCarrierSelection 
    '''     LaneRestrictCarrierSelection 
    '''     LaneWarnOnRestrictedCarrierSelection
    ''' </remarks>
    <DataContract()> _
    Public Class LimitLaneToCarrier
        Inherits DTOBaseClass


#Region " Data Members"

        Private _LLTCControl As Integer = 0
        <DataMember()> _
        Public Property LLTCControl() As Integer
            Get
                Return _LLTCControl
            End Get
            Set(ByVal value As Integer)
                _LLTCControl = value
            End Set
        End Property

        Private _LLTCCarrierControl As Integer = 0
        <DataMember()> _
        Public Property LLTCCarrierControl() As Integer
            Get
                Return _LLTCCarrierControl
            End Get
            Set(ByVal value As Integer)
                _LLTCCarrierControl = value
            End Set
        End Property

        Private _LLTCCarrierNumber As Integer = 0
        <DataMember()> _
        Public Property LLTCCarrierNumber() As Integer
            Get
                Return _LLTCCarrierNumber
            End Get
            Set(ByVal value As Integer)
                _LLTCCarrierNumber = value
            End Set
        End Property

        Private _LLTCCarrierName As String = ""
        <DataMember()> _
        Public Property LLTCCarrierName() As String
            Get
                Return Left(_LLTCCarrierName, 40)
            End Get
            Set(ByVal value As String)
                _LLTCCarrierName = Left(value, 40)
            End Set
        End Property

        Private _LLTCLaneControl As Integer = 0
        <DataMember()> _
        Public Property LLTCLaneControl() As Integer
            Get
                Return _LLTCLaneControl
            End Get
            Set(ByVal value As Integer)
                _LLTCLaneControl = value
            End Set
        End Property

        Private _LLTCModeTypeControl As Integer = 0
        <DataMember()> _
        Public Property LLTCModeTypeControl() As Integer
            Get
                Return _LLTCModeTypeControl
            End Get
            Set(ByVal value As Integer)
                _LLTCModeTypeControl = value
            End Set
        End Property

        Private _LLTCSequenceNumber As Integer = 0
        <DataMember()> _
        Public Property LLTCSequenceNumber() As Integer
            Get
                Return _LLTCSequenceNumber
            End Get
            Set(ByVal value As Integer)
                _LLTCSequenceNumber = value
            End Set
        End Property

        Private _LLTCSActive As Boolean = True
        <DataMember()> _
        Public Property LLTCSActive() As Boolean
            Get
                Return _LLTCSActive
            End Get
            Set(ByVal value As Boolean)
                _LLTCSActive = value
            End Set
        End Property

        Private _LLTCTempType As Integer = 0
        <DataMember()> _
        Public Property LLTCTempType() As Integer
            Get
                Return _LLTCTempType
            End Get
            Set(ByVal value As Integer)
                _LLTCTempType = value
            End Set
        End Property

        Private _LLTCMaxCases As Integer = 0
        <DataMember()> _
        Public Property LLTCMaxCases() As Integer
            Get
                Return _LLTCMaxCases
            End Get
            Set(ByVal value As Integer)
                _LLTCMaxCases = value
            End Set
        End Property

        Private _LLTCMaxWgt As Double = 0
        <DataMember()> _
        Public Property LLTCMaxWgt() As Double
            Get
                Return _LLTCMaxWgt
            End Get
            Set(ByVal value As Double)
                _LLTCMaxWgt = value
            End Set
        End Property

        Private _LLTCMaxCube As Integer = 0
        <DataMember()> _
        Public Property LLTCMaxCube() As Integer
            Get
                Return _LLTCMaxCube
            End Get
            Set(ByVal value As Integer)
                _LLTCMaxCube = value
            End Set
        End Property

        Private _LLTCMaxPL As Double = 0
        <DataMember()> _
        Public Property LLTCMaxPL() As Double
            Get
                Return _LLTCMaxPL
            End Get
            Set(ByVal value As Double)
                _LLTCMaxPL = value
            End Set
        End Property

        Private _LLTCTariffControl As Integer = 0
        <DataMember()> _
        Public Property LLTCTariffControl() As Integer
            Get
                Return _LLTCTariffControl
            End Get
            Set(ByVal value As Integer)
                _LLTCTariffControl = value
            End Set
        End Property

        Private _LLTCTariffName As String = ""
        <DataMember()> _
        Public Property LLTCTariffName() As String
            Get
                Return Left(_LLTCTariffName, 50)
            End Get
            Set(ByVal value As String)
                _LLTCTariffName = Left(value, 50)
            End Set
        End Property

        Private _LLTCTariffEquip As String = ""
        <DataMember()> _
        Public Property LLTCTariffEquip() As String
            Get
                Return Left(_LLTCTariffEquip, 50)
            End Get
            Set(ByVal value As String)
                _LLTCTariffEquip = Left(value, 50)
            End Set
        End Property

        Private _LLTCMinAllowedCost As Decimal = 0
        <DataMember()> _
        Public Property LLTCMinAllowedCost() As Decimal
            Get
                Return _LLTCMinAllowedCost
            End Get
            Set(ByVal value As Decimal)
                _LLTCMinAllowedCost = value
            End Set
        End Property

        Private _LLTCMaxAllowedCost As Decimal = 0
        <DataMember()> _
        Public Property LLTCMaxAllowedCost() As Decimal
            Get
                Return _LLTCMaxAllowedCost
            End Get
            Set(ByVal value As Decimal)
                _LLTCMaxAllowedCost = value
            End Set
        End Property

        Private _LLTCAllowAutoAssignment As Boolean = True
        <DataMember()> _
        Public Property LLTCAllowAutoAssignment() As Boolean
            Get
                Return _LLTCAllowAutoAssignment
            End Get
            Set(ByVal value As Boolean)
                _LLTCAllowAutoAssignment = value
            End Set
        End Property

        Private _LLTCIgnoreTariff As Boolean = False
        <DataMember()> _
        Public Property LLTCIgnoreTariff() As Boolean
            Get
                Return _LLTCIgnoreTariff
            End Get
            Set(ByVal value As Boolean)
                _LLTCIgnoreTariff = value
            End Set
        End Property

        Private _LLTCModDate As System.Nullable(Of Date)
        <DataMember()> _
        Public Property LLTCModDate() As System.Nullable(Of Date)
            Get
                Return _LLTCModDate
            End Get
            Set(ByVal value As System.Nullable(Of Date))
                _LLTCModDate = value
            End Set
        End Property

        Private _LLTCModUser As String = ""
        <DataMember()> _
        Public Property LLTCModUser() As String
            Get
                Return Left(_LLTCModUser, 100)
            End Get
            Set(ByVal value As String)
                _LLTCModUser = Left(value, 100)
            End Set
        End Property

        Private _LimitLaneToCarrierUpdated As Byte()
        <DataMember()> _
        Public Property LimitLaneToCarrierUpdated() As Byte()
            Get
                Return _LimitLaneToCarrierUpdated
            End Get
            Set(ByVal value As Byte())
                _LimitLaneToCarrierUpdated = value
            End Set
        End Property

        Private _Details As LimitLaneToCarrierDetails()
        <DataMember()> _
        Public Property Details() As LimitLaneToCarrierDetails()
            Get
                Return _Details
            End Get
            Set(ByVal value As LimitLaneToCarrierDetails())
                _Details = value
            End Set
        End Property

        Private _LaneRestrictCarrierSelection As Boolean
        ''' <summary>
        ''' Flag for Lane settings to restrict carrier selection by lane preferred carrier settings
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks>
        ''' Added by RHR for v-7.0.6.0 on 11/04/2016
        ''' </remarks>
        <DataMember()> _
        Public Property LaneRestrictCarrierSelection() As Boolean
            Get
                Return _LaneRestrictCarrierSelection
            End Get
            Set(ByVal value As Boolean)
                _LaneRestrictCarrierSelection = value
            End Set
        End Property

        Private _LaneWarnOnRestrictedCarrierSelection As Boolean
        ''' <summary>
        ''' Flag for Lane settings to show warning if a non-preferred carrier is selected
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks>
        ''' Added by RHR for v-7.0.6.0 on 11/04/2016
        ''' </remarks>
        <DataMember()> _
        Public Property LaneWarnOnRestrictedCarrierSelection() As Boolean
            Get
                Return _LaneWarnOnRestrictedCarrierSelection
            End Get
            Set(ByVal value As Boolean)
                _LaneWarnOnRestrictedCarrierSelection = value
            End Set
        End Property

        Private _CompRestrictCarrierSelection As Boolean
        ''' <summary>
        ''' Flag for Company settings to restrict carrier selection by lane preferred carrier settings
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks>
        ''' Added by RHR for v-7.0.6.0 on 11/04/2016
        ''' </remarks>
        <DataMember()> _
        Public Property CompRestrictCarrierSelection() As Boolean
            Get
                Return _CompRestrictCarrierSelection
            End Get
            Set(ByVal value As Boolean)
                _CompRestrictCarrierSelection = value
            End Set
        End Property

        Private _CompWarnOnRestrictedCarrierSelection As Boolean
        ''' <summary>
        ''' Flag for Company settings to show warning if a non-preferred carrier is selected
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks>
        ''' Added by RHR for v-7.0.6.0 on 11/04/2016
        ''' </remarks>
        <DataMember()> _
        Public Property CompWarnOnRestrictedCarrierSelection() As Boolean
            Get
                Return _CompWarnOnRestrictedCarrierSelection
            End Get
            Set(ByVal value As Boolean)
                _CompWarnOnRestrictedCarrierSelection = value
            End Set
        End Property



        Private _LLTCCarrierContControl As Integer
        ''' <summary>
        ''' Carrier Contact Control Number
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks>
        ''' Added by RHR for v-7.0.6.0 on 11/22/2016
        ''' </remarks>
        <DataMember()> _
        Public Property LLTCCarrierContControl() As Integer
            Get
                Return _LLTCCarrierContControl
            End Get
            Set(ByVal value As Integer)
                _LLTCCarrierContControl = value
            End Set
        End Property

        Private _CarrierContName As String
        ''' <summary>
        ''' Carrier Contact Name
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks>
        ''' Added by RHR for v-7.0.6.0 on 11/22/2016
        ''' </remarks>
        <DataMember()> _
        Public Property CarrierContName() As String
            Get
                Return Left(_CarrierContName, 25)
            End Get
            Set(ByVal value As String)
                _CarrierContName = Left(value, 25)
            End Set
        End Property

        Private _CarrierContact800 As String
        ''' <summary>
        ''' Carrier Contact 800 number
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks>
        ''' Added by RHR for v-7.0.6.0 on 11/22/2016
        ''' </remarks>
        <DataMember()> _
        Public Property CarrierContact800() As String
            Get
                Return Left(_CarrierContact800, 15)
            End Get
            Set(ByVal value As String)
                _CarrierContact800 = Left(value, 15)
            End Set
        End Property

        Private _CarrierContactPhone As String
        ''' <summary>
        ''' Carrier Contact phone number
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks>
        ''' Added by RHR for v-7.0.6.0 on 11/22/2016
        ''' </remarks>
        <DataMember()> _
        Public Property CarrierContactPhone() As String
            Get
                Return Left(_CarrierContactPhone, 20) ' Modified by RHR for v-8.4.003 on 06/25/2021
            End Get
            Set(ByVal value As String)
                _CarrierContactPhone = Left(value, 20) ' Modified by RHR for v-8.4.003 on 06/25/2021
            End Set
        End Property

        Private _CarrierContactFax As String
        ''' <summary>
        ''' Carrier Contact fax number
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks>
        ''' Added by RHR for v-7.0.6.0 on 11/22/2016
        ''' </remarks>
        <DataMember()> _
        Public Property CarrierContactFax() As String
            Get
                Return Left(_CarrierContactFax, 15)
            End Get
            Set(ByVal value As String)
                _CarrierContactFax = Left(value, 15)
            End Set
        End Property

        Private _CarrierContPhoneExt As String
        ''' <summary>
        ''' Carrier Contact phone extension
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks>
        ''' Added by RHR for v-7.0.6.0 on 11/22/2016
        ''' </remarks>
        <DataMember()> _
        Public Property CarrierContPhoneExt() As String
            Get
                Return Left(_CarrierContPhoneExt, 5)
            End Get
            Set(ByVal value As String)
                _CarrierContPhoneExt = Left(value, 5)
            End Set
        End Property

        Private _CarrierContactEMail As String
        ''' <summary>
        ''' Carrier Contact phone extension
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks>
        ''' Added by RHR for v-7.0.6.0 on 11/22/2016
        ''' </remarks>
        <DataMember()> _
        Public Property CarrierContactEMail() As String
            Get
                Return Left(_CarrierContactEMail, 255)
            End Get
            Set(ByVal value As String)
                _CarrierContactEMail = Left(value, 255)
            End Set
        End Property

        Private _CarrierContactDefault As Boolean
        ''' <summary>
        ''' Carrier Contact Default flag
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks>
        ''' Added by RHR for v-7.0.6.0 on 11/22/2016
        ''' </remarks>
        <DataMember()> _
        Public Property CarrierContactDefault() As Boolean
            Get
                Return _CarrierContactDefault
            End Get
            Set(ByVal value As Boolean)
                _CarrierContactDefault = value
            End Set
        End Property



#End Region

#Region " Public Methods"
        Public Overrides Function Clone() As DTOBaseClass
            Dim instance As New LimitLaneToCarrier
            instance = DirectCast(MemberwiseClone(), LimitLaneToCarrier)
            Return instance
        End Function

#End Region

    End Class
End Namespace


