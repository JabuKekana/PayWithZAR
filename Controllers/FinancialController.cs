using Elasticsearch.Net;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Nest;
using Newtonsoft.Json;
using SmartFinExPro.Models;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

public class FinancialController : Controller
{
    private readonly ILogger<FinancialController> _logger;
    private readonly IElasticLowLevelClient _elasticClient;
    private readonly IConfiguration _configuration;

    public FinancialController(ILogger<FinancialController> logger, IElasticLowLevelClient elasticClient, IConfiguration configuration)
    {
        _logger = logger;
        _elasticClient = elasticClient;
        _configuration = configuration;
    }

    public async Task<IActionResult> Index()
    {
        var yahooFinanceApiKey = _configuration["YahooFinanceApi:ApiKey"];
        var yahooFinanceApiHost = _configuration["YahooFinanceApi:ApiHost"];
        var yahooFinanceApiUrl = "https://" + yahooFinanceApiHost + "/market/v2/get-summary?region=US";

        using (var client = new HttpClient())
        {
            client.DefaultRequestHeaders.Add("X-RapidAPI-Key", yahooFinanceApiKey);
            client.DefaultRequestHeaders.Add("X-RapidAPI-Host", yahooFinanceApiHost);

            var response = await client.GetStringAsync(yahooFinanceApiUrl);

            // Parse the JSON response
            var marketSummary = JsonConvert.DeserializeObject<MarketSummary>(response);

            // Use marketSummary to display data in the view or store it in Elasticsearch
            var financialDataList = new List<FinancialData>();
            // Populate financialDataList based on marketSummary or other data as needed

            return View(financialDataList);
        }
    }
}
