Imports Microsoft.VisualBasic
Imports System
Imports System.Collections.Generic
Imports System.Linq
Imports System.Text
Imports System.Runtime.Serialization
Imports Ngl.Core.ChangeTracker

Namespace DataTransferObjects
    <DataContract()> _
    Public Class tblRouteType
        Inherits DTOBaseClass


#Region " Data Members"

        Private _RouteTypeControl As Integer = 0
        <DataMember()> _
        Public Property RouteTypeControl() As Integer
            Get
                Return Me._RouteTypeControl
            End Get
            Set(value As Integer)
                If ((Me._RouteTypeControl = value) _
                   = False) Then
                    Me._RouteTypeControl = value
                    Me.SendPropertyChanged("RouteTypeControl")
                End If
            End Set
        End Property



        Private _RouteTypeName As String
        <DataMember()> _
        Public Property RouteTypeName() As String
            Get
                Return Left(Me._RouteTypeName, 50)
            End Get
            Set(value As String)
                If (String.Equals(Me._RouteTypeName, value) = False) Then
                    Me._RouteTypeName = Left(value, 50)
                    Me.SendPropertyChanged("RouteTypeName")
                End If
            End Set
        End Property

        Private _RouteTypeDesc As String
        <DataMember()> _
        Public Property RouteTypeDesc() As String
            Get
                Return Left(Me._RouteTypeDesc, 255)
            End Get
            Set(value As String)
                If (String.Equals(Me._RouteTypeDesc, value) = False) Then
                    Me._RouteTypeDesc = Left(value, 255)
                    Me.SendPropertyChanged("RouteTypeDesc")
                End If
            End Set
        End Property


        Private _RouteTypeUpdated As Byte()
        <DataMember()> _
        Public Property RouteTypeUpdated() As Byte()
            Get
                Return Me._RouteTypeUpdated
            End Get
            Set(value As Byte())
                Me._RouteTypeUpdated = value
            End Set
        End Property

#End Region

#Region " Public Methods"
        Public Overrides Function Clone() As DTOBaseClass
            Dim instance As New tblRouteType
            instance = DirectCast(MemberwiseClone(), tblRouteType)
            Return instance
        End Function

#End Region

    End Class
End Namespace
