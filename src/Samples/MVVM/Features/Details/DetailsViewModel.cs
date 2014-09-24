using Magellan.Framework;
using Sample.Features.Details.Model;
using Sample.Features.Details.ServiceProxies;

namespace Sample.Features.Details
{
    public class DetailsViewModel : ViewModel
    {
        private readonly IDetailsService detailsService;

        public DetailsViewModel(IDetailsService detailsService)
        {
            this.detailsService = detailsService;
        }

        public void Initialize(int id)
        {
            Person = detailsService.GetDetails(id);
        }

        public Person Person { get; set; }
    }
}
