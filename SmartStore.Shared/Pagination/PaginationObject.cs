using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartStore.Shared.Pagination
{
    public class PaginationObject<T>
    {
        public PaginationObject(List<T> dataList, int totalCount , int pageIndex = 0 , int pageCount = 0)
        {
            DataList = dataList;
            TotalCount = totalCount;
            PageIndex = pageIndex;
            PageCount = pageCount;
        }

        public List<T> DataList { get; set; }
        public int TotalCount { get; }
        public int PageIndex { get; set; }
        public int PageCount { get; set; }
    }
}
