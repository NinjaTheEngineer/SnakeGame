using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Random = System.Random;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

public static class Utils {
    static Random _random;
    public static Random Random => _random ?? (_random = new Random());
    public static void InitRandom(int seed) => _random = new Random(seed);
    
    public static bool CheckForRegex(this Regex regex, string input) {
        if (input == null) {
            throw new ArgumentNullException("input");
        }
        if(regex == null) {
            throw new ArgumentNullException("regex");
        }
        //Match match = regex.Matches(input);
        if (regex.IsMatch(input)) {
            return true;
        }
        return false;
    }
    public static string ReverseWord(this string word)
    {
        string reversedWord = "";
        for (int i = word.Length - 1; i >= 0; i--)
        {
            reversedWord += word[i];
        }
        return reversedWord;
    }
    public static string GetFileAsString(string fileName) {
        try
        {
            string file = File.ReadAllText(fileName);
            return file;
        }
        catch (Exception e)
        {
            Debug.Log(e.Message);
        }
        return null;
    }
    public static string[] GetFileAsStringLines(string fileName)
    {
        try
        {
            string[] file = File.ReadAllLines(fileName);
            return file;
        }
        catch (Exception e)
        {
            Debug.Log(e.Message);
        }
        return null;
    }
    public static void ValidateUserNumberInput(out int num)
    {
        while (!int.TryParse(Console.ReadLine(), out num))
        {
            Debug.Log("You entered an invalid number.");
            Console.Write("Enter a valid number: ");
        }
    }
    public static void ValidateUserStringInput(out string text)
    {
        text = Console.ReadLine();
        while (text.Any(x => !char.IsLetterOrDigit(x) && !char.IsSeparator(x) && x != '-' && x != ':'))
        {
            Debug.Log("You entered an invalid string.");
            Console.Write("Enter a valid string: ");
            text = Console.ReadLine();
        }
    }
    public static void ValidateUserWordStringInput(out string text)
    {
        text = Console.ReadLine();
        while (text.Any(x => !char.IsLetter(x)))
        {
            Debug.Log("You entered an invalid string.");
            Console.Write("Enter a valid string: ");
            text = Console.ReadLine();
        }
    }
    public static string CleanText(this string text)
    {
        text = text.ToLower();
        text = text.Trim();
        return text;
    }
    public static string ListToString<T>(List<T> list)
    {
        return "[" + string.Join(", ", list) + "]";
    }
    public static string DictionaryToString<T, Y>(Dictionary<T, List<Y>> dictionary)
    {
        if (dictionary == null)
        {
            throw new ArgumentNullException("dictionary");
        }
        string dictionaryAsString = "";
        foreach (var kvp in dictionary)
        {
            dictionaryAsString += "{" + kvp.Key + "=";
            dictionaryAsString += "[" + ListToString(kvp.Value) + "], ";
        }
        dictionaryAsString += "}";
        return dictionaryAsString;
    }
    public static string DictionaryToString<T, Y>(Dictionary<T, Y> dictionary) {
        if (dictionary == null)
        {
            throw new ArgumentNullException("dictionary");
        }
        string dictionaryAsString = "{";
        foreach (var kvp in dictionary) {
            dictionaryAsString +=  kvp.Key + "=";
            dictionaryAsString += kvp.Value + ", ";
        }
        dictionaryAsString += "}";
        return dictionaryAsString;
    }
    public static string logf(this object o) => o == null ? "NULL" : o.ToString();
    public static T RandomSelect<T>(this IList<T> list)
    {
        if (list == null)
        {
            throw new ArgumentNullException("Exception: Random Select trial on List = " + list.logf());
        }
        return list[Random.Next(0, list.Count)];
    }
    public static double RandomDouble(double minValue, double maxValue) => Random.NextDouble() * (maxValue - minValue) + minValue;
    public static List<int> PickRandomInts(int returnCount, int minValue, int maxValue)
    {

        int optionCount = maxValue - minValue;
        returnCount = returnCount > optionCount ? optionCount : returnCount;
        if (returnCount <= 0)
        {
            return null;
        }

        List<int> randomInts = new List<int>(returnCount);
        List<int> possibleInts = new List<int>(optionCount);

        for (int i = minValue; i < maxValue; i++)
        {
            possibleInts.Add(i);
        }

        for (int i = 0; i < returnCount; i++)
        {
            int randomIndex = Random.Next(0, possibleInts.Count);
            randomInts.Add(possibleInts[randomIndex]);
            possibleInts.RemoveAt(randomIndex);
        }
        return randomInts;
    }
}
