Imports Microsoft.VisualBasic
Imports System
Imports System.Collections.Generic
Imports System.Linq
Imports System.Text
Imports System.Runtime.Serialization
Imports Ngl.Core.ChangeTracker

Namespace DataTransferObjects
    <DataContract()> _
    Public Class EDITransaction
        Inherits DTOBaseClass


#Region " Data Members"

        Private _tblEDITransControl As Integer
        <DataMember()> _
        Public Property tblEDITransControl() As Integer
            Get
                Return Me._tblEDITransControl
            End Get
            Set(ByVal value As Integer)
                If ((Me._tblEDITransControl = value) _
                   = False) Then
                    Me._tblEDITransControl = value
                End If
            End Set
        End Property

        Private _tblEDITransDate As Date
        <DataMember()> _
        Public Property tblEDITransDate() As Date
            Get
                Return Me._tblEDITransDate
            End Get
            Set(ByVal value As Date)
                If ((Me._tblEDITransDate = value) _
                   = False) Then
                    Me._tblEDITransDate = value
                End If
            End Set
        End Property

        Private _BookProNumber As String
        <DataMember()> _
        Public Property BookProNumber() As String
            Get
                Return Me._BookProNumber
            End Get
            Set(ByVal value As String)
                If (String.Equals(Me._BookProNumber, value) = False) Then
                    Me._BookProNumber = value
                End If
            End Set
        End Property

        Private _BookConsPrefix As String
        <DataMember()> _
        Public Property BookConsPrefix() As String
            Get
                Return Me._BookConsPrefix
            End Get
            Set(ByVal value As String)
                If (String.Equals(Me._BookConsPrefix, value) = False) Then
                    Me._BookConsPrefix = value
                End If
            End Set
        End Property

        Private _BookCarrOrderNumber As String
        <DataMember()> _
        Public Property BookCarrOrderNumber() As String
            Get
                Return Me._BookCarrOrderNumber
            End Get
            Set(ByVal value As String)
                If (String.Equals(Me._BookCarrOrderNumber, value) = False) Then
                    Me._BookCarrOrderNumber = value
                End If
            End Set
        End Property

        Private _BookOrderSequence As System.Nullable(Of Integer)
        <DataMember()> _
        Public Property BookOrderSequence() As System.Nullable(Of Integer)
            Get
                Return Me._BookOrderSequence
            End Get
            Set(ByVal value As System.Nullable(Of Integer))
                If (Me._BookOrderSequence.Equals(value) = False) Then
                    Me._BookOrderSequence = value
                End If
            End Set
        End Property

        Private _BookOrigName As String
        <DataMember()> _
        Public Property BookOrigName() As String
            Get
                Return Me._BookOrigName
            End Get
            Set(ByVal value As String)
                If (String.Equals(Me._BookOrigName, value) = False) Then
                    Me._BookOrigName = value
                End If
            End Set
        End Property

        Private _BookOrigAddress1 As String
        <DataMember()> _
        Public Property BookOrigAddress1() As String
            Get
                Return Me._BookOrigAddress1
            End Get
            Set(ByVal value As String)
                If (String.Equals(Me._BookOrigAddress1, value) = False) Then
                    Me._BookOrigAddress1 = value
                End If
            End Set
        End Property

        Private _BookOrigCity As String
        <DataMember()> _
        Public Property BookOrigCity() As String
            Get
                Return Me._BookOrigCity
            End Get
            Set(ByVal value As String)
                If (String.Equals(Me._BookOrigCity, value) = False) Then
                    Me._BookOrigCity = value
                End If
            End Set
        End Property

        Private _BookOrigState As String
        <DataMember()> _
        Public Property BookOrigState() As String
            Get
                Return Me._BookOrigState
            End Get
            Set(ByVal value As String)
                If (String.Equals(Me._BookOrigState, value) = False) Then
                    Me._BookOrigState = value
                End If
            End Set
        End Property

        Private _BookOrigCountry As String
        <DataMember()> _
        Public Property BookOrigCountry() As String
            Get
                Return Me._BookOrigCountry
            End Get
            Set(ByVal value As String)
                If (String.Equals(Me._BookOrigCountry, value) = False) Then
                    Me._BookOrigCountry = value
                End If
            End Set
        End Property

        Private _BookOrigZip As String
        <DataMember()> _
        Public Property BookOrigZip() As String
            Get
                Return Me._BookOrigZip
            End Get
            Set(ByVal value As String)
                If (String.Equals(Me._BookOrigZip, value) = False) Then
                    Me._BookOrigZip = value
                End If
            End Set
        End Property

        Private _BookDestName As String
        <DataMember()> _
        Public Property BookDestName() As String
            Get
                Return Me._BookDestName
            End Get
            Set(ByVal value As String)
                If (String.Equals(Me._BookDestName, value) = False) Then
                    Me._BookDestName = value
                End If
            End Set
        End Property

        Private _BookDestAddress1 As String
        <DataMember()> _
        Public Property BookDestAddress1() As String
            Get
                Return Me._BookDestAddress1
            End Get
            Set(ByVal value As String)
                If (String.Equals(Me._BookDestAddress1, value) = False) Then
                    Me._BookDestAddress1 = value
                End If
            End Set
        End Property

        Private _BookDestCity As String
        <DataMember()> _
        Public Property BookDestCity() As String
            Get
                Return Me._BookDestCity
            End Get
            Set(ByVal value As String)
                If (String.Equals(Me._BookDestCity, value) = False) Then
                    Me._BookDestCity = value
                End If
            End Set
        End Property

        Private _BookDestState As String
        <DataMember()> _
        Public Property BookDestState() As String
            Get
                Return Me._BookDestState
            End Get
            Set(ByVal value As String)
                If (String.Equals(Me._BookDestState, value) = False) Then
                    Me._BookDestState = value
                End If
            End Set
        End Property

        Private _BookDestCountry As String
        <DataMember()> _
        Public Property BookDestCountry() As String
            Get
                Return Me._BookDestCountry
            End Get
            Set(ByVal value As String)
                If (String.Equals(Me._BookDestCountry, value) = False) Then
                    Me._BookDestCountry = value
                End If
            End Set
        End Property

        Private _BookDestZip As String
        <DataMember()> _
        Public Property BookDestZip() As String
            Get
                Return Me._BookDestZip
            End Get
            Set(ByVal value As String)
                If (String.Equals(Me._BookDestZip, value) = False) Then
                    Me._BookDestZip = value
                End If
            End Set
        End Property

        Private _CarrierNumber As System.Nullable(Of Integer)
        <DataMember()> _
        Public Property CarrierNumber() As System.Nullable(Of Integer)
            Get
                Return Me._CarrierNumber
            End Get
            Set(ByVal value As System.Nullable(Of Integer))
                If (Me._CarrierNumber.Equals(value) = False) Then
                    Me._CarrierNumber = value
                End If
            End Set
        End Property

        Private _CarrierName As String
        <DataMember()> _
        Public Property CarrierName() As String
            Get
                Return Me._CarrierName
            End Get
            Set(ByVal value As String)
                If (String.Equals(Me._CarrierName, value) = False) Then
                    Me._CarrierName = value
                End If
            End Set
        End Property

        Private _CompNumber As System.Nullable(Of Integer)
        <DataMember()> _
        Public Property CompNumber() As System.Nullable(Of Integer)
            Get
                Return Me._CompNumber
            End Get
            Set(ByVal value As System.Nullable(Of Integer))
                If (Me._CompNumber.Equals(value) = False) Then
                    Me._CompNumber = value
                End If
            End Set
        End Property

        Private _CompName As String
        <DataMember()> _
        Public Property CompName() As String
            Get
                Return Me._CompName
            End Get
            Set(ByVal value As String)
                If (String.Equals(Me._CompName, value) = False) Then
                    Me._CompName = value
                End If
            End Set
        End Property

        Private _CompNatNumber As System.Nullable(Of Integer)
        <DataMember()> _
        Public Property CompNatNumber() As System.Nullable(Of Integer)
            Get
                Return Me._CompNatNumber
            End Get
            Set(ByVal value As System.Nullable(Of Integer))
                If (Me._CompNatNumber.Equals(value) = False) Then
                    Me._CompNatNumber = value
                End If
            End Set
        End Property

        Private _CompNatName As String
        <DataMember()> _
        Public Property CompNatName() As String
            Get
                Return Me._CompNatName
            End Get
            Set(ByVal value As String)
                If (String.Equals(Me._CompNatName, value) = False) Then
                    Me._CompNatName = value
                End If
            End Set
        End Property

        Private _tblEDITransXaction As String
        <DataMember()> _
        Public Property tblEDITransXaction() As String
            Get
                Return Me._tblEDITransXaction
            End Get
            Set(ByVal value As String)
                If (String.Equals(Me._tblEDITransXaction, value) = False) Then
                    Me._tblEDITransXaction = value
                End If
            End Set
        End Property

        Private _tblEDITransSenderCode As String
        <DataMember()> _
        Public Property tblEDITransSenderCode() As String
            Get
                Return Me._tblEDITransSenderCode
            End Get
            Set(ByVal value As String)
                If (String.Equals(Me._tblEDITransSenderCode, value) = False) Then
                    Me._tblEDITransSenderCode = value
                End If
            End Set
        End Property

        Private _tblEDITransReceiverCode As String
        <DataMember()> _
        Public Property tblEDITransReceiverCode() As String
            Get
                Return Me._tblEDITransReceiverCode
            End Get
            Set(ByVal value As String)
                If (String.Equals(Me._tblEDITransReceiverCode, value) = False) Then
                    Me._tblEDITransReceiverCode = value
                End If
            End Set
        End Property

        Private _tblEDITransISASequence As Integer
        <DataMember()> _
        Public Property tblEDITransISASequence() As Integer
            Get
                Return Me._tblEDITransISASequence
            End Get
            Set(ByVal value As Integer)
                If ((Me._tblEDITransISASequence = value) _
                   = False) Then
                    Me._tblEDITransISASequence = value
                End If
            End Set
        End Property

        Private _tblEDITransGSSequence As Integer
        <DataMember()> _
        Public Property tblEDITransGSSequence() As Integer
            Get
                Return Me._tblEDITransGSSequence
            End Get
            Set(ByVal value As Integer)
                If ((Me._tblEDITransGSSequence = value) _
                   = False) Then
                    Me._tblEDITransGSSequence = value
                End If
            End Set
        End Property

        Private _tblEDITransCarrierSCAC As String
        <DataMember()> _
        Public Property tblEDITransCarrierSCAC() As String
            Get
                Return Me._tblEDITransCarrierSCAC
            End Get
            Set(ByVal value As String)
                If (String.Equals(Me._tblEDITransCarrierSCAC, value) = False) Then
                    Me._tblEDITransCarrierSCAC = value
                End If
            End Set
        End Property

        Private _tblEDITransLoadNumber As String
        <DataMember()> _
        Public Property tblEDITransLoadNumber() As String
            Get
                Return Me._tblEDITransLoadNumber
            End Get
            Set(ByVal value As String)
                If (String.Equals(Me._tblEDITransLoadNumber, value) = False) Then
                    Me._tblEDITransLoadNumber = value
                End If
            End Set
        End Property

        Private _tblEDITransMessage As String
        <DataMember()> _
        Public Property tblEDITransMessage() As String
            Get
                Return Me._tblEDITransMessage
            End Get
            Set(ByVal value As String)
                If (String.Equals(Me._tblEDITransMessage, value) = False) Then
                    Me._tblEDITransMessage = value
                End If
            End Set
        End Property
#End Region

#Region " Public Methods"
        Public Overrides Function Clone() As DTOBaseClass
            Dim instance As New EDITransaction
            instance = DirectCast(MemberwiseClone(), EDITransaction)
            Return instance
        End Function

#End Region

    End Class
End Namespace

