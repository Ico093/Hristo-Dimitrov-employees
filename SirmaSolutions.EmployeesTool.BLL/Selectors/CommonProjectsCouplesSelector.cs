using System;
using System.Collections.Generic;
using System.Linq;
using SirmaSolutions.EmployeesTool.BLL.Entities;
using SirmaSolutions.EmployeesTool.BLL.Selectors.Interfaces;

namespace SirmaSolutions.EmployeesTool.BLL.Selectors
{
    public class CommonProjectsCouplesSelector : ICommonProjectsCouplesSelector
    {
        /// <summary>
        /// Gives you the list of all couples working toghether on a same project in the same time.
        /// </summary>
        /// <param name="jobHistories">List of all records of job history</param>
        /// <returns></returns>
        public List<CommonProjectsResult> Select(IList<JobHistory> jobHistories)
        {
            if (jobHistories.Count <= 1)
            {
                return new List<CommonProjectsResult>();
            }

            List<JobHistory> passedJobHistories = new List<JobHistory>();
            Dictionary<string, CommonProjectsResult> commonProjectResults = new Dictionary<string, CommonProjectsResult>();

            jobHistories = jobHistories.OrderBy(x => x.DateFrom).ThenBy(x => x.DateTo).ToList();

            passedJobHistories.Add(jobHistories.First());

            foreach (JobHistory jobHistory in jobHistories.Skip(1))
            {
                int notRelevantCount = 0;

                foreach (var passedJobHistory in passedJobHistories)
                {
                    if (passedJobHistory.DateTo < jobHistory.DateFrom)
                    {
                        notRelevantCount++;
                    }
                    else if ((passedJobHistory.EmployeeId != jobHistory.EmployeeId) && (passedJobHistory.ProjectId == jobHistory.ProjectId))
                    {
                        int days = GetDaysDifference(passedJobHistory, jobHistory);
                        string key = CreateKey(passedJobHistory.EmployeeId, jobHistory.EmployeeId);

                        if (commonProjectResults.ContainsKey(key))
                        {
                            commonProjectResults[key].AddDays(jobHistory.ProjectId, days);
                        }
                        else
                        {
                            commonProjectResults.Add(key,
                                new CommonProjectsResult(passedJobHistory.EmployeeId, jobHistory.EmployeeId,
                                    jobHistory.ProjectId, days));
                        }
                    }
                }

                passedJobHistories.Add(jobHistory);
                passedJobHistories.RemoveRange(0, notRelevantCount);
            }

            return commonProjectResults.Select(x => x.Value).OrderByDescending(x => x.Days).ToList();
        }

        /// <summary>
        /// Gets the difference in days from the 2 passed dates.
        /// </summary>
        /// <param name="date1">Date for comparison</param>
        /// <param name="date2">Date to compate</param>
        /// <returns></returns>
        protected int GetDaysDifference(JobHistory date1, JobHistory date2)
        {
            DateTime startDate = date1.DateFrom > date2.DateFrom
                                ? date1.DateFrom
                                : date2.DateFrom;
            DateTime endDate = date1.DateTo < date2.DateTo
                ? date1.DateTo
                : date2.DateTo;

            return (endDate - startDate).Days + 1;
        }

        /// <summary>
        /// Creates unique key for every 2 int keys
        /// </summary>
        /// <param name="key1">First key</param>
        /// <param name="key2">Second key</param>
        /// <returns>Unique key</returns>
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
