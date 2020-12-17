
var offsetVal = 100;

$(document).on("click", "#sc-right", function (e) {
    let liEleRight = $(this).parent().parent().find('ul').first().find('li');
    let ttRight = parseInt(liEleRight.first().css('width'));

    let lastLi = parseInt(liEleRight.last().offset().left) + parseInt(liEleRight.last().css('width'));
    let rightBot = parseInt($('#sc-right').offset().left);

    let rightDistance = rightBot - lastLi;
    if (rightDistance < 0) {
        let foset = (parseInt($('#tabs').css('left')) - offsetVal) + 'px';
        $('#tabs').css('left', foset);
    }
    determineBottomToHide();
});
//left
$(document).on("click", "#sc-left", function (e) {
    let liEleLeft = $(this).parent().parent().find('ul').first().find('li');
    let ttLeft = parseInt(liEleLeft.first().css('width'));

    let firstLi = parseInt(liEleLeft.first().offset().left) + 20;
    let leftBot = parseInt($('#sc-left').offset().left);

    let leftDistance = leftBot - firstLi;
    if (leftDistance > -40) {
        let foset = (parseInt($('#tabs').css('left')) + offsetVal) + 'px';
        $('#tabs').css('left', foset);
    }
    determineBottomToHide();
});

function determineBottomToHide() {
    let tabs = $('#tabs').first().find('li');
     

    //left
    let firstLi = parseInt(tabs.first().offset().left);
    let leftBot = parseInt($('#sc-left').parent().offset().left);

     
    let preLeftdistance = leftBot - firstLi;
    
    if (preLeftdistance <= 10) {
        $('#sc-left').hide();
    } else {
        $('#sc-left').show();
    }

    //right
    let lastLi = parseInt(tabs.last('li').offset().left);
    let rightBot = parseInt($('#sc-right').parent().offset().left);
     
    let preRightdistance = rightBot - lastLi;
  
    if (preRightdistance >= 142) {
        $('#sc-right').hide();
    } else {
        $('#sc-right').show();
    }


}

