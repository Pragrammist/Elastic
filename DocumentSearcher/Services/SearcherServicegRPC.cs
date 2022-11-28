using Grpc.Core;
using GrpcSearch;


namespace DocumentSearcher.Services
{
    public class SearcherServicegRPC : GrpcSearch.SearchService.SearchServiceBase
    {
        SearchService _service;
        public SearcherServicegRPC(SearchService service)
        {
            _service = service;
        }

        public override Task<SearchOutput> Search(QueryInput request, ServerCallContext context)
        {
            var searchResult = _service.SearchPayment(request.Query);
            
            var res = new SearchOutput();
            var listRes = res.Payments;
            foreach(var el in searchResult){
                var payment = new Payment
                {
                    Country = el.Country,
                    Description = el.Description,
                    InvoiceDate = el.InvoiceDate,
                    InvoiceNo = el.InvoiceNo,
                    Quantity = el.Quantity,
                    UnitPrice = el.UnitPrice,
                    StockCode = el.StockCode,
                };
                listRes.Add(payment);
            }
            
            return Task.FromResult<SearchOutput>(res);
        }
    }
}
