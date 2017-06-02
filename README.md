# DevStatsAPI
Current functionality includes:
- Jira Webhook for creating subtasks against Jira stories
- Historical recording of defect stats
- Historical burndown data

## Azure Webapp Settings that need setting up

| Setting Type | Name | Notes |
| ------ | ------ | ------ |
| Connection String | DevStatSQL | Set to local SQL Instance with integrated security in source control |
| Application Setting | JiraApiRoot | Blank in source control |
| Application Setting | JiraUserName | Blank in source control, currently handled as unencrypted in web.config |
| Application Setting | JiraPassword | Blank in source control, currently handled as unencrypted in web.config |
| Application Setting | AllowedIPAddresses | N/A in source control, Can be comma separated to secure specific controller actions |

## ToDo
- The date formatting code was lifted from another application for a fast turn around.  This is a mess and needs to be better.
- Require encryption of the JiraUserName and JiraPassword in the web.config

## API: JiraCreateSubtasks
This should be set up as a webhook in a Jira Cloud instance that targets the create event for new stories.  At the moment this does set custom fields.  Ideally i will make this more generic.
This creates two subtasks against a Story or Bug, one for a Product Owner Review and one for a Merge to Develop task.

## API: JiraSubtaskUpdated
This is a webhook that is added to a state transition.  The idea being that the parent will have it's state moved from To Do to In Progress when a subtask moves to In Progress.

