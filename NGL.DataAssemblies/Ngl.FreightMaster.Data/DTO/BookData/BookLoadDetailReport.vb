Imports Microsoft.VisualBasic
Imports System
Imports System.Collections.Generic
Imports System.Linq
Imports System.Text
Imports System.Runtime.Serialization
Imports Ngl.Core.ChangeTracker

Namespace DataTransferObjects
    <DataContract()> _
    Public Class BookLoadDetailReport
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

        Private _CarrierName As String = ""
        <DataMember()> _
        Public Property CarrierName() As String
            Get
                Return Left(_CarrierName, 60)
            End Get
            Set(ByVal value As String)
                _CarrierName = Left(value, 60)
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

        Private _BookCarrScheduleDate As System.Nullable(Of Date)
        <DataMember()> _
        Public Property BookCarrScheduleDate() As System.Nullable(Of Date)
            Get
                Return _BookCarrScheduleDate
            End Get
            Set(ByVal value As System.Nullable(Of Date))
                _BookCarrScheduleDate = value
            End Set
        End Property

        Private _BookCarrScheduleTime As System.Nullable(Of Date)
        <DataMember()> _
        Public Property BookCarrScheduleTime() As System.Nullable(Of Date)
            Get
                Return _BookCarrScheduleTime
            End Get
            Set(ByVal value As System.Nullable(Of Date))
                _BookCarrScheduleTime = value
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

        Private _BookCarrActualDate As System.Nullable(Of Date)
        <DataMember()> _
        Public Property BookCarrActualDate() As System.Nullable(Of Date)
            Get
                Return _BookCarrActualDate
            End Get
            Set(ByVal value As System.Nullable(Of Date))
                _BookCarrActualDate = value
            End Set
        End Property

        Private _BookCarrActualTime As System.Nullable(Of Date)
        <DataMember()> _
        Public Property BookCarrActualTime() As System.Nullable(Of Date)
            Get
                Return _BookCarrActualTime
            End Get
            Set(ByVal value As System.Nullable(Of Date))
                _BookCarrActualTime = value
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

        Private _BookCarrApptDate As System.Nullable(Of Date)
        <DataMember()> _
        Public Property BookCarrApptDate() As System.Nullable(Of Date)
            Get
                Return _BookCarrApptDate
            End Get
            Set(ByVal value As System.Nullable(Of Date))
                _BookCarrApptDate = value
            End Set
        End Property

        Private _BookCarrApptTime As System.Nullable(Of Date)
        <DataMember()> _
        Public Property BookCarrApptTime() As System.Nullable(Of Date)
            Get
                Return _BookCarrApptTime
            End Get
            Set(ByVal value As System.Nullable(Of Date))
                _BookCarrApptTime = value
            End Set
        End Property

        Private _BookTotalCube As Integer = 0
        <DataMember()> _
        Public Property BookTotalCube() As Integer
            Get
                Return _BookTotalCube
            End Get
            Set(ByVal value As Integer)
                _BookTotalCube = value
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

        Private _BookCarrActTime As System.Nullable(Of Date)
        <DataMember()> _
        Public Property BookCarrActTime() As System.Nullable(Of Date)
            Get
                Return _BookCarrActTime
            End Get
            Set(ByVal value As System.Nullable(Of Date))
                _BookCarrActTime = value
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
                _BookDestZip = Left(value, 20)  'Modified by RHR for v-8.4.003 on 06/25/2021
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

        Private _BookExpDelDateTime As Date?
        <DataMember()> _
        Public Property BookExpDelDateTime() As Date?
            Get
                Return _BookExpDelDateTime
            End Get
            Set(ByVal value As Date?)
                _BookExpDelDateTime = value
            End Set
        End Property

        Private _BookMustLeaveByDateTime As System.Nullable(Of Date)
        <DataMember()> _
        Public Property BookMustLeaveByDateTime() As System.Nullable(Of Date)
            Get
                Return _BookMustLeaveByDateTime
            End Get
            Set(ByVal value As System.Nullable(Of Date))
                _BookMustLeaveByDateTime = value
            End Set
        End Property


#End Region

#Region " Public Methods"
        Public Overrides Function Clone() As DTOBaseClass
            Dim instance As New BookLoadDetailReport
            instance = DirectCast(MemberwiseClone(), BookLoadDetailReport)
            Return instance
        End Function

#End Region

    End Class
End Namespace