

Imports Microsoft.VisualBasic
Imports System
Imports System.Collections.Generic
Imports System.Linq
Imports System.Text
Imports System.Runtime.Serialization
Imports Ngl.Core.ChangeTracker

Namespace DataTransferObjects
    <DataContract()> _
    Public Class PCMAddress
        Inherits DTOBaseClass


#Region " Data Members"

        Private _strAddress As String = ""
        <DataMember()> _
        Public Property strAddress() As String
            Get
                Return _strAddress
            End Get
            Set(ByVal value As String)
                _strAddress = value
                NotifyPropertyChanged("strAddress")
            End Set
        End Property

        Private _strCity As String = ""
        <DataMember()> _
        Public Property strCity() As String
            Get
                Return _strCity
            End Get
            Set(ByVal value As String)
                _strCity = value
                NotifyPropertyChanged("strCity")
            End Set
        End Property

        Private _strState As String = ""
        <DataMember()> _
        Public Property strState() As String
            Get
                Return _strState
            End Get
            Set(ByVal value As String)
                _strState = value
                NotifyPropertyChanged("strState")
            End Set
        End Property

        Private _strZip As String = ""
        <DataMember()> _
        Public Property strZip() As String
            Get
                Return _strZip
            End Get
            Set(ByVal value As String)
                _strZip = value
                NotifyPropertyChanged("strZip")
            End Set
        End Property
#End Region

#Region " Public Methods"
        Public Overrides Function Clone() As DTOBaseClass
            Dim instance As New PCMAddress
            instance = DirectCast(MemberwiseClone(), PCMAddress)
            Return instance
        End Function

        
#End Region

    End Class

End Namespace

