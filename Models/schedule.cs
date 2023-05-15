#nullable disable

using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace LudusStadium.Models
{
    public partial class schedule
    {
        public int ID { get; set; }
        public DateTime matchDate { get; set; }
        public int FK_Match_ID { get; set; }
        public int FK_Stadium_ID { get; set; }

        [JsonIgnore]
        public virtual stadium Stadium { get; set; }

        public virtual void Validate()
        {
            if (matchDate < DateTime.Now)
            {
                throw new ArgumentException("Match date cannot be in the past.");
            }
        }
    }
}