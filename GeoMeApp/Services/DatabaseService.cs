using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeoMeApp.Services
{
    internal class DatabaseService : Data.DataContext, IDatabaseService
    {
        public DatabaseService(string dataFilePath) : base(dataFilePath) 
        {
            Debug.Assert(dataFilePath != null && dataFilePath.Length > 0);
        }
    }
}
