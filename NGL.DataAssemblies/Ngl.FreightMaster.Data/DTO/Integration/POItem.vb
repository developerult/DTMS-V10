Imports Microsoft.VisualBasic
Imports System
Imports System.Collections.Generic
Imports System.Linq
Imports System.Text
Imports System.Runtime.Serialization
Imports Ngl.Core.ChangeTracker

Namespace DataTransferObjects
    <DataContract()> _
    Public Class POItem
        Inherits DTOBaseClass


#Region " Data Members"

        Private _POItemControl As Long = 0
        <DataMember()> _
        Public Property POItemControl() As Long
            Get
                Return _POItemControl
            End Get
            Set(ByVal value As Long)
                _POItemControl = value
            End Set
        End Property

        Private _ItemPONumber As String = ""
        <DataMember()> _
        Public Property ItemPONumber() As String
            Get
                Return Left(_ItemPONumber, 20)
            End Get
            Set(ByVal value As String)
                _ItemPONumber = Left(value, 20)
            End Set
        End Property

        Private _FixOffInvAllow As New System.Nullable(Of Double)
        <DataMember()> _
        Public Property FixOffInvAllow() As System.Nullable(Of Double)
            Get
                Return _FixOffInvAllow
            End Get
            Set(ByVal value As System.Nullable(Of Double))
                _FixOffInvAllow = value
            End Set
        End Property

        Private _FixFrtAllow As New System.Nullable(Of Decimal)
        <DataMember()> _
        Public Property FixFrtAllow() As System.Nullable(Of Decimal)
            Get
                Return _FixFrtAllow
            End Get
            Set(ByVal value As System.Nullable(Of Decimal))
                _FixFrtAllow = value
            End Set
        End Property

        Private _ItemNumber As String = ""
        <DataMember()> _
        Public Property ItemNumber() As String
            Get
                Return Left(_ItemNumber, 50)
            End Get
            Set(ByVal value As String)
                _ItemNumber = Left(value, 50)
            End Set
        End Property

        Private _QtyOrdered As New System.Nullable(Of Integer)
        <DataMember()> _
        Public Property QtyOrdered() As System.Nullable(Of Integer)
            Get
                Return _QtyOrdered
            End Get
            Set(ByVal value As System.Nullable(Of Integer))
                _QtyOrdered = value
            End Set
        End Property

        Private _FreightCost As New System.Nullable(Of Decimal)
        <DataMember()> _
        Public Property FreightCost() As System.Nullable(Of Decimal)
            Get
                Return _FreightCost
            End Get
            Set(ByVal value As System.Nullable(Of Decimal))
                _FreightCost = value
            End Set
        End Property

        Private _ItemCost As New System.Nullable(Of Decimal)
        <DataMember()> _
        Public Property ItemCost() As System.Nullable(Of Decimal)
            Get
                Return _ItemCost
            End Get
            Set(ByVal value As System.Nullable(Of Decimal))
                _ItemCost = value
            End Set
        End Property

        Private _Weight As New System.Nullable(Of Double)
        <DataMember()> _
        Public Property Weight() As System.Nullable(Of Double)
            Get
                Return _Weight
            End Get
            Set(ByVal value As System.Nullable(Of Double))
                _Weight = value
            End Set
        End Property

        Private _Cube As New System.Nullable(Of Integer)
        <DataMember()> _
        Public Property Cube() As System.Nullable(Of Integer)
            Get
                Return _Cube
            End Get
            Set(ByVal value As System.Nullable(Of Integer))
                _Cube = value
            End Set
        End Property

        Private _Pack As New System.Nullable(Of Short)
        <DataMember()> _
        Public Property Pack() As System.Nullable(Of Short)
            Get
                Return _Pack
            End Get
            Set(ByVal value As System.Nullable(Of Short))
                _Pack = value
            End Set
        End Property

        Private _Size As String = ""
        <DataMember()> _
        Public Property Size() As String
            Get
                Return Left(_Size, 255)
            End Get
            Set(ByVal value As String)
                _Size = Left(value, 255)
            End Set
        End Property

        Private _Description As String = ""
        <DataMember()> _
        Public Property Description() As String
            Get
                Return Left(_Description, 255)
            End Get
            Set(ByVal value As String)
                _Description = Left(value, 255)
            End Set
        End Property

        Private _Hazmat As String = ""
        <DataMember()> _
        Public Property Hazmat() As String
            Get
                Return _Hazmat
            End Get
            Set(ByVal value As String)
                _Hazmat = Left(value, 1)
            End Set
        End Property

        Private _CreatedUser As String = ""
        <DataMember()> _
        Public Property CreatedUser() As String
            Get
                Return Left(_CreatedUser, 100)
            End Get
            Set(ByVal value As String)
                _CreatedUser = Left(value, 100)
            End Set
        End Property

        Private _CreatedDate As New System.Nullable(Of Date)
        <DataMember()> _
        Public Property CreatedDate() As System.Nullable(Of Date)
            Get
                Return _CreatedDate
            End Get
            Set(ByVal value As System.Nullable(Of Date))
                _CreatedDate = value
            End Set
        End Property

        Private _Brand As String = ""
        <DataMember()> _
        Public Property Brand() As String
            Get
                Return Left(_Brand, 255)
            End Get
            Set(ByVal value As String)
                _Brand = Left(value, 255)
            End Set
        End Property

        Private _CostCenter As String = ""
        <DataMember()> _
        Public Property CostCenter() As String
            Get
                Return Left(_CostCenter, 50)
            End Get
            Set(ByVal value As String)
                _CostCenter = Left(value, 50)
            End Set
        End Property

        Private _LotNumber As String = ""
        <DataMember()> _
        Public Property LotNumber() As String
            Get
                Return Left(_LotNumber, 50)
            End Get
            Set(ByVal value As String)
                _LotNumber = Left(value, 50)
            End Set
        End Property

        Private _LotExpirationDate As New System.Nullable(Of Date)
        <DataMember()> _
        Public Property LotExpirationDate() As System.Nullable(Of Date)
            Get
                Return _LotExpirationDate
            End Get
            Set(ByVal value As System.Nullable(Of Date))
                _LotExpirationDate = value
            End Set
        End Property

        Private _GTIN As String = ""
        <DataMember()> _
        Public Property GTIN() As String
            Get
                Return Left(_GTIN, 50)
            End Get
            Set(ByVal value As String)
                _GTIN = Left(value, 50)
            End Set
        End Property

        Private _CustItemNumber As String = ""
        <DataMember()> _
        Public Property CustItemNumber() As String
            Get
                Return Left(_CustItemNumber, 50)
            End Get
            Set(ByVal value As String)
                _CustItemNumber = Left(value, 50)
            End Set
        End Property

        Private _CustomerNumber As New System.Nullable(Of Integer)
        <DataMember()> _
        Public Property CustomerNumber() As System.Nullable(Of Integer)
            Get
                Return _CustomerNumber
            End Get
            Set(ByVal value As System.Nullable(Of Integer))
                _CustomerNumber = value
            End Set
        End Property

        Private _POOrderSequence As Integer = 0
        <DataMember()> _
        Public Property POOrderSequence() As Integer
            Get
                Return _POOrderSequence
            End Get
            Set(ByVal value As Integer)
                _POOrderSequence = value
            End Set
        End Property

        Private _PalletType As String = ""
        <DataMember()> _
        Public Property PalletType() As String
            Get
                Return Left(_PalletType, 50)
            End Get
            Set(ByVal value As String)
                _PalletType = Left(value, 50)
            End Set
        End Property


        Private _POItemHazmatTypeCode As String = ""
        <DataMember()> _
        Public Property POItemHazmatTypeCode As String
            Get
                Return Left(_POItemHazmatTypeCode, 20)
            End Get
            Set(value As String)
                _POItemHazmatTypeCode = Left(value, 20)
            End Set
        End Property

        Private _POItem49CFRCode As String = ""
        <DataMember()> _
        Public Property POItem49CFRCode As String
            Get
                Return Left(_POItem49CFRCode, 20)
            End Get
            Set(value As String)
                _POItem49CFRCode = Left(value, 20)
            End Set
        End Property

        Private _POItemIATACode As String = ""
        <DataMember()> _
        Public Property POItemIATACode As String
            Get
                Return Left(_POItemIATACode, 20)
            End Get
            Set(value As String)
                _POItemIATACode = Left(value, 20)
            End Set
        End Property

        Private _POItemDOTCode As String = ""
        <DataMember()> _
        Public Property POItemDOTCode As String
            Get
                Return Left(_POItemDOTCode, 20)
            End Get
            Set(value As String)
                _POItemDOTCode = Left(value, 20)
            End Set
        End Property

        Private _POItemMarineCode As String = ""
        <DataMember()> _
        Public Property POItemMarineCode As String
            Get
                Return Left(_POItemMarineCode, 20)
            End Get
            Set(value As String)
                _POItemMarineCode = Left(value, 20)
            End Set
        End Property

        Private _POItemNMFCClass As String = ""
        <DataMember()> _
        Public Property POItemNMFCClass As String
            Get
                Return Left(_POItemNMFCClass, 20)
            End Get
            Set(value As String)
                _POItemNMFCClass = Left(value, 20)
            End Set
        End Property

        Private _POItemFAKClass As String = ""
        <DataMember()> _
        Public Property POItemFAKClass As String
            Get
                Return Left(_POItemFAKClass, 20)
            End Get
            Set(value As String)
                _POItemFAKClass = Left(value, 20)
            End Set
        End Property

        Private _POItemLimitedQtyFlag As System.Nullable(Of Boolean)
        <DataMember()> _
        Public Property POItemLimitedQtyFlag As System.Nullable(Of Boolean)
            Get
                Return _POItemLimitedQtyFlag
            End Get
            Set(value As System.Nullable(Of Boolean))
                _POItemLimitedQtyFlag = value
            End Set
        End Property

        Private _POItemPallets As System.Nullable(Of Double)
        <DataMember()> _
        Public Property POItemPallets As System.Nullable(Of Double)
            Get
                Return _POItemPallets
            End Get
            Set(value As System.Nullable(Of Double))
                _POItemPallets = value
            End Set
        End Property

        Private _POItemTies As System.Nullable(Of Double)
        <DataMember()> _
        Public Property POItemTies As System.Nullable(Of Double)
            Get
                Return _POItemTies
            End Get
            Set(value As System.Nullable(Of Double))
                _POItemTies = value
            End Set
        End Property

        Private _POItemHighs As System.Nullable(Of Double)
        <DataMember()> _
        Public Property POItemHighs As System.Nullable(Of Double)
            Get
                Return _POItemHighs
            End Get
            Set(value As System.Nullable(Of Double))
                _POItemHighs = value
            End Set
        End Property

        Private _POItemQtyPalletPercentage As System.Nullable(Of Double)
        <DataMember()> _
        Public Property POItemQtyPalletPercentage As System.Nullable(Of Double)
            Get
                Return _POItemQtyPalletPercentage
            End Get
            Set(value As System.Nullable(Of Double))
                _POItemQtyPalletPercentage = value
            End Set
        End Property

        Private _POItemQtyLength As System.Nullable(Of Double)
        <DataMember()> _
        Public Property POItemQtyLength As System.Nullable(Of Double)
            Get
                Return _POItemQtyLength
            End Get
            Set(value As System.Nullable(Of Double))
                _POItemQtyLength = value
            End Set
        End Property

        Private _POItemQtyWidth As System.Nullable(Of Double)
        <DataMember()> _
        Public Property POItemQtyWidth As System.Nullable(Of Double)
            Get
                Return _POItemQtyWidth
            End Get
            Set(value As System.Nullable(Of Double))
                _POItemQtyWidth = value
            End Set
        End Property

        Private _POItemQtyHeight As System.Nullable(Of Double)
        <DataMember()> _
        Public Property POItemQtyHeight As System.Nullable(Of Double)
            Get
                Return _POItemQtyHeight
            End Get
            Set(value As System.Nullable(Of Double))
                _POItemQtyHeight = value
            End Set
        End Property


        Private _POItemStackable As System.Nullable(Of Boolean)
        <DataMember()> _
        Public Property POItemStackable() As System.Nullable(Of Boolean)
            Get
                Return _POItemStackable
            End Get
            Set(ByVal value As System.Nullable(Of Boolean))
                _POItemStackable = value
            End Set
        End Property


        Private _POItemLevelOfDensity As System.Nullable(Of Integer)
        <DataMember()> _
        Public Property POItemLevelOfDensity() As System.Nullable(Of Integer)
            Get
                Return _POItemLevelOfDensity
            End Get
            Set(ByVal value As System.Nullable(Of Integer))
                _POItemLevelOfDensity = value
            End Set
        End Property

        Private _POItemUpdated As Byte()
        <DataMember()> _
        Public Property POItemUpdated() As Byte()
            Get
                Return _POItemUpdated
            End Get
            Set(ByVal value As Byte())
                _POItemUpdated = value
            End Set
        End Property

        Private _POItemUser1 As String = ""
        <DataMember()> _
        Public Property POItemUser1() As String
            Get
                Return Left(Me._POItemUser1, 4000)
            End Get
            Set(value As String)
                If (String.Equals(Me._POItemUser1, value) = False) Then
                    Me._POItemUser1 = Left(value, 4000)
                    Me.SendPropertyChanged("POItemUser1")
                End If
            End Set
        End Property

        Private _POItemUser2 As String = ""
        <DataMember()> _
        Public Property POItemUser2() As String
            Get
                Return Left(Me._POItemUser2, 4000)
            End Get
            Set(value As String)
                If (String.Equals(Me._POItemUser2, value) = False) Then
                    Me._POItemUser2 = Left(value, 4000)
                    Me.SendPropertyChanged("POItemUser2")
                End If
            End Set
        End Property

        Private _POItemUser3 As String = ""
        <DataMember()> _
        Public Property POItemUser3() As String
            Get
                Return Left(Me._POItemUser3, 4000)
            End Get
            Set(value As String)
                If (String.Equals(Me._POItemUser3, value) = False) Then
                    Me._POItemUser3 = Left(value, 4000)
                    Me.SendPropertyChanged("POItemUser3")
                End If
            End Set
        End Property

        Private _POItemUser4 As String = ""
        <DataMember()> _
        Public Property POItemUser4() As String
            Get
                Return Left(Me._POItemUser4, 4000)
            End Get
            Set(value As String)
                If (String.Equals(Me._POItemUser4, value) = False) Then
                    Me._POItemUser4 = Left(value, 4000)
                    Me.SendPropertyChanged("POItemUser4")
                End If
            End Set
        End Property

        Private _POItemAlphaCode As String = ""
        <DataMember()> _
        Public Property POItemCompAlphaCode() As String
            Get
                Return Left(_POItemAlphaCode, 50)
            End Get
            Set(ByVal value As String)
                _POItemAlphaCode = Left(value, 50)
                Me.SendPropertyChanged("POItemCompAlphaCode")
            End Set
        End Property

        Private _POItemLegalEntity As String = ""
        <DataMember()> _
        Public Property POItemCompLegalEntity() As String
            Get
                Return Left(_POItemLegalEntity, 50)
            End Get
            Set(ByVal value As String)
                _POItemLegalEntity = Left(value, 50)
                Me.SendPropertyChanged("POItemCompLegalEntity")
            End Set
        End Property

        Private _POItemNMFCSubClass As String = ""
        <DataMember()> _
        Public Property POItemNMFCSubClass() As String
            Get
                Return Left(_POItemNMFCSubClass, 20)
            End Get
            Set(ByVal value As String)
                _POItemNMFCSubClass = Left(value, 20)
                Me.SendPropertyChanged("POItemNMFCSubClass")
            End Set
        End Property

#End Region

#Region " Public Methods"
        Public Overrides Function Clone() As DTOBaseClass
            Dim instance As New POItem
            instance = DirectCast(MemberwiseClone(), POItem)
            Return instance
        End Function

#End Region

    End Class
End Namespace
