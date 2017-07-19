'use strict';

$(document).ready(function () {
    var ctx1 = document.getElementById('chart1').getContext('2d');
    var ctx2 = document.getElementById('chart2').getContext('2d');
    var ctx3 = document.getElementById('chart3').getContext('2d');
    var ctx4 = document.getElementById('chart4').getContext('2d');

    var chart1 = new Chart(ctx1, '@(TempData["chart1"])');
    var chart2 = new Chart(ctx2, '@(TempData["chart2"])');
    var chart3 = new Chart(ctx3, '@(TempData["chart3"])');
    var chart4 = new Chart(ctx4, '@(TempData["chart4"])');
});