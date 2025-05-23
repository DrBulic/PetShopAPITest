using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetShopAPITest.Data
{
    public class PetInfo
    {

        public Int64 id { get; set; }
        public Category category { get; set; }
        public string name { get; set; }
        public List<string> photoUrls { get; set; }
        public List<Tag> tags { get; set; }
        public string status { get; set; }


        public class Category
        {
            public Int64 id { get; set; }
            public string name { get; set; }
        }


        public class Tag
        {
            public Int64 id { get; set; }
            public string name { get; set; }
        }



    }
}
