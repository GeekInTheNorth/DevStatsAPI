﻿var devCapacity = 0;
var qaCapacity = 0;
var completedSprintStories = false;
var completedBacklogStories = false;

$(document).ready(function () {
    checkOptions();
    onTeamChange();

    $("#ddlSprints").change(function () {
        onSprintChange();
    });

    $("#ddlTeam").change(function () {
        onTeamChange();
    });
});

function onSprintChange() {
    completedSprintStories = false;
    completedBacklogStories = false;
    setButtonState(false);
    
    clearContainers();

    var sprintOption = $("#ddlSprints").find(":selected");
    var boardid = sprintOption.data("boardid");
    var sprintid = sprintOption.data("sprintid");
    var team = $("#ddlTeam").val();

    getSprintStories(boardid, sprintid);
    getRefinedStories(team, sprintid);
}

function onTeamChange() {
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

        if (completedSprintStories && completedBacklogStories)
            setButtonState(true);
    });
}

function fillSprintStories(data) {
    var sprintOption = $("#ddlSprints").find(":selected");
    var boardid = sprintOption.data("boardid");
    var sprintid = sprintOption.data("sprintid");

    if (boardid !== data.BoardId || sprintid !== data.SprintId)
        return;

    $("div#sprint-content").html(getHeaderRowMarkUp("tblsprintstories"));

    var stories = data.Stories;

    $.each(stories, function (index) {
        var story = stories[index];
        $("#tblsprintstories > tbody").append(getRowMarkUp(story, true));
    });

    addTotalRow("tblsprintstories", true);

    completedSprintStories = true;
}

function getRefinedStories(team, sprintid) {
    var refinedStoriesUrl = document.location.origin + '/api/sprintplanning/refinedstories/' + team + '/' + sprintid;

    $.get(refinedStoriesUrl, function (data) {
        fillRefinedStories(data);

        if (completedSprintStories && completedBacklogStories)
            setButtonState(true);
    });
}

function fillRefinedStories(data) {
    var sprintOption = $("#ddlSprints").find(":selected");
    var sprintid = sprintOption.data("sprintid");
    var team = $("#ddlTeam").val();

    if (data.Team !== team || sprintid !== data.SprintBeingPlanned)
        return;

    $("div#refined-items").html(getHeaderRowMarkUp("tblrefinedstories"));

    var stories = data.Stories;

    $.each(stories, function (index) {
        var story = stories[index];
        $("#tblrefinedstories > tbody").append(getRowMarkUp(story, false));
    });

    addTotalRow("tblrefinedstories", false);

    completedBacklogStories = true;
}

function clearContainers() {
    $("div#sprint-content").empty();
    $("div#refined-items").empty();

    $("div#sprint-content").text("Please Wait: Loading sprint data from Jira...");
    $("div#refined-items").text("Please Wait: Loading backlog data from Jira...");
}

function getHeaderRowMarkUp(tableName) {
    var markUp = "<table id='" + tableName + "' class='table table-hover'>";
    markUp += "<thead><tr>";
    markUp += "<th>Key</th>";
    markUp += "<th>Description</th>";
    markUp += "<th>Type</th>";
    markUp += "<th>Refinement</th>";
    markUp += "<th>State</th>";
    markUp += "<th class='numeric'>Dev Remaining</th>";
    markUp += "<th class='numeric'>QA Remaining</th>";
    markUp += "<th>Action</th>";
    markUp += "</tr></thead>";
    markUp += "<tbody></tbody></table>";

    return markUp;
}

