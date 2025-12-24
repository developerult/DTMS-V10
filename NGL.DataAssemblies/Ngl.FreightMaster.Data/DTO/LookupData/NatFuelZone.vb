Imports Microsoft.VisualBasic
Imports System
Imports System.Collections.Generic
Imports System.Linq
Imports System.Text
Imports System.Runtime.Serialization
Imports Ngl.Core.ChangeTracker

Namespace DataTransferObjects
    <DataContract()> _
    Public Class NatFuelZone
        Inherits DTOBaseClass


#Region " Data Members"
        Private _NatFuelZoneID As Integer = 0
        <DataMember()> _
        Public Property NatFuelZoneID() As Integer
            Get
                Return _NatFuelZoneID
            End Get
            Set(ByVal value As Integer)
                _NatFuelZoneID = value
            End Set
        End Property
 

        Private _NatFuelZoneName As String = ""
        <DataMember()> _
        Public Property NatFuelZoneName() As String
            Get
                Return Left(_NatFuelZoneName, 50)
            End Get
            Set(ByVal value As String)
                _NatFuelZoneName = Left(value, 50)
            End Set
        End Property

        Private _NatFuelZoneDesc As String = ""
        <DataMember()> _
        Public Property NatFuelZoneDesc() As String
            Get
                Return Left(_NatFuelZoneDesc, 255)
            End Get
            Set(ByVal value As String)
                _NatFuelZoneDesc = Left(value, 255)
            End Set
        End Property

        Private _NatFuelZoneIndex As Integer = 0
        <DataMember()> _
        Public Property NatFuelZoneIndex() As Integer
            Get
                Return _NatFuelZoneIndex
            End Get
            Set(ByVal value As Integer)
                _NatFuelZoneIndex = value
            End Set
        End Property

        Private _NatFuelZoneModDate As System.Nullable(Of Date)
        <DataMember()> _
        Public Property NatFuelZoneModDate() As System.Nullable(Of Date)
            Get
                Return _NatFuelZoneModDate
            End Get
            Set(ByVal value As System.Nullable(Of Date))
                _NatFuelZoneModDate = value
            End Set
        End Property

        Private _NatFuelZoneModUser As String = ""
        <DataMember()> _
        Public Property NatFuelZoneModUser() As String
            Get
                Return Left(_NatFuelZoneModUser, 100)
            End Get
            Set(ByVal value As String)
                _NatFuelZoneModUser = Left(value, 100)
            End Set
        End Property

        Private _NatFuelZoneUpdated As Byte()
        <DataMember()> _
        Public Property NatFuelZoneUpdated() As Byte()
            Get
                Return _NatFuelZoneUpdated
            End Get
            Set(ByVal value As Byte())
                _NatFuelZoneUpdated = value
            End Set
        End Property


#End Region

#Region " Public Methods"
        Public Overrides Function Clone() As DTOBaseClass
            Dim instance As New NatFuelZone
            instance = DirectCast(MemberwiseClone(), NatFuelZone)
            Return instance
        End Function

#End Region

    End Class
End Namespace