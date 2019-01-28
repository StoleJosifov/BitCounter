using System;
using BitCounter.Models;
using System.Collections.Generic;

namespace BitCounter.Services
{
    public interface IDataProviderService
    {
        string GetByteAsString(int num);
        int GetRandomByte();
        List<CounterModel> GetCounterData(string fileFullPath);
        List<CounterModel> GetCounterDataFromFolder(string folderPath);
    }
}
