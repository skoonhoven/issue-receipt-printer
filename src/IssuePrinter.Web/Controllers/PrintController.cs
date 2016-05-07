using System.Configuration;
using System.Web.Mvc;
using IssuePrinter.Core;

namespace IssuePrinter.Web.Controllers
{
    public class PrintController : Controller
    {
        private readonly PrintService _printService;

        public PrintController()
        {
            var ticketPrintServiceConfig = new TicketPrintServiceConfig
            {
                JiraHost = ConfigurationManager.AppSettings["JiraHost"],
                JiraUsername = ConfigurationManager.AppSettings["JiraUsername"],
                JiraPassword = ConfigurationManager.AppSettings["JiraPassword"],
                PrinterName = ConfigurationManager.AppSettings["PrinterName"],
            };

            _printService = new PrintService(ticketPrintServiceConfig);
        }        

        // GET: /Print/Issue/MF-800
        public ActionResult Issue(string key)
        {          
            _printService.PrintIssue(key);
            return null;   
        }

        // GET: /Print/Sprint/488
        public ActionResult Sprint(string key)
        {
            _printService.PrintSprintIssues(key);
            return null;
        }
   }
}
