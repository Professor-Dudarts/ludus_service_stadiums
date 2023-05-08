#nullable disable
using System;
using System.Collections.Generic;

namespace Ludus_Stadium.Models
{
    public partial class stadium
    {
        public int id { get; set; }
        public string name { get; set; }
        public string adress { get; set; }
        public int capacity { get; set; }
        public DateTime openingDate { get; set; }
    }
}