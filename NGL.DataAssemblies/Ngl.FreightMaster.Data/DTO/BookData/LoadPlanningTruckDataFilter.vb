Imports Microsoft.VisualBasic
Imports System
Imports System.Collections.Generic
Imports System.Linq
Imports System.Text
Imports System.Runtime.Serialization
Imports Ngl.Core.ChangeTracker

Namespace DataTransferObjects
    <DataContract()> _
    Public Class LoadPlanningTruckDataFilter
        Inherits DTOBaseClass


#Region " Data Members"
        Private _BookConsPrefixFilter As String
        <DataMember()> _
        Public Property BookConsPrefixFilter() As String
            Get
                Return Left(Me._BookConsPrefixFilter, 20)
            End Get
            Set(ByVal value As String)
                If (String.Equals(Me._BookConsPrefixFilter, value) = False) Then
                    Me._BookConsPrefixFilter = Left(value, 20)
                End If
            End Set
        End Property

        Private _CompControlFilter As Integer
        <DataMember()> _
        Public Property CompControlFilter() As Integer
            Get
                Return _CompControlFilter
            End Get
            Set(ByVal value As Integer)
                If (Me._CompControlFilter.Equals(value) = False) Then
                    Me._CompControlFilter = value
                End If
            End Set
        End Property

        Private _CarrierControlFilter As Integer
        <DataMember()> _
        Public Property CarrierControlFilter() As Integer
            Get
                Return _CarrierControlFilter
            End Get
            Set(ByVal value As Integer)
                If (Me._CarrierControlFilter.Equals(value) = False) Then
                    Me._CarrierControlFilter = value
                End If
            End Set
        End Property

        Private _StartDateFilter As Date
        <DataMember()> _
        Public Property StartDateFilter() As Date
            Get
                Return _StartDateFilter
            End Get
            Set(ByVal value As Date)
                If (String.Equals(Me._StartDateFilter, value) = False) Then
                    Me._StartDateFilter = value
                End If
            End Set
        End Property

        Private _StopDateFilter As Date
        <DataMember()> _
        Public Property StopDateFilter() As Date
            Get
                Return _StopDateFilter
            End Get
            Set(ByVal value As Date)
                If (String.Equals(Me._StopDateFilter, value) = False) Then
                    Me._StopDateFilter = value
                End If
            End Set
        End Property

        Private _OrigStartZipFilter As String
        <DataMember()> _
        Public Property OrigStartZipFilter() As String
            Get
                Return Left(Me._OrigStartZipFilter, 10)
            End Get
            Set(ByVal value As String)
                If (String.Equals(Me._OrigStartZipFilter, value) = False) Then
                    Me._OrigStartZipFilter = Left(value, 10)
                End If
            End Set
        End Property

        Private _OrigStopZipFilter As String
        <DataMember()> _
        Public Property OrigStopZipFilter() As String
            Get
                Return Left(Me._OrigStopZipFilter, 10)
            End Get
            Set(ByVal value As String)
                If (String.Equals(Me._OrigStopZipFilter, value) = False) Then
                    Me._OrigStopZipFilter = Left(value, 10)
                End If
            End Set
        End Property

        Private _DestStartZipFilter As String
        <DataMember()> _
        Public Property DestStartZipFilter() As String
            Get
                Return Left(Me._DestStartZipFilter, 10)
            End Get
            Set(ByVal value As String)
                If (String.Equals(Me._DestStartZipFilter, value) = False) Then
                    Me._DestStartZipFilter = Left(value, 10)
                End If
            End Set
        End Property

        Private _DestStopZipFilter As String
        <DataMember()> _
        Public Property DestStopZipFilter() As String
            Get
                Return Left(Me._DestStopZipFilter, 10)
            End Get
            Set(ByVal value As String)
                If (String.Equals(Me._DestStopZipFilter, value) = False) Then
                    Me._DestStopZipFilter = Left(value, 10)
                End If
            End Set
        End Property

        Private _OrigCityFilter As String
        <DataMember()> _
        Public Property OrigCityFilter() As String
            Get
                Return Left(Me._OrigCityFilter, 25)
            End Get
            Set(ByVal value As String)
                If (String.Equals(Me._OrigCityFilter, value) = False) Then
                    Me._OrigCityFilter = Left(value, 25)
                End If
            End Set
        End Property

        Private _DestCityFilter As String
        <DataMember()> _
        Public Property DestCityFilter() As String
            Get
                Return Left(Me._DestCityFilter, 25)
            End Get
            Set(ByVal value As String)
                If (String.Equals(Me._DestCityFilter, value) = False) Then
                    Me._DestCityFilter = Left(value, 25)
                End If
            End Set
        End Property

        Private _OrigSt1Filter As String
        <DataMember()> _
        Public Property OrigSt1Filter() As String
            Get
                Return Left(Me._OrigSt1Filter, 8)
            End Get
            Set(ByVal value As String)
                If (String.Equals(Me._OrigSt1Filter, value) = False) Then
                    Me._OrigSt1Filter = Left(value, 8)
                End If
            End Set
        End Property

        Private _OrigSt2Filter As String
        <DataMember()> _
        Public Property OrigSt2Filter() As String
            Get
                Return Left(Me._OrigSt2Filter, 8)
            End Get
            Set(ByVal value As String)
                If (String.Equals(Me._OrigSt2Filter, value) = False) Then
                    Me._OrigSt2Filter = Left(value, 8)
                End If
            End Set
        End Property

        Private _OrigSt3Filter As String
        <DataMember()> _
        Public Property OrigSt3Filter() As String
            Get
                Return Left(Me._OrigSt3Filter, 8)
            End Get
            Set(ByVal value As String)
                If (String.Equals(Me._OrigSt3Filter, value) = False) Then
                    Me._OrigSt3Filter = Left(value, 8)
                End If
            End Set
        End Property

        Private _OrigSt4Filter As String
        <DataMember()> _
        Public Property OrigSt4Filter() As String
            Get
                Return Left(Me._OrigSt4Filter, 8)
            End Get
            Set(ByVal value As String)
                If (String.Equals(Me._OrigSt4Filter, value) = False) Then
                    Me._OrigSt4Filter = Left(value, 8)
                End If
            End Set
        End Property

        Private _DestSt1Filter As String
        <DataMember()> _
        Public Property DestSt1Filter() As String
            Get
                Return Left(Me._DestSt1Filter, 8)
            End Get
            Set(ByVal value As String)
                If (String.Equals(Me._DestSt1Filter, value) = False) Then
                    Me._DestSt1Filter = Left(value, 8)
                End If
            End Set
        End Property

        Private _DestSt2Filter As String
        <DataMember()> _
        Public Property DestSt2Filter() As String
            Get
                Return Left(Me._DestSt2Filter, 8)
            End Get
            Set(ByVal value As String)
                If (String.Equals(Me._DestSt2Filter, value) = False) Then
                    Me._DestSt2Filter = Left(value, 8)
                End If
            End Set
        End Property

        Private _DestSt3Filter As String
        <DataMember()> _
        Public Property DestSt3Filter() As String
            Get
                Return Left(Me._DestSt3Filter, 8)
            End Get
            Set(ByVal value As String)
                If (String.Equals(Me._DestSt3Filter, value) = False) Then
                    Me._DestSt3Filter = Left(value, 8)
                End If
            End Set
        End Property

        Private _DestSt4Filter As String
        <DataMember()> _
        Public Property DestSt4Filter() As String
            Get
                Return Left(Me._DestSt4Filter, 8)
            End Get
            Set(ByVal value As String)
                If (String.Equals(Me._DestSt4Filter, value) = False) Then
                    Me._DestSt4Filter = Left(value, 8)
                End If
            End Set
        End Property

        Private _UseLoadDateFilter As Boolean
        <DataMember()> _
        Public Property UseLoadDateFilter() As Boolean
            Get
                Return _UseLoadDateFilter
            End Get
            Set(ByVal value As Boolean)
                If (Me._UseLoadDateFilter.Equals(value) = False) Then
                    Me._UseLoadDateFilter = value
                End If
            End Set
        End Property

        Private _LaneNumberFilter As String
        <DataMember()> _
        Public Property LaneNumberFilter() As String
            Get
                Return Left(Me._LaneNumberFilter, 50)
            End Get
            Set(ByVal value As String)
                If (String.Equals(Me._LaneNumberFilter, value) = False) Then
                    Me._LaneNumberFilter = Left(value, 50)
                End If
            End Set
        End Property

        Private _BookTransTypeFilter As String
        <DataMember()> _
        Public Property BookTransTypeFilter() As String
            Get
                Return Left(Me._BookTransTypeFilter, 50)
            End Get
            Set(ByVal value As String)
                If (String.Equals(Me._BookTransTypeFilter, value) = False) Then
                    Me._BookTransTypeFilter = Left(value, 50)
                End If
            End Set
        End Property

        Private _BookTranCodeFilter As String = "P"
        <DataMember()> _
        Public Property BookTranCodeFilter() As String
            Get
                Return Left(Me._BookTranCodeFilter, 3)
            End Get
            Set(ByVal value As String)
                If (String.Equals(Me._BookTranCodeFilter, value) = False) Then
                    Me._BookTranCodeFilter = Left(value, 3)
                End If
            End Set
        End Property
#End Region

#Region " Public Methods"
        Public Overrides Function Clone() As DTOBaseClass
            Dim instance As New LoadPlanningTruckDataFilter
            instance = DirectCast(MemberwiseClone(), LoadPlanningTruckDataFilter)
            Return instance
        End Function

#End Region

    End Class
End Namespace