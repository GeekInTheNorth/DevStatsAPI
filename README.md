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
| Application Setting | BitbucketUserName | Process User Account in Jira |
| Application Setting | BitbucketPassword | Process User Password in Bitbucket. Do look at using "App Passwords" with limited access rather than using the real password. |

## Jira Cloud Webhook Setup
In all of the following cases, the URL must be prefixed with the domain of the DevStats instance. Jira workflows require a transistion of state.  This means key data cannot be updated or analysed if something other than state is changed.

### Story Creation
URL: /api/jira/story/create/${issue.key}
Event jql: project in (CPR,OCT,CHR) AND issuetype in standardIssueTypes() AND issueType != "Task"
Event Options: Issue - Created

| Data Changes | Reason | Jira Projects |
| ------ | ------ | ------ |
| Create Merge Task | Because every story needs one and there is no variation. | CPR, CHR, OCT |
| Create PO Review Task | Because every story needs one and there is no variation. | CPR, CHR, OCT |

### Story Update
URL: api/jira/story/update/${issue.key}
Event jql: project in (CPR,OCT,CHR) AND issueType in standardIssueTypes()
Event Options: Issue - Updated

| Data Changes | Reason | Jira Projects |
| ------ | ------ | ------ |
| Copies Team from Bug/Story to Subtask | Prevents subtasks from appearing on other teams boards as orphans. | CPR, CHR, OCT |
| Copies Version from Bug/Story to Subtask | KPI reporting requires versions against rework subtasks. | CPR, CHR, OCT |
| Processes Work Logs | Used for developer Feature Return Rate KPI. This could be removed as KPI reporting at a group level is made more visible to the team. | CPR, CHR, OCT |
| Update Defect Analysis | Jira does not handle reporting of defects in the way that SMT want to consume the data. Module breakdown that we are required to report doesn't align with the modules we want to record.  This results in a report in Devstats running fast instead of requiring a manual effort in terms of reporting. If this is replaced by group level reporting, then this can be removed. | CPR, CHR, OCT |
| Sets Defect Returned on Bugs | This ensures that Bugs are have Defect Returned ticked if they have one or more reworks. This is required for the Defect Return Rate KPI reporting | CPR, CHR, OCT |
| Sets Release Originated | Reviews the Affected Versions field and copies the earliest value to Release Originated. Release Originated is used for SMT release reporting while we want to record all affected versions | CPR, CHR, OCT |
| Sets Assignee on Story/Bug | Sets the Assignee based on the Developer who owns the greatest quantity of subtasks.  Team members work at a subtask level when performing tasks which are automatically assigned to them as they move them into "In Progress" and "Done", so they very rarely change the story. | CPR, CHR, OCT |

### Story Delete
URL: api/jira/story/delete/${issue.key}
Event jql: project in (CPR,OCT,CHR) AND issuetype in standardIssueTypes()
Event Options: Issue - Deleted

| Data Changes | Reason | Jira Projects |
| ------ | ------ | ------ |
| Update Defect Analysis | Jira does not handle reporting of defects in the way that SMT want to consume the data. Module breakdown that we are required to report doesn't align with the modules we want to record.  This results in a report in Devstats running fast instead of requiring a manual effort in terms of reporting. If this is replaced by group level reporting, then this can be removed. | CPR, CHR, OCT |

### Subtask Update
URL: api/jira/subtask/update/${issue.key}
Event jql: N/A
Event Options: N/A
Workflow Transition: Add a Post-Function that executes a "Generic Event", this can target a WebHook.  I've used this so this event only gets triggered when moving between states.

| Data Changes | Reason | Jira Projects |
| ------ | ------ | ------ |
| Updates Parent State | This moves the Story/Bug to In Progress when a subtask moves to In Progress. Jira does not automatically perform this status change on parents. | CPR, CHR, OCT |

### Impact Analysis Model Update
URL: api/jira/iam/update/${issue.key}
Event jql: (project in (CHR,CPR,OCT,KFPAYROLL,OPAYSLIP) AND issuetype = Bug) OR (project in (SE32,SIP,SIQ,SGP,SPM,S12P) AND issuetype in (Correction, Defect))
Event Options: Issue - Updated

| Data Changes | Reason | Jira Projects |
| ------ | ------ | ------ |
| Updates IAM Score | This reviews the chosen values for the IAM Model Questions and assigns a score as per the HCM Engineering IAM guide. | CHR, CPR, OCT, KFPAYROLL, OPAYSLIP, SE32, SIP, SIQ, SGP, SPM, S12P |

## Other End points

### Post build status to Bitbucket
URL: api/bitbucket/build/status
Notes: Used to update the build status in bitbucket for any branch built regardless of build system. This functionality replaces the build status integrations from older versions of TeamCity to Bitbucket.  The following is the body content that should be provided:

```JSON
{ 
	"BuildNumber" = "<Build Number>",
	"CommitSha" = "SHA for the last commit on the branch being built",
	"Status" = "<INPROGRESS|SUCCESSFUL|FAILED>",
	"RepositoryName" = "<Repo Name>",
	"BitbucketOrganisation" = "<Org Name>"
}
```

## Database

DevStats uses Code First Entity Framework 6 with Migrations.  To create and apply migrations you need to use the nuget package manager console in the context of the DevStats.Data project.

If you need to alter and roll back migrations that are in development, you will need to make a change to the configuration.cs file to turn off automatic migrations until development is complete.  Autmatic migrations should be turned back on before pushing to master.

```C#
namespace DevStats.Data.Migrations
{
    using System.Data.Entity.Migrations;

    internal sealed class Configuration : DbMigrationsConfiguration<DevStatContext>
    {
        public Configuration()
        {
            // If actively developing DB changes, set this to false to allow you to manually roll forward/backward.  Set it back to true before merging to master
            AutomaticMigrationsEnabled = true;
            ContextKey = "DevStats.Data.DevStatContext";
        }
```

### Add Migrations

This will look at the DevStatContext and the linked entities as well as the DB updated to the previous migration to determin what table and column changes there are and it will script those as changes.

```C# Example
Add-Migration LogBuildStatusChanges
```

If you have not yet updated the database and you have made more changes to the entities or the context, running the same command again with "-Force" will regenerate the migration to include your extra changes.

### Apply updates to current DB

This will look at the migrations table in the database to determin which migrations to apply. See example below:

```C#
Update-Database
```

### Rolling back to a specific migration

This will roll back the DB to a specified migration. See example below:

```C#
Update-Database -target:CorrectApiUrlColumnOnApiLog
```