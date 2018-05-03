using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Windows;
using Microsoft.Win32;
using SirmaSolutions.EmployeesTool.BLL.Entities;
using SirmaSolutions.EmployeesTool.BLL.Selectors;
using SirmaSolutions.EmployeesTool.BLL.Selectors.Interfaces;
using SirmaSolutions.EmployeesTool.BLL.TextParsers;
using SirmaSolutions.EmployeesTool.BLL.TextParsers.Interfaces;

namespace SirmaSolutions.EmployeesTool.UI.WindowsTool
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void ButtonBase_OnClick(object sender, RoutedEventArgs e)
        {
            OpenFileDialog dialog = new OpenFileDialog();

            if (dialog.ShowDialog() == true)
            {
                FilePathTextBox.Text = dialog.FileName;
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            ErrorMessage.Text = string.Empty;
            IJobHistoryTextParser jobHistoryTextParser = new JobHistoryListParser();
            ICommonProjectsCouplesSelector commonProjectsCouplesSelector = new CommonProjectsCouplesSelector();

            if (File.Exists(FilePathTextBox.Text))
            {
                List<JobHistory> jobHistories;

                using (StreamReader reader = File.OpenText(FilePathTextBox.Text))
                {
                    jobHistories = jobHistoryTextParser.ParseFile(reader, dateFormatTextBox.Text);
                }

                List<CommonProjectsResult> results = commonProjectsCouplesSelector.Select(jobHistories);

                ResultsDataGrid.ItemsSource = results.SelectMany(x => x.ProjectIds.Select(xs =>
                    new Result()
                    {
                        EmployeeId1 = x.EmployeeId1,
                        EmployeeId2 = x.EmployeeId2,
                        ProjectId = xs.Key,
                        Days = xs.Value
                    })).OrderByDescending(x => x.Days);
            }
            else
            {
                ErrorMessage.Text = "File doesn't exist";
            }
        }

        private void ResultsDataGrid_AutoGeneratingColumn(object sender, System.Windows.Controls.DataGridAutoGeneratingColumnEventArgs e)
        {
            var displayName = GetPropertyDisplayName(e.PropertyDescriptor);

            if (!string.IsNullOrEmpty(displayName))
            {
                e.Column.Header = displayName;
            }
        }


        public static string GetPropertyDisplayName(object descriptor)
        {
            var pd = descriptor as PropertyDescriptor;

            if (pd != null)
            {
                // Check for DisplayName attribute and set the column header accordingly
                var displayName = pd.Attributes[typeof(DisplayNameAttribute)] as DisplayNameAttribute;

                if (displayName != null && displayName != DisplayNameAttribute.Default)
                {
                    return displayName.DisplayName;
                }

            }
            else
            {
                var pi = descriptor as PropertyInfo;

                if (pi != null)
                {
                    // Check for DisplayName attribute and set the column header accordingly
                    Object[] attributes = pi.GetCustomAttributes(typeof(DisplayNameAttribute), true);
                    for (int i = 0; i < attributes.Length; ++i)
                    {
                        var displayName = attributes[i] as DisplayNameAttribute;
                        if (displayName != null && displayName != DisplayNameAttribute.Default)
                        {
                            return displayName.DisplayName;
                        }
                    }
                }
            }

            return null;
        }
    }
}
