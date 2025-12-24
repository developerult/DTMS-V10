Imports Microsoft.VisualBasic
Imports System
Imports System.Collections.Generic
Imports System.Linq
Imports System.Text
Imports System.Runtime.Serialization
Imports Ngl.Core.ChangeTracker

Namespace DataTransferObjects
    <DataContract()> _
    Public Class CarrInsExpiredAlert
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

        Private _CarrierQualInsuranceDate As System.Nullable(Of Date)
        <DataMember()> _
        Public Property CarrierQualInsuranceDate() As System.Nullable(Of Date)
            Get
                Return Me._CarrierQualInsuranceDate
            End Get
            Set(ByVal value As System.Nullable(Of Date))
                If (Me._CarrierQualInsuranceDate.Equals(value) = False) Then
                    Me._CarrierQualInsuranceDate = value
                End If
            End Set
        End Property
#End Region

#Region " Public Methods"
        Public Overrides Function Clone() As DTOBaseClass
            Dim instance As New CarrInsExpiredAlert
            instance = DirectCast(MemberwiseClone(), CarrInsExpiredAlert)
            Return instance
        End Function

#End Region

    End Class
End Namespace


