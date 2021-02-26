using LangAppApi.Domain.Queries;
using System.Collections.Generic;

namespace LangAppApi.Domain.Common
{
    public class PagingResponse<T> where T : class
    {
        public int TotalItems { get; set; }
        public List<T> Items { get; set; }
        public PaginationQuery Query { get; set; }

        public PagingResponse()
        {
        }

        public PagingResponse(int totalItems, List<T> items, PaginationQuery query)
        {
            TotalItems = totalItems;
            Items = items;
            Query = query;
        }
    }
}