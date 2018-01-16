var myBarChart = null;

$(document).ready(function () {
    onTeamChange();

    $("#ddlTeam").change(function () {
        onTeamChange();
    });
});

function onTeamChange() {
    clearContainers();

    populateTaskingLevels();
}

function clearContainers() {
    $("#DevTTO").empty();
    $("#QATTO").empty();
    $("#DevReady").empty();
    $("#QAReady").empty();
}

function populateTaskingLevels() {

    var team = $("#ddlTeam").val();
    var commitSprintUrl = document.location.origin + '/api/taskingstatus/' + team;

    $.get(commitSprintUrl, function (data) {
        setupChart(data);
        $("#DevTTO").text(data["DevTTO"] + " hours");
        $("#QATTO").text(data["QATTO"] + " hours");
        $("#DevReady").text(data["DevReady"] + " hours");
        $("#QAReady").text(data["QAReady"] + " hours");
    });

}

function setupChart(reportdata) {
    var ctxB = document.getElementById("barChart").getContext('2d');
    if (myBarChart) {
        myBarChart.destroy();
    }
    myBarChart = new Chart(ctxB, {
        type: 'bar',
        data: {
            labels: ["To Task Out", "Ready"],
            datasets: [{
                label: 'Dev',
                data: [reportdata["DevTTO"], reportdata["DevReady"]],
                backgroundColor: [
                    'rgba(255, 99, 132, 0.2)',
                    'rgba(255, 99, 132, 0.2)'
                ],
                borderColor: [
                    'rgba(255,99,132,1)',
                    'rgba(255,99,132,1)'
                ],
                borderWidth: 1
            },
            {
                label: 'QA',
                data: [reportdata["QATTO"], reportdata["QAReady"]],
                backgroundColor: [
                    'rgba(54, 162, 235, 0.2)',
                    'rgba(54, 162, 235, 0.2)'
                ],
                borderColor: [
                    'rgba(54, 162, 235, 1)',
                    'rgba(54, 162, 235, 1)'
                ],
                borderWidth: 1
            }]
        },
        options: {
            scales: {
                yAxes: [{
                    stacked: true,
                    ticks: {
                        beginAtZero: true,
                        min: 0
                    }
                }],
                xAxes: [{
                    barThickness: 80,
                    stacked: true,
                }]
            }
        }
    });

}