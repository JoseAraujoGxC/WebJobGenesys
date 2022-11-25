using Newtonsoft.Json;
using RestSharp.Serializers;
using System.IO;

namespace WebJobGxCGenesys.Models
{
    public class JsonDotNetSerializer : ISerializer {

        private readonly Newtonsoft.Json.JsonSerializer _serializer;
        public string DateFormat { get; set; }
        public string RootElement { get; set; }
        public string Namespace { get; set; }
        public string ContentType { get; set; }

        public JsonDotNetSerializer() {
            ContentType = "application/json";
            _serializer = new Newtonsoft.Json.JsonSerializer {
                MissingMemberHandling = MissingMemberHandling.Ignore,
                NullValueHandling = NullValueHandling.Include,
                DefaultValueHandling = DefaultValueHandling.Include
            };
        }

        public JsonDotNetSerializer(Newtonsoft.Json.JsonSerializer serializer) {
            ContentType = "application/json";
            _serializer = serializer;
        }

        public string Serialize(object obj) {
            using (var stringWriter = new StringWriter()) {
                using (var jsonTextWriter = new JsonTextWriter(stringWriter)) {
                    jsonTextWriter.Formatting = Formatting.Indented;
                    jsonTextWriter.QuoteChar = '"';

                    _serializer.Serialize(jsonTextWriter, obj);

                    var result = stringWriter.ToString();
                    return result;
                }
            }
        }

    }

}
