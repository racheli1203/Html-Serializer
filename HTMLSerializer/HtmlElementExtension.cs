using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HTMLSerializer
{
    public static class HtmlElementExtension
    {
        public static IEnumerable<HtmlElement> GetElementsBySelector(this HtmlElement element, Selector selector)
        {
            if (element == null || selector == null)
                yield break;

            var descendants = element.Descendants();

            foreach (var descendant in descendants)
            {
                if (selector.Equals(descendant))
                {
                    if (selector.Child != null)
                    {
                        var newList = descendant.GetElementsBySelector(selector.Child);
                        foreach (var item in newList)
                        {
                            yield return item;
                        }
                    }
                    else
                    {
                        yield return descendant;
                    }
                }
            }
        }
    }
}
