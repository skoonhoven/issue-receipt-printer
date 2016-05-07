namespace IssuePrinter.Core.Models
{
    public class IssueCard
    {
        public string Key { get; set; }
        public string Summary { get; set; }
        public Priority Priority { get; set; }
        public string Type { get; set; }
        public string Rank { get; set; }
        public string StoryPoints { get; set; }
    }


    public enum Priority
    {
        Minor = 4,
        Major = 3,
        Critical = 2,
        Blocker = 1
    }
}
