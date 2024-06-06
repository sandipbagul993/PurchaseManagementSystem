﻿using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
#nullable disable
namespace Client.MetaData
{
  
    public class ProductViewModel
    {
        [JsonPropertyName("id")]
        public int Id { get; set; }

        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("price")]
        public decimal Price { get; set; }

      
    }
}
