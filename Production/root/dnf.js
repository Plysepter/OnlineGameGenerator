function dndStart(){document.getElementById("options").setAttribute("class","itemContainer"),document.getElementById("options").setAttribute("ondrop","dropped(event)"),document.getElementById("options").setAttribute("ondragover","dragOver(event)"),document.getElementById("question").innerHTML='<button id="check" class="button" onclick="dndCheck(event)">Check</button>'+question.question+'<button id="reset" class="button" onclick="dndReset()">Reset</button>',document.getElementById("options").innerHTML="";for(var e='<div id="itemDropContainer"  class="slideDown">',t=0;t<question.itemDrops.length;t++)e+='<section class="dropArea"><header class="question" id="itemDrop'+(t+1)+'Label">'+question.itemDrops[t]+'</header><div id="itemDrop'+(t+1)+'" class="itemContainer" ondrop="dropped(event)" ondragover="dragOver(event)"></div></section>';document.body.innerHTML+=e+"</div>",document.getElementById("itemDropContainer").style.display="block";for(var t=0;t<question.options.length;t++)document.getElementById("options").innerHTML+='<p class="item" id="matchItem'+t+'" draggable="true" ondragstart="dragTransfer(event)">'+question.options[t]+"</p>";currentGameSession.addIns.push("itemDropContainer")}function dragTransfer(e){e.dataTransfer.setData("Text",e.target.id)}function dragOver(e){e.preventDefault()}function dropped(e){e.preventDefault();var t=e.dataTransfer.getData("Text");"itemContainer"==e.target.className&&e.target.appendChild(document.getElementById(t)),"item"==e.target.className&&e.target.parentNode.appendChild(document.getElementById(t))}function dndCheck(){scoreReset(!1);for(var e=0,t=0;t<question.itemDrops.length;t++)for(var n="itemDrop"+(t+1),o=document.getElementById(n).childNodes,d=0;d<o.length;d++)for(var r=0;r<question.answer[t].length;r++){if(o[d].innerHTML==question.answer[t][r]){o[d].style.background="green",e+=1;break}o[d].style.background="red"}scoreIncrease(e)}function dndReset(){scoreReset(!1);for(var e=0;e<question.itemDrops.length;e++){for(var t="itemDrop"+(e+1),n=document.getElementById(t).childNodes,o=0;o<document.getElementById("options").childNodes.length;o++)document.getElementById("options").childNodes[o].style.background="#330072";for(;document.getElementById(t).childNodes.length>0;)n[0].style.background="#330072",document.getElementById("options").appendChild(n[0])}}