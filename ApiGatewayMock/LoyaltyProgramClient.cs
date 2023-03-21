using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace ApiGatewayMock;

public class LoyaltyProgramClient
{
    private readonly HttpClient _client;
    public LoyaltyProgramClient(HttpClient client)
    {
        _client = client;
    }

    public async Task<HttpResponseMessage> QueryUser(string id)
    {
        return await _client.GetAsync($"users/{int.Parse(id)}");
    }

    public async Task<HttpResponseMessage> RegisterUser(string name)
    {
        var user = new{name, Settings = new{}};
        return await _client.PostAsync("/users", CreateBody(user));
    }

    public async Task<HttpResponseMessage> UpdateUser(dynamic user)
    {
        return await _client.PutAsync($"users/{user.Id}", CreateBody(user));
    }

    private static StringContent CreateBody(object user)
    {
        var json = JsonSerializer.Serialize(user);
        return new StringContent(json, Encoding.UTF8, "application/json");  
    }

}
