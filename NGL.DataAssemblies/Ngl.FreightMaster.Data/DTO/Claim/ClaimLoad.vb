Imports Microsoft.VisualBasic
Imports System
Imports System.Collections.Generic
Imports System.Linq
Imports System.Text
Imports System.Runtime.Serialization
Imports Ngl.Core.ChangeTracker

Namespace DataTransferObjects
    <DataContract()> _
    Public Class ClaimLoad
        Inherits DTOBaseClass


#Region " Data Members"

        Private _ClaimLoadControl As Integer = 0

        Private _ClaimLoadClaimControl As Integer = 0

        Private _ClaimLoadType As String = ""

        Private _ClaimLoadItem As String = ""

        Private _ClaimLoadDesc As String = ""

        Private _ClaimLoadUnitCost As Decimal = 0

        Private _ClaimLoadUnitFrt As Decimal = 0

        Private _ClaimLoadUnitQty As Integer = 0

        Private _ClaimLoadLineCost As Decimal = 0

        Private _ClaimLoadUpdated As Byte()


        <DataMember()> _
        Public Property ClaimLoadControl() As Integer
            Get
                Return Me._ClaimLoadControl
            End Get
            Set(ByVal value As Integer)
                If ((Me._ClaimLoadControl = value) _
                   = False) Then
                    Me._ClaimLoadControl = value
                    Me.SendPropertyChanged("ClaimLoadControl")
                End If
            End Set
        End Property

        <DataMember()> _
        Public Property ClaimLoadClaimControl() As Integer
            Get
                Return Me._ClaimLoadClaimControl
            End Get
            Set(ByVal value As Integer)
                If ((Me._ClaimLoadClaimControl = value) = False) Then
                    Me._ClaimLoadClaimControl = value
                    Me.SendPropertyChanged("ClaimLoadClaimControl")
                End If
            End Set
        End Property

        <DataMember()> _
        Public Property ClaimLoadType() As String
            Get
                Return Left(Me._ClaimLoadType, 20)
            End Get
            Set(ByVal value As String)
                If (String.Equals(Me._ClaimLoadType, value) = False) Then
                    Me._ClaimLoadType = Left(value, 20)
                    Me.SendPropertyChanged("ClaimLoadType")
                End If
            End Set
        End Property

        <DataMember()> _
        Public Property ClaimLoadItem() As String
            Get
                Return Left(Me._ClaimLoadItem, 15)
            End Get
            Set(ByVal value As String)
                If (String.Equals(Me._ClaimLoadItem, value) = False) Then
                    Me._ClaimLoadItem = Left(value, 15)
                    Me.SendPropertyChanged("ClaimLoadItem")
                End If
            End Set
        End Property

        <DataMember()> _
        Public Property ClaimLoadDesc() As String
            Get
                Return Left(Me._ClaimLoadDesc, 20)
            End Get
            Set(ByVal value As String)
                If (String.Equals(Me._ClaimLoadDesc, value) = False) Then
                    Me._ClaimLoadDesc = Left(value, 20)
                    Me.SendPropertyChanged("ClaimLoadDesc")
                End If
            End Set
        End Property

        <DataMember()> _
        Public Property ClaimLoadUnitCost() As Decimal
            Get
                Return Me._ClaimLoadUnitCost
            End Get
            Set(ByVal value As Decimal)
                If ((Me._ClaimLoadUnitCost = value) = False) Then
                    Me._ClaimLoadUnitCost = value
                    Me.SendPropertyChanged("ClaimLoadUnitCost")
                End If
            End Set
        End Property

        <DataMember()> _
        Public Property ClaimLoadUnitFrt() As Decimal
            Get
                Return Me._ClaimLoadUnitFrt
            End Get
            Set(ByVal value As Decimal)
                If ((Me._ClaimLoadUnitFrt = value) = False) Then
                    Me._ClaimLoadUnitFrt = value
                    Me.SendPropertyChanged("ClaimLoadUnitFrt")
                End If
            End Set
        End Property

        <DataMember()> _
        Public Property ClaimLoadUnitQty() As Integer
            Get
                Return Me._ClaimLoadUnitQty
            End Get
            Set(ByVal value As Integer)
                If ((Me._ClaimLoadUnitQty = value) = False) Then
                    Me._ClaimLoadUnitQty = value
                    Me.SendPropertyChanged("ClaimLoadUnitQty")
                End If
            End Set
        End Property

        <DataMember()> _
        Public Property ClaimLoadLineCost() As Decimal
            Get
                Return Me._ClaimLoadLineCost
            End Get
            Set(ByVal value As Decimal)
                If ((Me._ClaimLoadLineCost = value) = False) Then
                    Me._ClaimLoadLineCost = value
                    Me.SendPropertyChanged("ClaimLoadLineCost")
                End If
            End Set
        End Property

        <DataMember()> _
        Public Property ClaimLoadUpdated() As Byte()
            Get
                Return Me._ClaimLoadUpdated
            End Get
            Set(ByVal value As Byte())
                _ClaimLoadUpdated = value
                Me.SendPropertyChanged("ClaimLoadUpdated")
            End Set
        End Property

#End Region

#Region " Public Methods"
        Public Overrides Function Clone() As DTOBaseClass
            Dim instance As New ClaimLoad
            instance = DirectCast(MemberwiseClone(), ClaimLoad)
            Return instance
        End Function

#End Region

    End Class

End Namespace