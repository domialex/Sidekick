//import { ipcRenderer, remote } from "electron";

//let windowX;
//let windowY;
//let dragging = false;

//const a = () => {
//    $('html, body').mousedown(function (e) {
//        ipcRenderer.send('asynchronous-message', 'down')
//        dragging = true;
//        windowX = e.pageX;
//        windowY = e.pageY;
//    });

//    $(window).mousemove(function (e) {
//        e.stopPropagation();
//        e.preventDefault();

//        if (dragging) {
//            try {
//                remote.BrowserWindow.getFocusedWindow().setPosition(e.screenX - windowX, e.screenY - windowY);
//            } catch (err) {
//                console.log(err);
//            }
//        }
//    });

//    $('.html, body').mouseup(function () {
//        dragging = false;
//        ipcRenderer.send('asynchronous-message', 'up')
//    });
//};

//a();
