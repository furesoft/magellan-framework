using Sample.Features.Details.Model;

namespace Sample.Features.Details.ServiceProxies
{
    public class DetailsService : IDetailsService
    {
        public Person GetDetails(int id)
        {
            return new Person()
                        {
                            FirstName = "John",
                            LastName = "Smith " + id,
                            Id = id,
                            Title = "Mr",
                        };
        }
    }
}
