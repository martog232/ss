using System.Collections.Generic;

namespace SS.Core.Utilities.Contract
{
    public interface IFileUtility
    {
        public IEnumerable<string> ReadXlsxFile(string fileName);
    }
}
