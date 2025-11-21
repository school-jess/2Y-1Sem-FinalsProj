$(document).ready(() => {
    $("#cancelEdit").hide();
    $("#edit").click((e) => {
        e.preventDefault();
        $("#cancelEdit").show();
        let userInfoElem = $("#userInfo");
        let isFaculty = userInfoElem.data("isFaculty") == "True";
        let userInfoChildren = userInfoElem.children().detach();
        let userInfoForm = $(`<form id="userInfo" data-is-faculty=${isFaculty ? "True" : "False"} method="post">`);
        userInfoElem.replaceWith(userInfoForm);
        userInfoForm.append(userInfoChildren);
        $("#edit").prop("type", "submit");
        $("#edit").text("submit");
        $("#editName").replaceWith($(`<input id="editName" name="Input.UserName" value="${$("#editName").text()}">`));
        $("#editCourse").replaceWith($(`<input id="editCourse" name="Input.UserCourse" value="${$("#editCourse").text()}">`));
        $("#editDepartment").replaceWith($(`<input id="editDepartment" name="Input.UserDepartment" value="${$("#editDepartment").text()}">`));
        $("#editEmail").replaceWith($(`<input id="editEmail" name="Input.UserEmail" type="email" value="${$("#editEmail").text()}">`));
        $("#editPassword").replaceWith($(`<input id="editPassword" name="Input.UserPassword" type="password" value="${$("#editPassword").text()}">`));
        if (isFaculty) {
            $("#editSubject").replaceWith(`<input id="editSubject" name="Input.UserSubject" value="${$("#editSubject").text()}">`);
        } else {
            $("#editGrade").replaceWith($(`<input id="editGrade" name="Input.UserGrade" type="number" value="${$("#editGrade").text()}">`));
        }
    });

    $("#cancelEdit").click((e) => {
        e.preventDefault();
        let userInfoElem = $("#userInfo");
        let isFaculty = userInfoElem.data("isFaculty") == "True";
        let userInfoChildren = userInfoElem.children().detach();
        let userInfoDef = $(`<div id="userInfo" data-is-faculty="${isFaculty ? "True" : "False"}">`);
        userInfoElem.replaceWith(userInfoDef);
        userInfoDef.append(userInfoChildren);
        $("#cancelEdit").hide();
        $("#edit").prop("type", "button");
        let cancelName = $(`<h1 id="bookName">`);
        let cancelCourse = $(`<h2 id="bookAuthor">`);
        let cancelDepartment = $(`<h3 id="bookReleaseDate"">`);
        let cancelEmail = $(`<p id="bookSynopsis">`);
        let cancelPassword = $(`<p id="bookClassificationId">`);
        cancelName.text($("#editName").val());
        cancelCourse.text($("#editCourse").val());
        cancelDepartment.text($("#editDepartment").val());
        cancelEmail.text($("#editEmail").val());
        cancelPassword.text($("#editPassword").val());
        $("#editName").replaceWith(cancelName);
        $("#editCourse").replaceWith(cancelCourse);
        $("#editDepartment").replaceWith(cancelDepartment);
        $("#editEmail").replaceWith(cancelEmail);
        $("#editPassword").replaceWith(cancelPassword);
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