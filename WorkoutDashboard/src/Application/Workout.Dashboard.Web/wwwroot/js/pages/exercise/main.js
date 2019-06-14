$(function() {

    this.ExerciseId = ExerciseId || 0;

    function renderTopStrRate(data) {
        $("#strrate-value").html(data);
    };

    $.get("/api/v1/exercises/"+ this.ExerciseId +"/strrate/top",
    function(data, status) {
        renderTopStrRate(data);
    });
});