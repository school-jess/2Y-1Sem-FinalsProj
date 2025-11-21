$(document).ready(() => {
    $("#edit").click((e) => {
        e.preventDefault();
        let userInfoElem = $("#userInfo");
        let isFaculty = userInfoElem.data("isFaculty") == "True";
        let userInfoChildren = userInfoElem.children().detach();
        let userInfoForm = $("<form method=\"post\">");
        userInfoElem.replaceWith(userInfoForm);
        userInfoForm.append(userInfoChildren);
        $("#edit").prop("type", "submit");
        $("#edit").text("submit");
        $("#editName").replaceWith($(`<input name="Input.UserName" value="${$("#editName").text()}">`));
        $("#editCourse").replaceWith($(`<input name="Input.UserCourse" value="${$("#editCourse").text()}">`));
        $("#editDepartment").replaceWith($(`<input name="Input.UserDepartment" value="${$("#editDepartment").text()}">`));
        $("#editEmail").replaceWith($(`<input name="Input.UserEmail" type="email" value="${$("#editEmail").text()}">`));
        $("#editPassword").replaceWith($(`<input name="Input.UserPassword" type="password" value="${$("#editPassword").text()}">`));
        if (isFaculty) {
            $("#editSubject").replaceWith(`<input name="Input.UserSubject" value="${$("#editSubject").text()}">`);
        } else {
            $("#editGrade").replaceWith($(`<input name="Input.UserGrade" type="number" value="${$("#editGrade").text()}">`));
        }
    });
});