# Jira Issue Printer #
If your team uses both Jira and a physical scrumboard to visualise work, you probably have spend some time on either writing down the issues on post-its, or printing, cutting and ordering the tickets. 

Printing single tickets and sprints from Jira became never so easy as of now. With the use of a receipt printer like the [Citizen CT-S2000](http://www.citizen-systems.com/us/printer/pos/ct-s2000) you are now saved the hassle of cutting your own issues, or deciphering the handwritten post it's of your fellow colleagues.  

[Insert video & image here]

# Installation #

1. Hook up a receipt printer to your network (I strongly suggest to use one with a build in cutter)
1. Copy web.config.template to web.config
    1. Add the configurations for the printer and credentials for a Jira account. An account with read only access is good enough (Replace the variables marked ${variablename})
1. Build & Publish the code somewhere, make sure it has access and rights to the just installed receipt printer
1. Test by accessing http://{webapplication}/print/issue/{known-issue-key} (Format = [A-Z]+-[0-9]+)
1. If everthing works out, the issue should roll out the printer soon.

# Usage #

The webservice exposes two api methods:

- /print/issue/{issue-key} (Format = [A-Z]+-[0-9]+)
- /print/sprint/{sprint-id} (Format = [0-9]+)

The bookmarklet can be used to print directly from jira. Currently it works only for printing single issues only. 

1. Configure the host of the webservice in the bookmarklet itself
1. Transform the javascript into valid bookmarklet code using this [converter](http://mrcoles.com/bookmarklet/)
1. Install the bookmarklet in your favourite browser.

Printing a whole sprint requires the following steps:

1. Open your board
1. Go to Sprint Reports
1. Select the sprint you need and get the sprintid from the url
   - EG https://{jira.host.nl}/secure/RapidBoard.jspa?rapidView=32&view=reporting&chart=sprintRetrospective&sprint=965
1. Go to http://{issueprinter.webservice.nl}/print/sprint/{sprintid}
1. Retrieve your issue cards from the printer.

Note: Printing issues in a batch  is limited to a 100 tickets maximum, to avoid to much waste during an accidental print action

# Architecture #

The system consists of a web service and a receipt printer. The web service acts upon commando's given, fetching the issue data from Jira, do the rendering into pages, and sends the issues as small pages to the receipt printer. 