
// whenever there is a table, hide extra columns on mobile view
//$(document).ready(function () {
//    var width = $(window).width();
//    if (width < 768) {
//        $('.table-striped tr').each(function (index, element) {
//            $(element).find('th').each(function (thIndex, thElement) {
//                if (thIndex > 1 && !$(thElement).hasClass("alwaysShow")) {
//                    console.log('hiding th ' + thIndex);
//                    $(thElement).hide();
//                }
//            });
//            $(element).find('td').each(function (tdIndex, tdElement) {
//                if (tdIndex > 1 && !$(tdElement).hasClass("alwaysShow")) {
//                    console.log('hiding td ' + tdIndex);
//                    $(tdElement).hide();
//                }
//            });
//        });
//    }
//});
