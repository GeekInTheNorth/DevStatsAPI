# DevStatsAPI
A WebAPI project for retaining development statistics.  This is currently limited to handling the WorkRemaining for an external burndown chart.  This may be expanded upon later.

## Azure Webapp Settings that need setting up
- Connection String: "DevStatSQL".
- Application Setting: "JiraApiRoot".
- Application Setting: "JiraUserName"
- Application Setting: "JiraPassword"

Values are either left intentionally blank or set to the local SQL instance with integrated security so that secure information is not included in source control.

## ToDo
- The date formatting code was lifted from another application for a fast turn around.  This is a mess and needs to be better.