//Configure the DOM and add required elements to run a drag and drop question.
function dndStart() {
	document.getElementById('options').setAttribute("class", "itemContainer");
	document.getElementById('options').setAttribute("ondrop", "dropped(event)");
	document.getElementById('options').setAttribute("ondragover", "dragOver(event)");
	document.getElementById('question').innerHTML = '<button id="check" class="button" onclick="dndCheck(event)">Check</button>' +
			question.question + '<button id="reset" class="button" onclick="dndReset()">Reset</button>';
	document.getElementById('options').innerHTML = "";
	var htmlOutput = '<div id="itemDropContainer"  class="slideDown">'
	//Create elements to be added to the DOM
	for(var i = 0; i < question.itemDrops.length; i++)
	{
		htmlOutput += '<section class="dropArea">' +
				'<header class="question" id="itemDrop' + (i + 1) + 'Label">' + question.itemDrops[i] + '</header>' +
				'<div id="itemDrop' + (i + 1) + '" class="itemContainer" ondrop="dropped(event)" ondragover="dragOver(event)"></div>' +
			'</section>';			
	}
	document.body.innerHTML += htmlOutput + '</div>';
	//Change the display layout of options
	document.getElementById('itemDropContainer').style.display = 'block';
	for(var i = 0; i < question.options.length; i++)
	{
		document.getElementById('options').innerHTML += '<p class="item" id="matchItem' + i + '" draggable="true" ondragstart="dragTransfer(event)">' + question.options[i] + '</p>';
	}
	currentGameSession.addIns.push('itemDropContainer');
}

//move content from one element to another when dragged
function dragTransfer(e){
	e.dataTransfer.setData("Text", e.target.id);
}

//Stop the default dragging behaviour
function dragOver(e){
	e.preventDefault();
}

//What happens when an item is dropped by the user
function dropped(e){
	e.preventDefault();
	var src = e.dataTransfer.getData("Text");
	//if the user drops the item onto a container then allow it.
	if(e.target.className == 'itemContainer')
	{
		e.target.appendChild(document.getElementById(src));
	}
}

//Go through all items and verify if they are in the correct container.
//Then colour the items based on if correct or not.
function dndCheck()
{
	scoreReset(false);
	var tally = 0;
	for(var a = 0; a < question.itemDrops.length; a++)
	{
		var itemBox = "itemDrop" + (a + 1);
		var nodes = document.getElementById(itemBox).childNodes;
		for(var i = 0; i < nodes.length; i++)
		{
			for( var y = 0; y < question.answer[a].length; y++)
			{
				if(nodes[i].innerHTML == question.answer[a][y])
				{
					nodes[i].style.background = 'green';
					tally += 1;
					break;
				}
				else
				{
					nodes[i].style.background = 'red';
				}
			}
		}
	}
	scoreIncrease(tally);
}

//reset item positions and recolour them.
function dndReset()
{
	scoreReset(false);
	for(var a = 0; a < question.itemDrops.length; a++)
	{
		var itemBox = "itemDrop" + (a + 1);
		var nodes = document.getElementById(itemBox).childNodes;
		for (var x = 0; x < document.getElementById('options').childNodes.length; x++)
		{
			document.getElementById('options').childNodes[x].style.background = '#330072';
		}
		while (document.getElementById(itemBox).childNodes.length > 0)
		{
			nodes[0].style.background = '#330072';
			document.getElementById('options').appendChild(nodes[0]);
		}
	}
}