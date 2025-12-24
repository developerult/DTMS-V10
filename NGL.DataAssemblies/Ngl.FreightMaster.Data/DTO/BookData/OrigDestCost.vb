Imports Microsoft.VisualBasic
Imports System
Imports System.Collections.Generic
Imports System.Linq
Imports System.Text
Imports System.Runtime.Serialization
Imports Ngl.Core.ChangeTracker

Namespace DataTransferObjects
    <DataContract()> _
    Public Class OrigDestCost
        Inherits DTOBaseClass


#Region " Data Members"
        Private _CarrierNumber As System.Nullable(Of Integer)
        <DataMember()> _
        Public Property CarrierNumber() As System.Nullable(Of Integer)
            Get
                Return Me._CarrierNumber
            End Get
            Set(ByVal value As System.Nullable(Of Integer))
                If (Me._CarrierNumber.Equals(value) = False) Then
                    Me._CarrierNumber = value
                End If
            End Set
        End Property

        Private _CompName As String = ""
        <DataMember()> _
        Public Property CompName() As String
            Get
                Return Me._CompName
            End Get
            Set(ByVal value As String)
                If (String.Equals(Me._CompName, value) = False) Then
                    Me._CompName = value
                End If
            End Set
        End Property

        Private _BookOrigCity As String = ""
        <DataMember()> _
        Public Property BookOrigCity() As String
            Get
                Return Me._BookOrigCity
            End Get
            Set(ByVal value As String)
                If (String.Equals(Me._BookOrigCity, value) = False) Then
                    Me._BookOrigCity = value
                End If
            End Set
        End Property

        Private _BookOrigState As String = ""
        <DataMember()> _
        Public Property BookOrigState() As String
            Get
                Return Me._BookOrigState
            End Get
            Set(ByVal value As String)
                If (String.Equals(Me._BookOrigState, value) = False) Then
                    Me._BookOrigState = value
                End If
            End Set
        End Property

        Private _BookDestCity As String = ""
        <DataMember()> _
        Public Property BookDestCity() As String
            Get
                Return Me._BookDestCity
            End Get
            Set(ByVal value As String)
                If (String.Equals(Me._BookDestCity, value) = False) Then
                    Me._BookDestCity = value
                End If
            End Set
        End Property

        Private _BookDestState As String = ""
        <DataMember()> _
        Public Property BookDestState() As String
            Get
                Return Me._BookDestState
            End Get
            Set(ByVal value As String)
                If (String.Equals(Me._BookDestState, value) = False) Then
                    Me._BookDestState = value
                End If
            End Set
        End Property

        Private _BookDateLoad As System.Nullable(Of Date)
        <DataMember()> _
        Public Property BookDateLoad() As System.Nullable(Of Date)
            Get
                Return Me._BookDateLoad
            End Get
            Set(ByVal value As System.Nullable(Of Date))
                If (Me._BookDateLoad.Equals(value) = False) Then
                    Me._BookDateLoad = value
                End If
            End Set
        End Property

        Private _BookTotalWgt As System.Nullable(Of Double)
        <DataMember()> _
        Public Property BookTotalWgt() As System.Nullable(Of Double)
            Get
                Return Me._BookTotalWgt
            End Get
            Set(ByVal value As System.Nullable(Of Double))
                If (Me._BookTotalWgt.Equals(value) = False) Then
                    Me._BookTotalWgt = value
                End If
            End Set
        End Property

        Private _BookTotalBFC As System.Nullable(Of Decimal)
        <DataMember()> _
        Public Property BookTotalBFC() As System.Nullable(Of Decimal)
            Get
                Return Me._BookTotalBFC
            End Get
            Set(ByVal value As System.Nullable(Of Decimal))
                If (Me._BookTotalBFC.Equals(value) = False) Then
                    Me._BookTotalBFC = value
                End If
            End Set
        End Property

        Private _CarrierName As String = ""
        <DataMember()> _
        Public Property CarrierName() As String
            Get
                Return Me._CarrierName
            End Get
            Set(ByVal value As String)
                If (String.Equals(Me._CarrierName, value) = False) Then
                    Me._CarrierName = value
                End If
            End Set
        End Property

        Private _BookProNumber As String = ""
        <DataMember()> _
        Public Property BookProNumber() As String
            Get
                Return Me._BookProNumber
            End Get
            Set(ByVal value As String)
                If (String.Equals(Me._BookProNumber, value) = False) Then
                    Me._BookProNumber = value
                End If
            End Set
        End Property

        Private _BookControl As Integer
        <DataMember()> _
        Public Property BookControl() As Integer
            Get
                Return Me._BookControl
            End Get
            Set(ByVal value As Integer)
                If ((Me._BookControl = value) _
                   = False) Then
                    Me._BookControl = value
                End If
            End Set
        End Property

        Private _CompNumber As System.Nullable(Of Integer)
        <DataMember()> _
        Public Property CompNumber() As System.Nullable(Of Integer)
            Get
                Return Me._CompNumber
            End Get
            Set(ByVal value As System.Nullable(Of Integer))
                If (Me._CompNumber.Equals(value) = False) Then
                    Me._CompNumber = value
                End If
            End Set
        End Property

#End Region

#Region " Public Methods"
        Public Overrides Function Clone() As DTOBaseClass
            Dim instance As New OrigDestCost
            instance = DirectCast(MemberwiseClone(), OrigDestCost)
            Return instance
        End Function

#End Region

    End Class
End Namespace

