$(document).ready(() => {
    $("#grade").show();
    $("#subject").hide();
    $("#Input_IsEducator").click(() => {
        if ($("#Input_IsEducator").prop("checked")) {
            $("#subject").show();
            $("#grade").hide();
        } else {
            $("#grade").show();
            $("#subject").hide();
        }
    });
});