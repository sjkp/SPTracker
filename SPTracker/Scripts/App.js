'use strict';

var context = SP.ClientContext.get_current();
var user = context.get_web().get_currentUser();

// This code runs when the DOM is ready and creates a context object which is needed to use the SharePoint object model
$(document).ready(function () {
    getUserName();
    SP.SOD.executeFunc('sp.js', 'SP.ClientContext', function () {

        var appContext = SP.ClientContext.get_current();

        var appWeb = appContext.get_web();
        var hostWebUrl = decodeURIComponent(getQueryStringParameter("SPHostUrl"));
        var appWebUrl = decodeURIComponent(getQueryStringParameter("SPAppWebUrl"));

        var hostWebContext=new SP.AppContextSite(appContext,hostWebUrl);

        var web = hostWebContext.get_site().get_rootWeb();
        appContext.load(web);

        appContext.executeQueryAsync(function () {
            $('#hostweb').text(web.get_title());
        },
        function (err) {
            console.log(err)
        });

    });
});

function getQueryStringParameter(urlParameterKey) {
    var params = document.URL.split('?')[1].split('&');
    var strParams = '';
    for (var i = 0; i < params.length; i = i + 1) {
        var singleParam = params[i].split('=');
        if (singleParam[0] == urlParameterKey)
            return decodeURIComponent(singleParam[1]);
    }
}

// This function prepares, loads, and then executes a SharePoint query to get the current users information
function getUserName() {
    context.load(user);
    context.executeQueryAsync(onGetUserNameSuccess, onGetUserNameFail);
}

// This function is executed if the above call is successful
// It replaces the contents of the 'message' element with the user name
function onGetUserNameSuccess() {
    $('#message').text('Hello ' + user.get_title());
}

// This function is executed if the above call fails
function onGetUserNameFail(sender, args) {
    alert('Failed to get user name. Error:' + args.get_message());
}
