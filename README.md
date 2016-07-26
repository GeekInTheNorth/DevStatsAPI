# DevStatsAPI
A WebAPI project for retaining development statistics.  This is currently limited to handling the WorkRemaining for an external burndown chart.  This may be expanded upon later.

## Source Notes
I have purposefully excluded Web.Release.Config and Web.Debug.Config from source control.  This is to allow me to use a connection string without sensitive db access being included in source control.  For Azure deployments, connection string information has been added to Application Settings in the portal.