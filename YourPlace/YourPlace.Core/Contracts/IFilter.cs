using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YourPlace.Infrastructure.Data.Entities;

namespace YourPlace.Core.Contracts
{
    public interface IFilter
    {

        public Task<List<Hotel>> FilterByCountry(string country);
        public Task<List<Hotel>> FilterByPeopleCount(int count);
        public Task<List<Hotel>> FilterByPrice(decimal price);
        public Task<List<Hotel>> FilterByDates(DateOnly arrivingDate, DateOnly leavingDate);
    }
}
