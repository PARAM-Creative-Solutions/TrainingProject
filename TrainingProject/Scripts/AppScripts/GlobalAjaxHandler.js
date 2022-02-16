/// <reference path="PNotifyMessages.js" />
$(document).ready(function () {
  
    $(function () {
        //setup ajax error handling
        $.ajaxSetup({ cache: false });
       // $.ajaxSetup();

        $(document).ajaxStart(function () {
            $.blockUI({ message: '<h1><i class="fa fa-circle-o-notch fa-spin" style="font-size:48px;color:red"></i> Just a moment...</h1>' });
        });
        $(document).ajaxStop(function () {
            $.unblockUI();
        });
        $(document).ajaxError(function(x, status, error) {
            $.unblockUI();

            switch (status.status) {
                case 590:
                    var Notify = MessageBoxOk("Sorry, Your Login Session Is Expired. Do You Want To Login Again?");
                    Notify.get().on('pnotify.confirm', function () {
                        window.location.href = "/Account/Login";
                    });
                    break;
                default:
                    MessageBoxError("An error occurred while processing your request");// + status.StatusCode + "\nError: " + error.ResponseText);

            }
           
        });

    });
});