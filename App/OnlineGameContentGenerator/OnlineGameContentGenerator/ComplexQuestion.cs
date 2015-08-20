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
                    ((Button)sender).Parent.Dispose();
                    item.Dispose();
                    applyChangesToQuesitonComplex((questionObjectComplex)game[(int)lastSelected.Tag]);
                }
                itemCount = tempItemCount;
            }
        }
    }
}
