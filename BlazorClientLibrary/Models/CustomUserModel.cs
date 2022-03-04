using Microsoft.AspNetCore.Components.WebAssembly.Authentication;
using System.Text.Json.Serialization;

namespace BlazorClientLibrary.Models
{
    public class CustomUserModel : RemoteUserAccount
    {
        [JsonPropertyName("groups")]
        public string[] Groups { get; set; }

        [JsonPropertyName("roles")]
        public string[] Roles { get; set; }

        [JsonPropertyName("wids")]
        public string[] Wids { get; set; }
    }
}
