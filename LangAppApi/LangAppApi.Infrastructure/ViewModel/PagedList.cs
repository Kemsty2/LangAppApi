using System;
using System.Collections.Generic;
using LangAppApi.Domain.Common;
using LangAppApi.Domain.Queries;

namespace LangAppApi.Infrastructure.ViewModel
{
    public class PagedList<T> where T : class
    {
        public int TotalItems { get; set; }
        public PaginationQuery ParamsNext { get; set; }
        public PaginationQuery ParamsCurrent { get; set; }
        public PaginationQuery ParamsPrevious { get; set; }
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public List<T> List { get; set; }
        public int TotalPages => (int)Math.Ceiling(TotalItems / (double)PageSize);
        public bool HasPreviousPage => PageNumber > 1;
        public bool HasNextPage => PageNumber < TotalPages;
        public int NextPageNumber => HasNextPage ? PageNumber + 1 : TotalPages;
        public int PreviousPageNumber => HasPreviousPage ? PageNumber - 1 : 1;

        public PagedList()
        {
        }

        public PagedList(PagingResponse<T> response)
        {
            PageNumber = response.Query.PageNumber;
            PageSize = response.Query.PageSize;
            TotalItems = response.TotalItems;
            List = response.Items;

            ParamsCurrent = response.Query;
            ParamsNext = new PaginationQuery
            {
                PageNumber = NextPageNumber,
                PageSize = response.Query.PageSize,
                Search = response.Query.Search,
                OpenDate = response.Query.OpenDate,
                CloseDate = response.Query.CloseDate
            };
            ParamsPrevious = new PaginationQuery
            {
                PageNumber = PreviousPageNumber,
                PageSize = response.Query.PageSize,
                Search = response.Query.Search,
                OpenDate = response.Query.OpenDate,
                CloseDate = response.Query.CloseDate
            };
        }

        public PagingHeader GetHeader()
        {
            return new PagingHeader(TotalItems, PageNumber, PageSize, TotalPages, HasPreviousPage, HasNextPage);
        }
    }
}