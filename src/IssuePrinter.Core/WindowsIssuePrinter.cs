using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Printing;
using IssuePrinter.Core.Helpers;
using IssuePrinter.Core.Models;

namespace IssuePrinter.Core
{
    public interface IIssuePrinter
    {
        void PrintIssue(IssueCard issueCard);
        void PrintIssues(IEnumerable<IssueCard> issues);
    }

    public class WindowsIssuePrinter : IIssuePrinter
    {
        private readonly PrintDocument _printDocument;
        private readonly Queue<IssueCard> _pendingIssues;

        public WindowsIssuePrinter(string printerName)
        {
            _printDocument = NewPrinterDocument(printerName);
            _printDocument.PrintPage += IssueFormatter;
            _pendingIssues = new Queue<IssueCard>();
        }

        private static PrintDocument NewPrinterDocument(string printerName)
        {
            var pd = new PrintDocument
            {
                PrinterSettings =
                {
                    PrinterName = printerName
                },
                DefaultPageSettings =
                {
                    Landscape = true,
                    Margins = new Margins(0,0,0,0)
                }
            };

            return pd;
        }

        private void IssueFormatter(object sender, PrintPageEventArgs ev)
        {
            if (_pendingIssues.Count > 0)
            {
                var issue = _pendingIssues.Dequeue();

                PrintKey(ev, issue);
                PrintSeparator(ev, 70, 400);
                PrintSummary(ev, issue);
                PrintPriority(ev, issue);
                PrintRank(ev, issue);
                PrintStoryPoints(ev, issue);
                PrintType(ev, issue);

            }

            ev.HasMorePages = _pendingIssues.Count > 0;    
        }

        private static void PrintKey(PrintPageEventArgs ev, IssueCard issueCard)
        {
            var titleFont = new Font(FontFamily.GenericSansSerif, 36);
            ev.Graphics.DrawString(issueCard.Key, titleFont, Brushes.Black, 0, 10);      
        }

        private static void PrintSeparator(PrintPageEventArgs ev, int yOffset, int lenght)
        {
            var boldBlackPen = new Pen(Color.Black, 3);

            ev.Graphics.DrawLine(boldBlackPen, 0, yOffset, lenght, yOffset);
        }

        private static void PrintSummary(PrintPageEventArgs ev, IssueCard issueCard)
        {
            var summaryFont = new Font(FontFamily.GenericSansSerif, 16);

            var wrappedSummary = IssueFormattingHelper.Wrap(issueCard.Summary, 35);
            var lineHeight = summaryFont.GetHeight(ev.Graphics);

            float offsetY = 80;

            foreach (var line in wrappedSummary)
            {
                ev.Graphics.DrawString(line, summaryFont, Brushes.Black, 0, offsetY);
                offsetY += lineHeight;
            }            
        }

       
        private static void PrintPriority(PrintPageEventArgs ev, IssueCard issue)
        {
            var resourceName = PriorityToString(issue);
            
            var resourcePath = String.Format("Resources.priority.{0}.png", resourceName);

            var priorityIcon = IssueFormattingHelper.OpenEmbeddedImage(resourcePath);

            ev.Graphics.DrawImage(priorityIcon,350,20);

        }

        private static string PriorityToString(IssueCard issue)
        {
            switch (issue.Priority)
            {
                case Priority.Minor:
                    return "minor";
                case Priority.Major:
                    return "major";
                case Priority.Critical:
                    return "critical";
                case Priority.Blocker:
                    return "blocker";
                default:
                    throw new ArgumentException();            
            }
        }

        private static void PrintRank(PrintPageEventArgs ev, IssueCard issue)
        {
            var font = new Font(FontFamily.GenericSansSerif, 12);
            ev.Graphics.DrawString("#"+issue.Rank, font, Brushes.Black, 0, 250);  
        }

        private static void PrintStoryPoints(PrintPageEventArgs ev, IssueCard issue)
        {
            var titleFont = new Font(FontFamily.GenericSansSerif, 12);
            ev.Graphics.DrawString("SP " + issue.StoryPoints, titleFont, Brushes.Black, 350, 250);
        }

        private static void PrintType(PrintPageEventArgs ev, IssueCard issue)
        {
            var titleFont = new Font(FontFamily.GenericSansSerif, 12);
            ev.Graphics.DrawString(issue.Type, titleFont, Brushes.Black, 175, 250);
        }

        public void PrintIssue(IssueCard issueCard)
        {
            if (issueCard == null) return;

            _pendingIssues.Enqueue(issueCard);
            _printDocument.Print();
        }

        public void PrintIssues(IEnumerable<IssueCard> issues)
        {
            if (issues == null) return;

            foreach (var issue in issues)
            {
                _pendingIssues.Enqueue(issue);    
            }
            
            _printDocument.Print();
        }
    }
}
