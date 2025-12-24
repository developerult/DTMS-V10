Imports Microsoft.VisualBasic
Imports System
Imports System.Collections.Generic
Imports System.Linq
Imports System.Text
Imports System.Runtime.Serialization
Imports Ngl.Core.ChangeTracker

Namespace DataTransferObjects
    <DataContract()> _
    Public Class EmailLocalizationData
        Inherits DTOBaseClass

#Region " Constructor"

        Public Sub New()
            MyBase.New()
        End Sub

        Public Sub New(ByVal type As Utilities.EmailLocalizationTypesEnum)
            MyBase.New()
            Me.Type = type
            PopulateDefaults()
        End Sub
#End Region

#Region " Data Members"

        Private _Type As Utilities.EmailLocalizationTypesEnum = Utilities.EmailLocalizationTypesEnum.LoadAccepted
        <DataMember()> _
        Public Property Type() As Utilities.EmailLocalizationTypesEnum
            Get
                Return _Type
            End Get
            Set(ByVal value As Utilities.EmailLocalizationTypesEnum)
                _Type = value
            End Set
        End Property

        Private _BodyFormat As String = ""
        <DataMember()> _
        Public Property BodyFormat() As String
            Get
                Return Left(_BodyFormat, 4000)
            End Get
            Set(ByVal value As String)
                _BodyFormat = Left(value, 4000)
            End Set
        End Property

        Private _BodyLocalized As String = ""
        <DataMember()> _
        Public Property BodyLocalized() As String
            Get
                Return Left(_BodyLocalized, 100)
            End Get
            Set(ByVal value As String)
                _BodyLocalized = Left(value, 100)
            End Set
        End Property

        Private _BodyKeys As String = ""
        <DataMember()> _
        Public Property BodyKeys() As String
            Get
                Return Left(_BodyKeys, 4000)
            End Get
            Set(ByVal value As String)
                _BodyKeys = Left(value, 4000)
            End Set
        End Property

        Private _BodyKeyList As New List(Of String)
        <DataMember()> _
        Public Property BodyKeyList() As List(Of String)
            Get
                Return _BodyKeyList
            End Get
            Set(ByVal value As List(Of String))
                _BodyKeyList = value
            End Set
        End Property

        Private _SubjectFormat As String = ""
        <DataMember()> _
        Public Property SubjectFormat() As String
            Get
                Return Left(_SubjectFormat, 100)
            End Get
            Set(ByVal value As String)
                _SubjectFormat = Left(value, 100)
            End Set
        End Property

        Private _SubjectLocalized As String = ""
        <DataMember()> _
        Public Property SubjectLocalized() As String
            Get
                Return Left(_SubjectLocalized, 100)
            End Get
            Set(ByVal value As String)
                _SubjectLocalized = Left(value, 100)
            End Set
        End Property

        Private _SubjectKeys As String = ""
        <DataMember()> _
        Public Property SubjectKeys() As String
            Get
                Return Left(_SubjectKeys, 4000)
            End Get
            Set(ByVal value As String)
                _SubjectKeys = Left(value, 4000)
            End Set
        End Property

        Private _SubjectKeyList As New List(Of String)
        <DataMember()> _
        Public Property SubjectKeyList() As List(Of String)
            Get
                Return _SubjectKeyList
            End Get
            Set(ByVal value As List(Of String))
                _SubjectKeyList = value
            End Set
        End Property




#End Region

