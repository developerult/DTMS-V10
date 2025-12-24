Imports Microsoft.VisualBasic
Imports System
Imports System.Collections.Generic
Imports System.Linq
Imports System.Text
Imports System.Runtime.Serialization
Imports Ngl.Core.ChangeTracker

Namespace DataTransferObjects
    <DataContract()> _
    Public Class tblCountries
        Inherits DTOBaseClass


#Region " Data Members"
        Private _CountryControl As Integer = 0
        <DataMember()> _
        Public Property CountryControl() As Integer
            Get
                Return _CountryControl
            End Get
            Set(ByVal value As Integer)
                _CountryControl = value
            End Set
        End Property

        Private _CountryISO As String = ""
        <DataMember()> _
        Public Property CountryISO() As String
            Get
                Return Left(_CountryISO, 2)
            End Get
            Set(ByVal value As String)
                _CountryISO = Left(value, 2)
            End Set
        End Property

        Private _CountryName As String = ""
        <DataMember()> _
        Public Property CountryName As String
            Get
                Return Left(_CountryName, 200)
            End Get
            Set(ByVal value As String)
                _CountryName = Left(value, 200)
            End Set
        End Property

        Private _CountryUpdated As Byte()
        <DataMember()> _
        Public Property CountryUpdated() As Byte()
            Get
                Return _CountryUpdated
            End Get
            Set(ByVal value As Byte())
                _CountryUpdated = value
            End Set
        End Property


#End Region

#Region " Public Methods"
        Public Overrides Function Clone() As DTOBaseClass
            Dim instance As New tblCountries
            instance = DirectCast(MemberwiseClone(), tblCountries)
            Return instance
        End Function

#End Region

    End Class
End Namespace