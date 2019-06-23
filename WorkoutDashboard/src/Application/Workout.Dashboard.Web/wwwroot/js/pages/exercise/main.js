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
});