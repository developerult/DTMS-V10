Imports Microsoft.VisualBasic
Imports System
Imports System.Collections.Generic
Imports System.Linq
Imports System.Text
Imports System.Runtime.Serialization
Imports Ngl.Core.ChangeTracker

Namespace DataTransferObjects
    <DataContract()> _
    Public Class vCarrier
        Inherits DTOBaseClass


#Region " Data Members"

        Private _CarrierControl As Integer = 0
        <DataMember()> _
        Public Property CarrierControl() As Integer
            Get
                Return _CarrierControl
            End Get
            Set(ByVal value As Integer)
                _CarrierControl = value
            End Set
        End Property

        Private _CarrierNumber As Integer = 0
        <DataMember()> _
        Public Property CarrierNumber() As Integer
            Get
                Return _CarrierNumber
            End Get
            Set(ByVal value As Integer)
                _CarrierNumber = value
            End Set
        End Property

        Private _CarrierName As String = ""
        <DataMember()> _
        Public Property CarrierName() As String
            Get
                Return Left(_CarrierName, 40)
            End Get
            Set(ByVal value As String)
                _CarrierName = Left(value, 40)
            End Set
        End Property


        Private _CarrierAllowWebTender As Boolean = False
        <DataMember()> _
        Public Property CarrierAllowWebTender() As Boolean
            Get
                Return _CarrierAllowWebTender
            End Get
            Set(ByVal value As Boolean)
                _CarrierAllowWebTender = value
            End Set
        End Property
 
#End Region

#Region " Public Methods"
        Public Overrides Function Clone() As DTOBaseClass
            Dim instance As New vCarrier
            instance = DirectCast(MemberwiseClone(), vCarrier)
             
            Return instance
        End Function

#End Region

    End Class
End Namespace