using System.ComponentModel;

namespace SirmaSolutions.EmployeesTool.UI.WindowsTool
{
    public class Result
    {
        [DisplayName("Employee ID #1")]
        public int EmployeeId1 { get; set; }

        [DisplayName("Employee ID #2")]
        public int EmployeeId2 { get; set; }

        [DisplayName("Project ID")]
        public int ProjectId { get; set; }

        [DisplayName("Days worked")]
        public int Days { get; set; }
    }
}
