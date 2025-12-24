Imports Microsoft.VisualBasic
Imports System
Imports System.Collections.Generic
Imports System.Linq
Imports System.Text
Imports System.Runtime.Serialization
Imports Ngl.Core.ChangeTracker

Namespace DataTransferObjects
    <DataContract()> _
    Public Class ReportsTree
        Inherits NEXTrackTreeNode


#Region " Data Members"
        Private _UserName As String = ""
        <DataMember()> _
        Public Property UserName() As String
            Get
                Return _UserName
            End Get
            Set(ByVal value As String)
                _UserName = value
            End Set
        End Property


#End Region

#Region " Public Methods"
        Public Overrides Function Clone() As DTOBaseClass
            Dim instance As New ReportsTree
            instance = DirectCast(MemberwiseClone(), ReportsTree)
            Return instance
        End Function

#End Region

    End Class
End Namespace

