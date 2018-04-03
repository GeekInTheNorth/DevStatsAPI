function showViewProperty(button) {
    var propertyName = $(button).data("propertyname");
    setControls(propertyName, false);
}

function showEditProperty(button) {
    var propertyName = $(button).data("propertyname");
    setControls(propertyName, true);
}

function saveProperty(button) {
    var propertyName = $(button).data("propertyname");
    var editControlSearch = "#" + propertyName + ".edit-property";
    var viewControlSearch = "#" + propertyName + ".view-property";
    var propertyValue = $(editControlSearch).val();

    var package = {
        Name: propertyName,
        Value: propertyValue
    };

    var postUrl = document.location.origin + "/api/systemproperty/save";

    $.post(postUrl, package)
        .done(function (data, textStatus, jqXHR) { showUpdateSuccess(propertyName, data, textStatus, jqXHR); })
        .fail(function (data, textStatus, jqXHR) { showUpdateFail(propertyName, data, textStatus, jqXHR); })
}

function showUpdateSuccess(propertyName, data, textStatus, jqXHR) {
    var editControlSearch = "#" + propertyName + ".edit-property";
    var viewControlSearch = "#" + propertyName + ".view-property";
    var typeControlSearch = "#" + propertyName + ".property-data-type";
    var dataType = $(typeControlSearch).text();

    var markUp = "<div class='alert alert-success alert-dismissible'><a href='#' class='close' data-dismiss='alert' aria-label='close'>&times;</a>" + propertyName + " successfully updated</div>";

    $(markUp).insertBefore("#tbl-properties");

    if (dataType !== "Password")
        $(viewControlSearch).text($(editControlSearch).val());

    setControls(propertyName, false);
}

function showUpdateFail(propertyName, data, textStatus, jqXHR) {
    var markUp = "<div class='alert alert-danger alert-dismissible'><a href='#' class='close' data-dismiss='alert' aria-label='close'>&times;</a>";
    markUp += jqXHR;

    if (data !== undefined && data.responseJSON !== undefined && data.responseJSON.Message !== undefined) {
        markUp += ": " + data.responseJSON.Message;
    }
    markUp += "</div>";

    $(markUp).insertBefore("#tbl-properties");
}

function setControls(propertyName, isEditMode) {
    var editControlSearch = "#" + propertyName + ".edit-property";
    var viewControlSearch = "#" + propertyName + ".view-property";
    var editButtonSearch = "#btn-edit-" + propertyName;
    var saveButtonSearch = "#btn-save-" + propertyName;
    var cancelButtonSearch = "#btn-cancel-" + propertyName;

    if (isEditMode) {
        $(editControlSearch).show();
        $(viewControlSearch).hide();
        $(editButtonSearch).hide();
        $(saveButtonSearch).show();
        $(cancelButtonSearch).show();
    } else {
        $(editControlSearch).hide();
        $(viewControlSearch).show();
        $(editButtonSearch).show();
        $(saveButtonSearch).hide();
        $(cancelButtonSearch).hide();
    }
}