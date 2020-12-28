using JobsityChallenge.CrossCutting.StockExchange;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace JobsityChallenge.CrossCutting.CsvHelper
{
    public interface ICsvParserService
    {
        StockValue ReadCsvFile(Stream stream);
    }
}
