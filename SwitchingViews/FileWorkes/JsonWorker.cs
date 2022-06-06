using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhoneBook.FileWorkes
{
    internal class JsonWorker
    {
        public static string GetDescription(string message,string replacement="")
        {
            
            var builder = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile(System.Configuration.ConfigurationManager.ConnectionStrings["JsonPath"].ConnectionString);
            var config = builder.Build();
            if (!replacement.Equals(string.Empty))
               return  config[message].Replace("{field}", replacement);
            return config[message];
        }
        
        

       
        

    }
}
