Imports Microsoft.VisualBasic
Imports System
Imports System.Collections.Generic
Imports System.Linq
Imports System.Text
Imports System.Runtime.Serialization
Imports Ngl.Core.ChangeTracker

Namespace DataTransferObjects
    <DataContract()> _
    Public Class SpecialCode
        Inherits DTOBaseClass


#Region " Data Members"

        Private _SpecialCodesControl As Integer = 0
        <DataMember()> _
        Public Property SpecialCodesControl() As Integer
            Get
                Return _SpecialCodesControl
            End Get
            Set(ByVal value As Integer)
                _SpecialCodesControl = value
            End Set
        End Property

        Private _Code As String = ""
        <DataMember()> _
        Public Property Code() As String
            Get
                Return Left(_Code, 2)
            End Get
            Set(ByVal value As String)
                _Code = Left(value, 2)
            End Set
        End Property

        Private _Description As String = ""
        <DataMember()> _
        Public Property Description() As String
            Get
                Return Left(_Description, 100)
            End Get
            Set(ByVal value As String)
                _Description = Left(value, 100)
            End Set
        End Property

        Private _SpecialCodesUpdated As Byte()
        <DataMember()> _
        Public Property SpecialCodesUpdated() As Byte()
            Get
                Return _SpecialCodesUpdated
            End Get
            Set(ByVal value As Byte())
                _SpecialCodesUpdated = value
            End Set
        End Property


#End Region

#Region " Public Methods"
        Public Overrides Function Clone() As DTOBaseClass
            Dim instance As New SpecialCode
            instance = DirectCast(MemberwiseClone(), SpecialCode)
            Return instance
        End Function

#End Region

    End Class
End Namespace
