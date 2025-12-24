Imports Microsoft.VisualBasic
Imports System
Imports System.Collections.Generic
Imports System.Linq
Imports System.Text
Imports System.Runtime.Serialization
Imports System.ComponentModel
Imports System.Data
Imports System.Data.Linq
Imports System.Data.Linq.Mapping
Imports System.Linq.Expressions
Imports System.Reflection
Imports Ngl.Core.ChangeTracker

Namespace DataTransferObjects
    <DataContract()> _
    Public Class tblSolutionDetail
        Inherits DTOBaseClass

#Region " Constructors "

        Public Sub New()
            MyBase.New()
        End Sub

        Public Sub New(ByRef parameters As WCFParameters)
            MyBase.New()
            Me.Parameters = parameters
        End Sub

        'Public Sub New(ByVal control As Integer, _
        '           ByVal c As Double, _
        '           ByVal w As Double, _
        '           ByVal p As Double, _
        '           ByVal cu As Double, _
        '           ByVal n As String, _
        '           Optional ByVal l As String = "NA", _
        '           Optional ByVal s As Integer = 0)
        '    MyBase.New()
        '    Me.Control = control
        '    SolutionDetailTotalCases = c
        '    SolutionDetailTotalWgt = w
        '    SolutionDetailTotalPL = p
        '    SolutionDetailTotalCube = cu
        '    OrderNumber = n
        '    LaneLocation = l
        '    RouteSequence = s
        'End Sub

        'Public Sub New(ByVal c As Integer, _
        '               ByVal n As String, _
        '               ByVal l As String, _
        '               ByVal s As Integer, _
        '               ByVal d As List(Of clsItemDetails), _
        '               Optional ByVal os As Integer = 0)
        '    MyBase.New()
        '    Control = c
        '    OrderNumber = n
        '    LaneNumber = l
        '    RouteSequence = s
        '    Details = d
        '    OrderSequence = os
        'End Sub


#End Region

