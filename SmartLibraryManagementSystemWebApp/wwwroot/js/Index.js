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
            let bookLink = $($(this).children()[1]);
            let bookLinkText = bookLink.text();
            let bookLinkTextSeperator = bookLinkText.indexOf("-");
            let bookName = bookLinkText.slice(0, bookLinkTextSeperator-1);
            if (!bookName.includes(searchVal)) $(this).hide();
            else $(this).show();
        });
    });
});