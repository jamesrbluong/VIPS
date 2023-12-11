var options = {
    autoResize: true,
    height: '100%',
    nodes: {
        font: {
            color: "#FFFFFF",
            align: 'center',
            face: "open sans"
        },
        fixed: false,
        widthConstraint: 200,
        heightConstraint: 100,
        shape: "circle",
        color: "rgba(10, 35, 63,1)"

    },

    physics: {
        enabled: true,
        barnesHut: {
            gravitationalConstant: -2000,
            centralGravity: 0.1,
            springLength: 200,
            springConstant: 0.05,
            damping: 0.15,
            avoidOverlap: 2
        }
    },
    edges: {
        smooth: {
            type: 'continuous', // Set type to 'continuous' for straight lines without curves
            roundness: 0 // Set roundness to 0 to remove any rounding effect
        }
    }
};

var network;
var dataUrl = '/Visualization/GetPartnerData';

// Make an AJAX request to fetch data from the server
$.ajax({
    url: dataUrl,
    type: 'GET',
    dataType: 'json',
    success: function (data) {
        var nodesArray = [];
        var edgesArray = [];

        // Assuming the data is an array of partners
        // Modify this part based on your actual data structure
        for (var i = 0; i < data.length; i++) {
            nodesArray.push({
                id: data[i].partnerId,
                label: data[i].name
                // Add other properties as needed
            });
        }

        // Initialize the network with the retrieved data
        var container = document.getElementById('mynetwork');

        var data = {
            nodes: new vis.DataSet(nodesArray),
            edges: new vis.DataSet(edgesArray)
        };

        network = new vis.Network(container, data, options);
    },
    error: function (error) {
        console.error('Error fetching data:', error);
    }
});

var newOptions = {
    nodes: {
        fixed: true
    }
}





network.on("click", neighbourhoodHighlight);


network.on("selectNode", function (params) {
    var nodeId = params.nodes[0];
    var node = network.body.nodes[nodeId];
    network.moveTo({
        position: { x: node.x, y: node.y },
        animation: true
    });
    if (nodes.get(nodeId).hiddenLabel == undefined) {
        document.getElementById("sidebarName").innerHTML = nodes.get(nodeId).label;
        //document.getElementById("sidebarTags").innerHTML = nodes.get(nodeId).tags;
        //document.getElementById("sidebarDesc").innerHTML = nodes.get(nodeId).desc;
    }
    else {
        document.getElementById("sidebarName").innerHTML = nodes.get(nodeId).hiddenLabel;
    }
    document.getElementById("sidebar").style.width = "500px";
});

network.on("deselectNode", function (params) {
    document.getElementById("sidebar").style.width = "0px";
});

var allNodes = nodes.get({ returnType: "Object" });
var highlightActive = false;



function neighbourhoodHighlight(params) {
    // if something is selected:
    if (params.nodes.length > 0) {
        highlightActive = true;
        var i, j;
        var selectedNode = params.nodes[0];
        var degrees = 1;

        // mark all nodes as hard to read.
        for (var nodeId in allNodes) {
            allNodes[nodeId].color = "rgba(167, 168, 169,0.5)";
            if (allNodes[nodeId].hiddenLabel === undefined) {
                allNodes[nodeId].hiddenLabel = allNodes[nodeId].label;
                allNodes[nodeId].label = undefined;
            }
        }
        var connectedNodes = network.getConnectedNodes(selectedNode);
        var allConnectedNodes = [];


        // all first degree nodes get their own color and their label back
        for (i = 0; i < connectedNodes.length; i++) {
            allNodes[connectedNodes[i]].color = "rgba(10, 35, 63,1)";
            if (allNodes[connectedNodes[i]].hiddenLabel !== undefined) {
                allNodes[connectedNodes[i]].label =
                    allNodes[connectedNodes[i]].hiddenLabel;
                allNodes[connectedNodes[i]].hiddenLabel = undefined;
            }
        }

        // the main node gets its own color and its label back.
        allNodes[selectedNode].color = "rgba(10, 35, 63,1)";
        if (allNodes[selectedNode].hiddenLabel !== undefined) {
            allNodes[selectedNode].label = allNodes[selectedNode].hiddenLabel;
            allNodes[selectedNode].hiddenLabel = undefined;
        }
    } else if (highlightActive === true) {
        // reset all nodes
        for (var nodeId in allNodes) {
            allNodes[nodeId].color = "rgba(10, 35, 63,1)";
            if (allNodes[nodeId].hiddenLabel !== undefined) {
                allNodes[nodeId].label = allNodes[nodeId].hiddenLabel;
                allNodes[nodeId].hiddenLabel = undefined;
            }
        }
        highlightActive = false;
    }
    var updateArray = [];
    for (nodeId in allNodes) {
        if (allNodes.hasOwnProperty(nodeId)) {
            updateArray.push(allNodes[nodeId]);
        }
    }
    nodes.update(updateArray);
}