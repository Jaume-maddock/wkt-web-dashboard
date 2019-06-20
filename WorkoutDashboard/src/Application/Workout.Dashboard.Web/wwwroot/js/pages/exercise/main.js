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

    function renderTopStrRate(data) {
        $("#box-top-str .info-box-value").html(data.StrRate);
        $("#box-top-str .info-box-date").html(data.Date);
    };

    $.get("/api/v1/exercises/"+ this.ExerciseId +"/strrate/top",
    function(responseData, status) {
        renderTopStrRate(responseData);
    });
});