(function($){
	$.fn.scpopup = function(options){

        var defaults = {

            // Settings Variables
            linkType        : "iframe",     // iframe, inline, html, image
            scWidth         : "65%",        // Width of popup container (in px, % or auto)
            scHeight        : "auto",       // Height of popup container (in px, % or auto)
            popupMaxWidth   : "700px;",     // Max width of popup container (in px, % or auto)
            popupMaxHeight  : "auto",       // Max width of popup container (in px, % or auto)
            popupTheme      : "test",       // Popup theme name (is an additional class added to parent)
            activeClass     : "active",     // Class name to use for active elements
            popupPosition   : "fixed",      // absolute, fixed
            effectOpen      : "",           // Popup opening effect
            effectClose     : "",           // Popup closing effect
            htmlContent     : "<h2>Title</h2><p>This content will go into the popup.</p>" // Must set linkType to html
        };

        var options = $.extend(defaults, options);
		
        // Functions to Specify Width and Height of Popup
        function scpopupWidth(scW) {
            $('#scpopup').css({'position' : options.popupPosition, 'margin-left' : '-' + scW/2 + 'px'});
        }
        function scpopupHeight(scH) {
            $('#scpopup').css({'position' : options.popupPosition, 'margin-top' : '-' + scH/2 + 'px'});
        }
        
        // Append Backdrop and Content Container
        $('div.popupbackdrop').remove();
		$('body').append('<div class="popupbackdrop"></div>');
		$('body').append('<div id="scpopup" class="scpopup"><div id="scpopupouter"><div id="scpopupinner"><div id="scpopuptitle"></div><div id="scpopupsubtitle"></div><div id="scpopupholder"><div id="scpopupcontent"></div><div class="clear"></div></div><div class="clear"></div></div><div class="clear"></div></div>');
        
        // Set Width and Height of Outer Container
        $('#scpopup').width(options.scWidth).height(options.scHeight).addClass(options.popupTheme);
        
        $(this).addClass('scpopuplink');
        
        // Click Event: Open
		$(this).on('click', function(e){
			e.preventDefault();
            
            // Margin/Width/Height for Popup
            scpopupWidth($('#scpopup').width());
            scpopupHeight($('#scpopup').height());
			
            // Setting Body/HTML tags to 100% width and height
			$('body', 'html').css({'width' : '100%', 'height' : '100%'});
			$('body').addClass('scpopupactive');
            
            // Transitions
			$('div.popupbackdrop').fadeIn(150).addClass(options.activeClass);
            $('#scpopup').fadeIn(300).addClass(options.activeClass);
			
			// Empty Title/Subtitle Holders on Click
			$('#scpopuptitle, #scpopupsubtitle').empty();
			
			// Load Title/Subtitles on Click
			$('<span></span>').text($(this).attr('title')).appendTo('#scpopuptitle');
			$('<span></span>').text($(this).attr('alt')).appendTo('#scpopupsubtitle');
			
            // Link Type (linkType)
            if(options.linkType == 'iframe'){
				$('#scpopupcontent').empty().append(
                    $('<iframe>', {src: this.href})
                );
                //$('#scpopupcontent').empty().append('<iframe src="' + this.href + '"></iframe>');
            }else if(options.linkType == 'inline'){
                //alert('inline');
            }else if(options.linkType == 'html') {
                $('#scpopupcontent').empty().append(options.htmlContent);
            }else if(options.linkType == 'image') {
                //alert('image');
            }
		});
        
        // Click Event: Close
		$('div.popupbackdrop').on('click', function(e){
			e.preventDefault();
            
			$('body').removeClass('scpopupactive');
			$(this).delay(50).fadeOut(300).removeClass(options.activeClass);
            $('#scpopup').delay(25).fadeOut(150).removeClass(options.activeClass);
		});
	};
})(jQuery);