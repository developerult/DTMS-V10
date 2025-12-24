Imports System.Collections.Generic
Imports System.Linq
Imports System.Text
Imports System.Runtime.Serialization
Imports Ngl.Core.ChangeTracker

Namespace DataTransferObjects
    <DataContract()> _
    Public Class tblExportFileSetting
        Inherits DTOBaseClass

#Region " Data Members"

        Private _EFSControl As Integer = 0
        <DataMember()> _
        Public Property EFSControl() As Integer
            Get
                Return _EFSControl
            End Get
            Set(ByVal value As Integer)
                _EFSControl = value
            End Set
        End Property

        Private _EFSFileName As String = ""
        <DataMember()> _
        Public Property EFSFileName() As String
            Get
                Return Left(_EFSFileName, 100)
            End Get
            Set(ByVal value As String)
                _EFSFileName = Left(value, 100)
            End Set
        End Property

        Private _EFSFileType As Integer = 0
        <DataMember()> _
        Public Property EFSFileType() As Integer
            Get
                Return _EFSFileType
            End Get
            Set(ByVal value As Integer)
                _EFSFileType = value
            End Set
        End Property

        Private _EFSFolder As String = ""
        <DataMember()> _
        Public Property EFSFolder() As String
            Get
                Return Left(_EFSFolder, 255)
            End Get
            Set(ByVal value As String)
                _EFSFolder = Left(value, 255)
            End Set
        End Property

        Private _EFSDesc As String = ""
        <DataMember()> _
        Public Property EFSDesc() As String
            Get
                Return Left(_EFSDesc, 255)
            End Get
            Set(ByVal value As String)
                _EFSDesc = Left(value, 255)
            End Set
        End Property

        Private _EFSUpdated As Byte()
        <DataMember()> _
        Public Property EFSUpdated() As Byte()
            Get
                Return _EFSUpdated
            End Get
            Set(ByVal value As Byte())
                _EFSUpdated = value
            End Set
        End Property


#End Region

#Region " Public Methods"
        Public Overrides Function Clone() As DTOBaseClass
            Dim instance As New tblExportFileSetting
            instance = DirectCast(MemberwiseClone(), tblExportFileSetting)
            Return instance
        End Function

#End Region
    End Class
End Namespace
