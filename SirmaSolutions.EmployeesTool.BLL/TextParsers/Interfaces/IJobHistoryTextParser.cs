using System.Collections.Generic;
using System.IO;
using SirmaSolutions.EmployeesTool.BLL.Entities;

namespace SirmaSolutions.EmployeesTool.BLL.TextParsers.Interfaces
{
    public interface IJobHistoryTextParser
    {
        List<JobHistory> ParseFile(StreamReader stream, string dateFormat);
    }
}
