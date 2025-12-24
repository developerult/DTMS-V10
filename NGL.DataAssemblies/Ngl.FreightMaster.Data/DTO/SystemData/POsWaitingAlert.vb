Imports Microsoft.VisualBasic
Imports System
Imports System.Collections.Generic
Imports System.Linq
Imports System.Text
Imports System.Runtime.Serialization
Imports Ngl.Core.ChangeTracker

Namespace DataTransferObjects
    <DataContract()> _
    Public Class POsWaitingAlert
        Inherits DTOBaseClass


#Region " Data Members"

        Private _PONumber As String
        <DataMember()> _
        Public Property PONumber() As String
            Get
                Return Me._PONumber
            End Get
            Set(ByVal value As String)
                If (String.Equals(Me._PONumber, value) = False) Then
                    Me._PONumber = value
                End If
            End Set
        End Property

        Private _OrderNumber As String
        <DataMember()> _
        Public Property OrderNumber() As String
            Get
                Return Me._OrderNumber
            End Get
            Set(ByVal value As String)
                If (String.Equals(Me._OrderNumber, value) = False) Then
                    Me._OrderNumber = value
                End If
            End Set
        End Property

        Private _PRONumber As String
        <DataMember()> _
        Public Property PRONumber() As String
            Get
                Return Me._PRONumber
            End Get
            Set(ByVal value As String)
                If (String.Equals(Me._PRONumber, value) = False) Then
                    Me._PRONumber = value
                End If
            End Set
        End Property

        Private _VendorNumber As String
        <DataMember()> _
        Public Property VendorNumber() As String
            Get
                Return Me._VendorNumber
            End Get
            Set(ByVal value As String)
                If (String.Equals(Me._VendorNumber, value) = False) Then
                    Me._VendorNumber = value
                End If
            End Set
        End Property

        Private _PODate As System.Nullable(Of Date)
        <DataMember()> _
        Public Property PODate() As System.Nullable(Of Date)
            Get
                Return Me._PODate
            End Get
            Set(ByVal value As System.Nullable(Of Date))
                If (Me._PODate.Equals(value) = False) Then
                    Me._PODate = value
                End If
            End Set
        End Property

        Private _ShipDate As System.Nullable(Of Date)
        <DataMember()> _
        Public Property ShipDate() As System.Nullable(Of Date)
            Get
                Return Me._ShipDate
            End Get
            Set(ByVal value As System.Nullable(Of Date))
                If (Me._ShipDate.Equals(value) = False) Then
                    Me._ShipDate = value
                End If
            End Set
        End Property

        Private _CreateUser As String
        <DataMember()> _
        Public Property CreateUser() As String
            Get
                Return Me._CreateUser
            End Get
            Set(ByVal value As String)
                If (String.Equals(Me._CreateUser, value) = False) Then
                    Me._CreateUser = value
                End If
            End Set
        End Property

        Private _CreateDate As System.Nullable(Of Date)
        <DataMember()> _
        Public Property CreateDate() As System.Nullable(Of Date)
            Get
                Return Me._CreateDate
            End Get
            Set(ByVal value As System.Nullable(Of Date))
                If (Me._CreateDate.Equals(value) = False) Then
                    Me._CreateDate = value
                End If
            End Set
        End Property

        Private _DefaultCustomer As System.Nullable(Of Integer)
        <DataMember()> _
        Public Property DefaultCustomer() As System.Nullable(Of Integer)
            Get
                Return Me._DefaultCustomer
            End Get
            Set(ByVal value As System.Nullable(Of Integer))
                If (Me._DefaultCustomer.Equals(value) = False) Then
                    Me._DefaultCustomer = value
                End If
            End Set
        End Property

        Private _CompControl As System.Nullable(Of Integer)
        <DataMember()> _
        Public Property CompControl() As System.Nullable(Of Integer)
            Get
                Return Me._CompControl
            End Get
            Set(ByVal value As System.Nullable(Of Integer))
                If (Me._CompControl.Equals(value) = False) Then
                    Me._CompControl = value
                End If
            End Set
        End Property

        Private _DefaultCustomerName As String
        <DataMember()> _
        Public Property DefaultCustomerName() As String
            Get
                Return Me._DefaultCustomerName
            End Get
            Set(ByVal value As String)
                If (String.Equals(Me._DefaultCustomerName, value) = False) Then
                    Me._DefaultCustomerName = value
                End If
            End Set
        End Property

        Private _DefaultCarrier As System.Nullable(Of Integer)
        <DataMember()> _
        Public Property DefaultCarrier() As System.Nullable(Of Integer)
            Get
                Return Me._DefaultCarrier
            End Get
            Set(ByVal value As System.Nullable(Of Integer))
                If (Me._DefaultCarrier.Equals(value) = False) Then
                    Me._DefaultCarrier = value
                End If
            End Set
        End Property

        Private _Warnings As String
        <DataMember()> _
        Public Property Warnings() As String
            Get
                Return Me._Warnings
            End Get
            Set(ByVal value As String)
                If (String.Equals(Me._Warnings, value) = False) Then
                    Me._Warnings = value
                End If
            End Set
        End Property

        Private _ImportType As String
        <DataMember()> _
        Public Property ImportType() As String
            Get
                Return Me._ImportType
            End Get
            Set(ByVal value As String)
                If (String.Equals(Me._ImportType, value) = False) Then
                    Me._ImportType = value
                End If
            End Set
        End Property

#End Region

#Region " Public Methods"
        Public Overrides Function Clone() As DTOBaseClass
            Dim instance As New POsWaitingAlert
            instance = DirectCast(MemberwiseClone(), POsWaitingAlert)
            Return instance
        End Function

#End Region

    End Class
End Namespace


