using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace JobsityChallenge.CrossCutting.StockExchange
{
    public interface IStockClient
    {
        Task<StockValue> GetStock(string code);
    }
}
