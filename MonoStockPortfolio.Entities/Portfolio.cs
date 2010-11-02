using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace MonoStockPortfolio.Entities
{
    public class Portfolio
    {
        public Portfolio() { }
        public Portfolio(int id) { ID = id; }
        public long? ID { get; private set; }
        public string Name { get; set; }

        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("ID", this.ID);
            info.AddValue("Name", this.Name);
        }
    }
}