Imports Microsoft.VisualBasic
Imports System
Imports System.Collections.Generic
Imports System.Linq
Imports System.Text
Imports System.Runtime.Serialization
Imports Ngl.Core.ChangeTracker


Namespace DataTransferObjects
    <DataContract()> _
    Public Class tblIntegrationType
        Inherits DTOBaseClass
 
#Region " Public Methods"
        Public Overrides Function Clone() As DTOBaseClass
            Dim instance As New tblIntegrationType
            instance = DirectCast(MemberwiseClone(), tblIntegrationType)
            Return instance
        End Function
#End Region
         
#Region " Data Members"

        Private _IntegrationTypeControl As Integer
        <DataMember()> _
        Public Property IntegrationTypeControl() As Integer
            Get
                Return Me._IntegrationTypeControl
            End Get
            Set(value As Integer)
                If ((Me._IntegrationTypeControl = value) _
                            = False) Then
                    Me._IntegrationTypeControl = value
                    Me.SendPropertyChanged("IntegrationTypeControl")
                End If
            End Set
        End Property




        Private _Name As String
        <DataMember()> _
        Public Property Name() As String
            Get
                Return Left(_Name, 50)
            End Get
            Set(value As String)
                If (String.Equals(Me._Name, value) = False) Then
                    Me._Name = Left(value, 50)
                    Me.SendPropertyChanged("Name")
                End If
            End Set
        End Property


        Private _Description As String
        <DataMember()> _
        Public Property Description() As String
            Get
                Return Left(_Description, 100)
            End Get
            Set(value As String)
                If (String.Equals(Me._Description, value) = False) Then
                    Me._Description = Left(value, 100)
                    Me.SendPropertyChanged("Description")
                End If
            End Set
        End Property


        Private _Notes As String
        <DataMember()> _
        Public Property Notes() As String
            Get
                Return Left(_Notes, 250)
            End Get
            Set(value As String)
                If (String.Equals(Me._Notes, value) = False) Then
                    Me._Notes = Left(value, 250)
                    Me.SendPropertyChanged("Notes")
                End If
            End Set
        End Property


        Private _TMSFirstVersion As String
        <DataMember()> _
        Public Property TMSFirstVersion() As String
            Get
                Return Left(_TMSFirstVersion, 10)
            End Get
            Set(value As String)
                If (String.Equals(Me._TMSFirstVersion, value) = False) Then
                    Me._TMSFirstVersion = Left(value, 10)
                    Me.SendPropertyChanged("TMSFirstVersion")
                End If
            End Set
        End Property


        Private _TMSEndVersion As String
        <DataMember()> _
        Public Property TMSEndVersion() As String
            Get
                Return Left(_TMSEndVersion, 10)
            End Get
            Set(value As String)
                If (String.Equals(Me._TMSEndVersion, value) = False) Then
                    Me._TMSEndVersion = Left(value, 10)
                    Me.SendPropertyChanged("TMSEndVersion")
                End If
            End Set
        End Property


        Private _IntegrationTypeModDate As System.Nullable(Of Date)
        <DataMember()> _
        Public Property IntegrationTypeModDate() As System.Nullable(Of Date)
            Get
                Return Me._IntegrationTypeModDate
            End Get
            Set(value As System.Nullable(Of Date))
                If (Me._IntegrationTypeModDate.Equals(value) = False) Then
                    Me._IntegrationTypeModDate = value
                    Me.SendPropertyChanged("IntegrationTypeModDate")
                End If
            End Set
        End Property


        Private _IntegrationTypeModUser As String
        <DataMember()> _
        Public Property IntegrationTypeModUser() As String
            Get
                Return Left(_IntegrationTypeModUser, 100)
            End Get
            Set(value As String)
                If (String.Equals(Me._IntegrationTypeModUser, value) = False) Then
                    Me._IntegrationTypeModUser = Left(value, 100)
                    Me.SendPropertyChanged("IntegrationTypeModUser")
                End If
            End Set
        End Property


        Private _IntegrationTypeUpdated As Byte()
        <DataMember()> _
        Public Property IntegrationTypeUpdated() As Byte()
            Get
                Return Me._IntegrationTypeUpdated
            End Get
            Set(value As Byte())
                If (Object.Equals(Me._IntegrationTypeUpdated, value) = False) Then
                    Me._IntegrationTypeUpdated = value
                    Me.SendPropertyChanged("IntegrationTypeUpdated")
                End If
            End Set
        End Property

       

#End Region












    End Class

End Namespace






'<Global.System.Data.Linq.Mapping.ColumnAttribute(Storage:="_IntegrationTypeControl", AutoSync:=AutoSync.OnInsert, DbType:="Int NOT NULL IDENTITY", IsPrimaryKey:=True, IsDbGenerated:=True, UpdateCheck:=UpdateCheck.Never)> _
'<Global.System.Data.Linq.Mapping.ColumnAttribute(Storage:="_Name", DbType:="NVarChar(50) NOT NULL", CanBeNull:=False, UpdateCheck:=UpdateCheck.Never)> _
'<Global.System.Data.Linq.Mapping.ColumnAttribute(Storage:="_Description", DbType:="NVarChar(100)", UpdateCheck:=UpdateCheck.Never)> _
'<Global.System.Data.Linq.Mapping.ColumnAttribute(Storage:="_Notes", DbType:="NChar(10)", UpdateCheck:=UpdateCheck.Never)> _
'<Global.System.Data.Linq.Mapping.ColumnAttribute(Storage:="_TMSFristVersion", DbType:="NChar(10)", UpdateCheck:=UpdateCheck.Never)> _
'<Global.System.Data.Linq.Mapping.ColumnAttribute(Storage:="_TMSEndVersion", DbType:="NChar(10)", UpdateCheck:=UpdateCheck.Never)> _
'<Global.System.Data.Linq.Mapping.ColumnAttribute(Storage:="_IntegrationTypeModUser", DbType:="NVarChar(100)", UpdateCheck:=UpdateCheck.Never)> _
'<Global.System.Data.Linq.Mapping.ColumnAttribute(Storage:="_IntegrationTypeModDate", DbType:="DateTime", UpdateCheck:=UpdateCheck.Never)> _
'<Global.System.Data.Linq.Mapping.ColumnAttribute(Storage:="_IntegrationTypeUpdated", AutoSync:=AutoSync.Always, DbType:="rowversion", IsDbGenerated:=True, IsVersion:=True, UpdateCheck:=UpdateCheck.Never)> _
'<Global.System.Data.Linq.Mapping.AssociationAttribute(Name:="tblIntegrationType_Integration", Storage:="_Integrations", ThisKey:="IntegrationTypeControl", OtherKey:="IntegrationTypeControl")> _
