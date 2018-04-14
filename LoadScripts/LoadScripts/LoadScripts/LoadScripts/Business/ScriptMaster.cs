using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LoadScripts.Business
{
    public class ScriptMaster
    {
        private MyDbContext context = new MyDbContext();
        List<Script> scripts;
        
        public ScriptMaster()
        {
        }
        
        public List<Script> LoadScriptsFromDB()
        {
            scripts = context.Scripts.Where(s => s.Active).OrderBy(o=>o.ScriptName).ToList();

            if (scripts == null) scripts = new List<Script>();
            scripts.Insert(0, new Script() {
                ScriptCode = "0",
                ScriptName = "ALL"
            });

            scripts.Insert(1, new Script()
            {
                ScriptCode = "26009",
                ScriptName = "NIFTYBANK"
            });

            scripts.Insert(2, new Script()
            {
                ScriptCode = "20000",
                ScriptName = "NIFTY"
            });

            return scripts;
        }

    }
}
