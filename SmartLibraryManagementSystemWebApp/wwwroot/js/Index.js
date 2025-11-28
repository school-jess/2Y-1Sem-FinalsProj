$(document).ready(function() {
    $("#searchBar").on("input", function() {
        let searchVal = $(this).val();
        if (searchVal === "") {
            $("ol li").each(function(elem){
                $(this).show();
            });
            return;
        }
        $("ol li").each(function(elem) {
            let aElem = $($(this).children()[0]);
            let divElem = $(aElem.children()[1]);
            let bookNameContainer = $(divElem.children()[0]);
            let bookLinkText = bookNameContainer.text();
            if (!bookLinkText.includes(searchVal)) $(this).hide();
            else $(this).show();
            console.log(bookLinkText.includes(searchVal));
        });
    });
});