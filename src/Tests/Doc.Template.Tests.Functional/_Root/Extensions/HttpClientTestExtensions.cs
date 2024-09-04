using Doc.Pulse.Tests.Functional._Root.Enums;
using Newtonsoft.Json;
using System.Text;
using Doc.Pulse.Core.Entities._Kernel;

namespace Doc.Pulse.Tests.Functional._Root.Extensions;

public static class HttpClientTestExtensions
{
    private static readonly HttpClientTestExtensionVerbs[] MustHaveBodyList = new HttpClientTestExtensionVerbs[2]
    {
            HttpClientTestExtensionVerbs.POST,
            HttpClientTestExtensionVerbs.PUT
    };

    public static async Task<ApiResponse<T>?> SendRequestAndDeserializeApiResponse<T>(this HttpClient _client, string url, HttpClientTestExtensionVerbs httpVerb = HttpClientTestExtensionVerbs.GET, StringContent? jsonContent = null)
    {
        if (jsonContent == null && MustHaveBodyList.Contains(httpVerb))
        {
            throw new ArgumentNullException("jsonContent");
        }

        return JsonConvert.DeserializeObject<ApiResponse<T>>(await (httpVerb switch
        {
            HttpClientTestExtensionVerbs.GET => await _client.GetAsync(url),
            HttpClientTestExtensionVerbs.POST => await _client.PostAsync(url, jsonContent),
            HttpClientTestExtensionVerbs.PUT => await _client.PutAsync(url, jsonContent),
            HttpClientTestExtensionVerbs.DELETE => await _client.DeleteAsync(url),
            _ => await _client.GetAsync(url),
        }).Content.ReadAsStringAsync());
    }

    public static async Task<T?> SendRequestAndDeserialize<T>(this HttpClient _client, string url, HttpClientTestExtensionVerbs httpVerb = HttpClientTestExtensionVerbs.GET, StringContent? jsonContent = null)
    {
        if (jsonContent == null && MustHaveBodyList.Contains(httpVerb))
        {
            throw new ArgumentNullException("jsonContent");
        }

        return JsonConvert.DeserializeObject<T>(await (httpVerb switch
        {
            HttpClientTestExtensionVerbs.GET => await _client.GetAsync(url),
            HttpClientTestExtensionVerbs.POST => await _client.PostAsync(url, jsonContent),
            HttpClientTestExtensionVerbs.PUT => await _client.PutAsync(url, jsonContent),
            HttpClientTestExtensionVerbs.DELETE => await _client.DeleteAsync(url),
            _ => await _client.GetAsync(url),
        }).Content.ReadAsStringAsync());
    }

    public static async Task<T?> SendRequestAndDeserialize<T, Y>(this HttpClient _client, string url, HttpClientTestExtensionVerbs httpVerb = HttpClientTestExtensionVerbs.GET, Y? command = null) where Y : class
    {
        if (command == null && MustHaveBodyList.Contains(httpVerb))
        {
            throw new ArgumentNullException("command");
        }

        StringContent content = new(System.Text.Json.JsonSerializer.Serialize(command), Encoding.UTF8, "application/json");
        var resp = await (httpVerb switch
        {
            HttpClientTestExtensionVerbs.GET => await _client.GetAsync(url),
            HttpClientTestExtensionVerbs.POST => await _client.PostAsync(url, content),
            HttpClientTestExtensionVerbs.PUT => await _client.PutAsync(url, content),
            HttpClientTestExtensionVerbs.DELETE => await _client.DeleteAsync(url),
            _ => await _client.GetAsync(url),
        }).Content.ReadAsStringAsync();

        return JsonConvert.DeserializeObject<T>(resp);
    }

    public static async Task<ApiResponse<T>?> SendRequestAndDeserializeApiResponse<T>(this HttpClient _client, string httpVerb, string url, StringContent? jsonContent = null)
    {
        HttpResponseMessage httpResponseMessage = new HttpResponseMessage();
        if (httpVerb == "get")
        {
            httpResponseMessage = await _client.GetAsync(url);
        }
        else if (httpVerb == "post" && jsonContent != null)
        {
            httpResponseMessage = await _client.PostAsync(url, jsonContent);
        }
        else if (httpVerb == "delete")
        {
            httpResponseMessage = await _client.DeleteAsync(url);
        }
        else if (httpVerb == "put" && jsonContent != null)
        {
            httpResponseMessage = await _client.PutAsync(url, jsonContent);
        }

        return JsonConvert.DeserializeObject<ApiResponse<T>>(await httpResponseMessage.Content.ReadAsStringAsync());
    }

    public static async Task<ApiResponse<T>?> GetAndDeserializeApiResponse<T>(this HttpClient _client, string url)
    {
        return JsonConvert.DeserializeObject<ApiResponse<T>>(await (await _client.GetAsync(url)).Content.ReadAsStringAsync());
    }

    public static async Task<T?> GetAndDeserializeApiResponseResult<T>(this HttpClient _client, string url)
    {
        ApiResponse<T>? apiResponse = JsonConvert.DeserializeObject<ApiResponse<T>>(await (await _client.GetAsync(url)).Content.ReadAsStringAsync());
        return (apiResponse == null) ? default(T) : apiResponse.Result;
    }

    public static async Task<T?> PutAndDeserializeApiResponseResult<T>(this HttpClient _client, string url, string jsonString)
    {
        StringContent content = new StringContent(jsonString.ToString(), Encoding.UTF8, "application/json");
        ApiResponse<T>? apiResponse = JsonConvert.DeserializeObject<ApiResponse<T>>(await (await _client.PutAsync(url, content)).Content.ReadAsStringAsync());
        return (apiResponse == null) ? default(T) : apiResponse.Result;
    }

    public static async Task<T?> PostAndDeserializeApiResponseResult<T>(this HttpClient _client, string url, string jsonString)
    {
        StringContent content = new StringContent(jsonString.ToString(), Encoding.UTF8, "application/json");
        ApiResponse<T>? apiResponse = JsonConvert.DeserializeObject<ApiResponse<T>>(await (await _client.PostAsync(url, content)).Content.ReadAsStringAsync());
        return (apiResponse == null) ? default(T) : apiResponse.Result;
    }

    public static async Task<T?> DeleteAndDeserializeApiResponseResult<T>(this HttpClient _client, string url)
    {
        ApiResponse<T>? apiResponse = JsonConvert.DeserializeObject<ApiResponse<T>>(await (await _client.DeleteAsync(url)).Content.ReadAsStringAsync());
        return (apiResponse == null) ? default(T) : apiResponse.Result;
    }
}
