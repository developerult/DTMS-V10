Imports Microsoft.VisualBasic
Imports System
Imports System.Collections.Generic
Imports System.Linq
Imports System.Text
Imports System.Runtime.Serialization
Imports Ngl.Core.ChangeTracker

Namespace DataTransferObjects
    <DataContract()> _
    Public Class CarrTarNoDriveDays
        Inherits DTOBaseClass


#Region " Data Members"
        Private _CarrTarNDDControl As Integer = 0
        <DataMember()> _
        Public Property CarrTarNDDControl() As Integer
            Get
                Return _CarrTarNDDControl
            End Get
            Set(ByVal value As Integer)
                _CarrTarNDDControl = value
            End Set
        End Property

        Private _CarrTarNDDCarrTarControl As Integer = 0
        <DataMember()> _
        Public Property CarrTarNDDCarrTarControl() As Integer
            Get
                Return _CarrTarNDDCarrTarControl
            End Get
            Set(ByVal value As Integer)
                _CarrTarNDDCarrTarControl = value
            End Set
        End Property

        Private _CarrTarNDDNoDrivingDate As Date
        <DataMember()> _
        Public Property CarrTarNDDNoDrivingDate() As Date
            Get
                Return _CarrTarNDDNoDrivingDate
            End Get
            Set(ByVal value As Date)
                _CarrTarNDDNoDrivingDate = value
            End Set
        End Property

        Private _CarrTarNDDModUser As String = ""
        <DataMember()> _
        Public Property CarrTarNDDModUser() As String
            Get
                Return Left(_CarrTarNDDModUser, 100)
            End Get
            Set(ByVal value As String)
                _CarrTarNDDModUser = Left(value, 100)
            End Set
        End Property

        Private _CarrTarNDDModDate As Date = DateTime.Now
        <DataMember()> _
        Public Property CarrTarNDDModDate() As Date
            Get
                Return _CarrTarNDDModDate
            End Get
            Set(ByVal value As Date)
                _CarrTarNDDModDate = value
            End Set
        End Property

        Private _CarrTarNDDUpdated As Byte()
        <DataMember()> _
        Public Property CarrTarNDDUpdated() As Byte()
            Get
                Return _CarrTarNDDUpdated
            End Get
            Set(ByVal value As Byte())
                _CarrTarNDDUpdated = value
            End Set
        End Property

#End Region

#Region " Public Methods"
        Public Overrides Function Clone() As DTOBaseClass
            Dim instance As New CarrTarNoDriveDays
            instance = DirectCast(MemberwiseClone(), CarrTarNoDriveDays)
            Return instance
        End Function

#End Region

    End Class
End Namespace