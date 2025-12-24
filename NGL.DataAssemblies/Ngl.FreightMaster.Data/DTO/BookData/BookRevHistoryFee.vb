Imports Microsoft.VisualBasic
Imports System
Imports System.Collections.Generic
Imports System.Linq
Imports System.Text
Imports System.Runtime.Serialization
Imports Ngl.Core.ChangeTracker

Namespace DataTransferObjects
    ''' <summary>
    ''' 
    ''' </summary>
    ''' <remarks>
    ''' Modified by RHR for v-8.2 on 7/3/2019
    '''  added references to the following missing properties/fields
    '''  BookRevHistFeesAccessorialOverRideReasonControl
    '''  BookRevHistFeesAccessorialDependencyTypeControl
    '''  BookRevHistFeesDependencyKey
    '''  BookRevHistFeesMissingFee to support tracking of missing expected carrier fees the default Is false.  
    ''' </remarks>
    <DataContract()>
    Public Class BookRevHistoryFee
        Inherits DTOBaseClass


#Region " Data Members"
        Private _BookRevHistFeesControl As Integer = 0
        <DataMember()>
        Public Property BookRevHistFeesControl() As Integer
            Get
                Return _BookRevHistFeesControl
            End Get
            Set(ByVal value As Integer)
                _BookRevHistFeesControl = value
            End Set
        End Property

        Private _BookRevHistFeesBookRevHistControl As Integer = 0
        <DataMember()>
        Public Property BookRevHistFeesBookRevHistControl() As Integer
            Get
                Return _BookRevHistFeesBookRevHistControl
            End Get
            Set(ByVal value As Integer)
                _BookRevHistFeesBookRevHistControl = value
            End Set
        End Property

        Private _BookRevHistFeesMinimum As Decimal = 0
        <DataMember()>
        Public Property BookRevHistFeesMinimum() As Decimal
            Get
                Return _BookRevHistFeesMinimum
            End Get
            Set(ByVal value As Decimal)
                _BookRevHistFeesMinimum = value
            End Set
        End Property

        Private _BookRevHistFeesValue As Decimal = 0
        <DataMember()>
        Public Property BookRevHistFeesValue() As Decimal
            Get
                Return _BookRevHistFeesValue
            End Get
            Set(ByVal value As Decimal)
                _BookRevHistFeesValue = value
            End Set
        End Property

        Private _BookRevHistFeesVariable As Double = 0
        <DataMember()>
        Public Property BookRevHistFeesVariable() As Double
            Get
                Return _BookRevHistFeesVariable
            End Get
            Set(ByVal value As Double)
                _BookRevHistFeesVariable = value
            End Set
        End Property

        Private _BookRevHistFeesAccessorialCode As Integer = 0
        <DataMember()>
        Public Property BookRevHistFeesAccessorialCode() As Integer
            Get
                Return _BookRevHistFeesAccessorialCode
            End Get
            Set(ByVal value As Integer)
                _BookRevHistFeesAccessorialCode = value
            End Set
        End Property

        Private _BookRevHistFeesAccessorialFeeTypeControl As Integer = 0
        <DataMember()>
        Public Property BookRevHistFeesAccessorialFeeTypeControl() As Integer
            Get
                Return _BookRevHistFeesAccessorialFeeTypeControl
            End Get
            Set(ByVal value As Integer)
                _BookRevHistFeesAccessorialFeeTypeControl = value
            End Set
        End Property

        Private _BookRevHistFeesOverRidden As Boolean = False
        <DataMember()>
        Public Property BookRevHistFeesOverRidden() As Boolean
            Get
                Return _BookRevHistFeesOverRidden
            End Get
            Set(ByVal value As Boolean)
                _BookRevHistFeesOverRidden = value
            End Set
        End Property

        Private _BookRevHistFeesVariableCode As Integer = 0
        <DataMember()>
        Public Property BookRevHistFeesVariableCode() As Integer
            Get
                Return _BookRevHistFeesVariableCode
            End Get
            Set(ByVal value As Integer)
                _BookRevHistFeesVariableCode = value
            End Set
        End Property

        Private _BookRevHistFeesVisible As Boolean = True
        <DataMember()>
        Public Property BookRevHistFeesVisible() As Boolean
            Get
                Return _BookRevHistFeesVisible
            End Get
            Set(ByVal value As Boolean)
                _BookRevHistFeesVisible = value
            End Set
        End Property

        Private _BookRevHistFeesAutoApprove As Boolean = False
        <DataMember()>
        Public Property BookRevHistFeesAutoApprove() As Boolean
            Get
                Return _BookRevHistFeesAutoApprove
            End Get
            Set(ByVal value As Boolean)
                _BookRevHistFeesAutoApprove = value
            End Set
        End Property

        Private _BookRevHistFeesAllowCarrierUpdates As Boolean = False
        <DataMember()>
        Public Property BookRevHistFeesAllowCarrierUpdates() As Boolean
            Get
                Return _BookRevHistFeesAllowCarrierUpdates
            End Get
            Set(ByVal value As Boolean)
                _BookRevHistFeesAllowCarrierUpdates = value
            End Set
        End Property

        Private _BookRevHistFeesCaption As String = ""
        <DataMember()>
        Public Property BookRevHistFeesCaption() As String
            Get
                Return Left(_BookRevHistFeesCaption, 50)
            End Get
            Set(ByVal value As String)
                _BookRevHistFeesCaption = Left(value, 50)
            End Set
        End Property

        Private _BookRevHistFeesEDICode As String = ""
        <DataMember()>
        Public Property BookRevHistFeesEDICode() As String
            Get
                Return Left(_BookRevHistFeesEDICode, 20)
            End Get
            Set(ByVal value As String)
                _BookRevHistFeesEDICode = Left(value, 20)
            End Set
        End Property

        Private _BookRevHistFeesTaxable As Boolean = True
        <DataMember()>
        Public Property BookRevHistFeesTaxable() As Boolean
            Get
                Return _BookRevHistFeesTaxable
            End Get
            Set(ByVal value As Boolean)
                _BookRevHistFeesTaxable = value
            End Set
        End Property

        Private _BookRevHistFeesIsTax As Boolean = False
        <DataMember()>
        Public Property BookRevHistFeesIsTax() As Boolean
            Get
                Return _BookRevHistFeesIsTax
            End Get
            Set(ByVal value As Boolean)
                _BookRevHistFeesIsTax = value
            End Set
        End Property

        Private _BookRevHistFeesTaxSortOrder As Integer = 0
        <DataMember()>
        Public Property BookRevHistFeesTaxSortOrder() As Integer
            Get
                Return _BookRevHistFeesTaxSortOrder
            End Get
            Set(ByVal value As Integer)
                _BookRevHistFeesTaxSortOrder = value
            End Set
        End Property

        Private _BookRevHistFeesBOLText As String = ""
        <DataMember()>
        Public Property BookRevHistFeesBOLText() As String
            Get
                Return Left(_BookRevHistFeesBOLText, 4000)
            End Get
            Set(ByVal value As String)
                _BookRevHistFeesBOLText = Left(value, 4000)
            End Set
        End Property

        Private _BookRevHistFeesBOLPlacement As String = ""
        <DataMember()>
        Public Property BookRevHistFeesBOLPlacement() As String
            Get
                Return Left(_BookRevHistFeesBOLPlacement, 100)
            End Get
            Set(ByVal value As String)
                _BookRevHistFeesBOLPlacement = Left(value, 100)
            End Set
        End Property

        Private _BookRevHistFeesAccessorialFeeAllocationTypeControl As Integer = 1
        <DataMember()>
        Public Property BookRevHistFeesAccessorialFeeAllocationTypeControl() As Integer
            Get
                Return _BookRevHistFeesAccessorialFeeAllocationTypeControl
            End Get
            Set(ByVal value As Integer)
                _BookRevHistFeesAccessorialFeeAllocationTypeControl = value
            End Set
        End Property

        Private _BookRevHistFeesTarBracketTypeControl As Integer = 4
        <DataMember()>
        Public Property BookRevHistFeesTarBracketTypeControl() As Integer
            Get
                Return _BookRevHistFeesTarBracketTypeControl
            End Get
            Set(ByVal value As Integer)
                _BookRevHistFeesTarBracketTypeControl = value
            End Set
        End Property

        Private _BookRevHistFeesAccessorialFeeCalcTypeControl As Integer = 1
        <DataMember()>
        Public Property BookRevHistFeesAccessorialFeeCalcTypeControl() As Integer
            Get
                Return _BookRevHistFeesAccessorialFeeCalcTypeControl
            End Get
            Set(ByVal value As Integer)
                _BookRevHistFeesAccessorialFeeCalcTypeControl = value
            End Set
        End Property

        Private _BookRevHistFeesModDate As System.Nullable(Of Date)
        <DataMember()>
        Public Property BookRevHistFeesModDate() As System.Nullable(Of Date)
            Get
                Return _BookRevHistFeesModDate
            End Get
            Set(ByVal value As System.Nullable(Of Date))
                _BookRevHistFeesModDate = value
            End Set
        End Property

        Private _BookRevHistFeesModUser As String = ""
        <DataMember()>
        Public Property BookRevHistFeesModUser() As String
            Get
                Return Left(_BookRevHistFeesModUser, 100)
            End Get
            Set(ByVal value As String)
                _BookRevHistFeesModUser = Left(value, 100)
            End Set
        End Property

        Private _BookRevHistFeesUpdated As Byte()
        <DataMember()>
        Public Property BookRevHistFeesUpdated() As Byte()
            Get
                Return _BookRevHistFeesUpdated
            End Get
            Set(ByVal value As Byte())
                _BookRevHistFeesUpdated = value
            End Set
        End Property

        Private _BookRevHistFeesAccessorialOverRideReasonControl As Integer = 0
        ''' <summary>
        ''' 
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks>
        '''  Modified by RHR for v-8.2 on 7/3/2019
        '''  added missing property "BookRevHistFeesAccessorialDependencyTypeControl"  
        ''' </remarks>
        <DataMember()>
        Public Property BookRevHistFeesAccessorialOverRideReasonControl() As Integer
            Get
                Return _BookRevHistFeesAccessorialOverRideReasonControl
            End Get
            Set(ByVal value As Integer)
                _BookRevHistFeesAccessorialOverRideReasonControl = value
            End Set
        End Property

        Private _BookRevHistFeesAccessorialDependencyTypeControl As Integer = 0
        ''' <summary>
        ''' 
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks>
        '''  Modified by RHR for v-8.2 on 7/3/2019
        '''  added missing property "BookRevHistFeesAccessorialDependencyTypeControl"  
        ''' </remarks>
        <DataMember()>
        Public Property BookRevHistFeesAccessorialDependencyTypeControl() As Integer
            Get
                Return _BookRevHistFeesAccessorialDependencyTypeControl
            End Get
            Set(ByVal value As Integer)
                _BookRevHistFeesAccessorialDependencyTypeControl = value
            End Set
        End Property

        Private _BookRevHistFeesDependencyKey As String = ""
        ''' <summary>
        ''' 
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks>
        '''  Modified by RHR for v-8.2 on 7/3/2019
        '''  added missing property "BookRevHistFeesDependencyKey"   
        ''' </remarks>
        <DataMember()>
        Public Property BookRevHistFeesDependencyKey() As String
            Get
                Return Left(_BookRevHistFeesDependencyKey, 100)
            End Get
            Set(ByVal value As String)
                _BookRevHistFeesDependencyKey = Left(value, 100)
            End Set
        End Property


        Private _BookRevHistFeesMissingFee As Boolean = False
        ''' <summary>
        ''' Flag to identify pending fees created by TMS Users when a carrier fails to send an expected fee electronically the default is false.
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks>
        '''  Modified by RHR for v-8.2 on 7/3/2019
        '''  added New field "BookRevHistFeesMissingFee" to support tracking of missing expected carrier fees
        '''  the default Is false.  
        ''' </remarks>
        <DataMember()>
        Public Property BookRevHistFeesMissingFee() As Boolean
            Get
                Return _BookRevHistFeesMissingFee
            End Get
            Set(ByVal value As Boolean)
                _BookRevHistFeesMissingFee = value
            End Set
        End Property

#End Region

#Region " Public Methods"
        Public Overrides Function Clone() As DTOBaseClass
            Dim instance As New BookRevHistoryFee
            instance = DirectCast(MemberwiseClone(), BookRevHistoryFee)
            Return instance
        End Function

#End Region

    End Class
End Namespace
