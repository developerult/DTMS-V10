using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BingMapsRESTToolkit;

namespace NGL.FM.Bing
{
    public class Class1
    {
        static private Resource[] GetResourcesFromRequest(BaseRestRequest rest_request)
        {
            var r = ServiceManager.GetResponseAsync(rest_request).GetAwaiter().GetResult();

            if (!(r != null && r.ResourceSets != null &&
                r.ResourceSets.Length > 0 &&
                r.ResourceSets[0].Resources != null &&
                r.ResourceSets[0].Resources.Length > 0))

                throw new Exception("No results found.");

            return r.ResourceSets[0].Resources;
        }

        static public void GeoCodeTest()
        {
            Console.WriteLine("Running Geocode Test");
            var request = new GeocodeRequest()
            {
                BingMapsKey = "AgzuAfKuHRODnun1D37-o-ohs3sTXkPZTM-OiPK5ze_hJy4d1AKMo8G5s7hK7mjF",
                //Query = "Seattle"
                Query = "New York, NY",
                //Query = "Chicago, IL, 60601, US",
                IncludeIso2 = true,
                IncludeNeighborhood = true,
                MaxResults = 1
            };

            var resources = GetResourcesFromRequest(request);

            foreach (var resource in resources)
            {
                System.Diagnostics.Debug.WriteLine("~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~");
                var location = resource as Location;
                ////Do something with the result.
                //var coords = result.Point.Coordinates;
                //if (coords != null && coords.Length == 2)
                //{
                //    var lat = coords[0];
                //    var lng = coords[1];
                //    System.Diagnostics.Debug.WriteLine($"Geocode Results - Lat: {lat} / Long: {lng}");
                //}


                if (location.GeocodePoints != null)
                {
                    foreach (var p in location.GeocodePoints)
                    {
                        var gp = p.GetCoordinate();
                        var gpLat = gp.Latitude;
                        var gpLong = gp.Longitude;

                        var gpUsageTypes = p.UsageTypes;
                        string strUseGP = "";
                        string sSepGP = "";
                        if (gpUsageTypes != null)
                        {
                            foreach (var u in gpUsageTypes)
                            {
                                strUseGP += (sSepGP + u);
                                sSepGP = ", ";
                            }
                            System.Diagnostics.Debug.WriteLine($"Geocode Points - Name: {location.Name} Lat: {gpLat} / Long: {gpLong} UsageTypes: {strUseGP} ");
                        }
                    }
                }

                System.Diagnostics.Debug.WriteLine("*************************************");

                var usageTypes = location.Point.UsageTypes;
                string strUse = "";
                string sSep = "";
                if (usageTypes != null)
                {
                    foreach (var u in usageTypes)
                    {
                        strUse += (sSep + u);
                        sSep = ", ";
                    }
                }
                var s = location.Point.GetCoordinate();
                var l = s.Latitude;
                var lg = s.Longitude;

                var coords = location.Point.Coordinates;
                if (coords != null && coords.Length == 2)
                {
                    var lat = coords[0];
                    var lng = coords[1];
                    System.Diagnostics.Debug.WriteLine($"Location Point - Name: {location.Name} Lat: {lat} / Long: {lng} UsageTypes: {strUse} Confidence: {location.Confidence}");

                }
                System.Diagnostics.Debug.WriteLine("~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~");
            }

            //Console.ReadLine();
        }

        static public async void GeocodeTest2()
        {
            //Create a request.
            var request = new GeocodeRequest()
            {
                Address = new SimpleAddress(){ AddressLine ="street", Locality="city", AdminDistrict="state", PostalCode="zip", CountryRegion = "US" },
                Query = "New York, NY",
                IncludeIso2 = true,
                IncludeNeighborhood = true,
                MaxResults = 1,
                BingMapsKey = "YOUR_BING_MAPS_KEY"
            };

      
            //Process the request by using the ServiceManager.
            var response = await request.Execute();

            if (response != null &&
                response.ResourceSets != null &&
                response.ResourceSets.Length > 0 &&
                response.ResourceSets[0].Resources != null &&
                response.ResourceSets[0].Resources.Length > 0)
            {
                Resource[] resources = response.ResourceSets[0].Resources;
                foreach (var resource in resources)
                {
                    var result = resource as Location;

                    var usageTypes = result.Point.UsageTypes;
                    string strUse = "";
                    string sSep = "";
                    foreach (var u in usageTypes)
                    {
                        strUse += (sSep + u);
                        sSep = ", ";
                    }
                    
                        var s = result.Point.GetCoordinate();
                    var l = s.Latitude;
                    var lg = s.Longitude;

                    var coords = result.Point.Coordinates;
                    if (coords != null && coords.Length == 2)
                    {
                        var lat = coords[0];
                        var lng = coords[1];
                        System.Diagnostics.Debug.WriteLine($"Geocode Results - Lat: {lat} / Long: {lng} UsageTypes: {strUse} Confidence: {result.Confidence}");
                        
                    }
                }                  
            }
        }

    }
}
