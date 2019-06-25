function formattedDate(d = new Date) {
    let month = String(d.getMonth() + 1);
    let day = String(d.getDate());
    const year = String(d.getFullYear());
  
    if (month.length < 2) month = '0' + month;
    if (day.length < 2) day = '0' + day;
  
    return `${day}/${month}/${year}`;
  };

$(function() {

    this.ExerciseId = ExerciseId || 0;

    function renderBoxInfo(id, data) {
        $(id + " .info-box-value").html(data.StrRate);
        if(data.Date !== undefined) {
            $(id + " .info-box-date").html(data.Date);
        }
    };

    function renderBoxInfoLift(id, data) {
        $(id + " .info-box-value").html(data.lift);
        if(data.repcount !== undefined) {
            $(id + " .info-box-rep").html(data.repcount);
        }
        if(data.workoutdate !== undefined) {
            $(id + " .info-box-date").html(data.workoutdate);
        }
    };

    function renderChart(id, data) {
        var areaChartData = {
            labels  : [],
            datasets: []
        };

        var currentColor = "#0384c4";
        
        

        var values = [];
        data.forEach(function(current){
            values.push(current.StrRate);
            areaChartData.labels.push(current.Date);
        });
            
        var dataset = {
            label               : "Str-Rate",
            strokeColor         : currentColor,
            pointColor          : currentColor,
            pointStrokeColor    : currentColor,
            pointHighlightFill  : '#fff',
            pointHighlightStroke: currentColor,
            data                : values
        };
        areaChartData.datasets.push(dataset);

        var config = {
            options: {
                scales: {
                    xAxes: [{
                        type: 'time',
                        distribution: 'linear',
                        time: {
                            displayFormats: {
                                'millisecond': 'MMM DD',
                                'second': 'MMM DD',
                                'minute': 'MMM DD',
                                'hour': 'MMM DD',
                                'day': 'MMM DD',
                                'week': 'MMM DD',
                                'month': 'MMM DD',
                                'quarter': 'MMM DD',
                                'year': 'MMM DD',
                            }
                        }
                    }],
                    yAxes: [{
                        ticks: {
                            beginAtZero: true
                        }
                    }]
                }
            }
        };

        var areaChartCanvas = $(id).get(0).getContext('2d');
        // This will get the first returned node in the jQuery collection.
        var areaChart       = new Chart(areaChartCanvas, config);
        var areaChartOptions = {
            scaleBeginAtZero: true,
            //Boolean - If we should show the scale at all
            showScale               : true,
            //Boolean - Whether grid lines are shown across the chart
            scaleShowGridLines      : false,
            //String - Colour of the grid lines
            scaleGridLineColor      : 'rgba(0,0,0,.05)',
            //Number - Width of the grid lines
            scaleGridLineWidth      : 1,
            //Boolean - Whether to show horizontal lines (except X axis)
            scaleShowHorizontalLines: true,
            //Boolean - Whether to show vertical lines (except Y axis)
            scaleShowVerticalLines  : true,
            //Boolean - Whether the line is curved between points
            bezierCurve             : false,
            //Number - Tension of the bezier curve between points
            bezierCurveTension      : 0.3,
            //Boolean - Whether to show a dot for each point
            pointDot                : true,
            //Number - Radius of each point dot in pixels
            pointDotRadius          : 3,
            //Number - Pixel width of point dot stroke
            pointDotStrokeWidth     : 1,
            //Number - amount extra to add to the radius to cater for hit detection outside the drawn point
            pointHitDetectionRadius : 20,
            //Boolean - Whether to show a stroke for datasets
            datasetStroke           : true,
            //Number - Pixel width of dataset stroke
            datasetStrokeWidth      : 2,
            //Boolean - Whether to fill the dataset with a color
            datasetFill             : false,
            //String - A legend template
            legendTemplate          : '<ul class="<%=name.toLowerCase()%>-legend"><% for (var i=0; i<datasets.length; i++){%><li><span style="background-color:<%=datasets[i].lineColor%>"></span><%if(datasets[i].label){%><%=datasets[i].label%><%}%></li><%}%></ul>',
            //Boolean - whether to maintain the starting aspect ratio or not when responsive, if set to false, will take up entire container
            maintainAspectRatio     : true,
            //Boolean - whether to make the chart responsive to window resizing
            responsive              : true
        };

        areaChart.datasetFill = false;
        areaChart.Line(areaChartData, areaChartOptions);

    };

    function renderBasicInfo (id, data) {
        $(id + " .exercise-name").html(data.name);
        $(id + " .group-name").html(data.typename);
    };

    $.get("/api/v1/exercises/"+ this.ExerciseId,
    function(responseData, status) {
        renderBasicInfo("#exercise-basic-info", responseData);
    });

    $.get("/api/v1/exercises/"+ this.ExerciseId +"/strrate/top",
    function(responseData, status) {
        renderBoxInfo("#box-top-str", responseData);
    });

    $.get("/api/v1/exercises/"+ this.ExerciseId +"/strrate/average",
    function(responseData, status) {
        renderBoxInfo("#box-mean-str", responseData);
    });

    $.get("/api/v1/exercises/"+ this.ExerciseId +"/strrate/current",
    function(responseData, status) {
        renderBoxInfo("#box-current-str", responseData);
    });

    $.get("/api/v1/exercises/"+ this.ExerciseId +"/lift/top",
    function(responseData, status) {
        renderBoxInfoLift("#box-top-lift", responseData);
    });

    $.get("/api/v1/exercises/"+ this.ExerciseId +"/strrate/evolution",
    function(responseData, status) {
        renderChart("#chart-strrate-evo", responseData);
    });
});