$(document).ready(function() {
	/* --------------------------------------------------------
       Color picker - demo only
       --------------------------------------------------------   */
    //$('<div class="color-picker"><a href="#" class="handle"><i class="icofont icofont-color-bucket"></i></a><div class="settings-header"><h3>Setting panel</h3></div><div class="section"><h3 class="color">Normal color schemes:</h3><div class="colors"><a href="#" class="color-1" ></a><a href="#" class="color-2" ></a><a href="#" class="color-3" ></a><a href="#" class="color-4" ></a><a href="#" class="color-5"></a></div></div><div class="section"><h3 class="color">Inverse color:</h3><div><a href="#" class="color-inverse"><img class="img img-fluid img-thumbnail" src="" /></a></div></div></div>').appendTo($('body'));

      /*Gradient Color*/
      
      $('.color-picker').animate({
          right: '-239px'
      });

      $('.color-picker a.handle').click(function(e) {
          e.preventDefault();
          var div = $('.color-picker');
          if (div.css('right') === '-239px') {
              $('.color-picker').animate({
                  right: '0px'
              });
          } else {
              $('.color-picker').animate({
                  right: '-239px'
              });
          }
      });
});
