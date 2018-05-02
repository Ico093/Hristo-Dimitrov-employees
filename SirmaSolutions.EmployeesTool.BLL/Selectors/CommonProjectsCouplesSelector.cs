using System;
using System.Collections.Generic;
using System.Linq;
using SirmaSolutions.EmployeesTool.BLL.Entities;
using SirmaSolutions.EmployeesTool.BLL.Selectors.Interfaces;

namespace SirmaSolutions.EmployeesTool.BLL.Selectors
{
    public class CommonProjectsCouplesSelector: ICommonProjectsCouplesSelector
    {
        public List<CommonProjectsResult> Select(IList<JobHistory> jobHistories)
        {
            if (jobHistories.Count == 0)
            {
                return new List<CommonProjectsResult>();
            }

            List<JobHistory> passedJobHistories = new List<JobHistory>();
            Dictionary<string, CommonProjectsResult> commonProjectResults = new Dictionary<string, CommonProjectsResult>();

            jobHistories = jobHistories.OrderBy(x => x.DateFrom).ThenBy(x => x.DateTo).ToList();

            passedJobHistories.Add(jobHistories.First());

            foreach (JobHistory jobHistory in jobHistories.Skip(1))
            {
                if (passedJobHistories.First().ProjectId != jobHistory.ProjectId)
                {
                    passedJobHistories.Clear();
                    passedJobHistories.Add(jobHistory);
                }
                else
                {
                    int notRelevantCount = 0;

                    foreach (var passedJobHistory in passedJobHistories)
                    {
                        if (passedJobHistory.DateTo < jobHistory.DateFrom)
                        {
                            notRelevantCount++;
                        }
                        else
                        {
                            DateTime startDate = passedJobHistory.DateFrom > jobHistory.DateFrom
                                ? passedJobHistory.DateFrom
                                : jobHistory.DateFrom;
                            DateTime endDate = passedJobHistory.DateTo < jobHistory.DateTo
                                ? passedJobHistory.DateTo
                                : jobHistory.DateTo;
                            int days = (endDate - startDate).Days + 1;
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
            }

            return commonProjectResults.Select(x=>x.Value).OrderByDescending(x=>x.Days).ToList();
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