function getRowMarkUp(story, isInSprint) {
    var markUp = "<tr id='" + story.Key+"'>";
    markUp += "<td><a href='" + story.Url + "' target='_blank'>" + story.Key + "</a></td>";
    markUp += "<td>" + story.Description + "</td>";
    markUp += "<td>" + story.Type + "</td>";
    markUp += "<td>" + story.Refinement + "</td>";
    markUp += "<td>" + story.State + "</td>";
    markUp += "<td class='numeric'>" + story.DevelopmentRemainingInHours + "</td>";
    markUp += "<td class='numeric'>" + story.TestingRemainingInHours + "</td>";

    if (isInSprint) {
        markUp += "<td><a onclick='javascript:removeFromSprint()' class='delete-button' data-key='" + story.Key + "'>[Remove]</a></td>";
    }
    else {
        markUp += "<td><a onclick='javascript:addToSprint()' class='add-button' data-key='" + story.Key + "'>[Add]</a></td>";
    }

    markUp += "</tr>";

    return markUp;
}

function addTotalRow(tableName, includeCapacity) {
    var dataRows = $("#" + tableName).find("tbody>tr");
    var firstRow = dataRows[0];
    var totalDevRemaining = 0;
    var totalQARemaining = 0;

    $.each(dataRows, function (index) {
        var dataRow = dataRows[index];
        totalDevRemaining += parseFloat(dataRow.cells[5].innerText);
        totalQARemaining += parseFloat(dataRow.cells[6].innerText);
    });

    var markUp = "<tfoot>"
    markUp += "<tr>";
    markUp += "<th class='active numeric' colspan='5'>Total</th>";
    markUp += "<th class='active numeric' id='totalDevRemaining'>" + totalDevRemaining.toFixed(2) + "</th>";
    markUp += "<th class='active numeric' id='totalQaRemaining'>" + totalQARemaining.toFixed(2) + "</th>";
    markUp += "<th class='active'>&nbsp;</th>";
    markUp += "</tr>";

    if (includeCapacity) {
        markUp += "<tr>";
        markUp += "<th class='active numeric' colspan='5'>Capacity</th>";
        markUp += "<th class='active'><input type='number' id='devCapacity' min='0' max='500' value='" + devCapacity.toFixed(2) + "'></input></th>"
        markUp += "<th class='active'><input type='number' id='qaCapacity' min='0' max='500' value='" + qaCapacity.toFixed(2) + "'></th>"
        markUp += "<th class='active'>&nbsp;</th>";
        markUp += "</tr>";

        var devRemainingCapacity = devCapacity - totalDevRemaining;
        var qaRemainingCapacity = qaCapacity - totalQARemaining;

        markUp += "<tr>";
        markUp += "<th class='active numeric' colspan='5'>Remaining</th>";
        markUp += "<th class='active numeric' id='devRemainingCapacity'>" + devRemainingCapacity.toFixed(2) + "</th>";
        markUp += "<th class='active numeric' id='qaRemainingCapacity'>" + qaRemainingCapacity.toFixed(2) + "</th>";
        markUp += "<th class='active'>&nbsp;</th>";
        markUp += "</tr>";
    }

    markUp += "</tfoot>";

    $("#" + tableName + " tfoot").remove();
    $("#" + tableName).append(markUp);

    $("#devCapacity").blur(function () { onDevCapacityChange(); });
    $("#qaCapacity").blur(function () { onQaCapacityChange(); });
}

function removeFromSprint() {
    var key = $(event.srcElement).data("key");
    var cells = $("tr#" + key).find("td");
    var markUp = "<tr id='" + key + "'>";

    $.each(cells, function (index) {
        if (index < 7) {
            markUp += cells[index].outerHTML;
        }
        else
        {
            markUp += "<td><a onclick='javascript:addToSprint()' class='add-button' data-key='" + key + "'>[Add]</a></td>"; 
        }
    });

    markUp += "</tr>";

    $("#tblrefinedstories > tbody").append(markUp);
    $("#tblsprintstories tr#" + key).remove();
    addTotalRow("tblsprintstories", true);
    addTotalRow("tblrefinedstories", false);
}

