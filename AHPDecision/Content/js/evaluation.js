function changeFactor(criteria1, criteria2, factor, projectId) {
    $.ajax({
        type: 'GET',
        url: '/Project/ChangeCriteriaComparisonFactor',
        data: {
            criteria1: criteria1,
            criteria2: criteria2,
            factor: factor
        },
        success: function () {
            $.ajax({
                url: '/Project/UpdateProjectsCriteriaConsistency',
                data: {
                    id: projectId
                },
                success: function () {
                    $.ajax({
                        type: 'GET',
                        url: '/Evaluation/GetCriteriaMenuFullPartialView',
                        data: {
                            id: projectId
                        },
                        success: function (data) {
                            var container = document.getElementById('criteria-menu-partial-container');
                            container.innerHTML = data;
                        },
                        error: function (xhr, status, error) {
                            alert(xhr.responseText);
                        }
                    });
                },
                error: function (xhr, status, error) {
                    alert("Neuspješno ažuriranje konzistentnosti");
                    alert(xhr.responseText);

                }
            });
        },
        error: function (xhr, status, error) {
            alert(error);
        }
    });
}
function changeAlternativesValue(alternative1, alternative2, criteria, factor, projectId) {
    $.ajax({
        type: 'GET',
        url: '/Project/ChangeAlternativeComparisonFactor',
        data: {
            alternative1: alternative1,
            alternative2: alternative2,
            criteria: criteria,
            factor: factor
        },
        success: function (data) {
            $.ajax({
                url: '/Project/UpdateProjectsAlternativesConsistency',
                data: {
                    id: projectId
                },
                success: function () {
                    $.ajax({
                        type: 'GET',
                        url: '/Evaluation/GetCriteriaAlternativesMenuFullPartialView',
                        data: {
                            id: projectId,
                            kriterij: criteria
                        },
                        success: function (data) {
                            var container = document.getElementById('criteria-alternatives-menu-partial-container');
                            container.innerHTML = data;
                            $(".scroll-main").wrap("<div class='scroll' style='height: 45%;'></div>");
                        },
                        error: function (xhr, status, error) {
                            alert(xhr.responseText);
                        }
                    });
                },
                error: function (xhr, status, error) {
                    alert("Neuspješno ažuriranje konzistentnosti za alternative");
                    alert(xhr.responseText);

                }
            });
        },
        error: function (xhr, status, error) {
            alert(error);
        }
    });
}