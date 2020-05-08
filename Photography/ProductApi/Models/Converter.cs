using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProductApi.Models
{
    public class Converter : IConverter
    {
        public T Convert<T>(object toConvert)
        {
           var serializedObject = JsonConvert.SerializeObject(toConvert);
            
           return (T)JsonConvert.DeserializeObject(serializedObject, typeof(T));
        }
    }
}
