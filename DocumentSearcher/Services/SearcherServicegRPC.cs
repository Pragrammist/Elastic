using Grpc.Core;
using GrpcGreeter;

namespace DocumentSearcher.Services
{
    public class SearcherServicegRPC : GrpcGreeter.SearcherServicegRPC.SearcherServicegRPCBase
    {
        SearchService _service;
        public SearcherServicegRPC(SearchService service)
        {
            
        }

        public override Task<SearchOutput> Search(QueryInput query, ServerCallContext context)
        {
            var output = new SearchOutput { };
            


            var search = _service.SearchPayment(query.Query);


            return Task.FromResult<SearchOutput>();
        }
    }
}
