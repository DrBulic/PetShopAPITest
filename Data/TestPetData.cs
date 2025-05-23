using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetShopAPITest.Data
{
    public static class PetTestData
    {
        //Test data with response Ok even with some wrong parameters.(Need to check is it expected behaviour )
        public static IEnumerable<TestCaseData> InvalidPetJsons_ResponseOK()
        {
            yield return new TestCaseData(@"
            {
                ""id"": 123,
                ""category"": {
                    ""id"": 4444,
                    ""name"": ""categoryName""
                },
                ""name"": ""Jacky"",
                ""photoUrls"": [""http://example.com/photo.jpg""],
                ""tags"": [
                    {
                        ""id"": ""123"",
                        ""name"": ""tagName""
                    }
                ],
                ""status"": ""available""
            }").SetName("ResponseOk_Invalid_Id_isNotZero");          

           

            yield return new TestCaseData(@"
            {
                ""id"": 0,
                ""category"": {
                    ""id"": 4444,
                    ""name"": 567
                },
                ""name"": ""Jacky"",
                ""photoUrls"": [""http://example.com/photo.jpg""],
                ""tags"": [
                    {
                        ""id"": ""123"",
                        ""name"": ""tagName""
                    }
                ],
                ""status"": ""available""
            }").SetName("ResponseOk_Invalid_CategoryName_IsInteger");

            yield return new TestCaseData(@"
            {
                ""id"":0 ,
                ""category"": {
                    ""id"": 123,
                    ""name"": ""categoryName""
                },
                ""name"": ""Jacky"",
                ""photoUrls"": [""http://example.com/photo.jpg""],
                ""tags"": [
                    {
                        ""id"": ""123"",
                        ""name"": ""tagName""
                    }
                ],
                ""status"": ""available""
            }").SetName("ResponseOk_Invalid_TagId_isString");

            yield return new TestCaseData(@"
            {
                ""id"":0 ,
                ""category"": {
                    ""id"": 123,
                    ""name"": ""categoryName""
                },
                ""name"": ""Jacky"",
                ""photoUrls"": [""http://example.com/photo.jpg""],
                ""tags"": [],
               
                ""status"": ""available""
            }").SetName("ResponseOK_Invalid_Tag_isEmpty");

            yield return new TestCaseData(@"
            {
                ""id"":0 ,
                ""category"": {
                    ""id"": 123,
                    ""name"": ""categoryName""
                },
                ""name"": ""Jacky"",
                ""photoUrls"": [""http://example.com/photo.jpg""],
                               
                ""status"": ""available""
            }").SetName("ResponseOK_Invalid_NoTag");

            yield return new TestCaseData(@"
            {
                ""id"": 0,
                ""category"": {
                    ""id"": 4444,
                    ""name"": ""categoryName""
                },
                ""name"": ""Jacky"",
                ""photoUrls"": [1234],
                ""tags"": [
                    {
                        ""id"": ""123"",
                        ""name"": ""tagName""
                    }
                ],
                ""status"": ""available""
            }").SetName("ResponseOk_Invalid_PhotoUrl_isInteger");




        }

        // Test data for Requests with wrong parameters and Response Fail
        public static IEnumerable<TestCaseData> InvalidPetJsons_ResponseFail()
        {
           
            yield return new TestCaseData(@"
            {
                ""id"": ""someString"",
                ""category"": {
                    ""id"": 4444,
                    ""name"": ""categoryName""
                },
                ""name"": ""Jacky"",
                ""photoUrls"": [""http://example.com/photo.jpg""],
                ""tags"": [
                    {
                        ""id"": 123,
                        ""name"": ""tagName""
                    }
                ],
                ""status"": ""available""
            }").SetName("ResponseFail_Invalid_Id_isString");

            yield return new TestCaseData(@"
            {
                ""id"": ,
                ""category"": {
                    ""id"": 4444,
                    ""name"": ""categoryName
                },
                ""name"": ""Jacky"",
                ""photoUrls"": [""http://example.com/photo.jpg""],
                ""tags"": [
                    {
                        ""id"": ""123"",
                        ""name"": ""tagName""
                    }
                ],
                ""status"": ""available""
            }").SetName("ResponseFail_Invalid_Id_isEmpty");

            yield return new TestCaseData(@"
            {
                ""id"":0 ,
                ""category"": {
                    ""id"": ""someString"",
                    ""name"": ""categoryName
                },
                ""name"": ""Jacky"",
                ""photoUrls"": [""http://example.com/photo.jpg""],
                ""tags"": [
                    {
                        ""id"": 123,
                        ""name"": ""tagName""
                    }
                ],
                ""status"": ""available""
            }").SetName("ResponseFail_Invalid_CategoryId_isString");

            yield return new TestCaseData(@"
            {
                ""id"": 0,
                ""category"": {
                    ""id"": ,
                    ""name"": ""categoryName""
                },
                ""name"": ""Jacky"",
                ""photoUrls"": [""http://example.com/photo.jpg""],
                ""tags"": [
                    {
                        ""id"": 123,
                        ""name"": ""tagName""
                    }
                ],
                ""status"": ""available""
            }").SetName("ResponseFail_Invalid_CategoryId_isEmpty");

            yield return new TestCaseData(@"
            {
                ""id"":0 ,
                ""category"": {
                    ""id"": 123,
                    ""name"": ""categoryName""
                },
                ""name"": ""Jacky"",
                ""photoUrls"": [""http://example.com/photo.jpg""],
                ""tags"": [
                    {
                        ""id"": ,
                        ""name"": ""tagName""
                    }
                ],
                ""status"": ""available""
            }").SetName("ResponseFail_Invalid_TagId_isEmpty");       



        }
    }

}
