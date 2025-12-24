using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BingMapsRESTToolkit;

namespace NGL.FM.Bing
{
    public class BingLocationAPI
    {

        static public GeoCoordinate GeoCodeAddress(string address1, string city, string state, string zip, string country, string key)
        {
            GeoCoordinate geoCoordinate = new GeoCoordinate();
            //Create a request
            var request = new GeocodeRequest()
            {
                //Query = "New York, NY",
                Address = new SimpleAddress() { AddressLine = address1, Locality = city, AdminDistrict = state, PostalCode = zip, CountryRegion = country },
                IncludeIso2 = true,
                IncludeNeighborhood = true,
                MaxResults = 1,
                BingMapsKey = key
            };
            //Process the request 
            var response = request.Execute().GetAwaiter().GetResult();
            if (response != null && response.ResourceSets?.Length > 0 && response.ResourceSets[0].Resources?.Length > 0)
            {
                Resource[] resources = response.ResourceSets[0].Resources;
                Location location;
                //Have to do it this way because I couldn't figure out how to do a lambda with ambiguous types
                List<Location> locations = new List<Location>();
                foreach (var resource in response.ResourceSets[0].Resources)
                {
                    locations.Add(resource as Location);
                }
                /*
                * ConfidenceLevelType   
                *  0 = None   - No confidence level set.     
                *  1 = High   - High confidence match   
                *  2 = Medium - Medium confidence match
                *  3 = Low    - Low confidence match
                * We want to return the location with the highest confidence level
                * If there are no records with ConfidenceLevelType.None (0) Then all we have to do is order by ConfidenceLevelType asc and pick the first record
                * If there is at least 1 record with ConfidenceLevelType.None AndAlso if at least 1 record exists with a higher ConfidenceLevelType Then remove all ConfidenceLevelType.None records, order by ConfidenceLevelType asc and pick the first record
                * If all records have ConfidenceLevelType.None then doesn't matter get the first one
                */
                if (!locations.Any(x => x.ConfidenceLevelType == ConfidenceLevelType.None)) { location = locations.OrderBy(x => x.ConfidenceLevelType).FirstOrDefault(); }
                else if (locations.Any(x => x.ConfidenceLevelType != ConfidenceLevelType.None)) { location = locations.Where(x => x.ConfidenceLevelType != ConfidenceLevelType.None).OrderBy(x => x.ConfidenceLevelType).FirstOrDefault(); }
                else { location = locations.FirstOrDefault(); }
                //Get the coordinates aka lat/long from the location
                var crd = location?.Point?.GetCoordinate();
                geoCoordinate.Latitude = crd.Latitude;
                geoCoordinate.Longitude = crd.Longitude;
            }
            return geoCoordinate;
        }


        ////static public GeoCoordinate GeoCodeAddress(string address1, string city, string state, string zip, string country, string key)
        ////{
        ////    GeoCoordinate geoCoordinate = new GeoCoordinate();
        ////    //Create a request
        ////    var request = new GeocodeRequest()
        ////    {
        ////        //Query = "New York, NY",
        ////        Address = new SimpleAddress() { AddressLine = address1, Locality = city, AdminDistrict = state, PostalCode = zip, CountryRegion = country },
        ////        IncludeIso2 = true,
        ////        IncludeNeighborhood = true,
        ////        MaxResults = 1,
        ////        BingMapsKey = key
        ////    };
        ////    //Process the request 
        ////    var response = ServiceManager.GetResponseAsync(request).GetAwaiter().GetResult();
        ////    if (response != null && response.ResourceSets?.Length > 0 && response.ResourceSets[0].Resources?.Length > 0)
        ////    {
        ////        Resource[] resources = response.ResourceSets[0].Resources;
        ////        Location location;
        ////        //Have to do it this way because I couldn't figure out how to do a lambda with ambiguous types
        ////        List<Location> locations = new List<Location>();
        ////        foreach (var resource in response.ResourceSets[0].Resources)
        ////        {
        ////            locations.Add(resource as Location);
        ////        }
        ////        /*
        ////        * ConfidenceLevelType   
        ////        *  0 = None   - No confidence level set.     
        ////        *  1 = High   - High confidence match   
        ////        *  2 = Medium - Medium confidence match
        ////        *  3 = Low    - Low confidence match
        ////        * We want to return the location with the highest confidence level
        ////        * If there are no records with ConfidenceLevelType.None (0) Then all we have to do is order by ConfidenceLevelType asc and pick the first record
        ////        * If there is at least 1 record with ConfidenceLevelType.None AndAlso if at least 1 record exists with a higher ConfidenceLevelType Then remove all ConfidenceLevelType.None records, order by ConfidenceLevelType asc and pick the first record
        ////        * If all records have ConfidenceLevelType.None then doesn't matter get the first one
        ////        */
        ////        if (!locations.Any(x => x.ConfidenceLevelType == ConfidenceLevelType.None)) { location = locations.OrderBy(x => x.ConfidenceLevelType).FirstOrDefault(); }
        ////        else if (locations.Any(x => x.ConfidenceLevelType != ConfidenceLevelType.None)) { location = locations.Where(x => x.ConfidenceLevelType != ConfidenceLevelType.None).OrderBy(x => x.ConfidenceLevelType).FirstOrDefault(); }
        ////        else { location = locations.FirstOrDefault(); }
        ////        //Get the coordinates aka lat/long from the location
        ////        var crd = location?.Point?.GetCoordinate();
        ////        geoCoordinate.Latitude = crd.Latitude;
        ////        geoCoordinate.Longitude = crd.Longitude;
        ////    }
        ////    return geoCoordinate;
        ////}

    }
}
