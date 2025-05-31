$(document).ready(function() {
    $(document).ajaxError(function(event, xhr, settings) {
        if (xhr.status === 401 || xhr.status === 403) {
            window.location.href = '/Account/Login?returnUrl=' + encodeURIComponent(window.location.pathname);
        }
    });
});