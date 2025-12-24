Imports Microsoft.VisualBasic
Imports System
Imports System.Collections.Generic
Imports System.Linq
Imports System.Text
Imports System.Runtime.Serialization
Imports Ngl.Core.ChangeTracker

Namespace DataTransferObjects
    <DataContract()> _
    Public Class CarrierDropLoad
        Inherits DTOBaseClass


#Region " Data Members"
        Private _CarrierDropControl As Integer = 0
        <DataMember()> _
        Public Property CarrierDropControl() As Integer
            Get
                Return _CarrierDropControl
            End Get
            Set(ByVal value As Integer)
                _CarrierDropControl = value
            End Set
        End Property

        Private _CarrierDropNumber As Integer = 0
        <DataMember()> _
        Public Property CarrierDropNumber() As Integer
            Get
                Return _CarrierDropNumber
            End Get
            Set(ByVal value As Integer)
                _CarrierDropNumber = value
            End Set
        End Property

        Private _CarrierDropContact As String = ""
        <DataMember()> _
        Public Property CarrierDropContact() As String
            Get
                Return Left(_CarrierDropContact, 50)
            End Get
            Set(ByVal value As String)
                _CarrierDropContact = Left(value, 50)
            End Set
        End Property

        Private _CarrierDropProNumber As String = ""
        <DataMember()> _
        Public Property CarrierDropProNumber() As String
            Get
                Return Left(_CarrierDropProNumber, 50)
            End Get
            Set(ByVal value As String)
                _CarrierDropProNumber = Left(value, 50)
            End Set
        End Property

        Private _CarrierDropReason As String = ""
        <DataMember()> _
        Public Property CarrierDropReason() As String
            Get
                Return Left(_CarrierDropReason, 255)
            End Get
            Set(ByVal value As String)
                _CarrierDropReason = Left(value, 255)
            End Set
        End Property

        Private _CarrierDropDate As System.Nullable(Of Date)
        <DataMember()> _
        Public Property CarrierDropDate() As System.Nullable(Of Date)
            Get
                Return _CarrierDropDate
            End Get
            Set(ByVal value As System.Nullable(Of Date))
                _CarrierDropDate = value
            End Set
        End Property

        Private _CarrierDropTime As System.Nullable(Of Date)
        <DataMember()> _
        Public Property CarrierDropTime() As System.Nullable(Of Date)
            Get
                Return _CarrierDropTime
            End Get
            Set(ByVal value As System.Nullable(Of Date))
                _CarrierDropTime = value
            End Set
        End Property

        Private _CarrierDropReasonLocalized As String = ""
        <DataMember()> _
        Public Property CarrierDropReasonLocalized() As String
            Get
                Return Left(_CarrierDropReasonLocalized, 255)
            End Get
            Set(ByVal value As String)
                _CarrierDropReasonLocalized = Left(value, 255)
            End Set
        End Property

        Private _CarrierDropReasonKeys As String = ""
        <DataMember()> _
        Public Property CarrierDropReasonKeys() As String
            Get
                Return Left(_CarrierDropReasonKeys, 4000)
            End Get
            Set(ByVal value As String)
                _CarrierDropReasonKeys = Left(value, 4000)
            End Set
        End Property

        Private _CarrierDropLoadUpdated As Byte()
        <DataMember()> _
        Public Property CarrierDropLoadUpdated() As Byte()
            Get
                Return _CarrierDropLoadUpdated
            End Get
            Set(ByVal value As Byte())
                _CarrierDropLoadUpdated = value
            End Set
        End Property

#End Region

#Region " Public Methods"
        Public Overrides Function Clone() As DTOBaseClass
            Dim instance As New CarrierDropLoad
            instance = DirectCast(MemberwiseClone(), CarrierDropLoad)
            Return instance
        End Function

#End Region

    End Class
End Namespace