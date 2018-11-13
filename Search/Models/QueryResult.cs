namespace Search.Models
{
    public class QueryResult
    {
        public SearchEngine SearchEngine { get; set; }
        public string Query { get; set; }
        public string SearchResult { get; set; }

        public QueryResult(SearchEngine searchEngine, string query, string searchResult)
        {
            SearchEngine = searchEngine;
            Query = query;
            SearchResult = searchResult;
        }
    }
}