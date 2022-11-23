using System;
using Elasticsearch.Net;
using Nest;


namespace Elastic
{
    public class Payment
    {
        public double UnitPrice { get; set; }
        public string Description { get; set; }
        public int Quantity { get; set; }
        public string Country { get; set; }
        public string InvoiceNo { get; set; }
        public string InvoiceDate { get; set; }
        public string StockCode { get; set; }
    }
    internal class Program
    {
        static void Main(string[] args)
        {
            var cloudId = "Learning:dXMtY2VudHJhbDEuZ2NwLmNsb3VkLmVzLmlvOjQ0MyRlZmU2Y2UwMGQwY2U0ZDA1YjgwZDBiMDBmMWE3MDliOCQ0NzY4OWMxZjU4YTc0NTBkOTQ5OTVjOGU2NThlYzE0OQ==";
            var credentials = new BasicAuthenticationCredentials("elastic", "riNNe9R6GkPPnSuDaHxwEjXx");
            var settings = new ConnectionSettings(cloudId, credentials);
            settings.DefaultFieldNameInferrer(p => p);
            settings.ThrowExceptions();
            IElasticClient client = new ElasticClient(settings);
            var response = client.Search<Payment>(t => t.Index("ecommerce_data"));
            var payments = response.Hits.Select(t => t.Source);
        }
    }
}