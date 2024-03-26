function searchContracts() {
    var searchString = document.getElementById('searchBox').value.toLowerCase();


var departmentMapping = {
    "brooks college of health": "nursing",
"coggin college of business": "business",
"college of arts and sciences": "arts",
"college of computing": "computing",
"college of engineering and construction": "engineering",
"college of education and human services": "education",
"college of construction": "construction",
"college of education": "education",
"college of human services": "human services"

    };


if (departmentMapping.hasOwnProperty(searchString)) {
    searchString = departmentMapping[searchString];
    }

window.location.href = '/Search/SearchView?searchString=' + encodeURIComponent(searchString);
}
