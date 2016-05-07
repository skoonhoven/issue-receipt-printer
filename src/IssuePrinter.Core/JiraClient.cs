using System;
using System.Collections.Generic;
using System.Linq;
using Atlassian.Jira;
using IssuePrinter.Core.Models;

namespace IssuePrinter.Core
{
    public interface IProjectManagementServiceClient
    {
        IssueCard GetIssue(string key);
        IEnumerable<IssueCard> GetIssuesForSprint(string sprintId);
        IEnumerable<IssueCard> GetIssuesFromQueryLanguage(string jql);
    }
    
    public class JiraClient : IProjectManagementServiceClient
    {
        private readonly Jira _jiraClient;
        private readonly List<IssueType> _issueTypes ;
        
        public JiraClient(string host, string username, string password)
        {
            _jiraClient = Jira.CreateRestClient(host, username, password);
            _jiraClient.Debug = true;
            _jiraClient.MaxIssuesPerRequest = 100;            
            _issueTypes = _jiraClient.GetIssueTypes().ToList();
        }


        public IssueCard GetIssue(string key)
        {

            try
            {
                var jiraIssue = (from i in _jiraClient.Issues
                                 where i.Key == key
                                 select i).FirstOrDefault();
                if (jiraIssue != null)
                {
                    return MapIssueToIssueCard(jiraIssue);
                }
            }
            catch (Exception e)
            {
                Console.Write(e);
            }
            

            return null;
        }
        
        public IEnumerable<IssueCard> GetIssuesForSprint(string sprintId)
        {
            var selectSprintJQL = String.Format("Sprint={0} ORDER BY Rank",sprintId);
            return GetIssuesFromQueryLanguage(selectSprintJQL);
        }

        public IEnumerable<IssueCard> GetIssuesFromQueryLanguage(string jql)
        {
            var issues = _jiraClient.GetIssuesFromJql(jql);

            var rank = 1;
            return issues.Select(sprintIssue => MapIssueToIssueCard(sprintIssue, rank++)).ToList();
        }

        private IssueCard MapIssueToIssueCard(Issue issue, int rank = 0)
        {
            var storyPoints = RetrieveStoryPoints(issue);

            return new IssueCard
            {
                Key = issue.Key.ToString(),
                Summary = issue.Summary,
                Priority = (Priority)int.Parse(issue.Priority.ToString()),
                StoryPoints = storyPoints,
                Type = GetIssueType(issue),
                Rank = rank.ToString()

            };
        }

        private static string RetrieveStoryPoints(Issue issue)
        {
            var storyPoints = Constants.EmptyStoryPoints;

            try
            {
                var storyPointsValue = issue[Constants.CfLabelStoryPoints];

                if (storyPointsValue != null)
                {
                    storyPoints = storyPointsValue.ToString();
                }
                
            }
            catch (Exception e)
            {
                
            }

            return storyPoints;
        }

        private string GetIssueType(Issue issue)
        {
            var issueTypeResult = _issueTypes.FirstOrDefault(it => it.Id == issue.Type.ToString());
            
            if (issueTypeResult != null)
            {
                return issueTypeResult.Name;
            }
            else
            {
                return issue.Type.ToString();
            }
        }
    }
}
