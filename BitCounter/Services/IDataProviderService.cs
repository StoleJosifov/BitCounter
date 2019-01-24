﻿using System;
using BitCounter.Models;
using System.Collections.Generic;

namespace BitCounter.Services
{
    public interface IDataProviderService
    {
        string GetRandomByteAsString();
        List<CounterModel> GetCounterData(string fileFullPath);
        List<CounterModel> GetCounterDataFromFolder(string folderPath);
    }
}
