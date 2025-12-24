Imports Microsoft.VisualBasic
Imports System
Imports System.Collections.Generic
Imports System.Linq
Imports System.Text
Imports System.Runtime.Serialization
Imports Ngl.Core.ChangeTracker

Namespace DataTransferObjects
    <DataContract()> _
    Public Class GenericResults

#Region " Data Members"
        Private _Control As Integer = 0
        <DataMember()> _
        Public Property Control() As Integer
            Get
                Return _Control
            End Get
            Set(ByVal value As Integer)
                _Control = value
            End Set
        End Property

        Private _Data As Object
        <DataMember()> _
        Public Property Data() As Object
            Get
                Return _Data
            End Get
            Set(ByVal value As Object)
                _Data = value
            End Set
        End Property

        Private _RetMsg As String
        <DataMember()> _
        Public Property RetMsg() As String
            Get
                Return _RetMsg
            End Get
            Set(ByVal value As String)
                _RetMsg = value
            End Set
        End Property

        Private _ErrNumber As Integer
        <DataMember()> _
        Public Property ErrNumber() As Integer
            Get
                Return _ErrNumber
            End Get
            Set(ByVal value As Integer)
                _ErrNumber = value
            End Set
        End Property




#End Region

    End Class

End Namespace
