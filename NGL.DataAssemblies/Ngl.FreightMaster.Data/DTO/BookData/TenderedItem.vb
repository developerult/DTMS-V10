Imports Microsoft.VisualBasic
Imports System
Imports System.Collections.Generic
Imports System.Linq
Imports System.Text
Imports System.Runtime.Serialization
Imports Ngl.Core.ChangeTracker

Namespace DataTransferObjects
    <DataContract()> _
    Public Class TenderedItem
        Inherits DTOBaseClass


#Region " Data Members"
         
        <DataMember()> Public Property Control() As Integer
            Get
                Return m_Control
            End Get
            Set(ByVal value As Integer)
                m_Control = value
            End Set
        End Property
        Private m_Control As Integer
        <DataMember()> Public Property ProNumber() As String
            Get
                Return m_ProNumber
            End Get
            Set(ByVal value As String)
                m_ProNumber = value
            End Set
        End Property
        Private m_ProNumber As String
        <DataMember()> Public Property CnsNumber() As String
            Get
                Return m_CnsNumber
            End Get
            Set(ByVal value As String)
                m_CnsNumber = Value
            End Set
        End Property
        Private m_CnsNumber As String
        <DataMember()> Public Property CnsIntegrity() As Boolean
            Get
                Return m_CnsIntegrity
            End Get
            Set(ByVal value As Boolean)
                m_CnsIntegrity = value
            End Set
        End Property
        Private m_CnsIntegrity As Boolean
        <DataMember()> Public Property OrderNumber() As String
            Get
                Return m_OrderNumber
            End Get
            Set(ByVal value As String)
                m_OrderNumber = Value
            End Set
        End Property
        Private m_OrderNumber As String
        <DataMember()> Public Property PickupName() As String
            Get
                Return m_PickupName
            End Get
            Set(ByVal value As String)
                m_PickupName = Value
            End Set
        End Property
        Private m_PickupName As String
        <DataMember()> Public Property PickupAddress() As String
            Get
                Return m_PickupAddress
            End Get
            Set(ByVal value As String)
                m_PickupAddress = Value
            End Set
        End Property
        Private m_PickupAddress As String
        <DataMember()> Public Property PickupCity() As String
            Get
                Return m_PickupCity
            End Get
            Set(ByVal value As String)
                m_PickupCity = Value
            End Set
        End Property
        Private m_PickupCity As String
        <DataMember()> Public Property PickupState() As String
            Get
                Return m_PickupState
            End Get
            Set(ByVal value As String)
                m_PickupState = Value
            End Set
        End Property
        Private m_PickupState As String
        <DataMember()> Public Property PickupZipCode() As String
            Get
                Return m_PickupZipCode
            End Get
            Set(ByVal value As String)
                m_PickupZipCode = Value
            End Set
        End Property
        Private m_PickupZipCode As String
        <DataMember()> Public Property DestinationName() As String
            Get
                Return m_DestinationName
            End Get
            Set(ByVal value As String)
                m_DestinationName = Value
            End Set
        End Property
        Private m_DestinationName As String
        <DataMember()> Public Property DestinationAddress() As String
            Get
                Return m_DestinationAddress
            End Get
            Set(ByVal value As String)
                m_DestinationAddress = Value
            End Set
        End Property
        Private m_DestinationAddress As String
        <DataMember()> Public Property DestinationCity() As String
            Get
                Return m_DestinationCity
            End Get
            Set(ByVal value As String)
                m_DestinationCity = Value
            End Set
        End Property
        Private m_DestinationCity As String
        <DataMember()> Public Property DestinationState() As String
            Get
                Return m_DestinationState
            End Get
            Set(ByVal value As String)
                m_DestinationState = Value
            End Set
        End Property
        Private m_DestinationState As String
        <DataMember()> Public Property DestinationZipCode() As String
            Get
                Return m_DestinationZipCode
            End Get
            Set(ByVal value As String)
                m_DestinationZipCode = Value
            End Set
        End Property
        Private m_DestinationZipCode As String
        <DataMember()> Public Property Cases() As Integer
            Get
                Return m_Cases
            End Get
            Set(ByVal value As Integer)
                m_Cases = Value
            End Set
        End Property
        Private m_Cases As Integer
        <DataMember()> Public Property Weight() As Double
            Get
                Return m_Weight
            End Get
            Set(ByVal value As Double)
                m_Weight = Value
            End Set
        End Property
        Private m_Weight As Double
        <DataMember()> Public Property Pallets() As Double
            Get
                Return m_Pallets
            End Get
            Set(ByVal value As Double)
                m_Pallets = Value
            End Set
        End Property
        Private m_Pallets As Double
        <DataMember()> Public Property Cubes() As Integer
            Get
                Return m_Cubes
            End Get
            Set(ByVal value As Integer)
                m_Cubes = Value
            End Set
        End Property
        Private m_Cubes As Integer
        <DataMember()> Public Property PickupDate() As Nullable(Of DateTime)
            Get
                Return m_PickupDate
            End Get
            Set(ByVal value As Nullable(Of DateTime))
                m_PickupDate = value
            End Set
        End Property
        Private m_PickupDate As Nullable(Of DateTime)
        <DataMember()> Public Property DeliveryDate() As Nullable(Of DateTime)
            Get
                Return m_DeliveryDate
            End Get
            Set(ByVal value As Nullable(Of DateTime))
                m_DeliveryDate = value
            End Set
        End Property
        Private m_DeliveryDate As Nullable(Of DateTime)
        <DataMember()> Public Property ContractCost() As Decimal
            Get
                Return m_ContractCost
            End Get
            Set(ByVal value As Decimal)
                m_ContractCost = Value
            End Set
        End Property
        Private m_ContractCost As Decimal
        <DataMember()> Public Property AssignedDate() As Nullable(Of DateTime)
            Get
                Return m_AssignedDate
            End Get
            Set(ByVal value As Nullable(Of DateTime))
                m_AssignedDate = value
            End Set
        End Property
        Private m_AssignedDate As Nullable(Of DateTime)
        <DataMember()> Public Property Comments() As String
            Get
                Return m_Comments
            End Get
            Set(ByVal value As String)
                m_Comments = value
            End Set
        End Property
        Private m_Comments As String

#End Region

#Region " Public Methods"
        Public Overrides Function Clone() As DTOBaseClass
            Dim instance As New TenderedItem
            instance = DirectCast(MemberwiseClone(), TenderedItem)
            Return instance
        End Function

#End Region

    End Class
End Namespace