using System.Collections.Generic;

namespace LangAppApi.Infrastructure.ViewModel
{
    public class OutPutModel<T> where T : class
    {
        public PagingHeader Paging { get; set; }
        public List<T> Data { get; set; }

        public OutPutModel()
        {
        }

        public OutPutModel(PagingHeader paging, List<T> data)
        {
            Paging = paging;
            Data = data;
        }
    }
}