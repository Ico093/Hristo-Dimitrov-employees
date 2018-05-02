using System;
using System.Collections.Generic;

namespace SirmaSolutions.EmployeesTool.BLL.Entities
{
    public class CommonProjectsResult
    {
        public CommonProjectsResult(int employeeId1, int employeeId2, int projectId, int days)
        {
            EmployeeId1 = employeeId1;
            EmployeeId2 = employeeId2;
            ProjectIds = new List<int>();
            ProjectIds.Add(projectId);

            if (days <= 0)
            {
                throw new ArgumentException("Days can't be negative number.");
            }
            Days = days;
        }

        public int EmployeeId1 { get; }
        public int EmployeeId2 { get; }
        public List<int> ProjectIds { get; }
        public int Days { get; private set; }

        public void AddDays(int days)
        {
            Days += days;
        }
    }
}
