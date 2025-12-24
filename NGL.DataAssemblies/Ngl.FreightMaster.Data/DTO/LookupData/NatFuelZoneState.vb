Imports Microsoft.VisualBasic
Imports System
Imports System.Collections.Generic
Imports System.Linq
Imports System.Text
Imports System.Runtime.Serialization
Imports Ngl.Core.ChangeTracker

Namespace DataTransferObjects
    <DataContract()> _
    Public Class NatFuelZoneState
        Inherits DTOBaseClass


#Region " Data Members"

        Private _NatFuelZoneStatesState As String = ""
        <DataMember()> _
        Public Property NatFuelZoneStatesState() As String
            Get
                Return Left(_NatFuelZoneStatesState, 2)
            End Get
            Set(ByVal value As String)
                _NatFuelZoneStatesState = Left(value, 2)
            End Set
        End Property

        Private _NatFuelZoneStatesNatFuelZoneID As Integer = 0
        <DataMember()> _
        Public Property NatFuelZoneStatesNatFuelZoneID() As Integer
            Get
                Return _NatFuelZoneStatesNatFuelZoneID
            End Get
            Set(ByVal value As Integer)
                _NatFuelZoneStatesNatFuelZoneID = value
            End Set
        End Property



 

        Private _NatFuelZoneStatesModDate As System.Nullable(Of Date)
        <DataMember()> _
        Public Property NatFuelZoneStatesModDate() As System.Nullable(Of Date)
            Get
                Return _NatFuelZoneStatesModDate
            End Get
            Set(ByVal value As System.Nullable(Of Date))
                _NatFuelZoneStatesModDate = value
            End Set
        End Property

        Private _NatFuelZoneStatesModUser As String = ""
        <DataMember()> _
        Public Property NatFuelZoneStatesModUser() As String
            Get
                Return Left(_NatFuelZoneStatesModUser, 100)
            End Get
            Set(ByVal value As String)
                _NatFuelZoneStatesModUser = Left(value, 100)
            End Set
        End Property

        Private _NatFuelZoneStatesUpdated As Byte()
        <DataMember()> _
        Public Property NatFuelZoneStatesUpdated() As Byte()
            Get
                Return _NatFuelZoneStatesUpdated
            End Get
            Set(ByVal value As Byte())
                _NatFuelZoneStatesUpdated = value
            End Set
        End Property


#End Region

#Region " Public Methods"
        Public Overrides Function Clone() As DTOBaseClass
            Dim instance As New NatFuelZoneState
            instance = DirectCast(MemberwiseClone(), NatFuelZoneState)
            Return instance
        End Function

#End Region

    End Class
End Namespace