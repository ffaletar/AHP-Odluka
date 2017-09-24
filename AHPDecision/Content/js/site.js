//auto scroll to alternative-row
//$("#alternative-button-autoscroll").click(function () {
//    $('html,body').animate({
//        scrollTop: ($("#alternative-row").offset().top - 55) + 'px'
//    },'slow');
//});

//auto scroll to criteria-row
//$("#criteria-button-autoscroll").click(function () {
//    $('html,body').animate({
//        scrollTop: ($("#criteria-row").offset().top - 55) + 'px'
//    }, 'slow');
//});

$(document).ready(function () {
    //$(".scroll-criteria-menu-treeview").wrap("<div class='scroll' style='height: 87%;'></div>");

    $(".scroll-main").wrap("<div class='scroll' style='height: 45%;'></div>");


});

function FillUpNewCriteriaPartial(nazivRoditelja, idRoditelja) {
    document.getElementById("parentId").value = idRoditelja;
    document.getElementById("parentName").innerHTML = nazivRoditelja;
}

function ShowModalDeleteCriteria(id) {
    document.getElementById("deleteCriteriaId").value = id;
    document.getElementById("modal-deleteCriteria").style.display = 'block';
}