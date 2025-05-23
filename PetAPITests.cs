using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using Newtonsoft.Json;
using PetShopAPITest.Data;
using FluentAssertions;
using System.Diagnostics.CodeAnalysis;
using System.Security.Cryptography.X509Certificates;
using Microsoft.Extensions.Configuration;


using System.Diagnostics;
using System.IO;

namespace PetShopAPITest
{
    public class PetTest : BaseApiClass
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

        [Test]
        public void AddNewPet_PayloadTest()
        {

            PetInfo newPet = CreateBasicUser();

            var response = AddNewPet(newPet);

            var responseBody = JsonConvert.DeserializeObject<PetInfo>(response.Content);

            Assert.Multiple(() =>
            {
                Assert.That(response.StatusCode, Is.EqualTo(System.Net.HttpStatusCode.OK));
                Assert.That(response.Headers, Is.Not.Null);
                Assert.That(response.Headers.Count, Is.EqualTo(7));

                Assert.That(responseBody.id, Is.GreaterThan(0));
                Assert.That(responseBody.name, Is.EqualTo("Rex"));
                Assert.That(responseBody.status, Is.EqualTo("available"));

                Assert.That(responseBody.photoUrls, Is.Not.Null);
                Assert.That(responseBody.photoUrls.Count, Is.EqualTo(1));
                Assert.That(responseBody.photoUrls[0], Is.EqualTo("http://example.com/photo.jpg"));

                Assert.That(responseBody.category, Is.Not.Null);
                Assert.That(responseBody.category.name, Is.EqualTo("dogs"));
                Assert.That(responseBody.category.id, Is.EqualTo(123));


                Assert.That(responseBody.tags, Is.Not.Null);
                Assert.That(responseBody.tags.Count, Is.EqualTo(1));
                Assert.That(responseBody.tags[0].name, Is.EqualTo("testTag"));
                Assert.That(responseBody.tags[0].id, Is.EqualTo(456));
            });


            var createdPet = GetPetByID_ReturnBody(responseBody.id);

            Assert.Multiple(() =>
            {
                Assert.That(createdPet.id, Is.GreaterThan(0), "Pet ID should be greater than 0");
                Assert.That(createdPet.name, Is.EqualTo("Rex"), "Pet name should be 'Rex'");
                Assert.That(createdPet.status, Is.EqualTo("available"), "Pet status should be 'available'");

                Assert.That(createdPet.photoUrls, Is.Not.Null, "photoUrls should not be null");
                Assert.That(createdPet.photoUrls.Count, Is.EqualTo(1), "There should be 1 photo URL");
                Assert.That(createdPet.photoUrls[0], Is.EqualTo("http://example.com/photo.jpg"), "Photo URL should match 'testString'");

                Assert.That(createdPet.category, Is.Not.Null, "Category should not be null");
                Assert.That(createdPet.category.name, Is.EqualTo("dogs"), "Category name should be 'dogs'");
                Assert.That(createdPet.category.id, Is.EqualTo(123), "Category ID should be 123");

                Assert.That(createdPet.tags, Is.Not.Null.And.Count.EqualTo(1), "There should be exactly 1 tag");
                Assert.That(createdPet.tags[0].name, Is.EqualTo("testTag"), "Tag name should be 'testTag'");
                Assert.That(createdPet.tags[0].id, Is.EqualTo(456), "Tag ID should be 456");
            });



        }

      

        [Test]
        public void AddNewPet_HttpResponseTest()
        {

            PetInfo newPet = CreateBasicUser();


            var response = AddNewPet(newPet);

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


        [Test]
        public void UpdatePet_HttpResponseTest()
        {

            PetInfo updatePet = CreateBasicUser();

            updatePet.name = "RexNEW";
            updatePet.status = "sold";
            updatePet.photoUrls[0] = "http://example.com/photoNEW.jpg";
            updatePet.category = new PetInfo.Category() { id = 12399, name = "dogsNEW" };
            updatePet.tags = new List<PetInfo.Tag>() { new PetInfo.Tag() { id = 45699, name = "testTagNEW" } };


            RestResponse response = UpdatePet(updatePet);

            var stopwatch = Stopwatch.StartNew();
            stopwatch.Stop();

            response.IsSuccessful.Should().BeTrue();
            response.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);

            Assert.Multiple(() =>
            {
                Assert.That(response.StatusCode, Is.EqualTo(System.Net.HttpStatusCode.OK), "Expected HTTP 200 OK");
                Assert.Less(stopwatch.ElapsedMilliseconds, 3000, "Response time exceeded 3 seconds");
                Assert.That(response.ContentType, Does.Contain("application/json"));
                Assert.That(response.Headers, Has.Some.Matches<Parameter>(h =>
                    h.Name == "Access-Control-Allow-Origin"));
                Assert.That(response.ResponseStatus, Is.EqualTo(ResponseStatus.Completed));
            });

        }

