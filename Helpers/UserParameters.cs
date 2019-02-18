using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Friendster.Helpers
{
    public class UserParameters
    {
        private const int MAX_PAGE_SIZE = 50;

        public int PageNumber { get; set; } = 1;

        private int _pageSize = 10;

        public int PageSize
        {
            get { return _pageSize; }
            set
            {
                _pageSize = value > MAX_PAGE_SIZE ? MAX_PAGE_SIZE : value;
            }
        }

        public int UserId { get; set; }
        public string Gender { get; set; }
        public int MinimumAge { get; set; } = 18;
        public int MaximumAge { get; set; } = 99;
        public string OrderBy { get; set; }
    }
}
