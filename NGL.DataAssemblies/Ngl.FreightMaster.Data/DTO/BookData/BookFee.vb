Imports Microsoft.VisualBasic
Imports System
Imports System.Collections.Generic
Imports System.Linq
Imports System.Text
Imports System.Runtime.Serialization
Imports Ngl.Core.ChangeTracker
Imports SerilogTracing

Namespace DataTransferObjects
    ''' <summary>
    ''' An Accessorial Fee record assigned to a specific order
    ''' </summary>
    ''' <remarks>
    ''' Modified by RHR for v-8.2 on 7/3/2019
    ''' Added BookFeesMissingFee a flag used to identify fees created by 
    ''' TMS Users when a carrier fails to send an expected fee electronically the default is false.
    ''' This value us updated as part of the fee approval processess all missing fees require manual
    ''' approval before the AP Audit Procedure will return true
    ''' </remarks> 
    <DataContract()>
    Public Class BookFee
        Inherits DTOBaseClass

        Public Sub New()
            MyBase.New()
            Logger = Logger.ForContext(Of BookFee)()
        End Sub

#Region " Data Members"
        Private _BookFeesControl As Integer = 0
        <DataMember()>
        Public Property BookFeesControl() As Integer
            Get
                Return _BookFeesControl
            End Get
            Set(ByVal value As Integer)
                _BookFeesControl = value
            End Set
        End Property

        Private _BookFeesBookControl As Integer = 0
        <DataMember()>
        Public Property BookFeesBookControl() As Integer
            Get
                Return _BookFeesBookControl
            End Get
            Set(ByVal value As Integer)
                _BookFeesBookControl = value
            End Set
        End Property

        Private _BookFeesMinimum As Decimal = 0
        <DataMember()>
        Public Property BookFeesMinimum() As Decimal
            Get
                Return _BookFeesMinimum
            End Get
            Set(ByVal value As Decimal)
                _BookFeesMinimum = value
            End Set
        End Property

        Private _BookFeesValue As Decimal = 0
        <DataMember()>
        Public Property BookFeesValue() As Decimal
            Get
                Return _BookFeesValue
            End Get
            Set(ByVal value As Decimal)
                Logger.Information("{BookFee} - Setting BookFeesValue to {BookFeesValue} from {PreviousBookFeesValue}",me, value, _BookFeesValue)
                _BookFeesValue = value
            End Set
        End Property

        Private _BookFeesVariable As Double = 0
        <DataMember()>
        Public Property BookFeesVariable() As Double
            Get
                Return _BookFeesVariable
            End Get
            Set(ByVal value As Double)
                Logger.Information("{BookFee} - Setting BookFeesVariable to {BookFeesVariable} from {PreviousBookFeesVariable}", value, _BookFeesVariable)
                _BookFeesVariable = value
            End Set
        End Property

        Private _BookFeesAccessorialCode As Integer = 0
        <DataMember()>
        Public Property BookFeesAccessorialCode() As Integer
            Get
                Return _BookFeesAccessorialCode
            End Get
            Set(ByVal value As Integer)
                _BookFeesAccessorialCode = value
            End Set
        End Property

        Private _BookFeesAccessorialFeeTypeControl As Integer = 0
        <DataMember()>
        Public Property BookFeesAccessorialFeeTypeControl() As Integer
            Get
                Return _BookFeesAccessorialFeeTypeControl
            End Get
            Set(ByVal value As Integer)
                Logger.Information("{BookFee} - Setting BookFeesAccessorialFeeTypeControl to {BookFeesAccessorialFeeTypeControl} from {PreviousBookFeesAccessorialFeeTypeControl}",Me, value, _BookFeesAccessorialFeeTypeControl)
                _BookFeesAccessorialFeeTypeControl = value
            End Set
        End Property

        Private _BookFeesOverRidden As Boolean = False
        <DataMember()>
        Public Property BookFeesOverRidden() As Boolean
            Get
                Return _BookFeesOverRidden
            End Get
            Set(ByVal value As Boolean)
                _BookFeesOverRidden = value
            End Set
        End Property

        Private _BookFeesVariableCode As Integer = 0
        <DataMember()>
        Public Property BookFeesVariableCode() As Integer
            Get
                Return _BookFeesVariableCode
            End Get
            Set(ByVal value As Integer)
                Logger.Information("{BookFee} - Setting BookFeesVariableCode to {BookFeesVariableCode} from {PreviousBookFeesVariableCode}", Me,value, _BookFeesVariable)
                _BookFeesVariableCode = value
                
            End Set
        End Property

        Private _BookFeesVisible As Boolean = True
        <DataMember()>
        Public Property BookFeesVisible() As Boolean
            Get
                Return _BookFeesVisible
            End Get
            Set(ByVal value As Boolean)
                _BookFeesVisible = value
            End Set
        End Property

        Private _BookFeesAutoApprove As Boolean = False
        <DataMember()>
        Public Property BookFeesAutoApprove() As Boolean
            Get
                Return _BookFeesAutoApprove
            End Get
            Set(ByVal value As Boolean)
                _BookFeesAutoApprove = value
            End Set
        End Property

        Private _BookFeesAllowCarrierUpdates As Boolean = False
        <DataMember()>
        Public Property BookFeesAllowCarrierUpdates() As Boolean
            Get
                Return _BookFeesAllowCarrierUpdates
            End Get
            Set(ByVal value As Boolean)
                _BookFeesAllowCarrierUpdates = value
            End Set
        End Property

        Private _BookFeesCaption As String = ""
        <DataMember()>
        Public Property BookFeesCaption() As String
            Get
                Return Left(_BookFeesCaption, 50)
            End Get
            Set(ByVal value As String)
                _BookFeesCaption = Left(value, 50)
            End Set
        End Property

        Private _BookFeesEDICode As String = ""
        <DataMember()>
        Public Property BookFeesEDICode() As String
            Get
                Return Left(_BookFeesEDICode, 20)
            End Get
            Set(ByVal value As String)
                _BookFeesEDICode = Left(value, 20)
            End Set
        End Property

        Private _BookFeesTaxable As Boolean = True
        <DataMember()>
        Public Property BookFeesTaxable() As Boolean
            Get
                Return _BookFeesTaxable
            End Get
            Set(ByVal value As Boolean)
                _BookFeesTaxable = value
            End Set
        End Property

        Private _BookFeesIsTax As Boolean = False
        <DataMember()>
        Public Property BookFeesIsTax() As Boolean
            Get
                Return _BookFeesIsTax
            End Get
            Set(ByVal value As Boolean)
                _BookFeesIsTax = value
            End Set
        End Property

        Private _BookFeesTaxSortOrder As Integer = 0
        <DataMember()>
        Public Property BookFeesTaxSortOrder() As Integer
            Get
                Return _BookFeesTaxSortOrder
            End Get
            Set(ByVal value As Integer)
                _BookFeesTaxSortOrder = value
            End Set
        End Property

        Private _BookFeesBOLText As String = ""
        <DataMember()>
        Public Property BookFeesBOLText() As String
            Get
                Return Left(_BookFeesBOLText, 4000)
            End Get
            Set(ByVal value As String)
                _BookFeesBOLText = Left(value, 4000)
            End Set
        End Property

        Private _BookFeesBOLPlacement As String = ""
        <DataMember()>
        Public Property BookFeesBOLPlacement() As String
            Get
                Return Left(_BookFeesBOLPlacement, 100)
            End Get
            Set(ByVal value As String)
                _BookFeesBOLPlacement = Left(value, 100)
            End Set
        End Property

        Private _BookFeesAccessorialFeeAllocationTypeControl As Integer = 1
        <DataMember()>
        Public Property BookFeesAccessorialFeeAllocationTypeControl() As Integer
            Get
                Return _BookFeesAccessorialFeeAllocationTypeControl
            End Get
            Set(ByVal value As Integer)
                Logger.Information("{BookFee} - Setting BookFeesAccessorialFeeAllocationTypeControl to {BookFeesAccessorialFeeAllocationTypeControl} from {PreviousBookFeesAccessorialFeeAllocationTypeControl}",Me, value, _BookFeesAccessorialFeeAllocationTypeControl)
                _BookFeesAccessorialFeeAllocationTypeControl = value
            End Set
        End Property

        Private _BookFeesTarBracketTypeControl As Integer = 4
        <DataMember()>
        Public Property BookFeesTarBracketTypeControl() As Integer
            Get
                Return _BookFeesTarBracketTypeControl
            End Get
            Set(ByVal value As Integer)
                Logger.Information("{BookFee} - Setting BookFeesTarBracketTypeControl to {BookFeesTarBracketTypeControl} from {PreviousBookFeesTarBracketTypeControl}",Me, value, _BookFeesTarBracketTypeControl)
                _BookFeesTarBracketTypeControl = value
            End Set
        End Property

        Private _BookFeesAccessorialFeeCalcTypeControl As Integer = 1
        <DataMember()>
        Public Property BookFeesAccessorialFeeCalcTypeControl() As Integer
            Get
                Return _BookFeesAccessorialFeeCalcTypeControl
            End Get
            Set(ByVal value As Integer)
                Logger.Information("{BookFee} - Setting BookFeesAccessorialFeeCalcTypeControl to {BookFeesAccessorialFeeCalcTypeControl} from {PreviousBookFeesAccessorialFeeCalcTypeControl}",Me, value, _BookFeesAccessorialFeeCalcTypeControl)
                _BookFeesAccessorialFeeCalcTypeControl = value
            End Set
        End Property

        Private _BookFeesAccessorialOverRideReasonControl As Integer = 0
        <DataMember()>
        Public Property BookFeesAccessorialOverRideReasonControl() As Integer
            Get
                Return _BookFeesAccessorialOverRideReasonControl
            End Get
            Set(ByVal value As Integer)
                _BookFeesAccessorialOverRideReasonControl = value
            End Set
        End Property

        Private _BookFeesAccessorialDependencyTypeControl As Integer = 0
        <DataMember()>
        Public Property BookFeesAccessorialDependencyTypeControl() As Integer
            Get
                Return _BookFeesAccessorialDependencyTypeControl
            End Get
            Set(ByVal value As Integer)
                _BookFeesAccessorialDependencyTypeControl = value
            End Set
        End Property

        Private _BookFeesDependencyKey As String = ""
        <DataMember()>
        Public Property BookFeesDependencyKey() As String
            Get
                Return Left(_BookFeesDependencyKey, 100)
            End Get
            Set(ByVal value As String)
                _BookFeesDependencyKey = Left(value, 100)
            End Set
        End Property

        Private _BookFeesModDate As System.Nullable(Of Date)
        <DataMember()>
        Public Property BookFeesModDate() As System.Nullable(Of Date)
            Get
                Return _BookFeesModDate
            End Get
            Set(ByVal value As System.Nullable(Of Date))
                _BookFeesModDate = value
            End Set
        End Property

        Private _BookFeesModUser As String = ""
        <DataMember()>
        Public Property BookFeesModUser() As String
            Get
                Return Left(_BookFeesModUser, 100)
            End Get
            Set(ByVal value As String)
                _BookFeesModUser = Left(value, 100)
            End Set
        End Property

        Private _BookFeesUpdated As Byte()
        <DataMember()>
        Public Property BookFeesUpdated() As Byte()
            Get
                Return _BookFeesUpdated
            End Get
            Set(ByVal value As Byte())
                _BookFeesUpdated = value
            End Set
        End Property

        Private _BookFeesAccessorialProfileSpecific As Boolean = False
        <DataMember()>
        Public Property BookFeesAccessorialProfileSpecific() As Boolean
            Get
                Return _BookFeesAccessorialProfileSpecific
            End Get
            Set(ByVal value As Boolean)
                _BookFeesAccessorialProfileSpecific = value
            End Set
        End Property

        Private _NotSupported As Boolean = False
        <DataMember()>
        Public Property NotSupported() As Boolean
            Get
                Return _NotSupported
            End Get
            Set(ByVal value As Boolean)
                _NotSupported = value
            End Set
        End Property
        'Modified by RHR 2/2/2016 v-7.0.4.1 
        Private _AllowOverwrite As Boolean = False
        Public Property AllowOverwrite() As Boolean
            Get
                Return _AllowOverwrite
            End Get
            Set(ByVal value As Boolean)
                _AllowOverwrite = value
            End Set
        End Property

        Private _BookFeesMissingFee As Boolean = False
        ''' <summary>
        ''' Flag to identify pending fees created by TMS Users when a carrier fails to send an expected fee electronically the default is false.
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks>
        '''  Modified by RHR for v-8.2 on 7/3/2019
        '''  added New field "BookFeesMissingFee" to support tracking of missing expected carrier fees
        '''  the default Is false.  
        ''' </remarks>
        <DataMember()>
        Public Property BookFeesMissingFee() As Boolean
            Get
                Return _BookFeesMissingFee
            End Get
            Set(ByVal value As Boolean)
                _BookFeesMissingFee = value
            End Set
        End Property


#End Region

#Region " Public Methods"
        Public Overrides Function Clone() As DTOBaseClass
            Dim instance As New BookFee
            Using Logger.StartActivity("BookFee.Clone")
                instance = DirectCast(MemberwiseClone(), BookFee)
            End Using

            Return instance
        End Function


        Public Overrides Function ToString() As String
            Return $"[{Me.BookFeesAccessorialCode}] {Me.BookFeesCaption} - {Me.BookFeesValue}, FeeType:{Me.BookFeesAccessorialFeeTypeControl}, DependencyType:{Me.BookFeesAccessorialDependencyTypeControl}, AllocationType: {Me.BookFeesAccessorialFeeAllocationTypeControl}, TariffBracketType: {Me.BookFeesTarBracketTypeControl}, Var/Code:{Me.BookFeesVariable}/{Me.BookFeesVariableCode}"
        End Function

#End Region

    End Class
End Namespace

