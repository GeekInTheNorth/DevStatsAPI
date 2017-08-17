$(document).ready(function () {
    onTeamChange();

    $("#ddlSprints").change(function () {
        onSprintChange();
    });

    $("#ddlTeam").change(function () {
        onTeamChange();
    });
});

function onSprintChange() {
    $("div.please-wait").show();
    clearContainers();

    var sprintOption = $("#ddlSprints").find(":selected");
    var boardid = sprintOption.data("boardid");
    var sprintid = sprintOption.data("sprintid");
    var team = $("#ddlTeam").val();

    getSprintStories(boardid, sprintid);
    getRefinedStories(team, sprintid);

    $("div.please-wait").hide();
}

function onTeamChange() {
    $("div.please-wait").show();
    clearContainers();

    populateSprintNames();
}

function populateSprintNames() {
    var sprintNameUrl = document.location.origin + '/api/sprintplanning/sprintlist';

    $.get(sprintNameUrl, function (data) {
        fillSprintOptions(data);
    });
}

function fillSprintOptions(data) {
    if (data === undefined) return;
    $("#ddlSprints").find("option").remove();

    $.each(data, function (dataIndex) {
        var team = $("#ddlTeam").val();
        var sprint = data[dataIndex];
        var displayName = sprint.BoardName + " : " + sprint.SprintName;

        if (displayName.indexOf(team) > -1) {
            var boardId = sprint.BoardId;
            var sprintId = sprint.SprintId;
            var optionMarkUp = "<option value='" + displayName + "' data-sprintid='" + sprintId + "' data-boardid='" + boardId + "'>" + displayName + "</option>";

            $("#ddlSprints").append(optionMarkUp);
        }
    });

    onSprintChange();
}

function getSprintStories(boardid, sprintid) {
    var sprintStoriesUrl = document.location.origin + '/api/sprintplanning/sprintstories/' + boardid + '/' + sprintid;

    $.get(sprintStoriesUrl, function (data) {
        fillSprintStories(data);
    });
}

function fillSprintStories(data) {
    $("div#sprint-content").html("<table id='tblsprintstories' class='defectdata'><thead><tr><th>Key</th><th>Description</th><th>Type</th><th>State</th><th class='numeric'>Dev Remaining</th><th class='numeric'>QA Remaining</th></tr></thead><tbody></tbody></table>");

    $.each(data, function (dataIndex) {
        var dataItem = data[dataIndex];
        $("#tblsprintstories > tbody").append(getRowMarkUp(dataItem));
    });
}

function getRefinedStories(team, sprintid) {
    var refinedStoriesUrl = document.location.origin + '/api/sprintplanning/refinedstories/' + team + '/' + sprintid;

    $.get(refinedStoriesUrl, function (data) {
        fillRefinedStories(data);
    });
}

function fillRefinedStories(data) {
    $("div#refined-items").html("<table id='tblrefinedstories' class='defectdata'><thead><tr><th>Key</th><th>Description</th><th>Type</th><th>State</th><th class='numeric'>Dev Remaining</th><th class='numeric'>QA Remaining</th></tr></thead><tbody></tbody></table>");

    $.each(data, function (dataIndex) {
        var dataItem = data[dataIndex];
        $("#tblrefinedstories > tbody").append(getRowMarkUp(dataItem));
    });
}

function clearContainers() {
    $("div#sprint-content").empty();
    $("div#refined-items").empty();
}

function getRowMarkUp(story) {
    var markUp = "<tr>";
    markUp += "<td><a href='" + story.Url + "' target='_blank'>" + story.Key + "</a></td>";
    markUp += "<td>" + story.Description + "</td>";
    markUp += "<td>" + story.Type + "</td>";
    markUp += "<td>" + story.State + "</td>";
    markUp += "<td class='numeric'>" + story.DevelopmentRemainingInHours + "</td>";
    markUp += "<td class='numeric'>" + story.TestingRemainingInHours + "</td>";
    markUp += "<tr>";

    return markUp;
}