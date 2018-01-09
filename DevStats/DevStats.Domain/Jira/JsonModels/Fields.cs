using System;
using Newtonsoft.Json;

namespace DevStats.Domain.Jira.JsonModels
{
    public class Fields
    {
        [JsonProperty("parent")]
        public Issue Parent { get; set; }

        [JsonProperty("fixVersions")]
        public FixVersion[] FixVersions { get; set; }

        [JsonProperty("priority")]
        public NameField Priority { get; set; }

        [JsonProperty("customfield_10100")]
        public object EpicLink { get; set; }

        [JsonProperty("labels")]
        public string[] Labels { get; set; }

        [JsonProperty("assignee")]
        public User Assignee { get; set; }

        [JsonProperty("status")]
        public NameField Status { get; set; }

        [JsonProperty("components")]
        public object[] Components { get; set; }

        [JsonProperty("customfield_13201")]
        public string AhaReference { get; set; }

        [JsonProperty("customfield_13712")]
        public double? BusinessValue { get; set; }

        [JsonProperty("customfield_13716")]
        public ValueField OctopusModule { get; set; }

        [JsonProperty("creator")]
        public User Creator { get; set; }

        [JsonProperty("subtasks")]
        public Issue[] Subtasks { get; set; }

        [JsonProperty("reporter")]
        public User Reporter { get; set; }

        [JsonProperty("customfield_13711")]
        public object ProblemReference { get; set; }

        [JsonProperty("customfield_13710")]
        public string AcceptanceCriteria { get; set; }

        [JsonProperty("customfield_13702")]
        public ValueField ReviewOutcome { get; set; }

        [JsonProperty("customfield_13701")]
        public ValueField TaskType { get; set; }

        [JsonProperty("customfield_13704")]
        public ValueField TShirtSize { get; set; }

        [JsonProperty("customfield_13703")]
        public ValueField Complexity { get; set; }

        [JsonProperty("customfield_13709")]
        public ValueField Refinement { get; set; }

        [JsonProperty("progress")]
        public Progress Progress { get; set; }

        [JsonProperty("votes")]
        public Votes Votes { get; set; }

        [JsonProperty("issuetype")]
        public Issuetype Issuetype { get; set; }

        [JsonProperty("timespent")]
        public object Timespent { get; set; }

        [JsonProperty("project")]
        public NameField Project { get; set; }

        [JsonProperty("customfield_13700")]
        public ValueField CascadeTeam { get; set; }

        [JsonProperty("resolutiondate")]
        public DateTime? Resolutiondate { get; set; }

        [JsonProperty("created")]
        public DateTime Created { get; set; }

        [JsonProperty("customfield_13800")]
        public double? AhaRank { get; set; }

        [JsonProperty("updated")]
        public string Updated { get; set; }

        [JsonProperty("timeoriginalestimate")]
        public int? Timeoriginalestimate { get; set; }

        [JsonProperty("description")]
        public string Description { get; set; }

        [JsonProperty("summary")]
        public string Summary { get; set; }

        [JsonProperty("customfield_10004")]
        public double? StoryPoints { get; set; }

        [JsonProperty("environment")]
        public object Environment { get; set; }

        [JsonProperty("duedate")]
        public DateTime? Duedate { get; set; }

        [JsonProperty("customfield_13715")]
        public ValueField PayrollModule { get; set; }

        [JsonProperty("customfield_13714")]
        public ValueField HRModule { get; set; }

        [JsonProperty("timetracking")]
        public TimeTracking TimeTracking { get; set; }

        [JsonProperty("customfield_14300")]
        public CategoryDetails Category { get; set; }

        [JsonProperty("customfield_15716")]
        public ValueField DefectScaleOfUserImpact { get; set; }

        [JsonProperty("customfield_15717")]
        public ValueField DefectScaleOfFunctionalImpact { get; set; }

        [JsonProperty("customfield_15718")]
        public ValueField DefectWorkaround { get; set; }

        [JsonProperty("customfield_15705")]
        public decimal? DefectScore { get; set; }
    }
}