using System;
using System.Text.Json.Serialization;
using System.Data;
using System.Collections.Generic;

namespace NutritionCalc
{
    public class Repository
    {
        [JsonPropertyName("totalHits")]
        public int TotalHits {get; set;}
        
        [JsonPropertyName("foods")]
        public List<Food> Foods {get; set;} //in fucking business.
        
    }
//Should be default 100 grams?
    public class Food{
        [JsonPropertyName("fdcId")]
        public int Id {get; set;}

        [JsonPropertyName("description")]
        public string Description{get;set;}

        [JsonPropertyName("foodNutrients")]
        public List<Nutrient> Nutrients {get; set;}
    }

    public class Nutrient{

        [JsonPropertyName("nutrientName")]
        public string Name{get;set;}

        [JsonPropertyName("value")]
        public double Value{get;set;}

        [JsonPropertyName("unitName")]
        public string UnitType{get;set;}

    }
}