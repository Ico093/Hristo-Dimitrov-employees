using System;

namespace SirmaSolutions.EmployeesTool.BLL.Entities
{
    public class JobHistory
    {
        private int _employeeId;
        private int _projectId;
        private DateTime _dateFrom;
        private DateTime _dateTo;

        public JobHistory(int employeeId, int projectId, DateTime dateFrom, DateTime dateTo)
        {
            _employeeId = employeeId;
            _projectId = projectId;
            _dateFrom = dateFrom;
            _dateTo = dateTo;
        }
    }
}
