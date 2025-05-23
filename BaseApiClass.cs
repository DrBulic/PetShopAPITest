using Newtonsoft.Json;
using PetShopAPITest.Data;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;


namespace PetShopAPITest
{
    public class BaseApiClass
    {
        protected RestClient client;
        protected RestRequest request;
        protected IConfiguration config;
        protected string apiKey;
        protected string baseUrl;
    


        public PetInfo GetPetByID_ReturnBody(Int64 petID)
        {          
            request = new RestRequest(petID.ToString());
            RestResponse response = client.Execute(request);
            var responseBody = JsonConvert.DeserializeObject<PetInfo>(response.Content);
            return responseBody;
        }


        public RestResponse GetPetByID(Int64 petID)
        {                       
            request = new RestRequest(petID.ToString());
            RestResponse response = client.Execute(request);          
            return response;
        }




        public RestResponse AddNewPet(PetInfo pet)
        {
            string body = JsonConvert.SerializeObject(pet);            
            request = new RestRequest();
            request.Method = Method.Post;
            request.AddHeader("api_key", apiKey);
            request.AddBody(body);
            return client.Execute(request);

        }

        public RestResponse AddNewPetNoApiKeyHeader(PetInfo pet)
        {
            string body = JsonConvert.SerializeObject(pet);
            request = new RestRequest();
            request.Method = Method.Post;           
            request.AddBody(body);
            return client.Execute(request);

        }


        // Method which takes Json format of Pet data, used for easier payload manipulation in wrong input tests
        public RestResponse AddNewPetNegative(string rawJson)
        {
            request = new RestRequest();
            request.Method = Method.Post;
            request.AddHeader("api_key", apiKey);          
            request.AddParameter("application/json", rawJson, ParameterType.RequestBody);
            return client.Execute(request);
        }

    


        public RestResponse UpdatePet(PetInfo pet)
        {
            string body = JsonConvert.SerializeObject(pet);            
            request = new RestRequest();
            request.Method = Method.Put;
            request.AddHeader("api_key", apiKey);           
            request.AddBody(body);
            return client.Execute(request);


        }

        // Create user with basic data for testing putpose
        public PetInfo CreateBasicUser()
        {
            PetInfo newPet = new PetInfo();
            newPet.name = "Rex";
            newPet.id = 0;
            newPet.status = "available";          
            newPet.photoUrls = new List<string>() { "http://example.com/photo.jpg" };
            newPet.category = new PetInfo.Category() { id = 123, name = "dogs" };
            newPet.tags = new List<PetInfo.Tag>() { new PetInfo.Tag() { id = 456, name = "testTag" } };
            return newPet;

        }


    }
}
