using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RestSharp;
using RestSharp.Authenticators;

namespace NGL.FM.NGLRestIntegrations
{
    public class PageFooterMsg
    {

        ////using Microsoft.AspNetCore.Mvc;
        ////using System.Collections.Generic;
        ////namespace RESTAPIDemo.Controllers
        ////    {
        ////        [Route("api/[controller]")]
        ////        [ApiController]
        ////        public class DefaultController : ControllerBase
        ////        {
        ////            private readonly Dictionary<int, string> authors = new Dictionary<int, string>();
        ////            public DefaultController()
        ////            {
        ////                authors.Add(1, "Joydip Kanjilal");
        ////                authors.Add(2, "Steve Smith");
        ////                authors.Add(3, "Michele Smith");
        ////            }
        ////            [HttpGet]
        ////            public List<string> Get()
        ////            {
        ////                List<string> lstAuthors = new List<string>();
        ////                foreach (KeyValuePair<int, string> keyValuePair in authors)
        ////                    lstAuthors.Add(keyValuePair.Value);
        ////                return lstAuthors;
        ////            }
        ////            [HttpGet("{id}", Name = "Get")]
        ////            public string Get(int id)
        ////            {
        ////                return authors[id];
        ////            }
        ////            [HttpPost]
        ////            public void Post([FromBody] string value)
        ////            {
        ////                authors.Add(4, value);
        ////            }
        ////            [HttpPut("{id}")]
        ////            public void Put(int id, [FromBody] string value)
        ////            {
        ////                authors[id] = value;
        ////            }
        ////            [HttpDelete("{id}")]
        ////            public void Delete(int id)
        ////            {
        ////                authors.Remove(id);
        ////            }
        ////        }
        ////    }

        public string CallPgFooterREST(string url, int PageControl)
        {
            string response = "";
            
            //First, create an instance of RestClient.
            //The following code snippet shows how you can instantiate and initialize the RestClient class. 
            //Note that we're passing the base URL to the constructor 
            //RestClient client = new RestClient("http://localhost:58179/api/");
            //RestClient client = new RestClient(url);
            RestClient restClient = new RestClient();
            restClient.BaseUrl = new Uri(url);

            //Next, you should create an instance of the RestRequest class by passing the resource name and the method to be used.
            //The following code snippet shows how this can be achieved.
            //RestRequest request = new RestRequest("Default", Method.GET); //"Default" refers to the controller name ie DefaultController
            //var request = new RestRequest("PageFooterMsg", Method.GET).AddParameter("id", PageControl);
            //RestRequest request = new RestRequest("NGLSysPage", Method.GET);
            var restRequest = new RestRequest(Method.GET).AddParameter("id", PageControl);

            //Lastly, you need to execute the request, deserialize the response, and assign it to an object as appropriate 
            //as shown in the code snippet given below.
            //IRestResponse<List<string>> response = client.Execute<List<string>>(request);

            //IRestResponse restResponse = client.Execute(request);
            //response = client.Execute(request).Content;

            //token = NGLSystem
            //UserControl = 99999999
            restClient.Authenticator = new HttpBasicAuthenticator("99999999", "NGLSystem");
            
            IRestResponse restResponse = restClient.Execute(restRequest);
            //Console.WriteLine("Status code: " + (int)restResponse.StatusCode);
            //Console.WriteLine("Status message " + restResponse.Content);
           
            //response = restResponse.Content;
            //string s = restResponse.Content;
            //response = s.Replace("\"", "");


            //If there is a network transport error(network is down, failed DNS lookup, etc), RestResponse.ResponseStatus will be set to ResponseStatus.Error, 
            //otherwise it will be ResponseStatus.Completed.

            //If an API returns a 404, ResponseStatus will still be Completed.
            //If you need access to the HTTP status code returned you will find it at RestResponse.StatusCode.
            //The Status property is an indicator of completion independent of the API error handling.

            //Normally, RestSharp doesn't throw an exception if the request fails.

            //However, it is possible to configure RestSharp to throw in different situations, when it normally doesn't throw in favour 
            //of giving you the error as a property.
            
            //restResponse.ErrorException //exceptions thrown during the request, if any
            //restResponse.ErrorMessage //Transport or other non HTTP errors generated while attempting request
            //restResponse.ResponseStatus //Status of the request. Will return Error for transport errors. HTTP errors will still return ResponseStatus.Completed, check StatusCode instead
            //restResponse.StatusCode //HTTP response status code
            //restResponse.StatusDescription //Description of HTTP status returned

            RestSharp.Deserializers.JsonDeserializer deserializer = new RestSharp.Deserializers.JsonDeserializer();
            var strContent = deserializer.Deserialize<string>(restResponse);

            if (string.IsNullOrWhiteSpace(strContent))
            {
                //check to see if there was an error with the REST service
                if (restResponse.ResponseStatus != ResponseStatus.Completed && !string.IsNullOrWhiteSpace(restResponse.ErrorMessage)) {
                    response = "!Error! " + restResponse.ErrorMessage;
                }
                else
                {
                    if(restResponse.StatusCode != System.Net.HttpStatusCode.OK && !string.IsNullOrWhiteSpace(restResponse.StatusDescription)){
                        response = "!Error! " + restResponse.StatusDescription;
                    }
                }
            } else { response = strContent; }
            
            return response;
        }

        //To make a POST request using RestSharp, you can use the following code:
        //RestRequest request = new RestRequest("Default", Method.POST);
        //request.AddJsonBody("Robert Michael");
        //var response = client.Execute(request);

        //public void sdfsdf(string url, int PageControl)
        //{
        //    string strURL = url + "/" + PageControl.ToString();
        //    RestClient client = new RestClient(strURL);
        //    //RestClient client = new RestClient("http://api.zippopotam.us");
        //    RestRequest request = new RestRequest("nl/3825", Method.GET);
        //    IRestResponse response = client.Execute(request);
        //}

    }
}

