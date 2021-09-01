using Newtonsoft.Json;

namespace FishardsBot {
    public struct ConfigJson {
        [JsonProperty("token")]
        public string Token { get; private set; }
        [JsonProperty("prefixes")]
        public string[] Prefixes { get; private set; }
    }
}