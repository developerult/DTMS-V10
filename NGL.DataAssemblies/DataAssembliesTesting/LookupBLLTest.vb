Imports System.Text
Imports Microsoft.VisualStudio.TestTools.UnitTesting
Imports System
Imports Ngl.FM.CarTar
Imports DAL = Ngl.FreightMaster.Data
Imports DTO = NGL.FreightMaster.Data.DataTransferObjects
Imports BLL = NGL.FM.BLL

<TestClass()> Public Class LookupBLLTest
    Inherits TestBase
      
    <TestMethod> _
    Public Sub GetCityStateZipLookup_BothPCM_And_TariffShippersTest()
        'this should return veggie juice warehouses and pcmiler geolocations without warehouse control numbers.
        Dim zipcode As Integer = 60638 ' veggie juice zip code.
        Dim getList As Integer = 0
        Dim veggiejuiceCompControl = 9623
        Dim lookupbll As New Ngl.FM.BLL.NGLLookupBLL(testParameters)
        Dim result As DTO.CityStateZipTariffLookup() = lookupbll.GetCityStateZipLookup(2, 0, zipcode.ToString, getList)
        If result Is Nothing Then
            Assert.Fail("GetCityStateZipLookup no results")
        End If
        If result.Length = 0 Then
            Assert.Fail("GetCityStateZipLookup zero results")
        End If
        Dim veggiewarehousefound As Boolean = False
        Dim geolocationfound As Boolean = False
        For Each item As DTO.CityStateZipTariffLookup In result
            If item.CompControl = veggiejuiceCompControl Then
                veggiewarehousefound = True
            End If
            If item.CompControl = 0 And item.CompStreetZip = zipcode.ToString Then
                geolocationfound = True
            End If
        Next
        If veggiewarehousefound = False Then
            Assert.Fail("GetCityStateZipLookup veggie juice not found")
        End If
        If geolocationfound = False Then
            Assert.Fail("GetCityStateZipLookup geolocation not found")
        End If
    End Sub

    <TestMethod> _
    Public Sub GetCityStateZipLookup_TariffShippers_OnlyTest()
        'this should return veggie juice warehouses and pcmiler geolocations without warehouse control numbers.
        Dim zipcode As Integer = 60638 ' veggie juice zip code.
        Dim getList As Integer = 1
        Dim veggiejuiceCompControl = 9623
        Dim lookupbll As New Ngl.FM.BLL.NGLLookupBLL(testParameters)
        Dim result As DTO.CityStateZipTariffLookup() = lookupbll.GetCityStateZipLookup(2, 0, zipcode.ToString, getList)
        If result Is Nothing Then
            Assert.Fail("GetCityStateZipLookup no results")
        End If
        If result.Length = 0 Then
            Assert.Fail("GetCityStateZipLookup zero results")
        End If
        Dim veggiewarehousefound As Boolean = False

        For Each item As DTO.CityStateZipTariffLookup In result
            If item.CompControl = veggiejuiceCompControl Then
                veggiewarehousefound = True
            End If

        Next
        If veggiewarehousefound = False Then
            Assert.Fail("GetCityStateZipLookup veggie juice not found")
        End If

    End Sub

    <TestMethod> _
    Public Sub GetTariffShippersCityStateZipTariffLookupTest()
        testParameters.DBServer = "NGLRDP06D"
        testParameters.Database = "NGLMAS2013dev"
        testParameters.UserName = "ngl\paul molenda"
        testParameters.SSOAName = "NGL"
        testParameters.WCFAuthCode = "NGLWCFDEV"
        testParameters.UseToken = True
        testParameters.UseExceptionEvents = True
        testParameters.ValidateAccess = True
        testParameters.USATToken = "9598bbee-8d07-4c5d-af1a-bf7dc34e6e9e"


        'testParameters.DBServer = "NGLRDP06D"
        'testParameters.Database = "NGLMASPROD"
        'testParameters.UserName = "bongards\tms"
        'testParameters.SSOAName = "NGL"
        'testParameters.WCFAuthCode = "NGLWCFDEV"
        'testParameters.UseToken = True
        'testParameters.UseExceptionEvents = True
        'testParameters.ValidateAccess = True
        'testParameters.USATToken = "" '"9598bbee-8d07-4c5d-af1a-bf7dc34e6e9e"

        'this should return veggie juice warehouses and pcmiler geolocations without warehouse control numbers.
        Dim compNumber As String = "31" 'bongards norwood
        Dim getList As Integer = 2
        Dim lookupbll As New NGL.FM.BLL.NGLLookupBLL(testParameters)
        Dim result As DTO.CityStateZipTariffLookup() = lookupbll.GetTariffShippersCityStateZipTariffLookup(2, 0, compNumber)
        If result Is Nothing Then
            Assert.Fail("GetCityStateZipLookup no results")
        End If
        If result.Length = 0 Then
            Assert.Fail("GetCityStateZipLookup zero results")
        End If
        Dim warehousefound As Boolean = False

        For Each item As DTO.CityStateZipTariffLookup In result
            If item.CompControl = 1 Then
                warehousefound = True
            End If

        Next
        If warehousefound = False Then
            Assert.Fail("GetTariffShippersCityStateZipTariffLookupTest not found")
        End If

    End Sub


    <TestMethod> _
    Public Sub GetCityStateZipLookup_BothPCM_OnlyTest()
        'this should return veggie juice warehouses and pcmiler geolocations without warehouse control numbers.
        Dim zipcode As Integer = 60638 ' veggie juice zip code.
        Dim getList As Integer = 0
        Dim lookupbll As New Ngl.FM.BLL.NGLLookupBLL(testParameters)
        Dim result As DTO.CityStateZipTariffLookup() = lookupbll.GetCityStateZipLookup(2, 0, zipcode.ToString, getList)
        If result Is Nothing Then
            Assert.Fail("GetCityStateZipLookup no results")
        End If
        If result.Length = 0 Then
            Assert.Fail("GetCityStateZipLookup zero results")
        End If
        Dim geolocationfound As Boolean = False
        For Each item As DTO.CityStateZipTariffLookup In result

            If item.CompControl = 0 And item.CompStreetZip = zipcode.ToString Then
                geolocationfound = True
            End If
        Next

        If geolocationfound = False Then
            Assert.Fail("GetCityStateZipLookup geolocation not found")
        End If
    End Sub

End Class