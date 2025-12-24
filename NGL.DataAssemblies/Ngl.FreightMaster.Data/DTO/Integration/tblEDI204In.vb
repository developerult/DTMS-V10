Imports Microsoft.VisualBasic
Imports System
Imports System.Collections.Generic
Imports System.Linq
Imports System.Text
Imports System.Runtime.Serialization
Imports Ngl.Core.ChangeTracker

'Added by LVV on 5/2/17 for v-7.0.6.105 EDI 204In
Namespace DataTransferObjects
    <DataContract()>
    Public Class tblEDI204In
        Inherits DTOBaseClass

#Region " Data Members "

        Private _EDI204InControl As Integer = 0
        <DataMember()>
        Public Property EDI204InControl() As Integer
            Get
                Return _EDI204InControl
            End Get
            Set(ByVal value As Integer)
                _EDI204InControl = value
            End Set
        End Property

        Private _CarrierSCAC As String = ""
        <DataMember()>
        Public Property CarrierSCAC() As String
            Get
                Return Left(_CarrierSCAC, 4)
            End Get
            Set(ByVal value As String)
                _CarrierSCAC = Left(value, 4)
            End Set
        End Property

        Private _CarrierNumber As Integer = 0
        <DataMember()>
        Public Property CarrierNumber() As Integer
            Get
                Return _CarrierNumber
            End Get
            Set(ByVal value As Integer)
                _CarrierNumber = value
            End Set
        End Property

        Private _CarrierName As String = ""
        <DataMember()>
        Public Property CarrierName() As String
            Get
                Return Left(_CarrierName, 40)
            End Get
            Set(ByVal value As String)
                _CarrierName = Left(value, 40)
            End Set
        End Property

        Private _OrderNumber As String = ""
        <DataMember()>
        Public Property OrderNumber() As String
            Get
                Return Left(_OrderNumber, 20)
            End Get
            Set(ByVal value As String)
                _OrderNumber = Left(value, 20)
            End Set
        End Property

        Private _OrderSequence As Integer = 0
        <DataMember()>
        Public Property OrderSequence() As Integer
            Get
                Return _OrderSequence
            End Get
            Set(ByVal value As Integer)
                _OrderSequence = value
            End Set
        End Property

        Private _PONumber As String = ""
        <DataMember()>
        Public Property PONumber() As String
            Get
                Return Left(_PONumber, 20)
            End Get
            Set(ByVal value As String)
                _PONumber = Left(value, 20)
            End Set
        End Property

        Private _TotalCases As Integer = 0
        <DataMember()>
        Public Property TotalCases() As Integer
            Get
                Return _TotalCases
            End Get
            Set(ByVal value As Integer)
                _TotalCases = value
            End Set
        End Property

        Private _TotalWgt As Double = 0
        <DataMember()>
        Public Property TotalWgt() As Double
            Get
                Return _TotalWgt
            End Get
            Set(ByVal value As Double)
                _TotalWgt = value
            End Set
        End Property

        Private _TotalPL As Double = 0
        <DataMember()>
        Public Property TotalPL() As Double
            Get
                Return _TotalPL
            End Get
            Set(ByVal value As Double)
                _TotalPL = value
            End Set
        End Property

        Private _TotalCube As Integer = 0
        <DataMember()>
        Public Property TotalCube() As Integer
            Get
                Return _TotalCube
            End Get
            Set(ByVal value As Integer)
                _TotalCube = value
            End Set
        End Property

        Private _ShipDate As System.Nullable(Of Date)
        <DataMember()>
        Public Property ShipDate() As System.Nullable(Of Date)
            Get
                Return _ShipDate
            End Get
            Set(ByVal value As System.Nullable(Of Date))
                _ShipDate = value
            End Set
        End Property

        Private _ReqDate As System.Nullable(Of Date)
        <DataMember()>
        Public Property ReqDate() As System.Nullable(Of Date)
            Get
                Return _ReqDate
            End Get
            Set(ByVal value As System.Nullable(Of Date))
                _ReqDate = value
            End Set
        End Property

        Private _SchedulePUDate As System.Nullable(Of Date)
        <DataMember()>
        Public Property SchedulePUDate() As System.Nullable(Of Date)
            Get
                Return _SchedulePUDate
            End Get
            Set(ByVal value As System.Nullable(Of Date))
                _SchedulePUDate = value
            End Set
        End Property

        Private _SchedulePUTime As System.Nullable(Of Date)
        <DataMember()>
        Public Property SchedulePUTime() As System.Nullable(Of Date)
            Get
                Return _SchedulePUTime
            End Get
            Set(ByVal value As System.Nullable(Of Date))
                _SchedulePUTime = value
            End Set
        End Property

        Private _ScheduleDelDate As System.Nullable(Of Date)
        <DataMember()>
        Public Property ScheduleDelDate() As System.Nullable(Of Date)
            Get
                Return _ScheduleDelDate
            End Get
            Set(ByVal value As System.Nullable(Of Date))
                _ScheduleDelDate = value
            End Set
        End Property

        Private _ScheduleDelTime As System.Nullable(Of Date)
        <DataMember()>
        Public Property ScheduleDelTime() As System.Nullable(Of Date)
            Get
                Return _ScheduleDelTime
            End Get
            Set(ByVal value As System.Nullable(Of Date))
                _ScheduleDelTime = value
            End Set
        End Property

        Private _OrigName As String = ""
        <DataMember()>
        Public Property OrigName() As String
            Get
                Return Left(_OrigName, 40)
            End Get
            Set(ByVal value As String)
                _OrigName = Left(value, 40)
            End Set
        End Property

        Private _OrigAddress1 As String = ""
        <DataMember()>
        Public Property OrigAddress1() As String
            Get
                Return Left(_OrigAddress1, 40)
            End Get
            Set(ByVal value As String)
                _OrigAddress1 = Left(value, 40)
            End Set
        End Property

        Private _OrigAddress2 As String = ""
        <DataMember()>
        Public Property OrigAddress2() As String
            Get
                Return Left(_OrigAddress2, 40)
            End Get
            Set(ByVal value As String)
                _OrigAddress2 = Left(value, 40)
            End Set
        End Property

        Private _OrigAddress3 As String = ""
        <DataMember()>
        Public Property OrigAddress3() As String
            Get
                Return Left(_OrigAddress3, 40)
            End Get
            Set(ByVal value As String)
                _OrigAddress3 = Left(value, 40)
            End Set
        End Property

        Private _OrigCity As String = ""
        <DataMember()>
        Public Property OrigCity() As String
            Get
                Return Left(_OrigCity, 25)
            End Get
            Set(ByVal value As String)
                _OrigCity = Left(value, 25)
            End Set
        End Property

        Private _OrigState As String = ""
        <DataMember()>
        Public Property OrigState() As String
            Get
                Return Left(_OrigState, 8)
            End Get
            Set(ByVal value As String)
                _OrigState = Left(value, 8)
            End Set
        End Property

        Private _OrigCountry As String = ""
        <DataMember()>
        Public Property OrigCountry() As String
            Get
                Return Left(_OrigCountry, 30)
            End Get
            Set(ByVal value As String)
                _OrigCountry = Left(value, 30)
            End Set
        End Property

        Private _OrigZip As String = ""
        <DataMember()>
        Public Property OrigZip() As String
            Get
                Return Left(_OrigZip, 10)
            End Get
            Set(ByVal value As String)
                _OrigZip = Left(value, 10)
            End Set
        End Property

        Private _OrigPhone As String = ""
        <DataMember()>
        Public Property OrigPhone() As String
            Get
                Return Left(_OrigPhone, 15)
            End Get
            Set(ByVal value As String)
                _OrigPhone = Left(value, 15)
            End Set
        End Property

        'Private _BookOrigIDENTIFICATIONCODEQUALIFIER As String = ""
        '<DataMember()>
        'Public Property BookOrigIDENTIFICATIONCODEQUALIFIER() As String
        '    Get
        '        Return Left(_BookOrigIDENTIFICATIONCODEQUALIFIER, 2)
        '    End Get
        '    Set(ByVal value As String)
        '        _BookOrigIDENTIFICATIONCODEQUALIFIER = Left(value, 2)
        '    End Set
        'End Property

        Private _OrigCompanyNumber As String = ""
        <DataMember()>
        Public Property OrigCompanyNumber() As String
            Get
                Return Left(_OrigCompanyNumber, 50)
            End Get
            Set(ByVal value As String)
                _OrigCompanyNumber = Left(value, 50)
            End Set
        End Property

        Private _DestName As String = ""
        <DataMember()>
        Public Property DestName() As String
            Get
                Return Left(_DestName, 40)
            End Get
            Set(ByVal value As String)
                _DestName = Left(value, 40)
            End Set
        End Property

        Private _DestAddress1 As String = ""
        <DataMember()>
        Public Property DestAddress1() As String
            Get
                Return Left(_DestAddress1, 40)
            End Get
            Set(ByVal value As String)
                _DestAddress1 = Left(value, 40)
            End Set
        End Property

        Private _DestAddress2 As String = ""
        <DataMember()>
        Public Property DestAddress2() As String
            Get
                Return Left(_DestAddress2, 40)
            End Get
            Set(ByVal value As String)
                _DestAddress2 = Left(value, 40)
            End Set
        End Property

        Private _DestAddress3 As String = ""
        <DataMember()>
        Public Property DestAddress3() As String
            Get
                Return Left(_DestAddress3, 40)
            End Get
            Set(ByVal value As String)
                _DestAddress3 = Left(value, 40)
            End Set
        End Property

        Private _DestCity As String = ""
        <DataMember()>
        Public Property DestCity() As String
            Get
                Return Left(_DestCity, 25)
            End Get
            Set(ByVal value As String)
                _DestCity = Left(value, 25)
            End Set
        End Property

        Private _DestState As String = ""
        <DataMember()>
        Public Property DestState() As String
            Get
                Return Left(_DestState, 2)
            End Get
            Set(ByVal value As String)
                _DestState = Left(value, 2)
            End Set
        End Property

        Private _DestCountry As String = ""
        <DataMember()>
        Public Property DestCountry() As String
            Get
                Return Left(_DestCountry, 30)
            End Get
            Set(ByVal value As String)
                _DestCountry = Left(value, 30)
            End Set
        End Property

        Private _DestZip As String = ""
        <DataMember()>
        Public Property DestZip() As String
            Get
                Return Left(_DestZip, 10)
            End Get
            Set(ByVal value As String)
                _DestZip = Left(value, 10)
            End Set
        End Property

        Private _DestPhone As String = ""
        <DataMember()>
        Public Property DestPhone() As String
            Get
                Return Left(_DestPhone, 15)
            End Get
            Set(ByVal value As String)
                _DestPhone = Left(value, 15)
            End Set
        End Property

        'Private _BookDestIDENTIFICATIONCODEQUALIFIER As String = ""
        '<DataMember()>
        'Public Property BookDestIDENTIFICATIONCODEQUALIFIER() As String
        '    Get
        '        Return Left(_BookDestIDENTIFICATIONCODEQUALIFIER, 2)
        '    End Get
        '    Set(ByVal value As String)
        '        _BookDestIDENTIFICATIONCODEQUALIFIER = Left(value, 2)
        '    End Set
        'End Property

        Private _DestCompanyNumber As String = ""
        <DataMember()>
        Public Property DestCompanyNumber() As String
            Get
                Return Left(_DestCompanyNumber, 50)
            End Get
            Set(ByVal value As String)
                _DestCompanyNumber = Left(value, 50)
            End Set
        End Property

        Private _LaneComments As String = ""
        <DataMember()>
        Public Property LaneComments() As String
            Get
                Return Left(_LaneComments, 255)
            End Get
            Set(ByVal value As String)
                _LaneComments = Left(value, 255)
            End Set
        End Property

        Private _Inbound As Boolean = False
        <DataMember()>
        Public Property Inbound() As Boolean
            Get
                Return _Inbound
            End Get
            Set(ByVal value As Boolean)
                _Inbound = value
            End Set
        End Property

        Private _EDI204InReceivedDate As System.Nullable(Of Date)
        <DataMember()>
        Public Property EDI204InReceivedDate() As System.Nullable(Of Date)
            Get
                Return _EDI204InReceivedDate
            End Get
            Set(ByVal value As System.Nullable(Of Date))
                _EDI204InReceivedDate = value
            End Set
        End Property

        Private _EDI204InStatusCode As Integer? = 0
        <DataMember()>
        Public Property EDI204InStatusCode() As Integer?
            Get
                Return _EDI204InStatusCode
            End Get
            Set(ByVal value As Integer?)
                _EDI204InStatusCode = value
            End Set
        End Property

        Private _EDI204InMessage As String = ""
        <DataMember()>
        Public Property EDI204InMessage() As String
            Get
                Return _EDI204InMessage
            End Get
            Set(ByVal value As String)
                _EDI204InMessage = value
            End Set
        End Property

        Private _Archived As Boolean = False
        <DataMember()>
        Public Property Archived() As Boolean
            Get
                Return _Archived
            End Get
            Set(ByVal value As Boolean)
                _Archived = value
            End Set
        End Property

        Private _EDI204InModDate As System.Nullable(Of Date)
        <DataMember()>
        Public Property EDI204InModDate() As System.Nullable(Of Date)
            Get
                Return _EDI204InModDate
            End Get
            Set(ByVal value As System.Nullable(Of Date))
                _EDI204InModDate = value
                Me.SendPropertyChanged("EDI204ModDate")
            End Set
        End Property

        Private _EDI204InModUser As String = ""
        <DataMember()>
        Public Property EDI204InModUser() As String
            Get
                Return Left(_EDI204InModUser, 100)
            End Get
            Set(ByVal value As String)
                _EDI204InModUser = Left(value, 100)
                Me.SendPropertyChanged("EDI204ModUser")
            End Set
        End Property

        Private _EDI204InUpdated As Byte()
        <DataMember()>
        Public Property EDI204InUpdated() As Byte()
            Get
                Return _EDI204InUpdated
            End Get
            Set(ByVal value As Byte())
                _EDI204InUpdated = value
            End Set
        End Property

        Private _EDI204InFileName204In As String = ""
        <DataMember()>
        Public Property EDI204InFileName204In() As String
            Get
                Return Left(_EDI204InFileName204In, 255)
            End Get
            Set(ByVal value As String)
                _EDI204InFileName204In = Left(value, 255)
                Me.SendPropertyChanged("EDI204FileName204")
            End Set
        End Property

        'Private _EDI204GS06 As Integer = 0
        '<DataMember()>
        'Public Property EDI204GS06() As Integer
        '    Get
        '        Return _EDI204GS06
        '    End Get
        '    Set(ByVal value As Integer)
        '        _EDI204GS06 = value
        '    End Set
        'End Property

#End Region

#Region " Public Methods"
        Public Overrides Function Clone() As DTOBaseClass
            Dim instance As New tblEDI204In
            instance = DirectCast(MemberwiseClone(), tblEDI204In)
            Return instance
        End Function

#End Region

    End Class
End Namespace
