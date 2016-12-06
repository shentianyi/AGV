using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AgvLibrary.Model.Message;

namespace AgvLibrary.Services.Interface
{
  public interface IOutcomingServices
    {
        BasicMessage OutComingByUniqNr(string UniqNr);

        bool CreateOutcomingMovement(string SourcePosition, DateTime CreatedAt);
    }
}
