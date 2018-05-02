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
            ProjectIds = new Dictionary<int, int>();
            ProjectIds.Add(projectId, days);

            if (days <= 0)
            {
                throw new ArgumentException("Days can't be negative number.");
            }

            Days = days;
        }

        public int EmployeeId1 { get; }
        public int EmployeeId2 { get; }
        public Dictionary<int, int> ProjectIds { get; }
        public int Days { get; private set; }

        public void AddDays(int projectId, int days)
        {
            if (ProjectIds.ContainsKey(projectId))
            {
                ProjectIds[projectId] += days;
            }
            else
            {
                ProjectIds.Add(projectId,days);
            }

            Days += days;
        }
    }
}
