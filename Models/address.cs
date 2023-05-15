#nullable disable

using System.Text.Json.Serialization;

namespace LudusStadium.Models
{
    public partial class address
    {
        [JsonIgnore]
        public int ID { get; set; }
        public string street { get; set; }
        public string city { get; set; }
        public string state { get; set; }
        public int number { get; set; }
        public string zip { get; set; }
    }
}