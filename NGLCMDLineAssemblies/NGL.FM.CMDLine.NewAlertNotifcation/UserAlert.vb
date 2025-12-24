Imports DTO = NGL.FreightMaster.Data.DataTransferObjects

Public Class UserAlert

    Private _User As DTO.tblUserSecurity
    Public Property User As DTO.tblUserSecurity
        Get
            Return _User
        End Get
        Set(value As DTO.tblUserSecurity)
            _User = value
        End Set
    End Property

    Private _Messages As List(Of DTO.tblAlertMessage)
    Public Property Messages As List(Of DTO.tblAlertMessage)
        Get
            Return _Messages
        End Get
        Set(value As List(Of DTO.tblAlertMessage))
            _Messages = value
        End Set
    End Property

End Class
