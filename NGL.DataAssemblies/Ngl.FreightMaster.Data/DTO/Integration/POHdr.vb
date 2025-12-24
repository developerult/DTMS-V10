Imports Microsoft.VisualBasic
Imports System
Imports System.Collections.Generic
Imports System.Linq
Imports System.Text
Imports System.Runtime.Serialization
Imports Ngl.Core.ChangeTracker

Namespace DataTransferObjects
    <DataContract()> _
    Public Class POHdr
        Inherits DTOBaseClass


#Region " Data Members"

        Private _POHdrControl As Long = 0
        <DataMember()> _
        Public Property POHdrControl() As Long
            Get
                Return _POHdrControl
            End Get
            Set(ByVal value As Long)
                _POHdrControl = value
            End Set
        End Property

        Private _POHDRnumber As String = ""
        <DataMember()> _
        Public Property POHDRnumber() As String
            Get
                Return Left(_POHDRnumber, 20)
            End Get
            Set(ByVal value As String)
                _POHDRnumber = Left(value, 20)
            End Set
        End Property

        Private _POHDRvendor As String = ""
        <DataMember()> _
        Public Property POHDRvendor() As String
            Get
                Return Left(_POHDRvendor, 50)
            End Get
            Set(ByVal value As String)
                _POHDRvendor = Left(value, 50)
            End Set
        End Property

        Private _POHDRPOdate As System.Nullable(Of Date)
        <DataMember()> _
        Public Property POHDRPOdate() As System.Nullable(Of Date)
            Get
                Return _POHDRPOdate
            End Get
            Set(ByVal value As System.Nullable(Of Date))
                _POHDRPOdate = value
            End Set
        End Property

        Private _POHDRShipdate As System.Nullable(Of Date)
        <DataMember()> _
        Public Property POHDRShipdate() As System.Nullable(Of Date)
            Get
                Return _POHDRShipdate
            End Get
            Set(ByVal value As System.Nullable(Of Date))
                _POHDRShipdate = value
            End Set
        End Property

        Private _POHDRBuyer As String = ""
        <DataMember()> _
        Public Property POHDRBuyer() As String
            Get
                Return Left(_POHDRBuyer, 10)
            End Get
            Set(ByVal value As String)
                _POHDRBuyer = Left(value, 10)
            End Set
        End Property

        Private _POHDRFrt As System.Nullable(Of Byte)
        <DataMember()> _
        Public Property POHDRFrt() As System.Nullable(Of Byte)
            Get
                Return _POHDRFrt
            End Get
            Set(ByVal value As System.Nullable(Of Byte))
                _POHDRFrt = value
            End Set
        End Property

        Private _POHDRCreateUser As String = ""
        <DataMember()> _
        Public Property POHDRCreateUser() As String
            Get
                Return Left(_POHDRCreateUser, 100)
            End Get
            Set(ByVal value As String)
                _POHDRCreateUser = Left(value, 100)
            End Set
        End Property

        Private _POHDRCreateDate As System.Nullable(Of Date)
        <DataMember()> _
        Public Property POHDRCreateDate() As System.Nullable(Of Date)
            Get
                Return _POHDRCreateDate
            End Get
            Set(ByVal value As System.Nullable(Of Date))
                _POHDRCreateDate = value
            End Set
        End Property

        Private _POHDRModUser As String = ""
        <DataMember()> _
        Public Property POHDRModUser() As String
            Get
                Return Left(_POHDRModUser, 100)
            End Get
            Set(ByVal value As String)
                _POHDRModUser = Left(value, 100)
            End Set
        End Property

        Private _POHDRTotalFrt As System.Nullable(Of Double)
        <DataMember()> _
        Public Property POHDRTotalFrt() As System.Nullable(Of Double)
            Get
                Return _POHDRTotalFrt
            End Get
            Set(ByVal value As System.Nullable(Of Double))
                _POHDRTotalFrt = value
            End Set
        End Property

        Private _POHDRTotalCost As System.Nullable(Of Double)
        <DataMember()> _
        Public Property POHDRTotalCost() As System.Nullable(Of Double)
            Get
                Return _POHDRTotalCost
            End Get
            Set(ByVal value As System.Nullable(Of Double))
                _POHDRTotalCost = value
            End Set
        End Property

        Private _POHDRWgt As System.Nullable(Of Double)
        <DataMember()> _
        Public Property POHDRWgt() As System.Nullable(Of Double)
            Get
                Return _POHDRWgt
            End Get
            Set(ByVal value As System.Nullable(Of Double))
                _POHDRWgt = value
            End Set
        End Property

        Private _POHDRCube As System.Nullable(Of Integer)
        <DataMember()> _
        Public Property POHDRCube() As System.Nullable(Of Integer)
            Get
                Return _POHDRCube
            End Get
            Set(ByVal value As System.Nullable(Of Integer))
                _POHDRCube = value
            End Set
        End Property

        Private _POHDRQty As System.Nullable(Of Integer)
        <DataMember()> _
        Public Property POHDRQty() As System.Nullable(Of Integer)
            Get
                Return _POHDRQty
            End Get
            Set(ByVal value As System.Nullable(Of Integer))
                _POHDRQty = value
            End Set
        End Property

        Private _POHDRLines As System.Nullable(Of Double)
        <DataMember()> _
        Public Property POHDRLines() As System.Nullable(Of Double)
            Get
                Return _POHDRLines
            End Get
            Set(ByVal value As System.Nullable(Of Double))
                _POHDRLines = value
            End Set
        End Property

        Private _POHDRConfirm As Boolean = False
        <DataMember()> _
        Public Property POHDRConfirm() As Boolean
            Get
                Return _POHDRConfirm
            End Get
            Set(ByVal value As Boolean)
                _POHDRConfirm = value
            End Set
        End Property

        Private _POHDRDefaultCustomer As System.Nullable(Of Integer)
        <DataMember()> _
        Public Property POHDRDefaultCustomer() As System.Nullable(Of Integer)
            Get
                Return _POHDRDefaultCustomer
            End Get
            Set(ByVal value As System.Nullable(Of Integer))
                _POHDRDefaultCustomer = value
            End Set
        End Property

        Private _POHDRDefaultCustomerName As String = ""
        <DataMember()> _
        Public Property POHDRDefaultCustomerName() As String
            Get
                Return Left(_POHDRDefaultCustomerName, 50)
            End Get
            Set(ByVal value As String)
                _POHDRDefaultCustomerName = Left(value, 50)
            End Set
        End Property

        Private _POHDRDefaultCarrier As System.Nullable(Of Integer)
        <DataMember()> _
        Public Property POHDRDefaultCarrier() As System.Nullable(Of Integer)
            Get
                Return _POHDRDefaultCarrier
            End Get
            Set(ByVal value As System.Nullable(Of Integer))
                _POHDRDefaultCarrier = value
            End Set
        End Property

        Private _POHDRReqDate As System.Nullable(Of Date)
        <DataMember()> _
        Public Property POHDRReqDate() As System.Nullable(Of Date)
            Get
                Return _POHDRReqDate
            End Get
            Set(ByVal value As System.Nullable(Of Date))
                _POHDRReqDate = value
            End Set
        End Property

        Private _POHDROrderNumber As String = ""
        <DataMember()> _
        Public Property POHDROrderNumber() As String
            Get
                Return Left(_POHDROrderNumber, 20)
            End Get
            Set(ByVal value As String)
                _POHDROrderNumber = Left(value, 20)
            End Set
        End Property

        Private _POHDRShipInstructions As String = ""
        <DataMember()> _
        Public Property POHDRShipInstructions() As String
            Get
                Return Left(_POHDRShipInstructions, 255)
            End Get
            Set(ByVal value As String)
                _POHDRShipInstructions = Left(value, 255)
            End Set
        End Property

        Private _POHDRCooler As System.Nullable(Of Boolean)
        <DataMember()> _
        Public Property POHDRCooler() As System.Nullable(Of Boolean)
            Get
                Return _POHDRCooler
            End Get
            Set(ByVal value As System.Nullable(Of Boolean))
                _POHDRCooler = value
            End Set
        End Property

        Private _POHDRFrozen As System.Nullable(Of Boolean)
        <DataMember()> _
        Public Property POHDRFrozen() As System.Nullable(Of Boolean)
            Get
                Return _POHDRFrozen
            End Get
            Set(ByVal value As System.Nullable(Of Boolean))
                _POHDRFrozen = value
            End Set
        End Property

        Private _POHDRDry As System.Nullable(Of Integer)
        <DataMember()> _
        Public Property POHDRDry() As System.Nullable(Of Integer)
            Get
                Return _POHDRDry
            End Get
            Set(ByVal value As System.Nullable(Of Integer))
                _POHDRDry = value
            End Set
        End Property

        Private _POHDRTemp As String = ""
        <DataMember()> _
        Public Property POHDRTemp() As String
            Get
                Return _POHDRTemp
            End Get
            Set(ByVal value As String)
                _POHDRTemp = Left(value, 1)
            End Set
        End Property

        Private _POHDRModVerify As String = ""
        <DataMember()> _
        Public Property POHDRModVerify() As String
            Get
                Return Left(_POHDRModVerify, 50)
            End Get
            Set(ByVal value As String)
                _POHDRModVerify = Left(value, 50)
            End Set
        End Property

        Private _POHDRCarType As String = ""
        <DataMember()> _
        Public Property POHDRCarType() As String
            Get
                Return Left(_POHDRCarType, 15)
            End Get
            Set(ByVal value As String)
                _POHDRCarType = Left(value, 15)
            End Set
        End Property

        Private _POHDRShipVia As String = ""
        <DataMember()> _
        Public Property POHDRShipVia() As String
            Get
                Return Left(_POHDRShipVia, 10)
            End Get
            Set(ByVal value As String)
                _POHDRShipVia = Left(value, 10)
            End Set
        End Property

        Private _POHDRShipViaType As String = ""
        <DataMember()> _
        Public Property POHDRShipViaType() As String
            Get
                Return Left(_POHDRShipViaType, 10)
            End Get
            Set(ByVal value As String)
                _POHDRShipViaType = Left(value, 10)
            End Set
        End Property


        Private _POHDRPallets As System.Nullable(Of Integer)
        <DataMember()> _
        Public Property POHDRPallets() As System.Nullable(Of Integer)
            Get
                Return _POHDRPallets
            End Get
            Set(ByVal value As System.Nullable(Of Integer))
                _POHDRPallets = value
            End Set
        End Property

        Private _POHDROtherCost As System.Nullable(Of Double)
        <DataMember()> _
        Public Property POHDROtherCost() As System.Nullable(Of Double)
            Get
                Return _POHDROtherCost
            End Get
            Set(ByVal value As System.Nullable(Of Double))
                _POHDROtherCost = value
            End Set
        End Property

        Private _POHDRStatusFlag As System.Nullable(Of Integer)
        <DataMember()> _
        Public Property POHDRStatusFlag() As System.Nullable(Of Integer)
            Get
                Return _POHDRStatusFlag
            End Get
            Set(ByVal value As System.Nullable(Of Integer))
                _POHDRStatusFlag = value
            End Set
        End Property

        Private _POHDRSortOrder As Byte = 0
        <DataMember()> _
        Public Property POHDRSortOrder() As Byte
            Get
                Return _POHDRSortOrder
            End Get
            Set(ByVal value As Byte)
                _POHDRSortOrder = value
            End Set
        End Property

        Private _POHDRPRONumber As String = ""
        <DataMember()> _
        Public Property POHDRPRONumber() As String
            Get
                Return Left(_POHDRPRONumber, 20)
            End Get
            Set(ByVal value As String)
                _POHDRPRONumber = Left(value, 20)
            End Set
        End Property

        Private _POHDRHoldLoad As Boolean = False
        <DataMember()> _
        Public Property POHDRHoldLoad() As Boolean
            Get
                Return _POHDRHoldLoad
            End Get
            Set(ByVal value As Boolean)
                _POHDRHoldLoad = value
            End Set
        End Property

        Private _POHDROrderSequence As Integer = 0
        <DataMember()> _
        Public Property POHDROrderSequence() As Integer
            Get
                Return _POHDROrderSequence
            End Get
            Set(ByVal value As Integer)
                _POHDROrderSequence = value
            End Set
        End Property

        Private _POHDRChepGLID As String = ""
        <DataMember()> _
        Public Property POHDRChepGLID() As String
            Get
                Return Left(_POHDRChepGLID, 50)
            End Get
            Set(ByVal value As String)
                _POHDRChepGLID = Left(value, 50)
            End Set
        End Property

        Private _POHDRCarrierEquipmentCodes As String = ""
        <DataMember()> _
        Public Property POHDRCarrierEquipmentCodes() As String
            Get
                Return Left(_POHDRCarrierEquipmentCodes, 50)
            End Get
            Set(ByVal value As String)
                _POHDRCarrierEquipmentCodes = Left(value, 50)
            End Set
        End Property

        Private _POHDRCarrierTypeCode As String = ""
        <DataMember()> _
        Public Property POHDRCarrierTypeCode() As String
            Get
                Return Left(_POHDRCarrierTypeCode, 20)
            End Get
            Set(ByVal value As String)
                _POHDRCarrierTypeCode = Left(value, 20)
            End Set
        End Property

        Private _POHDRPalletPositions As String = ""
        <DataMember()> _
        Public Property POHDRPalletPositions() As String
            Get
                Return Left(_POHDRPalletPositions, 50)
            End Get
            Set(ByVal value As String)
                _POHDRPalletPositions = Left(value, 50)
            End Set
        End Property

        Private _POHDRSchedulePUDate As System.Nullable(Of Date)
        <DataMember()> _
        Public Property POHDRSchedulePUDate() As System.Nullable(Of Date)
            Get
                Return _POHDRSchedulePUDate
            End Get
            Set(ByVal value As System.Nullable(Of Date))
                _POHDRSchedulePUDate = value
            End Set
        End Property

        Private _POHDRSchedulePUTime As String = ""
        <DataMember()> _
        Public Property POHDRSchedulePUTime() As String
            Get
                Return Left(_POHDRSchedulePUTime, 20)
            End Get
            Set(ByVal value As String)
                _POHDRSchedulePUTime = Left(value, 20)
            End Set
        End Property

        Private _POHDRScheduleDelDate As System.Nullable(Of Date)
        <DataMember()> _
        Public Property POHDRScheduleDelDate() As System.Nullable(Of Date)
            Get
                Return _POHDRScheduleDelDate
            End Get
            Set(ByVal value As System.Nullable(Of Date))
                _POHDRScheduleDelDate = value
            End Set
        End Property

        Private _POHDRScheduleDelTime As String = ""
        <DataMember()> _
        Public Property POHDRScheduleDelTime() As String
            Get
                Return Left(_POHDRScheduleDelTime, 20)
            End Get
            Set(ByVal value As String)
                _POHDRScheduleDelTime = Left(value, 20)
            End Set
        End Property

        Private _POHDRActPUDate As System.Nullable(Of Date)
        <DataMember()> _
        Public Property POHDRActPUDate() As System.Nullable(Of Date)
            Get
                Return _POHDRActPUDate
            End Get
            Set(ByVal value As System.Nullable(Of Date))
                _POHDRActPUDate = value
            End Set
        End Property

        Private _POHDRActPUTime As String = ""
        <DataMember()> _
        Public Property POHDRActPUTime() As String
            Get
                Return Left(_POHDRActPUTime, 20)
            End Get
            Set(ByVal value As String)
                _POHDRActPUTime = Left(value, 20)
            End Set
        End Property

        Private _POHDRActDelDate As System.Nullable(Of Date)
        <DataMember()> _
        Public Property POHDRActDelDate() As System.Nullable(Of Date)
            Get
                Return _POHDRActDelDate
            End Get
            Set(ByVal value As System.Nullable(Of Date))
                _POHDRActDelDate = value
            End Set
        End Property

        Private _POHDRActDelTime As String = ""
        <DataMember()> _
        Public Property POHDRActDelTime() As String
            Get
                Return Left(_POHDRActDelTime, 20)
            End Get
            Set(ByVal value As String)
                _POHDRActDelTime = Left(value, 20)
            End Set
        End Property



        Private _POHDROrigCompNumber As String = ""
        <DataMember()> _
        Public Property POHdrOrigCompNumber As String
            Get
                Return Left(_POHDROrigCompNumber, 50)
            End Get
            Set(value As String)
                _POHDROrigCompNumber = Left(value, 50)
            End Set
        End Property

        Private _POHDROrigName As String = ""
        <DataMember()> _
        Public Property POHDROrigName As String
            Get
                Return Left(_POHDROrigName, 40)
            End Get
            Set(value As String)
                _POHDROrigName = Left(value, 40)
            End Set
        End Property

        Private _POHDROrigAddress1 As String = ""
        <DataMember()> _
        Public Property POHDROrigAddress1 As String
            Get
                Return Left(_POHDROrigAddress1, 40)
            End Get
            Set(value As String)
                _POHDROrigAddress1 = Left(value, 40)
            End Set
        End Property

        Private _POHDROrigAddress2 As String = ""
        <DataMember()> _
        Public Property POHDROrigAddress2 As String
            Get
                Return Left(_POHDROrigAddress2, 40)
            End Get
            Set(value As String)
                _POHDROrigAddress2 = Left(value, 40)
            End Set
        End Property

        Private _POHDROrigAddress3 As String = ""
        <DataMember()> _
        Public Property POHDROrigAddress3 As String
            Get
                Return Left(_POHDROrigAddress3, 40)
            End Get
            Set(value As String)
                _POHDROrigAddress3 = Left(value, 40)
            End Set
        End Property

        Private _POHDROrigCity As String = ""
        <DataMember()> _
        Public Property POHDROrigCity As String
            Get
                Return Left(_POHDROrigCity, 25)
            End Get
            Set(value As String)
                _POHDROrigCity = Left(value, 25)
            End Set
        End Property

        Private _POHDROrigState As String = ""
        <DataMember()> _
        Public Property POHDROrigState As String
            Get
                Return Left(_POHDROrigState, 2)
            End Get
            Set(value As String)
                _POHDROrigState = Left(value, 2)
            End Set
        End Property

        Private _POHDROrigCountry As String = ""
        <DataMember()> _
        Public Property POHDROrigCountry As String
            Get
                Return Left(_POHDROrigCountry, 30)
            End Get
            Set(value As String)
                _POHDROrigCountry = Left(value, 30)
            End Set
        End Property

        Private _POHDROrigZip As String = ""
        <DataMember()> _
        Public Property POHDROrigZip As String
            Get
                Return Left(_POHDROrigZip, 20) 'Modified by RHR for v-8.4.003 on 06/25/2021
            End Get
            Set(value As String)
                _POHDROrigZip = Left(value, 20) 'Modified by RHR for v-8.4.003 on 06/25/2021
            End Set
        End Property

        Private _POHDROrigContactPhone As String = ""
        <DataMember()> _
        Public Property POHDROrigContactPhone As String
            Get
                Return Left(_POHDROrigContactPhone, 20) ' Modified by RHR for v-8.4.003 on 06/25/2021
            End Get
            Set(value As String)
                _POHDROrigContactPhone = Left(value, 20) ' Modified by RHR for v-8.4.003 on 06/25/2021
            End Set
        End Property

        Private _POHDROrigContactPhoneExt As String = ""
        <DataMember()> _
        Public Property POHDROrigContactPhoneExt As String
            Get
                Return Left(_POHDROrigContactPhoneExt, 50)
            End Get
            Set(value As String)
                _POHDROrigContactPhoneExt = Left(value, 50)
            End Set
        End Property

        Private _POHDROrigContactFax As String = ""
        <DataMember()> _
        Public Property POHDROrigContactFax As String
            Get
                Return Left(_POHDROrigContactFax, 15)
            End Get
            Set(value As String)
                _POHDROrigContactFax = Left(value, 15)
            End Set
        End Property

        Private _POHDRDestCompNumber As String = ""
        <DataMember()> _
        Public Property POHDRDestCompNumber As String
            Get
                Return Left(_POHDRDestCompNumber, 50)
            End Get
            Set(value As String)
                _POHDRDestCompNumber = Left(value, 50)
            End Set
        End Property

        Private _POHDRDestName As String = ""
        <DataMember()> _
        Public Property POHDRDestName As String
            Get
                Return Left(_POHDRDestName, 40)
            End Get
            Set(value As String)
                _POHDRDestName = Left(value, 40)
            End Set
        End Property

        Private _POHDRDestAddress1 As String = ""
        <DataMember()> _
        Public Property POHDRDestAddress1 As String
            Get
                Return Left(_POHDRDestAddress1, 40)
            End Get
            Set(value As String)
                _POHDRDestAddress1 = Left(value, 40)
            End Set
        End Property

        Private _POHDRDestAddress2 As String = ""
        <DataMember()> _
        Public Property POHDRDestAddress2 As String
            Get
                Return Left(_POHDRDestAddress2, 40)
            End Get
            Set(value As String)
                _POHDRDestAddress2 = Left(value, 40)
            End Set
        End Property

        Private _POHDRDestAddress3 As String = ""
        <DataMember()> _
        Public Property POHDRDestAddress3 As String
            Get
                Return Left(_POHDRDestAddress3, 40)
            End Get
            Set(value As String)
                _POHDRDestAddress3 = Left(value, 40)
            End Set
        End Property

        Private _POHDRDestCity As String = ""
        <DataMember()> _
        Public Property POHDRDestCity As String
            Get
                Return Left(_POHDRDestCity, 25)
            End Get
            Set(value As String)
                _POHDRDestCity = Left(value, 25)
            End Set
        End Property

        Private _POHDRDestState As String = ""
        <DataMember()> _
        Public Property POHDRDestState As String
            Get
                Return Left(_POHDRDestState, 2)
            End Get
            Set(value As String)
                _POHDRDestState = Left(value, 2)
            End Set
        End Property

        Private _POHDRDestCountry As String = ""
        <DataMember()> _
        Public Property POHDRDestCountry As String
            Get
                Return Left(_POHDRDestCountry, 30)
            End Get
            Set(value As String)
                _POHDRDestCountry = Left(value, 30)
            End Set
        End Property

        Private _POHDRDestZip As String = ""
        <DataMember()> _
        Public Property POHDRDestZip As String
            Get
                Return Left(_POHDRDestZip, 20) 'Modified by RHR for v-8.4.003 on 06/25/2021
            End Get
            Set(value As String)
                _POHDRDestZip = Left(value, 20) 'Modified by RHR for v-8.4.003 on 06/25/2021
            End Set
        End Property

        Private _POHDRDestContactPhone As String = ""
        <DataMember()> _
        Public Property POHDRDestContactPhone As String
            Get
                Return Left(_POHDRDestContactPhone, 20) ' Modified by RHR for v-8.4.003 on 06/25/2021
            End Get
            Set(value As String)
                _POHDRDestContactPhone = Left(value, 20) ' Modified by RHR for v-8.4.003 on 06/25/2021
            End Set
        End Property

        Private _POHDRDestContactPhoneExt As String = ""
        <DataMember()> _
        Public Property POHDRDestContactPhoneExt As String
            Get
                Return Left(_POHDRDestContactPhoneExt, 50)
            End Get
            Set(value As String)
                _POHDRDestContactPhoneExt = Left(value, 50)
            End Set
        End Property

        Private _POHDRDestContactFax As String = ""
        <DataMember()> _
        Public Property POHDRDestContactFax As String
            Get
                Return Left(_POHDRDestContactFax, 15)
            End Get
            Set(value As String)
                _POHDRDestContactFax = Left(value, 15)
            End Set
        End Property

        Private _POHDRPalletExchange As Boolean = False
        <DataMember()> _
        Public Property POHDRPalletExchange As Boolean
            Get
                Return _POHDRPalletExchange
            End Get
            Set(value As Boolean)
                _POHDRPalletExchange = value
            End Set
        End Property

        Private _POHDRPalletType As String = ""
        <DataMember()> _
        Public Property POHDRPalletType As String
            Get
                Return Left(_POHDRPalletType, 50)
            End Get
            Set(value As String)
                _POHDRPalletType = Left(value, 50)
            End Set
        End Property

        Private _POHDRComments As String = ""
        <DataMember()> _
        Public Property POHDRComments As String
            Get
                Return Left(_POHDRComments, 255)
            End Get
            Set(value As String)
                _POHDRComments = Left(value, 255)
            End Set
        End Property

        Private _POHDRCommentsConfidential As String = ""
        <DataMember()> _
        Public Property POHDRCommentsConfidential As String
            Get
                Return Left(_POHDRCommentsConfidential, 255)
            End Get
            Set(value As String)
                _POHDRCommentsConfidential = Left(value, 255)
            End Set
        End Property

        Private _POHDRInbound As Boolean = False
        <DataMember()> _
        Public Property POHDRInbound As Boolean
            Get
                Return _POHDRInbound
            End Get
            Set(value As Boolean)
                _POHDRInbound = value
            End Set
        End Property
         
        Private _POHDRRouteGuideNumber As String = ""
        <DataMember()> _
        Public Property POHDRRouteGuideNumber As String
            Get
                Return Left(_POHDRRouteGuideNumber, 50)
            End Get
            Set(value As String)
                _POHDRRouteGuideNumber = Left(value, 50)
            End Set
        End Property

        Private _POHDRDefaultRouteSequence As Integer = 0
        <DataMember()> _
        Public Property POHDRDefaultRouteSequence() As Integer
            Get
                Return _POHDRDefaultRouteSequence
            End Get
            Set(ByVal value As Integer)
                _POHDRDefaultRouteSequence = value
            End Set
        End Property

        Private _POHdrUpdated As Byte()
        <DataMember()> _
        Public Property POHdrUpdated() As Byte()
            Get
                Return _POHdrUpdated
            End Get
            Set(ByVal value As Byte())
                _POHdrUpdated = value
            End Set
        End Property

        Private _POItems As New List(Of POItem)
        <DataMember()> _
        Public Property POItems() As List(Of POItem)
            Get
                Return _POItems
            End Get
            Set(ByVal value As List(Of POItem))
                _POItems = value
            End Set
        End Property


        Private _POHDRUser1 As String = ""
        <DataMember()> _
        Public Property POHDRUser1() As String
            Get
                Return Left(Me._POHDRUser1, 4000)
            End Get
            Set(value As String)
                If (String.Equals(Me._POHDRUser1, value) = False) Then
                    Me._POHDRUser1 = Left(value, 4000)
                    Me.SendPropertyChanged("POHDRUser1")
                End If
            End Set
        End Property

        Private _POHDRUser2 As String = ""
        <DataMember()> _
        Public Property POHDRUser2() As String
            Get
                Return Left(Me._POHDRUser2, 4000)
            End Get
            Set(value As String)
                If (String.Equals(Me._POHDRUser2, value) = False) Then
                    Me._POHDRUser2 = Left(value, 4000)
                    Me.SendPropertyChanged("POHDRUser2")
                End If
            End Set
        End Property

        Private _POHDRUser3 As String = ""
        <DataMember()> _
        Public Property POHDRUser3() As String
            Get
                Return Left(Me._POHDRUser3, 4000)
            End Get
            Set(value As String)
                If (String.Equals(Me._POHDRUser3, value) = False) Then
                    Me._POHDRUser3 = Left(value, 4000)
                    Me.SendPropertyChanged("POHDRUser3")
                End If
            End Set
        End Property

        Private _POHDRUser4 As String = ""
        <DataMember()> _
        Public Property POHDRUser4() As String
            Get
                Return Left(Me._POHDRUser4, 4000)
            End Get
            Set(value As String)
                If (String.Equals(Me._POHDRUser4, value) = False) Then
                    Me._POHDRUser4 = Left(value, 4000)
                    Me.SendPropertyChanged("POHDRUser4")
                End If
            End Set
        End Property

        Private _POHDRAlphaCode As String = ""
        <DataMember()> _
        Public Property POHDRCompAlphaCode() As String
            Get
                Return Left(_POHDRAlphaCode, 50)
            End Get
            Set(ByVal value As String)
                _POHDRAlphaCode = Left(value, 50)
                Me.SendPropertyChanged("POHDRCompAlphaCode")
            End Set
        End Property

        Private _POHDRLegalEntity As String = ""
        <DataMember()> _
        Public Property POHDRCompLegalEntity() As String
            Get
                Return Left(_POHDRLegalEntity, 50)
            End Get
            Set(ByVal value As String)
                _POHDRLegalEntity = Left(value, 50)
                Me.SendPropertyChanged("POHDRCompLegalEntity")
            End Set
        End Property

        Private _POHDRModeTypeControl As Integer = 0
        <DataMember()> _
        Public Property POHDRModeTypeControl() As Integer
            Get
                Return _POHDRModeTypeControl
            End Get
            Set(ByVal value As Integer)
                _POHDRModeTypeControl = value
                Me.SendPropertyChanged("POHDRModeTypeControl")
            End Set
        End Property

        Private _POHDRMustLeaveByDateTime As System.Nullable(Of Date)
        <DataMember()> _
        Public Property POHDRMustLeaveByDateTime() As System.Nullable(Of Date)
            Get
                Return _POHDRMustLeaveByDateTime
            End Get
            Set(ByVal value As System.Nullable(Of Date))
                _POHDRMustLeaveByDateTime = value
                Me.SendPropertyChanged("POHDRMustLeaveByDateTime")
            End Set
        End Property

#End Region

#Region " Public Methods"
        Public Overrides Function Clone() As DTOBaseClass
            Dim instance As New POHdr
            instance = DirectCast(MemberwiseClone(), POHdr)
            Return instance
        End Function

#End Region

    End Class
End Namespace

