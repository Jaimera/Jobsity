using JobsityChallenge.CrossCutting.StockExchange;
using Microsoft.VisualBasic.FileIO;
using System;
using System.Globalization;
using System.IO;

namespace JobsityChallenge.CrossCutting.CsvHelper
{
    public class CsvParserService : ICsvParserService
    {
        public StockValue ReadCsvFile(Stream stream)
        {
            try
            {
                using (var streamReader = new StreamReader(stream))
                using (TextFieldParser parser = new TextFieldParser(stream))
                {
                    parser.SetDelimiters(new string[] { "," });

                    parser.ReadLine();

                    CultureInfo culture = new CultureInfo("en-US");
                    string[] fields = parser.ReadFields();

                    var stockValue = new StockValue();
                    stockValue.Symbol = fields[0];

                    if (fields[1] == "N/D")
                        return stockValue;

                    stockValue.Date = DateTime.Parse(fields[1]);
                    stockValue.Time = DateTime.Parse(fields[2]);
                    stockValue.Open = decimal.Parse(fields[3], culture);
                    stockValue.High = decimal.Parse(fields[4], culture);
                    stockValue.Low = decimal.Parse(fields[5], culture);
                    stockValue.Close = decimal.Parse(fields[6], culture);
                    stockValue.Volume = decimal.Parse(fields[7], culture);
                    

                    return stockValue;
                }
            }
            catch (UnauthorizedAccessException e)
            {
                throw new Exception(e.Message);
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }
    }
}
