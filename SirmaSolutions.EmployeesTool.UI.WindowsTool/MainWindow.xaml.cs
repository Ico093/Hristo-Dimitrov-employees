using System.Collections.Generic;
using System.IO;
using System.Linq;
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
                using (StreamReader reader = File.OpenText(FilePathTextBox.Text))
                {
                    List<JobHistory> jobHistories = jobHistoryTextParser.ParseFile(reader, dateFormatTextBox.Text);
                    List<CommonProjectsResult> results = commonProjectsCouplesSelector.Select(jobHistories);

                    ResultsDataGrid.ItemsSource = results.SelectMany(x => x.ProjectIds.Select(xs =>
                        new Result()
                        {
                            EmployeeId1 = x.EmployeeId1,
                            EmployeeId2 = x.EmployeeId2,
                            ProjectId = xs.Key,
                            Days = xs.Value
                        })).OrderByDescending(x=>x.Days);
                }
            }
            else
            {
                ErrorMessage.Text = "File doesn't exist";
            }
        }

        private void CalculateDays()
        {

        }
    }
}
