Imports Microsoft.VisualBasic
Imports System
Imports System.Collections.Generic
Imports System.Linq
Imports System.Text
Imports System.Runtime.Serialization
Imports Ngl.Core.ChangeTracker

'Added by LVV 5/13/16 for v-7.0.5.1 DAT

Namespace DataTransferObjects
    <DataContract()> _
    Public Class CarrierTenderData
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

        Private _CarrierContControl As Integer = 0
        <DataMember()> _
        Public Property CarrierContControl() As Integer
            Get
                Return _CarrierContControl
            End Get
            Set(ByVal value As Integer)
                _CarrierContControl = value
            End Set
        End Property

        Private _CarrierContName As String = ""
        <DataMember()> _
        Public Property CarrierContName() As String
            Get
                Return Left(_CarrierContName, 25)
            End Get
            Set(ByVal value As String)
                _CarrierContName = Left(value, 25)
            End Set
        End Property

        Private _CarrierContactPhone As String = ""
        <DataMember()> _
        Public Property CarrierContactPhone() As String
            Get
                Return Left(_CarrierContactPhone, 20) ' Modified by RHR for v-8.4.003 on 06/25/2021
            End Get
            Set(ByVal value As String)
                _CarrierContactPhone = Left(value, 20) ' Modified by RHR for v-8.4.003 on 06/25/2021
            End Set
        End Property

        Private _BookConsPrefix As String = ""
        <DataMember()> _
        Public Property BookConsPrefix() As String
            Get
                Return Left(_BookConsPrefix, 20)
            End Get
            Set(ByVal value As String)
                _BookConsPrefix = Left(value, 20)
            End Set
        End Property

#End Region

#Region " Public Methods"
        Public Overrides Function Clone() As DTOBaseClass
            Dim instance As New CarrierTenderData
            instance = DirectCast(MemberwiseClone(), CarrierTenderData)
            Return instance
        End Function

#End Region

    End Class
End Namespace


