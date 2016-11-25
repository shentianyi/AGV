using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AgvLibrary.Data
{
    public interface IDataContextFactory
    {
        System.Data.Linq.DataContext Context { get; }
        void SaveAll();
    }
}
