using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DungeonsAndDragonsApp.Model.DnD
{
    public class EquipmentResponse
    {
        [JsonProperty("equipment")]
        public Equipment Equipment { get; set; }
        [JsonProperty("quantity")]
        public int Quantity { get; set; }
    }

    public class Equipment
    {
        [JsonProperty("index")]
        public string Index { get; set; }
        [JsonProperty("name")]
        public string Name { get; set; }
        [JsonProperty("url")]
        public string Url { get; set; }
    }
}
