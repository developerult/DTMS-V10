Imports Microsoft.VisualBasic
Imports System
Imports System.Collections.Generic
Imports System.Linq
Imports System.Text
Imports System.Runtime.Serialization
Imports Ngl.Core.ChangeTracker

Namespace DataTransferObjects
    <DataContract()> _
    Public Class CarrierSCAC
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


        Private _SCAC As String = ""
        <DataMember()> _
        Public Property SCAC() As String
            Get
                Return Left(_SCAC, 255)
            End Get
            Set(ByVal value As String)
                _SCAC = Left(value, 255)
            End Set
        End Property

        Private _Carrier As String = ""
        <DataMember()> _
        Public Property Carrier() As String
            Get
                Return Left(_Carrier, 255)
            End Get
            Set(ByVal value As String)
                _Carrier = Left(value, 255)
            End Set
        End Property

#End Region

#Region " Public Methods"
        Public Overrides Function Clone() As DTOBaseClass
            Dim instance As New CarrierSCAC
            instance = DirectCast(MemberwiseClone(), CarrierSCAC)
            Return instance
        End Function

#End Region

    End Class
End Namespace