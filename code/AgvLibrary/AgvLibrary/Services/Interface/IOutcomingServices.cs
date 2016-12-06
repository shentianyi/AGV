using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AgvLibrary.Services.Interface
{
  public interface IOutcomingServices
    {
        bool OutComingByUniqNr(string UniqNr);

        bool CreateOutcomingMovement(string SourcePosition, DateTime CreatedAt);
    }
}
