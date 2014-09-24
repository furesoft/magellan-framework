using System.Collections.Generic;
using System.Linq;
using Sample.Features.Search.Model;

namespace Sample.Features.Search.ServiceProxies
{
    public class SearchService : ISearchService
    {
        public List<SearchResult> Search(string text)
        {
            return Enumerable.Range(1, 20)
                .Select(x => new SearchResult
                {
                    Id = x,
                    FirstName = "John",
                    LastName = "Smith " + x,
                    Title = "Mr"
                })
                .ToList();
        }
    }
}
