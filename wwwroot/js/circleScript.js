var listElements = document.getElementsByTagName("li");
var step = (2 * Math.PI) / listElements.length;
var angle = 360;
var circleCenterX = 800;
var circleCenterY = 250;
var radius = 190;
for (var i = 0; i < listElements.length; i++) {
    var element = listElements[i];
    var liLeft = Number(Math.round(circleCenterX + radius * Math.cos(angle)));
    var liTop = Number(Math.round(circleCenterY + radius * Math.sin(angle)));
    element.style.left = liLeft + "px";
    element.style.top = liTop + "px";
    angle += step;
}