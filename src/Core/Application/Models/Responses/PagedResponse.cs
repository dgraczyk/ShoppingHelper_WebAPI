using System;
using System.Collections.Generic;
using System.Linq;

namespace Application.Models.Responses
{
    public class PagedResponse<T>
    {
        public int CurrentPage { get; }
        public int TotalPages { get; }
        public int PageSize { get; }
        public int TotalCount { get; }

        public bool HasPrevious => CurrentPage > 1;
        public bool HasNext => CurrentPage < TotalPages;

        public IEnumerable<T> Items { get; }

        public PagedResponse(IEnumerable<T> source, int pageNumber, int pageSize)
        {
            TotalCount = source.Count();
            PageSize = pageSize;
            CurrentPage = pageNumber;
            TotalPages = (int)Math.Ceiling(TotalCount / (double)pageSize);

            var pagedSource = new List<T>();
            var pagedItems = source.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToList();

            pagedSource.AddRange(pagedItems);

            this.Items = pagedItems;
        }
    }
}
