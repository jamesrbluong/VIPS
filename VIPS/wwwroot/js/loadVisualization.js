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
    network.on("stabilizationIterationsDone", function () {
        network.setOptions({ physics: false });
    });

    // stringify result and send to hidden html value

    var positions = getPositions();
    positions = JSON.stringify(positions);

    $.ajax({
        url: '/Visualization/SetNodes',
        type: 'POST',
        dataType: 'json',
        data: positions,
        success: function (response) {
            console.error('Success:', response);
        },
        error: function (error) {
            console.error('Error fetching data:', error);
        }
    })

});










