using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace taskDemo
{
    public class UserRepository : Repository<userinfo>
    {
        public UserRepository(DbContext db) : base(db)
        {
        }
    }
}
