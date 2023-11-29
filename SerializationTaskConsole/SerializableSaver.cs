using SerializationTaskConsole.Model;
using System;
using System.Text;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Unicode;

namespace SerializationTaskConsole
{
    public class SerializableSaver<T>
    {
        JsonSerializerOptions options = new JsonSerializerOptions
        {
            WriteIndented = true,
            Encoder = JavaScriptEncoder.Create(UnicodeRanges.All),
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            PropertyNameCaseInsensitive = true
        };
        public void Save(T obj, string path) 
        {
            var result = JsonSerializer.Serialize(obj, options);
            File.WriteAllText(path, result, Encoding.UTF8);
        }

        public T Load(string path) 
        {
            using (StreamReader st = new StreamReader(path))
            {
                string json = st.ReadToEnd();
                T result = JsonSerializer.Deserialize<T>(json, options);
                return result;
            }

        }
    }
}
