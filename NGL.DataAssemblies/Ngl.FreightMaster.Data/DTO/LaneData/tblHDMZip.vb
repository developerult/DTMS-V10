Imports Microsoft.VisualBasic
Imports System
Imports System.Collections.Generic
Imports System.Linq
Imports System.Text
Imports System.Runtime.Serialization
Imports Ngl.Core.ChangeTracker

'Added By LVV on 9/26/16 for v-7.0.5.110 HDM Enhancement

Namespace DataTransferObjects
    <DataContract()> _
    Public Class tblHDMZip
        Inherits DTOBaseClass


#Region " Data Members"
        Private _HDMZipControl As Integer = 0
        <DataMember()> _
        Public Property HDMZipControl() As Integer
            Get
                Return _HDMZipControl
            End Get
            Set(ByVal value As Integer)
                _HDMZipControl = value
            End Set
        End Property

        Private _HDMZipHDMControl As Integer = 0
        <DataMember()> _
        Public Property HDMZipHDMControl() As Integer
            Get
                Return _HDMZipHDMControl
            End Get
            Set(ByVal value As Integer)
                _HDMZipHDMControl = value
            End Set
        End Property

        Private _HDMZipFrom As String = ""
        <DataMember()> _
        Public Property HDMZipFrom() As String
            Get
                Return Left(_HDMZipFrom, 20)
            End Get
            Set(ByVal value As String)
                _HDMZipFrom = Left(value, 20)
            End Set
        End Property

        Private _HDMZipTo As String = ""
        <DataMember()> _
        Public Property HDMZipTo() As String
            Get
                Return Left(_HDMZipTo, 20)
            End Get
            Set(ByVal value As String)
                _HDMZipTo = Left(value, 20)
            End Set
        End Property

        Private _HDMZipCity As String = ""
        <DataMember()> _
        Public Property HDMZipCity() As String
            Get
                Return Left(_HDMZipCity, 25)
            End Get
            Set(ByVal value As String)
                _HDMZipCity = Left(value, 25)
            End Set
        End Property

        Private _HDMZipState As String = ""
        <DataMember()> _
        Public Property HDMZipState() As String
            Get
                Return Left(_HDMZipState, 8)
            End Get
            Set(ByVal value As String)
                _HDMZipState = Left(value, 8)
            End Set
        End Property

        Private _HDMZipCountry As String = ""
        <DataMember()> _
        Public Property HDMZipCountry() As String
            Get
                Return Left(_HDMZipCountry, 30)
            End Get
            Set(ByVal value As String)
                _HDMZipCountry = Left(value, 30)
            End Set
        End Property

        Private _HDMZipModDate As System.Nullable(Of Date)
        <DataMember()> _
        Public Property HDMZipModDate() As System.Nullable(Of Date)
            Get
                Return _HDMZipModDate
            End Get
            Set(ByVal value As System.Nullable(Of Date))
                _HDMZipModDate = value
            End Set
        End Property

        Private _HDMZipModUser As String = ""
        <DataMember()> _
        Public Property HDMZipModUser() As String
            Get
                Return Left(_HDMZipModUser, 100)
            End Get
            Set(ByVal value As String)
                _HDMZipModUser = Left(value, 100)
            End Set
        End Property

        Private _HDMZipUpdated As Byte()
        <DataMember()> _
        Public Property HDMZipUpdated() As Byte()
            Get
                Return _HDMZipUpdated
            End Get
            Set(ByVal value As Byte())
                _HDMZipUpdated = value
            End Set
        End Property

#End Region

#Region " Public Methods"

        Public Overrides Function Clone() As DTOBaseClass
            Dim instance As New tblHDMZip
            instance = DirectCast(MemberwiseClone(), tblHDMZip)
            Return instance
        End Function

#End Region

    End Class
End Namespace
