Imports Microsoft.VisualBasic
Imports System
Imports System.Collections.Generic
Imports System.Linq
Imports System.Text
Imports System.Runtime.Serialization
Imports Ngl.Core.ChangeTracker

Namespace DataTransferObjects
    <DataContract()> _
    Public Class BookFinancial
        Inherits DTOBaseClass


#Region " Data Members"
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

        Private _BookCarrierControl As Integer = 0
        <DataMember()> _
        Public Property BookCarrierControl() As Integer
            Get
                Return _BookCarrierControl
            End Get
            Set(ByVal value As Integer)
                _BookCarrierControl = value
            End Set
        End Property

        Private _BookModDate As System.Nullable(Of Date)
        <DataMember()> _
        Public Property BookModDate() As System.Nullable(Of Date)
            Get
                Return _BookModDate
            End Get
            Set(ByVal value As System.Nullable(Of Date))
                _BookModDate = value
            End Set
        End Property

        Private _BookModUser As String = ""
        <DataMember()> _
        Public Property BookModUser() As String
            Get
                Return Left(_BookModUser, 100)
            End Get
            Set(ByVal value As String)
                _BookModUser = Left(value, 100)
            End Set
        End Property

        Private _BookDoNotInvoice As Boolean = False
        <DataMember()> _
        Public Property BookDoNotInvoice() As Boolean
            Get
                Return _BookDoNotInvoice
            End Get
            Set(ByVal value As Boolean)
                _BookDoNotInvoice = value
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

        Private _BookRouteFinalCode As String = ""
        <DataMember()> _
        Public Property BookRouteFinalCode() As String
            Get
                Return Left(_BookRouteFinalCode, 2)
            End Get
            Set(ByVal value As String)
                _BookRouteFinalCode = Left(value, 2)
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

        Private _BookFinARBookFrt As Decimal = 0
        <DataMember()> _
        Public Property BookFinARBookFrt() As Decimal
            Get
                Return _BookFinARBookFrt
            End Get
            Set(ByVal value As Decimal)
                _BookFinARBookFrt = value
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

        Private _BookFinARPayDate As System.Nullable(Of Date)
        <DataMember()> _
        Public Property BookFinARPayDate() As System.Nullable(Of Date)
            Get
                Return _BookFinARPayDate
            End Get
            Set(ByVal value As System.Nullable(Of Date))
                _BookFinARPayDate = value
            End Set
        End Property

        Private _BookFinARPayAmt As Decimal = 0
        <DataMember()> _
        Public Property BookFinARPayAmt() As Decimal
            Get
                Return _BookFinARPayAmt
            End Get
            Set(ByVal value As Decimal)
                _BookFinARPayAmt = value
            End Set
        End Property

        Private _BookFinARCheck As String = ""
        <DataMember()> _
        Public Property BookFinARCheck() As String
            Get
                Return Left(_BookFinARCheck, 50)
            End Get
            Set(ByVal value As String)
                _BookFinARCheck = Left(value, 50)
            End Set
        End Property

        Private _BookFinARGLNumber As String = ""
        <DataMember()> _
        Public Property BookFinARGLNumber() As String
            Get
                Return Left(_BookFinARGLNumber, 50)
            End Get
            Set(ByVal value As String)
                _BookFinARGLNumber = Left(value, 50)
            End Set
        End Property

        Private _BookFinARBalance As Decimal = 0
        <DataMember()> _
        Public Property BookFinARBalance() As Decimal
            Get
                Return _BookFinARBalance
            End Get
            Set(ByVal value As Decimal)
                _BookFinARBalance = value
            End Set
        End Property

        Private _BookFinARCurType As Integer = 0
        <DataMember()> _
        Public Property BookFinARCurType() As Integer
            Get
                Return _BookFinARCurType
            End Get
            Set(ByVal value As Integer)
                _BookFinARCurType = value
            End Set
        End Property

        Private _BookFinAPBillNumber As String = ""
        <DataMember()> _
        Public Property BookFinAPBillNumber() As String
            Get
                Return Left(_BookFinAPBillNumber, 50)
            End Get
            Set(ByVal value As String)
                _BookFinAPBillNumber = Left(value, 50)
            End Set
        End Property

        Private _BookFinAPBillNoDate As System.Nullable(Of Date)
        <DataMember()> _
        Public Property BookFinAPBillNoDate() As System.Nullable(Of Date)
            Get
                Return _BookFinAPBillNoDate
            End Get
            Set(ByVal value As System.Nullable(Of Date))
                _BookFinAPBillNoDate = value
            End Set
        End Property

        Private _BookFinAPBillInvDate As System.Nullable(Of Date)
        <DataMember()> _
        Public Property BookFinAPBillInvDate() As System.Nullable(Of Date)
            Get
                Return _BookFinAPBillInvDate
            End Get
            Set(ByVal value As System.Nullable(Of Date))
                _BookFinAPBillInvDate = value
            End Set
        End Property

        Private _BookFinAPActWgt As Integer = 0
        <DataMember()> _
        Public Property BookFinAPActWgt() As Integer
            Get
                Return _BookFinAPActWgt
            End Get
            Set(ByVal value As Integer)
                _BookFinAPActWgt = value
            End Set
        End Property

        Private _BookFinAPStdCost As Decimal = 0
        <DataMember()> _
        Public Property BookFinAPStdCost() As Decimal
            Get
                Return _BookFinAPStdCost
            End Get
            Set(ByVal value As Decimal)
                _BookFinAPStdCost = value
            End Set
        End Property

        Private _BookFinAPActCost As Decimal = 0
        <DataMember()> _
        Public Property BookFinAPActCost() As Decimal
            Get
                Return _BookFinAPActCost
            End Get
            Set(ByVal value As Decimal)
                _BookFinAPActCost = value
            End Set
        End Property

        Private _BookFinAPPayDate As System.Nullable(Of Date)
        <DataMember()> _
        Public Property BookFinAPPayDate() As System.Nullable(Of Date)
            Get
                Return _BookFinAPPayDate
            End Get
            Set(ByVal value As System.Nullable(Of Date))
                _BookFinAPPayDate = value
            End Set
        End Property

        Private _BookFinAPPayAmt As Decimal = 0
        <DataMember()> _
        Public Property BookFinAPPayAmt() As Decimal
            Get
                Return _BookFinAPPayAmt
            End Get
            Set(ByVal value As Decimal)
                _BookFinAPPayAmt = value
            End Set
        End Property

        Private _BookFinAPCheck As String = ""
        <DataMember()> _
        Public Property BookFinAPCheck() As String
            Get
                Return Left(_BookFinAPCheck, 15)
            End Get
            Set(ByVal value As String)
                _BookFinAPCheck = Left(value, 15)
            End Set
        End Property

        Private _BookFinAPGLNumber As String = ""
        <DataMember()> _
        Public Property BookFinAPGLNumber() As String
            Get
                Return Left(_BookFinAPGLNumber, 50)
            End Get
            Set(ByVal value As String)
                _BookFinAPGLNumber = Left(value, 50)
            End Set
        End Property

        Private _BookFinAPLastViewed As System.Nullable(Of Date)
        <DataMember()> _
        Public Property BookFinAPLastViewed() As System.Nullable(Of Date)
            Get
                Return _BookFinAPLastViewed
            End Get
            Set(ByVal value As System.Nullable(Of Date))
                _BookFinAPLastViewed = value
            End Set
        End Property

        Private _BookFinAPCurType As Integer = 0
        <DataMember()> _
        Public Property BookFinAPCurType() As Integer
            Get
                Return _BookFinAPCurType
            End Get
            Set(ByVal value As Integer)
                _BookFinAPCurType = value
            End Set
        End Property

        Private _BookFinCommStd As Decimal = 0
        <DataMember()> _
        Public Property BookFinCommStd() As Decimal
            Get
                Return _BookFinCommStd
            End Get
            Set(ByVal value As Decimal)
                _BookFinCommStd = value
            End Set
        End Property

        Private _BookFinCommAct As Decimal = 0
        <DataMember()> _
        Public Property BookFinCommAct() As Decimal
            Get
                Return _BookFinCommAct
            End Get
            Set(ByVal value As Decimal)
                _BookFinCommAct = value
            End Set
        End Property

        Private _BookFinCommPayDate As System.Nullable(Of Date)
        <DataMember()> _
        Public Property BookFinCommPayDate() As System.Nullable(Of Date)
            Get
                Return _BookFinCommPayDate
            End Get
            Set(ByVal value As System.Nullable(Of Date))
                _BookFinCommPayDate = value
            End Set
        End Property

        Private _BookFinCommPayAmt As Decimal = 0
        <DataMember()> _
        Public Property BookFinCommPayAmt() As Decimal
            Get
                Return _BookFinCommPayAmt
            End Get
            Set(ByVal value As Decimal)
                _BookFinCommPayAmt = value
            End Set
        End Property

        Private _BookFinCommtCheck As String = ""
        <DataMember()> _
        Public Property BookFinCommtCheck() As String
            Get
                Return Left(_BookFinCommtCheck, 15)
            End Get
            Set(ByVal value As String)
                _BookFinCommtCheck = Left(value, 15)
            End Set
        End Property

        Private _BookFinCommCreditAmt As Decimal = 0
        <DataMember()> _
        Public Property BookFinCommCreditAmt() As Decimal
            Get
                Return _BookFinCommCreditAmt
            End Get
            Set(ByVal value As Decimal)
                _BookFinCommCreditAmt = value
            End Set
        End Property

        Private _BookFinCommCreditPayDate As System.Nullable(Of Date)
        <DataMember()> _
        Public Property BookFinCommCreditPayDate() As System.Nullable(Of Date)
            Get
                Return _BookFinCommCreditPayDate
            End Get
            Set(ByVal value As System.Nullable(Of Date))
                _BookFinCommCreditPayDate = value
            End Set
        End Property

        Private _BookFinCommLoadCount As Integer = 0
        <DataMember()> _
        Public Property BookFinCommLoadCount() As Integer
            Get
                Return _BookFinCommLoadCount
            End Get
            Set(ByVal value As Integer)
                _BookFinCommLoadCount = value
            End Set
        End Property

        Private _BookFinCommGLNumber As String = ""
        <DataMember()> _
        Public Property BookFinCommGLNumber() As String
            Get
                Return Left(_BookFinCommGLNumber, 50)
            End Get
            Set(ByVal value As String)
                _BookFinCommGLNumber = Left(value, 50)
            End Set
        End Property

        Private _BookFinCheckClearedDate As System.Nullable(Of Date)
        <DataMember()> _
        Public Property BookFinCheckClearedDate() As System.Nullable(Of Date)
            Get
                Return _BookFinCheckClearedDate
            End Get
            Set(ByVal value As System.Nullable(Of Date))
                _BookFinCheckClearedDate = value
            End Set
        End Property

        Private _BookFinCheckClearedNumber As String = ""
        <DataMember()> _
        Public Property BookFinCheckClearedNumber() As String
            Get
                Return Left(_BookFinCheckClearedNumber, 50)
            End Get
            Set(ByVal value As String)
                _BookFinCheckClearedNumber = Left(value, 50)
            End Set
        End Property

        Private _BookFinCheckClearedAmt As Decimal = 0
        <DataMember()> _
        Public Property BookFinCheckClearedAmt() As Decimal
            Get
                Return _BookFinCheckClearedAmt
            End Get
            Set(ByVal value As Decimal)
                _BookFinCheckClearedAmt = value
            End Set
        End Property

        Private _BookFinCheckClearedDesc As String = ""
        <DataMember()> _
        Public Property BookFinCheckClearedDesc() As String
            Get
                Return Left(_BookFinCheckClearedDesc, 50)
            End Get
            Set(ByVal value As String)
                _BookFinCheckClearedDesc = Left(value, 50)
            End Set
        End Property

        Private _BookFinCheckClearedAcct As String = ""
        <DataMember()> _
        Public Property BookFinCheckClearedAcct() As String
            Get
                Return Left(_BookFinCheckClearedAcct, 50)
            End Get
            Set(ByVal value As String)
                _BookFinCheckClearedAcct = Left(value, 50)
            End Set
        End Property

        Private _BookFinAPActTax As Decimal = 0
        <DataMember()> _
        Public Property BookFinAPActTax() As Decimal
            Get
                Return _BookFinAPActTax
            End Get
            Set(ByVal value As Decimal)
                _BookFinAPActTax = value
            End Set
        End Property

        Private _BookFinAPExportFlag As Boolean = False
        <DataMember()> _
        Public Property BookFinAPExportFlag() As Boolean
            Get
                Return _BookFinAPExportFlag
            End Get
            Set(ByVal value As Boolean)
                _BookFinAPExportFlag = value
            End Set
        End Property

        Private _BookFinARFreightTax As Decimal = 0
        <DataMember()> _
        Public Property BookFinARFreightTax() As Decimal
            Get
                Return _BookFinARFreightTax
            End Get
            Set(ByVal value As Decimal)
                _BookFinARFreightTax = value
            End Set
        End Property

        Private _BookFinServiceFee As Decimal = 0
        <DataMember()> _
        Public Property BookFinServiceFee() As Decimal
            Get
                Return _BookFinServiceFee
            End Get
            Set(ByVal value As Decimal)
                _BookFinServiceFee = value
            End Set
        End Property

        Private _BookFinAPExportDate As System.Nullable(Of Date)
        <DataMember()> _
        Public Property BookFinAPExportDate() As System.Nullable(Of Date)
            Get
                Return _BookFinAPExportDate
            End Get
            Set(ByVal value As System.Nullable(Of Date))
                _BookFinAPExportDate = value
            End Set
        End Property

        Private _BookFinAPExportRetry As Integer = 0
        <DataMember()> _
        Public Property BookFinAPExportRetry() As Integer
            Get
                Return _BookFinAPExportRetry
            End Get
            Set(ByVal value As Integer)
                _BookFinAPExportRetry = value
            End Set
        End Property

        Private _ARCustomerText As String = ""
        <DataMember()> _
        Public Property ARCustomerText() As String
            Get
                Return _ARCustomerText
            End Get
            Friend Set(ByVal value As String)
                _ARCustomerText = value
            End Set
        End Property


        Private _APCarrierText As String = ""
        <DataMember()> _
        Public Property APCarrierText() As String
            Get
                Return _APCarrierText
            End Get
            Friend Set(ByVal value As String)
                _APCarrierText = value
            End Set
        End Property

        Private _APCommissionsText As String = ""
        <DataMember()> _
        Public Property APCommissionsText() As String
            Get
                Return _APCommissionsText
            End Get
            Friend Set(ByVal value As String)
                _APCommissionsText = value
            End Set
        End Property

        Private _BookSHID As String
        <DataMember()> _
        Public Property BookSHID() As String
            Get
                Return Left(_BookSHID, 50)
            End Get
            Set(ByVal value As String)
                _BookSHID = Left(value, 50)
            End Set
        End Property

       
#End Region

#Region " Public Methods"
        Public Overrides Function Clone() As DTOBaseClass
            Dim instance As New BookFinancial
            instance = DirectCast(MemberwiseClone(), BookFinancial)
            Return instance
        End Function

#End Region

    End Class
End Namespace
