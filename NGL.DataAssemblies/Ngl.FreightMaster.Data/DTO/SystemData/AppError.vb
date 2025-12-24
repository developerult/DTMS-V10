Imports Microsoft.VisualBasic
Imports System
Imports System.Collections.Generic
Imports System.Linq
Imports System.Text
Imports System.Runtime.Serialization
Imports Ngl.Core.ChangeTracker

Namespace DataTransferObjects
    <DataContract()> _
    Public Class AppError
        Inherits DTOBaseClass


#Region " Data Members"

        Private _Message As String = ""
        <DataMember()> _
        Public Property Message() As String
            Get
                Return Left(_Message, 1000)
            End Get
            Set(ByVal value As String)
                _Message = Left(value, 1000)
            End Set
        End Property

        Private _ErrTime As System.Nullable(Of Date)
        <DataMember()> _
        Public Property ErrTime() As System.Nullable(Of Date)
            Get
                Return _ErrTime
            End Get
            Set(ByVal value As System.Nullable(Of Date))
                _ErrTime = value
            End Set
        End Property

        Private _ErrUser As String = ""
        <DataMember()> _
        Public Property ErrUser() As String
            Get
                Return Left(_ErrUser, 100)
            End Get
            Set(ByVal value As String)
                _ErrUser = Left(value, 100)
            End Set
        End Property

        
#End Region

#Region " Public Methods"
        Public Overrides Function Clone() As DTOBaseClass
            Dim instance As New AppError
            instance = DirectCast(MemberwiseClone(), AppError)
            Return instance
        End Function

#End Region

    End Class
End Namespace
