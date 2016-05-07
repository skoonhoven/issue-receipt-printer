using System.Collections.Generic;
using IssuePrinter.Core.Models;

namespace IssuePrinter.Core
{
    public class PrintService
    {

        private readonly IProjectManagementServiceClient _projectManagementServiceClient;
        private readonly IIssuePrinter _issuePrinter;

        public PrintService(TicketPrintServiceConfig ticketPrintServiceConfig)
        {
            _projectManagementServiceClient = new JiraClient(ticketPrintServiceConfig.JiraHost, ticketPrintServiceConfig.JiraUsername, ticketPrintServiceConfig.JiraPassword);
            _issuePrinter = new WindowsIssuePrinter(ticketPrintServiceConfig.PrinterName);
        }

        public void PrintIssue(string key)
        {

            var issue = _projectManagementServiceClient.GetIssue(key);

            if (issue != null)
            {
                _issuePrinter.PrintIssue(issue);
            }
        }

        public void PrintSprintIssues(string sprintKey)
        {
            var issues = _projectManagementServiceClient.GetIssuesForSprint(sprintKey);

            PrintIssues(issues);
        }

        public void PrintJqlIssues(string jql)
        {
            var issues = _projectManagementServiceClient.GetIssuesFromQueryLanguage(jql);

            PrintIssues(issues);
        }

        private void PrintIssues(IEnumerable<IssueCard> issues)
        {
            if (issues != null)
            {
                _issuePrinter.PrintIssues(issues);
            }
        }
    }
}
