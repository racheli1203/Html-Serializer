using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace HTMLSerializer
{
    public class HtmlHelper
    {
        private readonly static HtmlHelper _instance = new HtmlHelper();
        public static HtmlHelper Instance => _instance;
        public string[] AllTags { get; private set; }
        public string[] selfClosing { get; private set; }

       private HtmlHelper()
        {
            try
            {
                // Assuming the "Tags" directory is in the same directory as the executable.
                string basePath = AppDomain.CurrentDomain.BaseDirectory;

                string allTagsJson = File.ReadAllText(Path.Combine(basePath, "./HtmlTags.json"));
                string selfClosingTagsJson = File.ReadAllText(Path.Combine(basePath, "./HtmlVoidTags.json"));

                AllTags = JsonSerializer.Deserialize<string[]>(allTagsJson);
                selfClosing = JsonSerializer.Deserialize<string[]>(selfClosingTagsJson);
            }
            catch (Exception ex)
            {
                Console.WriteLine("An error occurred while loading HTML tags: " + ex.Message);
                AllTags = new string[0];
                selfClosing = new string[0];
            }
        }
    }
}
