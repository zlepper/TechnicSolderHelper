$(document).ready(function () {
    $("td").click(function () {
        window.prompt("Copy to clipboard: Ctrl+c, Enter", $(this).text());
    })
})