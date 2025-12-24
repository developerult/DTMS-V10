Imports Microsoft.VisualBasic
Imports System
Imports System.Collections.Generic
Imports System.Linq
Imports System.Text
Imports System.Runtime.Serialization
Imports Ngl.Core.ChangeTracker

Namespace DataTransferObjects
    <DataContract()> _
    Public Class tblDataEntryField
        Inherits DTOBaseClass


#Region " Data Members"

        Private _DEControl As Integer = 0
        <DataMember()> _
        Public Property DEControl() As Integer
            Get
                Return _DEControl
            End Get
            Set(ByVal value As Integer)
                _DEControl = value
            End Set
        End Property

        Private _DEFieldName As String = ""
        <DataMember()> _
        Public Property DEFieldName() As String
            Get
                Return Left(_DEFieldName, 100)
            End Get
            Set(ByVal value As String)
                _DEFieldName = Left(value, 100)
            End Set
        End Property

        Private _DEFileType As Integer = 0
        <DataMember()> _
        Public Property DEFileType() As Integer
            Get
                Return _DEFileType
            End Get
            Set(ByVal value As Integer)
                _DEFileType = value
            End Set
        End Property

        Private _DEFlag As Boolean = False
        <DataMember()> _
        Public Property DEFlag() As Boolean
            Get
                Return _DEFlag
            End Get
            Set(ByVal value As Boolean)
                _DEFlag = value
            End Set
        End Property

        Private _DEFieldDesc As String = ""
        <DataMember()> _
        Public Property DEFieldDesc() As String
            Get
                Return Left(_DEFieldDesc, 255)
            End Get
            Set(ByVal value As String)
                _DEFieldDesc = Left(value, 255)
            End Set
        End Property

        Private _DEGLNumber As String = ""
        <DataMember()> _
        Public Property DEGLNumber() As String
            Get
                Return Left(_DEGLNumber, 50)
            End Get
            Set(ByVal value As String)
                _DEGLNumber = Left(value, 50)
            End Set
        End Property

        Private _DEFieldMapCode As String = ""
        <DataMember()> _
        Public Property DEFieldMapCode() As String
            Get
                Return Left(_DEFieldMapCode, 50)
            End Get
            Set(ByVal value As String)
                _DEFieldMapCode = Left(value, 50)
            End Set
        End Property

        Private _DEUpdated As Byte()
        <DataMember()> _
        Public Property DEUpdated() As Byte()
            Get
                Return _DEUpdated
            End Get
            Set(ByVal value As Byte())
                _DEUpdated = value
            End Set
        End Property


#End Region

#Region " Public Methods"
        Public Overrides Function Clone() As DTOBaseClass
            Dim instance As New tblDataEntryField
            instance = DirectCast(MemberwiseClone(), tblDataEntryField)
            Return instance
        End Function

#End Region
    End Class
End Namespace
