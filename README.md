# DevStats

DevStats is project focused on making data and KPIs visible to my own development team.  In some cases, code is specific to our instance of Jira when interrogating data.  Ideally this would be configurable, but the current priority is the functionality rather than usability beyond my own company's use.

Front end functionality includes:

 - User Accounts
 - Sprint Planner
 - Tasking Out Status
 - Most Valuable Professional voting
 - KPI Reports
 - Defect Reports
 - API Audit
 - API Execution (To allow for manual triggering of Jira Webhook end points)
 - Jira State Checker

End Points for Jira Webhooks:

 - Story Creation (Creates default sub-tasks.)
 - Story Update (Updates Team Ownership, Updates Work-Logs Data, Updates Defect Data)
 - Bug Update (Used for Impact Analysis Model updates across all teams within the group)
 - Story Deletion (Updates Defect Data)
 - Sub-Task Update (Forces state correction of parent stories)

## Azure Webapp Settings that need setting up
Unless otherwise stated, all settings are empty in source control.

| Setting Type | Name | Notes |
| ------ | ------ | ------ |
| Connection String | DevStatSQL | Set to local SQL Instance with integrated security in source control |
| Application Setting | JiraApiRoot | Root URL for the Jira Instance |
| Application Setting | JiraUserName | Process User Account in Jira |
| Application Setting | JiraPassword | Process User Password in Jira |
| Application Setting | JiraProjects | List of projects |
| Application Setting | JiraServiceDeskGroup | Used to identify id a bug is Internal or External. To be Deprecated. |
| Application Setting | EmailHost | Email Host, used for user account management |
| Application Setting | EmailPort | Email Port, used for user account management |
| Application Setting | EmailUserName | Email User Name, used for user account management, currently handled as unencrypted in web.config, to be resolved. |
| Application Setting | EmailPassword | Email Password, used for user account management, currently handled as unencrypted in web.config, to be resolved. |
| Application Setting | AllowedIPAddresses | N/A in source control, Can be comma separated to secure specific controller actions |
| Application Setting | AhaApiRoot | Blank in source control |
| Application Setting | AhaApiKey | Blank in source control |
| Application Setting | MvpDistributionEmail | Target email address for MVP notifications |

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

### Impact Analysis Model Update
URL: api/jira/iam/update/${issue.key}
Event jql: issuetype = Bug
Event Options: Issue - Updated
