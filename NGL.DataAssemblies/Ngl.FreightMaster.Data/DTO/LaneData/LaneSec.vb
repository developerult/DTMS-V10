Imports Microsoft.VisualBasic
Imports System
Imports System.Collections.Generic
Imports System.Linq
Imports System.Text
Imports System.Runtime.Serialization
Imports Ngl.Core.ChangeTracker

Namespace DataTransferObjects
    <DataContract()> _
    Public Class LaneSec
        Inherits DTOBaseClass


#Region " Data Members"
        Private _LaneSecControl As Integer = 0
        <DataMember()> _
        Public Property LaneSecControl() As Integer
            Get
                Return _LaneSecControl
            End Get
            Set(ByVal value As Integer)
                _LaneSecControl = value
            End Set
        End Property

        Private _LaneSecLaneControl As Integer = 0
        <DataMember()> _
        Public Property LaneSecLaneControl() As Integer
            Get
                Return _LaneSecLaneControl
            End Get
            Set(ByVal value As Integer)
                _LaneSecLaneControl = value
            End Set
        End Property

        Private _LaneSecPUNumber As String = ""
        <DataMember()> _
        Public Property LaneSecPUNumber() As String
            Get
                Return Left(_LaneSecPUNumber, 50)
            End Get
            Set(ByVal value As String)
                _LaneSecPUNumber = Left(value, 50)
            End Set
        End Property

        Private _LaneSecPUName As String = ""
        <DataMember()> _
        Public Property LaneSecPUName() As String
            Get
                Return Left(_LaneSecPUName, 40)
            End Get
            Set(ByVal value As String)
                _LaneSecPUName = Left(value, 40)
            End Set
        End Property

        Private _LaneSecPUAddress1 As String = ""
        <DataMember()> _
        Public Property LaneSecPUAddress1() As String
            Get
                Return Left(_LaneSecPUAddress1, 40)
            End Get
            Set(ByVal value As String)
                _LaneSecPUAddress1 = Left(value, 40)
            End Set
        End Property

        Private _LaneSecPUAddress2 As String = ""
        <DataMember()> _
        Public Property LaneSecPUAddress2() As String
            Get
                Return Left(_LaneSecPUAddress2, 40)
            End Get
            Set(ByVal value As String)
                _LaneSecPUAddress2 = Left(value, 40)
            End Set
        End Property

        Private _LaneSecPUAddress3 As String = ""
        <DataMember()> _
        Public Property LaneSecPUAddress3() As String
            Get
                Return Left(_LaneSecPUAddress3, 25)
            End Get
            Set(ByVal value As String)
                _LaneSecPUAddress3 = Left(value, 25)
            End Set
        End Property

        Private _LaneSecPUCity As String = ""
        <DataMember()> _
        Public Property LaneSecPUCity() As String
            Get
                Return Left(_LaneSecPUCity, 25)
            End Get
            Set(ByVal value As String)
                _LaneSecPUCity = Left(value, 25)
            End Set
        End Property

        Private _LaneSecPUState As String = ""
        <DataMember()> _
        Public Property LaneSecPUState() As String
            Get
                Return Left(_LaneSecPUState, 2)
            End Get
            Set(ByVal value As String)
                _LaneSecPUState = Left(value, 2)
            End Set
        End Property

        Private _LaneSecPUCountry As String = ""
        <DataMember()> _
        Public Property LaneSecPUCountry() As String
            Get
                Return Left(_LaneSecPUCountry, 30)
            End Get
            Set(ByVal value As String)
                _LaneSecPUCountry = Left(value, 30)
            End Set
        End Property

        Private _LaneSecPUZip As String = ""
        <DataMember()> _
        Public Property LaneSecPUZip() As String
            Get
                Return Left(_LaneSecPUZip, 10)
            End Get
            Set(ByVal value As String)
                _LaneSecPUZip = Left(value, 10)
            End Set
        End Property

        Private _LaneSecPUContactPhone As String = ""
        <DataMember()> _
        Public Property LaneSecPUContactPhone() As String
            Get
                Return Left(_LaneSecPUContactPhone, 15)
            End Get
            Set(ByVal value As String)
                _LaneSecPUContactPhone = Left(value, 15)
            End Set
        End Property

        Private _LaneSecPUContactFax As String = ""
        <DataMember()> _
        Public Property LaneSecPUContactFax() As String
            Get
                Return Left(_LaneSecPUContactFax, 15)
            End Get
            Set(ByVal value As String)
                _LaneSecPUContactFax = Left(value, 15)
            End Set
        End Property

        Private _LaneSecBrokerNumber As String = ""
        <DataMember()> _
        Public Property LaneSecBrokerNumber() As String
            Get
                Return Left(_LaneSecBrokerNumber, 50)
            End Get
            Set(ByVal value As String)
                _LaneSecBrokerNumber = Left(value, 50)
            End Set
        End Property

        Private _LaneSecBrokerName As String = ""
        <DataMember()> _
        Public Property LaneSecBrokerName() As String
            Get
                Return Left(_LaneSecBrokerName, 40)
            End Get
            Set(ByVal value As String)
                _LaneSecBrokerName = Left(value, 40)
            End Set
        End Property

        Private _LaneSecBrokerAddress1 As String = ""
        <DataMember()> _
        Public Property LaneSecBrokerAddress1() As String
            Get
                Return Left(_LaneSecBrokerAddress1, 40)
            End Get
            Set(ByVal value As String)
                _LaneSecBrokerAddress1 = Left(value, 40)
            End Set
        End Property

        Private _LaneSecBrokerAddress2 As String = ""
        <DataMember()> _
        Public Property LaneSecBrokerAddress2() As String
            Get
                Return Left(_LaneSecBrokerAddress2, 40)
            End Get
            Set(ByVal value As String)
                _LaneSecBrokerAddress2 = Left(value, 40)
            End Set
        End Property

        Private _LaneSecBrokerAddress3 As String = ""
        <DataMember()> _
        Public Property LaneSecBrokerAddress3() As String
            Get
                Return Left(_LaneSecBrokerAddress3, 40)
            End Get
            Set(ByVal value As String)
                _LaneSecBrokerAddress3 = Left(value, 40)
            End Set
        End Property

        Private _LaneSecBrokerCity As String = ""
        <DataMember()> _
        Public Property LaneSecBrokerCity() As String
            Get
                Return Left(_LaneSecBrokerCity, 25)
            End Get
            Set(ByVal value As String)
                _LaneSecBrokerCity = Left(value, 25)
            End Set
        End Property

        Private _LaneSecBrokerState As String = ""
        <DataMember()> _
        Public Property LaneSecBrokerState() As String
            Get
                Return Left(_LaneSecBrokerState, 2)
            End Get
            Set(ByVal value As String)
                _LaneSecBrokerState = Left(value, 2)
            End Set
        End Property

        Private _LaneSecBrokerCountry As String = ""
        <DataMember()> _
        Public Property LaneSecBrokerCountry() As String
            Get
                Return Left(_LaneSecBrokerCountry, 30)
            End Get
            Set(ByVal value As String)
                _LaneSecBrokerCountry = Left(value, 30)
            End Set
        End Property

        Private _LaneSecBrokerZip As String = ""
        <DataMember()> _
        Public Property LaneSecBrokerZip() As String
            Get
                Return Left(_LaneSecBrokerZip, 10)
            End Get
            Set(ByVal value As String)
                _LaneSecBrokerZip = Left(value, 10)
            End Set
        End Property

        Private _LaneSecBrokerContactPhone As String = ""
        <DataMember()> _
        Public Property LaneSecBrokerContactPhone() As String
            Get
                Return Left(_LaneSecBrokerContactPhone, 15)
            End Get
            Set(ByVal value As String)
                _LaneSecBrokerContactPhone = Left(value, 15)
            End Set
        End Property

        Private _LaneSecBrokerContactFax As String = ""
        <DataMember()> _
        Public Property LaneSecBrokerContactFax() As String
            Get
                Return Left(_LaneSecBrokerContactFax, 50)
            End Get
            Set(ByVal value As String)
                _LaneSecBrokerContactFax = Left(value, 50)
            End Set
        End Property

        Private _LaneSecBrokerContactName As String = ""
        <DataMember()> _
        Public Property LaneSecBrokerContactName() As String
            Get
                Return Left(_LaneSecBrokerContactName, 30)
            End Get
            Set(ByVal value As String)
                _LaneSecBrokerContactName = Left(value, 30)
            End Set
        End Property

        Private _LaneSecBrokerOpHourStart As System.Nullable(Of Date)
        <DataMember()> _
        Public Property LaneSecBrokerOpHourStart() As System.Nullable(Of Date)
            Get
                Return _LaneSecBrokerOpHourStart
            End Get
            Set(ByVal value As System.Nullable(Of Date))
                _LaneSecBrokerOpHourStart = value
            End Set
        End Property

        Private _LaneSecBrokerOpHourStop As System.Nullable(Of Date)
        <DataMember()> _
        Public Property LaneSecBrokerOpHourStop() As System.Nullable(Of Date)
            Get
                Return _LaneSecBrokerOpHourStop
            End Get
            Set(ByVal value As System.Nullable(Of Date))
                _LaneSecBrokerOpHourStop = value
            End Set
        End Property

        Private _LaneSecComment As String = ""
        <DataMember()> _
        Public Property LaneSecComment() As String
            Get
                Return Left(_LaneSecComment, 255)
            End Get
            Set(ByVal value As String)
                _LaneSecComment = Left(value, 255)
            End Set
        End Property

        Private _LaneSecModDate As System.Nullable(Of Date)
        <DataMember()> _
        Public Property LaneSecModDate() As System.Nullable(Of Date)
            Get
                Return _LaneSecModDate
            End Get
            Set(ByVal value As System.Nullable(Of Date))
                _LaneSecModDate = value
            End Set
        End Property

        Private _LaneSecModUser As String = ""
        <DataMember()> _
        Public Property LaneSecModUser() As String
            Get
                Return Left(_LaneSecModUser, 100)
            End Get
            Set(ByVal value As String)
                _LaneSecModUser = Left(value, 100)
            End Set
        End Property

        Private _LaneSecUpdated As Byte()
        <DataMember()> _
        Public Property LaneSecUpdated() As Byte()
            Get
                Return _LaneSecUpdated
            End Get
            Set(ByVal value As Byte())
                _LaneSecUpdated = value
            End Set
        End Property
#End Region

#Region " Public Methods"
        Public Overrides Function Clone() As DTOBaseClass
            Dim instance As New LaneSec
            instance = DirectCast(MemberwiseClone(), LaneSec)
            Return instance
        End Function

#End Region

    End Class
End Namespace