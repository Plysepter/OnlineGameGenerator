//Sets up the DOM for a multiple choice question.
function multiStart() {
	document.getElementById('question').innerHTML = question.question;
	document.getElementById('options').innerHTML = "";
	var htmlOutput = '<p>'
	for(var i = 1; i < question.options.length + 1; i++)
	{
		htmlOutput += '<button id="option' + i + '" class="option option' + i + '" onclick="multiCheck(event, ' + i + ')">' + question.options[i - 1] + '</button>';
		document.getElementById('options').innerHTML = htmlOutput + '</p>';
	}
}

//Check the users answer
function multiCheck(e, optionNumber)
{
	//reset question score
	var playerAnswer = e.target.textContent;
	//Check if user selected correct answer
	if(playerAnswer == question.answer)
	{
		if (question.nextQIndex == -1)
		{
			if ((currentGameSession.score.length - 1) == (currentGameSession.maxScore.length - 1))
			{
				scoreDecrease(currentGameSession.scoreWeights[-1]);
			}
			scoreIncrease(currentGameSession.scoreWeights[-1]);
		}
		else
		{
			if ((currentGameSession.score.length - 1) == currentGameSession.maxScore[question.nextQIndex])
			{
				scoreDecrease(currentGameSession.scoreWeights[question.nextQIndex]);
			}
			scoreIncrease(currentGameSession.scoreWeights[question.nextQIndex]);
		}
		//scoreIncrease(1);
	}
	multiResults(optionNumber);
}

//Colour coat the options
function multiResults(oNum) {
	for(var i = 0; i < question.options.length; i++)
	{
		if(question.options[i] == question.answer)
		{
			document.getElementById('option' + (i + 1)).style.background = 'Green';
		}
		else
		{
			document.getElementById('option' + (i + 1)).style.background = 'Red';
		}
	}
	//If popups have been enabled, activate them.
	if(question.hasOwnProperty('popup') && question.popup == true)
	{
		var pop = question.popupItems[oNum - 1];
		popupToggle(pop);
	}
}