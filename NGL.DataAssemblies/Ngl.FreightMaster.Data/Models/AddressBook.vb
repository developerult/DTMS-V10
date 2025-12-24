Imports Map = Ngl.API.Mapping
Imports DTran = Ngl.Core.Utility.DataTransformation
Imports DTO = Ngl.FreightMaster.Data.DataTransferObjects


Namespace Models
    'Added By LVV On 8/11/17 For v-8.0 TMS365
    ''' <summary>
    ''' 
    ''' </summary>
    ''' <remarks>
    ''' Modified by RHR for v-8.5.4.006 on 05/28/2024 added
    '''     Region, LoadItems, UnloadItems, RateRequestItems and Charges
    ''' </remarks>
    Public Class AddressBook

        Private _Name As String
        Private _Address1 As String
        Private _Address2 As String
        Private _Address3 As String
        Private _City As String
        Private _State As String
        Private _Country As String
        Private _Zip As String
        Private _LocationCode As String
        Private _Contact As Contact


        Public Property Name() As String
            Get
                Return _Name
            End Get
            Set(ByVal value As String)
                _Name = value
            End Set
        End Property

        Public Property Address1() As String
            Get
                Return _Address1
            End Get
            Set(ByVal value As String)
                _Address1 = value
            End Set
        End Property

        Public Property Address2() As String
            Get
                Return _Address2
            End Get
            Set(ByVal value As String)
                _Address2 = value
            End Set
        End Property

        Public Property Address3() As String
            Get
                Return _Address3
            End Get
            Set(ByVal value As String)
                _Address3 = value
            End Set
        End Property

        Public Property City() As String
            Get
                Return _City
            End Get
            Set(ByVal value As String)
                _City = value
            End Set
        End Property

        Public Property State() As String
            Get
                Return _State
            End Get
            Set(ByVal value As String)
                _State = value
            End Set
        End Property

        Public Property Country() As String
            Get
                Return _Country
            End Get
            Set(ByVal value As String)
                _Country = value
            End Set
        End Property

        Public Property Zip() As String
            Get
                Return _Zip
            End Get
            Set(ByVal value As String)
                _Zip = value
            End Set
        End Property

        Public Property LocationCode() As String
            Get
                Return _LocationCode
            End Get
            Set(ByVal value As String)
                _LocationCode = value
            End Set
        End Property

        Public Property Contact() As Contact
            Get
                Return _Contact
            End Get
            Set(ByVal value As Contact)
                _Contact = value
            End Set
        End Property

        ' Begin Modified by RHR for v-8.5.4.006 
        Private _Region As String

        Public Property Region() As String
            Get
                Return _Region
            End Get
            Set(ByVal Value As String)
                _Region = Value
            End Set
        End Property

        'Add property for LoadItems as boolean
        Private _LoadItems As Boolean
        Public Property LoadItems() As Boolean
            Get
                Return _LoadItems
            End Get
            Set(ByVal Value As Boolean)
                _LoadItems = Value
            End Set
        End Property

        'Add property for UnloadItems as boolean    
        Private _UnloadItems As Boolean
        Public Property UnloadItems() As Boolean
            Get
                Return _UnloadItems
            End Get
            Set(ByVal Value As Boolean)
                _UnloadItems = Value
            End Set
        End Property

        'Add property for lItems as list of RateRequestItem     
        Private _lItems As List(Of Models.RateRequestItem)
        Public Property lItems() As List(Of Models.RateRequestItem)
            Get
                Return _lItems
            End Get
            Set(ByVal Value As List(Of Models.RateRequestItem))
                _lItems = Value
            End Set
        End Property

        'Add property for lSpecialReqs as list of Map.RateRequest.SpecialRequirement enum   
        Private _lSpecialReqs As New List(Of Map.RateRequest.SpecialRequirement)
        Public Property lSpecialReqs() As List(Of Map.RateRequest.SpecialRequirement)
            Get
                Return _lSpecialReqs
            End Get
            Set(ByVal Value As List(Of Map.RateRequest.SpecialRequirement))
                _lSpecialReqs = Value
            End Set
        End Property


        'add property for lReferencesNumbers as list of Map.ReferenceNumber 
        Private _lReferencesNumbers As New List(Of Map.ReferenceNumber)
        Public Property lReferencesNumbers() As List(Of Map.ReferenceNumber)
            Get
                Return _lReferencesNumbers
            End Get
            Set(ByVal Value As List(Of Map.ReferenceNumber))
                _lReferencesNumbers = Value
            End Set
        End Property

        'add property for Charges as list of Map.Charge    
        Private _Charges As New List(Of Map.Charge)
        Public Property Charges() As List(Of Map.Charge)
            Get
                Return _Charges
            End Get
            Set(ByVal Value As List(Of Map.Charge))
                _Charges = Value
            End Set
        End Property

        'End Modified by RHR for v-8.5.4.006 

        Public Function MapNGLAPIAddressBook() As Map.AddressBook
            Return New Map.AddressBook With {
                .Name = Name,
                .Address1 = Address1,
                .Address2 = Address2,
                .Address3 = Address3,
                .City = City,
                .State = State,
                .Country = Country,
                .Zip = Zip,
                .LocationCode = LocationCode,
                .Contact = Contact.MapNGLAPIContact(),
                .Region = Region,
                .LoadItems = LoadItems,
                .UnloadItems = UnloadItems,
                .lItems = (From d In lItems Select d.MapNGLAPIRateRequestItem()).ToList(),
                .lSpecialReqs = lSpecialReqs,
                .lReferencesNumbers = lReferencesNumbers,
                .Charges = Charges
            }
        End Function

        'add a Function MapAPINGLAddressBook to save Map.AddressBook data to Model.AddressBook  
        Public Shared Function MapAPINGLAddressBook(ByVal addressBook As Map.AddressBook) As AddressBook
            Return New AddressBook With {
                .Name = addressBook.Name,
                .Address1 = addressBook.Address1,
                .Address2 = addressBook.Address2,
                .Address3 = addressBook.Address3,
                .City = addressBook.City,
                .State = addressBook.State,
                .Country = addressBook.Country,
                .Zip = addressBook.Zip,
                .LocationCode = addressBook.LocationCode,
                .Contact = Contact.MapAPINGLContact(addressBook.Contact),
                .Region = addressBook.Region,
                .LoadItems = addressBook.LoadItems,
                .UnloadItems = addressBook.UnloadItems,
                .lItems = (From d In addressBook.lItems Select RateRequestItem.MapAPINGLRateRequestItem(d)).ToList(),
                .lSpecialReqs = addressBook.lSpecialReqs,
                .lReferencesNumbers = addressBook.lReferencesNumbers,
                .Charges = addressBook.Charges
            }
        End Function
    End Class


End Namespace

