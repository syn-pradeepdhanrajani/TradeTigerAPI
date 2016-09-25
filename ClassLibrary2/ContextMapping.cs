using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassLibrary2
{
    public class ContextMapping : MyDbContext
    {
        public ContextMapping() : base() { }
        public DbSet<Nifty> StockQuotes { get; set; }
    }
}
