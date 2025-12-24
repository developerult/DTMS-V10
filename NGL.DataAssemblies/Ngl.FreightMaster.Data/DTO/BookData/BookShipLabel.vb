Imports Microsoft.VisualBasic
Imports System
Imports System.Collections.Generic
Imports System.Linq
Imports System.Text
Imports System.Runtime.Serialization
Imports Ngl.Core.ChangeTracker

Namespace DataTransferObjects
    <DataContract()> _
    Public Class BookShipLabel
        Inherits DTOBaseClass


#Region " Data Members"
        Private _BookShipLabelControl As Integer = 0
        <DataMember()> _
        Public Property BookShipLabelControl() As Integer
            Get
                Return _BookShipLabelControl
            End Get
            Set(ByVal value As Integer)
                _BookShipLabelControl = value
            End Set
        End Property

        Private _BookSHID As String
        <DataMember()> _
        Public Property BookSHID() As String
            Get
                Return Left(_BookSHID, 50)
            End Get
            Set(ByVal value As String)
                If (String.Equals(Me._BookSHID, value) = False) Then
                    Me._BookSHID = Left(value, 50)
                    Me.SendPropertyChanged("BookSHID")
                End If
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


        Private _ShipCompControl As Integer = 0
        <DataMember()> _
        Public Property ShipCompControl() As Integer
            Get
                Return _ShipCompControl
            End Get
            Set(ByVal value As Integer)
                _ShipCompControl = value
            End Set
        End Property

        Private _ShipCompNumber As Integer = 0
        <DataMember()> _
        Public Property ShipCompNumber() As Integer
            Get
                Return _ShipCompNumber
            End Get
            Set(ByVal value As Integer)
                _ShipCompNumber = value
            End Set
        End Property

        Private _ShipCompName As String = ""
        <DataMember()> _
        Public Property ShipCompName() As String
            Get
                Return Left(_ShipCompName, 40)
            End Get
            Set(ByVal value As String)
                _ShipCompName = Left(value, 40)
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

        Private _BookOrigCompControl As Integer = 0
        <DataMember()> _
        Public Property BookOrigCompControl() As Integer
            Get
                Return _BookOrigCompControl
            End Get
            Set(ByVal value As Integer)
                _BookOrigCompControl = value
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

        Private _BookOrigAddress3 As String = ""
        <DataMember()> _
        Public Property BookOrigAddress3() As String
            Get
                Return Left(_BookOrigAddress3, 40)
            End Get
            Set(ByVal value As String)
                _BookOrigAddress3 = Left(value, 40)
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

        Private _BookDestCompControl As Integer = 0
        <DataMember()> _
        Public Property BookDestCompControl() As Integer
            Get
                Return _BookDestCompControl
            End Get
            Set(ByVal value As Integer)
                _BookDestCompControl = value
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

        Private _BookDestAddress3 As String = ""
        <DataMember()> _
        Public Property BookDestAddress3() As String
            Get
                Return Left(_BookDestAddress3, 40)
            End Get
            Set(ByVal value As String)
                _BookDestAddress3 = Left(value, 40)
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

        Private _PrintQty As Integer = 1
        <DataMember()> _
        Public Property PrintQty() As Integer
            Get
                Return _PrintQty
            End Get
            Set(ByVal value As Integer)
                _PrintQty = value
            End Set
        End Property

        Private _BookShipLabelDetails As New List(Of BookShipLabelDetail)
        <DataMember()> _
        Public Property BookShipLabelDetails() As List(Of BookShipLabelDetail)
            Get
                Return _BookShipLabelDetails
            End Get
            Set(ByVal value As List(Of BookShipLabelDetail))
                _BookShipLabelDetails = value
            End Set
        End Property


#End Region


        
#Region " Public Methods"

        Public Overrides Function Clone() As DTOBaseClass
            Dim instance As New BookShipLabel
            instance = DirectCast(MemberwiseClone(), BookShipLabel)
            instance.BookShipLabelDetails = Nothing
            For Each item In BookShipLabelDetails
                instance.BookShipLabelDetails.Add(DirectCast(item.Clone, BookShipLabelDetail))
            Next
            Return instance
        End Function

        Public Function TotalCases() As Integer
            If Me.BookShipLabelDetails Is Nothing OrElse Me.BookShipLabelDetails.Count < 1 Then Return 0
            Return Me.BookShipLabelDetails.Sum(Function(x) x.BookTotalCases)
        End Function

        Public Function TotalWgt() As Double
            If Me.BookShipLabelDetails Is Nothing OrElse Me.BookShipLabelDetails.Count < 1 Then Return 0
            Return Me.BookShipLabelDetails.Sum(Function(x) x.BookTotalWgt)
        End Function

        Public Function TotalPL() As Double
            If Me.BookShipLabelDetails Is Nothing OrElse Me.BookShipLabelDetails.Count < 1 Then Return 0
            Return Me.BookShipLabelDetails.Sum(Function(x) x.BookTotalPL)
        End Function

        Public Function TotalCube() As Integer
            If Me.BookShipLabelDetails Is Nothing OrElse Me.BookShipLabelDetails.Count < 1 Then Return 0
            Return Me.BookShipLabelDetails.Sum(Function(x) x.BookTotalCube)
        End Function

        Public Function TotalPX() As Integer
            If Me.BookShipLabelDetails Is Nothing OrElse Me.BookShipLabelDetails.Count < 1 Then Return 0
            Return Me.BookShipLabelDetails.Sum(Function(x) x.BookTotalPX)
        End Function

        Public Function DateLoad() As System.Nullable(Of Date)
            If Me.BookShipLabelDetails Is Nothing OrElse Me.BookShipLabelDetails.Count < 1 Then Return Nothing
            Return Me.BookShipLabelDetails.Min(Function(x) x.BookDateLoad)
        End Function

        Public Function DateRequired() As System.Nullable(Of Date)
            If Me.BookShipLabelDetails Is Nothing OrElse Me.BookShipLabelDetails.Count < 1 Then Return Nothing
            Return Me.BookShipLabelDetails.Max(Function(x) x.BookDateRequired)
        End Function

        Public Function TotalBFC() As Decimal
            If Me.BookShipLabelDetails Is Nothing OrElse Me.BookShipLabelDetails.Count < 1 Then Return 0
            Return Me.BookShipLabelDetails.Sum(Function(x) x.BookTotalBFC)
        End Function

        Public Function TotalCarrierCost() As Decimal
            If Me.BookShipLabelDetails Is Nothing OrElse Me.BookShipLabelDetails.Count < 1 Then Return 0
            Return Me.BookShipLabelDetails.Sum(Function(x) x.BookRevTotalCost)
        End Function

        Public Function OrderNumbers() As String

            If Me.BookShipLabelDetails Is Nothing OrElse Me.BookShipLabelDetails.Count < 1 Then Return ""
            Dim strRet As String = ""
            Dim strOrderNumbers As List(Of String) = (From d In BookShipLabelDetails Select OrderNumber = d.BookCarrOrderNumber & "-" & d.BookOrderSequence.ToString()).ToList()
            If Not strOrderNumbers Is Nothing AndAlso strOrderNumbers.Count > 0 Then
                strRet = String.Join(",", strOrderNumbers)
            End If
            Return strRet

        End Function

        Public Sub populatePrintQty(ByVal LabelPrintMaxDefault As Integer)
            'put sp code in here
            Dim PL = TotalPL()
            If PL = 0 Then PL = 1
            PrintQty = If(PL < LabelPrintMaxDefault, PL, LabelPrintMaxDefault)
        End Sub

#End Region

    End Class
End Namespace