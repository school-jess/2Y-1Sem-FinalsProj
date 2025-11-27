$(document).ready(() => {
    $("#grade").show();
    $("#subject").hide();
    $("#isEducatorInput").change(() => {
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