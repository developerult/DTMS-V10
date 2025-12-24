Imports Microsoft.VisualBasic
Imports System
Imports System.Collections.Generic
Imports System.Linq
Imports System.Text
Imports System.Runtime.Serialization
Imports Ngl.Core.ChangeTracker

Namespace DataTransferObjects
    <DataContract()> _
    Public Class PCMDataStop
        Inherits DTOBaseClass


#Region " Data Members"

       
        Private mintBookControl As Integer = 0
        <DataMember()> _
        Public Property BookControl() As Integer
            Get
                Return mintBookControl
            End Get
            Set(ByVal Value As Integer)
                mintBookControl = Value
                NotifyPropertyChanged("BookControl")
            End Set
        End Property

        Private mintBookCustCompControl As Integer = 0
        <DataMember()> _
        Public Property BookCustCompControl() As Integer
            Get
                Return mintBookCustCompControl
            End Get
            Set(ByVal Value As Integer)
                mintBookCustCompControl = Value
                NotifyPropertyChanged("BookCustCompControl")
            End Set
        End Property

        Private mintBookLoadControl As Integer = 0
        <DataMember()> _
        Public Property BookLoadControl() As Integer
            Get
                Return mintBookLoadControl
            End Get
            Set(ByVal Value As Integer)
                mintBookLoadControl = Value
                NotifyPropertyChanged("BookLoadControl")
            End Set
        End Property

        Private mintBookODControl As Integer = 0
        <DataMember()> _
        Public Property BookODControl() As Integer
            Get
                Return mintBookODControl
            End Get
            Set(ByVal Value As Integer)
                mintBookODControl = Value
                NotifyPropertyChanged("BookODControl")
            End Set
        End Property

        Private mintBookStopNo As Integer = 0
        <DataMember()> _
        Public Property BookStopNo() As Integer
            Get
                Return mintBookStopNo
            End Get
            Set(ByVal Value As Integer)
                mintBookStopNo = Value
                NotifyPropertyChanged("BookStopNo")
            End Set
        End Property

        Private mintRouteType As Integer = 0
        <DataMember()> _
        Public Property RouteType() As Integer
            Get
                Return mintRouteType
            End Get
            Set(ByVal Value As Integer)
                mintRouteType = Value
                NotifyPropertyChanged("RouteType")
            End Set
        End Property

        Private mintDistType As Integer = 0
        <DataMember()> _
        Public Property DistType() As Integer
            Get
                Return mintDistType
            End Get
            Set(ByVal Value As Integer)
                mintDistType = Value
                NotifyPropertyChanged("DistType")
            End Set
        End Property

        Private mstrBookOrigZip As String = ""
        <DataMember()> _
        Public Property BookOrigZip() As String
            Get
                Return mstrBookOrigZip
            End Get
            Set(ByVal Value As String)
                mstrBookOrigZip = Value
                NotifyPropertyChanged("BookOrigZip")
            End Set
        End Property

        Private mstrBookDestZip As String = ""
        <DataMember()> _
        Public Property BookDestZip() As String
            Get
                Return mstrBookDestZip
            End Get
            Set(ByVal Value As String)
                mstrBookDestZip = Value
                NotifyPropertyChanged("BookDestZip")
            End Set
        End Property

        Private mstrBookOrigAddress1 As String = ""
        <DataMember()> _
        Public Property BookOrigAddress1() As String
            Get
                Return mstrBookOrigAddress1
            End Get
            Set(ByVal Value As String)
                mstrBookOrigAddress1 = Value
                NotifyPropertyChanged("BookOrigAddress1")
            End Set
        End Property

        Private mstrBookDestAddress1 As String = ""
        <DataMember()> _
        Public Property BookDestAddress1() As String
            Get
                Return mstrBookDestAddress1
            End Get
            Set(ByVal Value As String)
                mstrBookDestAddress1 = Value
                NotifyPropertyChanged("BookDestAddress1")
            End Set
        End Property

        Private mstrBookOrigCity As String = ""
        <DataMember()> _
        Public Property BookOrigCity() As String
            Get
                Return mstrBookOrigCity
            End Get
            Set(ByVal Value As String)
                mstrBookOrigCity = Value
                NotifyPropertyChanged("BookOrigCity")
            End Set
        End Property

        Private mstrBookDestCity As String = ""
        <DataMember()> _
        Public Property BookDestCity() As String
            Get
                Return mstrBookDestCity
            End Get
            Set(ByVal Value As String)
                mstrBookDestCity = Value
                NotifyPropertyChanged("BookDestCity")
            End Set
        End Property

        Private mstrBookOrigState As String = ""
        <DataMember()> _
        Public Property BookOrigState() As String
            Get
                Return mstrBookOrigState
            End Get
            Set(ByVal Value As String)
                mstrBookOrigState = Value
                NotifyPropertyChanged("BookOrigState")
            End Set
        End Property

        Private mstrBookDestState As String = ""
        <DataMember()> _
        Public Property BookDestState() As String
            Get
                Return mstrBookDestState
            End Get
            Set(ByVal Value As String)
                mstrBookDestState = Value
                NotifyPropertyChanged("BookDestState")
            End Set
        End Property

        Private mstrBookProNumber As String = ""
        <DataMember()> _
        Public Property BookProNumber() As String
            Get
                Return mstrBookProNumber
            End Get
            Set(ByVal Value As String)
                mstrBookProNumber = Value
                NotifyPropertyChanged("BookProNumber")
            End Set
        End Property

        Private mblnLaneOriginAddressUse As Boolean = True
        <DataMember()> _
        Public Property LaneOriginAddressUse() As Boolean
            Get
                Return mblnLaneOriginAddressUse
            End Get
            Set(ByVal Value As Boolean)
                mblnLaneOriginAddressUse = Value
                NotifyPropertyChanged("LaneOriginAddressUse")
            End Set
        End Property


#End Region

#Region " Public Methods"
        Public Overrides Function Clone() As DTOBaseClass
            Dim instance As New PCMDataStop
            instance = DirectCast(MemberwiseClone(), PCMDataStop)
            Return instance
        End Function

#End Region

    End Class

End Namespace