#Region " Server Properties "

        Private _HasBeenSplit As Boolean = False
        Public Property HasBeenSplit As Boolean
            Get
                Return _HasBeenSplit
            End Get
            Set(value As Boolean)
                _HasBeenSplit = value
            End Set
        End Property

        ''' <summary>
        ''' this methods is used to assign a temporary order sequence number starting at 1000001
        ''' during save any orders with a temporary order sequence number must be inserted to the 
        ''' database with the correct order sequnce number based on the final solution
        ''' used primarily for order splitting or large orders
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public ReadOnly Property nextOrderSequenceNbr As Integer
            Get
                Return TempOrderSequence + 1
            End Get
        End Property

        Private _TempControl As Integer = 0
        Public Property TempControl As Integer
            Get
                If _TempControl = 0 Then _TempControl = Me.SolutionDetailBookControl
                Return _TempControl
            End Get
            Set(value As Integer)
                _TempControl = value
            End Set
        End Property

        Public Property OrderNumber As String
            Get
                Return Me.SolutionDetailOrderNumber
            End Get
            Set(value As String)
                Me.SolutionDetailOrderNumber = value
            End Set
        End Property

        ''' <summary>
        ''' This method holds the temporary order sequnce number used by the 
        ''' when splitting loads.  all numbers start at 1000000 to avoid 
        ''' conflicts with actual order sequence numbers
        ''' </summary>
        ''' <remarks></remarks>
        Private _TempOrderSequence As Integer = 0
        Public Property TempOrderSequence As Integer
            Get
                If _TempOrderSequence = 0 Then _TempOrderSequence = Me.SolutionDetailOrderSequence
                Return _TempOrderSequence
            End Get
            Set(value As Integer)
                _TempOrderSequence = value
            End Set
        End Property

        Private _Details As New List(Of BookItem)
        Public Property Details As List(Of BookItem)
            Get
                Return _Details
            End Get
            Set(value As List(Of BookItem))
                _Details = value
                Me.SendPropertyChanged("Details")
            End Set
        End Property

        Public ReadOnly Property Cases() As Double
            Get
                If Not Details Is Nothing AndAlso Details.Count > 0 Then
                    SolutionDetailTotalCases = Details.Sum(Function(c) c.BookItemQtyOrdered)
                End If
                Return SolutionDetailTotalCases
            End Get
        End Property

        Public ReadOnly Property Wgt() As Double
            Get
                If Not Details Is Nothing AndAlso Details.Count > 0 Then
                    SolutionDetailTotalWgt = Details.Sum(Function(c) c.BookItemWeight)
                End If
                Return SolutionDetailTotalWgt
            End Get
        End Property

        Public ReadOnly Property Plts() As Double
            Get
                If Not Details Is Nothing AndAlso Details.Count > 0 Then
                    SolutionDetailTotalPL = Details.Sum(Function(c) c.BookItemPallets)
                End If
                Return SolutionDetailTotalPL
            End Get
        End Property

        Public ReadOnly Property Cubes() As Double
            Get
                If Not Details Is Nothing AndAlso Details.Count > 0 Then
                    SolutionDetailTotalCube = Details.Sum(Function(c) c.BookItemCube)
                End If
                Return SolutionDetailTotalCube
            End Get
        End Property

        Public ReadOnly Property Hazmat() As Boolean
            Get
                Return SolutionDetailIsHazmat
            End Get
        End Property

        Public ReadOnly Property LaneLocation() As String
            Get
                Dim strRet As String = Me.SolutionDetailDestAddress1 & "-" & Me.SolutionDetailDestCity & "-" & Me.SolutionDetailDestState
                Return strRet
            End Get
        End Property

        Public Property RouteSequence As Integer
            Get
                Return SolutionDetailDefaultRouteSequence
            End Get
            Set(value As Integer)
                SolutionDetailDefaultRouteSequence = value
            End Set
        End Property

#End Region

#Region " Data Members"
        Private _SolutionDetailControl As Long = 0
        <DataMember()> _
        Public Property SolutionDetailControl() As Long
            Get
                Return Me._SolutionDetailControl
            End Get
            Set(ByVal value As Long)
                If ((Me._SolutionDetailControl = value) = False) Then
                    Me._SolutionDetailControl = value
                    Me.SendPropertyChanged("SolutionDetailControl")
                End If
            End Set
        End Property

        Private _SolutionDetailSolutionTruckControl As Long = 0
        <DataMember()> _
        Public Property SolutionDetailSolutionTruckControl() As Long
            Get
                Return Me._SolutionDetailSolutionTruckControl
            End Get
            Set(ByVal value As Long)
                If ((Me._SolutionDetailSolutionTruckControl = value) = False) Then
                    Me._SolutionDetailSolutionTruckControl = value
                    Me.SendPropertyChanged("SolutionDetailSolutionTruckControl")
                End If
            End Set
        End Property

        Private _SolutionDetailBookControl As Integer = 0
        <DataMember()> _
        Public Property SolutionDetailBookControl() As Integer
            Get
                Return Me._SolutionDetailBookControl
            End Get
            Set(ByVal value As Integer)
                If ((Me._SolutionDetailBookControl = value) = False) Then
                    Me._SolutionDetailBookControl = value
                    Me.SendPropertyChanged("SolutionDetailBookControl")
                End If
            End Set
        End Property

        Private _SolutionDetailPOHdrControl As Long = 0
        <DataMember()> _
        Public Property SolutionDetailPOHdrControl() As Long
            Get
                Return Me._SolutionDetailPOHdrControl
            End Get
            Set(ByVal value As Long)
                If ((Me._SolutionDetailPOHdrControl = value) = False) Then
                    Me._SolutionDetailPOHdrControl = value
                    Me.SendPropertyChanged("SolutionDetailPOHdrControl")
                End If
            End Set
        End Property

        Private _SolutionDetailBookLoadControl As Integer = 0
        <DataMember()> _
        Public Property SolutionDetailBookLoadControl() As Integer
            Get
                Return Me._SolutionDetailBookLoadControl
            End Get
            Set(ByVal value As Integer)
                If ((Me._SolutionDetailBookLoadControl = value) = False) Then
                    Me._SolutionDetailBookLoadControl = value
                    Me.SendPropertyChanged("SolutionDetailBookLoadControl")
                End If
            End Set
        End Property

        Private _SolutionDetailProNumber As String = ""
        <DataMember()> _
        Public Property SolutionDetailProNumber() As String
            Get
                Return Left(Me._SolutionDetailProNumber, 20)
            End Get
            Set(value As String)
                If (String.Equals(Me._SolutionDetailProNumber, value) = False) Then
                    Me._SolutionDetailProNumber = Left(value, 20)
                    Me.SendPropertyChanged("SolutionDetailProNumber")
                End If
            End Set
        End Property

        Private _SolutionDetailPONumber As String = ""
        <DataMember()> _
        Public Property SolutionDetailPONumber() As String
            Get
                Return Left(Me._SolutionDetailPONumber, 20)
            End Get
            Set(value As String)
                If (String.Equals(Me._SolutionDetailPONumber, value) = False) Then
                    Me._SolutionDetailPONumber = Left(value, 20)
                    Me.SendPropertyChanged("SolutionDetailPONumber")
                End If
            End Set
        End Property


        Private _SolutionDetailOrderNumber As String = ""
        <DataMember()> _
        Public Property SolutionDetailOrderNumber() As String
            Get
                Return Left(Me._SolutionDetailOrderNumber, 20)
            End Get
            Set(value As String)
                If (String.Equals(Me._SolutionDetailOrderNumber, value) = False) Then
                    Me._SolutionDetailOrderNumber = Left(value, 20)
                    Me.SendPropertyChanged("SolutionDetailOrderNumber")
                End If
            End Set
        End Property

        Private _SolutionDetailOrderSequence As Integer = 0
        <DataMember()> _
        Public Property SolutionDetailOrderSequence() As Integer
            Get
                Return Me._SolutionDetailOrderSequence
            End Get
            Set(ByVal value As Integer)
                If ((Me._SolutionDetailOrderSequence = value) = False) Then
                    Me._SolutionDetailOrderSequence = value
                    Me.SendPropertyChanged("SolutionDetailOrderSequence")
                End If
            End Set
        End Property

        Private _SolutionDetailCom As String = ""
        <DataMember()> _
        Public Property SolutionDetailCom() As String
            Get
                Return Left(Me._SolutionDetailCom, 1)
            End Get
            Set(value As String)
                If (String.Equals(Me._SolutionDetailCom, value) = False) Then
                    Me._SolutionDetailCom = Left(value, 1)
                    Me.SendPropertyChanged("SolutionDetailCom")
                End If
            End Set
        End Property

        Private _SolutionDetailConsPrefix As String = ""
        <DataMember()> _
        Public Property SolutionDetailConsPrefix() As String
            Get
                Return Left(Me._SolutionDetailConsPrefix, 20)
            End Get
            Set(value As String)
                If (String.Equals(Me._SolutionDetailConsPrefix, value) = False) Then
                    Me._SolutionDetailConsPrefix = Left(value, 20)
                    Me.SendPropertyChanged("SolutionDetailConsPrefix")
                End If
            End Set
        End Property

        Private _SolutionDetailCompControl As Integer = 0
        <DataMember()> _
        Public Property SolutionDetailCompControl() As Integer
            Get
                Return Me._SolutionDetailCompControl
            End Get
            Set(ByVal value As Integer)
                If ((Me._SolutionDetailCompControl = value) = False) Then
                    Me._SolutionDetailCompControl = value
                    Me.SendPropertyChanged("SolutionDetailCompControl")
                End If
            End Set
        End Property

        Private _SolutionDetailCompNumber As Integer = 0
        <DataMember()> _
        Public Property SolutionDetailCompNumber() As Integer
            Get
                Return Me._SolutionDetailCompNumber
            End Get
            Set(ByVal value As Integer)
                If ((Me._SolutionDetailCompNumber = value) = False) Then
                    Me._SolutionDetailCompNumber = value
                    Me.SendPropertyChanged("SolutionDetailCompNumber")
                End If
            End Set
        End Property

        Private _SolutionDetailCompName As String = ""
        <DataMember()> _
        Public Property SolutionDetailCompName() As String
            Get
                Return Left(Me._SolutionDetailCompName, 40)
            End Get
            Set(value As String)
                If (String.Equals(Me._SolutionDetailCompName, value) = False) Then
                    Me._SolutionDetailCompName = Left(value, 40)
                    Me.SendPropertyChanged("SolutionDetailCompName")
                End If
            End Set
        End Property

        Private _SolutionDetailCompNatNumber As Integer = 0
        <DataMember()> _
        Public Property SolutionDetailCompNatNumber() As Integer
            Get
                Return Me._SolutionDetailCompNatNumber
            End Get
            Set(ByVal value As Integer)
                If ((Me._SolutionDetailCompNatNumber = value) = False) Then
                    Me._SolutionDetailCompNatNumber = value
                    Me.SendPropertyChanged("SolutionDetailCompNatNumber")
                End If
            End Set
        End Property

        Private _SolutionDetailCompNatName As String = ""
        <DataMember()> _
        Public Property SolutionDetailCompNatName() As String
            Get
                Return Left(Me._SolutionDetailCompNatName, 40)
            End Get
            Set(value As String)
                If (String.Equals(Me._SolutionDetailCompNatName, value) = False) Then
                    Me._SolutionDetailCompNatName = Left(value, 40)
                    Me.SendPropertyChanged("SolutionDetailCompNatName")
                End If
            End Set
        End Property

        Private _SolutionDetailODControl As Integer = 0
        <DataMember()> _
        Public Property SolutionDetailODControl() As Integer
            Get
                Return Me._SolutionDetailODControl
            End Get
            Set(ByVal value As Integer)
                If ((Me._SolutionDetailODControl = value) = False) Then
                    Me._SolutionDetailODControl = value
                    Me.SendPropertyChanged("SolutionDetailODControl")
                End If
            End Set
        End Property

        Private _SolutionDetailCarrierControl As Integer = 0
        <DataMember()> _
        Public Property SolutionDetailCarrierControl() As Integer
            Get
                Return Me._SolutionDetailCarrierControl
            End Get
            Set(ByVal value As Integer)
                If ((Me._SolutionDetailCarrierControl = value) = False) Then
                    Me._SolutionDetailCarrierControl = value
                    Me.SendPropertyChanged("SolutionDetailCarrierControl")
                End If
            End Set
        End Property

        Private _SolutionDetailCarrierNumber As System.Nullable(Of Integer)
        <DataMember()> _
        Public Property SolutionDetailCarrierNumber() As System.Nullable(Of Integer)
            Get
                Return Me._SolutionDetailCarrierNumber
            End Get
            Set(ByVal value As System.Nullable(Of Integer))
                If (Me._SolutionDetailCarrierNumber.Equals(value) = False) Then
                    Me._SolutionDetailCarrierNumber = value
                    Me.SendPropertyChanged("SolutionDetailCarrierNumber")
                End If
            End Set
        End Property

        Private _SolutionDetailCarrierName As String = ""
        <DataMember()> _
        Public Property SolutionDetailCarrierName() As String
            Get
                Return Left(Me._SolutionDetailCarrierName, 40)
            End Get
            Set(value As String)
                If (String.Equals(Me._SolutionDetailCarrierName, value) = False) Then
                    Me._SolutionDetailCarrierName = Left(value, 40)
                    Me.SendPropertyChanged("SolutionDetailCarrierName")
                End If
            End Set
        End Property

        Private _SolutionDetailOrigCompControl As Integer = 0
        <DataMember()> _
        Public Property SolutionDetailOrigCompControl() As Integer
            Get
                Return Me._SolutionDetailOrigCompControl
            End Get
            Set(ByVal value As Integer)
                If ((Me._SolutionDetailOrigCompControl = value) = False) Then
                    Me._SolutionDetailOrigCompControl = value
                    Me.SendPropertyChanged("SolutionDetailOrigCompControl")
                End If
            End Set
        End Property

        Private _SolutionDetailOrigName As String = ""
        <DataMember()> _
        Public Property SolutionDetailOrigName() As String
            Get
                Return Left(Me._SolutionDetailOrigName, 40)
            End Get
            Set(value As String)
                If (String.Equals(Me._SolutionDetailOrigName, value) = False) Then
                    Me._SolutionDetailOrigName = Left(value, 40)
                    Me.SendPropertyChanged("SolutionDetailOrigName")
                End If
            End Set
        End Property

        Private _SolutionDetailOrigAddress1 As String = ""
        <DataMember()> _
        Public Property SolutionDetailOrigAddress1() As String
            Get
                Return Left(Me._SolutionDetailOrigAddress1, 40)
            End Get
            Set(value As String)
                If (String.Equals(Me._SolutionDetailOrigAddress1, value) = False) Then
                    Me._SolutionDetailOrigAddress1 = Left(value, 40)
                    Me.SendPropertyChanged("SolutionDetailOrigAddress1")
                End If
            End Set
        End Property

        Private _SolutionDetailOrigAddress2 As String = ""
        <DataMember()> _
        Public Property SolutionDetailOrigAddress2() As String
            Get
                Return Left(Me._SolutionDetailOrigAddress2, 40)
            End Get
            Set(value As String)
                If (String.Equals(Me._SolutionDetailOrigAddress2, value) = False) Then
                    Me._SolutionDetailOrigAddress2 = Left(value, 40)
                    Me.SendPropertyChanged("SolutionDetailOrigAddress2")
                End If
            End Set
        End Property

        Private _SolutionDetailOrigAddress3 As String = ""
        <DataMember()> _
        Public Property SolutionDetailOrigAddress3() As String
            Get
                Return Left(Me._SolutionDetailOrigAddress3, 40)
            End Get
            Set(value As String)
                If (String.Equals(Me._SolutionDetailOrigAddress3, value) = False) Then
                    Me._SolutionDetailOrigAddress3 = Left(value, 40)
                    Me.SendPropertyChanged("SolutionDetailOrigAddress3")
                End If
            End Set
        End Property

        Private _SolutionDetailOrigCity As String = ""
        <DataMember()> _
        Public Property SolutionDetailOrigCity() As String
            Get
                Return Left(Me._SolutionDetailOrigCity, 25)
            End Get
            Set(value As String)
                If (String.Equals(Me._SolutionDetailOrigCity, value) = False) Then
                    Me._SolutionDetailOrigCity = Left(value, 25)
                    Me.SendPropertyChanged("SolutionDetailOrigCity")
                End If
            End Set
        End Property

        Private _SolutionDetailOrigState As String = ""
        <DataMember()> _
        Public Property SolutionDetailOrigState() As String
            Get
                Return Left(Me._SolutionDetailOrigState, 8)
            End Get
            Set(value As String)
                If (String.Equals(Me._SolutionDetailOrigState, value) = False) Then
                    Me._SolutionDetailOrigState = Left(value, 8)
                    Me.SendPropertyChanged("SolutionDetailOrigState")
                End If
            End Set
        End Property

        Private _SolutionDetailOrigCountry As String = ""
        <DataMember()> _
        Public Property SolutionDetailOrigCountry() As String
            Get
                Return Left(Me._SolutionDetailOrigCountry, 30)
            End Get
            Set(value As String)
                If (String.Equals(Me._SolutionDetailOrigCountry, value) = False) Then
                    Me._SolutionDetailOrigCountry = Left(value, 30)
                    Me.SendPropertyChanged("SolutionDetailOrigCountry")
                End If
            End Set
        End Property

        Private _SolutionDetailOrigZip As String = ""
        <DataMember()> _
        Public Property SolutionDetailOrigZip() As String
            Get
                Return Left(Me._SolutionDetailOrigZip, 10)
            End Get
            Set(value As String)
                If (String.Equals(Me._SolutionDetailOrigZip, value) = False) Then
                    Me._SolutionDetailOrigZip = Left(value, 10)
                    Me.SendPropertyChanged("SolutionDetailOrigZip")
                End If
            End Set
        End Property

        Private _SolutionDetailDestCompControl As Integer = 0
        <DataMember()> _
        Public Property SolutionDetailDestCompControl() As Integer
            Get
                Return Me._SolutionDetailDestCompControl
            End Get
            Set(ByVal value As Integer)
                If ((Me._SolutionDetailDestCompControl = value) = False) Then
                    Me._SolutionDetailDestCompControl = value
                    Me.SendPropertyChanged("SolutionDetailDestCompControl")
                End If
            End Set
        End Property

        Private _SolutionDetailDestName As String = ""
        <DataMember()> _
        Public Property SolutionDetailDestName() As String
            Get
                Return Left(Me._SolutionDetailDestName, 40)
            End Get
            Set(value As String)
                If (String.Equals(Me._SolutionDetailDestName, value) = False) Then
                    Me._SolutionDetailDestName = Left(value, 40)
                    Me.SendPropertyChanged("SolutionDetailDestName")
                End If
            End Set
        End Property

        Private _SolutionDetailDestAddress1 As String = ""
        <DataMember()> _
        Public Property SolutionDetailDestAddress1() As String
            Get
                Return Left(Me._SolutionDetailDestAddress1, 40)
            End Get
            Set(value As String)
                If (String.Equals(Me._SolutionDetailDestAddress1, value) = False) Then
                    Me._SolutionDetailDestAddress1 = Left(value, 40)
                    Me.SendPropertyChanged("SolutionDetailDestAddress1")
                End If
            End Set
        End Property

        Private _SolutionDetailDestAddress2 As String = ""
        <DataMember()> _
        Public Property SolutionDetailDestAddress2() As String
            Get
                Return Left(Me._SolutionDetailDestAddress2, 40)
            End Get
            Set(value As String)
                If (String.Equals(Me._SolutionDetailDestAddress2, value) = False) Then
                    Me._SolutionDetailDestAddress2 = Left(value, 40)
                    Me.SendPropertyChanged("SolutionDetailDestAddress2")
                End If
            End Set
        End Property

        Private _SolutionDetailDestAddress3 As String = ""
        <DataMember()> _
        Public Property SolutionDetailDestAddress3() As String
            Get
                Return Left(Me._SolutionDetailDestAddress3, 40)
            End Get
            Set(value As String)
                If (String.Equals(Me._SolutionDetailDestAddress3, value) = False) Then
                    Me._SolutionDetailDestAddress3 = Left(value, 40)
                    Me.SendPropertyChanged("SolutionDetailDestAddress3")
                End If
            End Set
        End Property

        Private _SolutionDetailDestCity As String = ""
        <DataMember()> _
        Public Property SolutionDetailDestCity() As String
            Get
                Return Left(Me._SolutionDetailDestCity, 25)
            End Get
            Set(value As String)
                If (String.Equals(Me._SolutionDetailDestCity, value) = False) Then
                    Me._SolutionDetailDestCity = Left(value, 25)
                    Me.SendPropertyChanged("SolutionDetailDestCity")
                End If
            End Set
        End Property

        Private _SolutionDetailDestState As String = ""
        <DataMember()> _
        Public Property SolutionDetailDestState() As String
            Get
                Return Left(Me._SolutionDetailDestState, 8)
            End Get
            Set(value As String)
                If (String.Equals(Me._SolutionDetailDestState, value) = False) Then
                    Me._SolutionDetailDestState = Left(value, 8)
                    Me.SendPropertyChanged("SolutionDetailDestState")
                End If
            End Set
        End Property

        Private _SolutionDetailDestCountry As String = ""
        <DataMember()> _
        Public Property SolutionDetailDestCountry() As String
            Get
                Return Left(Me._SolutionDetailDestCountry, 30)
            End Get
            Set(value As String)
                If (String.Equals(Me._SolutionDetailDestCountry, value) = False) Then
                    Me._SolutionDetailDestCountry = Left(value, 30)
                    Me.SendPropertyChanged("SolutionDetailDestCountry")
                End If
            End Set
        End Property

        Private _SolutionDetailDestZip As String = ""
        <DataMember()> _
        Public Property SolutionDetailDestZip() As String
            Get
                Return Left(Me._SolutionDetailDestZip, 10)
            End Get
            Set(value As String)
                If (String.Equals(Me._SolutionDetailDestZip, value) = False) Then
                    Me._SolutionDetailDestZip = Left(value, 10)
                    Me.SendPropertyChanged("SolutionDetailDestZip")
                End If
            End Set
        End Property

        Private _SolutionDetailBookFinARInvoiceDate As System.Nullable(Of Date)
        <DataMember()> _
        Public Property SolutionDetailBookFinARInvoiceDate() As System.Nullable(Of Date)
            Get
                Return Me._SolutionDetailBookFinARInvoiceDate
            End Get
            Set(value As System.Nullable(Of Date))
                If (Me._SolutionDetailBookFinARInvoiceDate.Equals(value) = False) Then
                    Me._SolutionDetailBookFinARInvoiceDate = value
                    Me.SendPropertyChanged("SolutionDetailBookFinARInvoiceDate")
                End If
            End Set
        End Property

        Private _SolutionDetailDateOrdered As System.Nullable(Of Date)
        <DataMember()> _
        Public Property SolutionDetailDateOrdered() As System.Nullable(Of Date)
            Get
                Return Me._SolutionDetailDateOrdered
            End Get
            Set(value As System.Nullable(Of Date))
                If (Me._SolutionDetailDateOrdered.Equals(value) = False) Then
                    Me._SolutionDetailDateOrdered = value
                    Me.SendPropertyChanged("SolutionDetailDateOrdered")
                End If
            End Set
        End Property

        Private _SolutionDetailDateLoad As System.Nullable(Of Date)
        <DataMember()> _
        Public Property SolutionDetailDateLoad() As System.Nullable(Of Date)
            Get
                Return Me._SolutionDetailDateLoad
            End Get
            Set(value As System.Nullable(Of Date))
                If (Me._SolutionDetailDateLoad.Equals(value) = False) Then
                    Me._SolutionDetailDateLoad = value
                    Me.SendPropertyChanged("SolutionDetailDateLoad")
                End If
            End Set
        End Property

        Private _SolutionDetailDateRequired As System.Nullable(Of Date)
        <DataMember()> _
        Public Property SolutionDetailDateRequired() As System.Nullable(Of Date)
            Get
                Return Me._SolutionDetailDateRequired
            End Get
            Set(value As System.Nullable(Of Date))
                If (Me._SolutionDetailDateRequired.Equals(value) = False) Then
                    Me._SolutionDetailDateRequired = value
                    Me.SendPropertyChanged("SolutionDetailDateRequired")
                End If
            End Set
        End Property

        Private _SolutionDetailTotalCases As Integer = 0
        <DataMember()> _
        Public Property SolutionDetailTotalCases() As Integer
            Get
                Return Me._SolutionDetailTotalCases
            End Get
            Set(ByVal value As Integer)
                If ((Me._SolutionDetailTotalCases = value) = False) Then
                    Me._SolutionDetailTotalCases = value
                    Me.SendPropertyChanged("SolutionDetailTotalCases")
                End If
            End Set
        End Property

        Private _SolutionDetailTotalWgt As Double = 0
        <DataMember()> _
        Public Property SolutionDetailTotalWgt() As Double
            Get
                Return Me._SolutionDetailTotalWgt
            End Get
            Set(ByVal value As Double)
                If ((Me._SolutionDetailTotalWgt = value) = False) Then
                    Me._SolutionDetailTotalWgt = value
                    Me.SendPropertyChanged("SolutionDetailTotalWgt")
                End If
            End Set
        End Property

        Private _SolutionDetailTotalPL As Double = 0
        <DataMember()> _
        Public Property SolutionDetailTotalPL() As Double
            Get
                Return Me._SolutionDetailTotalPL
            End Get
            Set(ByVal value As Double)
                If ((Me._SolutionDetailTotalPL = value) = False) Then
                    Me._SolutionDetailTotalPL = value
                    Me.SendPropertyChanged("SolutionDetailTotalPL")
                End If
            End Set
        End Property

        Private _SolutionDetailTotalCube As Integer = 0
        <DataMember()> _
        Public Property SolutionDetailTotalCube() As Integer
            Get
                Return Me._SolutionDetailTotalCube
            End Get
            Set(ByVal value As Integer)
                If ((Me._SolutionDetailTotalCube = value) = False) Then
                    Me._SolutionDetailTotalCube = value
                    Me.SendPropertyChanged("SolutionDetailTotalCube")
                End If
            End Set
        End Property

        Private _SolutionDetailTotalPX As Integer = 0
        <DataMember()> _
        Public Property SolutionDetailTotalPX() As Integer
            Get
                Return Me._SolutionDetailTotalPX
            End Get
            Set(ByVal value As Integer)
                If ((Me._SolutionDetailTotalPX = value) = False) Then
                    Me._SolutionDetailTotalPX = value
                    Me.SendPropertyChanged("SolutionDetailTotalPX")
                End If
            End Set
        End Property

        Private _SolutionDetailTotalBFC As Decimal = 0
        <DataMember()> _
        Public Property SolutionDetailTotalBFC() As Decimal
            Get
                Return Me._SolutionDetailTotalBFC
            End Get
            Set(ByVal value As Decimal)
                If ((Me._SolutionDetailTotalBFC = value) = False) Then
                    Me._SolutionDetailTotalBFC = value
                    Me.SendPropertyChanged("SolutionDetailTotalBFC")
                End If
            End Set
        End Property

        Private _SolutionDetailTranCode As String = ""
        <DataMember()> _
        Public Property SolutionDetailTranCode() As String
            Get
                Return Left(Me._SolutionDetailTranCode, 3)
            End Get
            Set(value As String)
                If (String.Equals(Me._SolutionDetailTranCode, value) = False) Then
                    Me._SolutionDetailTranCode = Left(value, 3)
                    Me.SendPropertyChanged("SolutionDetailTranCode")
                End If
            End Set
        End Property

        Private _SolutionDetailPayCode As String = ""
        <DataMember()> _
        Public Property SolutionDetailPayCode() As String
            Get
                Return Left(Me._SolutionDetailPayCode, 3)
            End Get
            Set(value As String)
                If (String.Equals(Me._SolutionDetailPayCode, value) = False) Then
                    Me._SolutionDetailPayCode = Left(value, 3)
                    Me.SendPropertyChanged("SolutionDetailPayCode")
                End If
            End Set
        End Property

        Private _SolutionDetailTypeCode As String = ""
        <DataMember()> _
        Public Property SolutionDetailTypeCode() As String
            Get
                Return Left(Me._SolutionDetailTypeCode, 20)
            End Get
            Set(value As String)
                If (String.Equals(Me._SolutionDetailTypeCode, value) = False) Then
                    Me._SolutionDetailTypeCode = Left(value, 20)
                    Me.SendPropertyChanged("SolutionDetailTypeCode")
                End If
            End Set
        End Property

        Private _SolutionDetailStopNo As Integer = 0
        <DataMember()> _
        Public Property SolutionDetailStopNo() As Integer
            Get
                Return Me._SolutionDetailStopNo
            End Get
            Set(ByVal value As Integer)
                If ((Me._SolutionDetailStopNo = value) = False) Then
                    Me._SolutionDetailStopNo = value
                    Me.SendPropertyChanged("SolutionDetailStopNo")
                End If
            End Set
        End Property

        Private _SolutionDetailRevBilledBFC As Decimal = 0
        <DataMember()> _
        Public Property SolutionDetailRevBilledBFC() As Decimal
            Get
                Return Me._SolutionDetailRevBilledBFC
            End Get
            Set(ByVal value As Decimal)
                If ((Me._SolutionDetailRevBilledBFC = value) = False) Then
                    Me._SolutionDetailRevBilledBFC = value
                    Me.SendPropertyChanged("SolutionDetailRevBilledBFC")
                End If
            End Set
        End Property

        Private _SolutionDetailRevCarrierCost As Decimal = 0
        <DataMember()> _
        Public Property SolutionDetailRevCarrierCost() As Decimal
            Get
                Return Me._SolutionDetailRevCarrierCost
            End Get
            Set(ByVal value As Decimal)
                If ((Me._SolutionDetailRevCarrierCost = value) = False) Then
                    Me._SolutionDetailRevCarrierCost = value
                    Me.SendPropertyChanged("SolutionDetailRevCarrierCost")
                End If
            End Set
        End Property

        Private _SolutionDetailRevOtherCost As Decimal = 0
        <DataMember()> _
        Public Property SolutionDetailRevOtherCost() As Decimal
            Get
                Return Me._SolutionDetailRevOtherCost
            End Get
            Set(ByVal value As Decimal)
                If ((Me._SolutionDetailRevOtherCost = value) = False) Then
                    Me._SolutionDetailRevOtherCost = value
                    Me.SendPropertyChanged("SolutionDetailRevOtherCost")
                End If
            End Set
        End Property

        Private _SolutionDetailRevTotalCost As Decimal = 0
        <DataMember()> _
        Public Property SolutionDetailRevTotalCost() As Decimal
            Get
                Return Me._SolutionDetailRevTotalCost
            End Get
            Set(ByVal value As Decimal)
                If ((Me._SolutionDetailRevTotalCost = value) = False) Then
                    Me._SolutionDetailRevTotalCost = value
                    Me.SendPropertyChanged("SolutionDetailRevTotalCost")
                End If
            End Set
        End Property

        Private _SolutionDetailRevLoadSavings As Decimal = 0
        <DataMember()> _
        Public Property SolutionDetailRevLoadSavings() As Decimal
            Get
                Return Me._SolutionDetailRevLoadSavings
            End Get
            Set(ByVal value As Decimal)
                If ((Me._SolutionDetailRevLoadSavings = value) = False) Then
                    Me._SolutionDetailRevLoadSavings = value
                    Me.SendPropertyChanged("SolutionDetailRevLoadSavings")
                End If
            End Set
        End Property

        Private _SolutionDetailRevCommPercent As Double = 0
        <DataMember()> _
        Public Property SolutionDetailRevCommPercent() As Double
            Get
                Return Me._SolutionDetailRevCommPercent
            End Get
            Set(ByVal value As Double)
                If ((Me._SolutionDetailRevCommPercent = value) = False) Then
                    Me._SolutionDetailRevCommPercent = value
                    Me.SendPropertyChanged("SolutionDetailRevCommPercent")
                End If
            End Set
        End Property

        Private _SolutionDetailRevCommCost As Decimal = 0
        <DataMember()> _
        Public Property SolutionDetailRevCommCost() As Decimal
            Get
                Return Me._SolutionDetailRevCommCost
            End Get
            Set(ByVal value As Decimal)
                If ((Me._SolutionDetailRevCommCost = value) = False) Then
                    Me._SolutionDetailRevCommCost = value
                    Me.SendPropertyChanged("SolutionDetailRevCommCost")
                End If
            End Set
        End Property

        Private _SolutionDetailRevGrossRevenue As Decimal = 0
        <DataMember()> _
        Public Property SolutionDetailRevGrossRevenue() As Decimal
            Get
                Return Me._SolutionDetailRevGrossRevenue
            End Get
            Set(ByVal value As Decimal)
                If ((Me._SolutionDetailRevGrossRevenue = value) = False) Then
                    Me._SolutionDetailRevGrossRevenue = value
                    Me.SendPropertyChanged("SolutionDetailRevGrossRevenue")
                End If
            End Set
        End Property

        Private _SolutionDetailRevNegRevenue As Integer = 0
        <DataMember()> _
        Public Property SolutionDetailRevNegRevenue() As Integer
            Get
                Return Me._SolutionDetailRevNegRevenue
            End Get
            Set(ByVal value As Integer)
                If ((Me._SolutionDetailRevNegRevenue = value) = False) Then
                    Me._SolutionDetailRevNegRevenue = value
                    Me.SendPropertyChanged("SolutionDetailRevNegRevenue")
                End If
            End Set
        End Property

        Private _SolutionDetailMilesFrom As Double = 0
        <DataMember()> _
        Public Property SolutionDetailMilesFrom() As Double
            Get
                Return Me._SolutionDetailMilesFrom
            End Get
            Set(ByVal value As Double)
                If ((Me._SolutionDetailMilesFrom = value) = False) Then
                    Me._SolutionDetailMilesFrom = value
                    Me.SendPropertyChanged("SolutionDetailMilesFrom")
                End If
            End Set
        End Property

        Private _SolutionDetailHoldLoad As Integer = 0
        <DataMember()> _
        Public Property SolutionDetailHoldLoad() As Integer
            Get
                Return Me._SolutionDetailHoldLoad
            End Get
            Set(ByVal value As Integer)
                If ((Me._SolutionDetailHoldLoad = value) = False) Then
                    Me._SolutionDetailHoldLoad = value
                    Me.SendPropertyChanged("SolutionDetailHoldLoad")
                End If
            End Set
        End Property

        Private _SolutionDetailTransType As String = ""
        <DataMember()> _
        Public Property SolutionDetailTransType() As String
            Get
                Return Left(Me._SolutionDetailTransType, 50)
            End Get
            Set(value As String)
                If (String.Equals(Me._SolutionDetailTransType, value) = False) Then
                    Me._SolutionDetailTransType = Left(value, 50)
                    Me.SendPropertyChanged("SolutionDetailTransType")
                End If
            End Set
        End Property


        Private _SolutionDetailRouteConsFlag As Boolean = True
        <DataMember()> _
        Public Property SolutionDetailRouteConsFlag() As Boolean
            Get
                Return Me._SolutionDetailRouteConsFlag
            End Get
            Set(ByVal value As Boolean)
                If ((Me._SolutionDetailRouteConsFlag = value) = False) Then
                    Me._SolutionDetailRouteConsFlag = value
                    Me.SendPropertyChanged("SolutionDetailRouteConsFlag")
                End If
            End Set
        End Property

        Private _SolutionDetailFinAPActTax As Decimal = 0
        <DataMember()> _
        Public Property SolutionDetailFinAPActTax() As Decimal
            Get
                Return Me._SolutionDetailFinAPActTax
            End Get
            Set(ByVal value As Decimal)
                If ((Me._SolutionDetailFinAPActTax = value) = False) Then
                    Me._SolutionDetailFinAPActTax = value
                    Me.SendPropertyChanged("SolutionDetailFinAPActTax")
                End If
            End Set
        End Property

        Private _SolutionDetailRevFreightTax As Decimal = 0
        <DataMember()> _
        Public Property SolutionDetailRevFreightTax() As Decimal
            Get
                Return Me._SolutionDetailRevFreightTax
            End Get
            Set(ByVal value As Decimal)
                If ((Me._SolutionDetailRevFreightTax = value) = False) Then
                    Me._SolutionDetailRevFreightTax = value
                    Me.SendPropertyChanged("SolutionDetailRevFreightTax")
                End If
            End Set
        End Property

        Private _SolutionDetailRevNetCost As Decimal = 0
        <DataMember()> _
        Public Property SolutionDetailRevNetCost() As Decimal
            Get
                Return Me._SolutionDetailRevNetCost
            End Get
            Set(ByVal value As Decimal)
                If ((Me._SolutionDetailRevNetCost = value) = False) Then
                    Me._SolutionDetailRevNetCost = value
                    Me.SendPropertyChanged("SolutionDetailRevNetCost")
                End If
            End Set
        End Property

        Private _SolutionDetailFinServiceFee As Decimal = 0
        <DataMember()> _
        Public Property SolutionDetailFinServiceFee() As Decimal
            Get
                Return Me._SolutionDetailFinServiceFee
            End Get
            Set(ByVal value As Decimal)
                If ((Me._SolutionDetailFinServiceFee = value) = False) Then
                    Me._SolutionDetailFinServiceFee = value
                    Me.SendPropertyChanged("SolutionDetailFinServiceFee")
                End If
            End Set
        End Property

        Private _SolutionDetailDateRequested As System.Nullable(Of Date)
        <DataMember()> _
        Public Property SolutionDetailDateRequested() As System.Nullable(Of Date)
            Get
                Return Me._SolutionDetailDateRequested
            End Get
            Set(value As System.Nullable(Of Date))
                If (Me._SolutionDetailDateRequested.Equals(value) = False) Then
                    Me._SolutionDetailDateRequested = value
                    Me.SendPropertyChanged("SolutionDetailDateRequested")
                End If
            End Set
        End Property

        Private _SolutionDetailCarrierEquipmentCodes As String = ""
        <DataMember()> _
        Public Property SolutionDetailCarrierEquipmentCodes() As String
            Get
                Return Left(Me._SolutionDetailCarrierEquipmentCodes, 50)
            End Get
            Set(value As String)
                If (String.Equals(Me._SolutionDetailCarrierEquipmentCodes, value) = False) Then
                    Me._SolutionDetailCarrierEquipmentCodes = Left(value, 50)
                    Me.SendPropertyChanged("SolutionDetailCarrierEquipmentCodes")
                End If
            End Set
        End Property

        Private _SolutionDetailLockAllCosts As Boolean = False
        <DataMember()> _
        Public Property SolutionDetailLockAllCosts() As Boolean
            Get
                Return Me._SolutionDetailLockAllCosts
            End Get
            Set(ByVal value As Boolean)
                If ((Me._SolutionDetailLockAllCosts = value) = False) Then
                    Me._SolutionDetailLockAllCosts = value
                    Me.SendPropertyChanged("SolutionDetailLockAllCosts")
                End If
            End Set
        End Property

        Private _SolutionDetailLockBFCCost As Boolean = False
        <DataMember()> _
        Public Property SolutionDetailLockBFCCost() As Boolean
            Get
                Return Me._SolutionDetailLockBFCCost
            End Get
            Set(ByVal value As Boolean)
                If ((Me._SolutionDetailLockBFCCost = value) = False) Then
                    Me._SolutionDetailLockBFCCost = value
                    Me.SendPropertyChanged("SolutionDetailLockBFCCost")
                End If
            End Set
        End Property

        Private _SolutionDetailRevNonTaxable As Decimal = 0
        <DataMember()> _
        Public Property SolutionDetailRevNonTaxable() As Decimal
            Get
                Return Me._SolutionDetailRevNonTaxable
            End Get
            Set(ByVal value As Decimal)
                If ((Me._SolutionDetailRevNonTaxable = value) = False) Then
                    Me._SolutionDetailRevNonTaxable = value
                    Me.SendPropertyChanged("SolutionDetailRevNonTaxable")
                End If
            End Set
        End Property

        Private _SolutionDetailPickupStopNumber As Integer = 0
        <DataMember()> _
        Public Property SolutionDetailPickupStopNumber() As Integer
            Get
                Return Me._SolutionDetailPickupStopNumber
            End Get
            Set(ByVal value As Integer)
                If ((Me._SolutionDetailPickupStopNumber = value) = False) Then
                    Me._SolutionDetailPickupStopNumber = value
                    Me.SendPropertyChanged("SolutionDetailPickupStopNumber")
                End If
            End Set
        End Property

        Private _SolutionDetailOrigStopNumber As Integer = 0
        <DataMember()> _
        Public Property SolutionDetailOrigStopNumber() As Integer
            Get
                Return Me._SolutionDetailOrigStopNumber
            End Get
            Set(ByVal value As Integer)
                If ((Me._SolutionDetailOrigStopNumber = value) = False) Then
                    Me._SolutionDetailOrigStopNumber = value
                    Me.SendPropertyChanged("SolutionDetailOrigStopNumber")
                End If
            End Set
        End Property

        Private _SolutionDetailDestStopNumber As Integer = 0
        <DataMember()> _
        Public Property SolutionDetailDestStopNumber() As Integer
            Get
                Return Me._SolutionDetailDestStopNumber
            End Get
            Set(ByVal value As Integer)
                If ((Me._SolutionDetailDestStopNumber = value) = False) Then
                    Me._SolutionDetailDestStopNumber = value
                    Me.SendPropertyChanged("SolutionDetailDestStopNumber")
                End If
            End Set
        End Property

        Private _SolutionDetailOrigMiles As Double = 0
        <DataMember()> _
        Public Property SolutionDetailOrigMiles() As Double
            Get
                Return Me._SolutionDetailOrigMiles
            End Get
            Set(ByVal value As Double)
                If ((Me._SolutionDetailOrigMiles = value) = False) Then
                    Me._SolutionDetailOrigMiles = value
                    Me.SendPropertyChanged("SolutionDetailOrigMiles")
                End If
            End Set
        End Property

        Private _SolutionDetailDestMiles As Double = 0
        <DataMember()> _
        Public Property SolutionDetailDestMiles() As Double
            Get
                Return Me._SolutionDetailDestMiles
            End Get
            Set(ByVal value As Double)
                If ((Me._SolutionDetailDestMiles = value) = False) Then
                    Me._SolutionDetailDestMiles = value
                    Me.SendPropertyChanged("SolutionDetailDestMiles")
                End If
            End Set
        End Property

        Private _SolutionDetailOrigPCMCost As Double = 0
        <DataMember()> _
        Public Property SolutionDetailOrigPCMCost() As Double
            Get
                Return Me._SolutionDetailOrigPCMCost
            End Get
            Set(ByVal value As Double)
                If ((Me._SolutionDetailOrigPCMCost = value) = False) Then
                    Me._SolutionDetailOrigPCMCost = value
                    Me.SendPropertyChanged("SolutionDetailOrigPCMCost")
                End If
            End Set
        End Property

        Private _SolutionDetailDestPCMCost As Double = 0
        <DataMember()> _
        Public Property SolutionDetailDestPCMCost() As Double
            Get
                Return Me._SolutionDetailDestPCMCost
            End Get
            Set(ByVal value As Double)
                If ((Me._SolutionDetailDestPCMCost = value) = False) Then
                    Me._SolutionDetailDestPCMCost = value
                    Me.SendPropertyChanged("SolutionDetailDestPCMCost")
                End If
            End Set
        End Property

        Private _SolutionDetailOrigPCMTime As String = ""
        <DataMember()> _
        Public Property SolutionDetailOrigPCMTime() As String
            Get
                Return Left(Me._SolutionDetailOrigPCMTime, 40)
            End Get
            Set(value As String)
                If (String.Equals(Me._SolutionDetailOrigPCMTime, value) = False) Then
                    Me._SolutionDetailOrigPCMTime = Left(value, 40)
                    Me.SendPropertyChanged("SolutionDetailOrigPCMTime")
                End If
            End Set
        End Property

        Private _SolutionDetailDestPCMTime As String = ""
        <DataMember()> _
        Public Property SolutionDetailDestPCMTime() As String
            Get
                Return Left(Me._SolutionDetailDestPCMTime, 40)
            End Get
            Set(value As String)
                If (String.Equals(Me._SolutionDetailDestPCMTime, value) = False) Then
                    Me._SolutionDetailDestPCMTime = Left(value, 40)
                    Me.SendPropertyChanged("SolutionDetailDestPCMTime")
                End If
            End Set
        End Property

        Private _SolutionDetailOrigPCMTolls As Decimal = 0
        <DataMember()> _
        Public Property SolutionDetailOrigPCMTolls() As Decimal
            Get
                Return Me._SolutionDetailOrigPCMTolls
            End Get
            Set(ByVal value As Decimal)
                If ((Me._SolutionDetailOrigPCMTolls = value) = False) Then
                    Me._SolutionDetailOrigPCMTolls = value
                    Me.SendPropertyChanged("SolutionDetailOrigPCMTolls")
                End If
            End Set
        End Property

        Private _SolutionDetailDestPCMTolls As Decimal = 0
        <DataMember()> _
        Public Property SolutionDetailDestPCMTolls() As Decimal
            Get
                Return Me._SolutionDetailDestPCMTolls
            End Get
            Set(ByVal value As Decimal)
                If ((Me._SolutionDetailDestPCMTolls = value) = False) Then
                    Me._SolutionDetailDestPCMTolls = value
                    Me.SendPropertyChanged("SolutionDetailDestPCMTolls")
                End If
            End Set
        End Property

        Private _SolutionDetailOrigPCMESTCHG As Double = 0
        <DataMember()> _
        Public Property SolutionDetailOrigPCMESTCHG() As Double
            Get
                Return Me._SolutionDetailOrigPCMESTCHG
            End Get
            Set(ByVal value As Double)
                If ((Me._SolutionDetailOrigPCMESTCHG = value) = False) Then
                    Me._SolutionDetailOrigPCMESTCHG = value
                    Me.SendPropertyChanged("SolutionDetailOrigPCMESTCHG")
                End If
            End Set
        End Property

        Private _SolutionDetailDestPCMESTCHG As Double = 0
        <DataMember()> _
        Public Property SolutionDetailDestPCMESTCHG() As Double
            Get
                Return Me._SolutionDetailDestPCMESTCHG
            End Get
            Set(ByVal value As Double)
                If ((Me._SolutionDetailDestPCMESTCHG = value) = False) Then
                    Me._SolutionDetailDestPCMESTCHG = value
                    Me.SendPropertyChanged("SolutionDetailDestPCMESTCHG")
                End If
            End Set
        End Property

        Private _SolutionDetailPickNumber As Integer = 0
        <DataMember()> _
        Public Property SolutionDetailPickNumber() As Integer
            Get
                Return Me._SolutionDetailPickNumber
            End Get
            Set(ByVal value As Integer)
                If ((Me._SolutionDetailPickNumber = value) = False) Then
                    Me._SolutionDetailPickNumber = value
                    Me.SendPropertyChanged("SolutionDetailPickNumber")
                End If
            End Set
        End Property

        Private _SolutionDetailOrigStopControl As Integer = 0
        <DataMember()> _
        Public Property SolutionDetailOrigStopControl() As Integer
            Get
                Return Me._SolutionDetailOrigStopControl
            End Get
            Set(ByVal value As Integer)
                If ((Me._SolutionDetailOrigStopControl = value) = False) Then
                    Me._SolutionDetailOrigStopControl = value
                    Me.SendPropertyChanged("SolutionDetailOrigStopControl")
                End If
            End Set
        End Property

        Private _SolutionDetailDestStopControl As Integer = 0
        <DataMember()> _
        Public Property SolutionDetailDestStopControl() As Integer
            Get
                Return Me._SolutionDetailDestStopControl
            End Get
            Set(ByVal value As Integer)
                If ((Me._SolutionDetailDestStopControl = value) = False) Then
                    Me._SolutionDetailDestStopControl = value
                    Me.SendPropertyChanged("SolutionDetailDestStopControl")
                End If
            End Set
        End Property

        Private _SolutionDetailRouteTypeCode As Integer = 0
        <DataMember()> _
        Public Property SolutionDetailRouteTypeCode() As Integer
            Get
                Return Me._SolutionDetailRouteTypeCode
            End Get
            Set(ByVal value As Integer)
                If ((Me._SolutionDetailRouteTypeCode = value) = False) Then
                    Me._SolutionDetailRouteTypeCode = value
                    Me.SendPropertyChanged("SolutionDetailRouteTypeCode")
                End If
            End Set
        End Property


        Private _SolutionDetailAlternateAddressLaneControl As Integer
        <DataMember()> _
        Public Property SolutionDetailAlternateAddressLaneControl As Integer
            Get
                Return _SolutionDetailAlternateAddressLaneControl
            End Get
            Set(value As Integer)
                If ((Me._SolutionDetailAlternateAddressLaneControl = value) = False) Then
                    Me._SolutionDetailAlternateAddressLaneControl = value
                    Me.SendPropertyChanged("SolutionDetailAlternateAddressLaneControl")
                End If

            End Set
        End Property

        Private _SolutionDetailAlternateAddressLaneNumber As String = ""
        <DataMember()> _
        Public Property SolutionDetailAlternateAddressLaneNumber() As String
            Get
                Return Left(Me._SolutionDetailAlternateAddressLaneNumber, 50)
            End Get
            Set(value As String)
                If (String.Equals(Me._SolutionDetailAlternateAddressLaneNumber, value) = False) Then
                    Me._SolutionDetailAlternateAddressLaneNumber = Left(value, 50)
                    Me.SendPropertyChanged("SolutionDetailAlternateAddressLaneNumber")
                End If
            End Set
        End Property

        Private _SolutionDetailDefaultRouteSequence As Integer
        <DataMember()> _
        Public Property SolutionDetailDefaultRouteSequence As Integer
            Get
                Return _SolutionDetailDefaultRouteSequence
            End Get
            Set(value As Integer)
                If ((Me._SolutionDetailDefaultRouteSequence = value) = False) Then
                    Me._SolutionDetailDefaultRouteSequence = value
                    Me.SendPropertyChanged("SolutionDetailDefaultRouteSequence")
                End If

            End Set
        End Property

        Private _SolutionDetailRouteGuideControl As Integer
        <DataMember()> _
        Public Property SolutionDetailRouteGuideControl As Integer
            Get
                Return _SolutionDetailRouteGuideControl
            End Get
            Set(value As Integer)
                If ((Me._SolutionDetailRouteGuideControl = value) = False) Then
                    Me._SolutionDetailRouteGuideControl = value
                    Me.SendPropertyChanged("SolutionDetailRouteGuideControl")
                End If

            End Set
        End Property

        Private _SolutionDetailRouteGuideNumber As String = ""
        <DataMember()> _
        Public Property SolutionDetailRouteGuideNumber() As String
            Get
                Return Left(Me._SolutionDetailRouteGuideNumber, 50)
            End Get
            Set(value As String)
                If (String.Equals(Me._SolutionDetailRouteGuideNumber, value) = False) Then
                    Me._SolutionDetailRouteGuideNumber = Left(value, 50)
                    Me.SendPropertyChanged("SolutionDetailRouteGuideNumber")
                End If
            End Set
        End Property

        Private _SolutionDetailCustomerApprovalTransmitted As Boolean
        <DataMember()> _
        Public Property SolutionDetailCustomerApprovalTransmitted As Boolean
            Get
                Return _SolutionDetailCustomerApprovalTransmitted
            End Get
            Set(value As Boolean)
                If ((Me._SolutionDetailCustomerApprovalTransmitted = value) = False) Then
                    Me._SolutionDetailCustomerApprovalTransmitted = value
                    Me.SendPropertyChanged("SolutionDetailCustomerApprovalTransmitted")
                End If

            End Set
        End Property

        Private _SolutionDetailCustomerApprovalRecieved As Boolean
        <DataMember()> _
        Public Property SolutionDetailCustomerApprovalRecieved As Boolean
            Get
                Return _SolutionDetailCustomerApprovalRecieved
            End Get
            Set(value As Boolean)
                If ((Me._SolutionDetailCustomerApprovalRecieved = value) = False) Then
                    Me._SolutionDetailCustomerApprovalRecieved = value
                    Me.SendPropertyChanged("SolutionDetailCustomerApprovalRecieved")
                End If

            End Set
        End Property

        Private _SolutionDetailIsHazmat As Boolean = True
        <DataMember()> _
        Public Property SolutionDetailIsHazmat() As Boolean
            Get
                Return Me._SolutionDetailIsHazmat
            End Get
            Set(ByVal value As Boolean)
                If ((Me._SolutionDetailIsHazmat = value) = False) Then
                    Me._SolutionDetailIsHazmat = value
                    Me.SendPropertyChanged("SolutionDetailIsHazmat")
                End If
            End Set
        End Property

        Private _SolutionDetailInbound As Boolean = True
        <DataMember()> _
        Public Property SolutionDetailInbound() As Boolean
            Get
                Return Me._SolutionDetailInbound
            End Get
            Set(ByVal value As Boolean)
                If ((Me._SolutionDetailInbound = value) = False) Then
                    Me._SolutionDetailInbound = value
                    Me.SendPropertyChanged("SolutionDetailInbound")
                End If
            End Set
        End Property

        Private _SolutionDetailBookCarrTarControl As Integer = 0
        <DataMember()> _
        Public Property SolutionDetailBookCarrTarControl() As Integer
            Get
                Return _SolutionDetailBookCarrTarControl
            End Get
            Set(ByVal value As Integer)
                _SolutionDetailBookCarrTarControl = value
            End Set
        End Property

        Private _SolutionDetailBookSHID As String
        <DataMember()> _
        Public Property SolutionDetailBookSHID() As String
            Get
                Return Left(Me._SolutionDetailBookSHID, 50)
            End Get
            Set(ByVal value As String)
                If (String.Equals(Me._SolutionDetailBookSHID, value) = False) Then
                    Me._SolutionDetailBookSHID = Left(value, 50)
                    Me.SendPropertyChanged("SolutionDetailBookSHID")
                End If
            End Set
        End Property

        Private _SolutionDetailRouteTypeName As String
        <DataMember()> _
        Public Property SolutionDetailRouteTypeName() As String
            Get
                Return Left(Me._SolutionDetailRouteTypeName, 50)
            End Get
            Set(ByVal value As String)
                If (String.Equals(Me._SolutionDetailRouteTypeName, value) = False) Then
                    Me._SolutionDetailRouteTypeName = Left(value, 50)
                    Me.SendPropertyChanged("SolutionDetailRouteTypeName")
                End If
            End Set
        End Property

        Private _SolutionDetailLaneNumber As String
        <DataMember()> _
        Public Property SolutionDetailLaneNumber() As String
            Get
                Return Left(Me._SolutionDetailLaneNumber, 50)
            End Get
            Set(ByVal value As String)
                If (String.Equals(Me._SolutionDetailLaneNumber, value) = False) Then
                    Me._SolutionDetailLaneNumber = Left(value, 50)
                    Me.SendPropertyChanged("SolutionDetailLaneNumber")
                End If
            End Set
        End Property

        Private _SolutionDetailLaneName As String
        <DataMember()> _
        Public Property SolutionDetailLaneName() As String
            Get
                Return Left(Me._SolutionDetailLaneName, 50)
            End Get
            Set(ByVal value As String)
                If (String.Equals(Me._SolutionDetailLaneName, value) = False) Then
                    Me._SolutionDetailLaneName = Left(value, 50)
                    Me.SendPropertyChanged("SolutionDetailLaneName")
                End If
            End Set
        End Property

        Private _SolutionDetailBookNotes As String
        <DataMember()> _
        Public Property SolutionDetailBookNotes() As String
            Get
                Return Me._SolutionDetailBookNotes
            End Get
            Set(ByVal value As String)
                If (String.Equals(Me._SolutionDetailBookNotes, value) = False) Then
                    Me._SolutionDetailBookNotes = value
                    Me.SendPropertyChanged("SolutionDetailBookNotes")
                End If
            End Set
        End Property

        Private _SolutionDetailBookModDate As System.Nullable(Of Date)
        <DataMember()> _
        Public Property SolutionDetailBookModDate() As System.Nullable(Of Date)
            Get
                Return _SolutionDetailBookModDate
            End Get
            Set(ByVal value As System.Nullable(Of Date))
                _SolutionDetailBookModDate = value
                Me.SendPropertyChanged("SolutionDetailBookModDate")
            End Set
        End Property

        Private _SolutionDetailBookModUser As String = ""
        <DataMember()> _
        Public Property SolutionDetailBookModUser() As String
            Get
                Return Left(_SolutionDetailBookModUser, 100)
            End Get
            Set(ByVal value As String)
                _SolutionDetailBookModUser = Left(value, 100)
                Me.SendPropertyChanged("SolutionDetailBookModUser")
            End Set
        End Property

        Private _SolutionDetailModDate As System.Nullable(Of Date)
        <DataMember()> _
        Public Property SolutionDetailModDate() As System.Nullable(Of Date)
            Get
                Return _SolutionDetailModDate
            End Get
            Set(ByVal value As System.Nullable(Of Date))
                _SolutionDetailModDate = value
                Me.SendPropertyChanged("SolutionDetailModDate")
            End Set
        End Property

        Private _SolutionDetailModUser As String = ""
        <DataMember()> _
        Public Property SolutionDetailModUser() As String
            Get
                Return Left(_SolutionDetailModUser, 100)
            End Get
            Set(ByVal value As String)
                _SolutionDetailModUser = Left(value, 100)
                Me.SendPropertyChanged("SolutionDetailModUser")
            End Set
        End Property

        Private _SolutionDetailUpdated As Byte()
        <DataMember()> _
        Public Property SolutionDetailUpdated() As Byte()
            Get
                Return _SolutionDetailUpdated
            End Get
            Set(ByVal value As Byte())
                _SolutionDetailUpdated = value
            End Set
        End Property


#End Region

#Region " Public Methods"
        Public Overrides Function Clone() As DTOBaseClass
            Dim instance As New tblSolutionDetail
            instance = DirectCast(MemberwiseClone(), tblSolutionDetail)
            instance.Details = Nothing
            For Each item In Details
                instance.Details.Add(DirectCast(item.Clone, BookItem))
            Next
            Return instance
        End Function

        Public Function CloneNoChildren() As DTOBaseClass
            Dim instance As New tblSolutionDetail
            instance = DirectCast(MemberwiseClone(), tblSolutionDetail)
            instance.Details = Nothing
            Return instance
        End Function

#End Region

    End Class
End Namespace