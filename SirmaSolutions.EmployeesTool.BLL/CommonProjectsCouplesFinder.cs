using SirmaSolutions.EmployeesTool.BLL.Entities;
using System.Collections.Generic;
using System.Linq;

namespace SirmaSolutions.EmployeesTool.BLL
{
    public class CommonProjectsCouplesFinder
    {
        public List<CommonProjectsResult> Find(IList<JobHistory> jobHistories)
        {
            if (jobHistories.Count == 0)
            {
                return new List<CommonProjectsResult>();
            }

            Dictionary<string, CommonProjectsResult> commonProjectResults = new Dictionary<string, CommonProjectsResult>();

            jobHistories = jobHistories.OrderBy(x => x.DateFrom).ThenBy(x => x.DateTo).ToList();

            JobHistory currentJobHistory = jobHistories[0];
            jobHistories.RemoveAt(0);

            foreach (JobHistory jobHistory in jobHistories)
            {
                int days;

                if (currentJobHistory.ProjectId != jobHistory.ProjectId)
                {
                    //Not fully inclusive
                    if (!(currentJobHistory.DateTo >= jobHistory.DateTo))
                    {
                        currentJobHistory = jobHistory;
                    }

                    continue;
                }

                //Fully inclusive
                if (currentJobHistory.DateTo >= jobHistory.DateTo)
                {
                    days = (jobHistory.DateTo - jobHistory.DateFrom).Days;
                    continue;
                }

                //Partially inclusive
                if (currentJobHistory.DateTo <= jobHistory.DateFrom)
                {
                    days = (jobHistory.DateFrom - currentJobHistory.DateTo).Days;
                }


                string key = CreateKey(currentJobHistory.EmployeeId, jobHistory.EmployeeId);

                if (commonProjectResults.ContainsKey(key))
                {
                    commonProjectResults[key].AddDays(days);
                }
                else
                {
                    commonProjectResults.Add(new CommonProjectsResult(currentJobHistory.EmployeeId, jobHistory.EmployeeId, currentJobHistory.ProjectId, days));
                }

                currentJobHistory = jobHistory;
            }
        }

        protected string CreateKey(int key1, int key2)
        {
            if (key1 > key2)
            {
                return $"{key2}|{key1}";
            }
            else
            {
                return $"{key1}|{key2}";
            }
        }
    }
}
