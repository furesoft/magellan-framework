using Sample.Features.Details.Model;

namespace Sample.Features.Details.ServiceProxies
{
    public interface IDetailsService
    {
        Person GetDetails(int id);
    }
}
