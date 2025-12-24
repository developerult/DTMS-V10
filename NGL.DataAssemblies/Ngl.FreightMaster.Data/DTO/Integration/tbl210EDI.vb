Imports Microsoft.VisualBasic
Imports System
Imports System.Collections.Generic
Imports System.Linq
Imports System.Text
Imports System.Runtime.Serialization
Imports Ngl.Core.ChangeTracker

'Added By LVV 2/29/16 for v-7.0.5.1 EDI Migration
Namespace DataTransferObjects
    <DataContract()> _
    Public Class tbl210EDI
        Inherits DTOBaseClass

#Region " Data Members "

        Private _EDI210Control As Integer = 0
        <DataMember()> _
        Public Property EDI210Control() As Integer
            Get
                Return _EDI210Control
            End Get
            Set(ByVal value As Integer)
                _EDI210Control = value
            End Set
        End Property

        Private _BookControl As Integer = 0
        <DataMember()> _
        Public Property BookControl() As Integer
            Get
                Return _BookControl
            End Get
            Set(ByVal value As Integer)
                _BookControl = value
            End Set
        End Property

        Private _BookProNumber As String = ""
        <DataMember()> _
        Public Property BookProNumber() As String
            Get
                Return Left(_BookProNumber, 20)
            End Get
            Set(ByVal value As String)
                _BookProNumber = Left(value, 20)
            End Set
        End Property

        Private _BookConsPrefix As String = ""
        <DataMember()> _
        Public Property BookConsPrefix() As String
            Get
                Return Left(_BookConsPrefix, 20)
            End Get
            Set(ByVal value As String)
                _BookConsPrefix = Left(value, 20)
            End Set
        End Property

        Private _BookCarrOrderNumber As String = ""
        <DataMember()> _
        Public Property BookCarrOrderNumber() As String
            Get
                Return Left(_BookCarrOrderNumber, 20)
            End Get
            Set(ByVal value As String)
                _BookCarrOrderNumber = Left(value, 20)
            End Set
        End Property

        Private _BookFinARInvoiceDate As System.Nullable(Of Date)
        <DataMember()> _
        Public Property BookFinARInvoiceDate() As System.Nullable(Of Date)
            Get
                Return _BookFinARInvoiceDate
            End Get
            Set(ByVal value As System.Nullable(Of Date))
                _BookFinARInvoiceDate = value
            End Set
        End Property

        Private _BookFinARInvoiceAmt As Decimal = 0
        <DataMember()> _
        Public Property BookFinARInvoiceAmt() As Decimal
            Get
                Return _BookFinARInvoiceAmt
            End Get
            Set(ByVal value As Decimal)
                _BookFinARInvoiceAmt = value
            End Set
        End Property

        Private _Currency As String = ""
        <DataMember()> _
        Public Property Currency() As String
            Get
                Return Left(_Currency, 3)
            End Get
            Set(ByVal value As String)
                _Currency = Left(value, 3)
            End Set
        End Property

        Private _BookDateLoad As System.Nullable(Of Date)
        <DataMember()> _
        Public Property BookDateLoad() As System.Nullable(Of Date)
            Get
                Return _BookDateLoad
            End Get
            Set(ByVal value As System.Nullable(Of Date))
                _BookDateLoad = value
            End Set
        End Property

        Private _BookCarrActDate As System.Nullable(Of Date)
        <DataMember()> _
        Public Property BookCarrActDate() As System.Nullable(Of Date)
            Get
                Return _BookCarrActDate
            End Get
            Set(ByVal value As System.Nullable(Of Date))
                _BookCarrActDate = value
            End Set
        End Property

        Private _BookTypeCode As String = ""
        <DataMember()> _
        Public Property BookTypeCode() As String
            Get
                Return Left(_BookTypeCode, 20)
            End Get
            Set(ByVal value As String)
                _BookTypeCode = Left(value, 20)
            End Set
        End Property

        Private _BookLoadPONumber As String = ""
        <DataMember()> _
        Public Property BookLoadPONumber() As String
            Get
                Return Left(_BookLoadPONumber, 20)
            End Get
            Set(ByVal value As String)
                _BookLoadPONumber = Left(value, 20)
            End Set
        End Property

        Private _BookLoadPONumber2 As String = ""
        <DataMember()> _
        Public Property BookLoadPONumber2() As String
            Get
                Return Left(_BookLoadPONumber2, 20)
            End Get
            Set(ByVal value As String)
                _BookLoadPONumber2 = Left(value, 20)
            End Set
        End Property

        Private _BookLoadPONumber3 As String = ""
        <DataMember()> _
        Public Property BookLoadPONumber3() As String
            Get
                Return Left(_BookLoadPONumber3, 20)
            End Get
            Set(ByVal value As String)
                _BookLoadPONumber3 = Left(value, 20)
            End Set
        End Property

        Private _BookLoadPONumber4 As String = ""
        <DataMember()> _
        Public Property BookLoadPONumber4() As String
            Get
                Return Left(_BookLoadPONumber4, 20)
            End Get
            Set(ByVal value As String)
                _BookLoadPONumber4 = Left(value, 20)
            End Set
        End Property

        Private _BookLoadPONumber5 As String = ""
        <DataMember()> _
        Public Property BookLoadPONumber5() As String
            Get
                Return Left(_BookLoadPONumber5, 20)
            End Get
            Set(ByVal value As String)
                _BookLoadPONumber5 = Left(value, 20)
            End Set
        End Property

        Private _BookLoadPONumber6 As String = ""
        <DataMember()> _
        Public Property BookLoadPONumber6() As String
            Get
                Return Left(_BookLoadPONumber6, 20)
            End Get
            Set(ByVal value As String)
                _BookLoadPONumber6 = Left(value, 20)
            End Set
        End Property

        Private _BookLoadPONumber7 As String = ""
        <DataMember()> _
        Public Property BookLoadPONumber7() As String
            Get
                Return Left(_BookLoadPONumber7, 20)
            End Get
            Set(ByVal value As String)
                _BookLoadPONumber7 = Left(value, 20)
            End Set
        End Property

        Private _BookLoadPONumber8 As String = ""
        <DataMember()> _
        Public Property BookLoadPONumber8() As String
            Get
                Return Left(_BookLoadPONumber8, 20)
            End Get
            Set(ByVal value As String)
                _BookLoadPONumber8 = Left(value, 20)
            End Set
        End Property

        Private _BookLoadPONumber9 As String = ""
        <DataMember()> _
        Public Property BookLoadPONumber9() As String
            Get
                Return Left(_BookLoadPONumber9, 20)
            End Get
            Set(ByVal value As String)
                _BookLoadPONumber9 = Left(value, 20)
            End Set
        End Property

        Private _BookLoadPONumber10 As String = ""
        <DataMember()> _
        Public Property BookLoadPONumber10() As String
            Get
                Return Left(_BookLoadPONumber10, 20)
            End Get
            Set(ByVal value As String)
                _BookLoadPONumber10 = Left(value, 20)
            End Set
        End Property

        Private _BookOrigName As String = ""
        <DataMember()> _
        Public Property BookOrigName() As String
            Get
                Return Left(_BookOrigName, 40)
            End Get
            Set(ByVal value As String)
                _BookOrigName = Left(value, 40)
            End Set
        End Property

        Private _BookOrigAddress1 As String = ""
        <DataMember()> _
        Public Property BookOrigAddress1() As String
            Get
                Return Left(_BookOrigAddress1, 40)
            End Get
            Set(ByVal value As String)
                _BookOrigAddress1 = Left(value, 40)
            End Set
        End Property

        Private _BookOrigAddress2 As String = ""
        <DataMember()> _
        Public Property BookOrigAddress2() As String
            Get
                Return Left(_BookOrigAddress2, 40)
            End Get
            Set(ByVal value As String)
                _BookOrigAddress2 = Left(value, 40)
            End Set
        End Property

        Private _BookOrigCity As String = ""
        <DataMember()> _
        Public Property BookOrigCity() As String
            Get
                Return Left(_BookOrigCity, 25)
            End Get
            Set(ByVal value As String)
                _BookOrigCity = Left(value, 25)
            End Set
        End Property

        Private _BookOrigState As String = ""
        <DataMember()> _
        Public Property BookOrigState() As String
            Get
                Return Left(_BookOrigState, 8)
            End Get
            Set(ByVal value As String)
                _BookOrigState = Left(value, 8)
            End Set
        End Property

        Private _BookOrigCountry As String = ""
        <DataMember()> _
        Public Property BookOrigCountry() As String
            Get
                Return Left(_BookOrigCountry, 30)
            End Get
            Set(ByVal value As String)
                _BookOrigCountry = Left(value, 30)
            End Set
        End Property

        Private _BookOrigZip As String = ""
        <DataMember()> _
        Public Property BookOrigZip() As String
            Get
                Return Left(_BookOrigZip, 20) 'Modified by RHR for v-8.4.003 on 06/25/2021
            End Get
            Set(ByVal value As String)
                _BookOrigZip = Left(value, 20) 'Modified by RHR for v-8.4.003 on 06/25/2021
            End Set
        End Property

        Private _BookDestName As String = ""
        <DataMember()> _
        Public Property BookDestName() As String
            Get
                Return Left(_BookDestName, 40)
            End Get
            Set(ByVal value As String)
                _BookDestName = Left(value, 40)
            End Set
        End Property

        Private _BookDestAddress1 As String = ""
        <DataMember()> _
        Public Property BookDestAddress1() As String
            Get
                Return Left(_BookDestAddress1, 40)
            End Get
            Set(ByVal value As String)
                _BookDestAddress1 = Left(value, 40)
            End Set
        End Property

        Private _BookDestAddress2 As String = ""
        <DataMember()> _
        Public Property BookDestAddress2() As String
            Get
                Return Left(_BookDestAddress2, 40)
            End Get
            Set(ByVal value As String)
                _BookDestAddress2 = Left(value, 40)
            End Set
        End Property

        Private _BookDestCity As String = ""
        <DataMember()> _
        Public Property BookDestCity() As String
            Get
                Return Left(_BookDestCity, 25)
            End Get
            Set(ByVal value As String)
                _BookDestCity = Left(value, 25)
            End Set
        End Property

        Private _BookDestState As String = ""
        <DataMember()> _
        Public Property BookDestState() As String
            Get
                Return Left(_BookDestState, 2)
            End Get
            Set(ByVal value As String)
                _BookDestState = Left(value, 2)
            End Set
        End Property

        Private _BookDestCountry As String = ""
        <DataMember()> _
        Public Property BookDestCountry() As String
            Get
                Return Left(_BookDestCountry, 30)
            End Get
            Set(ByVal value As String)
                _BookDestCountry = Left(value, 30)
            End Set
        End Property

        Private _BookDestZip As String = ""
        <DataMember()> _
        Public Property BookDestZip() As String
            Get
                Return Left(_BookDestZip, 20) 'Modified by RHR for v-8.4.003 on 06/25/2021
            End Get
            Set(ByVal value As String)
                _BookDestZip = Left(value, 20) 'Modified by RHR for v-8.4.003 on 06/25/2021
            End Set
        End Property

        Private _BookTotalWgt As Double = 0
        <DataMember()> _
        Public Property BookTotalWgt() As Double
            Get
                Return _BookTotalWgt
            End Get
            Set(ByVal value As Double)
                _BookTotalWgt = value
            End Set
        End Property

        Private _BookRevBilledBFC As Decimal = 0
        <DataMember()> _
        Public Property BookRevBilledBFC() As Decimal
            Get
                Return _BookRevBilledBFC
            End Get
            Set(ByVal value As Decimal)
                _BookRevBilledBFC = value
            End Set
        End Property

        Private _CarrierSCAC As String = ""
        <DataMember()> _
        Public Property CarrierSCAC() As String
            Get
                Return Left(_CarrierSCAC, 4)
            End Get
            Set(ByVal value As String)
                _CarrierSCAC = Left(value, 4)
            End Set
        End Property

        Private _CarrierName As String = ""
        <DataMember()> _
        Public Property CarrierName() As String
            Get
                Return Left(_CarrierName, 40)
            End Get
            Set(ByVal value As String)
                _CarrierName = Left(value, 40)
            End Set
        End Property

        Private _CarrierControl As Integer = 0
        <DataMember()> _
        Public Property CarrierControl() As Integer
            Get
                Return _CarrierControl
            End Get
            Set(ByVal value As Integer)
                _CarrierControl = value
            End Set
        End Property

        Private _CarrierNumber As Integer = 0
        <DataMember()> _
        Public Property CarrierNumber() As Integer
            Get
                Return _CarrierNumber
            End Get
            Set(ByVal value As Integer)
                _CarrierNumber = value
            End Set
        End Property

        Private _CompName As String = ""
        <DataMember()> _
        Public Property CompName() As String
            Get
                Return Left(_CompName, 40)
            End Get
            Set(ByVal value As String)
                _CompName = Left(value, 40)
            End Set
        End Property

        Private _CompNumber As Integer = 0
        <DataMember()> _
        Public Property CompNumber() As Integer
            Get
                Return _CompNumber
            End Get
            Set(ByVal value As Integer)
                _CompNumber = value
            End Set
        End Property

        Private _CompControl As Integer = 0
        <DataMember()> _
        Public Property CompControl() As Integer
            Get
                Return _CompControl
            End Get
            Set(ByVal value As Integer)
                _CompControl = value
            End Set
        End Property

        Private _LaneNumber As String = ""
        <DataMember()> _
        Public Property LaneNumber() As String
            Get
                Return Left(_LaneNumber, 50)
            End Get
            Set(ByVal value As String)
                _LaneNumber = Left(value, 50)
            End Set
        End Property

        Private _LaneOriginAddressUse As Boolean = False
        <DataMember()> _
        Public Property LaneOriginAddressUse() As Boolean
            Get
                Return _LaneOriginAddressUse
            End Get
            Set(ByVal value As Boolean)
                _LaneOriginAddressUse = value
            End Set
        End Property

        Private _CorrectionIndicator As String = ""
        <DataMember()> _
        Public Property CorrectionIndicator() As String
            Get
                Return Left(_CorrectionIndicator, 2)
            End Get
            Set(ByVal value As String)
                _CorrectionIndicator = Left(value, 2)
            End Set
        End Property

        Private _EDI400LoopFeesProcessed As Boolean = False
        <DataMember()> _
        Public Property EDI400LoopFeesProcessed() As Boolean
            Get
                Return _EDI400LoopFeesProcessed
            End Get
            Set(ByVal value As Boolean)
                _EDI400LoopFeesProcessed = value
            End Set
        End Property

        Private _FirstDateSent As System.Nullable(Of Date)
        <DataMember()> _
        Public Property FirstDateSent() As System.Nullable(Of Date)
            Get
                Return _FirstDateSent
            End Get
            Set(ByVal value As System.Nullable(Of Date))
                _FirstDateSent = value
            End Set
        End Property

        Private _LastDateSent As System.Nullable(Of Date)
        <DataMember()> _
        Public Property LastDateSent() As System.Nullable(Of Date)
            Get
                Return _LastDateSent
            End Get
            Set(ByVal value As System.Nullable(Of Date))
                _LastDateSent = value
            End Set
        End Property

        Private _EDI997Received As Boolean = False
        <DataMember()> _
        Public Property EDI997Received() As Boolean
            Get
                Return _EDI997Received
            End Get
            Set(ByVal value As Boolean)
                _EDI997Received = value
            End Set
        End Property

        Private _EDI997ReceivedDate As System.Nullable(Of Date)
        <DataMember()> _
        Public Property EDI997ReceivedDate() As System.Nullable(Of Date)
            Get
                Return _EDI997ReceivedDate
            End Get
            Set(ByVal value As System.Nullable(Of Date))
                _EDI997ReceivedDate = value
            End Set
        End Property

        Private _EDI210Retry As Integer = 0
        <DataMember()> _
        Public Property EDI210Retry() As Integer
            Get
                Return _EDI210Retry
            End Get
            Set(ByVal value As Integer)
                _EDI210Retry = value
            End Set
        End Property

        Private _BookTotalPL As Double = 0
        <DataMember()> _
        Public Property BookTotalPL() As Double
            Get
                Return _BookTotalPL
            End Get
            Set(ByVal value As Double)
                _BookTotalPL = value
            End Set
        End Property

        Private _CompEDISecurityQual As String = ""
        <DataMember()> _
        Public Property CompEDISecurityQual() As String
            Get
                Return Left(_CompEDISecurityQual, 2)
            End Get
            Set(ByVal value As String)
                _CompEDISecurityQual = Left(value, 2)
            End Set
        End Property

        Private _CompEDISecurityCode As String = ""
        <DataMember()> _
        Public Property CompEDISecurityCode() As String
            Get
                Return Left(_CompEDISecurityCode, 10)
            End Get
            Set(ByVal value As String)
                _CompEDISecurityCode = Left(value, 10)
            End Set
        End Property

        Private _CompEDIPartnerQual As String = ""
        <DataMember()> _
        Public Property CompEDIPartnerQual() As String
            Get
                Return Left(_CompEDIPartnerQual, 2)
            End Get
            Set(ByVal value As String)
                _CompEDIPartnerQual = Left(value, 2)
            End Set
        End Property

        Private _CompEDIPartnerCode As String = ""
        <DataMember()> _
        Public Property CompEDIPartnerCode() As String
            Get
                Return Left(_CompEDIPartnerCode, 15)
            End Get
            Set(ByVal value As String)
                _CompEDIPartnerCode = Left(value, 15)
            End Set
        End Property

        Private _ParamCompEDIPartnerQual As String = ""
        <DataMember()> _
        Public Property ParamCompEDIPartnerQual() As String
            Get
                Return Left(_ParamCompEDIPartnerQual, 2)
            End Get
            Set(ByVal value As String)
                _ParamCompEDIPartnerQual = Left(value, 2)
            End Set
        End Property

        Private _ParamCompEDIPartnerCode As String = ""
        <DataMember()> _
        Public Property ParamCompEDIPartnerCode() As String
            Get
                Return Left(_ParamCompEDIPartnerCode, 15)
            End Get
            Set(ByVal value As String)
                _ParamCompEDIPartnerCode = Left(value, 15)
            End Set
        End Property

        Private _EDI210StatusCode As Integer? = 0
        <DataMember()> _
        Public Property EDI210StatusCode() As Integer?
            Get
                Return _EDI210StatusCode
            End Get
            Set(ByVal value As Integer?)
                _EDI210StatusCode = value
            End Set
        End Property

        Private _EDI210Message As String = ""
        <DataMember()> _
        Public Property EDI210Message() As String
            Get
                Return _EDI210Message
            End Get
            Set(ByVal value As String)
                _EDI210Message = value
            End Set
        End Property

        Private _Archived As Boolean = False
        <DataMember()> _
        Public Property Archived() As Boolean
            Get
                Return _Archived
            End Get
            Set(ByVal value As Boolean)
                _Archived = value
            End Set
        End Property

        Private _EDI210ModDate As System.Nullable(Of Date)
        <DataMember()> _
        Public Property EDI210ModDate() As System.Nullable(Of Date)
            Get
                Return _EDI210ModDate
            End Get
            Set(ByVal value As System.Nullable(Of Date))
                _EDI210ModDate = value
                Me.SendPropertyChanged("EDI210ModDate")
            End Set
        End Property

        Private _EDI210ModUser As String = ""
        <DataMember()> _
        Public Property EDI210ModUser() As String
            Get
                Return Left(_EDI210ModUser, 100)
            End Get
            Set(ByVal value As String)
                _EDI210ModUser = Left(value, 100)
                Me.SendPropertyChanged("EDI210ModUser")
            End Set
        End Property

        Private _EDI210FileName210 As String = ""
        <DataMember()> _
        Public Property EDI210FileName210() As String
            Get
                Return Left(_EDI210FileName210, 255)
            End Get
            Set(ByVal value As String)
                _EDI210FileName210 = Left(value, 255)
                Me.SendPropertyChanged("EDI210FileName210")
            End Set
        End Property

        Private _EDI210FileName997 As String = ""
        <DataMember()> _
        Public Property EDI210FileName997() As String
            Get
                Return Left(_EDI210FileName997, 255)
            End Get
            Set(ByVal value As String)
                _EDI210FileName997 = Left(value, 255)
                Me.SendPropertyChanged("EDI210FileName997")
            End Set
        End Property

        Private _EDI210FileName820 As String = ""
        <DataMember()> _
        Public Property EDI210FileName820() As String
            Get
                Return Left(_EDI210FileName820, 255)
            End Get
            Set(ByVal value As String)
                _EDI210FileName820 = Left(value, 255)
                Me.SendPropertyChanged("EDI210FileName820")
            End Set
        End Property

        Private _EDI210Updated As Byte()
        <DataMember()> _
        Public Property EDI210Updated() As Byte()
            Get
                Return _EDI210Updated
            End Get
            Set(ByVal value As Byte())
                _EDI210Updated = value
            End Set
        End Property


#End Region

#Region " Public Methods"
        Public Overrides Function Clone() As DTOBaseClass
            Dim instance As New tbl210EDI
            instance = DirectCast(MemberwiseClone(), tbl210EDI)
            Return instance
        End Function

#End Region



    End Class
End Namespace
