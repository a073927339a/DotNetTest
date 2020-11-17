function showMsg(sts, msg) {
    $('.toast-container').html("");
    sts = sts.toUpperCase();

    if (sts == "SUCCESS") {
        $('#divMsg').toastmessage('showToast', {
            text: msg,
            position: 'top-center',
            type: 'success',
            sticky: true
        });
    }
    else if (sts == "ERROR") {
        $('#divMsg').toastmessage('showToast', {
            text: msg,
            position: 'top-center',
            type: 'error',
            sticky: true
        });
    }
    else if (sts == "NOTICE") {
        $('#divMsg').toastmessage('showToast', {
            text: msg,
            position: 'top-center',
            type: 'notice',
            sticky: true
        });
    }
}
