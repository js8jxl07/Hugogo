using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Hugogo.Model.Tables;

namespace Hugogo.IBusiness.Tables
{
    public interface IUserBusiness
    {
        User GetUserByUserId(string userId);
    }
}
