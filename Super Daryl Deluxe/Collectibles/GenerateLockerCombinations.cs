using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ISurvived
{
    static public class GenerateLockerCombinations
    {

        static public Dictionary<String, String> combinations;
        static Random number;

        static public void GenerateLockerCombinationsForSaveFile()
        {
            number = new Random();
            combinations = new Dictionary<string, string>();

            combinations.Add("Tim's Locker", "0" + number.Next(0, 10) + "0" + number.Next(0, 10) + "0" + number.Next(0, 10));
            combinations.Add("Someone's Locker", "0" + number.Next(0, 10) + "0" + number.Next(0, 10) + "0" + number.Next(0, 10));
        }
    }
}
