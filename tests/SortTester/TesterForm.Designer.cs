namespace SortTester
{
  partial class MainForm
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
      this.startButton = new System.Windows.Forms.Button();
      this.label1 = new System.Windows.Forms.Label();
      this.label2 = new System.Windows.Forms.Label();
      this.sourceOrderComboBox = new System.Windows.Forms.ComboBox();
      this.label3 = new System.Windows.Forms.Label();
      this.sort1ComboBox = new System.Windows.Forms.ComboBox();
      this.sort2ComboBox = new System.Windows.Forms.ComboBox();
      this.label4 = new System.Windows.Forms.Label();
      this.sort3ComboBox = new System.Windows.Forms.ComboBox();
      this.label5 = new System.Windows.Forms.Label();
      this.sort4ComboBox = new System.Windows.Forms.ComboBox();
      this.label6 = new System.Windows.Forms.Label();
      this.sort1CheckBox = new System.Windows.Forms.CheckBox();
      this.sort2CheckBox = new System.Windows.Forms.CheckBox();
      this.sort3CheckBox = new System.Windows.Forms.CheckBox();
      this.sort4CheckBox = new System.Windows.Forms.CheckBox();
      this.textBox1 = new System.Windows.Forms.TextBox();
      this.label7 = new System.Windows.Forms.Label();
      this.ElementsCountInput = new System.Windows.Forms.NumericUpDown();
      this.backgroundWorker = new System.ComponentModel.BackgroundWorker();
      this.loopsCountInput = new System.Windows.Forms.NumericUpDown();
      ((System.ComponentModel.ISupportInitialize)(this.ElementsCountInput)).BeginInit();
      ((System.ComponentModel.ISupportInitialize)(this.loopsCountInput)).BeginInit();
      this.SuspendLayout();
      // 
      // startButton
      // 
      this.startButton.Location = new System.Drawing.Point(213, 169);
      this.startButton.Name = "startButton";
      this.startButton.Size = new System.Drawing.Size(75, 23);
      this.startButton.TabIndex = 18;
      this.startButton.Text = "Start";
      this.startButton.UseVisualStyleBackColor = true;
      this.startButton.Click += new System.EventHandler(this.startButton_Click);
      // 
      // label1
      // 
      this.label1.AutoSize = true;
      this.label1.Location = new System.Drawing.Point(12, 15);
      this.label1.Name = "label1";
      this.label1.Size = new System.Drawing.Size(50, 13);
      this.label1.TabIndex = 0;
      this.label1.Text = "Elements";
      // 
      // label2
      // 
      this.label2.AutoSize = true;
      this.label2.Location = new System.Drawing.Point(209, 15);
      this.label2.Name = "label2";
      this.label2.Size = new System.Drawing.Size(71, 13);
      this.label2.TabIndex = 2;
      this.label2.Text = "Source order:";
      // 
      // sourceOrderComboBox
      // 
      this.sourceOrderComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
      this.sourceOrderComboBox.FormattingEnabled = true;
      this.sourceOrderComboBox.Items.AddRange(new object[] {
            "Random",
            "Match test order",
            "Reverse test order"});
      this.sourceOrderComboBox.Location = new System.Drawing.Point(286, 12);
      this.sourceOrderComboBox.Name = "sourceOrderComboBox";
      this.sourceOrderComboBox.Size = new System.Drawing.Size(121, 21);
      this.sourceOrderComboBox.TabIndex = 3;
      // 
      // label3
      // 
      this.label3.AutoSize = true;
      this.label3.Location = new System.Drawing.Point(12, 41);
      this.label3.Name = "label3";
      this.label3.Size = new System.Drawing.Size(49, 13);
      this.label3.TabIndex = 4;
      this.label3.Text = "First sort:";
      // 
      // sort1ComboBox
      // 
      this.sort1ComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
      this.sort1ComboBox.FormattingEnabled = true;   
      this.sort1ComboBox.Location = new System.Drawing.Point(85, 38);
      this.sort1ComboBox.Name = "sort1ComboBox";
      this.sort1ComboBox.Size = new System.Drawing.Size(121, 21);
      this.sort1ComboBox.TabIndex = 5;
      // 
      // sort2ComboBox
      // 
      this.sort2ComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
      this.sort2ComboBox.FormattingEnabled = true;
      this.sort2ComboBox.Location = new System.Drawing.Point(85, 65);
      this.sort2ComboBox.Name = "sort2ComboBox";
      this.sort2ComboBox.Size = new System.Drawing.Size(121, 21);
      this.sort2ComboBox.TabIndex = 8;
      this.sort2ComboBox.SelectedIndexChanged += new System.EventHandler(this.sort2ComboBox_SelectedIndexChanged);
      // 
      // label4
      // 
      this.label4.AutoSize = true;
      this.label4.Location = new System.Drawing.Point(12, 68);
      this.label4.Name = "label4";
      this.label4.Size = new System.Drawing.Size(67, 13);
      this.label4.TabIndex = 7;
      this.label4.Text = "Second sort:";
      // 
      // sort3ComboBox
      // 
      this.sort3ComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
      this.sort3ComboBox.FormattingEnabled = true;
      this.sort3ComboBox.Location = new System.Drawing.Point(85, 92);
      this.sort3ComboBox.Name = "sort3ComboBox";
      this.sort3ComboBox.Size = new System.Drawing.Size(121, 21);
      this.sort3ComboBox.TabIndex = 11;
      this.sort3ComboBox.SelectedIndexChanged += new System.EventHandler(this.sort3ComboBox_SelectedIndexChanged);
      // 
      // label5
      // 
      this.label5.AutoSize = true;
      this.label5.Location = new System.Drawing.Point(12, 95);
      this.label5.Name = "label5";
      this.label5.Size = new System.Drawing.Size(54, 13);
      this.label5.TabIndex = 10;
      this.label5.Text = "Third sort:";
      // 
      // sort4ComboBox
      // 
      this.sort4ComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
      this.sort4ComboBox.FormattingEnabled = true;
      this.sort4ComboBox.Location = new System.Drawing.Point(85, 119);
      this.sort4ComboBox.Name = "sort4ComboBox";
      this.sort4ComboBox.Size = new System.Drawing.Size(121, 21);
      this.sort4ComboBox.TabIndex = 14;
      this.sort4ComboBox.SelectedIndexChanged += new System.EventHandler(this.sort4ComboBox_SelectedIndexChanged);
      // 
      // label6
      // 
      this.label6.AutoSize = true;
      this.label6.Location = new System.Drawing.Point(12, 122);
      this.label6.Name = "label6";
      this.label6.Size = new System.Drawing.Size(45, 13);
      this.label6.TabIndex = 13;
      this.label6.Text = "4th sort:";
      // 
      // sort1CheckBox
      // 
      this.sort1CheckBox.AutoSize = true;
      this.sort1CheckBox.Checked = true;
      this.sort1CheckBox.CheckState = System.Windows.Forms.CheckState.Checked;
      this.sort1CheckBox.Location = new System.Drawing.Point(212, 40);
      this.sort1CheckBox.Name = "sort1CheckBox";
      this.sort1CheckBox.Size = new System.Drawing.Size(76, 17);
      this.sort1CheckBox.TabIndex = 6;
      this.sort1CheckBox.Text = "Ascending";
      this.sort1CheckBox.UseVisualStyleBackColor = true;
      // 
      // sort2CheckBox
      // 
      this.sort2CheckBox.AutoSize = true;
      this.sort2CheckBox.Checked = true;
      this.sort2CheckBox.CheckState = System.Windows.Forms.CheckState.Checked;
      this.sort2CheckBox.Location = new System.Drawing.Point(212, 67);
      this.sort2CheckBox.Name = "sort2CheckBox";
      this.sort2CheckBox.Size = new System.Drawing.Size(76, 17);
      this.sort2CheckBox.TabIndex = 9;
      this.sort2CheckBox.Text = "Ascending";
      this.sort2CheckBox.UseVisualStyleBackColor = true;
      // 
      // sort3CheckBox
      // 
      this.sort3CheckBox.AutoSize = true;
      this.sort3CheckBox.Checked = true;
      this.sort3CheckBox.CheckState = System.Windows.Forms.CheckState.Checked;
      this.sort3CheckBox.Location = new System.Drawing.Point(212, 94);
      this.sort3CheckBox.Name = "sort3CheckBox";
      this.sort3CheckBox.Size = new System.Drawing.Size(76, 17);
      this.sort3CheckBox.TabIndex = 12;
      this.sort3CheckBox.Text = "Ascending";
      this.sort3CheckBox.UseVisualStyleBackColor = true;
      // 
      // sort4CheckBox
      // 
      this.sort4CheckBox.AutoSize = true;
      this.sort4CheckBox.Checked = true;
      this.sort4CheckBox.CheckState = System.Windows.Forms.CheckState.Checked;
      this.sort4CheckBox.Location = new System.Drawing.Point(212, 121);
      this.sort4CheckBox.Name = "sort4CheckBox";
      this.sort4CheckBox.Size = new System.Drawing.Size(76, 17);
      this.sort4CheckBox.TabIndex = 15;
      this.sort4CheckBox.Text = "Ascending";
      this.sort4CheckBox.UseVisualStyleBackColor = true;
      // 
      // textBox1
      // 
      this.textBox1.Location = new System.Drawing.Point(15, 211);
      this.textBox1.Multiline = true;
      this.textBox1.Name = "textBox1";
      this.textBox1.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
      this.textBox1.Size = new System.Drawing.Size(575, 327);
      this.textBox1.TabIndex = 19;
      // 
      // label7
      // 
      this.label7.AutoSize = true;
      this.label7.Location = new System.Drawing.Point(12, 174);
      this.label7.Name = "label7";
      this.label7.Size = new System.Drawing.Size(56, 13);
      this.label7.TabIndex = 16;
      this.label7.Text = "Test loops";
      // 
      // ElementsCountInput
      // 
      this.ElementsCountInput.Increment = new decimal(new int[] {
            1000,
            0,
            0,
            0});
      this.ElementsCountInput.Location = new System.Drawing.Point(86, 13);
      this.ElementsCountInput.Maximum = new decimal(new int[] {
            10000000,
            0,
            0,
            0});
      this.ElementsCountInput.Minimum = new decimal(new int[] {
            10,
            0,
            0,
            0});
      this.ElementsCountInput.Name = "ElementsCountInput";
      this.ElementsCountInput.Size = new System.Drawing.Size(120, 20);
      this.ElementsCountInput.TabIndex = 1;
      this.ElementsCountInput.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
      this.ElementsCountInput.UpDownAlign = System.Windows.Forms.LeftRightAlignment.Left;
      this.ElementsCountInput.Value = new decimal(new int[] {
            10000,
            0,
            0,
            0});
      // 
      // backgroundWorker
      // 
      this.backgroundWorker.WorkerReportsProgress = true;
      this.backgroundWorker.WorkerSupportsCancellation = true;
      this.backgroundWorker.DoWork += new System.ComponentModel.DoWorkEventHandler(this.backgroundWorker_DoWork);
      this.backgroundWorker.ProgressChanged += new System.ComponentModel.ProgressChangedEventHandler(this.backgroundWorker_ProgressChanged);
      this.backgroundWorker.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.backgroundWorker_RunWorkerCompleted);
      // 
      // loopsCountInput
      // 
      this.loopsCountInput.Location = new System.Drawing.Point(86, 172);
      this.loopsCountInput.Maximum = new decimal(new int[] {
            1000,
            0,
            0,
            0});
      this.loopsCountInput.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
      this.loopsCountInput.Name = "loopsCountInput";
      this.loopsCountInput.Size = new System.Drawing.Size(67, 20);
      this.loopsCountInput.TabIndex = 17;
      this.loopsCountInput.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
      this.loopsCountInput.UpDownAlign = System.Windows.Forms.LeftRightAlignment.Left;
      this.loopsCountInput.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
      // 
      // MainForm
      // 
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.ClientSize = new System.Drawing.Size(602, 550);
      this.Controls.Add(this.loopsCountInput);
      this.Controls.Add(this.ElementsCountInput);
      this.Controls.Add(this.label7);
      this.Controls.Add(this.textBox1);
      this.Controls.Add(this.sort4CheckBox);
      this.Controls.Add(this.sort3CheckBox);
      this.Controls.Add(this.sort2CheckBox);
      this.Controls.Add(this.sort1CheckBox);
      this.Controls.Add(this.sort4ComboBox);
      this.Controls.Add(this.label6);
      this.Controls.Add(this.sort3ComboBox);
      this.Controls.Add(this.label5);
      this.Controls.Add(this.sort2ComboBox);
      this.Controls.Add(this.label4);
      this.Controls.Add(this.sort1ComboBox);
      this.Controls.Add(this.label3);
      this.Controls.Add(this.sourceOrderComboBox);
      this.Controls.Add(this.label2);
      this.Controls.Add(this.label1);
      this.Controls.Add(this.startButton);
      this.MaximizeBox = false;
      this.Name = "MainForm";
      this.Text = "Custom Sort Tester";
      this.Load += new System.EventHandler(this.MainForm_Load);
      ((System.ComponentModel.ISupportInitialize)(this.ElementsCountInput)).EndInit();
      ((System.ComponentModel.ISupportInitialize)(this.loopsCountInput)).EndInit();
      this.ResumeLayout(false);
      this.PerformLayout();

    }

    #endregion

    private System.Windows.Forms.Button startButton;
    private System.Windows.Forms.Label label1;
    private System.Windows.Forms.Label label2;
    private System.Windows.Forms.ComboBox sourceOrderComboBox;
    private System.Windows.Forms.Label label3;
    private System.Windows.Forms.ComboBox sort1ComboBox;
    private System.Windows.Forms.ComboBox sort2ComboBox;
    private System.Windows.Forms.Label label4;
    private System.Windows.Forms.ComboBox sort3ComboBox;
    private System.Windows.Forms.Label label5;
    private System.Windows.Forms.ComboBox sort4ComboBox;
    private System.Windows.Forms.Label label6;
    private System.Windows.Forms.CheckBox sort1CheckBox;
    private System.Windows.Forms.CheckBox sort2CheckBox;
    private System.Windows.Forms.CheckBox sort3CheckBox;
    private System.Windows.Forms.CheckBox sort4CheckBox;
    private System.Windows.Forms.TextBox textBox1;
    private System.Windows.Forms.Label label7;
    private System.Windows.Forms.NumericUpDown ElementsCountInput;
    private System.ComponentModel.BackgroundWorker backgroundWorker;
    private System.Windows.Forms.NumericUpDown loopsCountInput;
  }
}

