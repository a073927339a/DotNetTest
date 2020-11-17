function getInterviewedComments() {
    var list = [];
    $.ajax({
        type: 'Get',
        cache: false,
        async: false,
        url: '/Interview/ListInterviewedComments',
        dataType: 'json',
        success: function (response) {
            list = response;
        },
        error: function (response) {
            showError(response.responseJSON);
        },
    });
    return list;
}

function getSchool() {
    var def = $.Deferred();
    $.ajax({
        type: 'Get',
        cache: false,
        url: '@Url.Action("ListSchools", "Resume")',
        dataType: 'json',
        success: function (response) {
            def.resolve(response);
        },
        error: function (response) {
            def.reject();
            showError(response.responseJSON);
        },
    });
    return def;
}


function getSchooDept() {
    var def = $.Deferred();
    $.ajax({
        type: 'Get',
        cache: false,
        url: '/Resume/ListSchoolDepts',
        dataType: 'json',
        success: function (response) {
            def.resolve(response);
        },
        error: function (response) {
            def.reject();
            showError(response.responseJSON);
        },
    });
    return def;
}


function getJobCategory() {
    var def = $.Deferred();
    $.ajax({
        type: 'Get',
        cache: false,
        url: '/JobCategory/List',
        dataType: 'json',
        success: function (response) {
            def.resolve(response);
        },
        error: function (response) {
            def.reject();
            showError(response.responseJSON);
        },
    });
    return def;
}

function getJob() {
    var def = $.Deferred();
    $.ajax({
        type: 'Get',
        cache: false,
        url: '/Job/List',
        dataType: 'json',
        success: function (response) {
            def.resolve(response);
        },
        error: function (response) {
            def.reject();
            showError(response.responseJSON);
        },
    });
    return def;
}

function getGradeList() {
    var def = [];
    $.ajax({
        type: 'Get',
        cache: false,
        async: false,
        url: '/Resume/ListGrades',
        dataType: 'json',
        success: function (response) {
            def = response;
        },
        error: function (response) {
            showError(response.responseJSON);
        },
    });
    return def;
}


function ListInterviewedDetails(hids) {
    var def = $.Deferred();
    if (hids == null || hids.length == 0) {
        def.resolve([]);
        return def;
    }

    $.ajax({
        type: 'Post',
        cache: false,
        contentType: "application/json; charset=utf-8",
        dataType: 'json',
        url: '/Resume/ListInterviewedDetails',
        data: JSON.stringify({ "hids": hids }),
        success: function (response) {
            def.resolve(response);
        },
        error: function (response) {
            showError(response);
            def.reject([]);
        },
    });
    return def;
}

function ListHunterQueryJob(hids) {
    var def = $.Deferred();
    if (hids == null || hids.length == 0) {
        def.resolve([]);
        return def;
    }
    $.ajax({
        type: 'Post',
        cache: false,
        contentType: "application/json; charset=utf-8",
        dataType: 'json',
        url: '/Resume/ListHunterQueryJob',
        data: JSON.stringify({ "hids": hids }),
        success: function (response) {
            def.resolve(response);
        },
        error: function (response) {
            showError(response);
            def.reject([]);
        },
    });
    return def;
}

// 取得同為待邀約中的主管
function ListWaitingInterviewBosses(hids) {
    var def = $.Deferred();
    if (hids == null || hids.length == 0) {
        def.resolve([]);
        return def;
    }
    $.ajax({
        type: 'POST',
        cache: false,
        contentType: "application/json; charset=utf-8",
        dataType: 'json',
        url: '/Interview/ListWaitingInterviewBosses',
        data: JSON.stringify({ "hids": hids }),
        success: function (response) {
            def.resolve(response);
        },
        error: function (response) {
            showError(response);
            def.reject([]);
        },
    });
    return def;
}