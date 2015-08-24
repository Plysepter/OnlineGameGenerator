using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace OnlineGameContentGenerator
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            createApplyQuestionButton();
            newQuestion(false);
        }

        //List to hold the questions
        //List<questionObjectBase> game = new List<questionObjectBase>();
        private Dictionary<int, dynamic> game = new Dictionary<int, dynamic>();

        //questionNumber Keeps track of the number of questions
        int questionNumber = 1;
        // itemContainerCount keeps track of how many containers the question has
        int itemContainerCount = 0;
        //itemCount holds the selected questions item Count
        int itemCount = 0;
        //lastSelected Holds the last selected Question Panel
        Panel lastSelected;

        //Panel questionEdit holds the main information to edit for a question (the question, type of question and the weight of the question).
        Panel questionEdit = new Panel();

        //applyQuestionInfo sets up the question structure, once this has run, the question type cannot be changed.
        Button applyQuestionInfo = new Button();

        private void createQuestionEdit()
        {
            questionEdit.Name = "questionEdit";
            questionEdit.Size = new Size(320, 106);
            questionEdit.BackColor = System.Drawing.Color.DarkGray;

            //User can enter the question
            Label questionText = new Label();
            questionText.Location = new Point(3, 7);
            questionText.Text = "Question:";
            questionText.Name = "questionNum";
            questionText.Size = new Size(59, 20);
            TextBox questionTextBox = new TextBox();
            questionTextBox.Name = "questionText";
            questionTextBox.Location = new Point(68, 6);
            questionTextBox.Size = new Size(247, 20);
            questionTextBox.MaxLength = 500;

            //User can select What type of question it is
            Label questionTypeText = new Label();
            questionTypeText.Location = new Point(3, 32);
            questionTypeText.Text = "Type:";
            questionTypeText.Size = new Size(59, 20);
            ComboBox questionType = new ComboBox();
            questionType.Name = "type";
            questionType.Location = new Point(68, 29);
            questionType.Size = new Size(247, 20);
            questionType.Items.Add("Drag & Drop");
            questionType.Items.Add("Multiple Choice");

            //User can select the weighting of the question
            Label questionWeightText = new Label();
            questionWeightText.Location = new Point(3, 55);
            questionWeightText.Text = "Weight:";
            questionWeightText.Size = new Size(59, 20);
            NumericUpDown questionWeight = new NumericUpDown();
            questionWeight.Name = "weight";
            questionWeight.Location = new Point(68, 53);
            questionWeight.Size = new Size(247, 20);
            questionWeight.Maximum = 1000;
            questionWeight.Minimum = 1;

            questionEdit.Controls.Add(questionText);
            questionEdit.Controls.Add(questionTextBox);
            questionEdit.Controls.Add(questionTypeText);
            questionEdit.Controls.Add(questionType);
            questionEdit.Controls.Add(questionWeightText);
            questionEdit.Controls.Add(questionWeight);

            //bind the control to the main control panel
            pnlQuestionDetails.Controls.Add(questionEdit);
        }

        //Create button to be placed on QuestionEdit when it is a new question
        private void createApplyQuestionButton()
        {
            applyQuestionInfo.Name = "applyQuestionInfo";
            applyQuestionInfo.Text = "Apply";
            applyQuestionInfo.Location = new Point(245, 79);
            applyQuestionInfo.Size = new Size(70, 24);
            applyQuestionInfo.FlatStyle = FlatStyle.Flat;
            applyQuestionInfo.FlatAppearance.BorderSize = 0;
            applyQuestionInfo.FlatAppearance.BorderColor = System.Drawing.Color.Red;
            applyQuestionInfo.BackColor = System.Drawing.Color.Black;
            applyQuestionInfo.ForeColor = System.Drawing.Color.White;
            applyQuestionInfo.Click += applyQuestionSettingsEvent;
        }

        //Load information for the selected question
        private void loadPnlQuestionDetails()
        {
            //Remove leftovers from last question
            pnlQuestionDetails.Controls.Clear();
            pnlQuestionDetails.Controls.Add(questionEdit);

            //Get contents from object in game list
            questionEdit.Controls.Find("questionText", true)[0].Text = game[(int)lastSelected.Tag].questionText;
            questionEdit.Controls.Find("type", true)[0].Text = game[(int)lastSelected.Tag].questionType;
            questionEdit.Controls.Find("weight", true)[0].Text = game[(int)lastSelected.Tag].weight.ToString();

            //if the apply button doesn't exist, check if the question is "empty", if so then create apply button and enable the type combobox.
            if (questionEdit.Controls.Contains(applyQuestionInfo) == false)
            {
                questionEdit.Controls.Add(applyQuestionInfo);
            }
            //If apply button is there, check to see if the question needs it, if not, remove it.
            if (game[(int)lastSelected.Tag].questionType == null || game[(int)lastSelected.Tag].questionType == "")
            {
                questionEdit.Controls.Find("type", true)[0].Enabled = true;
            }
            if (game[(int)lastSelected.Tag].questionType != null)
            {
                questionEdit.Controls.Find("type", true)[0].Enabled = false;
                var apply = questionEdit.GetChildAtPoint(new Point(245, 79));
                questionEdit.Controls.Remove(apply);
            }
            try
            {
                loadQuestion();
            }
            catch (Exception)
            {
                //Do nothing, if load fails, it just means the question is empty and has nothing to load.
            }
        }

        //Simple error "handling" function
        private void errorHandle(string errMess)
        {
            MessageBox.Show("ERROR:\n" + errMess);
        }

        private void btnNewQuestion_Click(object sender, EventArgs e)
        {
            newQuestion(false);
        }

        private void btnAddOption_Click(object sender, EventArgs e)
        {
            newItem();
        }

        private void newItem()
        {
            var question = game[(int)lastSelected.Tag];
            if (question is questionObjectSimple)
            {
                newItemSimple();
            }
            else if (question is questionObjectComplex)
            {
                newItemComplex();
            }
        }

        private void applyQuestionSettingsEvent(object sender, EventArgs e)
        {
            applyQuestionSettings();
        }

        //Apply the main settings to question before adding to the question
        private void applyQuestionSettings()
        {
            try
            {
                var type = questionEdit.GetChildAtPoint(new Point(68, 29));
                var text = questionEdit.GetChildAtPoint(new Point(68, 6)).Text;

                //If user has not entered a question, throw error.
                if (text == "" || text == null)
                {
                    errorHandle("Please enter a question before continuing.");
                    return;
                }
                else
                {
                    //If user has not selected a type, throw error.
                    if (type.Text == "" || type.Text == null)
                    {
                        errorHandle("Please select a question type before continuing.");
                        return;
                    }
                    //Remove the apply button, disable changing the question type and store basic info into the array
                    else
                    {
                        var apply = questionEdit.GetChildAtPoint(new Point(245, 79));
                        var weight = questionEdit.GetChildAtPoint(new Point(68, 53)).Text;

                        if (type.Text == "Multiple Choice" || type.Text == "True or False")
                        {
                            questionObjectSimple question = new questionObjectSimple();
                            question.questionText = text;
                            question.questionType = type.Text;
                            question.questionNumber = lastSelected.Controls.Find("QuestionNumber", true)[0].Text;
                            int.TryParse(weight, out question.weight);
                            game[(int)lastSelected.Tag] = question;
                        }
                        else if (type.Text == "Drag & Drop")
                        {
                            questionObjectComplex question = new questionObjectComplex();
                            question.questionText = text;
                            question.questionType = type.Text;
                            question.questionNumber = lastSelected.Controls.Find("QuestionNumber", true)[0].Text;
                            int.TryParse(weight, out question.weight);
                            game[(int)lastSelected.Tag] = question;
                        }
                        else
                        {
                        }
                        questionEdit.Controls.Remove(apply);
                        type.Enabled = false;
                        lastSelected.Controls.Find("Question", true)[0].Text = text;
                        lastSelected.Controls.Find("QuestionType", true)[0].Text = type.Text;
                        itemCount = 0;
                        pnlQuestionOptions.Controls.Find("btnAddOption", true)[0].Enabled = true;
                    }
                }
            }
            catch (Exception)
            {
                errorHandle("Could not configure the question\nPlease try again.");
            }
        }

        private void btnApply_Click(object sender, EventArgs e)
        {
            applyAll();
        }

        private void applyAll()
        {
            var question = game[(int)lastSelected.Tag];
            if (question is questionObjectSimple)
            {
                applyQuestionSettings();
                applyChangesToQuesitonSimple((questionObjectSimple)question, true);
            }
            else if (question is questionObjectComplex)
            {
                applyQuestionSettings();
                applyChangesToQuesitonComplex((questionObjectComplex)question);
            }
        }

        private void btnGameGenerate_Click(object sender, EventArgs e)
        {
            applyAll();
            string colour = null;
            string gameN = null;
            string filename = "";

            Form2 gameName = new Form2();

            //Show testDialog as a modal dialog and determine if DialogResult = OK. 
            if (gameName.ShowDialog(this) == DialogResult.OK)
            {
                //Read the contents of gameName's TextBox.               
                gameN = gameName.txtName.Text;
                filename = gameName.txtName.Text + ".html";
            }
            else
            {
                return;
            }

            //Destroy the second form from memory because there is no reason
            gameName.Dispose();

            string directory = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            string path = Path.Combine(directory, filename);
            DialogResult = MessageBox.Show("Would you like to use a custom colour scheme?", "Change colour:", MessageBoxButtons.YesNo);
            if (DialogResult == DialogResult.Yes)
            {
                //Allow for changing of main colour
                colourPicker.ShowDialog();
                var colourChoice = colourPicker.Color;
                colour = ColorTranslator.ToHtml(colourChoice);
            }
            //Variables to check which game type frameworks should be connected to.
            bool dnd = false;
            bool mc = false;
            bool popup = false;

            //Go through each question within the game and trigger the correct framework variables
            foreach (var question in game)
            {
                if (question.Value is questionObjectSimple)
                {
                    mc = true;
                    //Check for if game uses popups in any of the questions. If it does, will include popup plugin.
                    for (int f = 0; f < question.Value.questionItems.Count; f++)
                    {
                        if (question.Value.questionItems[f].popups.popupEnabled == true)
                        {
                            popup = true;
                        }
                    }
                }
                else if (question.Value is questionObjectComplex)
                {
                    dnd = true;
                }
            }

            //Using StringBuilder instead of just a string or the original write, due to performance gains. When modifying a string repeatedly, a lot of overhead is used with realocating the memory. StringBuilder does
            //not deal with this issue and therefore saves time. The reason I am changing to just a single write vs multiple is due to errors. This way I can return out of the function without having a half completed file
            //saved to disk.

            //Setting the default max capacity to around 1500 as a start and it will automatically double when capacity is reached. Just to make sure that it is not constantly allocating more memory.
            StringBuilder documentContent = new StringBuilder("<!DOCTYPE html>", 1500);

            documentContent.Append("<html lang=\"en\">");
            documentContent.Append("<head>");
            documentContent.Append("<meta http-equiv=\"Content-Type\" content=\"text/html; charset=UTF-8\">");
            documentContent.Append("<link rel=\"stylesheet\" href=\"https://mylearningspace.wlu.ca/shared/root/tna.css\">");
            //if a question is a drag n drop then link the framework
            if (dnd == true)
            {
                documentContent.Append("<link rel=\"stylesheet\" href=\"https://mylearningspace.wlu.ca/shared/root/dnd.css\">");
            }
            //if a question is a multiple choice then link the appropriate framework
            if (mc == true)
            {
                if (popup == true)
                {
                    documentContent.Append("<link rel=\"stylesheet\" href=\"https://mylearningspace.wlu.ca/shared/root/pop.css\">");
                }
            }
            if (colour != null)
            {
                documentContent.Append("<style>#header,.item,.option,.question{background:" + colour + "}.button{color:" + colour + "}</style>");
            }
            documentContent.Append("<script src=\"https://mylearningspace.wlu.ca/shared/root/jsf.js\" type=\"text/javascript\" async></script>");
            //Writting Javascript underneath the css, this is why I check the booleans twice.
            if (dnd == true)
            {
                documentContent.Append("<script src=\"https://mylearningspace.wlu.ca/shared/root/dnf.js\" type=\"text/javascript\" async></script>");
            }
            if (mc == true)
            {
                documentContent.Append("<script src=\"https://mylearningspace.wlu.ca/shared/root/mpf.js\" type=\"text/javascript\" asnyc></script>");
            }
            documentContent.Append("<script>");
            bool multiQ = false;
            string questionArray = "";
            int maxScore = 0;
            int i = 1;
            List<questionItemSimple> simpleOptions = new List<questionItemSimple>();
            List<questionItemBase> complexOptions = new List<questionItemBase>();
            string options = "";
            string itemDrops = "";
            string popupItems = "";
            string answers = "";
            int nextQuestion;

            if (game.Count > 1)
            {
                multiQ = true;
            }

            foreach (var question in game)
            {
                options = "";
                popupItems = "";
                answers = "";
                nextQuestion = i;
                if (i != 1)
                {
                    questionArray += ", ";
                }
                //q stands for question
                questionArray += "q" + i;
                if (question.Value is questionObjectSimple)
                {
                    documentContent.Append("function q" + i + "() {");
                    documentContent.Append("question = {};");                    

                    for (int c = 0; c < question.Value.questionItems.Count; c++)
                    {
                        //Add correct items to the answers string
                        if (question.Value.questionItems[c].correct == true)
                        {
                            maxScore++;
                            if (answers == "")
                            {
                                //o for options
                                answers += "o[" + (c) + "]";
                            }
                            else
                            {
                                answers += ", o[" + (c) + "]";
                            }
                        }
                        //Add the items in the question to the options list
                        if (options == "")
                        {
                            options += "\"" + question.Value.questionItems[c].itemText + "\"";
                        }
                        else
                        {
                            options += ", \"" + question.Value.questionItems[c].itemText + "\"";
                        }
                    }
                    
                    documentContent.Append("var o = [" + options + "];");
                    //Enable popups and include popup items
                    for (int y = 0; y < question.Value.questionItems.Count; y++)
                    {
                        if (question.Value.questionItems[y].popups.popupEnabled == true)
                        {
                            popup = true;
                            if (popupItems != "")
                            {
                                popupItems += " ,";
                            }
                            popupItems += "[\"" + question.Value.questionItems[y].popups.popupTitle + "\", \"" + question.Value.questionItems[y].popups.popupBody + "\"]";
                        }
                    }
                    documentContent.Append("var popupItems = [" + popupItems + "];");
                    documentContent.Append("var answer = " + answers + ";");
                    documentContent.Append("enablePopups(" + popup.ToString().ToLower() + ");");
                    if (i == game.Count())
                    {
                        nextQuestion = -1;
                    }
                    documentContent.Append("question = {nextQIndex: " + nextQuestion + ", question: '" + question.Value.questionText + "', answer: answer, options: o, popup: " + popup.ToString().ToLower() + ", popupItems: popupItems};");

                    documentContent.Append("if (currentGameSession.scoreWeights.length >= question.nextQIndex){");
                    documentContent.Append("currentGameSession.score[question.nextQIndex] =0;");
                    documentContent.Append("currentGameSession.scoreWeights[question.nextQIndex] =" + question.Value.weight + ";");
                    documentContent.Append("currentGameSession.maxScore[question.nextQIndex] =" + question.Value.weight + ";}");
                    documentContent.Append("else {");
                    documentContent.Append("currentGameSession.score.push(0);");
                    documentContent.Append("currentGameSession.scoreWeights.push(" + question.Value.weight + ");");
                    documentContent.Append("currentGameSession.maxScore.push(" + question.Value.weight + ");");
                    documentContent.Append("}");

                    documentContent.Append("multiStart();");
                    documentContent.Append("}");
                }
                else if (question.Value is questionObjectComplex)
                {
                    options = "";
                    answers = "";
                    itemDrops = "";

                    documentContent.Append("function q" + i + "() {");
                    documentContent.Append("question = {};");

                    for (int j = 0; j < question.Value.questionItems.Count; j++)
                    {
                        //reshuffle complexOptions list to randomize the listings in the game
                        List<questionItemBase> complexOptionsTemp = new List<questionItemBase>();
                        try
                        {
                            for (int o = 0; o < question.Value.questionItems[j].items.Count; o++)
                            {
                                //Add itembox values into main item pool
                                complexOptions.Add(question.Value.questionItems[j].items[o]);
                            }
                        }
                        catch (Exception)
                        {

                            errorHandle("Cannot have a Container without items inside it. Sorry!");
                            return;
                        }
                    }

                    //Randomize the items in complexOptions so that the drag and drop doesn't display the options in the correct order.
                    Random rng = new Random();
                    int n = complexOptions.Count;
                    while (n > 1)
                    {
                        n--;
                        int k = rng.Next(0, n);
                        questionItemBase value = complexOptions[k];
                        complexOptions[k] = complexOptions[n];
                        complexOptions[n] = value;
                    }
                    int tempararyCounter = 0;
                    foreach (var item in complexOptions)
                    {
                        if (tempararyCounter != 0)
                        {
                            options += ", ";
                        }
                        options += "\"" + item.itemText + "\"";
                        tempararyCounter++;
                    }
                    for (int y = 0; y < question.Value.questionItems.Count; y++)
                    {
                        //create item drops in same loop
                        itemDrops += "'" + question.Value.questionItems[y].itemContainerLabel + "'";

                        //if not the last item, place a comma to separate the items
                        if (y != question.Value.questionItems.Count - 1)
                        {
                            itemDrops += ", ";
                        }

                        answers += "answers[" + y + "] = [";

                        for (int r = 0; r < question.Value.questionItems[y].items.Count; r++)
                        {
                            for (int q = 0; q < complexOptions.Count; q++)
                            {
                                //Compare each correct item to all items in the list to match the indexes
                                if (complexOptions[q] == question.Value.questionItems[y].items[r])
                                {
                                    answers += "[options[" + complexOptions.IndexOf(complexOptions[q]) + "]]";

                                    if (r != (question.Value.questionItems[y].items.Count - 1))
                                    {
                                        answers += ", ";
                                    }
                                    else
                                    {
                                        answers += "];";
                                    }
                                }
                            }
                        }
                    }
                    if (i == game.Count)
                    {
                        nextQuestion = -1;
                    }
                    maxScore = complexOptions.Count;
                    documentContent.Append("var options = [" + options + "];");
                    documentContent.Append("var itemDrops = [" + itemDrops + "];");
                    documentContent.Append("var answers = new Array(2);");
                    documentContent.Append(answers);
                    documentContent.Append("question = {nextQIndex: " + nextQuestion + ", question: '" + question.Value.questionText + "', answer: answers, options: options, itemDrops: itemDrops};");

                    documentContent.Append("if (currentGameSession.scoreWeights.length >= question.nextQIndex){");
                    documentContent.Append("currentGameSession.score[question.nextQIndex] =0;");
                    documentContent.Append("currentGameSession.scoreWeights[question.nextQIndex] =" + question.Value.weight + ";");
                    documentContent.Append("currentGameSession.maxScore[question.nextQIndex] =" + question.Value.weight + ";}");
                    documentContent.Append("else {");
                    documentContent.Append("currentGameSession.score.push(0);");
                    documentContent.Append("currentGameSession.scoreWeights.push(" + question.Value.weight + ");");
                    documentContent.Append("currentGameSession.maxScore.push(" + question.Value.weight + ");");
                    documentContent.Append("}");

                    documentContent.Append("dndStart();}");
                }
                i++;
            }
            documentContent.Append("window.onload = function startupConfiguration() {currentGameSession.questionArray = [" + questionArray + "];setMaxScore(" + maxScore + ", false);headerSetup('" + gameN + "', " + multiQ.ToString().ToLower() + ");q1();}");
            documentContent.Append("</script>");
            documentContent.Append("<title id=\"title\">Questions</title>");
            documentContent.Append("</head>");
            documentContent.Append("<body>");
            documentContent.Append("<header id=\"header\">");
            documentContent.Append("<h1 id=\"gameName\"></h1>");
            documentContent.Append("<p id=\"score\"></p>");
            documentContent.Append("</header>");
            documentContent.Append("<span id=\"spacing\"></span>");
            documentContent.Append("<section id=\"questionArea\" class=\"slideDown\">");
            documentContent.Append("<header id=\"question\" class=\"question\"></header>");
            documentContent.Append("<div id=\"options\"></div>");
            documentContent.Append("</section>");
            documentContent.Append("</body>");
            documentContent.Append("</html>");
            using (var w = new StreamWriter(path))
            {
                w.Write(documentContent);
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            saveAllTheThings();
        }

        private void btnLoad_Click(object sender, EventArgs e)
        {
            loadAllTheThings();
        }
    }
}

