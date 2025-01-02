using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Running;

namespace Cryptocurrency.Benchmark;

public class Program
{
    static void Main(string[] args)
    {
        var summary = BenchmarkRunner.Run<CryptoApiBenchmark>();
    }
}

public class CryptoApiBenchmark
{
    private static readonly HttpClient _httpClient = new()
    {
        BaseAddress = new Uri("http://localhost:5139/"),
        Timeout = TimeSpan.FromSeconds(30),
    };

    [Params("BTC", "ETH", "XRP")]
    public string CryptoCode;

    [Params(1, 5, 10)]
    public int N;

    [GlobalSetup]
    public void GlobalSetup()
    {
        _httpClient.DefaultRequestHeaders.Add("Authorization", "Bearer eyJhbGciOiJub25lIiwidHlwIjoiSldUIn0.eyJlbmNfcGF5bG9hZCI6IlRXSkNQc2tiM002SWsvdjBWWVV5ekhoRFNGdENoZVdSUDV0RFdmaGtWdHdKQU04QWlLaVpjUmN4cmM2WXZJVlUzNmhlb29pREVRUXZBejFDaEQwKzQ0b3o1ZjIxSGo1amNZYlFxRzRYK29YTFU0RFJZZmtHY0sreXZSeHNkaU5ubUNRekRSUmk2SmhkcjdBa0RBYml3K25OL2RVNnJNOEg4Ly9sM09sbGdLZFVxMi9NM0p5VTZpeHg5SXZmR2pEcVpTMGZYWVp5V3JnYUFFRFNsa0krOWFtM1ZWQUo1N0J2b2xiWXRmK3Z0M0ZsbERKZU81SXJtaDNrcFhVb3RpQWtnckJTYm9PZmxUTFdOZmFEczhkOWk3K0ZndTFqTitVUmZYSGZaSlhsMjUwPSJ9.");
    }

    [Benchmark]
    public async Task GetCryptoRates()
    {
        for (var i = 0; i < N; i++)
        {
            var response = await _httpClient.GetAsync($"crypto/GetCryptoRates?symbol={CryptoCode}");
            response.EnsureSuccessStatusCode();
        }
    }

    //[Benchmark]
    //public async Task Login()
    //{
    //    var requestContent = new StringContent(
    //        "{ \"username\": \"admin\", \"password\": \"Admin@123\" }",
    //        System.Text.Encoding.UTF8,
    //        "application/json");

    //    var response = await _httpClient.PostAsync("auth/Login", requestContent);
    //    response.EnsureSuccessStatusCode();
    //}

    [Benchmark]
    public async Task GetLastSearch()
    {
        for (var i = 0; i < N; i++)
        {
            var response = await _httpClient.GetAsync("api/crypto/GetLastSearch");
            response.EnsureSuccessStatusCode();
        }
    }
}
