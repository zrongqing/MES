using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using App.AppServices.DTOs;

namespace App.AppServices.Services;

public interface IApiClient
{
    /// <summary>
    /// Get请求
    /// </summary>
    /// <param name="url">api-url</param>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    Task<T?> GetAsync<T>(string url);

    /// <summary>
    /// Post请求
    /// </summary>
    /// <param name="url">api-url</param>
    /// <param name="data"></param>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    Task<T?> PostAsync<T>(string url, object data);

    Task<HttpResponseMessage> SendAsync(HttpRequestMessage request);
}

class ApiClient:IApiClient
{
    private readonly HttpClient _httpClient;
    private readonly JsonSerializerOptions _jsonOptions;

    public ApiClient(HttpClient httpClient)
    {
        _httpClient = httpClient;

        _jsonOptions = new JsonSerializerOptions
        {
            // PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            // PropertyNameCaseInsensitive = true,
            // WriteIndented = true
        };
    }

    /// <summary>
    /// Get请求
    /// </summary>
    /// <param name="url"></param>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public async Task<T?> GetAsync<T>(string url)
    {
        var response = await _httpClient.GetAsync(url);
        response.EnsureSuccessStatusCode();
        var json = await response.Content.ReadAsStringAsync();

        return JsonSerializer.Deserialize<T>(json, new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        });
    }

    public async Task<T?> PostAsync<T>(string url, object data)
    {
        var content = new StringContent(JsonSerializer.Serialize(data), Encoding.UTF8, "application/json");
        var response = await _httpClient.PostAsync(url, content);

        var statusCode = response.EnsureSuccessStatusCode();
        var json = await response.Content.ReadAsStringAsync();

        var request = JsonSerializer.Deserialize<T>(json, _jsonOptions);
        return request;
    }

    public Task<HttpResponseMessage> SendAsync(HttpRequestMessage request)
    {
        throw new NotImplementedException();
    }

    /// <summary>
    /// Post请求
    /// </summary>
    /// <param name="url">接口url</param>
    /// <param name="data">数据</param>
    /// <typeparam name="TRequest"></typeparam>
    /// <typeparam name="TResponse"></typeparam>
    /// <returns></returns>
    public async Task<TResponse?> PostAsync<TRequest, TResponse>(string url, TRequest data)
    {
        var content = new StringContent(JsonSerializer.Serialize(data), Encoding.UTF8, "application/json");
        var response = await _httpClient.PostAsync(url, content);

        var statusCode = response.EnsureSuccessStatusCode();
        var json = await response.Content.ReadAsStringAsync();

        return JsonSerializer.Deserialize<TResponse>(json, new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        });
    }

    /// <summary>
    /// 设置token
    /// </summary>
    /// <param name="token"></param>
    public void SetToken(string token)
    {
        _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
    }

    public async Task<string?> LoginAsync(LoginRequest request)
    {
        var json = JsonSerializer.Serialize(request);
        var content = new StringContent(json, Encoding.UTF8, "application/json");
        var response = await _httpClient.PostAsync("/api/account/login", content);

        if (!response.IsSuccessStatusCode)
            return null;

        var token = await response.Content.ReadAsStringAsync();
        return token.Trim('"'); // 假设返回的是纯 Token 字符串
    }
}