#Region " Public Methods"
        Public Overrides Function Clone() As DTOBaseClass
            Dim instance As New EmailLocalizationData
            instance = DirectCast(MemberwiseClone(), EmailLocalizationData)
            Return instance
        End Function


        Public Sub PopulateDefaults()
            Dim bodyitem As Utilities.EmailBodyLocalizedEnum
            Dim subjectitem As Utilities.EmailSubjectLocalizedEnum
            Select Case Me.Type
                Case Utilities.EmailLocalizationTypesEnum.LoadAccepted
                    bodyitem = Utilities.EmailBodyLocalizedEnum.EBody_LoadAccepted
                    subjectitem = Utilities.EmailSubjectLocalizedEnum.ESubject_LoadAccepted
                Case Utilities.EmailLocalizationTypesEnum.LoadManuallyAccepted
                    bodyitem = Utilities.EmailBodyLocalizedEnum.EBody_LoadManuallyAccepted
                    subjectitem = Utilities.EmailSubjectLocalizedEnum.ESubject_LoadAccepted
                Case Utilities.EmailLocalizationTypesEnum.LoadChangesAccepted
                    bodyitem = Utilities.EmailBodyLocalizedEnum.EBody_LoadChangesAccepted
                    subjectitem = Utilities.EmailSubjectLocalizedEnum.ESubject_LoadChangesAccepted
                Case Utilities.EmailLocalizationTypesEnum.LoadRejected
                    bodyitem = Utilities.EmailBodyLocalizedEnum.EBody_LoadRejected
                    subjectitem = Utilities.EmailSubjectLocalizedEnum.ESubject_LoadRejected
                Case Utilities.EmailLocalizationTypesEnum.LoadManuallyRejected
                    bodyitem = Utilities.EmailBodyLocalizedEnum.EBody_LoadManuallyRejected
                    subjectitem = Utilities.EmailSubjectLocalizedEnum.ESubject_LoadRejected
                Case Utilities.EmailLocalizationTypesEnum.LoadChangesRejected
                    bodyitem = Utilities.EmailBodyLocalizedEnum.EBody_LoadChangesRejected
                    subjectitem = Utilities.EmailSubjectLocalizedEnum.ESubject_LoadChangesRejected
                Case Utilities.EmailLocalizationTypesEnum.LoadExpired
                    bodyitem = Utilities.EmailBodyLocalizedEnum.EBody_LoadExpired
                    subjectitem = Utilities.EmailSubjectLocalizedEnum.ESubject_LoadExpired
                Case Utilities.EmailLocalizationTypesEnum.LoadUnfinalized
                    bodyitem = Utilities.EmailBodyLocalizedEnum.EBody_LoadUnfinalized
                    subjectitem = Utilities.EmailSubjectLocalizedEnum.ESubject_LoadUnfinalized
                Case Utilities.EmailLocalizationTypesEnum.LoadAutoTendered
                    bodyitem = Utilities.EmailBodyLocalizedEnum.EBody_LoadAutoTendered
                    subjectitem = Utilities.EmailSubjectLocalizedEnum.ESubject_LoadTendered
                Case Utilities.EmailLocalizationTypesEnum.LoadManuallyTendered
                    bodyitem = Utilities.EmailBodyLocalizedEnum.EBody_LoadManuallyTendered
                    subjectitem = Utilities.EmailSubjectLocalizedEnum.ESubject_LoadTendered
                Case Utilities.EmailLocalizationTypesEnum.LoadDropped
                    bodyitem = Utilities.EmailBodyLocalizedEnum.EBody_LoadDropped
                    subjectitem = Utilities.EmailSubjectLocalizedEnum.ESubject_LoadDropped
                Case Utilities.EmailLocalizationTypesEnum.LoadManuallyDropped
                    bodyitem = Utilities.EmailBodyLocalizedEnum.EBody_LoadManuallyDropped
                    subjectitem = Utilities.EmailSubjectLocalizedEnum.ESubject_LoadRejected
                Case Utilities.EmailLocalizationTypesEnum.LoadUnassigned
                    bodyitem = Utilities.EmailBodyLocalizedEnum.EBody_LoadManuallyUnassigned
                    subjectitem = Utilities.EmailSubjectLocalizedEnum.ESubject_LoadUnassigned
                Case Utilities.EmailLocalizationTypesEnum.LoadModifyUnaccepted
                    bodyitem = Utilities.EmailBodyLocalizedEnum.EBody_LoadModifyUnaccepted
                    subjectitem = Utilities.EmailSubjectLocalizedEnum.ESubject_LoadModifyUnaccepted
                Case Else
                    Return
            End Select

            Me.BodyLocalized = Utilities.getEmailBodyLocalizedString(bodyitem)
            Me.BodyFormat = Utilities.getEmailBodyNotLocalizedString(bodyitem)
            Me.SubjectLocalized = Utilities.getEmailSubjectLocalizedString(subjectitem)
            Me.SubjectFormat = Utilities.getEmailSubjectNotLocalizedString(subjectitem)

        End Sub

        Public Sub fillBodyKeysFromList()
            If Not Me.BodyKeyList Is Nothing AndAlso Me.BodyKeyList.Count > 0 Then
                Me.BodyKeys = String.Join(",", Me.BodyKeyList)
            End If
        End Sub

        Public Sub fillSubjectKeysFromList()
            If Not Me.SubjectKeyList Is Nothing AndAlso Me.SubjectKeyList.Count > 0 Then
                Me.SubjectKeys = String.Join(",", Me.SubjectKeyList)
            End If
        End Sub

        Public Sub fillKeysFromLists()
            fillBodyKeysFromList()
            fillSubjectKeysFromList()
        End Sub

        Public Function getFormattedSubject() As String
            Try
                If Not SubjectKeyList Is Nothing AndAlso SubjectKeyList.Count > 0 Then
                    Return String.Format(SubjectFormat, SubjectKeyList.ToArray())
                Else
                    Return SubjectFormat
                End If

            Catch ex As FormatException
                Return SubjectFormat
            End Try

        End Function

        Public Function getFormattedBody() As String
            Try
                If Not BodyKeyList Is Nothing AndAlso BodyKeyList.Count > 0 Then
                    Return String.Format(BodyFormat, BodyKeyList.ToArray())
                Else
                    Return BodyFormat
                End If
            Catch ex As FormatException
                Return BodyFormat
            End Try
        End Function

        Public Function getFormattedBodyNoHTML() As String
            Dim formatedStr As String = BodyFormat
            If Not String.IsNullOrEmpty(BodyFormat) Then
                formatedStr = formatedStr.Replace("<br />", "")
            End If
            Try
                If Not BodyKeyList Is Nothing AndAlso BodyKeyList.Count > 0 Then
                    Return String.Format(formatedStr, BodyKeyList.ToArray())
                Else
                    Return formatedStr
                End If
            Catch ex As FormatException
                Return formatedStr
            End Try
        End Function

#End Region

    End Class
End Namespace