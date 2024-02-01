using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YourPlace.Infrastructure.Data.Entities;

namespace YourPlace.Core.Contracts
{
    public interface IUsers
    {
        public Task CreateAccount(User user);
        public Task LogIn(User user);
        public Task DeleteAccount(User user);
        public Task UpdateAccount(User editedUser);
        public Task ResetPassword(User user, string newPassword);

    }
}
