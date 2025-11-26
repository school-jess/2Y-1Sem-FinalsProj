$(document).ready(() => {
    $("#cancelEdit").hide();
    $("#submitEdit").hide();
    let editName = "";
    let editCourse = "";
    let editDepartment = "";
    let editEmail = "";
    let editPassword = "";
    let profileImgFilePath = "";
    $("#edit").click((e) => {
        e.preventDefault();
        $("#cancelEdit").show();
        $("#submitEdit").show();
        $("#edit").hide();
        let isFaculty = $("#userInfo").data("isFaculty") == "True";
        $("#userInfo").attr("data-is-faculty", isFaculty ? "True" : "False");
        editName = $("#editName").text();
        editCourse = $("#editCourse").text();
        editDepartment = $("#editDepartment").text();
        editEmail = $("#editEmail").text();
        editPassword = $("#editPassword").text();
        profileImgFilePath = $("#editProfileImg").prop("src");
        $("#editName").replaceWith($(`<input id="editName" name="Input.UserName" value="${editName}">`));
        $("#editCourse").replaceWith($(`<input id="editCourse" name="Input.UserCourse" value="${editCourse}">`));
        $("#editDepartment").replaceWith($(`<input id="editDepartment" name="Input.UserDepartment" value="${editDepartment}">`));
        $("#editEmail").replaceWith($(`<input id="editEmail" name="Input.UserEmail" type="email" value="${editEmail}">`));
        $("#editPassword").replaceWith($(`<input id="editPassword" name="Input.UserPassword" type="password" value="${editPassword}">`));
        $("#editProfileImg").replaceWith($(`<input id="editProfileImg" type="file" name="Input.ProfileImg">`));
        if (isFaculty) {
            $("#editSubject").replaceWith(`<input id="editSubject" name="Input.UserSubject" value="${$("#editSubject").text()}">`);
        } else {
            $("#editGrade").replaceWith($(`<input id="editGrade" name="Input.UserGrade" type="number" value="${$("#editGrade").text()}">`));
        }
    });

    $("#cancelEdit").click((e) => {
        e.preventDefault();
        $("#cancelEdit").hide();
        $("#submitEdit").hide();
        $("#edit").show();
        let isFaculty = $("#userInfo").data("isFaculty") == "True";
        $("#userInfo").attr("data-is-faculty", isFaculty ? "True" : "False");
        let cancelName = $(`<p id="editName">`);
        let cancelCourse = $(`<p id="editCourse">`);
        let cancelDepartment = $(`<p id="editDepartment"">`);
        let cancelEmail = $(`<p id="editEmail">`);
        let cancelProfileImg = $(`<img id="editProfileImg">`);
        cancelName.text(editName);
        cancelCourse.text(editCourse);
        cancelDepartment.text(editDepartment);
        cancelEmail.text(editEmail);
        cancelProfileImg.prop("src", profileImgFilePath);
        $("#editName").replaceWith(cancelName);
        $("#editCourse").replaceWith(cancelCourse);
        $("#editDepartment").replaceWith(cancelDepartment);
        $("#editEmail").replaceWith(cancelEmail);
        $("#editPassword").prop("disabled", true);
        $("#editProfileImg").replaceWith(cancelProfileImg);
        if (isFaculty) {
            let cancelSubject = $(`<p id="editSubject">`);
            cancelSubject.text($("#editSubject").val());
            $("#editSubject").replaceWith(cancelSubject);
        } else {
            let cancelGrade = $(`<p id="editGrade">`);
            cancelGrade.text($("#editGrade").val());
            $("#editGrade").replaceWith(cancelGrade);
        }
        $("#edit").text("edit");
    });
});