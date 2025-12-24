Imports Microsoft.VisualBasic
Imports System
Imports System.Collections.Generic
Imports System.Linq
Imports System.Text
Imports System.Runtime.Serialization
Imports Ngl.Core.ChangeTracker

Namespace DataTransferObjects
    <DataContract()> _
    Public Class spNGL063SSRSResult
        Inherits DTOBaseClass

#Region " Data Members"

        Private _ParCSZC As String

        Private _ParCompanyName As String

        Private _ParCompanyAdd1 As String

        Private _ParCompanyAdd2 As String

        Private _ParCompanyPhone As String

        Private _ParCompanyFax As String

        Private _ParCompanyTaxID As String

        Private _ParCOMPANYICC As String

        Private _ParCOMPANYFMC As String

        Private _CarrierConMsg As String

        Private _Version As String

        Private _VersionDescription As String

        Private _VersionDate As Date

        Private _LogoURL As String

        Private _CarrierNumber As System.Nullable(Of Integer)

        Private _CarrierName As String

        Private _CarrierStreetAddress1 As String

        Private _CarrierStreetAddress2 As String

        Private _CarrierStreetAddress3 As String

        Private _CarrierStreetCity As String

        Private _CarrierStreetState As String

        Private _CarrierStreetCountry As String

        Private _CarrierStreetZip As String

        Private _Month As String

        Private _InvMonth As System.Nullable(Of Integer)

        Private _Loads As System.Nullable(Of Integer)

        Private _OnTime As System.Nullable(Of Integer)

        Private _Late As System.Nullable(Of Integer)

        Private _Early As System.Nullable(Of Integer)

        Private _YearNumber As System.Nullable(Of Integer)
        Private _ValuePerf As Decimal

        <DataMember()> _
        Public Property ParCSZC() As String
            Get
                Return Me._ParCSZC
            End Get
            Set(value As String)
                If (String.Equals(Me._ParCSZC, value) = False) Then
                    Me._ParCSZC = value
                End If
            End Set
        End Property

        <DataMember()> _
        Public Property ParCompanyName() As String
            Get
                Return Me._ParCompanyName
            End Get
            Set(value As String)
                If (String.Equals(Me._ParCompanyName, value) = False) Then
                    Me._ParCompanyName = value
                End If
            End Set
        End Property

        <DataMember()> _
        Public Property ParCompanyAdd1() As String
            Get
                Return Me._ParCompanyAdd1
            End Get
            Set(value As String)
                If (String.Equals(Me._ParCompanyAdd1, value) = False) Then
                    Me._ParCompanyAdd1 = value
                End If
            End Set
        End Property

        <DataMember()> _
        Public Property ParCompanyAdd2() As String
            Get
                Return Me._ParCompanyAdd2
            End Get
            Set(value As String)
                If (String.Equals(Me._ParCompanyAdd2, value) = False) Then
                    Me._ParCompanyAdd2 = value
                End If
            End Set
        End Property

        <DataMember()> _
        Public Property ParCompanyPhone() As String
            Get
                Return Me._ParCompanyPhone
            End Get
            Set(value As String)
                If (String.Equals(Me._ParCompanyPhone, value) = False) Then
                    Me._ParCompanyPhone = value
                End If
            End Set
        End Property

        <DataMember()> _
        Public Property ParCompanyFax() As String
            Get
                Return Me._ParCompanyFax
            End Get
            Set(value As String)
                If (String.Equals(Me._ParCompanyFax, value) = False) Then
                    Me._ParCompanyFax = value
                End If
            End Set
        End Property

        <DataMember()> _
        Public Property ParCompanyTaxID() As String
            Get
                Return Me._ParCompanyTaxID
            End Get
            Set(value As String)
                If (String.Equals(Me._ParCompanyTaxID, value) = False) Then
                    Me._ParCompanyTaxID = value
                End If
            End Set
        End Property

        <DataMember()> _
        Public Property ParCOMPANYICC() As String
            Get
                Return Me._ParCOMPANYICC
            End Get
            Set(value As String)
                If (String.Equals(Me._ParCOMPANYICC, value) = False) Then
                    Me._ParCOMPANYICC = value
                End If
            End Set
        End Property

        <DataMember()> _
        Public Property ParCOMPANYFMC() As String
            Get
                Return Me._ParCOMPANYFMC
            End Get
            Set(value As String)
                If (String.Equals(Me._ParCOMPANYFMC, value) = False) Then
                    Me._ParCOMPANYFMC = value
                End If
            End Set
        End Property

        <DataMember()> _
        Public Property CarrierConMsg() As String
            Get
                Return Me._CarrierConMsg
            End Get
            Set(value As String)
                If (String.Equals(Me._CarrierConMsg, value) = False) Then
                    Me._CarrierConMsg = value
                End If
            End Set
        End Property

        <DataMember()> _
        Public Property Version() As String
            Get
                Return Me._Version
            End Get
            Set(value As String)
                If (String.Equals(Me._Version, value) = False) Then
                    Me._Version = value
                End If
            End Set
        End Property

        <DataMember()> _
        Public Property VersionDescription() As String
            Get
                Return Me._VersionDescription
            End Get
            Set(value As String)
                If (String.Equals(Me._VersionDescription, value) = False) Then
                    Me._VersionDescription = value
                End If
            End Set
        End Property

        <DataMember()> _
        Public Property VersionDate() As Date
            Get
                Return Me._VersionDate
            End Get
            Set(value As Date)
                If ((Me._VersionDate = value) _
                            = False) Then
                    Me._VersionDate = value
                End If
            End Set
        End Property

        <DataMember()> _
        Public Property LogoURL() As String
            Get
                Return Me._LogoURL
            End Get
            Set(value As String)
                If (String.Equals(Me._LogoURL, value) = False) Then
                    Me._LogoURL = value
                End If
            End Set
        End Property

        <DataMember()> _
        Public Property CarrierNumber() As System.Nullable(Of Integer)
            Get
                Return Me._CarrierNumber
            End Get
            Set(value As System.Nullable(Of Integer))
                If (Me._CarrierNumber.Equals(value) = False) Then
                    Me._CarrierNumber = value
                End If
            End Set
        End Property

        <DataMember()> _
        Public Property CarrierName() As String
            Get
                Return Me._CarrierName
            End Get
            Set(value As String)
                If (String.Equals(Me._CarrierName, value) = False) Then
                    Me._CarrierName = value
                End If
            End Set
        End Property

        <DataMember()> _
        Public Property CarrierStreetAddress1() As String
            Get
                Return Me._CarrierStreetAddress1
            End Get
            Set(value As String)
                If (String.Equals(Me._CarrierStreetAddress1, value) = False) Then
                    Me._CarrierStreetAddress1 = value
                End If
            End Set
        End Property

        <DataMember()> _
        Public Property CarrierStreetAddress2() As String
            Get
                Return Me._CarrierStreetAddress2
            End Get
            Set(value As String)
                If (String.Equals(Me._CarrierStreetAddress2, value) = False) Then
                    Me._CarrierStreetAddress2 = value
                End If
            End Set
        End Property

        <DataMember()> _
        Public Property CarrierStreetAddress3() As String
            Get
                Return Me._CarrierStreetAddress3
            End Get
            Set(value As String)
                If (String.Equals(Me._CarrierStreetAddress3, value) = False) Then
                    Me._CarrierStreetAddress3 = value
                End If
            End Set
        End Property

        <DataMember()> _
        Public Property CarrierStreetCity() As String
            Get
                Return Me._CarrierStreetCity
            End Get
            Set(value As String)
                If (String.Equals(Me._CarrierStreetCity, value) = False) Then
                    Me._CarrierStreetCity = value
                End If
            End Set
        End Property

        <DataMember()> _
        Public Property CarrierStreetState() As String
            Get
                Return Me._CarrierStreetState
            End Get
            Set(value As String)
                If (String.Equals(Me._CarrierStreetState, value) = False) Then
                    Me._CarrierStreetState = value
                End If
            End Set
        End Property

        <DataMember()> _
        Public Property CarrierStreetCountry() As String
            Get
                Return Me._CarrierStreetCountry
            End Get
            Set(value As String)
                If (String.Equals(Me._CarrierStreetCountry, value) = False) Then
                    Me._CarrierStreetCountry = value
                End If
            End Set
        End Property

        <DataMember()> _
        Public Property CarrierStreetZip() As String
            Get
                Return Me._CarrierStreetZip
            End Get
            Set(value As String)
                If (String.Equals(Me._CarrierStreetZip, value) = False) Then
                    Me._CarrierStreetZip = value
                End If
            End Set
        End Property

        <DataMember()> _
        Public Property Month() As String
            Get
                Return Me._Month
            End Get
            Set(value As String)
                If (String.Equals(Me._Month, value) = False) Then
                    Me._Month = value
                End If
            End Set
        End Property

        <DataMember()> _
        Public Property InvMonth() As System.Nullable(Of Integer)
            Get
                Return Me._InvMonth
            End Get
            Set(value As System.Nullable(Of Integer))
                If (Me._InvMonth.Equals(value) = False) Then
                    Me._InvMonth = value
                End If
            End Set
        End Property

        <DataMember()> _
        Public Property Loads() As System.Nullable(Of Integer)
            Get
                Return Me._Loads
            End Get
            Set(value As System.Nullable(Of Integer))
                If (Me._Loads.Equals(value) = False) Then
                    Me._Loads = value
                End If
            End Set
        End Property

        <DataMember()> _
        Public Property OnTime() As System.Nullable(Of Integer)
            Get
                Return Me._OnTime
            End Get
            Set(value As System.Nullable(Of Integer))
                If (Me._OnTime.Equals(value) = False) Then
                    Me._OnTime = value
                End If
            End Set
        End Property

        <DataMember()> _
        Public Property Late() As System.Nullable(Of Integer)
            Get
                Return Me._Late
            End Get
            Set(value As System.Nullable(Of Integer))
                If (Me._Late.Equals(value) = False) Then
                    Me._Late = value
                End If
            End Set
        End Property

        <DataMember()> _
        Public Property Early() As System.Nullable(Of Integer)
            Get
                Return Me._Early
            End Get
            Set(value As System.Nullable(Of Integer))
                If (Me._Early.Equals(value) = False) Then
                    Me._Early = value
                End If
            End Set
        End Property
        <DataMember()> _
        Public Property YearNumber() As System.Nullable(Of Integer)
            Get
                Return Me._YearNumber
            End Get
            Set(value As System.Nullable(Of Integer))
                If (Me._YearNumber.Equals(value) = False) Then
                    Me._YearNumber = value
                End If
            End Set
        End Property

        <DataMember()> _
        Public Property ValuePerf() As Decimal
            Get
                Return Me._ValuePerf
            End Get
            Set(value As Decimal)
                If (Me._ValuePerf.Equals(value) = False) Then
                    Me._ValuePerf = value
                End If
            End Set
        End Property

#End Region

#Region " Public Methods"
        Public Overrides Function Clone() As DTOBaseClass
            Dim instance As New spNGL063SSRSResult
            instance = DirectCast(MemberwiseClone(), spNGL063SSRSResult)
            Return instance
        End Function

#End Region
    End Class
End Namespace