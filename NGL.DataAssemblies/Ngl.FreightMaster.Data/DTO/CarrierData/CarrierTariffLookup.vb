Imports Microsoft.VisualBasic
Imports System
Imports System.Collections.Generic
Imports System.Linq
Imports System.Text
Imports System.Runtime.Serialization
Imports Ngl.Core.ChangeTracker

Namespace DataTransferObjects
    <DataContract()> _
    Public Class CarrierTariffLookup
        Inherits DTOBaseClass


#Region " Data Members"

        Private _CarrTarControl As Integer = 0
        <DataMember()> _
        Public Property CarrTarControl() As Integer
            Get
                Return _CarrTarControl
            End Get
            Set(ByVal value As Integer)
                _CarrTarControl = value
            End Set
        End Property


        Private _CarrTarCompControl As Integer = 0
        <DataMember()> _
        Public Property CarrTarCompControl() As Integer
            Get
                Return _CarrTarCompControl
            End Get
            Set(ByVal value As Integer)
                _CarrTarCompControl = value
            End Set
        End Property


        Private _CarrTarCarrierControl As Integer = 0
        <DataMember()> _
        Public Property CarrTarCarrierControl() As Integer
            Get
                Return _CarrTarCarrierControl
            End Get
            Set(ByVal value As Integer)
                _CarrTarCarrierControl = value
            End Set
        End Property


        Private _CompNumber As Integer = 0
        <DataMember()> _
        Public Property CompNumber() As Integer
            Get
                Return _CompNumber
            End Get
            Set(ByVal value As Integer)
                _CompNumber = value
            End Set
        End Property

        Private _CarrierNumber As Integer = 0
        <DataMember()> _
        Public Property CarrierNumber() As Integer
            Get
                Return _CarrierNumber
            End Get
            Set(ByVal value As Integer)
                _CarrierNumber = value
            End Set
        End Property

        Private _CompName As String = ""
        <DataMember()> _
        Public Property CompName() As String
            Get
                Return _CompName
            End Get
            Set(ByVal value As String)
                _CompName = value
            End Set
        End Property

        Private _CarrierName As String = ""
        <DataMember()> _
        Public Property CarrierName() As String
            Get
                Return _CarrierName
            End Get
            Set(ByVal value As String)
                _CarrierName = value
            End Set
        End Property


        Private _CarrTarID As String = ""
        <DataMember()> _
        Public Property CarrTarID() As String
            Get
                Return Left(_CarrTarID, 50)
            End Get
            Set(ByVal value As String)
                _CarrTarID = Left(value, 50)
            End Set
        End Property

        Private _CarrTarBPBracketType As Integer = 0
        <DataMember()> _
        Public Property CarrTarBPBracketType() As Integer
            Get
                Return _CarrTarBPBracketType
            End Get
            Set(ByVal value As Integer)
                _CarrTarBPBracketType = value
            End Set
        End Property


        Private _CarrTarTLCapacityType As Integer = 0
        <DataMember()> _
        Public Property CarrTarTLCapacityType() As Integer
            Get
                Return _CarrTarTLCapacityType
            End Get
            Set(ByVal value As Integer)
                _CarrTarTLCapacityType = value
            End Set
        End Property


        Private _CarrTarTempType As Integer = 0
        <DataMember()> _
        Public Property CarrTarTempType() As Integer
            Get
                Return _CarrTarTempType
            End Get
            Set(ByVal value As Integer)
                _CarrTarTempType = value
            End Set
        End Property


        Private _CarrTarTariffType As String = ""
        <DataMember()> _
        Public Property CarrTarTariffType() As String
            Get
                Return Left(_CarrTarTariffType, 1)
            End Get
            Set(ByVal value As String)
                _CarrTarTariffType = Left(value, 1)
            End Set
        End Property

#End Region

#Region " Public Methods"
        Public Overrides Function Clone() As DTOBaseClass
            Dim instance As New CarrierTariffLookup
            instance = DirectCast(MemberwiseClone(), CarrierTariffLookup)
           
            Return instance
        End Function

#End Region

    End Class
End Namespace