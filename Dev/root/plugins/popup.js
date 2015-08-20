//Enable popups
function enablePopups(x){
	if(x == true)
	{
		var htmlOutput = '<div id="popupBoxPosition">' +
			'<div id="popupBoxWrapper">' +
				'<div id="popupBoxContent" style="text-align: center">' +
				'</div>' +
			'</div>' +
		'</div>';	
		document.body.innerHTML += htmlOutput;
		currentGameSession.addIns.push('popupBoxPosition');
	}
}

function popupToggle(content) {
	var htmlOutput = '<h3>' + content[0] + '</h3>' +
		'<p>' + content[1] + '</p>' +
		'<button id="closePopup" class="option" onclick="popupToggle(\'popupBoxPosition\')">Close</button>';
	var e = document.getElementById('popupBoxPosition');
	document.getElementById('popupBoxContent').innerHTML = htmlOutput;
	if(e.style.display == 'block')
	{
		e.style.display = 'none'
	}
	else
	{
		e.style.display = 'block';
	}
}

