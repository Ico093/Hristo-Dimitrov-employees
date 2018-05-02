using System.Collections.Generic;
using SirmaSolutions.EmployeesTool.BLL.Entities;

namespace SirmaSolutions.EmployeesTool.BLL.Selectors.Interfaces
{
    public interface ICommonProjectsCouplesSelector
    {
        List<CommonProjectsResult> Select(IList<JobHistory> jobHistories);
    }
}
