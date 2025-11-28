$(document).ready(() => {
    $("#grade").show();
    $("#subject").hide();
    $("#isEducatorLabel").click(() => {
        if ($("#isEducatorInput").prop("checked")) {
            console.log("hello");
            $("#subject").show();
            $("#grade").hide();
        } else {
            $("#grade").show();
            $("#subject").hide();
        }
    });
});