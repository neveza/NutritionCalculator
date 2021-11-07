using System;
using System.Threading.Tasks;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Collections.Generic;
using System.Text.Json;
using System.Linq;

/*
Connects to FDA's data base.
U.S. Department of Agriculture, Agricultural Research Service. 
FoodData Central, 2019. fdc.nal.usda.gov.

All data is used from there.

Future features to do:
    Serialize data so user can calculate total per day 
    then calculates total of each day for week total.
    Then maybe continue on for a month tally.

    Also need to reorganize and clean up.
*/


namespace NutritionCalc
{
    class Program
    {

        private static readonly HttpClient client = new HttpClient();

        private static async Task<Repository> ProcessRepositories(string GetSearch)
        {
            Console.WriteLine("Connecting...");
            var streamTask = client.GetStreamAsync("search?api_key=DEMO_KEY&query=" + GetSearch);
            var repositories = await JsonSerializer.DeserializeAsync<Repository>(await streamTask);
            return repositories;
        }

        private static double ParseWeightValue(string text)
        {
            int startOfMeasure = 0;
            string measure = "";
            double value = 0;
            for(int j = (text.Length-1); j >= 0; --j)
            {
                if(!Char.IsLetter(text[j]))
                {
                    startOfMeasure = j+1;
                    break;
                }
            }

            string number = "";
            for(int k = 0; k < startOfMeasure; ++k)
            {
                number += text[k];
            }
            //Console.WriteLine("Number: " + number);
            double.TryParse(number, out value);
            //Console.WriteLine(value);

            for(int n = startOfMeasure; n < text.Length; ++n)
            {
                measure += text[n];
            }
            //Console.WriteLine(measure);
            return WeightConversion(value, measure);
        }
        //returns value in terms of grams
        private static double WeightConversion(double value, string measure)
        {
            switch(measure.ToLower())
            {
                case"mg": return value/1000;
                case"lbs": return value * 454;
                case"oz":  return value * 28.35;
                //case:"cups" return value  *unable to do as too inconsistent
                case"floz": return value * 29.57; //based on water. Ideally used liquids fluid like water
                default:
                    return value;
            }
        }

        private static void ParseUserRequest(string[] args, ref List<string[]> searches, ref List<double> weights)
        {
            List<string> search = new List<string>();
            for(int i = 0; i < args.Length; ++i)
            {
                string partSearch = args[i].ToLower();
                //always check for symbols first to keep IsLetter condition from false trigger
                //may look into a better way
                if (partSearch == "+")
                {
                    searches.Add(search.ToArray());
                    search = new List<string>();
                    if(searches.Count != weights.Count)
                    {
                        weights.Add(100); //adds default of 100 grams if no input of weight
                    }
                }
                //if first char of string is not a letter, process the measure amount
                //It's unlikely anybody will have an food item that is a number at the start
                else if(!Char.IsLetter(partSearch[0]))
                {
                    weights.Add(ParseWeightValue(partSearch));
                }
                else{
                    search.Add(partSearch);
                }
            }
            searches.Add(search.ToArray());
            if(searches.Count != weights.Count)
            {
                weights.Add(100); //adds default of 100 grams if no input of weight
            }
        }

        static async Task Main(string[] args)
        {
            List<double> weights = new List<double>();
            List<string[]> searches = new List<string[]>();
            client.BaseAddress = new Uri("https://api.nal.usda.gov/fdc/v1/foods/");
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/json"));
            //client.DefaultRequestHeaders.Add("User-Agent", ".NET Foundation Repository Reporter");


            if (args.Length < 1)
            {
                Console.WriteLine("Did not search");
            }
            else{

                ParseUserRequest(args, ref searches, ref weights);

                Total total = new Total();
                
                foreach(var search in searches)
                {
                    string strSearch = "";
                    foreach(var word in search)
                    {
                        strSearch += (" " + word);
                    }
                    Console.WriteLine("Searching for: " + strSearch);
                    var repo = await ProcessRepositories(strSearch);
                    //Console.WriteLine("Total Hits" + repo.TotalHits);

                    var foods = from item in repo.Foods 
                                where item.Description.ToLower().Contains((search[search.Length - 1])) 
                                select item;

                    var food = foods.GetEnumerator();
                    food.MoveNext();
                    //Console.WriteLine("Desc: " + food.Current.Description);

                     foreach(var nutrient in food.Current.Nutrients)//use Linq to pick specifics?
                     {
                        total.ProcessNutrition(nutrient);
                     }
                }
                Console.WriteLine(" ");
                total.Print();
            }
        }
    }
}
