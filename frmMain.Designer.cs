
namespace VoskRecognizeTTS
{
    partial class frmMain
    {
        /// <summary>
        /// Обязательная переменная конструктора.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Освободить все используемые ресурсы.
        /// </summary>
        /// <param name="disposing">истинно, если управляемый ресурс должен быть удален; иначе ложно.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Код, автоматически созданный конструктором форм Windows

        /// <summary>
        /// Требуемый метод для поддержки конструктора — не изменяйте 
        /// содержимое этого метода с помощью редактора кода.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmMain));
            this.button1 = new System.Windows.Forms.Button();
            this.gbControls = new System.Windows.Forms.GroupBox();
            this.cbPlayRecognized = new System.Windows.Forms.CheckBox();
            this.tbTTSVolume = new System.Windows.Forms.TrackBar();
            this.tbTTSRate = new System.Windows.Forms.TrackBar();
            this.lbTTSVolume = new System.Windows.Forms.Label();
            this.lbTTSRate = new System.Windows.Forms.Label();
            this.btnSavePath = new System.Windows.Forms.Button();
            this.tbSavePath = new System.Windows.Forms.TextBox();
            this.cbTTSSavePath = new System.Windows.Forms.CheckBox();
            this.cbMicrofone = new System.Windows.Forms.ComboBox();
            this.btnWavPath = new System.Windows.Forms.Button();
            this.tbWavPath = new System.Windows.Forms.TextBox();
            this.rbWav = new System.Windows.Forms.RadioButton();
            this.rbMic = new System.Windows.Forms.RadioButton();
            this.rbTTS = new System.Windows.Forms.RadioButton();
            this.cbTTSVoice = new System.Windows.Forms.ComboBox();
            this.btnVoskPath = new System.Windows.Forms.Button();
            this.tbVoskPath = new System.Windows.Forms.TextBox();
            this.lbVoskPath = new System.Windows.Forms.Label();
            this.splitter1 = new System.Windows.Forms.Splitter();
            this.gbData = new System.Windows.Forms.GroupBox();
            this.tbText = new System.Windows.Forms.TextBox();
            this.tbResult = new System.Windows.Forms.TextBox();
            this.splitter3 = new System.Windows.Forms.Splitter();
            this.splitter2 = new System.Windows.Forms.Splitter();
            this.tbLog = new System.Windows.Forms.TextBox();
            this.backgroundWorkerRecognize = new System.ComponentModel.BackgroundWorker();
            this.panel1 = new System.Windows.Forms.Panel();
            this.btnRecognize = new System.Windows.Forms.Button();
            this.gbControls.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.tbTTSVolume)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.tbTTSRate)).BeginInit();
            this.gbData.SuspendLayout();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // button1
            // 
            resources.ApplyResources(this.button1, "button1");
            this.button1.Name = "button1";
            this.button1.UseVisualStyleBackColor = true;
            // 
            // gbControls
            // 
            this.gbControls.Controls.Add(this.cbPlayRecognized);
            this.gbControls.Controls.Add(this.tbTTSVolume);
            this.gbControls.Controls.Add(this.tbTTSRate);
            this.gbControls.Controls.Add(this.lbTTSVolume);
            this.gbControls.Controls.Add(this.lbTTSRate);
            this.gbControls.Controls.Add(this.btnSavePath);
            this.gbControls.Controls.Add(this.tbSavePath);
            this.gbControls.Controls.Add(this.cbTTSSavePath);
            this.gbControls.Controls.Add(this.cbMicrofone);
            this.gbControls.Controls.Add(this.btnWavPath);
            this.gbControls.Controls.Add(this.tbWavPath);
            this.gbControls.Controls.Add(this.rbWav);
            this.gbControls.Controls.Add(this.rbMic);
            this.gbControls.Controls.Add(this.rbTTS);
            this.gbControls.Controls.Add(this.cbTTSVoice);
            this.gbControls.Controls.Add(this.btnVoskPath);
            this.gbControls.Controls.Add(this.tbVoskPath);
            this.gbControls.Controls.Add(this.lbVoskPath);
            resources.ApplyResources(this.gbControls, "gbControls");
            this.gbControls.Name = "gbControls";
            this.gbControls.TabStop = false;
            // 
            // cbPlayRecognized
            // 
            resources.ApplyResources(this.cbPlayRecognized, "cbPlayRecognized");
            this.cbPlayRecognized.Name = "cbPlayRecognized";
            this.cbPlayRecognized.UseVisualStyleBackColor = true;
            // 
            // tbTTSVolume
            // 
            resources.ApplyResources(this.tbTTSVolume, "tbTTSVolume");
            this.tbTTSVolume.LargeChange = 10;
            this.tbTTSVolume.Maximum = 100;
            this.tbTTSVolume.Name = "tbTTSVolume";
            this.tbTTSVolume.TickFrequency = 5;
            this.tbTTSVolume.TickStyle = System.Windows.Forms.TickStyle.TopLeft;
            this.tbTTSVolume.Value = 100;
            // 
            // tbTTSRate
            // 
            resources.ApplyResources(this.tbTTSRate, "tbTTSRate");
            this.tbTTSRate.LargeChange = 1;
            this.tbTTSRate.Minimum = -10;
            this.tbTTSRate.Name = "tbTTSRate";
            this.tbTTSRate.TickStyle = System.Windows.Forms.TickStyle.TopLeft;
            this.tbTTSRate.Value = -1;
            // 
            // lbTTSVolume
            // 
            resources.ApplyResources(this.lbTTSVolume, "lbTTSVolume");
            this.lbTTSVolume.Name = "lbTTSVolume";
            // 
            // lbTTSRate
            // 
            resources.ApplyResources(this.lbTTSRate, "lbTTSRate");
            this.lbTTSRate.Name = "lbTTSRate";
            // 
            // btnSavePath
            // 
            resources.ApplyResources(this.btnSavePath, "btnSavePath");
            this.btnSavePath.Name = "btnSavePath";
            this.btnSavePath.UseVisualStyleBackColor = true;
            this.btnSavePath.Click += new System.EventHandler(this.btnSavePath_Click);
            // 
            // tbSavePath
            // 
            resources.ApplyResources(this.tbSavePath, "tbSavePath");
            this.tbSavePath.Name = "tbSavePath";
            // 
            // cbTTSSavePath
            // 
            resources.ApplyResources(this.cbTTSSavePath, "cbTTSSavePath");
            this.cbTTSSavePath.Name = "cbTTSSavePath";
            this.cbTTSSavePath.UseVisualStyleBackColor = true;
            this.cbTTSSavePath.CheckedChanged += new System.EventHandler(this.saveToFileCheckedChanged);
            // 
            // cbMicrofone
            // 
            this.cbMicrofone.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbMicrofone.FormattingEnabled = true;
            resources.ApplyResources(this.cbMicrofone, "cbMicrofone");
            this.cbMicrofone.Name = "cbMicrofone";
            // 
            // btnWavPath
            // 
            resources.ApplyResources(this.btnWavPath, "btnWavPath");
            this.btnWavPath.Name = "btnWavPath";
            this.btnWavPath.UseVisualStyleBackColor = true;
            this.btnWavPath.Click += new System.EventHandler(this.btnWavPath_Click);
            // 
            // tbWavPath
            // 
            resources.ApplyResources(this.tbWavPath, "tbWavPath");
            this.tbWavPath.Name = "tbWavPath";
            // 
            // rbWav
            // 
            resources.ApplyResources(this.rbWav, "rbWav");
            this.rbWav.Name = "rbWav";
            this.rbWav.Tag = "";
            this.rbWav.UseVisualStyleBackColor = true;
            this.rbWav.CheckedChanged += new System.EventHandler(this.selectCheckVariant);
            // 
            // rbMic
            // 
            resources.ApplyResources(this.rbMic, "rbMic");
            this.rbMic.Name = "rbMic";
            this.rbMic.Tag = "";
            this.rbMic.UseVisualStyleBackColor = true;
            this.rbMic.CheckedChanged += new System.EventHandler(this.selectCheckVariant);
            // 
            // rbTTS
            // 
            resources.ApplyResources(this.rbTTS, "rbTTS");
            this.rbTTS.Checked = true;
            this.rbTTS.Name = "rbTTS";
            this.rbTTS.TabStop = true;
            this.rbTTS.Tag = "";
            this.rbTTS.UseVisualStyleBackColor = true;
            this.rbTTS.CheckedChanged += new System.EventHandler(this.selectCheckVariant);
            // 
            // cbTTSVoice
            // 
            this.cbTTSVoice.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbTTSVoice.FormattingEnabled = true;
            resources.ApplyResources(this.cbTTSVoice, "cbTTSVoice");
            this.cbTTSVoice.Name = "cbTTSVoice";
            // 
            // btnVoskPath
            // 
            resources.ApplyResources(this.btnVoskPath, "btnVoskPath");
            this.btnVoskPath.Name = "btnVoskPath";
            this.btnVoskPath.UseVisualStyleBackColor = true;
            this.btnVoskPath.Click += new System.EventHandler(this.btnVoskPath_Click);
            // 
            // tbVoskPath
            // 
            resources.ApplyResources(this.tbVoskPath, "tbVoskPath");
            this.tbVoskPath.Name = "tbVoskPath";
            // 
            // lbVoskPath
            // 
            resources.ApplyResources(this.lbVoskPath, "lbVoskPath");
            this.lbVoskPath.Name = "lbVoskPath";
            // 
            // splitter1
            // 
            this.splitter1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            resources.ApplyResources(this.splitter1, "splitter1");
            this.splitter1.Name = "splitter1";
            this.splitter1.TabStop = false;
            // 
            // gbData
            // 
            this.gbData.Controls.Add(this.tbText);
            this.gbData.Controls.Add(this.tbResult);
            this.gbData.Controls.Add(this.splitter3);
            this.gbData.Controls.Add(this.splitter2);
            this.gbData.Controls.Add(this.tbLog);
            resources.ApplyResources(this.gbData, "gbData");
            this.gbData.Name = "gbData";
            this.gbData.TabStop = false;
            // 
            // tbText
            // 
            resources.ApplyResources(this.tbText, "tbText");
            this.tbText.Name = "tbText";
            // 
            // tbResult
            // 
            resources.ApplyResources(this.tbResult, "tbResult");
            this.tbResult.Name = "tbResult";
            this.tbResult.ReadOnly = true;
            // 
            // splitter3
            // 
            this.splitter3.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            resources.ApplyResources(this.splitter3, "splitter3");
            this.splitter3.Name = "splitter3";
            this.splitter3.TabStop = false;
            // 
            // splitter2
            // 
            this.splitter2.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            resources.ApplyResources(this.splitter2, "splitter2");
            this.splitter2.Name = "splitter2";
            this.splitter2.TabStop = false;
            // 
            // tbLog
            // 
            resources.ApplyResources(this.tbLog, "tbLog");
            this.tbLog.Name = "tbLog";
            this.tbLog.ReadOnly = true;
            // 
            // backgroundWorkerRecognize
            // 
            this.backgroundWorkerRecognize.WorkerSupportsCancellation = true;
            this.backgroundWorkerRecognize.DoWork += new System.ComponentModel.DoWorkEventHandler(this.backgroundWorkerRecognize_DoWork);
            this.backgroundWorkerRecognize.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.backgroundWorkerRecognize_RunWorkerCompleted);
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.btnRecognize);
            resources.ApplyResources(this.panel1, "panel1");
            this.panel1.Name = "panel1";
            // 
            // btnRecognize
            // 
            resources.ApplyResources(this.btnRecognize, "btnRecognize");
            this.btnRecognize.Name = "btnRecognize";
            this.btnRecognize.UseVisualStyleBackColor = true;
            this.btnRecognize.Click += new System.EventHandler(this.btnRecognize_Click);
            // 
            // frmMain
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.gbData);
            this.Controls.Add(this.splitter1);
            this.Controls.Add(this.gbControls);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.panel1);
            this.Name = "frmMain";
            this.gbControls.ResumeLayout(false);
            this.gbControls.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.tbTTSVolume)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.tbTTSRate)).EndInit();
            this.gbData.ResumeLayout(false);
            this.gbData.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.GroupBox gbControls;
        private System.Windows.Forms.Splitter splitter1;
        private System.Windows.Forms.GroupBox gbData;
        private System.Windows.Forms.Button btnVoskPath;
        private System.Windows.Forms.TextBox tbVoskPath;
        private System.Windows.Forms.Label lbVoskPath;
        private System.Windows.Forms.ComboBox cbTTSVoice;
        private System.Windows.Forms.ComboBox cbMicrofone;
        private System.Windows.Forms.Button btnWavPath;
        private System.Windows.Forms.TextBox tbWavPath;
        private System.Windows.Forms.RadioButton rbWav;
        private System.Windows.Forms.RadioButton rbMic;
        private System.Windows.Forms.RadioButton rbTTS;
        private System.Windows.Forms.Button btnSavePath;
        private System.Windows.Forms.TextBox tbSavePath;
        private System.Windows.Forms.CheckBox cbTTSSavePath;
        private System.ComponentModel.BackgroundWorker backgroundWorkerRecognize;
        private System.Windows.Forms.TextBox tbLog;
        private System.Windows.Forms.TextBox tbText;
        private System.Windows.Forms.TextBox tbResult;
        private System.Windows.Forms.Splitter splitter3;
        private System.Windows.Forms.Splitter splitter2;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button btnRecognize;
        private System.Windows.Forms.Label lbTTSVolume;
        private System.Windows.Forms.Label lbTTSRate;
        private System.Windows.Forms.TrackBar tbTTSRate;
        private System.Windows.Forms.TrackBar tbTTSVolume;
        private System.Windows.Forms.CheckBox cbPlayRecognized;
    }
}

