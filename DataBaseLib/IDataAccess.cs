using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataBaseLib
{
    public interface IDataAccess
    {
        public List<object[]> GetData(string table, string whereReq = null, string sortBy = null,
            params string[] columns);

        public List<object[]> GetData(string request);
        public void RawRequest(string request);

        public Task RawRequestAsync(string request);
    }
}
