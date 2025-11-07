$(document).ready(() => {
    $("#grade").show();
    $("#subject").hide();
    $("#IsEducator").change(() => {
        if ($("#IsEducator").prop("checked")) {
            $("#subject").show();
            $("#grade").hide();
        } else {
            $("#grade").show();
            $("#subject").hide();
        }
    });
});