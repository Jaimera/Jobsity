using JobsityChallenge.CrossCutting.CsvHelper;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace JobsityChallenge.CrossCutting.StockExchange
{
    public class StockClient : IStockClient
    {
        private readonly HttpClient client;
        private readonly ICsvParserService csvParser;

        public StockClient(ICsvParserService _csvParser)
        {
            csvParser = _csvParser;
            client = new HttpClient();
            client.BaseAddress = new Uri("https://stooq.com/q/l/");
        }

        public async Task<StockValue> GetStock(string code)
        {
            var response = await client.GetAsync($"?s={code}&f=sd2t2ohlcv&h&e=csv");

            var result = csvParser.ReadCsvFile(await response.Content.ReadAsStreamAsync());

            return result;
        }
    }
}
