// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.
$(function() {

    this.ExerciseId = 0;
    if(typeof ExerciseId !== undefined)
        this.ExerciseId = ExerciseId;

    function addRowSameLevel(id, parentId) {
        var itm = $(id)[0];
        var cln = itm.cloneNode(true);
        $(cln).insertAfter(itm);
        return $(cln);
    };

    function addRowChild(id, parentId) {
        var itm = $(id)[0];
        var cln = itm.cloneNode(true);
        $(parentId)[0].appendChild(cln);
        return $(cln);
    };

    function renderSideBarGroup($item, data) {
        $item.find(".group-name").html(data[0].typename);
        data.forEach(element => {
            var $subMenu = addRowChild(".group-submenu", $item.find(".treeview-menu"));
            $subMenu.find(".exercise-name").html(element.name);
            $subMenu.find(".exercise-link").attr("href", element._id);
            if(this.ExerciseId > 0 && element._id == this.ExerciseId) {
                $item.addClass("active menu-open");
                $subMenu.addClass("active");
            }

        });
        $item.find(".group-submenu")[0].remove() // Remove sample element that was used to clone for real elements
    };

    $.get("/api/v1/exercises/tree", function(responseData, status) {
        if(responseData === undefined || responseData.length === 0) return;
        responseData.forEach(group => {
            var $item = addRowSameLevel("#exercises-sidebar","#exercises-sidebar-header");
            renderSideBarGroup($item, group);
        });
        $("#exercises-sidebar")[0].remove(); // Remove sample element that was used to clone for real elements
    });


});
