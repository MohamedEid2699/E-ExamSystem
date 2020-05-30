
function adjustWindowSize(wnd) {
    var opts = wnd.options;
    //ipad pro
    if ($(window).width() == 1024) {
        opts.width = "68%";
    }
    //laptop
    else if ($(window).width() > 1024) {
        opts.width = "50%";
    }
    else {
        //mobile phone
        opts.width = "95%";
    }
    opts.height = "auto";

    wnd.setOptions(opts);
}

 // set grid's height to auto when data bound on the grid
function Grid_onDataBound(gridId) {
    var grid = "#" + gridId + " .k-grid-content";
    $(grid).attr("style", "height:auto");   
}

// display ModelState error alert for create and update record
function error_handler(e) {
    if (e.errors) {
        var message = "Failed\n";
        $.each(e.errors, function (key, value) {
            if ('errors' in value) {
                $.each(value.errors, function () {
                    message += "\u2022 " + this + "\n";
                });
            }
        });
        var errorMsg = document.getElementById('errorMessage');
        errorMsg.innerText = message;
        $('#errorMessage').css('display', 'block');
        $('#errorMessage').delay(10000).fadeOut(300); 
    }
}

function ShowSuccessMessage(divId) {
    var messageDivId = "#" + divId;
    window.scrollTo(0, 0);
    $(messageDivId).css('display', 'block');
    $(messageDivId).delay(1500).fadeOut(300);
}

// after error displayed, refresh the grid
function delete_sync_handler(e) {
    this.read();
}

function onRequestEnd(e) {
    if (e.type == "create" && !e.response.Errors) {
        ShowSuccessMessage('RecordCreatedSuccess');
    }

    if (e.type == "update" && !e.response.Errors) {
        ShowSuccessMessage('RecordUpdatedSuccess');
    }

    if (e.type == "destroy" && !e.response.Errors) {
        ShowSuccessMessage('RecordDeletedSuccess');
    }
    $('#errorMessage').delay(10000).fadeOut(300); 
}

//close the popup window when user click cancel button in popup window
function CloseWindow(windowId) {
    var id = "#" + windowId;
    var wnd = $(id).data("kendoWindow");
    wnd.close();
}

function iterate(object) {
    var html = '<ul style="padding:0;">';
    if (object !== null && object != undefined) {
        if (object.length > 0) {
            object.forEach(function (data) {
                html += '<li><i class="fa fa-circle" style="font-size:8px"></i> ' + data + '</li>';
            });
        } else {
            return '-';
        }
    } else {
        html += '<li>-</li>';
    }
    html += '</ul>';
    return html;
}

function GetUserRole() {
    $.ajax({
        type: "GET",
        url: '/Account/GetLoginUserRole',
        success: function (response) {
            return response;
        }
    });
}

//used in table which date that need to be filtered
function dateFilter(element) {
    element.kendoDatePicker({
        format: "yyyy-MM-dd"
    });
}

//used in table which has class name
function ClassNameFilter(e) {
    $.ajax({
        url: '/Class/GetClassNameList',
        type: "GET",
        success: function (result) {
            e.kendoDropDownList({
                optionLabel: 'Please select...',
                dataTextField: 'Text',
                dataValueField: 'Value',
                filter: "contains",
                dataSource: result
            });
        }
    });
}
//used in table which has exam name
function ExamNameFilter(e) {
    $.ajax({
        url: '/Exam/TeacherGetExamNameList',
        type: "GET",
        success: function (result) {
            e.kendoDropDownList({
                optionLabel: 'Please select...',
                dataTextField: 'Text',
                dataValueField: 'Value',
                filter: "contains",
                dataSource: result
            });
        }
    });
}
//used in table which has student exam status (passed / failed)
function StudentExamStatusFilter(e) {
    $.ajax({
        url: '/Exam/GetStudentExamStatus',
        type: "GET",
        success: function (result) {
            e.kendoDropDownList({
                optionLabel: 'Please select...',
                filter: "contains",
                dataSource: result
            });
        }
    });
}
//used in exam history for student page, when reset button clicked, set dropdownlist value = "" and refresh the grid
function ResetFilter(dropdownId, gridId) {
    var dropdown = $('#' + dropdownId).data('kendoDropDownList');
    dropdown.value('');
    var gridid = '#' + gridId;
    var grid = $(gridid).data('kendoGrid');
    grid.dataSource.filter([]);
}







