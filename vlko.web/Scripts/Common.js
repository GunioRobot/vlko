﻿var $loadingDialog;

// create loading dialog
function createLoading() {
	$loadingDialog = $('<div class="loading_dialog"></div>')
		.html('Loading...')
		.dialog({
			autoOpen: true,
			modal: true,
			close: function () { $(".loading_dialog").empty(); },
			title: 'Loading...'
		});
}

// close loading dialog
function closeLoading() {
	if ($loadingDialog) {
		$loadingDialog.dialog("close");
	}
	$loadingDialog = null;
}

// handle ajax exception
function ajaxException(xhr, ajaxOptions, thrownError) {
	closeLoading();
	$('<div></div>')
		.html(xhr.responseText)
		.dialog({
			autoOpen: true,
			width: $("#content").width(),
			modal: true,
			title: "Error:" + thrownError
		});
	alert(thrownError);
}

// create content dialog
function createContentDialog(settings) {
	var config = {
		buttons: { "Ok": function () { $(this).dialog("close") } },
		dialogName: "content_dialog"
	}
	$.extend(config, settings);

	var contentDialog = $('<div class="' + config.dialogName + '"></div>')

						.dialog({
							autoOpen: false,
							modal: true,
							width: $("#content").width(),
							draggable: true,
							resizable: true,
							title: config.title,
							close: function () {
								$("." + config.dialogName).empty(); 
								if (config.prevUrl !== null){
									addToHistory(config.prevUrl);
								}
							},
							buttons: config.buttons
						});
	fillContentWithData(contentDialog, config.data);
	return contentDialog;
}

function fillContentWithData(content, data) {
	content.html(data);
	content.children(":not(.ajax_container, .ajax_content)").hide();
	content.children(".ajax_container").children(":not(.ajax_content)").hide();
}

function updateEffect(content, callback) {
	content.effect("slide", { direction: "up" }, 300, callback);
}

// ajax history function
function addToHistory(url) {
	$.history.load(url);
}
// get current ajax url
function getCurrentHistoryUrl() {
	return $.history.appState;
}

// initialize ajax history plugin
$.history.init(function (url, phase) {
	if (phase == "check") {
		if (!url) {
			window.location = window.location.href;
		}
		else {
			if (url.charAt(0) == "%") {
				window.location = url;
			}
		}
	}
	if (phase == "init") {
		if (url && (url.charAt(0) == "%")) {
			window.location = url;
		}
	}
});