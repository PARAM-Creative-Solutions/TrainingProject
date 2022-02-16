function MessageBoxOK_Cancel(message) {
   
    var f = { 'dir1': 'down', 'dir2': 'right', 'modal': true ,'overlayClose': true};
    var notify = new PNotify(
        {
            title: 'Success!',
            text: message,
            type: 'success',
            icon: 'glyphicon glyphicon-info-sign',
            addclass: 'stack-modal',
            stack: f,
            hide: false,
            confirm:
            {
                confirm: true
            },
            buttons:
            {
                closer: false,
                sticker: false
            },
            history:
            {
                history: false
            },
            addclass: 'stack-modal',

        });
    return notify;
}

function InformationBoxOK_Cancel(message) {
    var f = { 'dir1': 'down', 'dir2': 'right', 'modal': true };
    var notify = new PNotify(
        {
            title: 'Information!',
            text: message,
            type: 'info',
            icon: 'glyphicon glyphicon-info-sign',
            addclass: 'stack-modal',
            stack: f,
            hide: false,
            confirm:
            {
                confirm: true
            },
            buttons:
            {
                closer: false,
                sticker: false
            },
            history:
            {
                history: false
            },
            addclass: 'stack-modal',

        });
    return notify;
}


function MessageBoxSuccess(message) {
    return new PNotify(
      {
          title: 'Success!',
          text: message,
          type: 'Success',
          icon: 'glyphicon glyphicon-info-sign',
          addclass: 'stack-modal',
          stack: { 'dir1': 'down', 'dir2': 'right', 'modal': true },
          hide: false,
          confirm:
          {
              confirm: true
          }
      });
}

function MessageBoxError(message) {
    return new PNotify(
      {
          title: 'Error!',
          text: message,
          type: 'error',
          icon: 'glyphicon glyphicon-info-sign',
          addclass: 'stack-modal',
          stack: { 'dir1': 'down', 'dir2': 'right', 'modal': true },
          hide: false,
          confirm:
          {
              confirm: true
          }
      });
}

function MessageBoxOk(message)
{
   var notify = new PNotify(
   {
     title: 'Success!',
     text: message,
     type: 'success',
     icon: 'glyphicon glyphicon-info-sign',
     addclass: 'stack-modal',
     stack:  { 'dir1': 'down', 'dir2': 'right', 'modal': true },
     hide: false,
     confirm:
     {
        confirm: true,
        buttons: [{
        text: 'Ok',
        addClass: 'btn-primary',                                               
     }, null]
   },
   buttons:
   {
      closer: false,
      sticker: false
   },
   history:
   {
      history: false
   },
   addclass: 'stack-modal',
 });
   return notify;
}

function MessageBoxOk_Flash(message) {
    var notify = new PNotify(
    {
        title: 'Success!',
        text: message,
        type: 'success',
        icon: 'glyphicon glyphicon-info-sign',
        addclass: 'stack-modal',
        stack: { 'dir1': 'down', 'dir2': 'right', 'modal': true },
        hide: true,
        delay: 1000,
        addclass: 'stack-modal',
    });
    return notify;
}

function MessageBoxInput(message) {
    return (new PNotify({
        title: 'Input Needed',
        text: message,
        icon: 'glyphicon glyphicon-question-sign',
        addclass: 'stack-modal',
        stack: { 'dir1': 'down', 'dir2': 'right', 'modal': true },
        hide: false,
        confirm: {
            prompt: true
        },
        buttons: {
            closer: false,
            sticker: false
        },
        history: {
            history: false
        }
    }));
}