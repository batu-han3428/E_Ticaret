export function toggleLoader() {
    if (!$("#preloder").is(":visible")) {
        $("#preloder").fadeIn(0);
    }
    else {
        $("#preloder").delay(200).fadeOut("slow");
    }
}