using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OrderApi.Models
{
    public interface IConverter
    {
        /// <summary>
        /// Generic method in order to convert easily.
        /// </summary>
        /// <typeparam name="T">Entity To convert TO</typeparam>
        /// <param name="toConvert">To be converted</param>
        /// <returns></returns>
        public T Convert<T>(object toConvert);
    }
}
