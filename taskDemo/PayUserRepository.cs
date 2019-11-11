using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace taskDemo
{
    public class PayUserRepository : Repository<Users>
    {
        public PayUserRepository(DbContext db) : base(db)
        {
        }
    }
}
