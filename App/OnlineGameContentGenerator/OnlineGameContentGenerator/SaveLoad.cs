using System;
using System.Collections.Generic;
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
                popup.Text = "Popup";
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
                        return;
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

        private void loadAllTheThings()
        {            
            //Credit to code goes to these glorious people http://www.techcoil.com/blog/how-to-save-and-load-objects-to-and-from-file-in-c/
            BinaryFormatter formatter = new BinaryFormatter();
            OpenFileDialog ofd1 = new OpenFileDialog();
            ofd1.InitialDirectory = Environment.SpecialFolder.Desktop.ToString();
            ofd1.Filter = "*Online Game File (*.ogf)|*.ogf";
            if (ofd1.ShowDialog() == DialogResult.OK)
            {
                pnlQuestions.Controls.Clear();
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
                    return;
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

                    // Close the writerFileStream when done.
                    writerFileStream.Close();
                }
                catch (Exception)
                {
                    errorHandle("Unable to save game, please try again.");
                } // end try-catch
            } // end public bool Load()

        }
    }
}
