Imports Microsoft.VisualBasic
Imports System
Imports System.Collections.Generic
Imports System.Linq
Imports System.Text
Imports System.Runtime.Serialization
Imports Ngl.Core.ChangeTracker

Namespace DataTransferObjects
    <DataContract()> _
    Public Class OptimizerCompSettings
        Inherits DTOBaseClass


#Region " Data Members"


        Private _CompControl As Integer = 0
        <DataMember()> _
        Public Property CompControl() As Integer
            Get
                Return _CompControl
            End Get
            Set(ByVal value As Integer)
                _CompControl = value
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

        Private _CompName As String = ""
        <DataMember()> _
        Public Property CompName() As String
            Get
                Return Left(_CompName, 40)
            End Get
            Set(ByVal value As String)
                _CompName = Left(value, 40)
            End Set
        End Property

        Private _CompNatNumber As Integer = 0
        <DataMember()> _
        Public Property CompNatNumber() As Integer
            Get
                Return _CompNatNumber
            End Get
            Set(ByVal value As Integer)
                _CompNatNumber = value
            End Set
        End Property

        Private _CompNatName As String = ""
        <DataMember()> _
        Public Property CompNatName() As String
            Get
                Return Left(_CompNatName, 40)
            End Get
            Set(ByVal value As String)
                _CompNatName = Left(value, 40)
            End Set
        End Property

        Private _CompStreetAddress1 As String = ""
        <DataMember()> _
        Public Property CompStreetAddress1() As String
            Get
                Return Left(_CompStreetAddress1, 40)
            End Get
            Set(ByVal value As String)
                _CompStreetAddress1 = Left(value, 40)
            End Set
        End Property

        Private _CompStreetAddress2 As String = ""
        <DataMember()> _
        Public Property CompStreetAddress2() As String
            Get
                Return Left(_CompStreetAddress2, 40)
            End Get
            Set(ByVal value As String)
                _CompStreetAddress2 = Left(value, 40)
            End Set
        End Property

        Private _CompStreetAddress3 As String = ""
        <DataMember()> _
        Public Property CompStreetAddress3() As String
            Get
                Return Left(_CompStreetAddress3, 40)
            End Get
            Set(ByVal value As String)
                _CompStreetAddress3 = Left(value, 40)
            End Set
        End Property

        Private _CompStreetCity As String = ""
        <DataMember()> _
        Public Property CompStreetCity() As String
            Get
                Return Left(_CompStreetCity, 25)
            End Get
            Set(ByVal value As String)
                _CompStreetCity = Left(value, 25)
            End Set
        End Property

        Private _CompStreetState As String = ""
        <DataMember()> _
        Public Property CompStreetState() As String
            Get
                Return Left(_CompStreetState, 8)
            End Get
            Set(ByVal value As String)
                _CompStreetState = Left(value, 8)
            End Set
        End Property

        Private _CompStreetCountry As String = ""
        <DataMember()> _
        Public Property CompStreetCountry() As String
            Get
                Return Left(_CompStreetCountry, 30)
            End Get
            Set(ByVal value As String)
                _CompStreetCountry = Left(value, 30)
            End Set
        End Property

        Private _CompStreetZip As String = ""
        <DataMember()> _
        Public Property CompStreetZip() As String
            Get
                Return Left(_CompStreetZip, 20) ' Modified by RHR for v-8.4.003 on 06/25/2021
            End Get
            Set(ByVal value As String)
                _CompStreetZip = Left(value, 20) ' Modified by RHR for v-8.4.003 on 06/25/2021
            End Set
        End Property

        Private _CompLatitude As Double = 0
        <DataMember()> _
        Public Property CompLatitude() As Double
            Get
                Return _CompLatitude
            End Get
            Set(ByVal value As Double)
                _CompLatitude = value
            End Set
        End Property

        Private _CompLongitude As Double = 0
        <DataMember()> _
        Public Property CompLongitude() As Double
            Get
                Return _CompLongitude
            End Get
            Set(ByVal value As Double)
                _CompLongitude = value
            End Set
        End Property

#End Region

#Region " Public Methods"

        Public Overrides Function Clone() As DTOBaseClass
            Dim instance As New OptimizerCompSettings
            instance = DirectCast(MemberwiseClone(), OptimizerCompSettings)
            Return instance
        End Function

#End Region

    End Class
End Namespace
