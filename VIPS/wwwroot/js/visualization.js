var options = {
    autoResize: true,
    height: window.innerHeight,
    nodes: {
        font: {
            color: "#FFFFFF",
            align: 'center',
            face: "open sans"
        },
        fixed: false,
        shape: "circle",
        color: "rgba(10, 35, 63,1)"
        
    },

    physics: {
        enabled: true,
        barnesHut: {
            gravitationalConstant: -1000,
            centralGravity: 0.1,
            springLength: 200,
            springConstant: 0.05,
            damping: 0.15,
            avoidOverlap: 1   
        }
    },
    edges: {
        smooth: {
            type: 'continuous', // Set type to 'continuous' for straight lines without curves
            roundness: 0 // Set roundness to 0 to remove any rounding effect
        }
    }
};


//var nodes = new vis.DataSet([
//    { id: 1, label: "Node 1" },
//    { id: 2, label: "Node 2" },
//    { id: 3, label: "Node 3" },
//    { id: 4, label: "Node 4" }
//]);

var nodes = new vis.DataSet();

nodes.add({ id: 1, label: "School of Computing", tags: "tag1, tag2", desc: "This is a node." });
nodes.add({ id: 2, label: "Partner 1", tags: "tag1, tag2", desc: "This is a node." });
nodes.add({ id: 3, label: "Partner 2", tags: "tag1, tag2", desc: "This is a node." });

for (var i = 4; i <= 100; i++) {
    nodes.add({ id: i, label: "Node " + i, tags: "tag1, tag2", desc: "This is a node."});
}



var edges = new vis.DataSet([
    { from: 1, to: 3 },
    { from: 1, to: 2 },
    { from: 2, to: 4 },
    { from: 2, to: 5 }
]);

var container = document.getElementById('mynetwork');

var data = {
    nodes: nodes,
    edges: edges
};


var network = new vis.Network(container, data, options);

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
        document.getElementById("sidebarTags").innerHTML = nodes.get(nodeId).tags;
        document.getElementById("sidebarDesc").innerHTML = nodes.get(nodeId).desc;
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