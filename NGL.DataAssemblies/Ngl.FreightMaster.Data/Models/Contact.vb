Imports Map = Ngl.API.Mapping
Imports DTran = Ngl.Core.Utility.DataTransformation
Imports DTO = Ngl.FreightMaster.Data.DataTransferObjects

Namespace Models
    'Added By LVV On 8/11/17 For v-8.0 TMS365
    Public Class Contact

        Private _ContactControl As Integer
        Private _ContactName As String
        Private _ContactTitle As String
        Private _ContactPhone As String
        Private _ContactPhoneExt As String
        Private _ContactFax As String
        Private _Contact800 As String
        Private _ContactEmail As String
        Private _ContactDefault As Boolean
        Private _ContactTender As Boolean
        Private _ContactScheduler As Boolean
        Private _ContactCarrierControl As Integer
        Private _ContactLECarControl As Integer
        Private _ContactCompControl As Integer

        Public Property ContactControl() As Integer
            Get
                Return _ContactControl
            End Get
            Set(ByVal value As Integer)
                _ContactControl = value
            End Set
        End Property

        Public Property ContactName() As String
            Get
                Return _ContactName
            End Get
            Set(ByVal value As String)
                _ContactName = value
            End Set
        End Property

        Public Property ContactTitle() As String
            Get
                Return _ContactTitle
            End Get
            Set(ByVal value As String)
                _ContactTitle = value
            End Set
        End Property

        Public Property ContactPhone() As String
            Get
                Return _ContactPhone
            End Get
            Set(ByVal value As String)
                _ContactPhone = value
            End Set
        End Property

        Public Property ContactPhoneExt() As String
            Get
                Return _ContactPhoneExt
            End Get
            Set(ByVal value As String)
                _ContactPhoneExt = value
            End Set
        End Property

        Public Property ContactFax() As String
            Get
                Return _ContactFax
            End Get
            Set(ByVal value As String)
                _ContactFax = value
            End Set
        End Property

        Public Property Contact800() As String
            Get
                Return _Contact800
            End Get
            Set(ByVal value As String)
                _Contact800 = value
            End Set
        End Property

        Public Property ContactEmail() As String
            Get
                Return _ContactEmail
            End Get
            Set(ByVal value As String)
                _ContactEmail = value
            End Set
        End Property

        Public Property ContactDefault() As Boolean
            Get
                Return _ContactDefault
            End Get
            Set(ByVal value As Boolean)
                _ContactDefault = value
            End Set
        End Property

        Public Property ContactTender() As Boolean
            Get
                Return _ContactTender
            End Get
            Set(ByVal value As Boolean)
                _ContactTender = value
            End Set
        End Property

        Public Property ContactScheduler() As Boolean
            Get
                Return _ContactScheduler
            End Get
            Set(ByVal value As Boolean)
                _ContactScheduler = value
            End Set
        End Property

        Public Property ContactCarrierControl() As Integer
            Get
                Return _ContactCarrierControl
            End Get
            Set(ByVal value As Integer)
                _ContactCarrierControl = value
            End Set
        End Property

        Public Property ContactLECarControl() As Integer
            Get
                Return _ContactLECarControl
            End Get
            Set(ByVal value As Integer)
                _ContactLECarControl = value
            End Set
        End Property

        Public Property ContactCompControl() As Integer
            Get
                Return _ContactCompControl
            End Get
            Set(ByVal value As Integer)
                _ContactCompControl = value
            End Set
        End Property

        Public Function MapNGLAPIContact()
            Return New Map.Contact With {
                .ContactControl = ContactControl,
                .ContactName = ContactName,
                .ContactTitle = ContactTitle,
                .ContactPhone = ContactPhone,
                .ContactPhoneExt = ContactPhoneExt,
                .ContactFax = ContactFax,
                .Contact800 = Contact800,
                .ContactEmail = ContactEmail,
                .ContactDefault = ContactDefault,
                .ContactTender = ContactTender,
                .ContactScheduler = ContactScheduler,
                .ContactCarrierControl = ContactCarrierControl,
                .ContactLECarControl = ContactLECarControl,
                .ContactCompControl = ContactCompControl
            }
        End Function

        'add a function MapAPINGLContact to map the API Map.Contact to NGL Model.Contact    
        Public Shared Function MapAPINGLContact(ByVal contact As Map.Contact)
            Return New Contact With {
                .ContactControl = contact.ContactControl,
                .ContactName = contact.ContactName,
                .ContactTitle = contact.ContactTitle,
                .ContactPhone = contact.ContactPhone,
                .ContactPhoneExt = contact.ContactPhoneExt,
                .ContactFax = contact.ContactFax,
                .Contact800 = contact.Contact800,
                .ContactEmail = contact.ContactEmail,
                .ContactDefault = contact.ContactDefault,
                .ContactTender = contact.ContactTender,
                .ContactScheduler = contact.ContactScheduler,
                .ContactCarrierControl = contact.ContactCarrierControl,
                .ContactLECarControl = contact.ContactLECarControl,
                .ContactCompControl = contact.ContactCompControl
            }
        End Function



    End Class


End Namespace

