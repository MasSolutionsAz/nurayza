using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Xml;

namespace ILoveBaku.MVC.Extensions
{
    public static class XmlExtension
    {
        public static string GetInnerText(this XmlDocument xmlDocument, string tagName)
        {
            return xmlDocument.GetElementsByTagName(tagName)?[0]?.InnerText;
        }
    }
}
