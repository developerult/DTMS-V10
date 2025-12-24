Imports Microsoft.VisualBasic
Imports System
Imports System.Collections.Generic
Imports System.Linq
Imports System.Text
Imports System.Runtime.Serialization
Imports Ngl.Core.ChangeTracker

Namespace DataTransferObjects
    <DataContract()> _
    Public Class Code
        Inherits DTOBaseClass


#Region " Data Members"

        Private _CodeKey As String = ""
        <DataMember()> _
        Public Property CodeKey() As String
            Get
                Return Left(_CodeKey, 5)
            End Get
            Set(ByVal value As String)
                _CodeKey = Left(value, 5)
            End Set
        End Property

        Private _CodeType As String = ""
        <DataMember()> _
        Public Property CodeType() As String
            Get
                Return Left(_CodeType, 10)
            End Get
            Set(ByVal value As String)
                _CodeType = Left(value, 10)
            End Set
        End Property

        Private _CodeDescription As String = ""
        <DataMember()> _
        Public Property CodeDescription() As String
            Get
                Return Left(_CodeDescription, 100)
            End Get
            Set(ByVal value As String)
                _CodeDescription = Left(value, 100)
            End Set
        End Property

        Private _CodeUpdated As Byte()
        <DataMember()> _
        Public Property CodeUpdated() As Byte()
            Get
                Return _CodeUpdated
            End Get
            Set(ByVal value As Byte())
                _CodeUpdated = value
            End Set
        End Property


#End Region

#Region " Public Methods"
        Public Overrides Function Clone() As DTOBaseClass
            Dim instance As New Code
            instance = DirectCast(MemberwiseClone(), Code)
            Return instance
        End Function

#End Region

    End Class
End Namespace
