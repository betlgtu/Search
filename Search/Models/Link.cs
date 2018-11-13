namespace Search.Models
{
    public class Link
    {
        public string URL { get; set; }
        public string Title { get; set; }

        public Link(string url, string title)
        {
            URL = url;
            if (!URL.StartsWith("http://") &&
                !URL.StartsWith("https://"))
                URL = "http://" + URL;
            Title = title;
        }
    }
}