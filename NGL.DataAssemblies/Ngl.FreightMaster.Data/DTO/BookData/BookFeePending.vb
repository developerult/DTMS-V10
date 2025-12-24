Imports Microsoft.VisualBasic
Imports System
Imports System.Collections.Generic
Imports System.Linq
Imports System.Text
Imports System.Runtime.Serialization
Imports Ngl.Core.ChangeTracker

Namespace DataTransferObjects
    ''' <summary>
    ''' A list of Pending Fees manually enterd via Settlement, Electronic API, EDI or via the AP Fee Approval screen
    ''' </summary>
    ''' <remarks>
    ''' Modified by RHR for v-8.2 on 7/3/2019
    ''' Added BookFeesPendingMissingFee a flag used to identify pending fees created by 
    ''' TMS Users when a carrier fails to send an expected fee electronically the default is false.
    ''' </remarks>
    <DataContract()>
    Public Class BookFeePending
        Inherits DTOBaseClass


#Region " Data Members"
        Private _BookFeesPendingControl As Integer = 0
        <DataMember()>
        Public Property BookFeesPendingControl() As Integer
            Get
                Return _BookFeesPendingControl
            End Get
            Set(ByVal value As Integer)
                _BookFeesPendingControl = value
            End Set
        End Property

        Private _BookFeesPendingBookControl As Integer = 0
        <DataMember()>
        Public Property BookFeesPendingBookControl() As Integer
            Get
                Return _BookFeesPendingBookControl
            End Get
            Set(ByVal value As Integer)
                _BookFeesPendingBookControl = value
            End Set
        End Property

        Private _BookFeesPendingMinimum As Decimal = 0
        <DataMember()>
        Public Property BookFeesPendingMinimum() As Decimal
            Get
                Return _BookFeesPendingMinimum
            End Get
            Set(ByVal value As Decimal)
                _BookFeesPendingMinimum = value
            End Set
        End Property

        Private _BookFeesPendingValue As Decimal = 0
        <DataMember()>
        Public Property BookFeesPendingValue() As Decimal
            Get
                Return _BookFeesPendingValue
            End Get
            Set(ByVal value As Decimal)
                _BookFeesPendingValue = value
            End Set
        End Property

        Private _BookFeesPendingVariable As Double = 0
        <DataMember()>
        Public Property BookFeesPendingVariable() As Double
            Get
                Return _BookFeesPendingVariable
            End Get
            Set(ByVal value As Double)
                _BookFeesPendingVariable = value
            End Set
        End Property

        Private _BookFeesPendingAccessorialCode As Integer = 0
        <DataMember()>
        Public Property BookFeesPendingAccessorialCode() As Integer
            Get
                Return _BookFeesPendingAccessorialCode
            End Get
            Set(ByVal value As Integer)
                _BookFeesPendingAccessorialCode = value
            End Set
        End Property

        Private _BookFeesPendingAccessorialFeeTypeControl As Integer = 0
        <DataMember()>
        Public Property BookFeesPendingAccessorialFeeTypeControl() As Integer
            Get
                Return _BookFeesPendingAccessorialFeeTypeControl
            End Get
            Set(ByVal value As Integer)
                _BookFeesPendingAccessorialFeeTypeControl = value
            End Set
        End Property

        Private _BookFeesPendingOverRidden As Boolean = False
        <DataMember()>
        Public Property BookFeesPendingOverRidden() As Boolean
            Get
                Return _BookFeesPendingOverRidden
            End Get
            Set(ByVal value As Boolean)
                _BookFeesPendingOverRidden = value
            End Set
        End Property

        Private _BookFeesPendingVariableCode As Integer = 0
        <DataMember()>
        Public Property BookFeesPendingVariableCode() As Integer
            Get
                Return _BookFeesPendingVariableCode
            End Get
            Set(ByVal value As Integer)
                _BookFeesPendingVariableCode = value
            End Set
        End Property

        Private _BookFeesPendingVisible As Boolean = True
        <DataMember()>
        Public Property BookFeesPendingVisible() As Boolean
            Get
                Return _BookFeesPendingVisible
            End Get
            Set(ByVal value As Boolean)
                _BookFeesPendingVisible = value
            End Set
        End Property

        Private _BookFeesPendingAutoApprove As Boolean = False
        <DataMember()>
        Public Property BookFeesPendingAutoApprove() As Boolean
            Get
                Return _BookFeesPendingAutoApprove
            End Get
            Set(ByVal value As Boolean)
                _BookFeesPendingAutoApprove = value
            End Set
        End Property

        Private _BookFeesPendingAllowCarrierUpdates As Boolean = False
        <DataMember()>
        Public Property BookFeesPendingAllowCarrierUpdates() As Boolean
            Get
                Return _BookFeesPendingAllowCarrierUpdates
            End Get
            Set(ByVal value As Boolean)
                _BookFeesPendingAllowCarrierUpdates = value
            End Set
        End Property

        Private _BookFeesPendingCaption As String = ""
        <DataMember()>
        Public Property BookFeesPendingCaption() As String
            Get
                Return Left(_BookFeesPendingCaption, 50)
            End Get
            Set(ByVal value As String)
                _BookFeesPendingCaption = Left(value, 50)
            End Set
        End Property

        Private _BookFeesPendingEDICode As String = ""
        <DataMember()>
        Public Property BookFeesPendingEDICode() As String
            Get
                Return Left(_BookFeesPendingEDICode, 20)
            End Get
            Set(ByVal value As String)
                _BookFeesPendingEDICode = Left(value, 20)
            End Set
        End Property

        Private _BookFeesPendingTaxable As Boolean = True
        <DataMember()>
        Public Property BookFeesPendingTaxable() As Boolean
            Get
                Return _BookFeesPendingTaxable
            End Get
            Set(ByVal value As Boolean)
                _BookFeesPendingTaxable = value
            End Set
        End Property

        Private _BookFeesPendingIsTax As Boolean = False
        <DataMember()>
        Public Property BookFeesPendingIsTax() As Boolean
            Get
                Return _BookFeesPendingIsTax
            End Get
            Set(ByVal value As Boolean)
                _BookFeesPendingIsTax = value
            End Set
        End Property

        Private _BookFeesPendingTaxSortOrder As Integer = 0
        <DataMember()>
        Public Property BookFeesPendingTaxSortOrder() As Integer
            Get
                Return _BookFeesPendingTaxSortOrder
            End Get
            Set(ByVal value As Integer)
                _BookFeesPendingTaxSortOrder = value
            End Set
        End Property

        Private _BookFeesPendingBOLText As String = ""
        <DataMember()>
        Public Property BookFeesPendingBOLText() As String
            Get
                Return Left(_BookFeesPendingBOLText, 4000)
            End Get
            Set(ByVal value As String)
                _BookFeesPendingBOLText = Left(value, 4000)
            End Set
        End Property

        Private _BookFeesPendingBOLPlacement As String = ""
        <DataMember()>
        Public Property BookFeesPendingBOLPlacement() As String
            Get
                Return Left(_BookFeesPendingBOLPlacement, 100)
            End Get
            Set(ByVal value As String)
                _BookFeesPendingBOLPlacement = Left(value, 100)
            End Set
        End Property

        Private _BookFeesPendingAccessorialFeeAllocationTypeControl As Integer = 1
        <DataMember()>
        Public Property BookFeesPendingAccessorialFeeAllocationTypeControl() As Integer
            Get
                Return _BookFeesPendingAccessorialFeeAllocationTypeControl
            End Get
            Set(ByVal value As Integer)
                _BookFeesPendingAccessorialFeeAllocationTypeControl = value
            End Set
        End Property

        Private _BookFeesPendingTarBracketTypeControl As Integer = 4
        <DataMember()>
        Public Property BookFeesPendingTarBracketTypeControl() As Integer
            Get
                Return _BookFeesPendingTarBracketTypeControl
            End Get
            Set(ByVal value As Integer)
                _BookFeesPendingTarBracketTypeControl = value
            End Set
        End Property

        Private _BookFeesPendingAccessorialFeeCalcTypeControl As Integer = 1
        <DataMember()>
        Public Property BookFeesPendingAccessorialFeeCalcTypeControl() As Integer
            Get
                Return _BookFeesPendingAccessorialFeeCalcTypeControl
            End Get
            Set(ByVal value As Integer)
                _BookFeesPendingAccessorialFeeCalcTypeControl = value
            End Set
        End Property

        Private _BookFeesPendingModDate As System.Nullable(Of Date)
        <DataMember()>
        Public Property BookFeesPendingModDate() As System.Nullable(Of Date)
            Get
                Return _BookFeesPendingModDate
            End Get
            Set(ByVal value As System.Nullable(Of Date))
                _BookFeesPendingModDate = value
            End Set
        End Property

        Private _BookFeesPendingModUser As String = ""
        <DataMember()>
        Public Property BookFeesPendingModUser() As String
            Get
                Return Left(_BookFeesPendingModUser, 100)
            End Get
            Set(ByVal value As String)
                _BookFeesPendingModUser = Left(value, 100)
            End Set
        End Property

        Private _BookFeesPendingUpdated As Byte()
        <DataMember()>
        Public Property BookFeesPendingUpdated() As Byte()
            Get
                Return _BookFeesPendingUpdated
            End Get
            Set(ByVal value As Byte())
                _BookFeesPendingUpdated = value
            End Set
        End Property


        'Added By LVV on 9/28/2017 for v-8.0 TMS365
        Private _BookFeesPendingApproved As Boolean = False
        <DataMember()>
        Public Property BookFeesPendingApproved() As Boolean
            Get
                Return _BookFeesPendingApproved
            End Get
            Set(ByVal value As Boolean)
                _BookFeesPendingApproved = value
            End Set
        End Property

        Private _BookFeesPendingApprovedDate As System.Nullable(Of Date)
        <DataMember()>
        Public Property BookFeesPendingApprovedDate() As System.Nullable(Of Date)
            Get
                Return _BookFeesPendingApprovedDate
            End Get
            Set(ByVal value As System.Nullable(Of Date))
                _BookFeesPendingApprovedDate = value
            End Set
        End Property

        Private _BookFeesPendingApprovedBy As String = ""
        <DataMember()>
        Public Property BookFeesPendingApprovedBy() As String
            Get
                Return Left(_BookFeesPendingApprovedBy, 100)
            End Get
            Set(ByVal value As String)
                _BookFeesPendingApprovedBy = Left(value, 100)
            End Set
        End Property

        Private _BookFeesPendingMessage As String = ""
        <DataMember()>
        Public Property BookFeesPendingMessage() As String
            Get
                Return Left(_BookFeesPendingMessage, 4000)
            End Get
            Set(ByVal value As String)
                _BookFeesPendingMessage = Left(value, 4000)
            End Set
        End Property

        Private _BookFeesPendingMissingFee As Boolean = False
        ''' <summary>
        ''' Flag to identify pending fees created by TMS Users when a carrier fails to send an expected fee electronically the default is false.
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks>
        '''  Modified by RHR for v-8.2 on 7/3/2019
        '''  added New field "BookFeesPendingMissingFee" to support tracking of missing expected carrier fees
        '''  the default Is false.  
        ''' </remarks>
        <DataMember()>
        Public Property BookFeesPendingMissingFee() As Boolean
            Get
                Return _BookFeesPendingMissingFee
            End Get
            Set(ByVal value As Boolean)
                _BookFeesPendingMissingFee = value
            End Set
        End Property

#End Region

#Region " Public Methods"
        Public Overrides Function Clone() As DTOBaseClass
            Dim instance As New BookFeePending
            instance = DirectCast(MemberwiseClone(), BookFeePending)
            Return instance
        End Function

#End Region

    End Class
End Namespace


