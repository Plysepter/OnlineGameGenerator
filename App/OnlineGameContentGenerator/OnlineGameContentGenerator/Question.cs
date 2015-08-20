using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace OnlineGameContentGenerator
{
    public partial class Form1 : Form
    {
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
    }
}
