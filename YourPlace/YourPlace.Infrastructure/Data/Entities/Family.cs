using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YourPlace.Infrastructure.Data.Entities
{
    [NotMapped]
    public class Family
    {
        public int TotalCount { get; set; }
        public int AdultsCount { get; set; }
        public int ChildrenCount { get; set; }

        public Family(int totalCount)
        {
            TotalCount = totalCount;
        }
    }
}
