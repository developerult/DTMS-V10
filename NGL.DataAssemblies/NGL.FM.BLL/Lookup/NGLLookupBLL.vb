Imports Microsoft.VisualBasic
Imports System
Imports System.Collections.Generic
Imports System.Linq
Imports System.Data.Linq
Imports System.Data.SqlClient
Imports System.ServiceModel
Imports Serilog

Imports DAL = NGL.FreightMaster.Data
Imports DTO = NGL.FreightMaster.Data.DataTransferObjects
Imports LTS = NGL.FreightMaster.Data.LTS
Imports DTran = NGL.Core.Utility.DataTransformation
Imports TAR = NGL.FM.CarTar


Public Class NGLLookupBLL : Inherits BLLBaseClass


#Region " Constructors "

    Public Sub New(ByVal oParameters As DAL.WCFParameters)
        MyBase.New()
        Me.Parameters = oParameters
        Me.SourceClass = "NGLLookupBLL"
        Me.Logger = Me.Logger.ForContext(Of NGLLookupBLL)
    End Sub

#End Region

#Region " Properties "



#End Region


#Region "DAL Wrapper Methods"
 
#Region "NGLtblAccessorial Data"

    ''' <summary>
    ''' This is a wrapper for NGLtblAccessorialData.GetAvailableAccessorialFeesByCarrTarControl
    ''' and is used to get the Available Accessorial Fees for a contract.  
    ''' It removes fuel fees based on if the contract is setup as a percent or a per mile fuel.
    ''' </summary>
    ''' <param name="CarrTarControl"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function GetAvailableAccessorialFeesByCarrTarControl(ByVal CarrTarControl As Integer) As DTO.tblAccessorial()
        Dim retVals As New List(Of DTO.tblAccessorial)
        retVals = NGLtblAccessorialData.GetAvailableAccessorialFeesByCarrTarControl(CarrTarControl).ToList
        Dim tomrrow As Date = Date.Now.AddDays(1)
        'TODO: Room for optimization here. maybe a new method, because we dont need the fuel surcharge just the UseRatePerMile flag
        Dim fuel As DTO.FuelSurchargeResult = CarrierBLL.GetCarrierFuelSurchargeByContract(CarrTarControl, 0, "", tomrrow)
        If fuel Is Nothing OrElse fuel.CarrTarControl = 0 OrElse fuel.CarrierControl = 0 Then
            Return Nothing
        End If
        If fuel.UseRatePerMile.HasValue And fuel.UseRatePerMile.Value Then
            'remove the percent becasue the tariff uses rate permile, they should not be able to add percent
            retVals.RemoveAll(Function(i) i.AccessorialCode = 2) '2==percent
        Else
            'remove the permile becasue the tariff uses rate percent, they should not be able to add permile
            retVals.RemoveAll(Function(i) i.AccessorialCode = 9) '9==ratePerMile
        End If
        Return retVals.ToArray
    End Function

#End Region

 

 

#End Region

