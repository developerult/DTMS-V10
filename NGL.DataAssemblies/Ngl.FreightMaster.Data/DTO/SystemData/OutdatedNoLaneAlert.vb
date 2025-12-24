Imports Microsoft.VisualBasic
Imports System
Imports System.Collections.Generic
Imports System.Linq
Imports System.Text
Imports System.Runtime.Serialization
Imports Ngl.Core.ChangeTracker

Namespace DataTransferObjects
    <DataContract()> _
    Public Class OutdatedNoLaneAlert
        Inherits DTOBaseClass


#Region " Data Members"

        Private _POHNLOrderNumber As String
        <DataMember()> _
        Public Property POHNLOrderNumber() As String
            Get
                Return Me._POHNLOrderNumber
            End Get
            Set(ByVal value As String)
                If (String.Equals(Me._POHNLOrderNumber, value) = False) Then
                    Me._POHNLOrderNumber = value
                End If
            End Set
        End Property

        Private _POHNLvendor As String
        <DataMember()> _
        Public Property POHNLvendor() As String
            Get
                Return Me._POHNLvendor
            End Get
            Set(ByVal value As String)
                If (String.Equals(Me._POHNLvendor, value) = False) Then
                    Me._POHNLvendor = value
                End If
            End Set
        End Property

        Private _POHNLnumber As String
        <DataMember()> _
        Public Property POHNLnumber() As String
            Get
                Return Me._POHNLnumber
            End Get
            Set(ByVal value As String)
                If (String.Equals(Me._POHNLnumber, value) = False) Then
                    Me._POHNLnumber = value
                End If
            End Set
        End Property

        Private _POHNLPOdate As System.Nullable(Of Date)
        <DataMember()> _
        Public Property POHNLPOdate() As System.Nullable(Of Date)
            Get
                Return Me._POHNLPOdate
            End Get
            Set(ByVal value As System.Nullable(Of Date))
                If (Me._POHNLPOdate.Equals(value) = False) Then
                    Me._POHNLPOdate = value
                End If
            End Set
        End Property

        Private _POHNLShipdate As System.Nullable(Of Date)
        <DataMember()> _
        Public Property POHNLShipdate() As System.Nullable(Of Date)
            Get
                Return Me._POHNLShipdate
            End Get
            Set(ByVal value As System.Nullable(Of Date))
                If (Me._POHNLShipdate.Equals(value) = False) Then
                    Me._POHNLShipdate = value
                End If
            End Set
        End Property

        Private _POHNLCreateDate As System.Nullable(Of Date)
        <DataMember()> _
        Public Property POHNLCreateDate() As System.Nullable(Of Date)
            Get
                Return Me._POHNLCreateDate
            End Get
            Set(ByVal value As System.Nullable(Of Date))
                If (Me._POHNLCreateDate.Equals(value) = False) Then
                    Me._POHNLCreateDate = value
                End If
            End Set
        End Property

        Private _POHNLDefaultCustomer As System.Nullable(Of Integer)
        <DataMember()> _
        Public Property POHNLDefaultCustomer() As System.Nullable(Of Integer)
            Get
                Return Me._POHNLDefaultCustomer
            End Get
            Set(ByVal value As System.Nullable(Of Integer))
                If (Me._POHNLDefaultCustomer.Equals(value) = False) Then
                    Me._POHNLDefaultCustomer = value
                End If
            End Set
        End Property

        Private _POHNLReqDate As System.Nullable(Of Date)
        <DataMember()> _
        Public Property POHNLReqDate() As System.Nullable(Of Date)
            Get
                Return Me._POHNLReqDate
            End Get
            Set(ByVal value As System.Nullable(Of Date))
                If (Me._POHNLReqDate.Equals(value) = False) Then
                    Me._POHNLReqDate = value
                End If
            End Set
        End Property

        Private _ItemNumber As String
        <DataMember()> _
        Public Property ItemNumber() As String
            Get
                Return Me._ItemNumber
            End Get
            Set(ByVal value As String)
                If (String.Equals(Me._ItemNumber, value) = False) Then
                    Me._ItemNumber = value
                End If
            End Set
        End Property

        Private _QtyOrdered As System.Nullable(Of Integer)
        <DataMember()> _
        Public Property QtyOrdered() As System.Nullable(Of Integer)
            Get
                Return Me._QtyOrdered
            End Get
            Set(ByVal value As System.Nullable(Of Integer))
                If (Me._QtyOrdered.Equals(value) = False) Then
                    Me._QtyOrdered = value
                End If
            End Set
        End Property

        Private _Description As String
        <DataMember()> _
        Public Property Description() As String
            Get
                Return Me._Description
            End Get
            Set(ByVal value As String)
                If (String.Equals(Me._Description, value) = False) Then
                    Me._Description = value
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

        Private _CompName As String
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

#End Region

#Region " Public Methods"
        Public Overrides Function Clone() As DTOBaseClass
            Dim instance As New OutdatedNoLaneAlert
            instance = DirectCast(MemberwiseClone(), OutdatedNoLaneAlert)
            Return instance
        End Function

#End Region

    End Class
End Namespace


