using System;
using System.Collections.Generic;
using System.IO;
using Android.Util;

namespace MonoStockPortfolio.Core.StockData
{
    public class CsvParser
    {
        public static IEnumerable<string[]> ParseCsvIntoStockQuotes(string csvText)
        {
            using (var sr = new StringReader(csvText))
            {
                var lines = new List<string[]>();

                try
                {
                    string line;
                    while ((line = sr.ReadLine()) != null)
                    {
                        var tokens = line.Split(',');
                        for (int i = 0; i < tokens.Length; i++)
                        {
                            tokens[i] = tokens[i].Trim('\"');
                        }
                        lines.Add(tokens);
                    }
                }
                catch (Exception)
                {
                    Log.Error("ParseCSV", "Error in retrieving/parsing stock information");
                }

                return lines;
            }
        }        
    }
}