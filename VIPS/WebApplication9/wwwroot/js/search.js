function autocomplete(inp, arr) {
    
    var currentFocus;
    /*execute when text is in the field*/
    inp.addEventListener("input", function (e) {
        var a, b, i, val = this.value;
        /*close autocompleted lists*/
        closeAllLists();
        if (!val) { return false; }
        currentFocus = -1;
        /*DIV Element Creation*/
        a = document.createElement("DIV");
        a.setAttribute("id", this.id + "autocomplete-list");
        a.setAttribute("class", "autocomplete-items");
       
        this.parentNode.appendChild(a);
        /*for(each item in array)*/
        for (i = 0; i < arr.length; i++) {
            /*check if text matches uppercase*/
            if (arr[i].substr(0, val.length).toUpperCase() == val.toUpperCase()) {
                /*create a DIV element for each matching element:*/
                b = document.createElement("DIV");
                /*make the matching letters bold:*/
                b.innerHTML = "<strong>" + arr[i].substr(0, val.length) + "</strong>";
                b.innerHTML += arr[i].substr(val.length);
                /*input field to hold current array item val*/
                b.innerHTML += "<input type='hidden' value='" + arr[i] + "'>";
                /*function for click*/
                b.addEventListener("click", function (e) {
                    /*autocomplete value*/
                    inp.value = this.getElementsByTagName("input")[0].value;
                    
                    closeAllLists();
                });
                a.appendChild(b);
            }
        }
    });
    
    inp.addEventListener("keydown", function (e) {
        var x = document.getElementById(this.id + "autocomplete-list");
        if (x) x = x.getElementsByTagName("div");
        if (e.keyCode == 40) {
           /*ArrowDown = More Focus*/
            currentFocus++;
            /*More Visbility*/
            addActive(x);
        } else if (e.keyCode == 38) { 
            currentFocus--;
           
            addActive(x);
        } else if (e.keyCode == 13) {
            /*Make Enter Key Prevent Enter*/
            e.preventDefault();
            if (currentFocus > -1) {
                
                if (x) x[currentFocus].click();
            }
        }
    });
    function addActive(x) {
        /*Active Item Classification*/
        if (!x) return false;
        /*Remving Active on all items*/
        removeActive(x);
        if (currentFocus >= x.length) currentFocus = 0;
        if (currentFocus < 0) currentFocus = (x.length - 1);
        
        x[currentFocus].classList.add("autocomplete-active");
    }
    function removeActive(x) {
        
        for (var i = 0; i < x.length; i++) {
            x[i].classList.remove("autocomplete-active");
        }
    }
    function closeAllLists(elmnt) {
        
        var x = document.getElementsByClassName("autocomplete-items");
        for (var i = 0; i < x.length; i++) {
            if (elmnt != x[i] && elmnt != inp) {
                x[i].parentNode.removeChild(x[i]);
            }
        }
    }
    /*Execute when clicked*/
    document.addEventListener("click", function (e) {
        closeAllLists(e.target);
    });
}

/*An array containing all the department*/
var department = ["Brooks College of Health", "Brooks", "Health", "Coggin College of Business", "Coggin", "Business", "College of Arts and Sciences", "Art", "Science", "College of Computing, Engineering, and Construction", "Computing", "Engineering", "Construction", "College of Education and Human Services", "Education", "Human Services", "Hicks Honor College", "Honors"];

/*initiate the autocomplete function on the "myInput" element, and pass along the countries array as possible autocomplete values:*/
autocomplete(document.getElementById("myInput"), department);