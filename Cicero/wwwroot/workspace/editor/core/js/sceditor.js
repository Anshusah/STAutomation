
(function ($) {
    var base; /* Base Variable */
	var editors=[]; 						// Array Containing all the Editors that will be Created
	var myLayout; 							// variable for layout 
    $.sceditor = function (el, op) { 		// Core Function
        base = this; 						// this objects to base variable 
        base.$el = $(el); 					// passed base variable to base.$el variable
        base.el = el; 						// passed elements  to base.el variable
        base.$el.data("sceditor", base); 	// creating data variable to store data.
        base.init = function () { 			// first/initial function, which will be called firtime when age load
			base.op = $.extend({}, $.sceditor.defaultOptions, op);// extending function/overwriting default option by new options
			
			/* ******************************************************************************* */
			/* Page Pane Layout JS *********************************************************** */
			/* ******************************************************************************* */
			function toggleLiveResizing () {
				$.each( $.layout.config.borderPanes, function (i, pane) {
					var o = myLayout.options[ pane ];
					o.livePaneResizing = !o.livePaneResizing;
				});
			};
			
			// set EVERY 'state' here so will undo ALL layout changes
			// used by the 'Reset State' button: myLayout.loadState( stateResetSettings )
			var stateResetSettings = {
				west__size			: 250,
				west__initClosed	: false,
				west__initHidden	: false,
			};
			
			
			
			/* ******************************************************************************* */
			/* Preparing Layout ************************************************************** */
			/* ******************************************************************************* */
			myLayout = $('div.wrapper').layout({
				
				// Reference Only - these options are NOT required because 'true' is the default
				closable						: true,		// pane can open & close
				resizable						: true,		// when open, pane can be resized 
				slidable						: true,		// when closed, pane can 'slide' open over other panes - closes on mouse-out
				livePaneResizing				: true,
				
				togglerLength_open				: 50,		// Toggler Height
				togglerLength_closed			: "100%",
				spacing_open					: 1,		// Space between pane and adjacent panes - when pane is 'open'
				spacing_closed					: 6,		// Space between pane and adjacent panes - when pane is 'closed'
				
				// Pane Settings
				north__size						: 20,
				north__minSize					: 31,
				north__spacing_open				: 0,		// cosmetic spacing
				north__togglerLength_open		: 0,		// HIDE the toggler button
				north__togglerLength_closed		: -1,		// "100%" OR -1 = full width of pane
				north__resizable				: false,
				north__slidable					: false,
				
				south__minSize					: 100,
				south__onresize					: function(){ base.prepareStage()},
				south__onhide					: function(){ base.prepareStage()},
				south__onshow					: function(){ base.prepareStage()},
				south__onopen					: function(){ base.prepareStage()},
				south__onclose					: function(){ base.prepareStage()},
				south__onopen_end               : function(){ base.prepareStage()},
				south__onopen_start             : function(){ base.prepareStage()},
				south__onclose_end              : function(){ base.prepareStage()},
				south__onclose_start            : function(){ base.prepareStage()},
				south__maxSize					: 400,
				south__spacing_open				: 1,
				south__slidable					: true,		// REFERENCE - cannot slide if spacing_closed = 0
				south__initClosed				: false,
				
				east__size						: 50,
				east__spacing_open				: 1,		// cosmetic spacing
				east__togglerLength_open		: 0,		// HIDE the toggler button
				east__togglerLength_closed		: -1,		// "100%" OR -1 = full width of pane
				east__resizable					: false,
				east__slidable					: false,
				east__fxName					: "none",
				
				west__showOverflowOnHover		: true,		// enable showOverflow on west-pane so CSS popups will overlap north pane
				west__animatePaneSizing			: false,
				west__fxSpeed_size				: "fast",	// 'fast' animation when resizing west-pane
				west__fxSpeed_open				: 300,		// 1-second animation when opening west-pane
				west__minSize					: 150,
				west__maxSize					: 600,
				
				// Enable State Management
				stateManagement__enabled		: true, 	// automatic cookie load & save enabled by default
				
				showDebugMessages				: true 		// log and/or display messages from debugging & testing code
			});
			
			
			
			/* ******************************************************************************* */
			/* if there is no state-cookie, then DISABLE state management initially ********** */
			/* ******************************************************************************* */
			var cookieExists = !$.isEmptyObject( myLayout.readCookie() );
			
			
			
			/* ******************************************************************************* */
			/* if (!cookieExists) toggleStateManagement( true, false ); ********************** */
			/* when the input file change while opening file ********************************* */
			/* ******************************************************************************* */
			$('input#openner[type=file]').change(function () {
				base.fileHasBeenOpened();
			});
			
			
			
			/* ******************************************************************************* */
			/* When Stage is Resized ********************************************************* */
			/* ******************************************************************************* */
			$(window).resize(function(){
				base.prepareStage();
			})
			
			
			
			/* ******************************************************************************* */
			/* Loading Tree Nav ************************************************************** */
			/* ******************************************************************************* */



             $(base.op.configTree.treeRootElement).fileTree({
				root			: base.op.configTree.root,
				script			: base.op.configTree.script,
				expandSpeed		: 300,
				collapseSpeed	: 300,
				multiFolder		: true
			}, function(file) {
				
				// when any files are clicked from left nav, open on right-hand side
				base.bootLoader({'data':file,'base':base.op.configTree.baseUrl})
			});
			
			
			
			/* ******************************************************************************* */
			/* When Tab Close Button is Clicked ********************************************** */
			/* ******************************************************************************* */
			$('div.editortabbar').on('click', '.close', function(e) {
				e.preventDefault();
				var id = $(this).attr('data-tab-id');
				if(base.isEdited(id).length>0){
					if(confirm("Do you want to save " + base.getFileName(id) + " ?")){
						base.fileSave(id);
						base.closeEditor(id);
					}else{
						base.closeEditor(id);
					}
				}else{
					base.closeEditor(id);
				}
			});
			
			
			
			/* ******************************************************************************* */
			/* Top Navigation **************************************************************** */
			/* ******************************************************************************* */
			$('ul.sitenavigation a').on('click', function(e){
				e.preventDefault();
			});
			
			$('ul.sitenavigation li.parent').hover(
				function() {
					$(this).find('ul.sitenavdropdown.primary').show();
				},
				function() {
					$(this).find('ul.sitenavdropdown.primary').hide();
				}
			);
			
			
			
			/* ******************************************************************************* */
			/* Click Event on Top Menus ****************************************************** */
			/* ******************************************************************************* */
			$('body').on('click','[data-action]',function(){
				eval("base."+$(this).data('action')+"()");
				$(this).parents("ul.sitenavdropdown").hide();
			});
			$('div.editortabbar').tabs();// Initialize Tabs
        }
		
		
		
		/* ******************************************************************************* */
		/* Adding New Editor ************************************************************* */
		/* ******************************************************************************* */
		base.addAce = function (e) {
		
			// The Panel ID is a Timestamp Plus a Random Number from 0 to 10000
			var tabUniqueId = (e.uniqueId=='')?base.getUniqueId():e.uniqueId;
			if($(base.op.configAce.tabListUl).find("li#panel_nav_"+tabUniqueId).length>0){
				if($(base.op.configAce.tabListUl).find("li#panel_nav_"+tabUniqueId).hasClass("ui-tabs-active")){
				}else{
					$(base.op.configAce.tabListUl).find("li").removeClass("ui-state-active");
					$(base.op.configAce.tabListUl).find("li#panel_nav_"+tabUniqueId+" a.ui-tabs-anchor").click();
				}
				return false;
			}
			
			var URL = e.fileName; // Gets page name
			var page = URL.substring(URL.lastIndexOf('/') + 1);  
			var tabsElement = $(base.op.configAce.tabListUl).parent('div.editortabbar');
			var tabsUlElement = $(base.op.configAce.tabListUl);
			var newTabNavElement = $('<li class="unselected unsaved" id="panel_nav_' + tabUniqueId + '"><div class="tabbttn"><span class="name"><a href="#panel_' + tabUniqueId + '"> ' + page + '</a></span></div></li>');
			tabsUlElement.append(newTabNavElement);
			var newTabPanelElement = $('<div id="panel_' + tabUniqueId + '" data-tab-id="' + tabUniqueId + '"><!--New editor: ' + tabUniqueId + ': <br/>--></div>');
			tabsElement.append(newTabPanelElement);
			tabsElement.tabs('refresh');
			var tabIndex = $('div.editortabbar ul#editorfilelist li').index($('#panel_nav_' + tabUniqueId));
			tabsElement.tabs('option', 'active', tabIndex);
			var newEditorElement = $('<div id="editor_' + tabUniqueId + '" class="ace_editor">'+e.fileContent+'</div>');

			newTabPanelElement.append(newEditorElement);
			ace.require("ace/ext/language_tools");
			var editor = ace.edit('editor_' + tabUniqueId);
            editor.getSession().setUseWorker(false);
			editor.setTheme("ace/theme/"+base.op.configAce.setTheme);
			var re = /(?:\.([^.]+))?$/;
			var ext = re.exec(page)[1]; 
			
			if(base.op.configAce.autoSetMode){
				editor.getSession().setMode("ace/mode/"+base.getModeExtension(ext));
			}else{
				editor.getSession().setMode("ace/mode/"+base.op.configAce.setMode);
			}
			//alert(base.op.configAce.setUseWrapMode)
			
			// Editor Options
			editor.gotoLine(1); // Goto Line Number
			editor.selection.getCursor(); // Get the Current Cursor Line and Column
			editor.session.getLength(); // Get Total Number of Lines
			editor.getSession().setTabSize(parseInt(base.op.configAce.setTabSize)); // Set Default Tab Size
			editor.getSession().setUseWrapMode(base.stringToBoolean(base.op.configAce.setUseWrapMode)); // Toggle Word Wrapping
			document.getElementById('editor_' + tabUniqueId).style.fontSize=base.op.configAce.fontSize; // Set the Font Size
			editor.setShowPrintMargin(base.stringToBoolean(base.op.configAce.setShowPrintMargin)); // Set Print Margin Visibility
			editor.setReadOnly(base.stringToBoolean(base.op.configAce.setReadOnly)); // Set the Editor to Read-only
			editor.setAutoScrollEditorIntoView();
			editor.renderer.setScrollMargin(0, 0);
			editor.on("change",function(){base.onEdit()})
			
			// Resize codeeditor
			base.prepareStage();
			editor.resize();
			editors.push({
				id			: tabUniqueId,
				instance	: editor
			});
			var tabCloseButton 	= $('<a href="#" class="close"  data-tab-id="'+tabUniqueId+'"></a>');
			$('li#panel_nav_' + tabUniqueId + ' div.tabbttn').append(tabCloseButton);
		}
		
		
		
		
		/* ******************************************************************************* */
		/* Base Functions **************************************************************** */
		/* ******************************************************************************* */
		
		/* String to Boolean */
		base.stringToBoolean= function(string){
			switch(string.toLowerCase()){
				case "true": case "yes": case "1": return true;
				case "false": case "no": case "0": case null: return false;
				default: return Boolean(string);
			}
		}
		
		/* File Extension Map */
		base.extensionMapping=function(){
			var _extMap={
				'action':'java',
				'as'	: 'actionscript',
				'ascx'	: 'dot',
				'ashx'	: 'dot',
                'cshtml' : 'dot',
                'tmpl' : 'html',
				'asmx'	: 'dot',
				'asp'	: 'dot',
				'aspx'	: 'dot',
				'axd'	: 'dot',
				'bat'	: 'batchfile',
				'btm'	: 'batchfile',
				'c'		: 'c_cpp',
				'cfm'	: 'coldfusion',
				'cfml'	: 'coldfusion',
				'clj'	: 'clojure',
				'cpp'	: 'c_cpp',
				'css'	: 'css',
				'cs'	: 'csharp',
				'cxx'	: 'c_cpp',
				'do'	: 'java',
				'dpj'	: 'java',
				'erb'	: 'ruby',
				'h'		: 'c_cpp',
				'haml'	: 'haml',
				'hdl'	: 'c_app',
				'hpp'	: 'c_cpp',
				'htm'	: 'html',
				'html'	: 'html',
				'hxx'	: 'c_cpp',
				'jar'	: 'java',
				'java'	: 'java',
				'jhtml'	: 'html',
				'jar'	: 'java',
				'js'	: 'javascript',
				'json'	: 'json',
				'jsp'	: 'java',
				'jspx'	: 'java',
				'less'	: 'less',
				'log'	: 'text',
				'master': 'csharp',
				'phtml'	: 'php',
				'pl'	: 'perl',
				'py'	: 'python',
				'pm'	: 'perl',
				'rb'	: 'ruby',
				'rhtml'	: 'ruby',
				'rjs'	: 'ruby',
				'rss'	: 'xml',
				'sass'	: 'sass',
				'scss'	: 'scss',
				'smf'	: 'matlab',
				'sms'	: 'matlab',
				'sql'	: 'mysql',
				'svg'	: 'xml',
				'txt'	: 'text',
				'xhtml'	: 'html',
				'xml'	: 'xml',
				'xrb'	: 'xml',
				'wss'	: 'java',
				'yaws'	: 'erlang',
			}
			return _extMap;	
		}
		
		/* When User Clicks on Undefined Function */
		base.undefined=function(){
			alert("Sorry This method is not available in this theme");
		}
		
		/* Creating Unique ID for Code Editor */
		base.getUniqueId=function(){
			return new Date().getTime() + Math.floor(Math.random() * 10000);
		}
		
		/* htmlentities function of php, which we need while opening a new file */
		base.htmlentities=function(e){
			return (jQuery('<div />').text(e).html()).replace(/\\/g, "");
		}
		/* Resizing Stage When Panels Resizes or Window Resizes */
		base.prepareStage=function(){
			var footerHeight;
			if($(".footertoolbar").is(":visible")){
				footerHeight=parseInt($(".footertoolbar").height());
			}else{
				footerHeight=5;
			}
			var content_height=$(window).height()-(footerHeight+58);
			$(".ace_editor").each(function(){
				$(this).css({'height' :(content_height)+"px"})
			});
			$(".filemanager_main").css({'height' :(content_height-0)+"px"})
			$.each(editors, function( index, value ) {
				value.instance.resize();
			});
		}
		
		/* Helps to Load Content File from Server */
		base.bootLoader=function(e) {
			$('<iframe src="/admin/ajax/file-content.html?path='+e.data.file+'" style="display:none;" />').appendTo("body").on("load",function(n){
				elmnts=$(this).contents().find("html:first").find("body:first").html();
				$("iframe").remove();
				base.addAce({'uniqueId':e.data.id,'fileName':e.data.file,'fileContent':elmnts})
			})
		}
		
		/* When User Edits a File, it Will Show Asterisk on its Tab */
		base.onEdit=function(){
			if($(base.op.configAce.tabListUl+">li.ui-state-active").find("a.ui-tabs-anchor span").length<1){
			var data="<span class='unsaved'>*</span>";
			$(base.op.configAce.tabListUl).find("li.ui-state-active a.ui-tabs-anchor").prepend(data);
			}
		}
		
		/* Let the System Know if the File has been Edited */
		base.isEdited=function(e){
			return $(base.op.configAce.tabListUl+">li#panel_nav_"+e).find("a.ui-tabs-anchor span");
		}
		
		/* Closing Tabs and Codeeditor */
		base.closeEditor=function(e){
			var editor=base.getEditorInstance(e).instance;
			editors.splice(base.getEditorInstance(e).index,1);
			// Destroy the Editor Instance
			editor.destroy();
			// Remove the Panel and Panel Nav DOM
			$('div.editortabbar').find('#panel_nav_' + e).remove();
			$('div.editortabbar').find('#panel_' + e).remove();
			$(base.op.configAce.tabListUl).find("li:last-child a.ui-tabs-anchor").click();
		}
		
		/* Get Editor Instance */
		base.getEditorInstance=function(e){
			for(var i=0; i<=editors.length-1; i++){
				if(editors[i].id==e){
					return {"instance":editors[i].instance,"index":(i)};
				}
			}
		}
		
		/* Function for Getting Active Editor */
		base.getActiveEditor=function(){
			return $(base.op.configAce.tabListUl+" li.ui-state-active a.close").attr('data-tab-id')	
		}
		
		/* Get File */
		base.getFileName=function(e){
			return 	(
					$(base.op.configAce.tabListUl+">li#panel_nav_"+e)
					.find("a.ui-tabs-anchor")
					.html()
					.replace(/<span.*?>.*?<\/span>/m, "")
					);
		}
		
		/* To Create New File */
		base.fileNew=function(){
			base.addAce({'fileName':'untitled.cshtml',"fileContent":"// New cs file ",'uniqueId':base.getUniqueId()})
		}
		
		/* Function for New from Template */
		base.newFromTemplate=function(){
			console.log("Clicked: New from Template");
		}
		
		/* Function for New Folder */
		base.newFolder=function(){
			console.log("Clicked: New Folder");
		}
		
		/* When User Clicks on Open */
		base.fileHasBeenOpened=function(){
			var filePath=$('#openner').val();
			var files = document.getElementById('openner').files;
			
			if (!files.length) {
				alert('Please select a file!');
				return;
			}
			
			var file = files[0];
			var start =  0;
			var stop = file.size - 1;
			var reader = new FileReader();
			
			reader.onloadend = function(evt) {
				if (evt.target.readyState == FileReader.DONE) { // DONE == 2
					base.addAce({'uniqueId':base.getUniqueId(),'fileName':filePath,'fileContent':base.htmlentities(evt.target.result)})
				}
			};
			
			var blob = file.slice(start,stop+1);
			reader.readAsBinaryString(blob);
		}
		
		/* Function to Open File */
		base.fileOpen=function(){
			$("#openner").click();
		}
		
		/* File Save */
		base.fileSave=function(e){
			var fileContent=base.getEditorInstance(e).instance.getSession().getValue();
			$.post(base.op.configTree.baseUrl+'/core/filetree/savefile.html',{'fileName':$('a[date-file-id="'+base.getActiveEditor()+'"]').attr("rel"),'fileContent':fileContent},function(){});
		}
		
		/* When User Clicks on Save Option */
		base.saveFile=function(){
			if(editors.length>0){
				if($(base.op.configAce.tabListUl+">li.ui-state-active").find("a.ui-tabs-anchor span").length>0){
				$(base.op.configAce.tabListUl+">li.ui-state-active").find("a.ui-tabs-anchor span").remove();
				}
				base.fileSave(base.getActiveEditor())
			}
		}
		
		/* Function for Saving a File (CURRENTLY NOT WORKING) */
		base.saveAs=function(){
			//var pom = document.createElement('a');
			//pom.setAttribute('href', 'data:text/plain;charset=utf-8,' + encodeURIComponent("fsad fds af"));
			//pom.setAttribute('download', "test.html");
			//pom.click();
			//alert("test");
			
			//var win=window.open(url, '_top');
  			//win.focus();
			//uriContent = "data:application/octet-stream," + encodeURIComponent("test test");
			//window.open(uriContent, 'neuesDokument.txt');
		}
		
		/* Function for Save All */
		base.saveAll=function(){
			console.log("Clicked: Save All");
		}
		
		/* Close File */
		base.closeFile=function(){
			if(editors.length>0){
				if(base.isEdited(base.getActiveEditor()).length>0){
					if(confirm("do you want to save "+ base.getFileName(base.getActiveEditor()) +" ?")){
						base.fileSave(base.getActiveEditor());
					}
				}
				base.closeEditor(base.getActiveEditor());
			}
		}
		base.getModeExtension=function(e){
			if(base.extensionMapping().hasOwnProperty(e)){
				return base.extensionMapping()[e];
			}else{
				return e;
			}
		}
		/* Close All Files */
		base.closeAllFiles=function(){
			if(editors.length>0){
				$(base.op.configAce.tabListUl+ " li").each(function(){
					var is_edited=$(this).find("a.ui-tabs-anchor span");
					var current_id=$(this).find("a.close").attr("data-tab-id")
					if(is_edited.length>0){
						base.getFileName(current_id);
						if(confirm("do you want to save "+ base.getFileName(current_id) +" ?")){
							base.fileSave(current_id);
							base.closeEditor(current_id);
						}else{
							base.closeEditor(current_id);
						}
					}else{
						base.closeEditor(current_id);
					}
				})
			}
		}
		
		/* Function for Undo */
		base.undoAction=function(){
			console.log("Clicked: Undo");
		}
		
		/* Function for Redo */
		base.redoAction=function(){
			console.log("Clicked: Redo");
		}
		
		/* Function for Cut */
		base.cutAction=function(){
			console.log("Clicked: Cut");
		}
		
		/* Function for Copy */
		base.copyAction=function(){
			console.log("Clicked: Copy");
		}
		
		/* Function for Paste */
		base.pasteAction=function(){
			console.log("Clicked: Paste");
		}
		
		/* Function for Indent */
		base.indentAction=function(){
			console.log("Clicked: Indent");
		}
		
		/* Function for Outdent */
		base.outdentAction=function(){
			console.log("Clicked: Outdent");
		}
		
		/* Function for Toggle Comment */
		base.toggleComment=function(){
			console.log("Clicked: Toggle Comment");
		}
		
		/* Function for Uppercase Convert */
		base.uppercaseConvert=function(){
			console.log("Clicked: Uppercase");
		}
		
		/* Function for Lowercase Convert */
		base.lowercaseConvert=function(){
			console.log("Clicked: Lowercase");
		}
		
		/* Function for Show Autocomplete */
		base.showAutocomplete=function(){
			console.log("Clicked: Show Autocomplete");
		}
		
		/* Function to Select All Code in Code Editor */
		base.selectAll=function(){
			base.getEditorInstance(base.getActiveEditor()).instance.session.selection.selectAll();
		}
		
		/* Function for Finding an Item */
		base.findItem=function(){
			console.log("Clicked: Find...");
		}
		
		/* Function for Find Next Item */
		base.findNext=function(){
			console.log("Clicked: Find Next");
		}
		
		/* Function for Find Previous Item */
		base.findPrevious=function(){
			console.log("Clicked: Find Previous");
		}
		
		/* Function for Replace */
		base.replaceAction=function(){
			console.log("Clicked: Replace...");
		}
		
		/* Function for Replace Next */
		base.replaceNext=function(){
			console.log("Clicked: Replace Next");
		}
		
		/* Function for Replace Previous */
		base.replacePrevious=function(){
			console.log("Clicked: Replace Previous");
		}
		
		/* Function for Replace All */
		base.replaceAll=function(){
			console.log("Clicked: Replace All");
		}
		
		/* Function for Find in Files */
		base.findInFiles=function(){
			console.log("Clicked: Find in Files...");
		}
		
		/* Function for Increasing Font Size */
		base.increaseFontSize=function(){
			console.log("Clicked: Increase Font Size");
		}
		
		/* Function for Decreasing Font Size */
		base.decreaseFontSize=function(){
			console.log("Clicked: Decrease Font Size");
		}
		
		/* Function for Wrapping Lines */
		base.wrapLines=function(){
			console.log("Clicked: Wrap Lines");
		}
		
		/* Function for Wrapping to Viewport */
		base.wrapToViewport=function(){
			console.log("Clicked: Wrap to Viewport");
		}
		
		/* Function for Goto File */
		base.goToFile=function(){
			console.log("Clicked: Goto File...");
		}
		
		/* Function for Goto Line */
		base.goToLine=function(){
			console.log("Clicked: Goto Line...");
		}
		
		/* Function for Beautiful Selection */
		base.beautifulSelection=function(){
			console.log("Clicked: Beautiful Selection");
		}
		
		/* Function for Stripping Whitespace */
		base.stripWhitespace=function(){
			console.log("Clicked: Strip Whitespace");
		}
		
		/* Function for Preview */
		base.fileOutputPreview=function(){
			console.log("Clicked: Preview");
		}
		
		
		
		/* ******************************************************************************* */
		/* Initialiazing First Function When Page is Loaded ****************************** */
		/* ******************************************************************************* */
        base.init();
    }
	
    $.sceditor.defaultOptions = {configAce:{
					setTheme			: "twilight",
					setMode				: "javascript",
					setTabSize			: "10",
					setUseWrapMode		: "true",
					fontSize			: "12px",
					setShowPrintMargin	: "true",
					setReadOnly			: "false",
					treeNavigation		: '.sitenavdropdown',
					autoSetMode			: true,
					tabListUl			: "#editorfilelist"
				},				
				/* Values for Tree */
				configTree:{
					treeRootElement		: "#filetree",
                    root: "./wwwroot/workspace/themes/",
                    baseUrl: "/admin/seaport-cargo/themes.html",
					script				: '/admin/ajax/file-tree.html',
					xpandSpeed			: 300,
					collapseSpeed		: 300,
					multiFolder			: true
				}
		
    };
	
    $.fn.sceditor = function (op) {
        return this.each(function () {
            (new $.sceditor(this, op));
        });
    };
})(jQuery);

















