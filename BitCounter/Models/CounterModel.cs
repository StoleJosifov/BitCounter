using System.Collections.Generic;

namespace BitCounter.Models
{
    public class CounterModel
    {
        public string ByteString;

        public CounterModel()
        { }

        public CounterModel(string byteString, List<int> countersList)
        {
            this.ByteString = byteString;
            this.Counter1 = countersList[0];
            this.Counter2 = countersList[1];
            this.Counter3 = countersList[2];
            this.Counter4 = countersList[3];
            this.Counter5 = countersList[4];
            this.Counter6 = countersList[5];
            this.Counter7 = countersList[6];
            this.Counter8 = countersList[7];
        }

        public int Counter1 { get; set; }
        public int Counter2 { get; set; }
        public int Counter3 { get; set; }
        public int Counter4 { get; set; }
        public int Counter5 { get; set; }
        public int Counter6 { get; set; }
        public int Counter7 { get; set; }
        public int Counter8 { get; set; }
    }
}
