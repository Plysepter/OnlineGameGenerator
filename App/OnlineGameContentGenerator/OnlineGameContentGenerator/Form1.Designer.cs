namespace OnlineGameContentGenerator
{
    partial class Form1
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.btnAddOption = new System.Windows.Forms.Button();
            this.pnlCommonFunctions = new System.Windows.Forms.Panel();
            this.btnNewQuestion = new System.Windows.Forms.Button();
            this.btnGameGenerate = new System.Windows.Forms.Button();
            this.pnlQuestionOptions = new System.Windows.Forms.Panel();
            this.btnApply = new System.Windows.Forms.Button();
            this.pnlQuestions = new System.Windows.Forms.FlowLayoutPanel();
            this.panel6 = new System.Windows.Forms.Panel();
            this.checkBox8 = new System.Windows.Forms.CheckBox();
            this.checkBox9 = new System.Windows.Forms.CheckBox();
            this.button5 = new System.Windows.Forms.Button();
            this.label11 = new System.Windows.Forms.Label();
            this.textBox12 = new System.Windows.Forms.TextBox();
            this.label12 = new System.Windows.Forms.Label();
            this.textBox13 = new System.Windows.Forms.TextBox();
            this.label13 = new System.Windows.Forms.Label();
            this.textBox14 = new System.Windows.Forms.TextBox();
            this.pnlQuestionDetails = new System.Windows.Forms.FlowLayoutPanel();
            this.colourPicker = new System.Windows.Forms.ColorDialog();
            this.btnSave = new System.Windows.Forms.Button();
            this.btnLoad = new System.Windows.Forms.Button();
            this.pnlCommonFunctions.SuspendLayout();
            this.pnlQuestionOptions.SuspendLayout();
            this.panel6.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnAddOption
            // 
            this.btnAddOption.BackColor = System.Drawing.Color.Black;
            this.btnAddOption.Enabled = false;
            this.btnAddOption.FlatAppearance.BorderColor = System.Drawing.Color.Red;
            this.btnAddOption.FlatAppearance.BorderSize = 0;
            this.btnAddOption.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnAddOption.ForeColor = System.Drawing.SystemColors.ButtonHighlight;
            this.btnAddOption.Location = new System.Drawing.Point(264, 1);
            this.btnAddOption.Name = "btnAddOption";
            this.btnAddOption.Size = new System.Drawing.Size(70, 26);
            this.btnAddOption.TabIndex = 1;
            this.btnAddOption.Text = "Add Option";
            this.btnAddOption.UseVisualStyleBackColor = false;
            this.btnAddOption.Click += new System.EventHandler(this.btnAddOption_Click);
            // 
            // pnlCommonFunctions
            // 
            this.pnlCommonFunctions.BackColor = System.Drawing.SystemColors.ControlDarkDark;
            this.pnlCommonFunctions.Controls.Add(this.btnNewQuestion);
            this.pnlCommonFunctions.Controls.Add(this.btnLoad);
            this.pnlCommonFunctions.Controls.Add(this.btnSave);
            this.pnlCommonFunctions.Controls.Add(this.btnGameGenerate);
            this.pnlCommonFunctions.Location = new System.Drawing.Point(0, 401);
            this.pnlCommonFunctions.Name = "pnlCommonFunctions";
            this.pnlCommonFunctions.Size = new System.Drawing.Size(205, 112);
            this.pnlCommonFunctions.TabIndex = 1;
            // 
            // btnNewQuestion
            // 
            this.btnNewQuestion.BackColor = System.Drawing.Color.Black;
            this.btnNewQuestion.FlatAppearance.BorderColor = System.Drawing.Color.Red;
            this.btnNewQuestion.FlatAppearance.BorderSize = 0;
            this.btnNewQuestion.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnNewQuestion.ForeColor = System.Drawing.SystemColors.ButtonHighlight;
            this.btnNewQuestion.Location = new System.Drawing.Point(12, 6);
            this.btnNewQuestion.Name = "btnNewQuestion";
            this.btnNewQuestion.Size = new System.Drawing.Size(186, 20);
            this.btnNewQuestion.TabIndex = 1;
            this.btnNewQuestion.Text = "New Question";
            this.btnNewQuestion.UseVisualStyleBackColor = false;
            this.btnNewQuestion.Click += new System.EventHandler(this.btnNewQuestion_Click);
            // 
            // btnGameGenerate
            // 
            this.btnGameGenerate.BackColor = System.Drawing.Color.Black;
            this.btnGameGenerate.FlatAppearance.BorderColor = System.Drawing.Color.Red;
            this.btnGameGenerate.FlatAppearance.BorderSize = 0;
            this.btnGameGenerate.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnGameGenerate.ForeColor = System.Drawing.SystemColors.ButtonHighlight;
            this.btnGameGenerate.Location = new System.Drawing.Point(12, 32);
            this.btnGameGenerate.Name = "btnGameGenerate";
            this.btnGameGenerate.Size = new System.Drawing.Size(186, 20);
            this.btnGameGenerate.TabIndex = 1;
            this.btnGameGenerate.Text = "Generate Game";
            this.btnGameGenerate.UseVisualStyleBackColor = false;
            this.btnGameGenerate.Click += new System.EventHandler(this.btnGameGenerate_Click);
            // 
            // pnlQuestionOptions
            // 
            this.pnlQuestionOptions.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.pnlQuestionOptions.Controls.Add(this.btnApply);
            this.pnlQuestionOptions.Controls.Add(this.btnAddOption);
            this.pnlQuestionOptions.Location = new System.Drawing.Point(208, 480);
            this.pnlQuestionOptions.Name = "pnlQuestionOptions";
            this.pnlQuestionOptions.Size = new System.Drawing.Size(347, 33);
            this.pnlQuestionOptions.TabIndex = 2;
            // 
            // btnApply
            // 
            this.btnApply.BackColor = System.Drawing.Color.Black;
            this.btnApply.FlatAppearance.BorderColor = System.Drawing.Color.Red;
            this.btnApply.FlatAppearance.BorderSize = 0;
            this.btnApply.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnApply.ForeColor = System.Drawing.SystemColors.ButtonHighlight;
            this.btnApply.Location = new System.Drawing.Point(3, 1);
            this.btnApply.Name = "btnApply";
            this.btnApply.Size = new System.Drawing.Size(92, 26);
            this.btnApply.TabIndex = 1;
            this.btnApply.Text = "Apply Changes";
            this.btnApply.UseVisualStyleBackColor = false;
            this.btnApply.Click += new System.EventHandler(this.btnApply_Click);
            // 
            // pnlQuestions
            // 
            this.pnlQuestions.AutoScroll = true;
            this.pnlQuestions.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.pnlQuestions.FlowDirection = System.Windows.Forms.FlowDirection.TopDown;
            this.pnlQuestions.Location = new System.Drawing.Point(0, 0);
            this.pnlQuestions.Margin = new System.Windows.Forms.Padding(0);
            this.pnlQuestions.Name = "pnlQuestions";
            this.pnlQuestions.Size = new System.Drawing.Size(205, 401);
            this.pnlQuestions.TabIndex = 0;
            this.pnlQuestions.WrapContents = false;
            // 
            // panel6
            // 
            this.panel6.BackColor = System.Drawing.SystemColors.ControlDarkDark;
            this.panel6.Controls.Add(this.checkBox8);
            this.panel6.Controls.Add(this.checkBox9);
            this.panel6.Controls.Add(this.button5);
            this.panel6.Controls.Add(this.label11);
            this.panel6.Controls.Add(this.textBox12);
            this.panel6.Controls.Add(this.label12);
            this.panel6.Controls.Add(this.textBox13);
            this.panel6.Controls.Add(this.label13);
            this.panel6.Controls.Add(this.textBox14);
            this.panel6.Location = new System.Drawing.Point(208, 376);
            this.panel6.Name = "panel6";
            this.panel6.Size = new System.Drawing.Size(320, 101);
            this.panel6.TabIndex = 2;
            // 
            // checkBox8
            // 
            this.checkBox8.AutoSize = true;
            this.checkBox8.Location = new System.Drawing.Point(68, 35);
            this.checkBox8.Name = "checkBox8";
            this.checkBox8.Size = new System.Drawing.Size(57, 17);
            this.checkBox8.TabIndex = 4;
            this.checkBox8.Text = "Popup";
            this.checkBox8.UseVisualStyleBackColor = true;
            // 
            // checkBox9
            // 
            this.checkBox9.AutoSize = true;
            this.checkBox9.Location = new System.Drawing.Point(6, 35);
            this.checkBox9.Name = "checkBox9";
            this.checkBox9.Size = new System.Drawing.Size(60, 17);
            this.checkBox9.TabIndex = 4;
            this.checkBox9.Text = "Correct";
            this.checkBox9.UseVisualStyleBackColor = true;
            // 
            // button5
            // 
            this.button5.BackColor = System.Drawing.Color.Black;
            this.button5.FlatAppearance.BorderColor = System.Drawing.Color.Red;
            this.button5.FlatAppearance.BorderSize = 0;
            this.button5.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button5.ForeColor = System.Drawing.SystemColors.ButtonHighlight;
            this.button5.Location = new System.Drawing.Point(295, 5);
            this.button5.Name = "button5";
            this.button5.Size = new System.Drawing.Size(20, 20);
            this.button5.TabIndex = 1;
            this.button5.Text = "X";
            this.button5.UseVisualStyleBackColor = false;
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F);
            this.label11.Location = new System.Drawing.Point(3, 78);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(68, 13);
            this.label11.TabIndex = 0;
            this.label11.Text = "Popup Body:";
            // 
            // textBox12
            // 
            this.textBox12.Location = new System.Drawing.Point(72, 75);
            this.textBox12.Name = "textBox12";
            this.textBox12.Size = new System.Drawing.Size(217, 20);
            this.textBox12.TabIndex = 1;
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F);
            this.label12.Location = new System.Drawing.Point(3, 55);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(64, 13);
            this.label12.TabIndex = 0;
            this.label12.Text = "Popup Title:";
            // 
            // textBox13
            // 
            this.textBox13.Location = new System.Drawing.Point(72, 52);
            this.textBox13.Name = "textBox13";
            this.textBox13.Size = new System.Drawing.Size(217, 20);
            this.textBox13.TabIndex = 1;
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F);
            this.label13.Location = new System.Drawing.Point(3, 12);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(50, 13);
            this.label13.TabIndex = 0;
            this.label13.Text = "Option 1:";
            // 
            // textBox14
            // 
            this.textBox14.Location = new System.Drawing.Point(59, 9);
            this.textBox14.Name = "textBox14";
            this.textBox14.Size = new System.Drawing.Size(230, 20);
            this.textBox14.TabIndex = 1;
            // 
            // pnlQuestionDetails
            // 
            this.pnlQuestionDetails.AutoScroll = true;
            this.pnlQuestionDetails.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.pnlQuestionDetails.FlowDirection = System.Windows.Forms.FlowDirection.TopDown;
            this.pnlQuestionDetails.Location = new System.Drawing.Point(208, 0);
            this.pnlQuestionDetails.Name = "pnlQuestionDetails";
            this.pnlQuestionDetails.Size = new System.Drawing.Size(347, 481);
            this.pnlQuestionDetails.TabIndex = 3;
            this.pnlQuestionDetails.WrapContents = false;
            // 
            // colourPicker
            // 
            this.colourPicker.FullOpen = true;
            // 
            // btnSave
            // 
            this.btnSave.BackColor = System.Drawing.Color.Black;
            this.btnSave.FlatAppearance.BorderColor = System.Drawing.Color.Red;
            this.btnSave.FlatAppearance.BorderSize = 0;
            this.btnSave.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSave.ForeColor = System.Drawing.SystemColors.ButtonHighlight;
            this.btnSave.Location = new System.Drawing.Point(12, 58);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(186, 20);
            this.btnSave.TabIndex = 1;
            this.btnSave.Text = "Save";
            this.btnSave.UseVisualStyleBackColor = false;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // btnLoad
            // 
            this.btnLoad.BackColor = System.Drawing.Color.Black;
            this.btnLoad.FlatAppearance.BorderColor = System.Drawing.Color.Red;
            this.btnLoad.FlatAppearance.BorderSize = 0;
            this.btnLoad.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnLoad.ForeColor = System.Drawing.SystemColors.ButtonHighlight;
            this.btnLoad.Location = new System.Drawing.Point(12, 84);
            this.btnLoad.Name = "btnLoad";
            this.btnLoad.Size = new System.Drawing.Size(186, 20);
            this.btnLoad.TabIndex = 1;
            this.btnLoad.Text = "Load";
            this.btnLoad.UseVisualStyleBackColor = false;
            this.btnLoad.Click += new System.EventHandler(this.btnLoad_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.ControlDark;
            this.ClientSize = new System.Drawing.Size(554, 512);
            this.Controls.Add(this.pnlQuestionDetails);
            this.Controls.Add(this.pnlQuestions);
            this.Controls.Add(this.pnlQuestionOptions);
            this.Controls.Add(this.panel6);
            this.Controls.Add(this.pnlCommonFunctions);
            this.MaximizeBox = false;
            this.MaximumSize = new System.Drawing.Size(570, 550);
            this.MinimumSize = new System.Drawing.Size(570, 550);
            this.Name = "Form1";
            this.Text = "Online Game Generator";
            this.pnlCommonFunctions.ResumeLayout(false);
            this.pnlQuestionOptions.ResumeLayout(false);
            this.panel6.ResumeLayout(false);
            this.panel6.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel pnlCommonFunctions;
        private System.Windows.Forms.Button btnNewQuestion;
        private System.Windows.Forms.Button btnGameGenerate;
        private System.Windows.Forms.Button btnAddOption;
        private System.Windows.Forms.Panel pnlQuestionOptions;
        private System.Windows.Forms.FlowLayoutPanel pnlQuestions;
        private System.Windows.Forms.Panel panel6;
        private System.Windows.Forms.CheckBox checkBox8;
        private System.Windows.Forms.CheckBox checkBox9;
        private System.Windows.Forms.Button button5;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.TextBox textBox12;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.TextBox textBox13;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.TextBox textBox14;
        private System.Windows.Forms.FlowLayoutPanel pnlQuestionDetails;
        private System.Windows.Forms.Button btnApply;
        private System.Windows.Forms.ColorDialog colourPicker;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.Button btnLoad;
    }
}

