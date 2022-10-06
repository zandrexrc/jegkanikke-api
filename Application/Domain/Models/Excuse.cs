using System.Text.Json.Serialization;

namespace Application.Domain.Models
{
    public class Excuse
    {
        [JsonPropertyName("id")]
        public string Id { get; set; }
        [JsonPropertyName("message")]
        public string Message { get; set; }
    }
}

