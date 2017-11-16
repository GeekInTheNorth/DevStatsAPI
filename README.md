# DevStats

DevStats is project focused on making data and KPIs visible to my own development team.  In some cases, code is specific to our instance of Jira when interrogating data.  Ideally this would be configurable, but the current priority is the functionality rather than usability beyond my own company's use.

Front end functionality includes:

 - User Accounts
 - Sprint Planner
 - Most Valuable Professional voting
 - KPI Reports
 - Defect Reports
 - API Audit
 - API Execution (To allow for manual triggering of Jira Webhook end points)
 - Jira State Checker

End Points for Jira Webhooks:

 - Story Creation (Creates default sub-tasks.)
 - Story Update (Updates Team Ownership, Updates Work-Logs Data, Updates Defect Data)
 - Story Deletion (Updates Defect Data)
 - Sub-Task Update (Forces state correction of parent stories)

## Azure Webapp Settings that need setting up

| Setting Type | Name | Notes |
| ------ | ------ | ------ |
| Connection String | DevStatSQL | Set to local SQL Instance with integrated security in source control |
| Application Setting | JiraApiRoot | Blank in source control |
| Application Setting | JiraUserName | Blank in source control, currently handled as unencrypted in web.config |
| Application Setting | JiraPassword | Blank in source control, currently handled as unencrypted in web.config |
| Application Setting | JiraProjects | Blank in source control |
| Application Setting | JiraServiceDeskGroup | Blank in source control, used to identify id a bug is Internal or External. To be Deprecated. |
| Application Setting | EmailHost | Email Host, used for user account management |
| Application Setting | EmailPort | Email Port, used for user account management |
| Application Setting | EmailUserName | Email User Name, used for user account management, currently handled as unencrypted in web.config |
| Application Setting | EmailPassword | Email Password, used for user account management, currently handled as unencrypted in web.config |
| Application Setting | AllowedIPAddresses | N/A in source control, Can be comma separated to secure specific controller actions |
| Application Setting | AhaApiRoot | Blank in source control |
| Application Setting | AhaApiKey | Blank in source control |

## Jira Cloud Webhook Setup
In all of the following cases, the URL must be prefixed with the domain of the DevStats instance.  The suggested JQL is slightly different in my own depoyed instance as it includes project filters.

### Story Creation
URL: /api/jira/story/create/${issue.key}
Event jql: issuetype in standardIssueTypes() AND issueType != "Task"
Event Options: Issue - Created

### Story Update
URL: api/jira/story/update/${issue.key}
Event jql: issuetype in standardIssueTypes()
Event Options: Issue - Updated

### Story Delete
URL: api/jira/story/delete/${issue.key}
Event jql: issuetype in standardIssueTypes()
Event Options: Issue - Deleted

### Subtask Update
URL: api/jira/subtask/update/${issue.key}
Event jql: N/A
Event Options: N/A
Workflow Transition: Add a Post-Function that executes a "Generic Event", this can target a WebHook.  I've used this so this event only gets triggered when moving between states.