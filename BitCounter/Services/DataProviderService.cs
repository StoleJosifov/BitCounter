using BitCounter.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;

namespace BitCounter.Services
{
    public sealed class DataProviderService : IDataProviderService
    {
        public List<CounterModel> GetCounterData(string fileFullPath)
        {
            var rawJsonString = File.ReadAllText(fileFullPath);
            var normalizedJsonString = rawJsonString.Replace("][", ",");
            var data = JsonConvert.DeserializeObject<List<CounterModel>>(normalizedJsonString);
            return data;
        }

        public string GetRandomByteAsString()
        {
            var rand = new Random();
            var byteAsString = Convert.ToString(rand.Next(255), 2).PadLeft(8, '0');
            return byteAsString;
        }
    }
}
