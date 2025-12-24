Imports Microsoft.VisualBasic
Imports System
Imports System.Collections.Generic
Imports System.Linq
Imports System.Text
Imports System.Runtime.Serialization
Imports Ngl.Core.ChangeTracker

'Added By LVV on 9/21/16 for v-7.0.5.110 HDM Enhancement

Namespace DataTransferObjects
    <DataContract()> _
    Public Class tblHDMTariffXref
        Inherits DTOBaseClass


#Region " Data Members"

        Private _HDMTariffXrefControl As Integer = 0
        <DataMember()> _
        Public Property HDMTariffXrefControl() As Integer
            Get
                Return _HDMTariffXrefControl
            End Get
            Set(ByVal value As Integer)
                _HDMTariffXrefControl = value
            End Set
        End Property

        Private _HDMTariffXrefCarrTarControl As Integer = 0
        <DataMember()> _
        Public Property HDMTariffXrefCarrTarControl() As Integer
            Get
                Return _HDMTariffXrefCarrTarControl
            End Get
            Set(ByVal value As Integer)
                _HDMTariffXrefCarrTarControl = value
            End Set
        End Property

        Private _HDMTariffXrefHDMControl As Integer = 0
        <DataMember()> _
        Public Property HDMTariffXrefHDMControl() As Integer
            Get
                Return _HDMTariffXrefHDMControl
            End Get
            Set(ByVal value As Integer)
                _HDMTariffXrefHDMControl = value
            End Set
        End Property

        Private _HDMTariffXrefCarrierControl As Integer = 0
        <DataMember()> _
        Public Property HDMTariffXrefCarrierControl() As Integer
            Get
                Return _HDMTariffXrefCarrierControl
            End Get
            Set(ByVal value As Integer)
                _HDMTariffXrefCarrierControl = value
            End Set
        End Property

#End Region

#Region " Public Methods"

        Public Overrides Function Clone() As DTOBaseClass
            Dim instance As New tblHDMTariffXref
            instance = DirectCast(MemberwiseClone(), tblHDMTariffXref)
            Return instance
        End Function

#End Region

    End Class
End Namespace
