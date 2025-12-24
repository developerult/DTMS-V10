Imports Microsoft.VisualBasic
Imports System
Imports System.Collections.Generic
Imports System.Linq
Imports System.Text
Imports System.Runtime.Serialization
Imports Ngl.Core.ChangeTracker

Namespace DataTransferObjects
    <DataContract()> _
    Public Class ClaimLoadTypeCode
        Inherits DTOBaseClass


#Region " Data Members"
        Private _ClaimLoadTypeCode As String = ""
        <DataMember()> _
        Public Property ClaimLoadTypeCode() As String
            Get
                Return Me._ClaimLoadTypeCode
            End Get
            Set(ByVal value As String)
                If (String.Equals(Me._ClaimLoadTypeCode, value) = False) Then
                    Me._ClaimLoadTypeCode = value
                    Me.SendPropertyChanged("ClaimLoadTypeCode")
                End If
            End Set
        End Property

        Private _ClaimLoadTypeCodesUpdated As Byte()
        <DataMember()> _
        Public Property ClaimLoadTypeCodesUpdated() As Byte()
            Get
                Return Me._ClaimLoadTypeCodesUpdated
            End Get
            Set(ByVal value As Byte())
                _ClaimLoadTypeCodesUpdated = value
                Me.SendPropertyChanged("ClaimLoadTypeCodesUpdated")
            End Set
        End Property

#End Region

#Region " Public Methods"
        Public Overrides Function Clone() As DTOBaseClass
            Dim instance As New ClaimLoadTypeCode
            instance = DirectCast(MemberwiseClone(), ClaimLoadTypeCode)
            Return instance
        End Function

#End Region

    End Class

End Namespace
