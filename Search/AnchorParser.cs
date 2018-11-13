using CsQuery;
using Search.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;

namespace Search
{
    public class AnchorParser
    {
        private Regex UrlRegex = new Regex(@"^http(s)?://([\w-]+.)+[\w-]+(/[\w- ./?%&=])?$");
        private Regex LinkRegex = new Regex(@"^([\w-]+.)+[\w-]+(/[\w- ./?%&=])?$");

        public Link[] ParseLinks(string domain, string html)
        {
            if(string.IsNullOrEmpty(html))
                return new Link[0];

            CQ cq = CQ.Create(html);

            List<Link> links = new List<Link>();
            
            foreach (IDomObject obj in cq.Find("cite"))
            {
                string url = GetInnerText(obj);
                if (!IsValid(url))
                    continue;
                AddLink(links, domain, url);
            }

            foreach (IDomObject obj in cq.Find("a"))
            {
                string url = obj.GetAttribute("href");

                if (!IsValid(url))
                    continue;
                
                AddLink(links, domain, url);
            }

            return links.ToArray();
        }

        private void AddLink(List<Link> links, string domain, string url)
        {
            if (url.StartsWith("/"))
                url = domain + url;

            Link link = new Link(url, url);
            links.Add(link);
        }

        private bool IsValid(string url)
        {
            if (string.IsNullOrEmpty(url))
                return false;
            string decodedURL = HttpUtility.UrlDecode(url);
            return Uri.IsWellFormedUriString(decodedURL, UriKind.RelativeOrAbsolute);
        }

        private string GetInnerText(IDomObject obj)
        {
            if (obj is null)
                return null;

            if (!obj.HasChildren)
                return obj.NodeValue;

            StringBuilder sb = new StringBuilder();

            foreach (IDomObject childNode in obj.ChildNodes)
            {
                string childText = GetInnerText(childNode);
                sb.Append(childText);
            }

            return sb.ToString();
        }
    }
}