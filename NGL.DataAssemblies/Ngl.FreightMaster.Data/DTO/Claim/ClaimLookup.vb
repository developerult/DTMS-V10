Imports Microsoft.VisualBasic
Imports System
Imports System.Collections.Generic
Imports System.Linq
Imports System.Text
Imports System.Runtime.Serialization
Imports Ngl.Core.ChangeTracker

Namespace DataTransferObjects
    <DataContract()> _
    Public Class ClaimLookup
        Inherits DTOBaseClass


#Region " Data Members"
        Private _ClaimControl As Integer = 0
        <DataMember()> _
        Public Property ClaimControl() As Integer
            Get
                Return Me._ClaimControl
            End Get
            Friend Set(ByVal value As Integer)
                Me._ClaimControl = value
            End Set
        End Property

        Private _CarrierName As String = ""
        <DataMember()> _
        Public Property CarrierName() As String
            Get
                Return Me._CarrierName
            End Get
            Friend Set(ByVal value As String)
                Me._CarrierName = value
            End Set
        End Property

        Private _CarrierNumber As Integer = 0
        <DataMember()> _
        Public Property CarrierNumber() As Integer
            Get
                Return Me._CarrierNumber
            End Get
            Friend Set(ByVal value As Integer)
                Me._CarrierNumber = value
            End Set
        End Property

        Private _ClaimProNumber As String = ""
        <DataMember()> _
        Public Property ClaimProNumber() As String
            Get
                Return Me._ClaimProNumber
            End Get
            Friend Set(ByVal value As String)
                Me._ClaimProNumber = value
            End Set
        End Property

        Private _ClaimVendName As String = ""
        <DataMember()> _
        Public Property ClaimVendName() As String
            Get
                Return Me._ClaimVendName
            End Get
            Friend Set(ByVal value As String)
                Me._ClaimVendName = value
            End Set
        End Property

        Private _ClaimFB As String = ""
        <DataMember()> _
        Public Property ClaimFB() As String
            Get
                Return Me._ClaimFB
            End Get
            Friend Set(ByVal value As String)
                Me._ClaimFB = value
            End Set
        End Property
#End Region

#Region " Public Methods"
        Public Overrides Function Clone() As DTOBaseClass
            Dim instance As New ClaimLookup
            instance = DirectCast(MemberwiseClone(), ClaimLookup)
            Return instance
        End Function

#End Region


    End Class

End Namespace