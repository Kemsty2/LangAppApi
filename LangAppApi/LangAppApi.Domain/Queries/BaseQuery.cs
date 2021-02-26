using System;

namespace LangAppApi.Domain.Queries
{
    public class BaseQuery
    {
        /// <summary>
        ///
        /// </summary>
        public DateTime? OpenDate { get; set; }

        /// <summary>
        ///
        /// </summary>
        public DateTime? CloseDate { get; set; }

        /// <summary>
        ///
        /// </summary>
        public string Search { get; set; }
    }
}