using System;
using Newtonsoft.Json;

namespace Module2_vikram.DataModel
{
    public class NotHeroModel
    {
     
		[JsonProperty(PropertyName = "Id")]
		public string ID { get; set; }

		[JsonProperty(PropertyName = "Longitude")]
		public float Longitude { get; set; }

		[JsonProperty(PropertyName = "Latitude")]
		public float Latitude { get; set; }

		[JsonProperty(PropertyName = "Hero_Name")]
		public string Hero_Name { get; set; }

		[JsonProperty(PropertyName = "url")]
		public string url { get; set; }
        }
    }
