using System;
using System.Collections.Generic;

namespace LangAppApi.Domain.Queries
{
    public class PaginationQuery : BaseQuery

    {
    public int PageNumber { get; set; }
    public int PageSize { get; set; }
    public bool IsPaged { get; set; } = false;

    public Dictionary<string, string> ValidateQuery()
    {
        var result = new Dictionary<string, string>();

        if (!IsPaged) return result;

        if (PageNumber <= 0 || PageSize <= 0)
        {
            result.Add("PageNumber", "Le numéro de la page doit toujours être supérieur à 0");
            result.Add("PageSize", "La taille de la page doit toujours être supérieur à 0");
        }

        if (Convert.ToDateTime(OpenDate) > Convert.ToDateTime(CloseDate))
        {
            result.Add("OpenDate", "OpenDate must be less than or equal to CloseDate!");
        }

        return result;
    }

    public bool IsValid()
    {
        return ValidateQuery().Count <= 0;
    }
}
}