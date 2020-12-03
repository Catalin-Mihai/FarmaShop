const sidebarToggleCookieName = "sidebar_toggle";
let sidebarToggle;

$(document).ready(function () {
    
    $('#sidebarCollapse').on('click', function () {
        $('#sidebar').toggleClass('active');
    });

});