Imports Microsoft.VisualBasic
Imports System
Imports System.Collections.Generic
Imports System.Linq
Imports System.Text
Imports System.Runtime.Serialization
Imports Ngl.Core.ChangeTracker

Namespace DataTransferObjects
    <DataContract()> _
    Public Class BookChanges
        Inherits DTOBaseClass


#Region " Data Members"
         
        Private _HasChangedBookDateLoad As Boolean
        <DataMember()> _
        Public Property HasChangedBookDateLoad() As Boolean
            Get
                Return _HasChangedBookDateLoad
            End Get
            Set(ByVal value As Boolean)
                _HasChangedBookDateLoad = value
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

        Private _HasChangedBookTotalCases As Boolean
        <DataMember()> _
        Public Property HasChangedBookTotalCases() As Boolean
            Get
                Return _HasChangedBookTotalCases
            End Get
            Set(ByVal value As Boolean)
                _HasChangedBookTotalCases = value
            End Set
        End Property

        Private _BookTotalCases As Integer = 0
        <DataMember()> _
        Public Property BookTotalCases() As Integer
            Get
                Return _BookTotalCases
            End Get
            Set(ByVal value As Integer)
                _BookTotalCases = value
            End Set
        End Property

        Private _HasChangedBookTotalWgt As Boolean
        <DataMember()> _
        Public Property HasChangedBookTotalWgt() As Boolean
            Get
                Return _HasChangedBookTotalWgt
            End Get
            Set(ByVal value As Boolean)
                _HasChangedBookTotalWgt = value
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

        Private _HasChangedBookTotalPL As Boolean
        <DataMember()> _
        Public Property HasChangedBookTotalPL() As Boolean
            Get
                Return _HasChangedBookTotalPL
            End Get
            Set(ByVal value As Boolean)
                _HasChangedBookTotalPL = value
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

        'Private _BookTotalItems As Integer = 0
        '<DataMember()> _
        'Public Property BookTotalItems() As Integer
        '    Get
        '        Return _BookTotalItems
        '    End Get
        '    Set(ByVal value As Integer)
        '        _BookTotalItems = value
        '    End Set
        'End Property

        Private _HasChangedBookDateOrdered As Boolean
        <DataMember()> _
        Public Property HasChangedBookDateOrdered() As Boolean
            Get
                Return _HasChangedBookDateOrdered
            End Get
            Set(ByVal value As Boolean)
                _HasChangedBookDateOrdered = value
            End Set
        End Property

        Private _BookDateOrdered As System.Nullable(Of Date)
        <DataMember()> _
        Public Property BookDateOrdered() As System.Nullable(Of Date)
            Get
                Return _BookDateOrdered
            End Get
            Set(ByVal value As System.Nullable(Of Date))
                _BookDateOrdered = value
            End Set
        End Property

        Private _HasChangedCompanyNumber As Boolean
        <DataMember()> _
        Public Property HasChangedCompanyNumber() As Boolean
            Get
                Return _HasChangedCompanyNumber
            End Get
            Set(ByVal value As Boolean)
                _HasChangedCompanyNumber = value
            End Set
        End Property

        Private _CompanyNumber As String = ""
        <DataMember()> _
        Public Property CompanyNumber() As String
            Get
                Return _CompanyNumber
            End Get
            Set(ByVal value As String)
                _CompanyNumber = value
            End Set
        End Property

        Private _HasChangedBookDateRequired As Boolean
        <DataMember()> _
        Public Property HasChangedBookDateRequired() As Boolean
            Get
                Return _HasChangedBookDateRequired
            End Get
            Set(ByVal value As Boolean)
                _HasChangedBookDateRequired = value
            End Set
        End Property

        Private _BookDateRequired As System.Nullable(Of Date)
        <DataMember()> _
        Public Property BookDateRequired() As System.Nullable(Of Date)
            Get
                Return _BookDateRequired
            End Get
            Set(ByVal value As System.Nullable(Of Date))
                _BookDateRequired = value
            End Set
        End Property

        Private _HasChangedBookCarrOrderNumber As Boolean
        <DataMember()> _
        Public Property HasChangedBookCarrOrderNumber() As Boolean
            Get
                Return _HasChangedBookCarrOrderNumber
            End Get
            Set(ByVal value As Boolean)
                _HasChangedBookCarrOrderNumber = value
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

        Private _HasChangedBookTransType As Boolean
        <DataMember()> _
        Public Property HasChangedBookTransType() As Boolean
            Get
                Return _HasChangedBookTransType
            End Get
            Set(ByVal value As Boolean)
                _HasChangedBookTransType = value
            End Set
        End Property

        Private _BookTransType As String = ""
        <DataMember()> _
        Public Property BookTransType() As String
            Get
                Return Left(_BookTransType, 50)
            End Get
            Set(ByVal value As String)
                _BookTransType = Left(value, 50)
            End Set
        End Property

        Private _HasChangedBookRevTotalCost As Boolean
        <DataMember()> _
        Public Property HasChangedBookRevTotalCost() As Boolean
            Get
                Return _HasChangedBookRevTotalCost
            End Get
            Set(ByVal value As Boolean)
                _HasChangedBookRevTotalCost = value
            End Set
        End Property

        Private _BookRevTotalCost As Decimal = 0
        <DataMember()> _
        Public Property BookRevTotalCost() As Decimal
            Get
                Return _BookRevTotalCost
            End Get
            Set(ByVal value As Decimal)
                _BookRevTotalCost = value
            End Set
        End Property

        Private _HasChangedBookOrigCity As Boolean
        <DataMember()> _
        Public Property HasChangedBookOrigCity() As Boolean
            Get
                Return _HasChangedBookOrigCity
            End Get
            Set(ByVal value As Boolean)
                _HasChangedBookOrigCity = value
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

        Private _HasChangedBookOrigState As Boolean
        <DataMember()> _
        Public Property HasChangedBookOrigState() As Boolean
            Get
                Return _HasChangedBookOrigState
            End Get
            Set(ByVal value As Boolean)
                _HasChangedBookOrigState = value
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

        Private _HasChangedBookOrigCountry As Boolean
        <DataMember()> _
        Public Property HasChangedBookOrigCountry() As Boolean
            Get
                Return _HasChangedBookOrigCountry
            End Get
            Set(ByVal value As Boolean)
                _HasChangedBookOrigCountry = value
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

        Private _HasChangedBookOrigZip As Boolean
        <DataMember()> _
        Public Property HasChangedBookOrigZip() As Boolean
            Get
                Return _HasChangedBookOrigZip
            End Get
            Set(ByVal value As Boolean)
                _HasChangedBookOrigZip = value
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

        Private _HasChangedBookOrigAddress1 As Boolean
        <DataMember()> _
        Public Property HasChangedBookOrigAddress1() As Boolean
            Get
                Return _HasChangedBookOrigAddress1
            End Get
            Set(ByVal value As Boolean)
                _HasChangedBookOrigAddress1 = value
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

        Private _HasChangedBookDestCity As Boolean
        <DataMember()> _
        Public Property HasChangedBookDestCity() As Boolean
            Get
                Return _HasChangedBookDestCity
            End Get
            Set(ByVal value As Boolean)
                _HasChangedBookDestCity = value
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

        Private _HasChangedBookDestState As Boolean
        <DataMember()> _
        Public Property HasChangedBookDestState() As Boolean
            Get
                Return _HasChangedBookDestState
            End Get
            Set(ByVal value As Boolean)
                _HasChangedBookDestState = value
            End Set
        End Property

        Private _BookDestState As String = ""
        <DataMember()> _
        Public Property BookDestState() As String
            Get
                Return Left(_BookDestState, 8)
            End Get
            Set(ByVal value As String)
                _BookDestState = Left(value, 8)
            End Set
        End Property

        Private _HasChangedBookDestCountry As Boolean
        <DataMember()> _
        Public Property HasChangedBookDestCountry() As Boolean
            Get
                Return _HasChangedBookDestCountry
            End Get
            Set(ByVal value As Boolean)
                _HasChangedBookDestCountry = value
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

        Private _HasChangedBookDestZip As Boolean
        <DataMember()> _
        Public Property HasChangedBookDestZip() As Boolean
            Get
                Return _HasChangedBookDestZip
            End Get
            Set(ByVal value As Boolean)
                _HasChangedBookDestZip = value
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

        Private _HasChangedBookDestAddress1 As Boolean
        <DataMember()> _
        Public Property HasChangedBookDestAddress1() As Boolean
            Get
                Return _HasChangedBookDestAddress1
            End Get
            Set(ByVal value As Boolean)
                _HasChangedBookDestAddress1 = value
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

        Private _HasChangedBookMustLeaveByDateTime As Boolean
        <DataMember()> _
        Public Property HasChangedBookMustLeaveByDateTime() As Boolean
            Get
                Return _HasChangedBookMustLeaveByDateTime
            End Get
            Set(ByVal value As Boolean)
                _HasChangedBookMustLeaveByDateTime = value
            End Set
        End Property

        Private _BookMustLeaveByDateTime As Date?
        <DataMember()> _
        Public Property BookMustLeaveByDateTime() As Date?
            Get
                Return _BookMustLeaveByDateTime
            End Get
            Set(ByVal value As Date?)
                _BookMustLeaveByDateTime = value
            End Set
        End Property

        Private _HasChangedBookProNumber As Boolean
        <DataMember()> _
        Public Property HasChangedBookProNumber() As Boolean
            Get
                Return _HasChangedBookProNumber
            End Get
            Set(ByVal value As Boolean)
                _HasChangedBookProNumber = value
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

        Private _HasChangedBookLoadCom As Boolean
        <DataMember()> _
        Public Property HasChangedBookLoadCom() As Boolean
            Get
                Return _HasChangedBookLoadCom
            End Get
            Set(ByVal value As Boolean)
                _HasChangedBookLoadCom = value
            End Set
        End Property

        Private _BookLoadCom As String
        <DataMember()> _
        Public Property BookLoadCom() As String
            Get
                If Len(Trim(_BookLoadCom)) < 1 Then _BookLoadCom = "D"
                Return _BookLoadCom
            End Get
            Set(ByVal value As String)
                _BookLoadCom = value
            End Set
        End Property

        Private _HasChangedLaneNumber As Boolean
        <DataMember()> _
        Public Property HasChangedLaneNumber() As Boolean
            Get
                Return _HasChangedLaneNumber
            End Get
            Set(ByVal value As Boolean)
                _HasChangedLaneNumber = value
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

        Private _HasChangedBookModeTypeControl As Boolean
        <DataMember()> _
        Public Property HasChangedBookModeTypeControl() As Boolean
            Get
                Return _HasChangedBookModeTypeControl
            End Get
            Set(ByVal value As Boolean)
                _HasChangedBookModeTypeControl = value
            End Set
        End Property

        Private _BookModeTypeControl As Integer = 0
        <DataMember()> _
        Public Property BookModeTypeControl() As Integer
            Get
                Return _BookModeTypeControl
            End Get
            Set(ByVal value As Integer)
                _BookModeTypeControl = value
            End Set
        End Property

        Private _HasChangedBookUser1 As Boolean
        <DataMember()> _
        Public Property HasChangedBookUser1() As Boolean
            Get
                Return _HasChangedBookUser1
            End Get
            Set(ByVal value As Boolean)
                _HasChangedBookUser1 = value
            End Set
        End Property

        Private _BookUser1 As String = ""
        <DataMember()> _
        Public Property BookUser1() As String
            Get
                Return Left(_BookUser1, 4000)
            End Get
            Set(ByVal value As String)
                _BookUser1 = Left(value, 4000)
            End Set
        End Property

        Private _HasChangedBookUser2 As Boolean
        <DataMember()> _
        Public Property HasChangedBookUser2() As Boolean
            Get
                Return _HasChangedBookUser2
            End Get
            Set(ByVal value As Boolean)
                _HasChangedBookUser2 = value
            End Set
        End Property

        Private _BookUser2 As String = ""
        <DataMember()> _
        Public Property BookUser2() As String
            Get
                Return Left(_BookUser2, 4000)
            End Get
            Set(ByVal value As String)
                _BookUser2 = Left(value, 4000)
            End Set
        End Property

        Private _HasChangedBookUser3 As Boolean
        <DataMember()> _
        Public Property HasChangedBookUser3() As Boolean
            Get
                Return _HasChangedBookUser3
            End Get
            Set(ByVal value As Boolean)
                _HasChangedBookUser3 = value
            End Set
        End Property

        Private _BookUser3 As String = ""
        <DataMember()> _
        Public Property BookUser3() As String
            Get
                Return Left(_BookUser3, 4000)
            End Get
            Set(ByVal value As String)
                _BookUser3 = Left(value, 4000)
            End Set
        End Property

        Private _HasChangedBookUser4 As Boolean
        <DataMember()> _
        Public Property HasChangedBookUser4() As Boolean
            Get
                Return _HasChangedBookUser4
            End Get
            Set(ByVal value As Boolean)
                _HasChangedBookUser4 = value
            End Set
        End Property

        Private _BookUser4 As String = ""
        <DataMember()> _
        Public Property BookUser4() As String
            Get
                Return Left(_BookUser4, 4000)
            End Get
            Set(ByVal value As String)
                _BookUser4 = Left(value, 4000)
            End Set
        End Property

#End Region

#Region " Public Methods"
        Public Overrides Function Clone() As DTOBaseClass
            Dim instance As New BookChanges
            instance = DirectCast(MemberwiseClone(), BookChanges)

           
            Return instance
        End Function

#End Region

    End Class
End Namespace