#Region " Public Methods"

    ''' <summary>
    ''' This is a wrapper to the carrierdataprovider GetTariffShippers methods if carriercontrol > 0
    ''' then we get the tariffs that have the same carriercontrol
    ''' </summary>
    ''' <param name="sortKey"></param>
    ''' <param name="carrierControl"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function GetTariffShippers(ByVal sortKey As Integer, _
                                      ByVal carrierControl As Integer) As DTO.vComp()
        Dim tariffShippers As DTO.vComp()
        If (carrierControl > 0) Then
            tariffShippers = CarrierBLL.GetTariffShippersByCarrier(sortKey, carrierControl)
        Else
            tariffShippers = CarrierBLL.GetTariffShippers(sortKey)
        End If
        Return tariffShippers
    End Function

    ''' <summary>
    ''' looks for companies that have tariffs, takes the first 20 results. 
    ''' The take amount can be changed using optional parameter take.
    ''' </summary>
    ''' <param name="sortKey"></param>
    ''' <param name="carrierControl"></param>
    ''' <param name="contains"></param>
    ''' <param name="take">optional</param>
    ''' <returns></returns>
    ''' <remarks>
    ''' Modified By LVV on 11/14/16 for v-7.0.5.110 Tariff Search > 5
    '''  Added Optional parameter for take default 20
    ''' </remarks>
    Public Function GetTariffShippersContains(ByVal sortKey As Integer, _
                                                ByVal carrierControl As Integer, _
                                                ByVal contains As String, _
                                                Optional ByVal take As Integer = 20) As DTO.vComp()
        Dim tariffShippers As DTO.vComp() = GetTariffShippers(sortKey, carrierControl)
        If Not String.IsNullOrEmpty(contains) Then
            Dim result = (From v In tariffShippers
                            Where v.CompName.ToUpper().Contains(contains.ToUpper()) Or
                                  v.CompNumber.ToString().Contains(contains.ToUpper()) Or
                                  v.CompStreetCity.ToUpper().Contains(contains.ToUpper()) Or
                                  v.CompStreetZip.ToUpper().Contains(contains.ToUpper()) Or
                                  v.CompStreetState.ToUpper().Contains(contains.ToUpper())
                            Select v).Take(take).ToArray()
            Return result
        Else
            Return tariffShippers
        End If
        Return tariffShippers
    End Function

    ''' <summary>
    ''' This is a wrapper method to convert the data to CityStateZipTariffLookup objects.
    ''' </summary>
    ''' <param name="sortKey"></param>
    ''' <param name="carrierControl"></param>
    ''' <param name="contains"></param>
    ''' <param name="take">optional</param>
    ''' <returns></returns>
    ''' <remarks>
    ''' Modified By LVV on 11/14/16 for v-7.0.5.110 Tariff Search > 5
    '''  Added Optional parameter for take
    ''' </remarks>
    Public Function GetTariffShippersCityStateZipTariffLookup(ByVal sortKey As Integer, _
                                                ByVal carrierControl As Integer, _
                                                ByVal contains As String, _
                                                Optional ByVal take As Integer = 20) As DTO.CityStateZipTariffLookup()
        Dim tariffShippers = (From ts In GetTariffShippersContains(sortKey, carrierControl, contains, take)
                                                               Select selectDTO(ts)).ToArray()
        Return tariffShippers
    End Function



    ''' <summary>
    ''' Get the list of the companies that have tariffs with the zip code passed in.
    ''' and a list of cities from PCMiler that have the same zip code.
    ''' Both lists are merged into one list sorted by city ascending.
    ''' </summary>
    ''' <param name="sortKey"></param>
    ''' <param name="carrierControl"></param>
    ''' <param name="postalCode"></param>
    ''' <param name="getLists">
    ''' 0 = get both lists tariffShippers and PCMiler list.
    ''' 1 = get just the tariffShippers list
    ''' 2 = get just the PCMiler list.
    ''' </param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function GetCityStateZipLookup(ByVal sortKey As Integer, _
                                          ByVal carrierControl As Integer, _
                                          ByVal postalCode As String,
                                          ByVal getLists As Integer,
                                          Optional ByVal Take As Integer = 20) As DTO.CityStateZipTariffLookup()
        Dim tariffShippers As IEnumerable(Of DTO.CityStateZipTariffLookup)
        Dim pcmResult As IEnumerable(Of DTO.PCMAddress)
        Dim returnDefaultResult(1) As DTO.CityStateZipTariffLookup
        Select Case getLists
            Case 0
                'get both
                tariffShippers = (From ts In GetTariffShippers(sortKey, carrierControl)
                                                                Where ts.CompStreetZip.ToUpper = postalCode.ToUpper
                                                               Select selectDTO(ts))
                pcmResult = PCMilerBLL.cityStateZipLookup(postalCode)
                If pcmResult Is Nothing Then Return returnDefaultResult
                Dim pcmList = (From pcms In pcmResult Select selectDTO(pcms))
                Return (From u In tariffShippers.Union(pcmList) Order By u.CompStreetCity).ToArray()
            Case 1
                'get just the tariffShippers list
                'tariffShippers = (From ts In GetTariffShippers(sortKey, carrierControl)
                '                                              Where ts.CompStreetZip.ToUpper = postalCode.ToUpper
                '                                             Select selectDTO(ts))
                'Return (From u In tariffShippers Order By u.CompStreetCity).ToArray()
                Return NGLCarrTarContractData.GetTariffShippersByCarrierAndZip(sortKey, carrierControl, postalCode, Take)
            Case 2
                'modified by  v-7.0.5.0 02/26/2016 added error handlers to back end so the front end is more responsive
                'we can call GetCityStateZipLookup on every key press now.  the back end still limits the pcmiler lookup
                'to 3 characters but the warehouse returns a list with the first character match.
                If Not String.IsNullOrWhiteSpace(postalCode) AndAlso postalCode.Length() > 2 Then
                    ' get just the PCMiler list.
                    pcmResult = PCMilerBLL.cityStateZipLookup(postalCode.ToUpper)
                    If pcmResult Is Nothing Then Return returnDefaultResult
                    Return (From pcms In pcmResult Order By pcms.strCity Select selectDTO(pcms)).ToArray()
                Else
                    Return Nothing
                End If

            Case Else
                'get both
                tariffShippers = (From ts In GetTariffShippers(sortKey, carrierControl)
                                                                Where ts.CompStreetZip.ToUpper = postalCode.ToUpper
                                                               Select selectDTO(ts))
                pcmResult = PCMilerBLL.cityStateZipLookup(postalCode)
                If pcmResult Is Nothing Then Return returnDefaultResult
                Dim pcmList = (From pcms In pcmResult Select selectDTO(pcms))
                Return (From u In tariffShippers.Union(pcmList) Order By u.CompStreetCity).ToArray()
        End Select

    End Function



