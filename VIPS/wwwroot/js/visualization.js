if (document.body.contains(document.getElementById('mynetwork'))) {
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
            borderWidth: 2,
            color: "rgba(10, 35, 63,1)"

        },
        physics: {
            enabled: false,
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
            width: 16,
            color: "black"
        },
        layout: {
            improvedLayout: false
        },
        interaction: {
            dragNodes: false,
            navigationButtons: false,
            selectConnectedEdges: false,

            hideNodesOnDrag: false,
            hideEdgesOnDrag: false,
            hideEdgesOnZoom: false,
        }



    };

    var network;


    var nodeUrl = '/Visualization/GetNodeData';
    var edgeUrl = '/Visualization/GetEdgeData';

    var nodesArray = [];
    var edgesArray = [];


    $.when(
        $.ajax({
            url: nodeUrl,
            type: 'GET',
            dataType: 'json',
            success: function (data) {
                console.log(JSON.stringify(data));

                for (var i = 0; i < data.length; i++) {
                    var type = data[i].nodeId.slice(0,1);

                    if (type === "s") {
                        nodesArray.push({
                            id: data[i].nodeId,
                            label: data[i].name,
                            x: data[i].x,
                            y: data[i].y,
                            type: "school",
                            color: "purple",
                        });
                    }
                    else if (type === "d") {
                        nodesArray.push({
                            id: data[i].nodeId,
                            label: data[i].name,
                            schoolId: data[i].schoolId,
                            x: data[i].x,
                            y: data[i].y,
                            type: "department",
                            color: "#0A233F"
                        });
                    }
                    else if (type === "p") {
                        nodesArray.push({
                            id: data[i].nodeId,
                            label: data[i].name,
                            x: data[i].x,
                            y: data[i].y,
                            type: "partner",
                            color: "red"
                        });
                    }
                    else {

                    }
                    
                }
            },
            error: function (error) {
                console.error('Error fetching data:', error);
            }
        }),
        $.ajax({
            url: edgeUrl,
            type: 'GET',
            dataType: 'json',
            success: function (params) {
                for (var i = 0; i < params.length; i++) {
                    var tempEdge = ""
                    console.log("edge test" + JSON.stringify(params));
                    if (params[i].contractId != 0) {
                        tempEdge =
                        {
                            id: i,
                            from: params[i].fromId,
                            to: params[i].toId,
                            ContractId: params[i].contractId,
                            ExpirationDate: params[i].expirationDate,
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
                            ExpirationDate: params[i].expirationDate,
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

        data.edges.forEach(function (edge) {
            var color = checkExpiration(edge.ExpirationDate);
            edge.color = color;
        });

        network = new vis.Network(container, data, options);

        /*
        data.nodes.forEach(function (node) {
            if (node.type === "school") {
                network.clusterByConnection(node.id);
            }
        });
        */

        network.on("stabilizationIterationsDone", function () {
            network.setOptions({ physics: false });

        });

        function countConnectionsPartner(nodeId) {
            var connectedNodes = network.getConnectedNodes(nodeId);

            var connectedNodesCount = connectedNodes.length;

            return connectedNodesCount;
        }

        function countConnectionsDepartment(nodeId) {
            var connectedNodes = network.getConnectedNodes(nodeId);
            var connectedNodesCount = 0;
            connectedNodes.forEach(function (nodeId) {
                if (data.nodes.get(nodeId).type == "partner") {
                    connectedNodesCount++;
                }
            });

            return connectedNodesCount;

        }

        function countConnectionsSchool(nodeId) {
            var connectedNodes = network.getConnectedNodes(nodeId);

            var connectedNodesCount = 0;
            connectedNodes.forEach(function (nodeId) {
                if (data.nodes.get(nodeId).type == "partner") {
                    connectedNodesCount++;
                }
                var neighbors = network.getConnectedNodes(nodeId);
                neighbors.forEach(function (nodeId) {
                    if (data.nodes.get(nodeId).type == "partner") {
                        connectedNodesCount++;
                    }

                });
            });

            return connectedNodesCount;

        }

        // network.on('click', neighbourhoodHighlight);
        network.on('selectNode', function (params) {
            console.log("selectNode");
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

                                var depts = data.depts;
                                var contracts = data.contracts;

                                console.log(JSON.stringify(depts));
                                console.log(JSON.stringify(contracts));



                                var newElements = [];
                                newElements.push(createAnchor(node.label, "sidebarTitle")); // push school name
                                newElements.push(createAnchor("Number of Connections: " + countConnectionsSchool(node.id), "sidebarEntry"));

                                if (data !== undefined && data.length != 0) {

                                    // ADD CONSTANT SCHOOL INFO

                                    if (depts.length > 0) {
                                        newElements.push(createAnchor("Associated Departments: ", "sidebarData")); // work on

                                        var ulDepts = document.createElement('ul');

                                        for (i = 0; i < depts.length; i++) {
                                            var li = document.createElement('li')
                                            var anchor = createAnchor(depts[i].departmentName, "sidebarEntry");

                                            // newElements.push(anchor);
                                            li.appendChild(anchor);
                                            ulDepts.appendChild(li);

                                        }
                                        newElements.push(ulDepts);

                                    }

                                    

                                    if (contracts.length > 0) {
                                        newElements.push(createAnchor("Associated Contracts: ", "sidebarData"));

                                        var ulContracts = document.createElement('ul');

                                        for (i = 0; i < contracts.length; i++) {
                                            var li = document.createElement('li')
                                            var anchor = createAnchor(contracts[i].contractId, "sidebarEntry");
                                            anchor.href = '/Search/Contract/' + contracts[i].contractId;
                                            anchor.setAttribute('target', '_blank');

                                            // newElements.push(anchor);
                                            li.appendChild(anchor);
                                            ulContracts.appendChild(li);

                                        }
                                        newElements.push(ulContracts);

                                    }
                                }
                                else {
                                    newElements.push(createAnchor("No departments", "sidebarData")); 
                                }


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
                                newElements.push(createAnchor("Number of Connections: " + countConnectionsDepartment(node.id), "sidebarEntry"));

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
                                newElements.push(createAnchor("Number of Connections: " + countConnectionsPartner(node.id), "sidebarEntry"));

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
                    data: { contractId: edge.ContractId },
                    success: function (params) {
                        var updatedEdge =
                        {
                            id: edgeId,
                            ContractId: params.contractID,
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
                        var edge = data.edges.get(edgeId);

                        var anchor = createAnchor(edge.ContractName + " [" + edge.ContractId + "]", "sidebarTitle");
                        anchor.href = '/Search/Contract/' + edge.ContractId;
                        anchor.setAttribute('target', '_blank');

                        sidebarEdge.replaceChildren(
                            anchor,
                            edge.Department && createAnchor("Department: " + edge.Department, "sidebarData"),
                            edge.AgencyName && createAnchor("Partner Name: " + edge.AgencyName, "sidebarData"),
                            edge.Owner && createAnchor("Owner: " + edge.Owner, "sidebarData"),
                            edge.FacultyInitiator && createAnchor("Faculty Initiator: " + edge.FacultyInitiator, "sidebarData"),
                            edge.StageName && createAnchor("Stage Name: " + edge.StageName, "sidebarData"),
                            edge.City && createAnchor("City: " + edge.City, "sidebarData"),
                            edge.State && createAnchor("State: " + edge.State, "sidebarData"),
                            edge.Year && createAnchor("Year: " + edge.Year, "sidebarData"),
                            edge.CreatedOn && createAnchor("Created On: " + edge.CreatedOn, "sidebarData"),
                            edge.UpdatedOn && createAnchor("Updated On: " + edge.UpdatedOn, "sidebarData"),
                            edge.Renewal && createAnchor("Renewal: " + edge.Renewal, "sidebarData")
                        );

                    },
                    error: function (error) {
                        console.error('Error fetching data:', error);
                    }
                });

            }
            else {
                document.getElementById("sidebarName").innerHTML = beforeEdge.hiddenLabel;
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
        var regex = /[a-f0-9]{8}(?:-[a-f0-9]{4}){3}-[a-f0-9]{12}/;
        var match = regex.exec(value);
        return match != null;
    }

    function checkExpiration(expirationDate) {
        if (expirationDate == null) {
            return '76BC43'; // UNF Leaf
        }
        expirationDate = Date.parse(expirationDate);
        var today = new Date();
        var sixMonths = new Date(today.getFullYear(), today.getMonth() + 6, today.getDate());
        // var twoMonth = new Date(today.getFullYear(), today.getMonth() + 2, today.getDate());

        if (expirationDate < today) { // already expired
            return '#C41F4B'; // UNF Sunset
        }
        else if (expirationDate < sixMonths) { // will expire within the month
            return 'FFC62F'; // UNF Sunshine
        }
        else {
            return '76BC43'; // UNF Leaf
        }
        
        
    }
    
}
else {
    console.log("mynetwork not in html");
}





/*
var newOptions = {
    nodes: {
        fixed: true
    }
    
}
*/




