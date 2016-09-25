using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ClassLibrary2;
using System.Data.Entity;

namespace ClassLibrary2
{
    public class Stocks
    {
        MyDbContext context = new MyDbContext();


        public Stocks()
        {
        }

        public List<Nifty> GetNiftyData()
        {
            List<Nifty> nfqoutes = context.Nifties.ToList() ;
            return nfqoutes;

        }
    }
}
