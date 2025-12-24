Imports Microsoft.VisualBasic
Imports System
Imports System.Collections.Generic
Imports System.Linq
Imports System.Text
Imports System.Runtime.Serialization
Imports Ngl.Core.ChangeTracker

Namespace DataTransferObjects
    <DataContract()> _
    Public Class Claim
        Inherits DTOBaseClass


#Region " Data Members"
        Private _ClaimControl As Integer = 0
        <DataMember()> _
        Public Property ClaimControl() As Integer
            Get
                Return Me._ClaimControl
            End Get
            Set(ByVal value As Integer)
                If ((Me._ClaimControl = value) _
                   = False) Then
                    Me._ClaimControl = value
                    Me.SendPropertyChanged("ClaimControl")
                End If
            End Set
        End Property


        Private _ClaimCustCompControl As Integer = 0
        <DataMember()> _
        Public Property ClaimCustCompControl() As Integer
            Get
                Return Me._ClaimCustCompControl
            End Get
            Set(ByVal value As Integer)
                If ((Me._ClaimCustCompControl = value) _
                   = False) Then
                    Me._ClaimCustCompControl = value
                    Me.SendPropertyChanged("ClaimCustCompControl")
                End If
            End Set
        End Property

        Private _ClaimProNumber As String = ""
        <DataMember()> _
        Public Property ClaimProNumber() As String
            Get
                Return Left(Me._ClaimProNumber, 20)
            End Get
            Set(ByVal value As String)
                If (String.Equals(Me._ClaimProNumber, value) = False) Then
                    Me._ClaimProNumber = Left(value, 20)
                    Me.SendPropertyChanged("ClaimProNumber")
                End If
            End Set
        End Property

        Private _ClaimCarrierControl As Integer = 0
        <DataMember()> _
        Public Property ClaimCarrierControl() As Integer
            Get
                Return Me._ClaimCarrierControl
            End Get
            Set(ByVal value As Integer)
                If ((Me._ClaimCarrierControl = value) = False) Then
                    Me._ClaimCarrierControl = value
                    Me.SendPropertyChanged("ClaimCarrierControl")
                End If
            End Set
        End Property

        Private _ClaimCarrierContact As String = ""
        <DataMember()> _
        Public Property ClaimCarrierContact() As String
            Get
                Return Left(Me._ClaimCarrierContact, 30)
            End Get
            Set(ByVal value As String)
                If (String.Equals(Me._ClaimCarrierContact, value) = False) Then
                    Me._ClaimCarrierContact = Left(value, 30)
                    Me.SendPropertyChanged("ClaimCarrierContact")
                End If
            End Set
        End Property

        Private _ClaimCarrierContactPhone As String = ""
        <DataMember()> _
        Public Property ClaimCarrierContactPhone() As String
            Get
                Return Left(Me._ClaimCarrierContactPhone, 20)
            End Get
            Set(ByVal value As String)
                If (String.Equals(Me._ClaimCarrierContactPhone, value) = False) Then
                    Me._ClaimCarrierContactPhone = Left(value, 20)
                    Me.SendPropertyChanged("ClaimCarrierContactPhone")
                End If
            End Set
        End Property

        Private _ClaimVendCompControl As Integer = 0
        <DataMember()> _
        Public Property ClaimVendCompControl() As Integer
            Get
                Return Me._ClaimVendCompControl
            End Get
            Set(ByVal value As Integer)
                If ((Me._ClaimVendCompControl = value) = False) Then
                    Me._ClaimVendCompControl = value
                    Me.SendPropertyChanged("ClaimVendCompControl")
                End If
            End Set
        End Property

        Private _ClaimVendName As String = ""
        <DataMember()> _
        Public Property ClaimVendName() As String
            Get
                Return Left(Me._ClaimVendName, 40)
            End Get
            Set(ByVal value As String)
                If (String.Equals(Me._ClaimVendName, value) = False) Then
                    Me._ClaimVendName = Left(value, 40)
                    Me.SendPropertyChanged("ClaimVendName")
                End If
            End Set
        End Property

        Private _ClaimVendAddress1 As String = ""
        <DataMember()> _
        Public Property ClaimVendAddress1() As String
            Get
                Return Left(Me._ClaimVendAddress1, 40)
            End Get
            Set(ByVal value As String)
                If (String.Equals(Me._ClaimVendAddress1, value) = False) Then
                    Me._ClaimVendAddress1 = Left(value, 40)
                    Me.SendPropertyChanged("ClaimVendAddress1")
                End If
            End Set
        End Property

        Private _ClaimVendAddress2 As String = ""
        <DataMember()> _
        Public Property ClaimVendAddress2() As String
            Get
                Return Left(Me._ClaimVendAddress2, 40)
            End Get
            Set(ByVal value As String)
                If (String.Equals(Me._ClaimVendAddress2, value) = False) Then
                    Me._ClaimVendAddress2 = Left(value, 40)
                    Me.SendPropertyChanged("ClaimVendAddress2")
                End If
            End Set
        End Property

        Private _ClaimVendAddress3 As String = ""
        <DataMember()> _
        Public Property ClaimVendAddress3() As String
            Get
                Return Left(Me._ClaimVendAddress3, 40)
            End Get
            Set(ByVal value As String)
                If (String.Equals(Me._ClaimVendAddress3, value) = False) Then
                    Me._ClaimVendAddress3 = Left(value, 40)
                    Me.SendPropertyChanged("ClaimVendAddress3")
                End If
            End Set
        End Property

        Private _ClaimVendCity As String = ""
        <DataMember()> _
        Public Property ClaimVendCity() As String
            Get
                Return Left(Me._ClaimVendCity, 25)
            End Get
            Set(ByVal value As String)
                If (String.Equals(Me._ClaimVendCity, value) = False) Then
                    Me._ClaimVendCity = Left(value, 25)
                    Me.SendPropertyChanged("ClaimVendCity")
                End If
            End Set
        End Property

        Private _ClaimVendState As String = ""
        <DataMember()> _
        Public Property ClaimVendState() As String
            Get
                Return Left(Me._ClaimVendState, 8)
            End Get
            Set(ByVal value As String)
                If (String.Equals(Me._ClaimVendState, value) = False) Then
                    Me._ClaimVendState = Left(value, 8)
                    Me.SendPropertyChanged("ClaimVendState")
                End If
            End Set
        End Property

        Private _ClaimVendCountry As String = ""
        <DataMember()> _
        Public Property ClaimVendCountry() As String
            Get
                Return Left(Me._ClaimVendCountry, 30)
            End Get
            Set(ByVal value As String)
                If (String.Equals(Me._ClaimVendCountry, value) = False) Then
                    Me._ClaimVendCountry = Left(value, 30)
                    Me.SendPropertyChanged("ClaimVendCountry")
                End If
            End Set
        End Property

        Private _ClaimVendZip As String = ""
        <DataMember()> _
        Public Property ClaimVendZip() As String
            Get
                Return Left(Me._ClaimVendZip, 10)
            End Get
            Set(ByVal value As String)
                If (String.Equals(Me._ClaimVendZip, value) = False) Then
                    Me._ClaimVendZip = Left(value, 10)
                    Me.SendPropertyChanged("ClaimVendZip")
                End If
            End Set
        End Property

        Private _ClaimVendPhone As String = ""
        <DataMember()> _
        Public Property ClaimVendPhone() As String
            Get
                Return Left(Me._ClaimVendPhone, 15)
            End Get
            Set(ByVal value As String)
                If (String.Equals(Me._ClaimVendPhone, value) = False) Then
                    Me._ClaimVendPhone = Left(value, 15)
                    Me.SendPropertyChanged("ClaimVendPhone")
                End If
            End Set
        End Property

        Private _ClaimVendFax As String = ""
        <DataMember()> _
        Public Property ClaimVendFax() As String
            Get
                Return Left(Me._ClaimVendFax, 15)
            End Get
            Set(ByVal value As String)
                If (String.Equals(Me._ClaimVendFax, value) = False) Then
                    Me._ClaimVendFax = Left(value, 15)
                    Me.SendPropertyChanged("ClaimVendFax")
                End If
            End Set
        End Property

        Private _ClaimConsCompControl As Integer = 0
        <DataMember()> _
        Public Property ClaimConsCompControl() As Integer
            Get
                Return Me._ClaimConsCompControl
            End Get
            Set(ByVal value As Integer)
                If ((Me._ClaimConsCompControl = value) = False) Then
                    Me._ClaimConsCompControl = value
                    Me.SendPropertyChanged("ClaimConsCompControl")
                End If
            End Set
        End Property

        Private _ClaimConsName As String = ""
        <DataMember()> _
        Public Property ClaimConsName() As String
            Get
                Return Left(Me._ClaimConsName, 40)
            End Get
            Set(ByVal value As String)
                If (String.Equals(Me._ClaimConsName, value) = False) Then
                    Me._ClaimConsName = Left(value, 40)
                    Me.SendPropertyChanged("ClaimConsName")
                End If
            End Set
        End Property

        Private _ClaimConsAddress1 As String = ""
        <DataMember()> _
        Public Property ClaimConsAddress1() As String
            Get
                Return Left(Me._ClaimConsAddress1, 40)
            End Get
            Set(ByVal value As String)
                If (String.Equals(Me._ClaimConsAddress1, value) = False) Then
                    Me._ClaimConsAddress1 = Left(value, 40)
                    Me.SendPropertyChanged("ClaimConsAddress1")
                End If
            End Set
        End Property

        Private _ClaimConsAddress2 As String = ""
        <DataMember()> _
        Public Property ClaimConsAddress2() As String
            Get
                Return Left(Me._ClaimConsAddress2, 40)
            End Get
            Set(ByVal value As String)
                If (String.Equals(Me._ClaimConsAddress2, value) = False) Then
                    Me._ClaimConsAddress2 = Left(value, 40)
                    Me.SendPropertyChanged("ClaimConsAddress2")
                End If
            End Set
        End Property

        Private _ClaimConsAddress3 As String = ""
        <DataMember()> _
        Public Property ClaimConsAddress3() As String
            Get
                Return Left(Me._ClaimConsAddress3, 50)
            End Get
            Set(ByVal value As String)
                If (String.Equals(Me._ClaimConsAddress3, value) = False) Then
                    Me._ClaimConsAddress3 = Left(value, 50)
                    Me.SendPropertyChanged("ClaimConsAddress3")
                End If
            End Set
        End Property

        Private _ClaimConsCity As String = ""
        <DataMember()> _
        Public Property ClaimConsCity() As String
            Get
                Return Left(Me._ClaimConsCity, 25)
            End Get
            Set(ByVal value As String)
                If (String.Equals(Me._ClaimConsCity, value) = False) Then
                    Me._ClaimConsCity = Left(value, 25)
                    Me.SendPropertyChanged("ClaimConsCity")
                End If
            End Set
        End Property

        Private _ClaimConsState As String = ""
        <DataMember()> _
        Public Property ClaimConsState() As String
            Get
                Return Left(Me._ClaimConsState, 2)
            End Get
            Set(ByVal value As String)
                If (String.Equals(Me._ClaimConsState, value) = False) Then
                    Me._ClaimConsState = Left(value, 2)
                    Me.SendPropertyChanged("ClaimConsState")
                End If
            End Set
        End Property

        Private _ClaimConsCountry As String = ""
        <DataMember()> _
        Public Property ClaimConsCountry() As String
            Get
                Return Left(Me._ClaimConsCountry, 30)
            End Get
            Set(ByVal value As String)
                If (String.Equals(Me._ClaimConsCountry, value) = False) Then
                    Me._ClaimConsCountry = Left(value, 30)
                    Me.SendPropertyChanged("ClaimConsCountry")
                End If
            End Set
        End Property

        Private _ClaimConsZip As String = ""
        <DataMember()> _
        Public Property ClaimConsZip() As String
            Get
                Return Left(Me._ClaimConsZip, 10)
            End Get
            Set(ByVal value As String)
                If (String.Equals(Me._ClaimConsZip, value) = False) Then
                    Me._ClaimConsZip = Left(value, 10)
                    Me.SendPropertyChanged("ClaimConsZip")
                End If
            End Set
        End Property

        Private _ClaimConsPhone As String = ""
        <DataMember()> _
        Public Property ClaimConsPhone() As String
            Get
                Return Left(Me._ClaimConsPhone, 15)
            End Get
            Set(ByVal value As String)
                If (String.Equals(Me._ClaimConsPhone, value) = False) Then
                    Me._ClaimConsPhone = Left(value, 15)
                    Me.SendPropertyChanged("ClaimConsPhone")
                End If
            End Set
        End Property

        Private _ClaimConsFax As String = ""
        <DataMember()> _
        Public Property ClaimConsFax() As String
            Get
                Return Left(Me._ClaimConsFax, 15)
            End Get
            Set(ByVal value As String)
                If (String.Equals(Me._ClaimConsFax, value) = False) Then
                    Me._ClaimConsFax = Left(value, 15)
                    Me.SendPropertyChanged("ClaimConsFax")
                End If
            End Set
        End Property

        Private _ClaimDateSubm As System.Nullable(Of Date)
        <DataMember()> _
        Public Property ClaimDateSubm() As System.Nullable(Of Date)
            Get
                Return Me._ClaimDateSubm
            End Get
            Set(ByVal value As System.Nullable(Of Date))
                If (Me._ClaimDateSubm.Equals(value) = False) Then
                    Me._ClaimDateSubm = value
                    Me.SendPropertyChanged("ClaimDateSubm")
                End If
            End Set
        End Property

        Private _ClaimDateAck As System.Nullable(Of Date)
        <DataMember()> _
        Public Property ClaimDateAck() As System.Nullable(Of Date)
            Get
                Return Me._ClaimDateAck
            End Get
            Set(ByVal value As System.Nullable(Of Date))
                If (Me._ClaimDateAck.Equals(value) = False) Then
                    Me._ClaimDateAck = value
                    Me.SendPropertyChanged("ClaimDateAck")
                End If
            End Set
        End Property

        Private _ClaimDatePaid As System.Nullable(Of Date)
        <DataMember()> _
        Public Property ClaimDatePaid() As System.Nullable(Of Date)
            Get
                Return Me._ClaimDatePaid
            End Get
            Set(ByVal value As System.Nullable(Of Date))
                If (Me._ClaimDatePaid.Equals(value) = False) Then
                    Me._ClaimDatePaid = value
                    Me.SendPropertyChanged("ClaimDatePaid")
                End If
            End Set
        End Property

        Private _ClaimCheckNo As String = ""
        <DataMember()> _
        Public Property ClaimCheckNo() As String
            Get
                Return Left(Me._ClaimCheckNo, 20)
            End Get
            Set(ByVal value As String)
                If (String.Equals(Me._ClaimCheckNo, value) = False) Then
                    Me._ClaimCheckNo = Left(value, 20)
                    Me.SendPropertyChanged("ClaimCheckNo")
                End If
            End Set
        End Property

        Private _ClaimCheckAmt As Decimal = 0
        <DataMember()> _
        Public Property ClaimCheckAmt() As Decimal
            Get
                Return Me._ClaimCheckAmt
            End Get
            Set(ByVal value As Decimal)
                If ((Me._ClaimCheckAmt = value) = False) Then
                    Me._ClaimCheckAmt = value
                    Me.SendPropertyChanged("ClaimCheckAmt")
                End If
            End Set
        End Property

        Private _ClaimClaimAmt As Decimal
        <DataMember()> _
        Public Property ClaimClaimAmt() As Decimal
            Get
                Return Me._ClaimClaimAmt
            End Get
            Set(ByVal value As Decimal)
                If ((Me._ClaimClaimAmt = value) = False) Then
                    Me._ClaimClaimAmt = value
                    Me.SendPropertyChanged("ClaimClaimAmt")
                End If
            End Set
        End Property

        Private _ClaimDiff As Decimal
        <DataMember()> _
        Public Property ClaimDiff() As Decimal
            Get
                Return Me._ClaimDiff
            End Get
            Set(ByVal value As Decimal)
                If ((Me._ClaimDiff = value) = False) Then
                    Me._ClaimDiff = value
                    Me.SendPropertyChanged("ClaimDiff")
                End If
            End Set
        End Property

        Private _ClaimFB As String = ""
        <DataMember()> _
        Public Property ClaimFB() As String
            Get
                Return Left(Me._ClaimFB, 50)
            End Get
            Set(ByVal value As String)
                If (String.Equals(Me._ClaimFB, value) = False) Then
                    Me._ClaimFB = Left(value, 50)
                    Me.SendPropertyChanged("ClaimFB")
                End If
            End Set
        End Property

        Private _ClaimConnLine As String = ""
        <DataMember()> _
        Public Property ClaimConnLine() As String
            Get
                Return Left(Me._ClaimConnLine, 30)
            End Get
            Set(ByVal value As String)
                If (String.Equals(Me._ClaimConnLine, value) = False) Then
                    Me._ClaimConnLine = Left(value, 30)
                    Me.SendPropertyChanged("ClaimConnLine")
                End If
            End Set
        End Property

        Private _ClaimInvName As String = ""
        <DataMember()> _
        Public Property ClaimInvName() As String
            Get
                Return Left(Me._ClaimInvName, 30)
            End Get
            Set(ByVal value As String)
                If (String.Equals(Me._ClaimInvName, value) = False) Then
                    Me._ClaimInvName = Left(value, 30)
                    Me.SendPropertyChanged("ClaimInvName")
                End If
            End Set
        End Property

        Private _ClaimInvPhone As String = ""
        <DataMember()> _
        Public Property ClaimInvPhone() As String
            Get
                Return Left(Me._ClaimInvPhone, 50)
            End Get
            Set(ByVal value As String)
                If (String.Equals(Me._ClaimInvPhone, value) = False) Then
                    Me._ClaimInvPhone = Left(value, 50)
                    Me.SendPropertyChanged("ClaimInvPhone")
                End If
            End Set
        End Property

        Private _ClaimTruckNo As String = ""
        <DataMember()> _
        Public Property ClaimTruckNo() As String
            Get
                Return Left(Me._ClaimTruckNo, 20)
            End Get
            Set(ByVal value As String)
                If (String.Equals(Me._ClaimTruckNo, value) = False) Then
                    Me._ClaimTruckNo = Left(value, 20)
                    Me.SendPropertyChanged("ClaimTruckNo")
                End If
            End Set
        End Property

        Private _ClaimShipDesc As String = ""
        <DataMember()> _
        Public Property ClaimShipDesc() As String
            Get
                Return Left(Me._ClaimShipDesc, 50)
            End Get
            Set(ByVal value As String)
                If (String.Equals(Me._ClaimShipDesc, value) = False) Then
                    Me._ClaimShipDesc = Left(value, 50)
                    Me.SendPropertyChanged("ClaimShipDesc")
                End If
            End Set
        End Property

        Private _ClaimShipFrom As String = ""
        <DataMember()> _
        Public Property ClaimShipFrom() As String
            Get
                Return Left(Me._ClaimShipFrom, 40)
            End Get
            Set(ByVal value As String)
                If (String.Equals(Me._ClaimShipFrom, value) = False) Then
                    Me._ClaimShipFrom = Left(value, 40)
                    Me.SendPropertyChanged("ClaimShipFrom")
                End If
            End Set
        End Property

        Private _ClaimShipTo As String = ""
        <DataMember()> _
        Public Property ClaimShipTo() As String
            Get
                Return Left(Me._ClaimShipTo, 40)
            End Get
            Set(ByVal value As String)
                If (String.Equals(Me._ClaimShipTo, value) = False) Then
                    Me._ClaimShipTo = Left(value, 40)
                    Me.SendPropertyChanged("ClaimShipTo")
                End If
            End Set
        End Property

        Private _ClaimFinalDest As String = ""
        <DataMember()> _
        Public Property ClaimFinalDest() As String
            Get
                Return Left(Me._ClaimFinalDest, 40)
            End Get
            Set(ByVal value As String)
                If (String.Equals(Me._ClaimFinalDest, value) = False) Then
                    Me._ClaimFinalDest = Left(value, 40)
                    Me.SendPropertyChanged("ClaimFinalDest")
                End If
            End Set
        End Property

        Private _ClaimBOLIssueby As String = ""
        <DataMember()> _
        Public Property ClaimBOLIssueby() As String
            Get
                Return Left(Me._ClaimBOLIssueby, 40)
            End Get
            Set(ByVal value As String)
                If (String.Equals(Me._ClaimBOLIssueby, value) = False) Then
                    Me._ClaimBOLIssueby = Left(value, 40)
                    Me.SendPropertyChanged("ClaimBOLIssueby")
                End If
            End Set
        End Property

        Private _ClaimBOLIssueDate As System.Nullable(Of Date)
        <DataMember()> _
        Public Property ClaimBOLIssueDate() As System.Nullable(Of Date)
            Get
                Return Me._ClaimBOLIssueDate
            End Get
            Set(ByVal value As System.Nullable(Of Date))
                If (Me._ClaimBOLIssueDate.Equals(value) = False) Then
                    Me._ClaimBOLIssueDate = value
                    Me.SendPropertyChanged("ClaimBOLIssueDate")
                End If
            End Set
        End Property

        Private _ClaimRemark As String = ""
        <DataMember()> _
        Public Property ClaimRemark() As String
            Get
                Return Left(Me._ClaimRemark, 80)
            End Get
            Set(ByVal value As String)
                If (String.Equals(Me._ClaimRemark, value) = False) Then
                    Me._ClaimRemark = Left(value, 80)
                    Me.SendPropertyChanged("ClaimRemark")
                End If
            End Set
        End Property

        Private _ClaimVia As String = ""
        <DataMember()> _
        Public Property ClaimVia() As String
            Get
                Return Left(Me._ClaimVia, 40)
            End Get
            Set(ByVal value As String)
                If (String.Equals(Me._ClaimVia, value) = False) Then
                    Me._ClaimVia = Left(value, 40)
                    Me.SendPropertyChanged("ClaimVia")
                End If
            End Set
        End Property

        Private _ClaimOrderNumber As String = ""
        <DataMember()> _
        Public Property ClaimOrderNumber() As String
            Get
                Return Left(Me._ClaimOrderNumber, 50)
            End Get
            Set(ByVal value As String)
                If (String.Equals(Me._ClaimOrderNumber, value) = False) Then
                    Me._ClaimOrderNumber = Left(value, 50)
                    Me.SendPropertyChanged("ClaimOrderNumber")
                End If
            End Set
        End Property

        Private _ClaimDeclined As Boolean = False
        <DataMember()> _
        Public Property ClaimDeclined() As Boolean
            Get
                Return Me._ClaimDeclined
            End Get
            Set(ByVal value As Boolean)
                If ((Me._ClaimDeclined = value) _
                   = False) Then
                    Me._ClaimDeclined = value
                    Me.SendPropertyChanged("ClaimDeclined")
                End If
            End Set
        End Property

        Private _ClaimDeclinedAmt As Decimal = 0
        <DataMember()> _
        Public Property ClaimDeclinedAmt() As Decimal
            Get
                Return Me._ClaimDeclinedAmt
            End Get
            Set(ByVal value As Decimal)
                If ((Me._ClaimDeclinedAmt = value) = False) Then
                    Me._ClaimDeclinedAmt = value
                    Me.SendPropertyChanged("ClaimDeclinedAmt")
                End If
            End Set
        End Property

        Private _ClaimDeclinedDate As System.Nullable(Of Date)
        <DataMember()> _
        Public Property ClaimDeclinedDate() As System.Nullable(Of Date)
            Get
                Return Me._ClaimDeclinedDate
            End Get
            Set(ByVal value As System.Nullable(Of Date))
                If (Me._ClaimDeclinedDate.Equals(value) = False) Then
                    Me._ClaimDeclinedDate = value
                    Me.SendPropertyChanged("ClaimDeclinedDate")
                End If
            End Set
        End Property

        Private _ClaimDeclinedByCarrRep As String = ""
        <DataMember()> _
        Public Property ClaimDeclinedByCarrRep() As String
            Get
                Return Left(Me._ClaimDeclinedByCarrRep, 50)
            End Get
            Set(ByVal value As String)
                If (String.Equals(Me._ClaimDeclinedByCarrRep, value) = False) Then
                    Me._ClaimDeclinedByCarrRep = Left(value, 50)
                    Me.SendPropertyChanged("ClaimDeclinedByCarrRep")
                End If
            End Set
        End Property

        Private _ClaimDeclinedReason As String = ""
        <DataMember()> _
        Public Property ClaimDeclinedReason() As String
            Get
                Return Left(Me._ClaimDeclinedReason, 250)
            End Get
            Set(ByVal value As String)
                If (String.Equals(Me._ClaimDeclinedReason, value) = False) Then
                    Me._ClaimDeclinedReason = Left(value, 250)
                    Me.SendPropertyChanged("ClaimDeclinedReason")
                End If
            End Set
        End Property

        Private _ClaimModDate As System.Nullable(Of Date)
        <DataMember()> _
        Public Property ClaimModDate() As System.Nullable(Of Date)
            Get
                Return Me._ClaimModDate
            End Get
            Set(ByVal value As System.Nullable(Of Date))
                If (Me._ClaimModDate.Equals(value) = False) Then
                    Me._ClaimModDate = value
                    Me.SendPropertyChanged("ClaimModDate")
                End If
            End Set
        End Property

        Private _ClaimModUser As String = ""
        <DataMember()> _
        Public Property ClaimModUser() As String
            Get
                Return Left(Me._ClaimModUser, 100)
            End Get
            Set(ByVal value As String)
                If (String.Equals(Me._ClaimModUser, value) = False) Then
                    Me._ClaimModUser = Left(value, 100)
                    Me.SendPropertyChanged("ClaimModUser")
                End If
            End Set
        End Property

        Private _ClaimUpdated As Byte()
        <DataMember()> _
        Public Property ClaimUpdated() As Byte()
            Get
                Return Me._ClaimUpdated
            End Get
            Set(ByVal value As Byte())
                _ClaimUpdated = value
                Me.SendPropertyChanged("ClaimUpdated")
            End Set
        End Property

#End Region

#Region " Public Methods"
        Public Overrides Function Clone() As DTOBaseClass
            Dim instance As New Claim
            instance = DirectCast(MemberwiseClone(), Claim)
            Return instance
        End Function

#End Region

    End Class

End Namespace