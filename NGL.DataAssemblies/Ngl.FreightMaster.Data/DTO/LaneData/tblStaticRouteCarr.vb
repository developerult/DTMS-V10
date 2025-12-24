Imports Microsoft.VisualBasic
Imports System
Imports System.Collections.Generic
Imports System.Linq
Imports System.Text
Imports System.Runtime.Serialization
Imports Ngl.Core.ChangeTracker

Namespace DataTransferObjects
    <DataContract()> _
    Public Class tblStaticRouteCarr
            Inherits DTOBaseClass


#Region " Data Members"
        Private _StaticRouteCarrControl As Integer = 0
        <DataMember()> _
        Public Property StaticRouteCarrControl() As Integer
            Get
                Return _StaticRouteCarrControl
            End Get
            Set(ByVal value As Integer)
                _StaticRouteCarrControl = value
            End Set
        End Property

        Private _StaticRouteCarrStaticRouteControl As Integer = 0
        <DataMember()> _
        Public Property StaticRouteCarrStaticRouteControl() As Integer
            Get
                Return _StaticRouteCarrStaticRouteControl
            End Get
            Set(ByVal value As Integer)
                _StaticRouteCarrStaticRouteControl = value
            End Set
        End Property

        Private _StaticRouteCarrCarrierControl As Integer = 0
        <DataMember()> _
        Public Property StaticRouteCarrCarrierControl() As Integer
            Get
                Return _StaticRouteCarrCarrierControl
            End Get
            Set(ByVal value As Integer)
                _StaticRouteCarrCarrierControl = value
            End Set
        End Property

        Private _StaticRouteCarrCarrierNumber As Integer = 0
        <DataMember()> _
        Public Property StaticRouteCarrCarrierNumber() As Integer
            Get
                Return _StaticRouteCarrCarrierNumber
            End Get
            Set(ByVal value As Integer)
                _StaticRouteCarrCarrierNumber = value
            End Set
        End Property

        Private _StaticRouteCarrCarrierName As String = ""
        <DataMember()> _
        Public Property StaticRouteCarrCarrierName() As String
            Get
                Return Left(_StaticRouteCarrCarrierName, 50)
            End Get
            Set(ByVal value As String)
                _StaticRouteCarrCarrierName = Left(value, 50)
            End Set
        End Property

        Private _StaticRouteCarrName As String = ""
        <DataMember()> _
        Public Property StaticRouteCarrName() As String
            Get
                Return Left(_StaticRouteCarrName, 50)
            End Get
            Set(ByVal value As String)
                _StaticRouteCarrName = Left(value, 50)
            End Set
        End Property

        Private _StaticRouteCarrDescription As String = ""
        <DataMember()> _
        Public Property StaticRouteCarrDescription() As String
            Get
                Return Left(_StaticRouteCarrDescription, 255)
            End Get
            Set(ByVal value As String)
                _StaticRouteCarrDescription = Left(value, 255)
            End Set
        End Property

        Private _StaticRouteCarrRouteTypeCode As Integer = 0
        <DataMember()> _
        Public Property StaticRouteCarrRouteTypeCode() As Integer
            Get
                Return _StaticRouteCarrRouteTypeCode
            End Get
            Set(ByVal value As Integer)
                _StaticRouteCarrRouteTypeCode = value
            End Set
        End Property

        Private _StaticRouteCarrAutoTenderFlag As Boolean = False
        <DataMember()> _
        Public Property StaticRouteCarrAutoTenderFlag() As Boolean
            Get
                Return _StaticRouteCarrAutoTenderFlag
            End Get
            Set(ByVal value As Boolean)
                _StaticRouteCarrAutoTenderFlag = value
            End Set
        End Property

        Private _StaticRouteCarrTendLeadTime As Integer = 0
        <DataMember()> _
        Public Property StaticRouteCarrTendLeadTime() As Integer
            Get
                Return _StaticRouteCarrTendLeadTime
            End Get
            Set(ByVal value As Integer)
                _StaticRouteCarrTendLeadTime = value
            End Set
        End Property

        Private _StaticRouteCarrMaxStops As Integer = 0
        <DataMember()> _
        Public Property StaticRouteCarrMaxStops() As Integer
            Get
                Return _StaticRouteCarrMaxStops
            End Get
            Set(ByVal value As Integer)
                _StaticRouteCarrMaxStops = value
            End Set
        End Property

        Private _StaticRouteCarrHazmatFlag As Boolean = False
        <DataMember()> _
        Public Property StaticRouteCarrHazmatFlag() As Boolean
            Get
                Return _StaticRouteCarrHazmatFlag
            End Get
            Set(ByVal value As Boolean)
                _StaticRouteCarrHazmatFlag = value
            End Set
        End Property

        Private _StaticRouteCarrTransType As Integer = 0
        <DataMember()> _
        Public Property StaticRouteCarrTransType() As Integer
            Get
                Return _StaticRouteCarrTransType
            End Get
            Set(ByVal value As Integer)
                _StaticRouteCarrTransType = value
            End Set
        End Property

        Private _StaticRouteCarrRouteSequence As Integer = 0
        <DataMember()> _
        Public Property StaticRouteCarrRouteSequence() As Integer
            Get
                Return _StaticRouteCarrRouteSequence
            End Get
            Set(ByVal value As Integer)
                _StaticRouteCarrRouteSequence = value
            End Set
        End Property

        Private _StaticRouteCarrRequireAutoTenderApproval As Boolean = False
        <DataMember()> _
        Public Property StaticRouteCarrRequireAutoTenderApproval() As Boolean
            Get
                Return _StaticRouteCarrRequireAutoTenderApproval
            End Get
            Set(ByVal value As Boolean)
                _StaticRouteCarrRequireAutoTenderApproval = value
            End Set
        End Property

        Private _StaticRouteCarrAutoAcceptLoads As Boolean = False
        <DataMember()> _
        Public Property StaticRouteCarrAutoAcceptLoads() As Boolean
            Get
                Return _StaticRouteCarrAutoAcceptLoads
            End Get
            Set(ByVal value As Boolean)
                _StaticRouteCarrAutoAcceptLoads = value
            End Set
        End Property

        Private _StaticRouteStateFilter As String
        <DataMember()> _
         Public Property StaticRouteStateFilter As String
            Get
                Return Left(_StaticRouteStateFilter, 255)
            End Get
            Set(value As String)
                _StaticRouteStateFilter = Left(value, 255)
            End Set
        End Property


        Private _StaticRouteCarrModDate As System.Nullable(Of Date)
        <DataMember()> _
        Public Property StaticRouteCarrModDate() As System.Nullable(Of Date)
            Get
                Return _StaticRouteCarrModDate
            End Get
            Set(ByVal value As System.Nullable(Of Date))
                _StaticRouteCarrModDate = value
            End Set
        End Property

        Private _StaticRouteCarrModUser As String = ""
        <DataMember()> _
        Public Property StaticRouteCarrModUser() As String
            Get
                Return Left(_StaticRouteCarrModUser, 100)
            End Get
            Set(ByVal value As String)
                _StaticRouteCarrModUser = Left(value, 100)
            End Set
        End Property

        Private _StaticRouteCarrURI As String = ""
        <DataMember()> _
        Public Property StaticRouteCarrURI() As String
            Get
                Return Left(_StaticRouteCarrURI, 500)
            End Get
            Set(ByVal value As String)
                _StaticRouteCarrURI = Left(value, 500)
            End Set
        End Property

        Private _StaticRouteCarrUpdated As Byte()
        <DataMember()> _
        Public Property StaticRouteCarrUpdated() As Byte()
            Get
                Return _StaticRouteCarrUpdated
            End Get
            Set(ByVal value As Byte())
                _StaticRouteCarrUpdated = value
            End Set
        End Property

#End Region

#Region " Public Methods"
        Public Overrides Function Clone() As DTOBaseClass
            Dim instance As New tblStaticRouteCarr
            instance = DirectCast(MemberwiseClone(), tblStaticRouteCarr)
            Return instance
        End Function

#End Region

    End Class
End Namespace