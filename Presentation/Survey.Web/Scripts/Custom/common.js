function ajaxindicatorstart(text) {
    if (text == undefined || text == null || text == "") {
        text = 'Processing...Please wait.'
    }
    if (jQuery('body').find('#resultLoading').attr('id') != 'resultLoading') {
        jQuery('body').append('<div id="resultLoading" style="display:none"><div><i class="fa fa-spinner"></i><div>' + text
            + '</div></div><div class="bg"></div></div>');
    }
    jQuery('#resultLoading').css({
        'width': '100%',
        'height': '100%',
        'position': 'fixed',
        'z-index': '10000000',
        'top': '0',
        'left': '0',
        'right': '0',
        'bottom': '0',
        'margin': 'auto'
    });
    jQuery('#resultLoading .bg').css({
        'background': '#000000',
        'opacity': '0.7',
        'width': '100%',
        'height': '100%',
        'position': 'absolute',
        'top': '0'
    });
    jQuery('#resultLoading>div:first').css({
        'width': '250px',
        'height': '75px',
        'text-align': 'center',
        'position': 'fixed',
        'top': '0',
        'left': '0',
        'right': '0',
        'bottom': '0',
        'margin': 'auto',
        'font-size': '16px',
        'z-index': '10',
        'color': '#ffffff'
    });
    jQuery('#resultLoading .bg').height('100%');
    jQuery('#resultLoading').fadeIn(300);
    jQuery('body').css('cursor', 'wait');
}
function ajaxindicatorstop() {
    jQuery('#resultLoading .bg').height('100%');
    jQuery('#resultLoading').fadeOut(300);
    jQuery('body').css('cursor', 'default');
}
function ClearMessage() {
    if (document.getElementById("divMessage") != null) {
        document.getElementById("divMessage").innerText = "";
        document.getElementById("divMessage").style.display = "none";
    }
}
function showErrorMessage(message) {
    $("#MessageModal").modal('show');

    $('#divMessage').removeClass('alert-info').addClass('alert-danger')
        .empty().show().append("<p>");
    $('#divMessage p').html("");
    $('#divMessage p').html(message);
}
function showSuccessMessage(message) {
    $("#MessageModal").modal('show');
    $('#divMessage').removeClass('alert-danger').addClass('alert-info')
    .empty().show().append("<p>");
    $('#divMessage p').html("");
    $('#divMessage p').html(message);
}


function hideMessage(message) {
    $("#MessageModal").modal('hide');
    $('#divMessage').empty();
}

function logErrorMessage(message) {
    if (window.console)
        console.log(message);
}
$(document).ready(function () {
    $(".dropdown, .btn-group").hover(function () {
        var dropdownMenu = $(this).children(".dropdown-menu");
        if (dropdownMenu.is(":visible")) {
            dropdownMenu.parent().toggleClass("open");
        }
    });
    //collapse
    var selectIds = $('#panel1, #panel2, #panel3');
    $(function ($) {
        selectIds.on('show.bs.collapse hidden.bs.collapse', function () {
            $(this).prev().find('.glyphicon').toggleClass('glyphicon-plus glyphicon-minus');
        })
    });
});