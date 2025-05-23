using FluentAssertions;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using PetShopAPITest.Data;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetShopAPITest
{
    public  class PetAPI_WrongInputTests : BaseApiClass
    {

        [SetUp]
        public void Setup()
        {
            config = new ConfigurationBuilder()
                 .SetBasePath(Directory.GetCurrentDirectory())
                 .AddJsonFile("appsettings.json")
                 .Build();

            apiKey = config["ApiSettings:ApiKey"];
            baseUrl = config["ApiSettings:BaseUrl"];

            client = new RestClient(baseUrl);
        }


        //Test with response Ok even with some wrong parameters.(Need to check is it expected behaviour)
        [Test, TestCaseSource(typeof(PetTestData), nameof(PetTestData.InvalidPetJsons_ResponseOK))]
        public void AddNewPet_ResonseOkTest(string rawJson)
        {
            var response = AddNewPetNegative(rawJson);
            var responseBody = JsonConvert.DeserializeObject<PetInfo>(response.Content);

            Assert.Multiple(() =>
            {
                Assert.That(response.StatusCode, Is.EqualTo(System.Net.HttpStatusCode.OK));
                Assert.That(responseBody, Is.Not.Null);
            });
        }

        // Test for Requests with wrong parameters and Response Fail
        [Test, TestCaseSource(typeof(PetTestData), nameof(PetTestData.InvalidPetJsons_ResponseFail))]
        public void AddNewPet_ResponseFailTest(string rawJson)
        {
            var response = AddNewPetNegative(rawJson);
            var responseBody = JsonConvert.DeserializeObject<PetInfo>(response.Content);

            Assert.Multiple(() =>
            {
                Assert.That(response.StatusCode, Is.Not.EqualTo(System.Net.HttpStatusCode.OK));
                Assert.That(responseBody, Is.Not.Null);
            });
        }

        //Test for request without api_key header, response is ok, need to check is it expected beahaviour
        [Test]
        public void AddNewPet_NoApiKeyHeaderHttpResponseTest()
        {

            PetInfo newPet = CreateBasicUser();


            var response = AddNewPetNoApiKeyHeader(newPet);

            var stopwatch = Stopwatch.StartNew();
            stopwatch.Stop();

            response.IsSuccessful.Should().BeTrue();
            response.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
            Assert.That(response.StatusCode, Is.EqualTo(System.Net.HttpStatusCode.OK), "Expected HTTP 200 OK");
            Assert.Less(stopwatch.ElapsedMilliseconds, 3000, "Response time exceeded 3 seconds");
            Assert.That(response.ContentType, Does.Contain("application/json"), "Expected JSON content");
            Assert.That(response.ContentType, Does.Contain("application/json"));
            Assert.That(response.Headers, Has.Some.Matches<Parameter>(h =>
                h.Name == "Access-Control-Allow-Origin"));
            Assert.That(response.ResponseStatus, Is.EqualTo(ResponseStatus.Completed));

        }

        [TearDown]
        public void TearDown()
        {
            client.Dispose();
        }



    }
}
