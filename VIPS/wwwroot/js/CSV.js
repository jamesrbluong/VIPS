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
        if (cells.length > 8 && typeof cells[8].textContent !== 'undefined') {
            let isDuplicate = cells[8].textContent.trim() === "true";
            rows[i].style.display = isDuplicate ? "none" : "table-row";
        }
    }
}