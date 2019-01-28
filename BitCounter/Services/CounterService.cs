using BitCounter.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace BitCounter.Services
{
    public sealed class CounterService : IHostedService
    {
        private readonly ILogger<CounterService> logger;
        private readonly IDataProviderService dataProviderService;
        private readonly IOptions<SaveSettings> settings;

        private readonly int saveFileIntervalSeconds;
        private List<int> previousCountersList;
        private Timer counterTimer;
        private SaveSettings saveSettings;


        public CounterService(
            IDataProviderService dataProviderService,
            IOptions<SaveSettings> settings,
            ILogger<CounterService> logger)
        {
            this.saveSettings = settings.Value;
            this.logger = logger;
            this.dataProviderService = dataProviderService;
            this.settings = settings;
            this.saveFileIntervalSeconds = GetSaveIntervalInSeconds(this.saveSettings.SavePeriod);
            this.previousCountersList = Enumerable.Repeat(0, 8).ToList();
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            this.logger.LogInformation("Counter is starting.");
            this.counterTimer = new Timer(StartCounting, null, TimeSpan.Zero, TimeSpan.FromSeconds(this.saveFileIntervalSeconds));
            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            this.logger.LogInformation("Counter is stopping.");
            this.counterTimer.Change(Timeout.Infinite, 0);
            this.logger.LogInformation("Counter stopped.");
            return Task.CompletedTask;
        }

        private void StartCounting(object state)
        {
            var dataList = GetData();
            SaveData(dataList);
        }   

        private void SaveData(List<CounterModel> dataList)
        {
            var folderPath = this.saveSettings.SaveFolder;
            if (!Directory.Exists(this.saveSettings.SaveFolder))
            {
                this.logger.LogWarning("Save folder does not exists. Folder path set to default project path.");
                folderPath = Directory.GetCurrentDirectory();
            }
            var fileFullPath = folderPath + "\\cnts_" + DateTime.Now.ToString("MM-dd-yyyy") + ".json";
            this.logger.LogInformation("File full path is set to {0}.", fileFullPath);
            using (var file = File.AppendText(fileFullPath))
            {
                var serializer = new JsonSerializer();
                serializer.Serialize(file, dataList);
                this.logger.LogInformation("Data saved to file.");
            }

        }
        
        private List<CounterModel> GetData()
        {
            var dataBatch = new List<CounterModel>();

            for (int i = 0; i < this.saveFileIntervalSeconds; i++)
            {
                var randomByte = this.dataProviderService.GetRandomByte();
                var randomByteAsString = this.dataProviderService.GetByteAsString(randomByte);
                this.logger.LogInformation("Random byte generated : {0}", randomByteAsString);
                var countersList = CountBits(randomByte);
                var item = new CounterModel(randomByteAsString, countersList);
                this.logger.LogInformation("Counter model created");
                dataBatch.Add(item);
            }
            return dataBatch;
        }

        private List<int> CountBits(int randomByte)
        {
            var currentCountersList = this.previousCountersList;
            for (int j = 0; j < 8; j++)
            {
                currentCountersList[j] +=  randomByte & 1;
                randomByte = randomByte >> 1;
            }
            this.previousCountersList = currentCountersList;
            return currentCountersList;
        }


        #region Helpers
        private static int GetSaveIntervalInSeconds(string saveFileInterval)
        {
            if (!string.IsNullOrEmpty(saveFileInterval))
            {
                //get value
                var value = Convert.ToInt32(saveFileInterval.Split(" ")[0]);
                //get units
                var units = saveFileInterval.Split(" ")[1].ToLower();
                //convert to seconds
                return ConvertToSeconds(value, units);
            }
            else
            {
                //default value
                return 60;
            }
        }

        private static int ConvertToSeconds(int value, string units)
        {
            if (units.Contains("sec"))
            {
                return value;
            }
            else if (units.Contains("min"))
            {
                return (value * 60);
            }
            else if (units.Contains("h"))
            {
                return (value * 60 * 60);
            }
            else
            {
                //default value
                return 60;
            }
        }
        #endregion
    }
}
