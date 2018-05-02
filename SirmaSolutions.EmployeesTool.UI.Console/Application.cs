using System.Collections.Generic;
using System.IO;
using System.Linq;
using SirmaSolutions.EmployeesTool.BLL.Entities;
using SirmaSolutions.EmployeesTool.BLL.Selectors.Interfaces;
using SirmaSolutions.EmployeesTool.BLL.TextParsers.Interfaces;

namespace SirmaSolutions.EmployeesTool.UI.Console
{
    public class Application
    {
        private IJobHistoryTextParser _jobHistoryParser;
        private ICommonProjectsCouplesSelector _commonProjectsCouplesSelector;

        public Application(IJobHistoryTextParser jobHistoryParser, ICommonProjectsCouplesSelector commonProjectsCouplesSelector)
        {
            _jobHistoryParser = jobHistoryParser;
            _commonProjectsCouplesSelector = commonProjectsCouplesSelector;
        }

        public void Run()
        {
            List<JobHistory> jobHistories;
            System.Console.WriteLine("Enter path to file:");
            string pathToFile = System.Console.ReadLine();

            while (!File.Exists(pathToFile))
            {
                System.Console.WriteLine("Enter correct path to file:");
                pathToFile = System.Console.ReadLine();
            }

            System.Console.WriteLine("Enter date format used in the file:");
            string dateFormat = System.Console.ReadLine();

            using (StreamReader reader = File.OpenText(pathToFile))
            {
                jobHistories = _jobHistoryParser.ParseFile(reader, dateFormat);
            }

            List<CommonProjectsResult> commonProjectsResults = _commonProjectsCouplesSelector.Select(jobHistories);
            if (commonProjectsResults.Count == 0)
            {
                System.Console.WriteLine("There wasn't a couple that worked toghether.");
            }
            else
            {
                var longestWorkingCouple = commonProjectsResults.First();

                System.Console.WriteLine($"Employee:{longestWorkingCouple.EmployeeId1}" +
                                         $"\nEmployee:{longestWorkingCouple.EmployeeId2}" +
                                         $"\nProjects:{string.Join(",", longestWorkingCouple.ProjectIds.Select(x => x.Key))}" +
                                         $"\nDays:{longestWorkingCouple.Days}");
            }
        }
    }
}
