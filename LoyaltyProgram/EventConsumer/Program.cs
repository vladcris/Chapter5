
using System.Text.Json;

var start = await GetStartIdFromDatastore();
var end = 100;
var uri = new Uri($"http://host.docker.internal:5505/events?start={start}&end={end}"); /// special offers events

var client = new HttpClient();
client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));

using HttpResponseMessage resp = await client.GetAsync( uri );

await ProcessEvents(await resp.Content.ReadAsStreamAsync());
await SaveStartIdToDataStore(start);


Task<long> GetStartIdFromDatastore() => Task.FromResult(0L);

Task SaveStartIdToDataStore(long startId) => Task.CompletedTask;

async Task ProcessEvents(Stream content)
{
    //var string res = await JsonSerializer.DeserializeAsync<SpecialOfferEvent[]>(content);

    var events = await JsonSerializer.DeserializeAsync<SpecialOfferEvent[]>(content, new JsonSerializerOptions { PropertyNameCaseInsensitive = true }) ?? new SpecialOfferEvent[0];
    
    foreach (var raisedEvent in events)
    {
        System.Console.WriteLine(raisedEvent);
        start = Math.Max(start, raisedEvent.SequenceNumber + 1);
    }
}

public record SpecialOfferEvent(long SequenceNumber, 
                                DateTimeOffset OccuredAt,
                                string Name, 
                                object Content);