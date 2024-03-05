

var options = {
    autoResize: true,
    height: '100%',
    nodes: {
        font: {
            color: "#FFFFFF",
            align: 'center',
            face: "open sans",
            size: 62
        },
        fixed: false,
        widthConstraint: 500, // 200
        heightConstraint: 250, // 100
        shape: "circle",
        shapeProperties: {
            interpolation: false    // 'true' for intensive zooming
        },
        mass: 4,
        color: "rgba(10, 35, 63,1)"

    },
    physics: {
        enabled: true,
        barnesHut: {
            gravitationalConstant: -2000,
            centralGravity: 0.1,
            springLength: 2000,
            springConstant: 0.01,
            damping: 0.15,
            avoidOverlap: 1
        }
    },
    edges: {
        smooth: {
            type: 'continuous', // Set type to 'continuous' for straight lines without curves
            roundness: 0 // Set roundness to 0 to remove any rounding effect
        },
        width: 3,
        color: "black"
    },
    layout: {
        improvedLayout: false
    },
    interaction: {
        dragNodes: false,
        navigationButtons: true,
        selectConnectedEdges: false,

        hideNodesOnDrag: false,
        hideEdgesOnDrag: false,
        hideEdgesOnZoom: false,
    }



};

var network;

var schoolUrl = '/Visualization/GetSchoolData';
var deptUrl = '/Visualization/GetDepartmentData';
var partnerUrl = '/Visualization/GetPartnerData';
var edgeUrl = '/Visualization/GetEdgeData';

var nodesArray = [];
var edgesArray = [];

// var dataUrl = '/Visualization/GetPartnerData';
console.log("this is loadVisualization.js");
document.getElementById("response").innerHTML = "Updating the visualization graph...";

updateProgressBar(0);

$.when(
    $.ajax({
        url: schoolUrl,
        type: 'GET',
        dataType: 'json',
        success: function (data) {
            var schoolArray = [];
            for (var i = 0; i < data.length; i++) {
                schoolArray.push({
                    id: "s" + data[i].schoolId,
                    label: data[i].name,
                    type: "school",
                    color: "purple",
                    width: 500,
                    height: 250


                });
            }
            nodesArray = nodesArray.concat(schoolArray);
            updateProgressBar(15);
        },
        error: function (error) {
            console.error('Error fetching data:', error);
        }
    }),
    $.ajax({
        url: deptUrl,
        type: 'GET',
        dataType: 'json',
        success: function (data) {
            var deptArray = [];
            for (var i = 0; i < data.length; i++) {
                deptArray.push({
                    id: "d" + data[i].departmentId,
                    label: data[i].name,
                    schoolId: data[i].schoolId,
                    type: "department",
                    width: 400,
                    height: 200
                });
            }
            nodesArray = nodesArray.concat(deptArray);
            updateProgressBar(25);
        },
        error: function (error) {
            console.error('Error fetching data:', error);
        }
    }),
    $.ajax({
        url: partnerUrl,
        type: 'GET',
        dataType: 'json',
        success: function (data) {
            var partnerArray = [];
            for (var i = 0; i < data.length; i++) {
                partnerArray.push({
                    id: "p" + data[i].partnerId,
                    label: data[i].name,
                    type: "partner",
                    color: "red",
                    width: 300,
                    height: 150
                });
            }
            nodesArray = nodesArray.concat(partnerArray);
            updateProgressBar(40);
        },
        error: function (error) {
            console.error('Error fetching data:', error);
        }
    })
).then(function () {
    var container = document.getElementById('mynetwork');

    var data = {
        nodes: new vis.DataSet(nodesArray),
        edges: new vis.DataSet(edgesArray)
    };

    network = new vis.Network(container, data, options);
    updateProgressBar(50);
    network.on("stabilizationIterationsDone", function () {
        // network.setOptions({ physics: false });
        network.storePositions();
        var allNodes = data.nodes.get();
        allNodes = JSON.stringify(allNodes);
        updateProgressBar(75);
        $.ajax({
            url: '/Visualization/SetNodes',
            type: 'POST',
            dataType: 'json',
            data: { nodes: allNodes },
            success: function (response) {
                console.log('Success:', response);
                document.getElementById("response").innerHTML = "The visualization graph has successfully been updated";
                updateProgressBar(100);
            },
            error: function (error) {
                console.error('Error fetching data:', error);
                document.getElementById("response").innerHTML = "ERROR: The visualization graph has not been updated. Resubmit the contract and try again. ";

            }
        });
    });
});

function updateProgressBar(n) {
    var progressBar = document.getElementById("progressBar");
    var progressContainer = document.getElementById("progressContainer");
    if (progressContainer.style.display === 'none') {
        progressContainer.style.display = 'initial'
    }
    progressBar.innerHTML = n;
    progressBar.setAttribute("style", "width:" + n + "%");
    if (n === 100) {
        progressContainer.style.transition = 'opacity 5s ease';
        progressContainer.style.opacity = '0';
    }
}