#End Region

#Region "Private Methods"

    Private Function selectDTO(ByVal vcomp As DTO.vComp) As DTO.CityStateZipTariffLookup
        Dim c As New DTO.CityStateZipTariffLookup
        c.CompAbrev = vcomp.CompAbrev
        c.CompControl = vcomp.CompControl
        c.CompName = vcomp.CompName
        c.CompNumber = vcomp.CompNumber
        c.CompStreetAddress1 = vcomp.CompStreetAddress1
        c.CompStreetAddress2 = vcomp.CompStreetAddress2
        c.CompStreetAddress2 = vcomp.CompStreetAddress3
        c.CompStreetCity = vcomp.CompStreetCity
        c.CompStreetState = vcomp.CompStreetState
        c.CompStreetZip = vcomp.CompStreetZip
        c.CompStreetCountry = vcomp.CompStreetCountry
        If Not String.IsNullOrEmpty(c.CompStreetCity) And _
           Not String.IsNullOrEmpty(c.CompStreetState) And _
           Not String.IsNullOrEmpty(c.CompStreetZip) Then
            c.FormatCityStateZip = c.CompStreetCity & ", " & c.CompStreetState & " " & c.CompStreetZip
        End If
        Return c
    End Function

    Private Function selectDTO(ByVal address As DTO.PCMAddress) As DTO.CityStateZipTariffLookup
        If address Is Nothing Then Return Nothing
        Dim c As New DTO.CityStateZipTariffLookup
        c.CompStreetAddress1 = address.strAddress
        c.CompStreetCity = address.strCity
        c.CompStreetState = address.strState
        c.CompStreetZip = address.strZip
        If Not String.IsNullOrEmpty(c.CompStreetCity) And _
            Not String.IsNullOrEmpty(c.CompStreetState) And _
            Not String.IsNullOrEmpty(c.CompStreetZip) Then
            c.FormatCityStateZip = c.CompStreetCity & ", " & c.CompStreetState & " " & c.CompStreetZip
        End If
        'where is the country?
        Return c
    End Function
#End Region

End Class
