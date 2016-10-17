function readDefinitionsFile(input) {


    $("#txtFileName").text(input.files[0].name);
    if (input.files && input.files[0]) {
        var reader = new window.FileReader();
        $(".blockOverlay").css('display', 'block');
        reader.onload = function (e) {
            try {
                loadFile(e.target.result, ",");
            } catch (err) {
                $(".blockOverlay").css('display', 'none');
                // Clear file browser
                clearFileBrower();
                swal("Error", err, "error");
            }
        };
        reader.readAsText(input.files[0]);
    }
    input = $("#ottiRunFile");
    input.replaceWith(input.val('').clone(true));
}

function loadFile(text, seperator) {
    debugger;
    var lines = [];
    lines = text.split(/[\r\n]+/g); // tolerate both Windows and Unix linebreaks    
    if (lines.length < 2) {
        throw ("No data in file");
    }
    alert(lines);
    ////columns = [];
    ////var headers = lines[0].split(seperator);
    ////var firstDataRow = lines[1].split(seperator);
    ////for (var index = 0; index < firstDataRow.length; index++) {
    ////    if (isNotEmptyNullOrUndefined(firstDataRow[index])) {
    ////        if (!isNotEmptyNullOrUndefined(headers[index]))
    ////            columns.push("AsofDate");
    ////        else {
    ////            if (/^[0-9]*$/.test(headers[index]))
    ////                columns.push(headers[index]);
    ////            else {
    ////                throw ("Wrong format");
    ////            }
    ////        }
    ////    }
    ////}

    //read the file and get all the data.
    ////dataTableArray = [];
    ////for (var i = 1; i < lines.length; i++) {
    ////    var line = lines[i];
    ////    if (!isNotEmptyNullOrUndefined(line))
    ////        continue;
    ////    if (line.match(/^#/))
    ////        continue;
    ////    var lineData = line.split(seperator);

    ////    //create data row
    ////    var datarow = [];
    ////    var lastIndex = undefined;
    ////    var status = "";
    ////    for (var index = 0; index < lineData.length; index++) {
    ////        if (lineData[index] == "") { //Remove this condition
    ////            if (lastIndex == undefined)
    ////                lastIndex = index;

    ////            if (lastIndex == (index - 1)) {
    ////                if (lineData[index] != "") {
    ////                    lastIndex = undefined;
    ////                    throw ("Wrong data/Invalid format at row " + (i + 1));
    ////                }
    ////            }
    ////        }
    ////        if (lastIndex == (index - 1)) {
    ////            if (lineData[index] != "") {
    ////                lastIndex == undefined;
    ////                throw ("Wrong data/Invalid format at row " + (i + 1));
    ////            }

    ////        }

    ////        datarow.push(lineData[index]);
    ////    }
    ////    //if (datarow.length != columns.length) {
    ////    //    throw ("Wrong data/Invalid format at row " + (i + 1));
    ////    //}


    ////    // push data in final array
    ////    dataTableArray.push(datarow);
    ////}
  ////alert(dataTableArray);
    //dtBackFillingData = refreshDataTable(dtBackFillingData, $("#dtBackFillingData"), columns, []);
    //dtBackFillingData.fnClearTable();
    //if (dataTableArray.length > 0)
    //    dtBackFillingData.fnAddData(dataTableArray);

    //    $.each(dataTableArray, function (index, array) {
    //        $.each(defIdArr, function (i, defId) {
    //            defIdMap[defId][array[i * 2]] = array[i * 2 + 1];
    //        });
    //    })    

    $(".blockOverlay").css('display', 'none');
    // RefreshTable();
}
