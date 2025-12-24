Imports Microsoft.VisualBasic
Imports System
Imports System.Collections.Generic
Imports System.Linq
Imports System.Text
Imports System.Runtime.Serialization
Imports Ngl.Core.ChangeTracker

Namespace DataTransferObjects
    <DataContract()> _
    Public Class BookTruckLoadData
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

        Private _BookCustCompControl As Integer = 0
        <DataMember()> _
        Public Property BookCustCompControl() As Integer
            Get
                Return _BookCustCompControl
            End Get
            Set(ByVal value As Integer)
                _BookCustCompControl = value
            End Set
        End Property

        Private _BookOrigZip As String = ""
        <DataMember()> _
        Public Property BookOrigZip() As String
            Get
                Return _BookOrigZip
            End Get
            Set(ByVal value As String)
                _BookOrigZip = value
            End Set
        End Property

        Private _BookDestZip As String = ""
        <DataMember()> _
        Public Property BookDestZip() As String
            Get
                Return _BookDestZip
            End Get
            Set(ByVal value As String)
                _BookDestZip = value
            End Set
        End Property

        Private _BookOrigAddress1 As String = ""
        <DataMember()> _
        Public Property BookOrigAddress1() As String
            Get
                Return _BookOrigAddress1
            End Get
            Set(ByVal value As String)
                _BookOrigAddress1 = value
            End Set
        End Property

        Private _BookDestAddress1 As String = ""
        <DataMember()> _
        Public Property BookDestAddress1() As String
            Get
                Return _BookDestAddress1
            End Get
            Set(ByVal value As String)
                _BookDestAddress1 = value
            End Set
        End Property

        Private _BookOrigCity As String = ""
        <DataMember()> _
        Public Property BookOrigCity() As String
            Get
                Return _BookOrigCity
            End Get
            Set(ByVal value As String)
                _BookOrigCity = value
            End Set
        End Property

        Private _BookDestCity As String = ""
        <DataMember()> _
        Public Property BookDestCity() As String
            Get
                Return _BookDestCity
            End Get
            Set(ByVal value As String)
                _BookDestCity = value
            End Set
        End Property

        Private _BookOrigState As String = ""
        <DataMember()> _
        Public Property BookOrigState() As String
            Get
                Return _BookOrigState
            End Get
            Set(ByVal value As String)
                _BookOrigState = value
            End Set
        End Property

        Private _BookDestState As String = ""
        <DataMember()> _
        Public Property BookDestState() As String
            Get
                Return _BookDestState
            End Get
            Set(ByVal value As String)
                _BookDestState = value
            End Set
        End Property

        Private _BookLoadControl As Integer = 0
        <DataMember()> _
        Public Property BookLoadControl() As Integer
            Get
                Return _BookLoadControl
            End Get
            Set(ByVal value As Integer)
                _BookLoadControl = value
            End Set
        End Property

        Private _BookODControl As Integer = 0
        <DataMember()> _
        Public Property BookODControl() As Integer
            Get
                Return _BookODControl
            End Get
            Set(ByVal value As Integer)
                _BookODControl = value
            End Set
        End Property

        Private _BookProNumber As String = ""
        <DataMember()> _
        Public Property BookProNumber() As String
            Get
                Return _BookProNumber
            End Get
            Set(ByVal value As String)
                _BookProNumber = value
            End Set
        End Property

        Private _LaneOriginAddressUse As Boolean = False
        <DataMember()> _
        Public Property LaneOriginAddressUse() As Boolean
            Get
                Return _LaneOriginAddressUse
            End Get
            Set(ByVal value As Boolean)
                _LaneOriginAddressUse = value
            End Set
        End Property

        Private _BookStopNo As Integer = 0
        <DataMember()> _
        Public Property BookStopNo() As Integer
            Get
                Return _BookStopNo
            End Get
            Set(ByVal value As Integer)
                _BookStopNo = value
            End Set
        End Property

        Private _BookConsPrefix As String = ""
        <DataMember()> _
        Public Property BookConsPrefix() As String
            Get
                Return _BookConsPrefix
            End Get
            Set(ByVal value As String)
                _BookConsPrefix = value
            End Set
        End Property
#End Region

#Region " Public Methods"
        Public Overrides Function Clone() As DTOBaseClass
            Dim instance As New BookTruckLoadData
            instance = DirectCast(MemberwiseClone(), BookTruckLoadData)
            Return instance
        End Function
#End Region

    End Class

End Namespace
