Imports Microsoft.VisualBasic
Imports System
Imports System.Collections.Generic
Imports System.Linq
Imports System.Text
Imports System.Runtime.Serialization
Imports Ngl.Core.ChangeTracker

Namespace DataTransferObjects
    <DataContract()> _
    Public Class CarrInsExpPerShipAlert
        Inherits DTOBaseClass


#Region " Data Members"

        Private _CarrierControl As System.Nullable(Of Integer)
        <DataMember()> _
        Public Property CarrierControl() As System.Nullable(Of Integer)
            Get
                Return Me._CarrierControl
            End Get
            Set(ByVal value As System.Nullable(Of Integer))
                If (Me._CarrierControl.Equals(value) = False) Then
                    Me._CarrierControl = value
                End If
            End Set
        End Property

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

        Private _Exposure As System.Nullable(Of Decimal)
        <DataMember()> _
        Public Property Exposure() As System.Nullable(Of Decimal)
            Get
                Return Me._Exposure
            End Get
            Set(ByVal value As System.Nullable(Of Decimal))
                If (Me._Exposure.Equals(value) = False) Then
                    Me._Exposure = value
                End If
            End Set
        End Property

        Private _BookConsPrefix As String
        <DataMember()> _
        Public Property BookConsPrefix() As String
            Get
                Return Me._BookConsPrefix
            End Get
            Set(ByVal value As String)
                If (String.Equals(Me._BookConsPrefix, value) = False) Then
                    Me._BookConsPrefix = value
                End If
            End Set
        End Property

        Private _BookProNumber As String
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

        Private _ProductValue As System.Nullable(Of Decimal)
        <DataMember()> _
        Public Property ProductValue() As System.Nullable(Of Decimal)
            Get
                Return Me._ProductValue
            End Get
            Set(ByVal value As System.Nullable(Of Decimal))
                If (Me._ProductValue.Equals(value) = False) Then
                    Me._ProductValue = value
                End If
            End Set
        End Property

        Private _AlertDate As System.Nullable(Of Date)
        <DataMember()> _
        Public Property AlertDate() As System.Nullable(Of Date)
            Get
                Return Me._AlertDate
            End Get
            Set(ByVal value As System.Nullable(Of Date))
                If (Me._AlertDate.Equals(value) = False) Then
                    Me._AlertDate = value
                End If
            End Set
        End Property

        Private _Message As String
        <DataMember()> _
        Public Property Message() As String
            Get
                Return Me._Message
            End Get
            Set(ByVal value As String)
                If (String.Equals(Me._Message, value) = False) Then
                    Me._Message = value
                End If
            End Set
        End Property
#End Region

#Region " Public Methods"
        Public Overrides Function Clone() As DTOBaseClass
            Dim instance As New CarrInsExpPerShipAlert
            instance = DirectCast(MemberwiseClone(), CarrInsExpPerShipAlert)
            Return instance
        End Function

#End Region

    End Class
End Namespace


