//On load of page, set up this key information.
window.onload = function startupConfiguration() {
	//array of question functions, this is what the system uses to go through the questions in a quiz
	currentGameSession.questionArray = [question1, question2];
	//Sets the games max score
	setMaxScore(10, false);
	//Sets up the quiz title and wether it is a multi question quiz.
	headerSetup('Jimmy Hendrix Quiz', true);
	//load question 1 to start
	question1();
}


function question1() {
	//clear the question variable to fill with new content
	question = {};
	//add the weight of the question to the scoreWeights array. (Sets the weight)
	currentGameSession.scoreWeights.push(5);
	//Sets the questions max score
	currentGameSession.maxScore.push(5);
	var options = ["A", "B", "C", "D"];
	//Popups is only for multiple choice questions and does not need to be included
	var popupItems = [["Hello A!", "Sup?"],["Hello B!", "Sup?"], ["Hello C!", "Sup?"], ["Hello D!", "Sup?"]];
	var answer = options[1];
	//if not true popups will not be expected by the system.
	enablePopups(true);
	//Define the contents of the question variable, this contains everything that the selected mode's module should expect, in this case what the multiple choice game mode requries to run.
	//nextQIndex determines where in the questionArray the next button will go, if last question or only question, set to -1.
	question = {nextQIndex: 1, question: 'Select Option B', answer: answer, options: options, popup: true, popupItems: popupItems};
	//Trigger the required module to run the question, in this case it is a multiple choice so I start the multiple choice frameworks multiStart function.
	multiStart();
}

function question2() {
	question = {};
	currentGameSession.scoreWeights.push(10);
	currentGameSession.maxScore.push(10);
	var options = ["Played guitar with his teeth", "Said he felt small after first hearing Jimi Hendrix", "Played guitar in The Who",
		"Played guitar in Cream", "Had a band called The Experience", "Set his guitar on fire at Monterey Pop Festival", "Smashed his guitar at Monterey Pop Festival",
		"Played guitar on “Sunshine of Your Love”", "Was said to steal Pete Townshend’s act", "Had played guitar in John Mayall’s Blues Breakers"];

	var itemDrops = ['Jimi Hendrix', 'Eric Clapton', 'Pete Townshend', 'Jimi Hendrix & Pete Townshend'];
	
	var answers = new Array(2);
	answers[0] = [options[4]];
	answers[1] = [options[7], options[5]];
	answers[2] = [options[0], options[2], options[9]];
	answers[3] = [options[1], options[3], options[6], options[8]];
	
	//Since this is the last question in the quiz, the nextQIndex is set to -1.
	question = {nextQIndex: -1, question: 'Match the information to the correct person', answer: answers, options: options, itemDrops: itemDrops};
	//This one starts the drag and drop module, same idea as the multiple choice one above.
	dndStart();
}