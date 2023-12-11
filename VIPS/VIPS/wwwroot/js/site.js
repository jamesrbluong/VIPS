// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.


document.getElementById("showAllButton").addEventListener("click", function () {
    showAllObjects();
});

document.getElementById("showFilteredButton").addEventListener("click", function () {
    showNonDuplicateObjects();
});

function showAllObjects() {
    // Display all table rows
    console.log("showAllObjects");
    let table = document.getElementById("objectTable");
    let rows = table.getElementsByTagName("tr");
    for (let i = 0; i < rows.length; i++) {
        rows[i].style.display = "table-row";
    }
}

function showNonDuplicateObjects() {
    // Hide rows where Duplicate is true (non-visible column)
    console.log("showNonDuplicateObjects");
    let table = document.getElementById("objectTable");
    let rows = table.getElementsByTagName("tr");
    for (let i = 1; i < rows.length; i++) { // Start from 1 to skip header row
        let cells = rows[i].getElementsByTagName("td");

        // Check if there are enough cells and if the cell at index 54 exists
        if (cells.length > 13 && typeof cells[13].textContent !== 'undefined') {
            let isDuplicate = cells[13].textContent.trim() === "true";
            rows[i].style.display = isDuplicate ? "none" : "table-row";
        }
    }
}


// Function to show row by contractID
function showRowByContractID1(contractID) {
    // Hide all rows
    $("#detailTable tbody tr").hide();

    // Show the row with the specified ContractID
    var matchingRows = $("#detailTable tbody tr").filter(function () {
        return $(this).find("td:first-child").text().trim() === contractID;
    }).show();

    // Create a new table with the first 10 columns
    var newTable = $("<table>").attr("id", "newDetailTable").addClass("custom-table");

    // Clone the headers from #detailTable
    var headers = $("#detailTable thead tr th:lt(10)").clone();

    // Append the headers to the new table
    newTable.append($("<thead>").append(headers));

    // Clone and append the matching rows with the first 10 columns to the new table body
    newTable.append($("<tbody>").append(matchingRows.clone().find("td:lt(10)")));

    // Append the new table to the container (change 'containerId' to your actual container ID)
    $("#containerId").html(newTable);

    // Show the new table
    newTable.show();
}

function showRowByContractID2(contractID) {
    // Hide all rows
    $("#detailTable tbody tr").hide();

    // Show the row with the specified ContractID
    var matchingRows = $("#detailTable tbody tr").filter(function () {
        return $(this).find("td:first-child").text().trim() === contractID;
    }).show();

    // Create a new table with the first 10 columns
    var newTable = $("<table>").attr("id", "newDetailTable").addClass("custom-table");

    // Clone the headers from #detailTable
    // Clone the headers from #detailTable, selecting headers 11 to 21
    var headers = $("#detailTable thead tr th:gt(9):lt(10)").clone();

    // Append the headers to the new table
    newTable.append($("<thead>").append(headers));

    // Clone and append the matching rows with columns 11 to 21 to the new table body
    newTable.append($("<tbody>").append(matchingRows.clone().find("td:gt(9):lt(10)")));

    // Append the new table to the container (change 'containerId' to your actual container ID)
    $("#containerId2").html(newTable);

    // Show the new table
    newTable.show();
}

function showRowByContractID3(contractID) {
    $("#detailTable tbody tr").hide();

    var matchingRows = $("#detailTable tbody tr").filter(function () {
        return $(this).find("td:first-child").text().trim() === contractID;
    }).show();

    var newTable = $("<table>").attr("id", "newDetailTable").addClass("custom-table");

    var headers = $("#detailTable thead tr th:gt(19):lt(10)").clone();

    newTable.append($("<thead>").append(headers));

    newTable.append($("<tbody>").append(matchingRows.clone().find("td:gt(19):lt(10)")));

    $("#containerId3").html(newTable);

    newTable.show();
}

function showRowByContractID4(contractID) {
    $("#detailTable tbody tr").hide();

    var matchingRows = $("#detailTable tbody tr").filter(function () {
        return $(this).find("td:first-child").text().trim() === contractID;
    }).show();

    var newTable = $("<table>").attr("id", "newDetailTable").addClass("custom-table");

    var headers = $("#detailTable thead tr th:gt(29):lt(10)").clone();


    newTable.append($("<thead>").append(headers));

    newTable.append($("<tbody>").append(matchingRows.clone().find("td:gt(29):lt(10)")));

    $("#containerId4").html(newTable);

    newTable.show();
}

function showRowByContractID5(contractID) {
    $("#detailTable tbody tr").hide();

    var matchingRows = $("#detailTable tbody tr").filter(function () {
        return $(this).find("td:first-child").text().trim() === contractID;
    }).show();

    var newTable = $("<table>").attr("id", "newDetailTable").addClass("custom-table");

    var headers = $("#detailTable thead tr th:gt(39):lt(10)").clone();

    newTable.append($("<thead>").append(headers));

    newTable.append($("<tbody>").append(matchingRows.clone().find("td:gt(39):lt(10)")));

    $("#containerId5").html(newTable);

    newTable.show();
}

function showRowByContractID6(contractID) {
    $("#detailTable tbody tr").hide();

    var matchingRows = $("#detailTable tbody tr").filter(function () {
        return $(this).find("td:first-child").text().trim() === contractID;
    }).show();

    var newTable = $("<table>").attr("id", "newDetailTable").addClass("custom-table");

    var headers = $("#detailTable thead tr th:gt(49):lt(10)").clone();

    newTable.append($("<thead>").append(headers));

    newTable.append($("<tbody>").append(matchingRows.clone().find("td:gt(49):lt(10)")));

    $("#containerId5").html(newTable);

    newTable.show();
}