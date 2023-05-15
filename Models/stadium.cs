#nullable disable

using System.Text.Json.Serialization;

namespace LudusStadium.Models
{
    public partial class stadium
    {
        public int ID { get; set; }
        public string name { get; set; }
        public int capacity { get; set; }

        [JsonIgnore]
        public int FK_Address_ID { get; set; }

        public virtual address Address { get; set; }
    }
}