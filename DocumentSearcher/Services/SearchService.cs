using Nest;
using Serilog;
using DocumentSearcher.Entities;
using Elasticsearch.Net;
using Newtonsoft.Json.Linq;
using System.Text;
using Microsoft.Extensions.Primitives;
using SmartFormat.Extensions;
using SmartFormat;

namespace DocumentSearcher.Services
{
    public class SearchService
    {
        IElasticClient _elastic;
        public SearchService(IElasticClient elastic)
        {
            _elastic = elastic;
        }
        public IEnumerable<Payment> SearchPayment(string query, int page = 0)
        {
            var command = @"
            {
              ""query"": {
                ""bool"": {
                  ""should"": [
                    {
                      ""match_phrase"": {
                        ""Description"": ""{0}""
                      }
                    },
                    {
                      ""match_phrase"": {
                        ""Country"": ""{0}""
                      }
                    },
                    {
                      ""match"": {
                        ""Description"": {
                          ""query"": ""{0}"",
                          ""minimum_should_match"": 3
                        }
                      }
                    }
                  ]
                }
              }
            }";
            //command = InsertQuery(command, query);
            //var res = _elastic.LowLevel.Search<StringResponse>("ecommerce_data", PostData.String(command));
            //if (!res.Success)
            //{
            //    Log.Error(res.DebugInformation);
            //    throw new InvalidOperationException("search are failure");
            //}

            //var obj = JObject.Parse(res.Body);
            //var values = obj["hits"]?["hits"]?.Select(s => s?["_source"]?.ToObject<Payment>()) ?? Enumerable.Empty<Payment>();

            //return values;


            var searchResult = _elastic.Search<Payment>(s => s.Index("ecommerce_data").Query(d => d.Bool(b => b.Should(SearchPaymentShould(query)))));
            if (!searchResult.IsValid)
            {
                var error = searchResult.ServerError.ToString();
                Log.Error(error);
                throw new InvalidOperationException($"Some exception with elastric search\n{error}");
            }
            
            Log.Information("Returned hits");
            return searchResult.Hits.Select(t => t.Source);
        }


        Func<QueryContainerDescriptor<Payment>, QueryContainer>[] SearchPaymentShould(string query)
        {

            return new Func<QueryContainerDescriptor<Payment>, QueryContainer>[]
            {
                q => MathchDescription(q, query),
                q => MatchPraseMathchDescription(q, query),
                q => MatchCountry(q, query)
            };
        }
        QueryContainer MathchDescription(QueryContainerDescriptor<Payment> query, string value) => query.Match(m => m.Field(t => t.Description).Query(value));
        QueryContainer MatchPraseMathchDescription(QueryContainerDescriptor<Payment> query, string value) => query.MatchPhrase(m => m.Field(t => t.Description).Query(value));
        QueryContainer MatchCountry(QueryContainerDescriptor<Payment> query, string value) => query.MatchPhrase(m => m.Field(t => t.Country).Query(value));
        string InsertQuery(string command,string query) => command.Replace("{0}", query); 
    }
}
