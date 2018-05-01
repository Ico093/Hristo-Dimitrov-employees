using SirmaSolutions.EmployeesTool.BLL.Entities;
using SirmaSolutions.EmployeesTool.BLL.TextParsers;

namespace SirmaSolutions.EmployeesTool.BLL.Tests.Proxies
{
    public class JobHistoryListParserProxy:JobHistoryListParser
    {
        public JobHistory ParseLineTest(string line, string dateFormat)
        {
            return base.ParseLine(line, dateFormat);
        }
    }
}
