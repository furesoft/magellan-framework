using System.Collections.Generic;
using Sample.Features.Search.Model;

namespace Sample.Features.Search.ServiceProxies
{
    public interface ISearchService
    {
        List<SearchResult> Search(string text);
    }
}
