# DevStatsAPI
A WebAPI project for retaining development statistics.  This is currently limited to handling the WorkRemaining for an external burndown chart.  This may be expanded upon later.

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
- Sort out localisation issues with date entries between server and client
- Require encryption of the JiraUserName and JiraPassword in the web.config
