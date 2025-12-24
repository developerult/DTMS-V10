Imports Microsoft.VisualBasic
Imports System
Imports System.Collections.Generic
Imports System.Linq
Imports System.Text
Imports System.Runtime.Serialization
Imports Ngl.Core.ChangeTracker
Imports DTO = Ngl.FreightMaster.Data.DataTransferObjects

Namespace DataTransferObjects
    <DataContract()> _
    Public Class BookFilters
        Inherits DTOBaseClass


#Region " Data Members"

        Private _VSolutionControl As Integer = 0
        <DataMember()> _
        Public Property VSolutionControl() As Integer
            Get
                Return _VSolutionControl
            End Get
            Set(ByVal value As Integer)
                _VSolutionControl = value
            End Set
        End Property

        Private _NatAccountNumber As Integer = 0
        <DataMember()> _
        Public Property NatAccountNumber() As Integer
            Get
                Return _NatAccountNumber
            End Get
            Set(ByVal value As Integer)
                _NatAccountNumber = value
            End Set
        End Property

        Private _CompControl As Integer = 0
        <DataMember()> _
        Public Property CompControl() As Integer
            Get
                Return _CompControl
            End Get
            Set(ByVal value As Integer)
                _CompControl = value
            End Set
        End Property

        Private _CompName As String = ""
        <DataMember()> _
        Public Property CompName() As String
            Get
                Return _CompName
            End Get
            Set(ByVal value As String)
                _CompName = value
            End Set
        End Property

        Private _CompNumber As Integer = 0
        <DataMember()> _
        Public Property CompNumber() As Integer
            Get
                Return _CompNumber
            End Get
            Set(ByVal value As Integer)
                _CompNumber = value
            End Set
        End Property

        Private _FromDate As System.Nullable(Of Date)
        <DataMember()> _
        Public Property FromDate() As System.Nullable(Of Date)
            Get
                Return _FromDate
            End Get
            Set(ByVal value As System.Nullable(Of Date))
                _FromDate = value
            End Set
        End Property

        Private _ToDate As System.Nullable(Of Date)
        <DataMember()> _
        Public Property ToDate() As System.Nullable(Of Date)
            Get
                Return _ToDate
            End Get
            Set(ByVal value As System.Nullable(Of Date))
                _ToDate = value
            End Set
        End Property

        Private _LoadDate_Or_ReqDate As Boolean = True
        <DataMember()> _
        Public Property LoadDate_Or_ReqDate() As Boolean
            Get
                Return _LoadDate_Or_ReqDate
            End Get
            Set(ByVal value As Boolean)
                _LoadDate_Or_ReqDate = value
            End Set
        End Property

        Private _HoldAfterDate As System.Nullable(Of Date)
        <DataMember()> _
        Public Property HoldAfterDate() As System.Nullable(Of Date)
            Get
                Return _HoldAfterDate
            End Get
            Set(ByVal value As System.Nullable(Of Date))
                _HoldAfterDate = value
            End Set
        End Property

        Private _OptOrigCompControl As Integer = 0
        <DataMember()> _
        Public Property OptOrigCompControl() As Integer
            Get
                Return _OptOrigCompControl
            End Get
            Set(ByVal value As Integer)
                _OptOrigCompControl = value
            End Set
        End Property

        Private _OptOrigCompName As String = ""
        <DataMember()> _
        Public Property OptOrigCompName() As String
            Get
                Return _OptOrigCompName
            End Get
            Set(ByVal value As String)
                _OptOrigCompName = value
            End Set
        End Property

        Private _OptOrigCompNumber As Integer = 0
        <DataMember()> _
        Public Property OptOrigCompNumber() As Integer
            Get
                Return _OptOrigCompNumber
            End Get
            Set(ByVal value As Integer)
                _OptOrigCompNumber = value
            End Set
        End Property

        Private _OriginStates As List(Of DTO.vLookupList)
        <DataMember()> _
        Public Property OriginStates() As List(Of DTO.vLookupList)
            Get
                Return _OriginStates
            End Get
            Set(ByVal value As List(Of DTO.vLookupList))
                _OriginStates = value
            End Set
        End Property


        Private _OriginCity As String = ""
        <DataMember()> _
        Public Property OriginCity() As String
            Get
                Return _OriginCity
            End Get
            Set(ByVal value As String)
                _OriginCity = value
            End Set
        End Property

        Private _OriginStartZip As String = ""
        <DataMember()> _
        Public Property OriginStartZip() As String
            Get
                Return _OriginStartZip
            End Get
            Set(ByVal value As String)
                _OriginStartZip = value
            End Set
        End Property

        Private _OriginStopZip As String = ""
        <DataMember()> _
        Public Property OriginStopZip() As String
            Get
                Return _OriginStopZip
            End Get
            Set(ByVal value As String)
                _OriginStopZip = value
            End Set
        End Property
         
        Private _DestStates As List(Of DTO.vLookupList)
        <DataMember()> _
        Public Property DestStates() As List(Of DTO.vLookupList)
            Get
                Return _DestStates
            End Get
            Set(ByVal value As List(Of DTO.vLookupList))
                _DestStates = value
            End Set
        End Property
         
        Private _DestCity As String = ""
        <DataMember()> _
        Public Property DestCity() As String
            Get
                Return _DestCity
            End Get
            Set(ByVal value As String)
                _DestCity = value
            End Set
        End Property

        Private _DestStartZip As String = ""
        <DataMember()> _
        Public Property DestStartZip() As String
            Get
                Return _DestStartZip
            End Get
            Set(ByVal value As String)
                _DestStartZip = value
            End Set
        End Property

        Private _DestStopZip As String = ""
        <DataMember()> _
        Public Property DestStopZip() As String
            Get
                Return _DestStopZip
            End Get
            Set(ByVal value As String)
                _DestStopZip = value
            End Set
        End Property
         
        Private _LaneControl As Integer = 0
        <DataMember()> _
        Public Property LaneControl() As Integer
            Get
                Return _LaneControl
            End Get
            Set(ByVal value As Integer)
                _LaneControl = value
            End Set
        End Property

        Private _LaneName As String = ""
        <DataMember()> _
        Public Property LaneName() As String
            Get
                Return _LaneName
            End Get
            Set(ByVal value As String)
                _LaneName = value
            End Set
        End Property

        Private _LaneNumber As Integer = ""
        <DataMember()> _
        Public Property LaneNumber() As Integer
            Get
                Return _LaneNumber
            End Get
            Set(ByVal value As Integer)
                _LaneNumber = value
            End Set
        End Property

        Private _TransTypeControl As Integer = ""
        <DataMember()> _
        Public Property TransTypeControl() As Integer
            Get
                Return _TransTypeControl
            End Get
            Set(ByVal value As Integer)
                _TransTypeControl = value
            End Set
        End Property
         
#End Region

#Region " Public Methods"
        Public Overrides Function Clone() As DTOBaseClass
            Dim instance As New BookFilters
            instance = DirectCast(MemberwiseClone(), BookFilters)
            Return instance
        End Function

#End Region

    End Class
End Namespace
