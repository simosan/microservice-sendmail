using System.ComponentModel.DataAnnotations;
using System.Text;
using System.Text.Json;
using Azure;
using RestSharp;
using SimUserManager.Models;
using SimUserManager.Services;
using Amazon.XRay.Recorder.Core;

namespace SimUserManager.Services;

public class Auth0UserService
{
    private readonly IConfiguration _config;
    private readonly RestClient _client;

    public Auth0UserService(IConfiguration configuration)
    {
        this._config = configuration;
        this._client = new RestClient();
    }

    public int Auth0UserCreate(UserViewModel user, string token)
    {
        //Auth0に登録するユーザ属性
        var uemail = user.Email;
        var uname = user.Lastname + user.Firstname;
        var utitle = user.PositionName;
        var udepartment = user.Department;
        var usection = user.Section;
        var uid = user.UserId;
        var uinitpasswd = $"{_config["Auth0Settings:InitPasswd"]}";

        var userEndpoint = $"{_config["Auth0Settings:AccessEndpoint"]}/api/v2/users";
        var request = new RestRequest(userEndpoint, Method.Post);
        request.AddHeader("Accept", "application/json");
        request.AddHeader("Authorization", $"Bearer {token}");
        request.AddBody(new
        {
            email = uemail,
            user_metadata = new
            {
                department = udepartment,
                section = usection,
                title = utitle,
                userid = uid,
                firstname = user.Firstname,
                lastname = user.Lastname
            },
            blocked = false,
            email_verified = false,
            name = uname,
            connection = "Username-Password-Authentication",
            password = uinitpasswd,
            verify_email = false
        });

        var response = this._client.Execute(request);

        return (int)response.StatusCode;
    }

    public string? Auth0UserIdObtain(Users user, string token)
    {
        AWSXRayRecorder.Instance.BeginSubsegment(_config["Auth0Settings:AccessEndpoint"]);
        //Auth0に登録されているユーザのid（Auth0Id）を取得するためにemailを使う
        var uemail = user.Email;
        var userEndpoint = $"{_config["Auth0Settings:AccessEndpoint"]}/api/v2/users";
        var request = new RestRequest(userEndpoint, Method.Get);
        request.AddParameter("q", $"email:{uemail}");
        request.AddParameter("search_engine", "v3");
        request.AddHeader("Authorization", $"Bearer {token}");

        var response = this._client.Execute(request);
        AWSXRayRecorder.Instance.EndSubsegment();
        if ((int)response.StatusCode == 200)
        {
            var doc = JsonDocument.Parse(response.Content!);
            var uid = doc.RootElement
               .EnumerateArray()
               .Select(s => s.GetProperty("user_id").GetString())
               .FirstOrDefault();
            
            return uid;
        }

        return response.StatusCode.ToString();
    }


    public int Auth0UserDelete(string uid, string token)
    {
        var userEndpoint = $"{_config["Auth0Settings:AccessEndpoint"]}/api/v2/users/{uid}";
        var request = new RestRequest(userEndpoint, Method.Delete);
        request.AddHeader("Authorization", $"Bearer {token}");
        var response = this._client.Execute(request);

        return (int)response.StatusCode;
    }
}
