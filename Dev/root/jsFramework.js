//Generic variable to place all required question information into to pass to the game modes module.
var question;
//Stores all the games constants. limiting global pollution. 
var currentGameSession = {scoreWeights: [0], maxScore: [0], questionArray: [], addIns: []};

//move the user to the next question in the 'game'
function nextQuestion() {
	removeLeftOvers();
	if(question.nextQIndex != -1)
	{
		//move the array to the current questions nextQIndex
		currentGameSession.questionArray[question.nextQIndex]();
		//by default the previous button is disabled since it is the first question, since we are moving forward, we can enable it to the user.
		document.getElementById('prevQ').disabled = false;
	}
	else
	{
		//if it was the last question in the quiz, finish the game
		finish();
	}
}

//Moves the user back a question.
function prevQuestion() {
	removeLeftOvers();
	//Moves back as long as user is not on the first question, as there is nowhere to move to.
	if(question.nextQIndex != 0)
	{
		//Ensure it is safe before moving the user anywhere
		if (question.nextQIndex == -1)
		{
			currentGameSession.questionArray[currentGameSession.questionArray.length - 2]();
		}
		else if(question.nextQIndex > 1)
		{
			currentGameSession.questionArray[question.nextQIndex - 2]();
		}
		//enable both navigation buttons.
		document.getElementById('prevQ').disabled = false;
		document.getElementById('nextQ').disabled = false;
	}
}

//Clean up after the previous question. I decided to make a generic module that should be able to clean up after each question type without expecting the question type to have its own clean up process.
function removeLeftOvers() {
	//The addIns array is to place all the element id's that a question type has added to the DOM.
	//Then This function goes through that array and simply removes all the added elements from the DOM, cleaning up for the next question.
	for(var i = 0; i < currentGameSession.addIns.length; i++) {
		document.body.removeChild(document.getElementById(currentGameSession.addIns[i]));
	}
	//Clear the addIns array as it is a per question thing, it is VERY IMPORTANT that this is properly populated by the question types created.
	currentGameSession.addIns = [];
}

//If the game has finished then show the user their total score and let them know they have finished.
function finish() {
	document.getElementById('questionArea').innerHTML = '<header id="question" class="question">Complete!</header>' +
		'<div id="options">' +
			'<p>You scored ' + currentGameSession.scoreWeights[0] + " out of " + currentGameSession.maxScore[0] + '</p>' +
		'</div>';
	//Hide all the navigation buttons and the score counter as it is already being displayed somewhere else.
	document.getElementById('nextQ').style.visibility = "hidden";
	document.getElementById('prevQ').style.visibility = "hidden";
	document.getElementById('score').style.visibility = "hidden";
}

//Update the score of the user
//amount is passed from the game type engine and is based off of the questions score weight. The reason it is not just the score weight is because not all questions are yes/no.
function scoreIncrease(amount) {
	if(question.nextQIndex != 0)
	{
		//if it is the last question, we have to go to the array index differently as using the index of -1 is not going to get us anywhere.
		currentGameSession.scoreWeights[question.nextQIndex] = amount;
	}
	calculateCurrentScore();
	//update the score to the user.
	document.getElementById('score').innerHTML = currentGameSession.scoreWeights[0] + " of " + currentGameSession.maxScore[0];
}

//This allows us to lower the score if we choose to, I do this when I wanted to reset a drag and drop excersize
function scoreDecrease(amount) {
	if(question.nextQIndex != 0)
	{
		currentGameSession.scoreWeights[question.nextQIndex] -= amount;
	}
	calculateCurrentScore();
	//update score for the user
	document.getElementById('score').innerHTML = currentGameSession.scoreWeights[0] + " of " + currentGameSession.maxScore[0];
}

//This function allows to reset either the entire games score or just a single questions previously earned score.
function scoreReset(hardReset) {
	//Reset all scores
	if (hardReset == true)
	{
		for(var i = 0; i <= currentGameSession.scoreWeights.length; i++)
		{
			currentGameSession.scoreWeights[i] = 0;
		}
	}
	//reset current question only
	else if(question.nextQIndex != 0)
	{
		//access the questions score
		currentGameSession.scoreWeights[question.nextQIndex] = 0;
	}
	calculateCurrentScore();
	//update score to user
	document.getElementById('score').innerHTML = currentGameSession.scoreWeights[0] + " of " + currentGameSession.maxScore[0];
}

//Sets the max score
function setMaxScore(max, isQuestionMax) {
	//if it is not a questions max score then set the games max score.
	if (isQuestionMax == false)
	{
		currentGameSession.maxScore[0] = max;
	}
	//if it is the question specific max score then set that questions score.
	else if(question.nextQIndex != 0)
	{
		currentGameSession.maxScore[question.nextQIndex] = max;
	}
	//update the score for the user
	document.getElementById('score').innerHTML = currentGameSession.scoreWeights[0] + " of " + currentGameSession.maxScore[0];
}

//tally users score
function calculateCurrentScore() {
	var tempTotal = 0;
	for(var i = -1; i < currentGameSession.scoreWeights.length; i++) {
		if(currentGameSession.scoreWeights[i] != undefined)
		{
			if(i != 0)
			{
				tempTotal += currentGameSession.scoreWeights[i];
			}
		}
		else
		{
			tempTotal += 0;
		}
	}
	currentGameSession.scoreWeights[0] = tempTotal;
}

//configure the page header to put 'previous' and 'next' buttons if there are multiple questions in a game.
function headerSetup(gameName, isMultiQuestion) {
	if(isMultiQuestion == true)
	{
		document.getElementById('gameName').innerHTML = '<button type="button" id="prevQ" class="button" onclick="prevQuestion()">Prev</button>' +
			gameName + '<button type="button" id="nextQ" class="button" onclick="nextQuestion()">Next</button>';	
		document.getElementById('score').innerHTML = currentGameSession.scoreWeights[0] + " of " + currentGameSession.maxScore[0];
		document.getElementById('prevQ').disabled = true;
	}
	else
	{
		document.getElementById('gameName').innerHTML = gameName;
		document.getElementById('score').innerHTML = currentGameSession.scoreWeights[0] + " of " + currentGameSession.maxScore[0];
	}
}
