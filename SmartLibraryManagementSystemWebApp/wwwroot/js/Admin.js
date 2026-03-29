$(document).ready(() => {
    let userName = "";
    let hasFine = false;
    let hasLoan = false;
    let isAdmin = false;
    $(document).on("click", ".edit-user", function (e) {
        e.preventDefault();
        let userId = $(this).data("userId");
        $(this).hide();
        $(`#isDelete-${userId}`).val("0");
        $(`#cancelEdit-${userId}`).show();
        $(`#submitEdit-${userId}`).show();
        userName = $(`#userName-${userId}`).text().trim();
        hasFine = $(`#hasFine-${userId}`).is(":checked");
        hasLoan = $(`#hasLoan-${userId}`).is(":checked");
        isAdmin = $(`#isAdmin-${userId}`).is(":checked");
        $(`#userName-${userId}`).replaceWith($(`<input id="userName-${userId}" form="editUserForm-${userId}" name="Input.UserName" value="${$(`#userName-${userId}`).text().trim()}" />`));
        $(`#hasFine-${userId}`).prop("disabled", false);
        $(`#hasLoan-${userId}`).prop("disabled", false);
        $(`#isAdmin-${userId}`).prop("disabled", false);
    });

    $(document).on("click", ".cancelEditUser", function (e) {
        e.preventDefault();
        $(this).hide();
        let userId = $(this).data("userId");
        $(`#submitEdit-${userId}`).hide();
        $(`#editUser-${userId}`).show();
        let newUserNameElem = $(`<div id="userName-${userId}">`);
        newUserNameElem.text(userName);
        $(`#userName-${userId}`).replaceWith(newUserNameElem);
        $(`#hasFine-${userId}`).prop("disabled", true);
        $(`#hasFine-${userId}`).prop("checked", hasFine);
        $(`#hasLoan-${userId}`).prop("disabled", true);
        $(`#hasLoan-${userId}`).prop("checked", hasLoan);
        $(`#isAdmin-${userId}`).prop("disabled", true);
        $(`#isAdmin-${userId}`).prop("checked", isAdmin);
    });
});