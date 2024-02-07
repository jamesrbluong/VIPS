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
        improvedLayout: true
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
var viusalizationUrl = '/Visualization/GetVisualizationData';

var nodesArray = [];
var edgesArray = [];

// var dataUrl = '/Visualization/GetPartnerData';


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
        },
        error: function (error) {
            console.error('Error fetching data:', error);
        }
    }),
    $.ajax({
        url: viusalizationUrl,
        type: 'GET',
        dataType: 'json',
        success: function (params) {
            for (var i = 0; i < params.length; i++) {
                var tempEdge = ""

                if (params[i].contractId != 0) {
                    tempEdge =
                    {
                        from: params[i].fromId,
                        to: params[i].toId,
                        id: params[i].contractId,
                        CreatedOn: "",
                        ContractName: "",
                        Owner: "",
                        StageName: "",
                        UpdatedOn: "",
                        AgencyName: "",
                        City: "",
                        Department: "",
                        FacultyInitiator: "",
                        Renewal: "",
                        State: "",
                        Year: "",
                        type: "contract"
                    }
                }
                else {
                    tempEdge =
                    {
                        from: params[i].fromId,
                        to: params[i].toId,
                        CreatedOn: "",
                        ContractName: "",
                        Owner: "",
                        StageName: "",
                        UpdatedOn: "",
                        AgencyName: "",
                        City: "",
                        Department: "",
                        FacultyInitiator: "",
                        Renewal: "",
                        State: "",
                        Year: "",
                        type: "contract"
                    }
                }
                edgesArray.push(tempEdge);
                
            }
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

    /*
    for (i = 0; i < nodesArray.length; i++) {
        if (nodesArray[i].type == "school") {
            var clusterNode = network.clusterByConnection(nodesArray[i].id);
            // console.log(clusterNode);
            // network.clustering.updateClusteredNode(clusterNode.id, { shape: 'box', label: nodesArray[i].label });
        }
    }
    */
    
    network.on("stabilizationIterationsDone", function () {
        network.setOptions({ physics: false });
    });

    // network.on('click', neighbourhoodHighlight);
    network.on('selectNode', function (params) {
        var nodeId = params.nodes[0];
        var node = data.nodes.get(nodeId);
        var sidebarNode = document.getElementById("sidebarNode");
        var sidebarEdge = document.getElementById("sidebarEdge");
        var ul = document.createElement('ul');

        var nodeLocation = network.body.nodes[nodeId];

        sidebarEdge.replaceChildren();

        network.moveTo({
            position: { x: nodeLocation.x, y: nodeLocation.y },
            animation: true
        });

        if (network.isCluster(nodeId) === true) {
            // Open the cluster associated with the clicked node
            network.openCluster(nodeId);
        }
        else {
            if (node.hiddenLabel == undefined) {

                if (node.type == 'school') {
                    $.ajax({
                        url: '/Visualization/FillSchoolData',
                        type: 'GET',
                        dataType: 'json',
                        data: { stringId: node.id }, // JSON.stringify( { departmentId: data.nodes.get(nodeId).id } )
                        success: function (data) {
                            console.log(JSON.stringify(data));
                            var newElements = [];
                            newElements.push(createAnchor(node.label, "sidebarTitle")); // push school name

                            // ADD CONSTANT SCHOOL INFO
                            newElements.push(createAnchor("Associated Departments: ", "sidebarData")); // work on

                            for (i = 0; i < data.length; i++) {
                                var li = document.createElement('li')
                                var anchor = createAnchor(data[i].departmentName, "sidebarEntry");

                                // newElements.push(anchor);
                                li.appendChild(anchor);
                                ul.appendChild(li);

                            }

                            /* contract list
                            newElements.push(createAnchor("Associated Contracts: ", "sidebarData"));
                            for (i = 0; i < data.length; i++) {
                                var li = document.createElement('li')
                                var anchor = createAnchor(data[i].departmentId, "sidebarEntry");
                                anchor.href = '/Search/Contract/' + data[i].departmentId;
                                anchor.setAttribute('target', '_blank');

                                // newElements.push(anchor);
                                li.appendChild(anchor);
                                ul.appendChild(li);

                            }
                            */
                            newElements.push(ul);

                            sidebarNode.replaceChildren(...newElements);



                        },
                        error: function (error) {
                            console.error('Error fetching data:', error);
                        }
                    });
                }
                else if (node.type == 'department') {
                    $.ajax({
                        url: '/Visualization/FillDepartmentData',
                        type: 'GET',
                        dataType: 'json',
                        data: { departmentId: node.id }, // JSON.stringify( { departmentId: data.nodes.get(nodeId).id } )
                        success: function (data) {
                            var newElements = [];
                            newElements.push(createAnchor(node.label, "sidebarTitle")); // push partner name

                            // ADD CONSTANT PARTNER INFO

                            newElements.push(createAnchor("Associated Contracts: ", "sidebarData"));

                            for (i = 0; i < data.length; i++) {
                                var li = document.createElement('li')
                                var anchor = createAnchor(data[i].contractId, "sidebarEntry");
                                anchor.href = '/Search/Contract/' + data[i].contractId;
                                anchor.setAttribute('target', '_blank');

                                // newElements.push(anchor);
                                li.appendChild(anchor);
                                ul.appendChild(li);

                            }
                            newElements.push(ul);

                            sidebarNode.replaceChildren(...newElements);



                        },
                        error: function (error) {
                            console.error('Error fetching data:', error);
                        }
                    });
                }
                else if (node.type == 'partner') {
                    $.ajax({
                        url: '/Visualization/FillPartnerData',
                        type: 'GET',
                        dataType: 'json',
                        data: { partnerId: node.id }, // JSON.stringify( { departmentId: data.nodes.get(nodeId).id } )
                        success: function (data) {
                            console.log(JSON.stringify(data));

                            var newElements = [];
                            newElements.push(createAnchor(node.label, "sidebarTitle")); // push partner name

                            // ADD CONSTANT PARTNER INFO

                            newElements.push(createAnchor("Associated Contracts: ", "sidebarData"));

                            for (i = 0; i < data.length; i++) {
                                var li = document.createElement('li')
                                var anchor = createAnchor(data[i].contractId, "sidebarEntry");
                                anchor.href = '/Search/Contract/' + data[i].contractId;
                                anchor.setAttribute('target', '_blank');

                                // newElements.push(anchor);
                                li.appendChild(anchor);
                                ul.appendChild(li);

                            }
                            newElements.push(ul);

                            sidebarNode.replaceChildren(...newElements);

                        },
                        error: function (error) {
                            console.error('Error fetching data:', error);
                        }
                    });
                }
            }
            else {
                document.getElementById("sidebarName").innerHTML = node.hiddenLabel;
            }
            openSidebar();
            network.redraw();
        }
    });

    network.on("deselectNode", function (params) {
        closeSidebar();
    });

    network.on('selectEdge', function (info) {
        var edgeId = info.edges[0];
        var edge = data.edges.get(edgeId);
        var sidebarNode = document.getElementById("sidebarNode");
        var sidebarEdge = document.getElementById("sidebarEdge");

        sidebarNode.replaceChildren();

        if (edge.hiddenLabel == undefined) {
            $.ajax({
                url: '/Visualization/FillContractData',
                type: 'GET',
                dataType: 'json',
                data: { contractId: edge.id }, 
                success: function (params) {
                    var updatedEdge =
                    {
                        id: params.contractID,
                        CreatedOn: params.createdOn,
                        ContractName: params.contractName,
                        Owner: params.owner,
                        StageName: params.stageName,
                        UpdatedOn: params.updatedOn,
                        AgencyName: params.agencyName,
                        City: params.city,
                        Department: params.department,
                        FacultyInitiator: params.facultyInitiator,
                        Renewal: params.renewal,
                        State: params.state,
                        Year: params.year 
                    };

                    data.edges.update(updatedEdge);
                    edge = data.edges.get(edgeId);

                    var anchor = createAnchor(edge.ContractName + " [" + edge.id + "]", "sidebarTitle");
                    anchor.href = '/Search/Contract/' + edge.id;
                    anchor.setAttribute('target', '_blank');

                    sidebarEdge.replaceChildren(
                        anchor,
                        createAnchor("Department: " + edge.Department, "sidebarData"),
                        createAnchor("Partner Name: " + edge.AgencyName, "sidebarData"),
                        createAnchor("Owner: " + edge.Owner, "sidebarData"),
                        createAnchor("Faculty Initiator: " + edge.FacultyInitiator, "sidebarData"),
                        createAnchor("Stage Name: " + edge.StageName, "sidebarData"),
                        createAnchor("Created On: " + edge.CreatedOn, "sidebarData"),
                        createAnchor("City: " + edge.City, "sidebarData"),
                        createAnchor("State: " + edge.State, "sidebarData"),
                        createAnchor("Year: " + edge.Year, "sidebarData"),
                        createAnchor("Updated On: " + edge.UpdatedOn, "sidebarData"),
                        createAnchor("Renewal: " + edge.Renewal, "sidebarData"),
                    );
                    
                },
                error: function (error) {
                    console.error('Error fetching data:', error);
                }
            });
            
        }
        else {
            document.getElementById("sidebarName").innerHTML = edge.hiddenLabel;
        }
        if (!isGuid(edgeId)) {
            openSidebar();
        }
        

        network.redraw();
    });

    network.on("deselectEdge", function (params) {
        closeSidebar();
    });

});

function openSidebar() {
    document.getElementById("sidebar").style.visibility = "visible";
    document.getElementById("sidebar").style.width = "30%";
    document.getElementById("mynetwork").style.width = "70%";
}

function closeSidebar() {
    document.getElementById("sidebar").style.visibility = "hidden";
    document.getElementById("sidebar").style.width = "0px";
    document.getElementById("mynetwork").style.width = "100%";
}

function createAnchor(textContent, c) {
    var anchor = document.createElement('a');
    anchor.textContent = textContent;
    anchor.className = c;
    return anchor;
}

function isGuid(value) {
    var regex = /[a-f0-9]{8}(?:-[a-f0-9]{4}){3}-[a-f0-9]{12}/i;
    var match = regex.exec(value);
    return match != null;
}



/*
var newOptions = {
    nodes: {
        fixed: true
    }
    
}
*/




