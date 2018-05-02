using System;

namespace SirmaSolutions.EmployeesTool.BLL.Entities
{
    public class JobHistory
    {
        public JobHistory(int employeeId, int projectId, DateTime dateFrom, DateTime dateTo)
        {
            EmployeeId = employeeId;
            ProjectId = projectId;
            DateFrom = dateFrom;
            DateTo = dateTo;
        }

        public int EmployeeId { get; }
        public int ProjectId { get; }
        public DateTime DateFrom { get; }
        public DateTime DateTo { get; }
    }
}
