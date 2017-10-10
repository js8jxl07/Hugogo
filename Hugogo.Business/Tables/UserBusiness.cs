using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Hugogo.IBusiness.Tables;
using Hugogo.Model.Tables;

namespace Hugogo.Business.Tables
{
    public class UserBusiness : IUserBusiness
    {
        public User GetUserByUserId(string userId)
        {
            return new User{Id = 1,RealName = "ssss"};
        }
    }
}
