using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;
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
            questionTextBox.MaxLength = 100;

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

        /// <summary>
        /// newQuestion creates a new panel object in pnlQuestions. This new panel contains 4 children.
        /// questionNum: The position in the list of questions the current question is.
        /// questionQuestion: The text of the question being asked.
        /// questionType: The game type of the question (Multiple choice, True or False, Drag & Drop, ect).
        /// deleteQuestion: A button to delete the current question.
        /// </summary>
        private void newQuestion(bool loading)
        {
            if (questionNumber != 99 || questionNumber > 99)
            {
                Panel question = new Panel();
                question.Size = new Size(180, 60);
                question.Name = questionNumber.ToString();
                question.Tag = questionNumber;
                question.Click += selectQuestion;

                Label questionNum = new Label();
                Label questionQuestion = new Label();
                Label questionType = new Label();
                Button deleteQuestion = new Button();

                //Configure questionNum
                questionNum.Name = "QuestionNumber";
                questionNum.Location = new Point(145, 40);
                questionNum.Text = "#" + questionNumber;
                questionNum.Font = new Font("Microsoft Sans Serif", 12, FontStyle.Bold);
                questionNum.Click += selectQuestion;

                //Configure questionType
                questionType.Name = "QuestionType";
                questionType.Location = new Point(3, 32);
                questionType.Click += selectQuestion;

                //Configure questionQuestion
                questionQuestion.Text = "Click to edit";
                questionQuestion.Name = "Question";
                questionQuestion.Location = new Point(3, 7);
                questionQuestion.Font = new Font("Microsoft Sans Serif", 9);
                questionQuestion.Click += selectQuestion;

                //Configure deleteQuestion
                deleteQuestion.Name = "deleteQuestion";
                deleteQuestion.Text = "X";
                deleteQuestion.Location = new Point(158, 2);
                deleteQuestion.Size = new Size(20, 20);
                deleteQuestion.FlatStyle = FlatStyle.Flat;
                deleteQuestion.FlatAppearance.BorderSize = 0;
                deleteQuestion.FlatAppearance.BorderColor = System.Drawing.Color.Red;
                deleteQuestion.BackColor = System.Drawing.Color.Black;
                deleteQuestion.ForeColor = System.Drawing.Color.White;
                deleteQuestion.Click += deleteAQuestion;

                //Do not add to the game array if loading from save file.
                if (loading == false)
                {

                    ///Place an empty object in the list to act as a placeholder for this question.
                    ///The list index and the question index should remain the same (eg. Question 1 has an index of 0,
                    ///so in the game list, you can get its details at index 0.
                    ///View deleteAQuestion for more info.
                    game.Add(questionNumber, new questionObjectBase());

                }

                //Bind to panel
                question.Controls.Add(questionNum);
                question.Controls.Add(questionQuestion);
                question.Controls.Add(questionType);
                question.Controls.Add(deleteQuestion);
                pnlQuestions.Controls.Add(question);
                questionNumber++;
                createQuestionEdit();
                selectQuestion(question, null);
            }
            else
            {
                MessageBox.Show("Maximum questions reached");
            }
        }

        private void selectQuestion(object sender, EventArgs e)
        {
            if (sender is Panel)
            {
                //First Question created does not have a previous object to revert
                if (lastSelected == null)
                {
                    lastSelected = (Panel)sender;
                    lastSelected.BackColor = System.Drawing.Color.White;
                }
                //Takes previous question and deselects it, sets new question as the lastSelected variable, then displays the question as selected to user.
                else
                {
                    lastSelected.BackColor = System.Drawing.Color.DarkGray;
                    lastSelected = (Panel)sender;
                    lastSelected.BackColor = System.Drawing.Color.White;
                }
            }
            else
            {
                try
                {
                    //Making sure that the children of the question panel will still change the question, so you do not have to click a specific spot on the panel.
                    lastSelected.BackColor = System.Drawing.Color.DarkGray;
                    lastSelected = ((Panel)((Label)sender).Parent);
                    lastSelected.BackColor = System.Drawing.Color.White;
                }
                catch (Exception)
                {
                    errorHandle("Failed to select the chosen question, please try again.");
                }
            }
            loadPnlQuestionDetails();
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

        private void newItemSimple()
        {
            //Creates a panel that will be used to allow users to add options to a multi choice question
            questionObjectSimple question = game[(int)lastSelected.Tag] as questionObjectSimple;

            itemCount++;
            Panel pnlItem = new Panel();
            pnlItem.Name = itemCount.ToString();
            pnlItem.Size = new Size(320, 46);
            pnlItem.BackColor = System.Drawing.Color.DarkGray;

            //User can enter the items text
            Label itemNum = new Label();
            itemNum.Location = new Point(4, 10);
            itemNum.Text = "#" + itemCount.ToString();
            itemNum.Name = "itemNum";
            itemNum.Size = new Size(29, 13);
            TextBox itemTextBox = new TextBox();
            itemTextBox.Name = "itemText";
            itemTextBox.Location = new Point(33, 7);
            itemTextBox.Size = new Size(253, 20);
            itemTextBox.MaxLength = 100;

            //Configure deleteItem
            Button deleteItem = new Button();
            deleteItem.Name = "deleteItem";
            deleteItem.Text = "X";
            deleteItem.Location = new Point(292, 7);
            deleteItem.Size = new Size(20, 20);
            deleteItem.FlatStyle = FlatStyle.Flat;
            deleteItem.FlatAppearance.BorderSize = 0;
            deleteItem.FlatAppearance.BorderColor = System.Drawing.Color.Red;
            deleteItem.BackColor = System.Drawing.Color.Black;
            deleteItem.ForeColor = System.Drawing.Color.White;
            deleteItem.Click += deleteASimpleItem;

            CheckBox correct = new CheckBox();
            correct.Name = "correct";
            correct.Text = "Correct";
            correct.Location = new Point(8, 27);
            correct.Size = new Size(60, 17);

            CheckBox popup = new CheckBox();
            popup.Name = "popup";
            popup.Text = "Popups";
            popup.Location = new Point(74, 27);
            popup.Size = new Size(60, 17);
            popup.Click += popup_Click;

            if (question != null)
            {
                int index = itemCount - 1;

                question.questionItems.Add(new questionItemSimple());


                if (question.questionItems[index] != null)
                {
                    if (question.questionItems[index].correct == true)
                    {
                        correct.Checked = true;
                    }

                    if (question.questionItems[index].popups == null)
                    {
                        question.questionItems[index].popups = new popups();
                    }
                    if (question.questionItems[index].popups.popupEnabled == true)
                    {
                        popUpGenerate(null, popup);
                    }
                }
                else
                {
                    errorHandle("Item could not be added. Please try again.");
                }
            }

            pnlItem.Controls.Add(itemNum);
            pnlItem.Controls.Add(itemTextBox);
            pnlItem.Controls.Add(deleteItem);
            pnlItem.Controls.Add(correct);
            pnlItem.Controls.Add(popup);
            //add the new item box to the main panel
            pnlQuestionDetails.Controls.Add(pnlItem);
        }
        //DnD box
        private void newItemComplex()
        {
            //Creates a panel for the user to enter a DND container
            questionObjectComplex question = game[(int)lastSelected.Tag] as questionObjectComplex;

            if (question != null)
            {
                if (question.questionItems == null)
                {
                    question.questionItems = new List<itemContainer>();
                }
                question.questionItems.Add(new itemContainer());
                itemContainerCount++;
            }
            FlowLayoutPanel pnlItemContainer = new FlowLayoutPanel();
            pnlItemContainer.Name = itemContainerCount.ToString();
            pnlItemContainer.Size = new Size(320, 36);
            pnlItemContainer.AutoSize = true;
            pnlItemContainer.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            pnlItemContainer.BackColor = System.Drawing.Color.DimGray;

            //User can enter the items text
            Label itemContainerNum = new Label();
            itemContainerNum.Location = new Point(4, 10);
            itemContainerNum.Text = "#" + itemContainerCount.ToString();
            itemContainerNum.Name = "Box" + itemContainerCount.ToString();
            itemContainerNum.Size = new Size(29, 13);
            TextBox itemContainerTextBox = new TextBox();
            itemContainerTextBox.Name = "itemText";
            itemContainerTextBox.Location = new Point(33, 7);
            itemContainerTextBox.Size = new Size(177, 20);
            itemContainerTextBox.MaxLength = 100;

            //Configure addItemContainer
            Button addItem = new Button();
            addItem.Name = "addItem";
            addItem.Text = "Add Item";
            addItem.Location = new Point(213, 7);
            addItem.Size = new Size(60, 20);
            addItem.FlatStyle = FlatStyle.Flat;
            addItem.FlatAppearance.BorderSize = 0;
            addItem.FlatAppearance.BorderColor = System.Drawing.Color.Red;
            addItem.BackColor = System.Drawing.Color.Black;
            addItem.ForeColor = System.Drawing.Color.White;
            addItem.Click += addItem_Click;

            //Configure deleteItem
            Button deleteItemContainer = new Button();
            deleteItemContainer.Name = "deleteItem";
            deleteItemContainer.Text = "X";
            deleteItemContainer.Location = new Point(292, 7);
            deleteItemContainer.Size = new Size(20, 20);
            deleteItemContainer.FlatStyle = FlatStyle.Flat;
            deleteItemContainer.FlatAppearance.BorderSize = 0;
            deleteItemContainer.FlatAppearance.BorderColor = System.Drawing.Color.Red;
            deleteItemContainer.BackColor = System.Drawing.Color.Black;
            deleteItemContainer.ForeColor = System.Drawing.Color.White;
            deleteItemContainer.Click += deleteItemContainer_Click;

            pnlItemContainer.Controls.Add(itemContainerNum);
            pnlItemContainer.Controls.Add(itemContainerTextBox);
            pnlItemContainer.Controls.Add(addItem);
            pnlItemContainer.Controls.Add(deleteItemContainer);
            pnlQuestionDetails.Controls.Add(pnlItemContainer);
        }
        //DnD item
        void addItem_Click(object sender, EventArgs e)
        {
            //Creates a panel for the user to enter items into a DND container. WILL SPAWN IN THE SELECTED CONTAINER
            questionObjectComplex question = game[(int)lastSelected.Tag] as questionObjectComplex;

            Panel pnlItem = new Panel();
            pnlItem.Name = itemCount.ToString();
            pnlItem.Size = new Size(315, 36);
            pnlItem.BackColor = System.Drawing.Color.DarkGray;

            //User can enter the items text
            Label itemNum = new Label();
            itemNum.Location = new Point(4, 10);
            itemNum.Text = "#" + (itemCount + 1).ToString();
            itemNum.Name = "itemNum";
            itemNum.Size = new Size(29, 13);
            TextBox itemTextBox = new TextBox();
            itemTextBox.Name = "itemBaseText";
            itemTextBox.Location = new Point(33, 7);
            itemTextBox.Size = new Size(253, 20);
            itemTextBox.MaxLength = 100;

            //Configure deleteItem
            Button deleteItem = new Button();
            deleteItem.Name = "deleteItem";
            deleteItem.Text = "X";
            deleteItem.Location = new Point(292, 7);
            deleteItem.Size = new Size(20, 20);
            deleteItem.FlatStyle = FlatStyle.Flat;
            deleteItem.FlatAppearance.BorderSize = 0;
            deleteItem.FlatAppearance.BorderColor = System.Drawing.Color.Red;
            deleteItem.BackColor = System.Drawing.Color.Black;
            deleteItem.ForeColor = System.Drawing.Color.White;
            deleteItem.Click += deleteABaseItem;
            itemCount++;

            if (question != null)
            {
                int box = int.Parse(((Button)sender).Parent.Name);
                //If the list of items doesn't exist, create it
                if (question.questionItems[box - 1].items == null)
                {
                    question.questionItems[box - 1].items = new List<questionItemBase>();
                }
                question.questionItems[box - 1].items.Add(new questionItemBase());
                game[(int)lastSelected.Tag] = question;
            }

            var panel = ((Button)sender).Parent;
            pnlItem.Controls.Add(itemNum);
            pnlItem.Controls.Add(itemTextBox);
            pnlItem.Controls.Add(deleteItem);
            panel.Controls.Add(pnlItem);
        }

        private void popUpGenerate(popups sender, object sender2)
        {
            //Generates the popup controls on a simpleItem panel when the popup checkbox is checked
            //If the box is unchecked, remove the popup controls
            popups popupObj = sender;
            CheckBox popup = (CheckBox)sender2;

            //Create popup elements
            Label popupTitle = new Label();
            popupTitle.Location = new Point(4, 53);
            popupTitle.Text = "Popup Title:";
            popupTitle.Size = new Size(64, 13);
            popupTitle.Name = "popupTitle";
            TextBox popupTitleTextBox = new TextBox();
            if (popupObj != null)
            {
                popupTitleTextBox.Text = popupObj.popupTitle;
            }
            else
            {
                popupTitleTextBox.Text = "";
            }
            popupTitleTextBox.Location = new Point(71, 50);
            popupTitleTextBox.Size = new Size(215, 20);
            popupTitleTextBox.MaxLength = 100;

            Label popupBody = new Label();
            popupBody.Location = new Point(4, 76);
            popupBody.Text = "Popup Body:";
            popupBody.Size = new Size(64, 13);
            popupBody.Name = "popupBody";
            TextBox popupBodyTextBox = new TextBox();
            popupBodyTextBox.Name = "popupBodyTextBox";
            if (popupObj != null)
            {
                popupBodyTextBox.Text = popupObj.popupBody;
            }
            popupBodyTextBox.Location = new Point(71, 73);
            popupBodyTextBox.Size = new Size(215, 20);
            popupBodyTextBox.MaxLength = 100;

            if (popup.Enabled == true)
            {
                popup.Parent.Controls.Add(popupTitle);
                popup.Parent.Controls.Add(popupTitleTextBox);
                popup.Parent.Controls.Add(popupBody);
                popup.Parent.Controls.Add(popupBodyTextBox);
                popup.Parent.Size = new Size(320, 96);
            }
            else
            {
                popup.Parent.Controls.Remove(popupTitle);
                popup.Parent.Controls.Remove(popupTitleTextBox);
                popup.Parent.Controls.Remove(popupBody);
                popup.Parent.Controls.Remove(popupBodyTextBox);
                popup.Parent.Size = new Size(320, 46);
            }
        }
        private void popup_Click(object sender, EventArgs e)
        {
            popUpGenerate(null, sender);
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
                }
                else
                {
                    //If user has not selected a type, throw error.
                    if (type.Text == "" || type.Text == null)
                    {
                        errorHandle("Please select a question type before continuing.");
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
                            errorHandle("Invalid question type, please select one from the drop down menu.");
                            return;
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

        //Insert information entered in the pnlQuestionDetails into the question array when it is a questionObjectSimple
        private void applyChangesToQuesitonSimple(questionObjectSimple question, bool enableCorrectCheck)
        {
            question.questionText = pnlQuestionDetails.Controls[0].Controls[1].Text;
            question.weight = int.Parse(pnlQuestionDetails.Controls[0].Controls[5].Text);
            itemCount = pnlQuestionDetails.Controls.Count - 1;

            //make sure there is at least one correct answer
            bool hasCorrect = false;

            for (int i = 0; i < itemCount; i++)
            {
                //Grab the item from the list of items
                Panel c = (Panel)pnlQuestionDetails.Controls[i + 1];
                if (c.Name != "questionEdit")
                {
                    question.questionItems[i].itemText = c.Controls[1].Text;
                    question.questionItems[i].correct = ((CheckBox)c.Controls.Find("correct", true)[0]).Checked;
                    if (((CheckBox)c.Controls.Find("correct", true)[0]).Checked == true)
                    {
                        hasCorrect = true;
                    }
                    question.questionItems[i].popups.popupEnabled = ((CheckBox)c.Controls.Find("popup", true)[0]).Checked;
                    //If the popup checkbox is false the other controls do not exist
                    if (((CheckBox)c.Controls.Find("popup", true)[0]).Checked)
                    {
                        question.questionItems[i].popups.popupTitle = c.Controls[6].Text;
                        question.questionItems[i].popups.popupBody = c.Controls[8].Text;
                    }
                    else
                    {
                        question.questionItems[i].popups.popupTitle = "";
                        question.questionItems[i].popups.popupBody = "";
                    }
                }
            }
            if (enableCorrectCheck == true)
            {
                if (hasCorrect == false)
                {
                    errorHandle("Please make sure at least one option is marked as correct.");
                }
            }
            //apply the changes to the question array
            game[(int)lastSelected.Tag] = question;
        }

        //Insert information entered in the pnlQuestionDetails into the question array when it is a questionObjectComplex
        private void applyChangesToQuesitonComplex(questionObjectComplex question)
        {
            question = new questionObjectComplex();
            question.questionText = pnlQuestionDetails.Controls.Find("questionEdit", true)[0].Controls.Find("questionText", true)[0].Text;
            question.questionType = questionEdit.GetChildAtPoint(new Point(68, 29)).Text;
            question.questionNumber = pnlQuestionDetails.Controls.Find("questionEdit", true)[0].Controls.Find("questionNum", true)[0].Text;
            question.weight = int.Parse(pnlQuestionDetails.Controls.Find("questionEdit", true)[0].Controls.Find("weight", true)[0].Text);

            question.questionItems = new List<itemContainer>();
            int panelNumber = 0;
            foreach (Panel c in pnlQuestionDetails.Controls)
            {
                if (c.Name != "questionEdit")
                {
                    //Go through container panels and create new container based off the panel name (is number)
                    itemContainer tempContainer = new itemContainer();
                    tempContainer.itemContainerLabel = c.Controls[1].Text;
                    tempContainer.items = new List<questionItemBase>();
                    question.questionItems.Add(tempContainer);
                    //Go through each item panel, then add item to corresponding container object.                    
                    for (int t = 0; t < (c.Controls.Count - 4); t++)
                    {
                        var item = c.Controls[t + 4];
                        questionItemBase tempItem = new questionItemBase();
                        tempItem.itemText = item.Controls[1].Text;
                        question.questionItems[panelNumber - 1].items.Add(tempItem);
                    }
                }
                panelNumber++;
            }
            game[(int)lastSelected.Tag] = question;
        }

        /// <summary>
        /// Will delete a question from both the pnlQuestions Panel AND the game list.
        /// You will see that there is only one line of code for the game list but there is much more code for the Panels.
        /// The reasoning behind this is that in order for the numbering of the questions to be accurate, The code has to run through each Panel to update.
        /// Logically the array should also be reorganized because they have to stay in sync. The game list is a list not an array, so when the index is removed, it automatically
        /// moves the indexes around. Example: Remove 3 from (1,2,3,4) ends up being (1,2,4) which changes to (1,2,3). Since the auto select of questions is built to send the user
        /// to the question previous, this should keep them both in sync with minimal effort.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void deleteAQuestion(object sender, EventArgs e)
        {
            //Checks to make sure the user cannot delete if there is only one question in the list
            if (questionNumber != 2)
            {
                //Verify the user wants to remove the question, as working on a 30 item Drag and drop with accidental deletion would be painful.
                DialogResult = MessageBox.Show("Are you sure you wish to delete this question?", "Delete Question:", MessageBoxButtons.YesNo);
                if (DialogResult == DialogResult.Yes)
                {
                    var question = ((Button)sender).Parent;
                    //If the question is the selected question, change selected question to the question above.
                    if (question == lastSelected)
                    {
                        if ((int)question.Tag == 1)
                        {
                            selectQuestion(pnlQuestions.Controls[(int)question.Tag - 1], null);
                        }
                        else
                        {
                            selectQuestion(pnlQuestions.Controls[(int)question.Tag - 1], null);
                        }
                    }
                    //Remove question from lists then destroy it from memory.
                    game.Remove((int)question.Tag);
                    pnlQuestions.Controls.Remove(question);
                    question.Dispose();
                    //Set the question count back to one, then go through all panels in the list and renumber them.
                    //Reason for resetting questionNumber is because otherwise if you have 1,2,3 and delete #2 you do not end up with 1,3,3.
                    questionNumber = 1;

                    //Creates new dictionary to temporarily store the dictionary as I reset the keys
                    Dictionary<int, dynamic> tempGameReplacer = new Dictionary<int, dynamic>();
                    foreach (Panel c in pnlQuestions.Controls)
                    {
                        c.Tag = questionNumber;
                        //Moves contents into new dictionary
                        try
                        {
                            //if the question removed was the first one, have to increment all movement by 1 since the key '1' is not availible
                            tempGameReplacer.Add(questionNumber, game[questionNumber + 1]);
                        }
                        catch
                        {
                            tempGameReplacer.Add(questionNumber, game[questionNumber]);
                        }
                        var questionNumLabel = c.Controls.Find("QuestionNumber", true);
                        questionNumLabel[0] = (Label)questionNumLabel[0];
                        questionNumLabel[0].Text = "#" + questionNumber;
                        questionNumber++;
                    }
                    game = tempGameReplacer;
                }
            }
            else
            {
                //Block users from deleting all questions, I am not wanting blank generated games!
                errorHandle("Games must contain at least one question! \nAdd another question to delete this one.");
            }

        }

        void deleteItemContainer_Click(object sender, EventArgs e)
        {
            //Verify the user wants to remove the item
            DialogResult = MessageBox.Show("Are you sure you wish to delete this group?", "Delete group:", MessageBoxButtons.YesNo);
            if (DialogResult == DialogResult.Yes)
            {
                var itemContainer = ((Button)sender).Parent;
                // var itemsRemoved = itemContainer.Controls.Count();
                //Remove item from lists then destroy it from memory.
                pnlQuestions.Controls.Remove(itemContainer);
                itemContainer.Dispose();
                //Set the item container count back to 0, then go through all panels in the list and renumber them.
                //Reason for resetting itemContainerCount is because otherwise if you have 1,2,3 and delete #2 you do not end up with 1,3,3.
                itemContainerCount = 0;

                //Flag for if an error occurs
                bool unexpectedFailure = false;
                int tempItemCount = itemCount;
                //i contains the count for the item renumbering
                int i = 0;

                //Renumbers the containers now that one is removed as well as the sub items
                foreach (Panel c in pnlQuestionDetails.Controls)
                {
                    if (c.Name != "questionEdit")
                    {
                        var itemCNum = c.Controls[0];
                        itemCNum.Text = "#" + (itemContainerCount + 1);
                        itemCNum.Name = "#" + (itemContainerCount + 1);
                        itemContainerCount++;
                        itemCount += c.Controls.Count - 4;

                        var itemNum = c.Controls.Find("itemNum", true);
                        try
                        {
                            for (int y = 0; y < (c.Controls.Count - 4); y++)
                            {
                                itemNum[y].Text = "#" + (i + 1);
                                i++;
                            }
                        }
                        catch (Exception)
                        {
                            unexpectedFailure = true;
                        }
                        tempItemCount = i;
                    }
                }

                if (unexpectedFailure == true)
                {
                    errorHandle("Could not delete the item correctly.");
                }
                else
                {
                    applyChangesToQuesitonComplex((questionObjectComplex)game[(int)lastSelected.Tag]);
                }
                itemCount = tempItemCount;
            }
        }

        private void deleteABaseItem(object sender, EventArgs e)
        {
            //Verify the user wants to remove the item
            DialogResult = MessageBox.Show("Are you sure you wish to delete this item?", "Delete item:", MessageBoxButtons.YesNo);
            if (DialogResult == DialogResult.Yes)
            {
                var item = ((Button)sender).Parent;
                //Flag for if an error occurs
                bool unexpectedFailure = false;
                int tempItemCount = itemCount;
                //i contains the count for the item renumbering
                int i = 0;


                //((Button)sender).Parent.Parent.Controls.Remove(item);

                foreach (Panel c in pnlQuestionDetails.Controls)
                {
                    if (c.Name != "questionEdit")
                    {
                        var itemNum = c.Controls.Find("itemNum", true);
                        try
                        {
                            for (int y = 0; y < (c.Controls.Count - 4); y++)
                            {
                                itemNum[y].Text = "#" + (i + 1);
                                i++;
                            }
                        }
                        catch (Exception)
                        {
                            unexpectedFailure = true;
                        }
                        tempItemCount = i;
                    }
                }

                if (unexpectedFailure == true)
                {
                    errorHandle("Could not delete the item, please try again.");
                }
                else
                {
                    //questionObjectComplex question = game[(int)lastSelected.Tag] as questionObjectComplex;
                    //question.questionItems[i].items.Remove(item);
                    ((Button)sender).Parent.Dispose();
                    item.Dispose();
                    applyChangesToQuesitonComplex((questionObjectComplex)game[(int)lastSelected.Tag]);
                }
                itemCount = tempItemCount;
            }
        }

        private void deleteASimpleItem(object sender, EventArgs e)
        {
            //Verify the user wants to remove the item
            DialogResult = MessageBox.Show("Are you sure you wish to delete this item?", "Delete item:", MessageBoxButtons.YesNo);
            if (DialogResult == DialogResult.Yes)
            {
                var item = ((Button)sender).Parent;
                //Remove item from lists then destroy it from memory.
                ((Button)sender).Parent.Parent.Controls.Remove(item);

                //Set the item count back to 0, then go through all panels in the list and renumber them.
                //Reason for resetting itemCount is because otherwise if you have 1,2,3 and delete #2 you do not end up with 1,3,3.
                itemCount = 0;
                foreach (Panel c in pnlQuestionDetails.Controls)
                {
                    if (c.Name != "questionEdit")
                    {
                        c.Controls.Find("itemNum", true)[0].Text = "#" + (itemCount + 1);
                        c.Name = itemCount.ToString();
                        itemCount++;
                    }
                }
                //questionObjectSimple question = game[(int)lastSelected.Tag] as questionObjectSimple;
                //question.questionItems.RemoveAt(int.Parse(item.Name));
                item.Dispose();
                applyChangesToQuesitonSimple((questionObjectSimple)game[(int)lastSelected.Tag], false);
            }
        }

        private void loadQuestion()
        {
            var question = game[(int)lastSelected.Tag];
            itemCount = 0;
            itemContainerCount = 0;

            if (question is questionObjectSimple)
            {
                loadSimpleQuestion();
            }
            if (question is questionObjectComplex)
            {
                loadComplexQuestion();
            }
        }

        private void loadComplexQuestion()
        {
            questionObjectComplex question = game[(int)lastSelected.Tag];
            //Go through the game list, recreate the item boxes and their items in the ui for editting.
            for (int i = 0; i < question.questionItems.Count; i++)
            {
                itemContainerCount++;
                FlowLayoutPanel pnlItemContainer = new FlowLayoutPanel();
                pnlItemContainer.Name = itemContainerCount.ToString();
                pnlItemContainer.Size = new Size(320, 36);
                pnlItemContainer.AutoSize = true;
                pnlItemContainer.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
                pnlItemContainer.BackColor = System.Drawing.Color.DimGray;

                //User can enter the items text
                Label itemContainerNum = new Label();
                itemContainerNum.Location = new Point(4, 10);
                itemContainerNum.Text = "#" + itemContainerCount.ToString();
                itemContainerNum.Name = itemContainerCount.ToString();
                itemContainerNum.Size = new Size(29, 13);
                TextBox itemContainerTextBox = new TextBox();
                itemContainerTextBox.Name = "itemText";
                itemContainerTextBox.Text = question.questionItems[i].itemContainerLabel;
                itemContainerTextBox.Location = new Point(33, 7);
                itemContainerTextBox.Size = new Size(177, 20);
                itemContainerTextBox.MaxLength = 100;

                //Configure addItemContainer
                Button addItem = new Button();
                addItem.Name = "addItem";
                addItem.Text = "Add Item";
                addItem.Location = new Point(213, 7);
                addItem.Size = new Size(60, 20);
                addItem.FlatStyle = FlatStyle.Flat;
                addItem.FlatAppearance.BorderSize = 0;
                addItem.FlatAppearance.BorderColor = System.Drawing.Color.Red;
                addItem.BackColor = System.Drawing.Color.Black;
                addItem.ForeColor = System.Drawing.Color.White;
                addItem.Click += addItem_Click;

                //Configure deleteItem
                Button deleteItemContainer = new Button();
                deleteItemContainer.Name = "deleteItem";
                deleteItemContainer.Text = "X";
                deleteItemContainer.Location = new Point(292, 7);
                deleteItemContainer.Size = new Size(20, 20);
                deleteItemContainer.FlatStyle = FlatStyle.Flat;
                deleteItemContainer.FlatAppearance.BorderSize = 0;
                deleteItemContainer.FlatAppearance.BorderColor = System.Drawing.Color.Red;
                deleteItemContainer.BackColor = System.Drawing.Color.Black;
                deleteItemContainer.ForeColor = System.Drawing.Color.White;
                deleteItemContainer.Click += deleteItemContainer_Click;

                pnlItemContainer.Controls.Add(itemContainerNum);
                pnlItemContainer.Controls.Add(itemContainerTextBox);
                pnlItemContainer.Controls.Add(addItem);
                pnlItemContainer.Controls.Add(deleteItemContainer);
                pnlQuestionDetails.Controls.Add(pnlItemContainer);

                for (int y = 0; y < question.questionItems[i].items.Count; y++)
                {
                    itemCount++;
                    Panel pnlItem = new Panel();
                    pnlItem.Name = itemCount.ToString();
                    pnlItem.Size = new Size(315, 36);
                    pnlItem.BackColor = System.Drawing.Color.DarkGray;

                    //User can enter the items text
                    Label itemNum = new Label();
                    itemNum.Location = new Point(4, 10);
                    itemNum.Text = "#" + (itemCount).ToString();
                    itemNum.Name = "itemNum";
                    itemNum.Size = new Size(29, 13);
                    TextBox itemTextBox = new TextBox();
                    itemTextBox.Name = "itemBaseText";
                    itemTextBox.Location = new Point(33, 7);
                    itemTextBox.Size = new Size(253, 20);
                    itemTextBox.MaxLength = 100;
                    itemTextBox.Text = question.questionItems[i].items[y].itemText;

                    //Configure deleteItem
                    Button deleteItem = new Button();
                    deleteItem.Name = "deleteItem";
                    deleteItem.Text = "X";
                    deleteItem.Location = new Point(292, 7);
                    deleteItem.Size = new Size(20, 20);
                    deleteItem.FlatStyle = FlatStyle.Flat;
                    deleteItem.FlatAppearance.BorderSize = 0;
                    deleteItem.FlatAppearance.BorderColor = System.Drawing.Color.Red;
                    deleteItem.BackColor = System.Drawing.Color.Black;
                    deleteItem.ForeColor = System.Drawing.Color.White;
                    deleteItem.Click += deleteABaseItem;

                    pnlItem.Controls.Add(itemNum);
                    pnlItem.Controls.Add(itemTextBox);
                    pnlItem.Controls.Add(deleteItem);
                    pnlItemContainer.Controls.Add(pnlItem);
                }
            }
        }

        private void loadSimpleQuestion()
        {
            questionObjectSimple question = game[(int)lastSelected.Tag] as questionObjectSimple;
            //Go through the game list, recreate the item boxes and their items in the ui for editting.
            var tempCount = question.questionItems.Count;
            for (int i = 0; i < tempCount; i++)
            {
                //itemCount++;
                Panel pnlItem = new Panel();
                pnlItem.Name = itemCount.ToString();
                pnlItem.Size = new Size(320, 46);
                pnlItem.BackColor = System.Drawing.Color.DarkGray;

                //User can enter the items text
                Label itemNum = new Label();
                itemNum.Location = new Point(4, 10);
                itemNum.Text = "#" + (i + 1);
                itemNum.Name = "itemNum";
                itemNum.Size = new Size(29, 13);
                TextBox itemTextBox = new TextBox();
                itemTextBox.Name = "itemText";
                itemTextBox.Location = new Point(33, 7);
                itemTextBox.Size = new Size(253, 20);
                itemTextBox.MaxLength = 100;
                itemTextBox.Text = question.questionItems[i].itemText;

                //Configure deleteItem
                Button deleteItem = new Button();
                deleteItem.Name = "deleteItem";
                deleteItem.Text = "X";
                deleteItem.Location = new Point(292, 7);
                deleteItem.Size = new Size(20, 20);
                deleteItem.FlatStyle = FlatStyle.Flat;
                deleteItem.FlatAppearance.BorderSize = 0;
                deleteItem.FlatAppearance.BorderColor = System.Drawing.Color.Red;
                deleteItem.BackColor = System.Drawing.Color.Black;
                deleteItem.ForeColor = System.Drawing.Color.White;
                deleteItem.Click += deleteASimpleItem;

                CheckBox correct = new CheckBox();
                correct.Name = "correct";
                correct.Text = "Correct";
                correct.Location = new Point(8, 27);
                correct.Size = new Size(60, 17);

                CheckBox popup = new CheckBox();
                popup.Name = "popup";
                popup.Text = "Popups";
                popup.Location = new Point(74, 27);
                popup.Size = new Size(60, 17);
                popup.Click += popup_Click;

                if (question != null)
                {
                    if (question.questionItems[i] != null)
                    {
                        if (question.questionItems[i].correct == true)
                        {
                            correct.Checked = true;
                        }

                        if (question.questionItems[i].popups == null)
                        {
                            question.questionItems[i].popups = new popups();
                        }
                        if (question.questionItems[i].popups.popupEnabled == true)
                        {
                            popup.Checked = true;
                            pnlItem.Controls.Add(popup);

                            popups popupObj = new popups();
                            popupObj.popupEnabled = true;
                            popupObj.popupTitle = question.questionItems[i].popups.popupTitle;
                            popupObj.popupBody = question.questionItems[i].popups.popupBody;
                            popUpGenerate(popupObj, popup);
                        }
                    }
                    else
                    {
                        errorHandle("Item could not be added. Please try again.");
                    }
                }

                pnlItem.Controls.Add(itemNum);
                pnlItem.Controls.Add(itemTextBox);
                pnlItem.Controls.Add(deleteItem);
                pnlItem.Controls.Add(correct);
                pnlItem.Controls.Add(popup);
                pnlQuestionDetails.Controls.Add(pnlItem);
                itemCount = i;
            }
        }

        private void btnGameGenerate_Click(object sender, EventArgs e)
        {
            applyAll();
            string colour = null;
            string gameN = null;

            Form2 gameName = new Form2();

            //Show testDialog as a modal dialog and determine if DialogResult = OK. 
            if (gameName.ShowDialog(this) == DialogResult.OK)
            {
                //Read the contents of gameName's TextBox.               
                gameN = gameName.txtName.Text;
            }
            gameName.Dispose();

            string directory = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            string filename = "index.html";
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
                    for (int i = 0; i < question.Value.questionItems.Count; i++)
                    {
                        if (question.Value.questionItems[i].popups.popupEnabled == true)
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

            using (var w = new StreamWriter(path))
            {
                w.WriteLine("<!DOCTYPE html>");
                w.WriteLine("<html lang=\"en\">");
                w.WriteLine("<head>");
                w.WriteLine("<meta http-equiv=\"Content-Type\" content=\"text/html; charset=UTF-8\">");
                w.WriteLine("<link rel=\"stylesheet\" href=\"https://mylearningspace.wlu.ca/shared/root/tna.css\">");
                //if a question is a drag n drop then link the framework
                if (dnd == true)
                {
                    w.WriteLine("<link rel=\"stylesheet\" href=\"https://mylearningspace.wlu.ca/shared/root/dnd.css\">");
                    w.WriteLine("<script src=\"https://mylearningspace.wlu.ca/shared/root/dnf.js\" type=\"text/javascript\"></script>");
                }
                //if a question is a multiple choice then link the appropriate framework
                if (mc == true)
                {
                    w.WriteLine("<script src=\"https://mylearningspace.wlu.ca/shared/root/mpf.js\" type=\"text/javascript\"></script>");
                    if (popup == true)
                    {
                        w.WriteLine("<link rel=\"stylesheet\" href=\"https://mylearningspace.wlu.ca/shared/root/pop.css\">");
                    }
                }
                if (colour != null)
                {
                    w.WriteLine("<style>#header,.item,.option,.question{background:" + colour + "}.button{color:" + colour + "}</style>");
                }
                w.Write("<script>");
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

                int x = 0;
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
                    x++;
                    if (question.Value is questionObjectSimple)
                    {
                        w.Write("function q" + i + "() {");
                        w.Write("question = {};");
                        w.Write("currentGameSession.scoreWeights.push(" + question.Value.weight + ");");

                        for (int c = 0; c < question.Value.questionItems.Count; c++)
                        {
                            //Add correct items to the answers string
                            if (question.Value.questionItems[c].correct == true)
                            {
                                maxScore++;
                                if (answers == "")
                                {
                                    //o for options
                                    answers += "o[" + (x - 1) + "]";
                                }
                                else
                                {
                                    answers += ", o[" + (x - 1) + "]";
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
                        w.Write("currentGameSession.maxScore.push(" + maxScore + ");");
                        w.Write("var o = [" + options + "];");
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
                        w.Write("var popupItems = [" + popupItems + "];");
                        w.Write("var answer = " + answers + ";");
                        w.Write("enablePopups(" + popup.ToString().ToLower() + ");");
                        if (i == game.Count())
                        {
                            nextQuestion = -1;
                        }
                        w.Write("question = {nextQIndex: " + nextQuestion + ", question: '" + question.Value.questionText + "', answer: answer, options: o, popup: " + popup.ToString().ToLower() + ", popupItems: popupItems};");
                        w.Write("multiStart();");
                        w.Write("}");
                    }
                    else if (question.Value is questionObjectComplex)
                    {
                        options = "";
                        answers = "";
                        itemDrops = "";

                        w.Write("function q" + i + "() {");
                        w.Write("question = {};");
                        w.Write("currentGameSession.scoreWeights.push(" + question.Value.weight + ");");

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

                            answers += "\nanswers[" + y + "] = [";

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
                        w.Write("currentGameSession.maxScore.push(" + maxScore + ");");
                        w.Write("var options = [" + options + "];");
                        w.Write("var itemDrops = [" + itemDrops + "];");
                        w.Write("var answers = new Array(2);");
                        w.Write(answers);
                        w.Write("question = {nextQIndex: " + nextQuestion + ", question: '" + question.Value.questionText + "', answer: answers, options: options, itemDrops: itemDrops};");
                        w.Write("dndStart();}");
                    }
                    i++;
                }
                w.Write("window.onload = function startupConfiguration() {currentGameSession.questionArray = [" + questionArray + "];setMaxScore(" + maxScore + ", false);headerSetup('" + gameN + "', " + multiQ.ToString().ToLower() + ");q1();}");
                w.Write("</script>");
                w.WriteLine("<script src=\"https://mylearningspace.wlu.ca/shared/root/jsf.js\" type=\"text/javascript\"></script>");
                w.WriteLine("<title id=\"title\">Questions</title>");
                w.WriteLine("</head>");
                w.WriteLine("<body>");
                w.WriteLine("<header id=\"header\">");
                w.WriteLine("<h1 id=\"gameName\"></h1>");
                w.WriteLine("<p id=\"score\"></p>");
                w.WriteLine("</header>");
                w.WriteLine("<span id=\"spacing\"></span>");
                w.WriteLine("<section id=\"questionArea\" class=\"slideDown\">");
                w.WriteLine("<header id=\"question\" class=\"question\"></header>");
                w.WriteLine("<div id=\"options\"></div>");
                w.WriteLine("</section>");
                w.WriteLine("</body>");
                w.WriteLine("</html>");
            }
        }

        private void saveAllTheThings()
        {
            BinaryFormatter formatter = new BinaryFormatter();
            SaveFileDialog sfd1 = new SaveFileDialog();
            sfd1.Filter = "*Online Game File (*.ogf)|*.ogf";
            if (sfd1.ShowDialog() == DialogResult.OK)
            {
                string filePath = sfd1.FileName;
                try
                {
                    // Create a FileStream that will write data to file.
                    FileStream writerFileStream =
                        new FileStream(filePath, FileMode.Create, FileAccess.Write);
                    // Save our dictionary of friends to file
                    formatter.Serialize(writerFileStream, game);

                    // Close the writerFileStream when we are done.
                    writerFileStream.Close();
                }
                catch (Exception)
                {
                    Console.WriteLine("Unable to save our friends' information");
                } // end try-catch
            } // end public bool Load()

        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            saveAllTheThings();
        }

        private void loadAllTheThings()
        {
            pnlQuestions.Controls.Clear();
            //Credit to code goes to these glorious people http://www.techcoil.com/blog/how-to-save-and-load-objects-to-and-from-file-in-c/
            BinaryFormatter formatter = new BinaryFormatter();
            OpenFileDialog ofd1 = new OpenFileDialog();
            ofd1.InitialDirectory = Environment.SpecialFolder.Desktop.ToString();
            ofd1.Filter = "*Online Game File (*.ogf)|*.ogf";
            if (ofd1.ShowDialog() == DialogResult.OK)
            {
                string fileName = ofd1.FileName;

                //reset global variables for reuse.
                game = null;
                questionNumber = 1;

                try
                {
                    // Create a FileStream will gain read access to the 
                    // data file.
                    FileStream readerFileStream = new FileStream(fileName,
                        FileMode.Open, FileAccess.Read);
                    // Reconstruct information from file.
                    game = (Dictionary<int, dynamic>)
                    formatter.Deserialize(readerFileStream);
                    // Close the readerFileStream when we are done
                    readerFileStream.Close();

                }
                catch (Exception)
                {
                    errorHandle("Invalid or corrupt file selected.");
                } // end try-catch


                int questionCount = game.Count;
                //Go through all questions in the game and rebuild the ui from it.
                for (int i = 0; i < questionCount; i++)
                {
                    //create the question list
                    newQuestion(true);
                    //apply all the "changes" aka fill the ui in with the correct information
                    applyAll();
                }
            }
        }

        private void btnLoad_Click(object sender, EventArgs e)
        {
            loadAllTheThings();
        }
    }
}

