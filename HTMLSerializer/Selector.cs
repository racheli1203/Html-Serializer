using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HTMLSerializer
{
    public class Selector
    {
        public String TagName { get; set; }
        public String Id { get; set; }
        public List<string> Classes { get; set; } = new List<string>();
        public Selector Parent { get; set; }
        public Selector Child { get; set; }

        public static Selector Parse(string query)
        {
            var htmlHelper = HtmlHelper.Instance;
            string[] partsQuery = query.Split(' ');
            Selector root = null;
            Selector currentSelector = null;

            foreach (string partQ in partsQuery)
            {
                var part = AddSpaceBeforeHashAndDot(partQ);
                string[] parts = part.Split(' ');

                if (root == null)
                {
                    root = new Selector();
                    currentSelector = root;
                }
                else
                {
                    currentSelector.Child = new Selector();
                    currentSelector.Child.Parent = currentSelector;
                    currentSelector = currentSelector.Child;
                }

                foreach (string subPart in parts)
                {
                    if (subPart.StartsWith("#"))
                    {
                        currentSelector.Id = subPart.Substring(1);
                    }
                    else if (subPart.StartsWith("."))
                    {
                        currentSelector.Classes.Add(subPart.Substring(1));
                    }
                    else if (htmlHelper.selfClosing.Contains(subPart) || htmlHelper.AllTags.Contains(subPart))
                    {
                        currentSelector.TagName = subPart;
                    }
                }
            }

            return root;
        }
        public override bool Equals(object? obj)
        {
            if (obj is HtmlElement)
            {
                HtmlElement? element = obj as HtmlElement;


                if (element != null)
                {

                    bool tagNameMatches = string.IsNullOrEmpty(TagName) || TagName == element.Name;
                    bool idMatches = string.IsNullOrEmpty(Id) || Id == element.Id;
                    bool classesMatch = Classes.All(className => element.Classes.Contains(className));

                    return tagNameMatches && idMatches && classesMatch;
                }

            }
            return false;
        }



        static string AddSpaceBeforeHashAndDot(string input)
        {
            StringBuilder result = new StringBuilder();

            foreach (char character in input)
            {
                if (character == '#' || character == '.')
                {
                    result.Append(' ').Append(character);
                }
                else
                {
                    result.Append(character);
                }
            }

            return result.ToString();
        }

        public static void PrintTree2(Selector root)
        {
            Selector current = root;
            while (current != null)
            {
                Console.WriteLine("TagName: " + current.TagName);
                Console.WriteLine("Id: " + current.Id);
                Console.WriteLine("Classes:");
                foreach (var className in current.Classes)
                {
                    Console.WriteLine("- " + className);
                }
                Console.WriteLine("Parent: " + (current.Parent != null ? current.Parent.TagName : "None"));
                Console.WriteLine("Child: " + (current.Child != null ? current.Child.TagName : "None"));
                

                current = current.Child;
            }
        }
    }
}
