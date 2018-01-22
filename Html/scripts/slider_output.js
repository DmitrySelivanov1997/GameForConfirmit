$(function() {
    var el;
    $("#drawTimeSlider").change(function() {
    el = $(this);
    el
    .next("#drawTimeOutput")
    .text(el.val());
    })
    .trigger('change');
    });
 $(function() {
     var el;
     $("#turnsTimeSlider").change(function() {
     el = $(this);
    el
    .next("#turnsTimeOutput")
    .text(el.val());
    })
    .trigger('change');
    });