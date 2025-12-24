Imports Microsoft.VisualBasic
Imports System
Imports System.Collections.Generic
Imports System.Linq
Imports System.Text
Imports System.Runtime.Serialization
Imports Ngl.Core.ChangeTracker

'Added By LVV 2/29/16 for v-7.0.5.1 EDI Migration
Namespace DataTransferObjects
    <DataContract()> _
    Public Class tblEDI990
        Inherits DTOBaseClass

#Region " Data Members "

        Private _EDI990Control As Integer = 0
        <DataMember()> _
        Public Property EDI990Control() As Integer
            Get
                Return _EDI990Control
            End Get
            Set(ByVal value As Integer)
                _EDI990Control = value
            End Set
        End Property

        Private _CarrierSCAC As String = ""
        <DataMember()> _
        Public Property CarrierSCAC() As String
            Get
                Return Left(_CarrierSCAC, 4)
            End Get
            Set(ByVal value As String)
                _CarrierSCAC = Left(value, 4)
            End Set
        End Property

        Private _BookConsPrefix As String = ""
        <DataMember()> _
        Public Property BookConsPrefix() As String
            Get
                Return Left(_BookConsPrefix, 20)
            End Get
            Set(ByVal value As String)
                _BookConsPrefix = Left(value, 20)
            End Set
        End Property

        Private _EDI990Received As Boolean = False
        <DataMember()> _
        Public Property EDI990Received() As Boolean
            Get
                Return _EDI990Received
            End Get
            Set(ByVal value As Boolean)
                _EDI990Received = value
            End Set
        End Property

        Private _EDI990ReceivedDate As System.Nullable(Of Date)
        <DataMember()> _
        Public Property EDI990ReceivedDate() As System.Nullable(Of Date)
            Get
                Return _EDI990ReceivedDate
            End Get
            Set(ByVal value As System.Nullable(Of Date))
                _EDI990ReceivedDate = value
            End Set
        End Property

        Private _EDI990StatusCode As String = ""
        <DataMember()> _
        Public Property EDI990StatusCode() As String
            Get
                Return Left(_EDI990StatusCode, 100)
            End Get
            Set(ByVal value As String)
                _EDI990StatusCode = Left(value, 100)
            End Set
        End Property

        Private _EDI990FileName As String = ""
        <DataMember()> _
        Public Property EDI990FileName() As String
            Get
                Return Left(_EDI990FileName, 255)
            End Get
            Set(ByVal value As String)
                _EDI990FileName = Left(value, 255)
            End Set
        End Property

        Private _EDI990ModDate As System.Nullable(Of Date)
        <DataMember()> _
        Public Property EDI990ModDate() As System.Nullable(Of Date)
            Get
                Return _EDI990ModDate
            End Get
            Set(ByVal value As System.Nullable(Of Date))
                _EDI990ModDate = value
            End Set
        End Property

        Private _EDI990ModUser As String = ""
        <DataMember()> _
        Public Property EDI990ModUser() As String
            Get
                Return Left(_EDI990ModUser, 100)
            End Get
            Set(ByVal value As String)
                _EDI990ModUser = Left(value, 100)
            End Set
        End Property

        Private _EDI990Updated As Byte()
        <DataMember()> _
        Public Property EDI990Updated() As Byte()
            Get
                Return _EDI990Updated
            End Get
            Set(ByVal value As Byte())
                _EDI990Updated = value
            End Set
        End Property

#End Region

#Region " Public Methods"
        Public Overrides Function Clone() As DTOBaseClass
            Dim instance As New tblEDI990
            instance = DirectCast(MemberwiseClone(), tblEDI990)
            Return instance
        End Function

#End Region

    End Class
End Namespace
