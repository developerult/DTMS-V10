Imports Microsoft.VisualBasic
Imports System
Imports System.Collections.Generic
Imports System.Linq
Imports System.Text
Imports System.Runtime.Serialization
Imports Ngl.Core.ChangeTracker

Namespace DataTransferObjects
    <DataContract()> _
    Public Class LoadType
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

        Private _LoadTypeName As String = ""
        <DataMember()> _
        Public Property LoadTypeName() As String
            Get
                Return Left(_LoadTypeName, 25)
            End Get
            Set(ByVal value As String)
                _LoadTypeName = Left(value, 25)
            End Set
        End Property

        Private _LoadTypeGroup As String = ""
        <DataMember()> _
        Public Property LoadTypeGroup() As String
            Get
                Return Left(_LoadTypeGroup, 50)
            End Get
            Set(ByVal value As String)
                _LoadTypeGroup = Left(value, 50)
            End Set
        End Property

        Private _LoadTypeUpdated As Byte()
        <DataMember()> _
        Public Property LoadTypeUpdated() As Byte()
            Get
                Return _LoadTypeUpdated
            End Get
            Set(ByVal value As Byte())
                _LoadTypeUpdated = value
            End Set
        End Property


#End Region

#Region " Public Methods"
        Public Overrides Function Clone() As DTOBaseClass
            Dim instance As New LoadType
            instance = DirectCast(MemberwiseClone(), LoadType)
            Return instance
        End Function

#End Region

    End Class
End Namespace
