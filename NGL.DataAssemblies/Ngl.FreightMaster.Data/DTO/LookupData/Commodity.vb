Imports Microsoft.VisualBasic
Imports System
Imports System.Collections.Generic
Imports System.Linq
Imports System.Text
Imports System.Runtime.Serialization
Imports Ngl.Core.ChangeTracker

Namespace DataTransferObjects
    <DataContract()> _
    Public Class Commodity
        Inherits DTOBaseClass


#Region " Data Members"
        Private _ID As Integer = 0
        <DataMember()> _
        Public Property ID() As Integer
            Get
                Return _ID
            End Get
            Set(ByVal value As Integer)
                _ID = value
            End Set
        End Property

        Private _CommCodeType As String = ""
        <DataMember()> _
        Public Property CommCodeType() As String
            Get
                Return Left(_CommCodeType, 3)
            End Get
            Set(ByVal value As String)
                _CommCodeType = Left(value, 3)
            End Set
        End Property

        Private _TempType As String = ""
        <DataMember()> _
        Public Property TempType() As String
            Get
                Return Left(_TempType, 50)
            End Get
            Set(ByVal value As String)
                _TempType = Left(value, 50)
            End Set
        End Property


        Private _CommCodeDescription As String = ""
        <DataMember()> _
        Public Property CommCodeDescription() As String
            Get
                Return Left(_CommCodeDescription, 50)
            End Get
            Set(ByVal value As String)
                _CommCodeDescription = Left(value, 50)
            End Set
        End Property
  
        Private _CommCodeUpdated As Byte()
        <DataMember()> _
        Public Property CommCodeUpdated() As Byte()
            Get
                Return _CommCodeUpdated
            End Get
            Set(ByVal value As Byte())
                _CommCodeUpdated = value
            End Set
        End Property


#End Region

#Region " Public Methods"
        Public Overrides Function Clone() As DTOBaseClass
            Dim instance As New Commodity
            instance = DirectCast(MemberwiseClone(), Commodity)
            Return instance
        End Function

#End Region

    End Class
End Namespace
