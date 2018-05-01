﻿using SirmaSolutions.EmployeesTool.BLL.Entities;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;

namespace SirmaSolutions.EmployeesTool.BLL.TextParsers
{
    public class JobHistoryListParser
    {
        public List<JobHistory> ParseFile(StreamReader stream, string dateFormat)
        {
            List<JobHistory> jobHistoryList = new List<JobHistory>();

            string line;

            try
            {
                while ((line = stream.ReadLine()) != null)
                {
                    JobHistory parsedJobHistory = ParseLine(line, dateFormat);
                    jobHistoryList.Add(parsedJobHistory);
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Line {jobHistoryList.Count}: Problem occured while parsing.", ex);
            }

            return jobHistoryList;
        }

        protected JobHistory ParseLine(string line, string dateFormat)
        {
            string[] splittedString = line.Split(',').Select(x=>x.Trim()).ToArray<string>();
            int employeeId, projectId;
            DateTime dateFrom, dateTo;

            if (splittedString.Length != 4)
            {
                throw new ArgumentException("Invalid number of entries.");
            }

            if (!int.TryParse(splittedString[0], out employeeId))
            {
                throw new ArgumentException("Invalid employee id.");
            }

            if (!int.TryParse(splittedString[1], out projectId))
            {
                throw new ArgumentException("Invalid project id.");
            }

            if (!DateTime.TryParseExact(splittedString[2], dateFormat, CultureInfo.InvariantCulture, DateTimeStyles.None, out dateFrom))
            {
                throw new FormatException("Invalid date from.");
            }

            if (!DateTime.TryParseExact(splittedString[3], dateFormat, CultureInfo.InvariantCulture, DateTimeStyles.None, out dateTo))
            {
                throw new FormatException("Invalid date to.");
            }

            return new JobHistory(employeeId, projectId, dateFrom, dateTo);
        }
    }
}
