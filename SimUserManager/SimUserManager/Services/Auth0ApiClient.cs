using System.Text;
using System.Text.Json;
using RestSharp;

namespace SimUserManager.Services;

public class Auth0ApiClient
{
    private readonly IConfiguration _config;
    private readonly RestClient _client;
    
    public Auth0ApiClient(IConfiguration configuration)
    {
        this._config = configuration;
        this._client = new RestClient();
    }

    public string? GetAccessToken()
    {
        var tokenEndpoint = $"{_config["Auth0Settings:AccessEndpoint"]}/oauth/token";
        var clientCredentials = Convert.ToBase64String(
            Encoding.UTF8.GetBytes(
                $"{_config["Auth0Settings:ClientId"]}:{_config["Auth0Settings:ClientSecret"]}"));
        var grant_type = "client_credentials";
        var client_id = _config["Auth0Settings:ClientId"];
        var client_secret = _config["Auth0Settings:ClientSecret"];
        var audience = $"{_config["Auth0Settings:AccessEndpoint"]}/api/v2/";
        
        var request = new RestRequest(tokenEndpoint,Method.Post);
        request.AddHeader("content-type", "application/x-www-form-urlencoded");
        request.AddParameter(
            "application/x-www-form-urlencoded",
            $"grant_type={grant_type}&client_id={client_id}&client_secret={client_secret}&audience={audience}",
            ParameterType.RequestBody);
        var response = _client.Execute(request); 
        var jsonlist = JsonSerializer.Deserialize<Auth0TokenResponse>(response.Content!);

        return jsonlist?.access_token;
    }

    private class Auth0TokenResponse
    {
        public required string access_token { get; set; }
    }
}