        [Test]
        public void UpdatePet_PayloadTest()
        {
            PetInfo updatePet = CreateBasicUser();

            updatePet.name = "RexNEW";
            updatePet.status = "sold";
            updatePet.photoUrls[0] = "http://example.com/photoNEW.jpg";         
            updatePet.category = new PetInfo.Category() { id = 12399, name = "dogsNEW" };
            updatePet.tags = new List<PetInfo.Tag>() { new PetInfo.Tag() { id = 45699, name = "testTagNEW" } };

            RestResponse response = UpdatePet(updatePet);

         
            PetInfo responseBody = JsonConvert.DeserializeObject<PetInfo>(response.Content);

            Assert.Multiple(() =>
            {
                // Status code
                Assert.That(response.StatusCode, Is.EqualTo(System.Net.HttpStatusCode.OK), "Status code should be 200 OK");

                // Response body main fields
                Assert.That(responseBody.id, Is.GreaterThan(0), "ID should be greater than 0");
                Assert.That(responseBody.name, Is.EqualTo("RexNEW"), "Name should be 'Rex'");
                Assert.That(responseBody.status, Is.EqualTo("sold"), "Status should be 'available'");

                // Photo URLs
                Assert.That(responseBody.photoUrls, Is.Not.Null, "photoUrls should not be null");
                Assert.That(responseBody.photoUrls.Count, Is.EqualTo(1), "There should be 1 photo URL");
                Assert.That(responseBody.photoUrls[0], Is.EqualTo("http://example.com/photoNEW.jpg"), "First photo URL should be 'http://example.com/photoNEW.jpg'");

                // Category
                Assert.That(responseBody.category, Is.Not.Null, "Category should not be null");
                Assert.That(responseBody.category.name, Is.EqualTo("dogsNEW"), "Category name should be 'dogs'");
                Assert.That(responseBody.category.id, Is.EqualTo(12399), "Category ID should be 123");

                // Tags
                Assert.That(responseBody.tags, Is.Not.Null.And.Count.EqualTo(1), "There should be exactly 1 tag");
                Assert.That(responseBody.tags[0].name, Is.EqualTo("testTagNEW"), "Tag name should be 'testTag'");
                Assert.That(responseBody.tags[0].id, Is.EqualTo(45699), "Tag ID should be 456");
            });



        }

        [Test]
        public void GetPetById_PayloadTest()
        {
            PetInfo newPet = CreateBasicUser();
            var response = AddNewPet(newPet);
            var responseBody = JsonConvert.DeserializeObject<PetInfo>(response.Content);
            var response2 = GetPetByID(responseBody.id);
            var createdPet = JsonConvert.DeserializeObject<PetInfo>(response2.Content);

            Assert.Multiple(() =>
            {
                Assert.That(createdPet.id, Is.GreaterThan(0), "Pet ID should be greater than 0");
                Assert.That(createdPet.name, Is.EqualTo("Rex"), "Pet name should be 'Rex'");
                Assert.That(createdPet.status, Is.EqualTo("available"), "Pet status should be 'available'");

                Assert.That(createdPet.photoUrls, Is.Not.Null.And.Count.EqualTo(1), "There should be 1 photo URL");
                Assert.That(createdPet.photoUrls[0], Is.EqualTo("http://example.com/photo.jpg"), "Photo URL should be 'testString'");

                Assert.That(createdPet.category, Is.Not.Null, "Category should not be null");
                Assert.That(createdPet.category.name, Is.EqualTo("dogs"), "Category name should be 'dogs'");
                Assert.That(createdPet.category.id, Is.EqualTo(123), "Category ID should be 123");

                Assert.That(createdPet.tags, Is.Not.Null.And.Count.GreaterThanOrEqualTo(1), "There should be at least 1 tag");
                Assert.That(createdPet.tags[0].name, Is.EqualTo("testTag"), "First tag name should be 'testTag'");
                Assert.That(createdPet.tags[0].id, Is.EqualTo(456), "First tag ID should be 456");
            });


        }


        [Test]
        public void GetPetById_HttpResponseTest()
        {
            PetInfo newPet = CreateBasicUser();
            var response1 = AddNewPet(newPet);
            var responseBody = JsonConvert.DeserializeObject<PetInfo>(response1.Content);
            var response = GetPetByID(responseBody.id);

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