function addToSprint() {
    var key = $(event.srcElement).data("key");
    var cells = $("tr#" + key).find("td");
    var markUp = "<tr id='" + key + "'>";

    $.each(cells, function (index) {
        if (index < 7) {
            markUp += cells[index].outerHTML;
        }
        else {
            markUp += "<td><a onclick='javascript:removeFromSprint()' class='delete-button' data-key='" + key + "'>[Remove]</a></td>";
        }
    });

    markUp += "</tr>";
    
    $("#tblsprintstories > tbody").append(markUp);
    $("#tblrefinedstories tr#" + key).remove();
    addTotalRow("tblsprintstories", true);
    addTotalRow("tblrefinedstories", false);
}

function onDevCapacityChange() {
    devCapacity = parseFloat($("#devCapacity").val());
    var totalDevRemaining = parseFloat($("#totalDevRemaining").text());
    var devCapacityRemaining = devCapacity - totalDevRemaining;

    $("#devRemainingCapacity").text(devCapacityRemaining.toFixed(2));
}

function onQaCapacityChange() {
    qaCapacity = parseFloat($("#qaCapacity").val());
    var totalQaRemaining = parseFloat($("#totalQaRemaining").text());
    var qaRemainingCapacity = qaCapacity - totalQaRemaining;

    $("#qaRemainingCapacity").text(qaRemainingCapacity.toFixed(2));
}

function onSprintUpdate() {
    var sprintOption = $("#ddlSprints").find(":selected");
    var boardid = sprintOption.data("boardid");
    var sprintid = sprintOption.data("sprintid");

    var package = new Object();
    package.TeamName = $("#ddlTeam").val();
    package.Keys = [];
    var $actionButtons = $("#tblsprintstories a.delete-button");

    $.each($actionButtons, function (index) {
        var button = $actionButtons[index];
        var key = $(button).data("key");

        package.Keys.push(key);
    });

    var commitSprintUrl = document.location.origin + '/api/sprintplanning/sprintstories/' + boardid + '/' + sprintid;   

    $.post(commitSprintUrl, package)
        .done(function (data, textStatus, jqXHR) { showSprintUpdateSuccess(data, textStatus, jqXHR); })
        .fail(function (data, textStatus, jqXHR) { showSprintUpdateFail(data, textStatus, jqXHR); });
}

function setButtonState(newState) {
    if (newState) {
        $("#btnRefresh").removeAttr('disabled', 'disabled');
        $("#btnCommit").removeAttr('disabled', 'disabled');
    }
    else {
        $("#btnRefresh").attr('disabled', 'disabled');
        $("#btnCommit").attr('disabled', 'disabled');
    }
}

function showSprintUpdateSuccess(data, textStatus, jqXHR) {
    var markUp = "<div class='alert alert-success alert-dismissible'><a href='#' class='close' data-dismiss='alert' aria-label='close'>&times;</a>Sprint successfully updated</div>";

    $(markUp).insertAfter("#filter-bar");
}

function showSprintUpdateFail(data, textStatus, jqXHR) {
    var markUp = "<div class='alert alert-danger alert-dismissible'><a href='#' class='close' data-dismiss='alert' aria-label='close'>&times;</a>";
    markUp += jqXHR;

    if (data !== undefined && data.responseJSON !== undefined && data.responseJSON.Message !== undefined) {
        markUp += ": " + data.responseJSON.Message;
    }
    markUp += "</div>";

    $(markUp).insertAfter("#filter-bar");
}

function checkOptions() {
    var teamName = $("input#teamName").val();

    if (teamName === undefined || teamName === null || teamName === "")
        return;

    var hasMatchingTeam = false;
    $("#ddlTeam > option").each(function () {
        if (this.value.toLowerCase() === teamName.toLowerCase())
            hasMatchingTeam = true;
    });

    if (hasMatchingTeam) {
        $("#ddlTeam > option").each(function () {
            if (this.value.toLowerCase() !== teamName.toLowerCase())
                $(this).remove();
        });
    }
}