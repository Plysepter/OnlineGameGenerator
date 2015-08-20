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
        //Multiple Choice item
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
            popup.Text = "Popup";
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
                    return;
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
            popupTitleTextBox.Name = "popupTitleTextBox";
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
            popupBodyTextBox.MaxLength = 1500;

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
            popUpGenerate(null, sender);
        }

        //Insert information entered in the pnlQuestionDetails into the question array when it is a questionObjectSimple
        private void applyChangesToQuesitonSimple(questionObjectSimple question, bool enableCorrectCheck)
        {
            question.questionText = pnlQuestionDetails.Controls[0].Controls[1].Text;
            question.weight = int.Parse(pnlQuestionDetails.Controls[0].Controls[5].Text);
            itemCount = pnlQuestionDetails.Controls.Count - 1;

            //make sure there is at least one correct answer
            bool hasCorrect = false;
            question.questionItems = new List<questionItemSimple>(); ;

            for (int i = 0; i < itemCount; i++)
            {
                //Grab the item from the list of items
                Panel c = (Panel)pnlQuestionDetails.Controls[i + 1];
                question.questionItems.Add(new questionItemSimple());
                if (c.Name != "questionEdit")
                {
                    question.questionItems[i].itemText = c.Controls.Find("itemText", true)[0].Text;
                    question.questionItems[i].correct = ((CheckBox)c.Controls.Find("correct", true)[0]).Checked;
                    if (((CheckBox)c.Controls.Find("correct", true)[0]).Checked == true)
                    {
                        hasCorrect = true;
                    }
                    question.questionItems[i].popups = new popups();
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
            if (enableCorrectCheck == true)
            {
                if (hasCorrect == false)
                {
                    errorHandle("Please make sure at least one option is marked as correct.");
                    return;
                }
            }
            //apply the changes to the question array
            game[(int)lastSelected.Tag] = question;
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
    }
}
