using System;
using System.Collections.Generic;


namespace NutritionCalc
{
    class Total{

       public List<Nutrient> Nutrients {get; set;}

       public void Add(string name, double value, string unitType)
       {
           if(Nutrients.Count != 0)
           {
              foreach(var nut in Nutrients)
              {
                  if(nut.Name == name)
                  {
                       nut.Value += value;
                       return;
                  }
              }
           }

           Nutrient nutrient = new Nutrient();
           nutrient.Name = name;
           nutrient.Value = value;
           nutrient.UnitType = unitType;

           Nutrients.Add(nutrient);
       }


       public void ProcessNutrition(Nutrient nutrient)
        {
            //filtering out the ones I don't need
            switch(nutrient.Name)
            {
                case "Protein":Add("Protein", nutrient.Value, nutrient.UnitType); return;
                case "Total lipid (fat)":Add("Total Fat", nutrient.Value, nutrient.UnitType); return;
                case "Energy": Add("Calories", nutrient.Value, nutrient.UnitType); return;
                case "Carbohydrate, by difference":Add("Carbs", nutrient.Value, nutrient.UnitType); return;
                case "Sugars, total including NLEA": Add("Sugar", nutrient.Value, nutrient.UnitType);return;
                case "Fiber, total dietary": Add("Fiber", nutrient.Value, nutrient.UnitType);return;
                case "Calcium, Ca": Add("Calcium", nutrient.Value, nutrient.UnitType);return;
                case "Iron, Fe": Add("Iron", nutrient.Value, nutrient.UnitType);return;
                case "Magnesium, Mg": Add("Magnesium", nutrient.Value, nutrient.UnitType);return;
                case "Potassium, K": Add("Potassium", nutrient.Value, nutrient.UnitType);return;
                case "Sodium, Na": Add("Sodium", nutrient.Value, nutrient.UnitType);return;
                //case "Zinc, Zn": return;
                //case "Copper, Cu": return;
                case "Vitamin A, RAE": Add("Vitamin A", nutrient.Value, nutrient.UnitType);return;
                case "Vitamin E (alpha-tocopherol)": Add("Vitamin E", nutrient.Value, nutrient.UnitType);return;
                case "Vitamin C, total ascorbic acid":Add("Vitamin C", nutrient.Value, nutrient.UnitType);return;
                case "Vitamin B-6": Add("Vitamin B-6", nutrient.Value, nutrient.UnitType); return;
                case "Vitamin B-12": Add("Vitamin B-12", nutrient.Value, nutrient.UnitType);return;
                case "Vitamin K (phylloquinone)": Add("Vitamin K", nutrient.Value, nutrient.UnitType);return;
                case "Cholesterol": Add("Cholesterol", nutrient.Value, nutrient.UnitType);return;
                case "Fatty acids, total saturated": Add("Total Saturated Fats", nutrient.Value, nutrient.UnitType);return;
                case "Fatty acids, total monounsaturated": Add("Total Monounsaturated Fats", nutrient.Value, nutrient.UnitType);return;
                case "Fatty acids, total polyunsaturated": Add("Total Polyunsaturated Fats", nutrient.Value, nutrient.UnitType);return;
                default: return;
            }
        }

        public void Print()
        {
            Console.WriteLine("Total Nutrition:");
            foreach (var nutrient in Nutrients)
            {
                Console.WriteLine("    " + nutrient.Name + ": " + Math.Round(nutrient.Value, 2) + nutrient.UnitType);
            }
        }

        public Total()
        {
            Nutrients = new List<Nutrient>();
        }
    }
}