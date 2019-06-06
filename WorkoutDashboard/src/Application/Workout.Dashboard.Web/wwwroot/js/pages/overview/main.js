$(function () {

    var predefinedColors  = ['#1184f2', '#f27f11', '#db3157', '#538e6d', '#020259' ,'#4f2204', '#ffdc00'];


    function ColorPalette() {
        var currentColor = 0;

        this.getColor = function() {
            var col = predefinedColors[currentColor];
            if(currentColor < (predefinedColors.length - 1)) {
                currentColor++;
            } else {
                currentColor = 0;
            }
            return col;
        }
    }

    function getRandomColor() {
        var letters = '0123456789ABCDEF'.split('');
        var color = '#';
        for (var i = 0; i < 6; i++ ) {
            color += letters[Math.floor(Math.random() * 16)];
        }
        return color;
    }
    
    /* Area chart */
    $.get("/api/v1/querymodels/GetNumberOfWorkoutsInSlicedPeriod?startdate=2019-01-01&enddate=2019-04-01&daysInSlice=30",
        function(data, status) {
            var labels = [];
            var values = [];
            data.forEach(function(current){
                var currentDate = new Date(current.startDate);
                labels.push(currentDate.toLocaleString('en-us', {month: 'long'}));
                values.push(current.count);
            });

            var areaChartData = {
                labels  : labels,
                datasets: [
                    {
                        label               : 'Number of workouts per month',
                        fillColor           : 'rgba(60,141,188,0.9)',
                        strokeColor         : 'rgba(60,141,188,0.8)',
                        pointColor          : '#3b8bba',
                        pointStrokeColor    : 'rgba(60,141,188,1)',
                        pointHighlightFill  : '#fff',
                        pointHighlightStroke: 'rgba(60,141,188,1)',
                        data                : values
                    }
                ]
            };

            var areaChartCanvas = $('#areaChart').get(0).getContext('2d');
            // This will get the first returned node in the jQuery collection.
            var areaChart       = new Chart(areaChartCanvas);
            var areaChartOptions = {
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
                bezierCurve             : true,
                //Number - Tension of the bezier curve between points
                bezierCurveTension      : 0.3,
                //Boolean - Whether to show a dot for each point
                pointDot                : false,
                //Number - Radius of each point dot in pixels
                pointDotRadius          : 4,
                //Number - Pixel width of point dot stroke
                pointDotStrokeWidth     : 1,
                //Number - amount extra to add to the radius to cater for hit detection outside the drawn point
                pointHitDetectionRadius : 20,
                //Boolean - Whether to show a stroke for datasets
                datasetStroke           : true,
                //Number - Pixel width of dataset stroke
                datasetStrokeWidth      : 2,
                //Boolean - Whether to fill the dataset with a color
                datasetFill             : true,
                //String - A legend template
                legendTemplate          : '<ul class="<%=name.toLowerCase()%>-legend"><% for (var i=0; i<datasets.length; i++){%><li><span style="background-color:<%=datasets[i].lineColor%>"></span><%if(datasets[i].label){%><%=datasets[i].label%><%}%></li><%}%></ul>',
                //Boolean - whether to maintain the starting aspect ratio or not when responsive, if set to false, will take up entire container
                maintainAspectRatio     : true,
                //Boolean - whether to make the chart responsive to window resizing
                responsive              : true
            };

            areaChart.datasetFill = false;
            areaChart.Line(areaChartData, areaChartOptions);

        }
    );

    /* Bars */
    var topLiftByPeriodChartData = {};
    /* Get Pie Data from server */
    $.get("/api/v1/querymodels/GetTopLiftInSlicedPeriod?startdate=2019-01-01&enddate=2019-04-01&daysInSlice=30",
        function(data, status) {
            var labels = [];
            var values = [];
            data.forEach(function(current){
                var currentDate = new Date(current.startDate);
                labels.push(currentDate.toLocaleString('en-us', {month: 'long'}));
                values.push(current.maxLift);
            });

            topLiftByPeriodChartData = {
                labels  : labels,
                datasets: [
                    {
                        label               : 'Top lift per month',
                        fillColor           : 'rgba(60,141,188,0.9)',
                        strokeColor         : 'rgba(60,141,188,0.8)',
                        pointColor          : '#3b8bba',
                        pointStrokeColor    : 'rgba(60,141,188,1)',
                        pointHighlightFill  : '#fff',
                        pointHighlightStroke: 'rgba(60,141,188,1)',
                        data                : values
                    }
                ]
            };

            //-------------
            //- BAR CHART -
            //-------------
            var barChartCanvas                   = $('#barChart').get(0).getContext('2d');
            var barChart                         = new Chart(barChartCanvas);
            var barChartData                     = topLiftByPeriodChartData;
            barChartData.datasets[0].fillColor   = '#00a65a';
            barChartData.datasets[0].strokeColor = '#00a65a';
            barChartData.datasets[0].pointColor  = '#00a65a';
            var barChartOptions                  = {
                //Boolean - Whether the scale should start at zero, or an order of magnitude down from the lowest value
                scaleBeginAtZero        : true,
                //Boolean - Whether grid lines are shown across the chart
                scaleShowGridLines      : true,
                //String - Colour of the grid lines
                scaleGridLineColor      : 'rgba(0,0,0,.05)',
                //Number - Width of the grid lines
                scaleGridLineWidth      : 1,
                //Boolean - Whether to show horizontal lines (except X axis)
                scaleShowHorizontalLines: true,
                //Boolean - Whether to show vertical lines (except Y axis)
                scaleShowVerticalLines  : true,
                //Boolean - If there is a stroke on each bar
                barShowStroke           : true,
                //Number - Pixel width of the bar stroke
                barStrokeWidth          : 2,
                //Number - Spacing between each of the X value sets
                barValueSpacing         : 5,
                //Number - Spacing between data sets within X values
                barDatasetSpacing       : 1,
                //String - A legend template
                legendTemplate          : '<ul class="<%=name.toLowerCase()%>-legend"><% for (var i=0; i<datasets.length; i++){%><li><span style="background-color:<%=datasets[i].fillColor%>"></span><%if(datasets[i].label){%><%=datasets[i].label%><%}%></li><%}%></ul>',
                //Boolean - whether to make the chart responsive
                responsive              : true,
                maintainAspectRatio     : true
            };

            barChartOptions.datasetFill = false;
            barChart.Bar(barChartData, barChartOptions);

        }
    );

    
    /* Doughnut */
    /* Get Pie Data from server */
    $.get("/api/v1/querymodels/GetNumberOfExercisesByGroupInPeriod?startdate=2019-01-01&enddate=2019-04-01",
        function(data, status) {
            var doughnutData = [];
            var palette = new ColorPalette();
            data.groupExercises.forEach(function(current){
                var currentColor = palette.getColor();
                doughnutData.push(
                    {
                        value    : current.count,
                        color    : currentColor,
                        highlight: currentColor,
                        label    : current.groupName
                    }
                );
            });

            //-------------
            //- PIE CHART -
            //-------------
            // Get context with jQuery - using jQuery's .get() method.
            var pieChartCanvas = $('#pieChart').get(0).getContext('2d');
            var pieChart       = new Chart(pieChartCanvas);
            var pieOptions     = {
                //Boolean - Whether we should show a stroke on each segment
                segmentShowStroke    : true,
                //String - The colour of each segment stroke
                segmentStrokeColor   : '#fff',
                //Number - The width of each segment stroke
                segmentStrokeWidth   : 2,
                //Number - The percentage of the chart that we cut out of the middle
                percentageInnerCutout: 50, // This is 0 for Pie charts
                //Number - Amount of animation steps
                animationSteps       : 100,
                //String - Animation easing effect
                animationEasing      : 'easeOutBounce',
                //Boolean - Whether we animate the rotation of the Doughnut
                animateRotate        : true,
                //Boolean - Whether we animate scaling the Doughnut from the centre
                animateScale         : false,
                //Boolean - whether to make the chart responsive to window resizing
                responsive           : true,
                // Boolean - whether to maintain the starting aspect ratio or not when responsive, if set to false, will take up entire container
                maintainAspectRatio  : true,
                //String - A legend template
                legendTemplate       : '<ul class="<%=name.toLowerCase()%>-legend"><% for (var i=0; i<segments.length; i++){%><li><span style="background-color:<%=segments[i].fillColor%>"></span><%if(segments[i].label){%><%=segments[i].label%><%}%></li><%}%></ul>'
            }
            //Create pie or douhnut chart
            // You can switch between pie and douhnut using the method below.
            pieChart.Doughnut(doughnutData, pieOptions)

        }
    );

    /* Lines */
    $.get("/api/v1/querymodels/GetStrRateByGroupInPeriod?startdate=2019-01-01&enddate=2019-04-01&daysInSlice=30",
        function(data, status) {
            var areaChartData = {
                labels  : [],
                datasets: []
            };
            data[0].strRateInPeriods.forEach(function(period){
                var currentDate = new Date(period.startDate);
                areaChartData.labels.push(currentDate.toLocaleString('en-us', {month: 'long'}));
            });
            var palette = new ColorPalette();
            data.forEach(function(current){
                var values = [];
                var currentColor = palette.getColor();
                current.strRateInPeriods.forEach(function(str){
                    //if(str.strRate < 250)
                    values.push(str.strRate);
                });
                var dataset = {
                    label               : current.exerciseTypeName.toString(),
                    strokeColor         : currentColor,
                    pointColor          : currentColor,
                    pointStrokeColor    : currentColor,
                    pointHighlightFill  : '#fff',
                    pointHighlightStroke: currentColor,
                    data                : values
                };
                areaChartData.datasets.push(dataset);
            });

            var config = {
                options: {
                    scales: {
                        xAxes: [{
                            type: 'time',
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
                        }]
                    }
                }
            };

            var areaChartCanvas = $('#lineChart').get(0).getContext('2d');
            // This will get the first returned node in the jQuery collection.
            var areaChart       = new Chart(areaChartCanvas, config);
            var areaChartOptions = {
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

        }
    );

});