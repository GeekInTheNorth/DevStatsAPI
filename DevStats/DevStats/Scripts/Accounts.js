function updateUser(button, action) {
    var userId = $(button).data("id");
    var userName = $(button).data("username");

    var updateUrl = document.location.origin + '/api/account/' + action + '/' + userId;

    $.post(updateUrl)
        .done(function (data, textStatus, jqXHR) { showUpdateSuccess(userName, action, data, textStatus, jqXHR); })
        .fail(function (data, textStatus, jqXHR) { showUpdateFail(userName, action, data, textStatus, jqXHR); });
}

function deleteUser(button) {
    var userId = $(button).data("id");
    var userName = $(button).data("username");

    if (!confirm("Are you sure you want to delete " + userName))
        return;

    var deleteUrl = document.location.origin + '/api/account/delete/' + userId;

    $.ajax({
        url: deleteUrl,
        type: 'DELETE',
        success: function (data, textStatus, jqXHR) {
            showUpdateSuccess(userName, "delete", data, textStatus, jqXHR);
        },
        error: function (data, textStatus, jqXHR) {
            showUpdateFail(userName, "delete", data, textStatus, jqXHR);
        }
    });
}

function showUpdateSuccess(userName, action, data, textStatus, jqXHR) {
    var markUp = "<div class='alert alert-success alert-dismissible'><a href='#' class='close' data-dismiss='alert' aria-label='close'>&times;</a>Successfully " + action + "d " + userName + ". Refresh to see changes.</div>";

    $(markUp).insertBefore("#tbl-users");
}

function showUpdateFail(userName, action, data, textStatus, jqXHR) {
    var markUp = "<div class='alert alert-danger alert-dismissible'><a href='#' class='close' data-dismiss='alert' aria-label='close'>&times;</a>Failed to " + action + " " + userName + ": ";
    markUp += jqXHR;

    if (data !== undefined && data.responseJSON !== undefined && data.responseJSON.Message !== undefined) {
        markUp += ": " + data.responseJSON.Message;
    }
    markUp += "</div>";

    $(markUp).insertBefore("#tbl-users");
}