using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
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
            newQuestion();
        }

        //List to hold the questions
        List<questionObjectBase> game = new List<questionObjectBase>();
        //private Dictionary<int, object> extended;

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
            questionType.Items.Add("True or False");

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
        private void newQuestion()
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

                ///Place an empty object in the list to act as a placeholder for this question.
                ///The list index and the question index should remain the same (eg. Question 1 has an index of 0,
                ///so in the game list, you can get its details at index 0.
                ///View deleteAQuestion for more info.
                game.Add(new questionObjectBase());

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
            questionEdit.Controls.Find("questionText", true)[0].Text = game[(int)lastSelected.Tag - 1].questionText;
            questionEdit.Controls.Find("type", true)[0].Text = game[(int)lastSelected.Tag - 1].questionType;
            questionEdit.Controls.Find("weight", true)[0].Text = game[(int)lastSelected.Tag - 1].weight.ToString();

            //if the apply button doesn't exist, check if the question is "empty", if so then create apply button and enable the type combobox.
            if (questionEdit.Controls.Contains(applyQuestionInfo) == false)
            {
                questionEdit.Controls.Add(applyQuestionInfo);
            }
            //If apply button is there, check to see if the question needs it, if not, remove it.
            if (game[(int)lastSelected.Tag - 1].questionType == null || game[(int)lastSelected.Tag - 1].questionType == "")
            {
                questionEdit.Controls.Find("type", true)[0].Enabled = true;
            }
            if (game[(int)lastSelected.Tag - 1].questionType != null)
            {
                questionEdit.Controls.Find("type", true)[0].Enabled = false;
                var apply = questionEdit.GetChildAtPoint(new Point(245, 79));
                questionEdit.Controls.Remove(apply);
            }
            try
            {
                loadQuestions();
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
            newQuestion();
        }

        private void btnAddOption_Click(object sender, EventArgs e)
        {
            newItem();
        }

        private void newItem()
        {
            var question = game[(int)lastSelected.Tag - 1];
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
            questionObjectSimple question = game[(int)lastSelected.Tag - 1] as questionObjectSimple;

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
                itemTextBox.Text = question.questionText;

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
                        popUpGenerate(popup);
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
        }
        //DnD box
        private void newItemComplex()
        {
            questionObjectComplex question = game[(int)lastSelected.Tag - 1] as questionObjectComplex;

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
            questionObjectComplex question = game[(int)lastSelected.Tag - 1] as questionObjectComplex;

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
                game[(int)lastSelected.Tag - 1] = question;
            }

            var panel = ((Button)sender).Parent;
            pnlItem.Controls.Add(itemNum);
            pnlItem.Controls.Add(itemTextBox);
            pnlItem.Controls.Add(deleteItem);
            panel.Controls.Add(pnlItem);
        }

        private void popUpGenerate(object sender)
        {
            //Create popup elements
            Label popupTitle = new Label();
            popupTitle.Location = new Point(4, 53);
            popupTitle.Text = "Popup Title:";
            popupTitle.Size = new Size(64, 13);
            popupTitle.Name = "popupTitle";
            TextBox popupTitleTextBox = new TextBox();
            popupTitleTextBox.Name = "popupTitleTextBox";
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
            popupBodyTextBox.Location = new Point(71, 73);
            popupBodyTextBox.Size = new Size(215, 20);
            popupBodyTextBox.MaxLength = 100;

            CheckBox popup = (CheckBox)sender;
            if (popup.Checked == true)
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
            popUpGenerate(sender);
        }


        private void applyQuestionSettingsEvent(object sender, EventArgs e)
        {
            applyQuestionSettings();
        }

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
                        questionEdit.Controls.Remove(apply);
                        type.Enabled = false;

                        var weight = questionEdit.GetChildAtPoint(new Point(68, 53)).Text;

                        if (type.Text == "Multiple Choice" || type.Text == "True or False")
                        {
                            questionObjectSimple question = new questionObjectSimple();
                            question.questionText = text;
                            question.questionType = type.Text;
                            question.questionNumber = lastSelected.Controls.Find("QuestionNumber", true)[0].Text;
                            int.TryParse(weight, out question.weight);
                            game[(int)lastSelected.Tag - 1] = question;
                        }
                        else if (type.Text == "Drag & Drop")
                        {
                            questionObjectComplex question = new questionObjectComplex();
                            question.questionText = text;
                            question.questionType = type.Text;
                            question.questionNumber = lastSelected.Controls.Find("QuestionNumber", true)[0].Text;
                            int.TryParse(weight, out question.weight);
                            game[(int)lastSelected.Tag - 1] = question;
                        }
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
            var question = game[(int)lastSelected.Tag - 1];
            if (question is questionObjectSimple)
            {
                applyChangesToQuesitonSimple((questionObjectSimple)question);
            }
            else if (question is questionObjectComplex)
            {
                applyChangesToQuesitonComplex((questionObjectComplex)question);
            }
            applyQuestionSettings();
        }

        //Insert information entered in the pnlQuestionDetails into the question array when it is a questionObjectSimple
        private void applyChangesToQuesitonSimple(questionObjectSimple question)
        {
            question.questionText = pnlQuestionDetails.Controls.Find("questionEdit", true)[0].Controls.Find("questionText", true)[0].Text;
            question.questionNumber = pnlQuestionDetails.Controls.Find("questionEdit", true)[0].Name;
            question.weight = int.Parse(pnlQuestionDetails.Controls.Find("questionEdit", true)[0].Controls.Find("weight", true)[0].Text);
            for (int i = 0; i < itemCount; i++)
            {
                //Grab the item from the list of items
                Panel c = (Panel)pnlQuestionDetails.Controls[i + 1];
                if (c.Name != "questionEdit")
                {
                    question.questionItems[i].itemText = c.Controls.Find("itemText", true)[0].Text;
                    question.questionItems[i].correct = ((CheckBox)c.Controls.Find("correct", true)[0]).Checked;
                    question.questionItems[i].popups.popupEnabled = ((CheckBox)c.Controls.Find("popup", true)[0]).Checked;
                    //If the popup checkbox is false the other controls do not exist
                    if (((CheckBox)c.Controls.Find("popup", true)[0]).Checked)
                    {
                        question.questionItems[i].popups.popupTitle = c.Controls.Find("popupTitleTextBox", true)[0].Text;
                        question.questionItems[i].popups.popupBody = c.Controls.Find("popupBodyTextBox", true)[0].Text;
                    }
                    else
                    {
                        question.questionItems[i].popups.popupTitle = "";
                        question.questionItems[i].popups.popupBody = "";
                    }
                }
            }
            //apply the changes to the question array
            game[(int)lastSelected.Tag - 1] = question;
        }

        //Insert information entered in the pnlQuestionDetails into the question array when it is a questionObjectComplex
        private void applyChangesToQuesitonComplex(questionObjectComplex question)
        {
            question.questionText = pnlQuestionDetails.Controls.Find("questionEdit", true)[0].Controls.Find("questionText", true)[0].Text;
            question.questionNumber = pnlQuestionDetails.Controls.Find("questionEdit", true)[0].Controls.Find("questionNum", true)[0].Text;
            question.weight = int.Parse(pnlQuestionDetails.Controls.Find("questionEdit", true)[0].Controls.Find("weight", true)[0].Text);

            //Flag for if an error occurs
            bool emptyBox = false;
            foreach (Panel c in pnlQuestionDetails.Controls)
            {
                if (c.Name != "questionEdit")
                {
                    try
                    {
                            question.questionItems[int.Parse(c.Name) - 1].itemContainerLabel = c.Controls[1].Text;
                            //The number of items within a item box
                            int tempItemCount = c.Controls.Count - 4;
                            for (int i = 0; i < tempItemCount; i++)
                            {
                                var test = c.Controls[i + 4];
                                question.questionItems[int.Parse(c.Name) - 1].items[i].itemText = test.Controls[1].Text;
                            }
                    }
                    catch (Exception)
                    {
                        emptyBox = true;
                    }
                }
            }
            if (emptyBox == true)
            {
                errorHandle("One or More of the Drop boxes does not contain any items.\nPlease remove the box or add at least one item.");
            }
            game[(int)lastSelected.Tag - 1] = question;
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
                            selectQuestion(pnlQuestions.Controls[(int)question.Tag], null);
                        }
                        else
                        {
                            selectQuestion(pnlQuestions.Controls[(int)question.Tag - 2], null);
                        }
                    }
                    //Remove question from lists then destroy it from memory.
                    game.RemoveAt((int)question.Tag - 1);
                    pnlQuestions.Controls.Remove(question);
                    question.Dispose();
                    //Set the question count back to one, then go through all panels in the list and renumber them.
                    //Reason for resetting questionNumber is because otherwise if you have 1,2,3 and delete #2 you do not end up with 1,3,3.
                    questionNumber = 1;
                    foreach (Panel c in pnlQuestions.Controls)
                    {
                        c.Tag = questionNumber;
                        var questionNumLabel = c.Controls.Find("QuestionNumber", true);
                        questionNumLabel[0] = (Label)questionNumLabel[0];
                        questionNumLabel[0].Text = "#" + questionNumber;
                        questionNumber++;
                    }
                }
            }
            else
            {
                //Block users from deleting all questions, I am not wanting blank generated games!
                MessageBox.Show("Sorry, Games must contain at least one question! \nAdd another question to delete this one.");
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
                    errorHandle("Could not delete the item, please try again.");
                }
                else
                {
                    applyChangesToQuesitonComplex((questionObjectComplex)game[(int)lastSelected.Tag - 1]);
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


                ((Button)sender).Parent.Parent.Controls.Remove(item);

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
                    applyChangesToQuesitonComplex((questionObjectComplex)game[(int)lastSelected.Tag - 1]);
                }
                itemCount = tempItemCount;
                item.Dispose();
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
                questionObjectSimple question = game[(int)lastSelected.Tag - 1] as questionObjectSimple;
                question.questionItems.RemoveAt(int.Parse(item.Name) - 1);
                item.Dispose();
            }
        }

        private void loadQuestions()
        {
            foreach (var question in game)
            {
                itemCount = 0;
                itemContainerCount = 0;

                if (question is questionObjectSimple)
                {
                    newItemSimple();
                }
                else if (question is questionObjectComplex)
                {
                    loadComplexQuestion((questionObjectComplex)question);
                }
            }
        }

        private void loadComplexQuestion(questionObjectComplex question)
        {
            //Go through the game list, recreate the item boxes and their items in the ui for editting.
            for (int i = 0; i < question.questionItems.Count; i++)
            {
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
                itemContainerCount++;

                for (int y = 0; y < question.questionItems[i].items.Count; y++)
                {
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
                    itemCount++;

                    pnlItem.Controls.Add(itemNum);
                    pnlItem.Controls.Add(itemTextBox);
                    pnlItem.Controls.Add(deleteItem);
                    pnlItemContainer.Controls.Add(pnlItem);
                }
            }                
        }
    }
}