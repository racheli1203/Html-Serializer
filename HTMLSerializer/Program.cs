using HTMLSerializer;
using System.Text.RegularExpressions;

namespace HTMLSerializer
{
    internal class Program
    {
        
        static async Task Main(string[] args)
        {
         
            HtmlElement rootElement=null;
            HtmlElement currentElement=null;
            var html = await Load("https://yourweddingcountdown.com/4fcbd");            
            var cleanHtml = new Regex("\\s").Replace(html, " ");
            var htmlLines = new Regex("<(.*?)>").Split(cleanHtml).Where(s => s.Length > 0);
            var htmlHelper = HtmlHelper.Instance;
            
            foreach (var line in htmlLines)
            {
                string firstWord = line.Split(" ")[0];
                if (firstWord == "/html")
                {
                    break;
                }

                else if (firstWord.StartsWith("/"))
                {
                    if (currentElement != null && currentElement.Parent != null)
                    {
                        currentElement = currentElement.Parent;
                    }
                }
                else if (htmlHelper.AllTags.Contains(firstWord) || htmlHelper.selfClosing.Contains(firstWord))
                {
                    HtmlElement element = new HtmlElement();
                    element.Name = firstWord;
                    var attributes = Regex.Matches(line, "([^\\s]*?)=\"(.*?)\"");
                    foreach (Match attribute in attributes)
                    {
                        var attributeName = attribute.Groups[1].Value;
                        var attributeValue = attribute.Groups[2].Value;
                        element.Attributes.Add(attribute.Value);

                        if (attributeName.ToLower() == "class")
                        {
                            var listClass = attributeValue.Split(' ').ToList();
                            foreach (string listItem in listClass)
                            {
                                element.Classes.Add(listItem);
                            }
                        }
                        else if (attributeName.ToLower() == "id")
                        {
                            var id = attributeValue;
                            element.Id = id;
                        }
                    }
                    if (currentElement == null)
                    {
                        rootElement = element;
                    }
                    else
                    {
                        currentElement.Children.Add(element);
                        element.Parent = currentElement;
                    }
                    if (htmlHelper.AllTags.Any(s => s == firstWord) && line[line.Length - 1] != '/')
                    {
                        currentElement = element;
                    }
                }
                else
                {
                    if (currentElement != null)
                    {
                        // Exclude lines with scriptv tags or other content you want to filter
                        if (!line.Contains("<script") && !line.Contains("(function("))
                        {
                            currentElement.InnerHtml += line;
                        }
                    }
                }
            }
            //$$("#inner #header .container")
            //div h6
            string selectorQuery = "div#banners";
            Selector selector = Selector.Parse(selectorQuery);
            var res = rootElement.GetElementsBySelector(selector);
            var duplicates = new HashSet<HtmlElement>(res);


           // PrintTree(rootElement, " ");
            foreach(var d in duplicates)
            {
                Console.WriteLine(d.Name);
            }


        }

        static async Task<string> Load(string url)
        {
            HttpClient client = new HttpClient();
            var response = await client.GetAsync(url);
            var html = await response.Content.ReadAsStringAsync();
            return html;
        }

        //static void PrintTree(HtmlElement root, string indent)
        //{
        //    Console.WriteLine();

        //    Console.WriteLine(indent + "Id: " + root.Id);

        //    Console.WriteLine(indent + "Tag name: " + root.Name);

        //    Console.WriteLine(indent + "Attributes:");
        //    if (root.Attributes != null)
        //    {
        //        foreach (var attribute in root.Attributes)
        //        {
        //            Console.WriteLine(indent + "* " + attribute);
        //        }
        //    }
        //    Console.WriteLine(indent + "Classes:");
        //    if (root.Classes != null)
        //    {
        //        foreach (var className in root.Classes)
        //        {
        //            Console.WriteLine(indent + "* " + className);
        //        }
        //    }

        //    Console.WriteLine(indent + "Inner HTML: " + root.InnerHtml);

        //    Console.WriteLine(indent + "Parent: " + (root.Parent != null ? root.Parent.Name : "None"));

        //    Console.WriteLine(indent + "Children:");
        //    foreach (var child in root.Children)
        //    {
        //        PrintTree(child, indent + "  ");
        //    }
        //}

    }
}