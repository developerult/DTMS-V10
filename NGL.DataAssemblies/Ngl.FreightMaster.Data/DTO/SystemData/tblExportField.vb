Imports Microsoft.VisualBasic
Imports System
Imports System.Collections.Generic
Imports System.Linq
Imports System.Text
Imports System.Runtime.Serialization
Imports Ngl.Core.ChangeTracker

Namespace DataTransferObjects
    <DataContract()> _
    Public Class tblExportField
        Inherits DTOBaseClass

#Region " Data Members"

        Private _ExportControl As Integer = 0
        <DataMember()> _
        Public Property ExportControl() As Integer
            Get
                Return _ExportControl
            End Get
            Set(ByVal value As Integer)
                _ExportControl = value
            End Set
        End Property

        Private _ExportFieldName As String = ""
        <DataMember()> _
        Public Property ExportFieldName() As String
            Get
                Return Left(_ExportFieldName, 100)
            End Get
            Set(ByVal value As String)
                _ExportFieldName = Left(value, 100)
            End Set
        End Property

        Private _ExportFileType As Integer = 0
        <DataMember()> _
        Public Property ExportFileType() As Integer
            Get
                Return _ExportFileType
            End Get
            Set(ByVal value As Integer)
                _ExportFileType = value
            End Set
        End Property

        Private _ExportFlag As Boolean = False
        <DataMember()> _
        Public Property ExportFlag() As Boolean
            Get
                Return _ExportFlag
            End Get
            Set(ByVal value As Boolean)
                _ExportFlag = value
            End Set
        End Property

        Private _ExportFieldDesc As String = ""
        <DataMember()> _
        Public Property ExportFieldDesc() As String
            Get
                Return Left(_ExportFieldDesc, 255)
            End Get
            Set(ByVal value As String)
                _ExportFieldDesc = Left(value, 255)
            End Set
        End Property

        Private _ExportUpdated As Byte()
        <DataMember()> _
        Public Property ExportUpdated() As Byte()
            Get
                Return _ExportUpdated
            End Get
            Set(ByVal value As Byte())
                _ExportUpdated = value
            End Set
        End Property


#End Region

#Region " Public Methods"
        Public Overrides Function Clone() As DTOBaseClass
            Dim instance As New tblExportField
            instance = DirectCast(MemberwiseClone(), tblExportField)
            Return instance
        End Function

#End Region
    End Class
End Namespace
