using Search.Models;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Web;

namespace Search
{
    public class SearchExecutor
    {
        public async Task<QueryResult> SearchAsync(SearchEngine searchEngine, string query, CancellationToken ct)
        {
            string searchQuery = searchEngine.URL + HttpUtility.UrlEncode(query);
            HttpClient httpClient = new HttpClient();
            HttpResponseMessage response = await httpClient.GetAsync(searchQuery, ct);
            string searchResult = await response.Content.ReadAsStringAsync();
            return new QueryResult(searchEngine, query, searchResult);
        }

        public async Task<QueryResult> SearchAsync(SearchEngine[] searchEngines, string query)
        {
            if (searchEngines == null || searchEngines.Length <= 0)
            {
                return null;
            }
            else
            {
                CancellationTokenSource cts = new CancellationTokenSource();
                IEnumerable<Task<QueryResult>> searchTasks = searchEngines.Select(searchEngine => SearchAsync(searchEngine, query, cts.Token));
                Task<QueryResult> searchResult = await Task.WhenAny(searchTasks);
                cts.Cancel();
                return await searchResult;
            }
        }
    }
}