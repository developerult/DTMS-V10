Imports Microsoft.VisualBasic
Imports System
Imports System.Collections.Generic
Imports System.Linq
Imports System.Text
Imports System.Runtime.Serialization
Imports Ngl.Core.ChangeTracker

Namespace DataTransferObjects
    <DataContract()> _
    Public Class CarrContractExpiredAlert
        Inherits DTOBaseClass


#Region " Data Members"

        Private _CarrierControl As System.Nullable(Of Integer)
        <DataMember()> _
        Public Property CarrierControl() As System.Nullable(Of Integer)
            Get
                Return Me._CarrierControl
            End Get
            Set(ByVal value As System.Nullable(Of Integer))
                If (Me._CarrierControl.Equals(value) = False) Then
                    Me._CarrierControl = value
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

        Private _AlertDate As System.Nullable(Of Date)
        <DataMember()> _
        Public Property AlertDate() As System.Nullable(Of Date)
            Get
                Return Me._AlertDate
            End Get
            Set(ByVal value As System.Nullable(Of Date))
                If (Me._AlertDate.Equals(value) = False) Then
                    Me._AlertDate = value
                End If
            End Set
        End Property

        Private _CarrierQualContractExpiresDate As System.Nullable(Of Date)
        <DataMember()> _
        Public Property CarrierQualContractExpiresDate() As System.Nullable(Of Date)
            Get
                Return Me._CarrierQualContractExpiresDate
            End Get
            Set(ByVal value As System.Nullable(Of Date))
                If (Me._CarrierQualContractExpiresDate.Equals(value) = False) Then
                    Me._CarrierQualContractExpiresDate = value
                End If
            End Set
        End Property

#End Region

#Region " Public Methods"
        Public Overrides Function Clone() As DTOBaseClass
            Dim instance As New CarrContractExpiredAlert
            instance = DirectCast(MemberwiseClone(), CarrContractExpiredAlert)
            Return instance
        End Function

#End Region

    End Class
End Namespace


