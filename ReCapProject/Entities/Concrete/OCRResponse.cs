using Newtonsoft.Json;

namespace Entities
{
    public class OcrResponse
    {
        [JsonProperty("Field 4A")]
        public string Field4A { get; set; }

        [JsonProperty("Field 4D")]
        public string Field4D { get; set; }

        [JsonProperty("Field 5")]
        public string Field5 { get; set; }

        public string Error { get; set; }

        public bool IsSuccess => string.IsNullOrEmpty(Error);
    }
}
