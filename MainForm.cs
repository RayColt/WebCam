using DShowNET;
using DShowNET.Device;
using ImgProcessing;
using System;
using System.Collections;
using System.ComponentModel;
using System.Configuration;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows.Forms;
using Timer = System.Windows.Forms.Timer;
using Timing;
using WebCam;

namespace Webcam
{
    /// <summary> Summary description for MainForm. </summary>
    /// <summary> Summary description for MainForm. </summary>
    public class MainForm : Form, ISampleGrabberCB
    {
        bool dontSaveNext = true, firstRun = true;
        private int cPercent = 96, minSave = 96, count = 0, count2 = 0;
        string saveTime = "";
        private Counter counter = new Counter();
        private float sec = 0;
        private Bitmap a, b;
        private Bitmap wanted;
        private ToolBar toolBar;
        private Panel videoPanel;
        private Panel stillPanel;
        private PictureBox pictureBox;
        private ToolBarButton toolBarBtnTune;
        private ToolBarButton toolBarBtnGrab;
        private ToolBarButton toolBarBtnView;
        private ToolBarButton toolBarBtnConfig;
        private ToolBarButton toolBarBtnSave;
        private ImageList imgListToolBar;
        private PictureBox pictureBox1;
        private Label lblPercent;
        private Timer timer1;
        private Label lblSavePercent;
        private Label lblSaveOrNot;
        private PictureBox pictureBox2;
        private Button btnStart;
        private GroupBox groupBoxImageProcTime;
        private Label label4;
        private Label lblTotalTime;
        private Label label8;
        private Label lblTime;
        private Label lblCount;
        private Button btnStop;
        private Button btnAbout;
        private Label lblTimerValue;
        private TrackBar trckBarTimer;
        private TrackBar trckBarSaveValue;
        private IContainer components;
        private string path = "";
        private Configuration config;
        private string LogFile = @"\log.txt";

        public MainForm()
        {
            try
            {
                // Get the current configuration file.
                config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
                path = ConfigurationManager.AppSettings.Get("path");
                string log = ConfigurationManager.AppSettings.Get("logfile");
                if (!String.IsNullOrEmpty(log)) LogFile = log;
#if DEBUG
                Console.WriteLine("!!MSG: Directory path " + path);
#endif
                // Determine whether the directory exists.
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
#if DEBUG
                    Console.WriteLine("!!MSG: The directory was created successfully at {0}.", Directory.GetCreationTime(path));
#endif
                }
                else
                {
#if DEBUG
                    Console.WriteLine("!!MSG: Directory " + path + " exists already.");
#endif
                }
            }
            catch (Exception e)
            {
#if DEBUG
                Console.WriteLine("!!ERR-MSG: The process failed: {0}", e.ToString());
#endif
            }
            // Launch CamWatcher
            BootWatcher(path);
            // Required for Windows Form Designer support
            InitializeComponent();
            trckBarSaveValue.Value = cPercent;
            lblSavePercent.Text = cPercent + "%";
            timer1.Interval = trckBarTimer.Value;
            lblTimerValue.Text = trckBarTimer.Value.ToString();
        }

        /// <summary> Clean up any resources being used. </summary>
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                CloseInterfaces();
                if (components != null)
                {
                    components.Dispose();
                }
            }
            base.Dispose(disposing);
        }

        /// <summary> Launch HOTFOLDER SAVE DIRECTORY console </summary>
        private void BootWatcher(string p)
        {  
            new CamWatcher(p, LogFile);
        }

        #region Windows Form Designer generated code
        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.toolBar = new System.Windows.Forms.ToolBar();
            this.toolBarBtnTune = new System.Windows.Forms.ToolBarButton();
            this.toolBarBtnGrab = new System.Windows.Forms.ToolBarButton();
            this.toolBarBtnSave = new System.Windows.Forms.ToolBarButton();
            this.toolBarBtnView = new System.Windows.Forms.ToolBarButton();
            this.toolBarBtnConfig = new System.Windows.Forms.ToolBarButton();
            this.imgListToolBar = new System.Windows.Forms.ImageList(this.components);
            this.videoPanel = new System.Windows.Forms.Panel();
            this.stillPanel = new System.Windows.Forms.Panel();
            this.lblTimerValue = new System.Windows.Forms.Label();
            this.trckBarTimer = new System.Windows.Forms.TrackBar();
            this.pictureBox2 = new System.Windows.Forms.PictureBox();
            this.lblSaveOrNot = new System.Windows.Forms.Label();
            this.lblSavePercent = new System.Windows.Forms.Label();
            this.trckBarSaveValue = new System.Windows.Forms.TrackBar();
            this.lblPercent = new System.Windows.Forms.Label();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.pictureBox = new System.Windows.Forms.PictureBox();
            this.btnStop = new System.Windows.Forms.Button();
            this.btnStart = new System.Windows.Forms.Button();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.groupBoxImageProcTime = new System.Windows.Forms.GroupBox();
            this.label4 = new System.Windows.Forms.Label();
            this.lblTotalTime = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.lblTime = new System.Windows.Forms.Label();
            this.lblCount = new System.Windows.Forms.Label();
            this.btnAbout = new System.Windows.Forms.Button();
            this.stillPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.trckBarTimer)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.trckBarSaveValue)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox)).BeginInit();
            this.groupBoxImageProcTime.SuspendLayout();
            this.SuspendLayout();
            // 
            // toolBar
            // 
            this.toolBar.Buttons.AddRange(new System.Windows.Forms.ToolBarButton[] {
            this.toolBarBtnTune,
            this.toolBarBtnGrab,
            this.toolBarBtnSave,
            this.toolBarBtnView,
            this.toolBarBtnConfig});
            this.toolBar.Cursor = System.Windows.Forms.Cursors.Hand;
            this.toolBar.DropDownArrows = true;
            this.toolBar.ImageList = this.imgListToolBar;
            this.toolBar.Location = new System.Drawing.Point(0, 0);
            this.toolBar.Name = "toolBar";
            this.toolBar.ShowToolTips = true;
            this.toolBar.Size = new System.Drawing.Size(942, 42);
            this.toolBar.TabIndex = 0;
            this.toolBar.ButtonClick += new System.Windows.Forms.ToolBarButtonClickEventHandler(this.toolBar_ButtonClick);
            // 
            // toolBarBtnTune
            // 
            this.toolBarBtnTune.ImageIndex = 0;
            this.toolBarBtnTune.Name = "toolBarBtnTune";
            this.toolBarBtnTune.Text = "Tune";
            this.toolBarBtnTune.ToolTipText = "TV tuner dialog";
            // 
            // toolBarBtnGrab
            // 
            this.toolBarBtnGrab.ImageIndex = 1;
            this.toolBarBtnGrab.Name = "toolBarBtnGrab";
            this.toolBarBtnGrab.Text = "Grab";
            this.toolBarBtnGrab.ToolTipText = "Grab picture from stream";
            // 
            // toolBarBtnSave
            // 
            this.toolBarBtnSave.ImageIndex = 2;
            this.toolBarBtnSave.Name = "toolBarBtnSave";
            this.toolBarBtnSave.Text = "Save";
            this.toolBarBtnSave.ToolTipText = "Save image to file";
            // 
            // toolBarBtnView
            // 
            this.toolBarBtnView.ImageIndex = 3;
            this.toolBarBtnView.Name = "toolBarBtnView";
            this.toolBarBtnView.Text = "View";
            this.toolBarBtnView.ToolTipText = "View pictures";
            // 
            // toolBarBtnConfig
            // 
            this.toolBarBtnConfig.ImageIndex = 4;
            this.toolBarBtnConfig.Name = "toolBarBtnConfig";
            this.toolBarBtnConfig.Text = "Select Save Dir";
            this.toolBarBtnConfig.ToolTipText = "Select Save Directory Pictures";
            // 
            // imgListToolBar
            // 
            this.imgListToolBar.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imgListToolBar.ImageStream")));
            this.imgListToolBar.TransparentColor = System.Drawing.Color.Transparent;
            this.imgListToolBar.Images.SetKeyName(0, "");
            this.imgListToolBar.Images.SetKeyName(1, "");
            this.imgListToolBar.Images.SetKeyName(2, "");
            this.imgListToolBar.Images.SetKeyName(3, "");
            this.imgListToolBar.Images.SetKeyName(4, "");
            // 
            // videoPanel
            // 
            this.videoPanel.BackColor = System.Drawing.Color.Black;
            this.videoPanel.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.videoPanel.Location = new System.Drawing.Point(8, 40);
            this.videoPanel.Name = "videoPanel";
            this.videoPanel.Size = new System.Drawing.Size(461, 304);
            this.videoPanel.TabIndex = 1;
            this.videoPanel.Paint += new System.Windows.Forms.PaintEventHandler(this.videoPanel_Paint);
            this.videoPanel.Resize += new System.EventHandler(this.videoPanel_Resize);
            // 
            // stillPanel
            // 
            this.stillPanel.AutoScroll = true;
            this.stillPanel.AutoScrollMargin = new System.Drawing.Size(8, 8);
            this.stillPanel.AutoScrollMinSize = new System.Drawing.Size(32, 32);
            this.stillPanel.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.stillPanel.Controls.Add(this.lblTimerValue);
            this.stillPanel.Controls.Add(this.trckBarTimer);
            this.stillPanel.Controls.Add(this.pictureBox2);
            this.stillPanel.Controls.Add(this.lblSaveOrNot);
            this.stillPanel.Controls.Add(this.lblSavePercent);
            this.stillPanel.Controls.Add(this.trckBarSaveValue);
            this.stillPanel.Controls.Add(this.lblPercent);
            this.stillPanel.Controls.Add(this.pictureBox1);
            this.stillPanel.Controls.Add(this.pictureBox);
            this.stillPanel.Location = new System.Drawing.Point(475, 42);
            this.stillPanel.Name = "stillPanel";
            this.stillPanel.Size = new System.Drawing.Size(461, 302);
            this.stillPanel.TabIndex = 3;
            // 
            // lblTimerValue
            // 
            this.lblTimerValue.Location = new System.Drawing.Point(400, 16);
            this.lblTimerValue.Name = "lblTimerValue";
            this.lblTimerValue.Size = new System.Drawing.Size(40, 24);
            this.lblTimerValue.TabIndex = 10;
            this.lblTimerValue.Text = "375";
            // 
            // trckBarTimer
            // 
            this.trckBarTimer.LargeChange = 300;
            this.trckBarTimer.Location = new System.Drawing.Point(400, 48);
            this.trckBarTimer.Maximum = 1000;
            this.trckBarTimer.Minimum = 7;
            this.trckBarTimer.Name = "trckBarTimer";
            this.trckBarTimer.Orientation = System.Windows.Forms.Orientation.Vertical;
            this.trckBarTimer.Size = new System.Drawing.Size(45, 240);
            this.trckBarTimer.SmallChange = 10;
            this.trckBarTimer.TabIndex = 9;
            this.trckBarTimer.Value = 375;
            this.trckBarTimer.ValueChanged += new System.EventHandler(this.trckBarTimer_ValueChanged);
            // 
            // pictureBox2
            // 
            this.pictureBox2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pictureBox2.Location = new System.Drawing.Point(3, 196);
            this.pictureBox2.Name = "pictureBox2";
            this.pictureBox2.Size = new System.Drawing.Size(250, 94);
            this.pictureBox2.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBox2.TabIndex = 6;
            this.pictureBox2.TabStop = false;
            this.pictureBox2.Click += new System.EventHandler(this.pictureBox2_Click);
            // 
            // lblSaveOrNot
            // 
            this.lblSaveOrNot.Location = new System.Drawing.Point(261, 196);
            this.lblSaveOrNot.Name = "lblSaveOrNot";
            this.lblSaveOrNot.Size = new System.Drawing.Size(78, 32);
            this.lblSaveOrNot.TabIndex = 5;
            // 
            // lblSavePercent
            // 
            this.lblSavePercent.Location = new System.Drawing.Point(344, 16);
            this.lblSavePercent.Name = "lblSavePercent";
            this.lblSavePercent.Size = new System.Drawing.Size(40, 24);
            this.lblSavePercent.TabIndex = 4;
            // 
            // trckBarSaveValue
            // 
            this.trckBarSaveValue.Enabled = false;
            this.trckBarSaveValue.Location = new System.Drawing.Point(344, 48);
            this.trckBarSaveValue.Maximum = 100;
            this.trckBarSaveValue.Name = "trckBarSaveValue";
            this.trckBarSaveValue.Orientation = System.Windows.Forms.Orientation.Vertical;
            this.trckBarSaveValue.Size = new System.Drawing.Size(45, 240);
            this.trckBarSaveValue.TabIndex = 3;
            this.trckBarSaveValue.Scroll += new System.EventHandler(this.trckBarSaveValue_Scroll);
            this.trckBarSaveValue.ValueChanged += new System.EventHandler(this.trckBarSaveValue_ValueChanged);
            // 
            // lblPercent
            // 
            this.lblPercent.Location = new System.Drawing.Point(272, 99);
            this.lblPercent.Name = "lblPercent";
            this.lblPercent.Size = new System.Drawing.Size(40, 32);
            this.lblPercent.TabIndex = 2;
            this.lblPercent.Click += new System.EventHandler(this.lblPercent_Click);
            // 
            // pictureBox1
            // 
            this.pictureBox1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pictureBox1.Location = new System.Drawing.Point(3, 99);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(250, 94);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBox1.TabIndex = 1;
            this.pictureBox1.TabStop = false;
            // 
            // pictureBox
            // 
            this.pictureBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pictureBox.Location = new System.Drawing.Point(3, 2);
            this.pictureBox.Name = "pictureBox";
            this.pictureBox.Size = new System.Drawing.Size(250, 94);
            this.pictureBox.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBox.TabIndex = 0;
            this.pictureBox.TabStop = false;
            this.pictureBox.Click += new System.EventHandler(this.pictureBox_Click);
            // 
            // btnStop
            // 
            this.btnStop.BackColor = System.Drawing.SystemColors.ScrollBar;
            this.btnStop.Enabled = false;
            this.btnStop.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnStop.Location = new System.Drawing.Point(847, 355);
            this.btnStop.Name = "btnStop";
            this.btnStop.Size = new System.Drawing.Size(78, 40);
            this.btnStop.TabIndex = 8;
            this.btnStop.Text = "St&op";
            this.btnStop.UseVisualStyleBackColor = false;
            this.btnStop.Click += new System.EventHandler(this.btnStop_Click);
            // 
            // btnStart
            // 
            this.btnStart.BackColor = System.Drawing.SystemColors.ScrollBar;
            this.btnStart.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnStart.Location = new System.Drawing.Point(724, 355);
            this.btnStart.Name = "btnStart";
            this.btnStart.Size = new System.Drawing.Size(89, 40);
            this.btnStart.TabIndex = 7;
            this.btnStart.Text = "&Start";
            this.btnStart.UseVisualStyleBackColor = false;
            this.btnStart.Click += new System.EventHandler(this.btnStart_Click);
            // 
            // timer1
            // 
            this.timer1.Interval = 600;
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // groupBoxImageProcTime
            // 
            this.groupBoxImageProcTime.Controls.Add(this.label4);
            this.groupBoxImageProcTime.Controls.Add(this.lblTotalTime);
            this.groupBoxImageProcTime.Controls.Add(this.label8);
            this.groupBoxImageProcTime.Controls.Add(this.lblTime);
            this.groupBoxImageProcTime.ForeColor = System.Drawing.Color.DarkRed;
            this.groupBoxImageProcTime.Location = new System.Drawing.Point(8, 344);
            this.groupBoxImageProcTime.Name = "groupBoxImageProcTime";
            this.groupBoxImageProcTime.Size = new System.Drawing.Size(536, 56);
            this.groupBoxImageProcTime.TabIndex = 17;
            this.groupBoxImageProcTime.TabStop = false;
            this.groupBoxImageProcTime.Text = "Image Processing Time";
            // 
            // label4
            // 
            this.label4.ForeColor = System.Drawing.Color.DarkRed;
            this.label4.Location = new System.Drawing.Point(200, 24);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(64, 24);
            this.label4.TabIndex = 19;
            this.label4.Text = "Total Time";
            this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.label4.UseMnemonic = false;
            // 
            // lblTotalTime
            // 
            this.lblTotalTime.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblTotalTime.ForeColor = System.Drawing.Color.DarkRed;
            this.lblTotalTime.Location = new System.Drawing.Point(272, 24);
            this.lblTotalTime.Name = "lblTotalTime";
            this.lblTotalTime.Size = new System.Drawing.Size(240, 24);
            this.lblTotalTime.TabIndex = 18;
            this.lblTotalTime.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.lblTotalTime.UseMnemonic = false;
            // 
            // label8
            // 
            this.label8.ForeColor = System.Drawing.Color.DarkRed;
            this.label8.Location = new System.Drawing.Point(8, 24);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(32, 24);
            this.label8.TabIndex = 17;
            this.label8.Text = "Time";
            this.label8.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.label8.UseMnemonic = false;
            // 
            // lblTime
            // 
            this.lblTime.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblTime.ForeColor = System.Drawing.Color.DarkRed;
            this.lblTime.Location = new System.Drawing.Point(48, 24);
            this.lblTime.Name = "lblTime";
            this.lblTime.Size = new System.Drawing.Size(136, 24);
            this.lblTime.TabIndex = 16;
            this.lblTime.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.lblTime.UseMnemonic = false;
            // 
            // lblCount
            // 
            this.lblCount.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblCount.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(178)), true);
            this.lblCount.ForeColor = System.Drawing.Color.DarkRed;
            this.lblCount.Location = new System.Drawing.Point(565, 373);
            this.lblCount.Name = "lblCount";
            this.lblCount.Size = new System.Drawing.Size(140, 16);
            this.lblCount.TabIndex = 18;
            this.lblCount.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.lblCount.UseMnemonic = false;
            // 
            // btnAbout
            // 
            this.btnAbout.BackColor = System.Drawing.Color.Transparent;
            this.btnAbout.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnAbout.Location = new System.Drawing.Point(872, 9);
            this.btnAbout.Name = "btnAbout";
            this.btnAbout.Size = new System.Drawing.Size(64, 27);
            this.btnAbout.TabIndex = 19;
            this.btnAbout.Text = "&About";
            this.btnAbout.UseVisualStyleBackColor = false;
            this.btnAbout.Click += new System.EventHandler(this.btnAbout_Click);
            // 
            // MainForm
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
            this.ClientSize = new System.Drawing.Size(942, 405);
            this.Controls.Add(this.btnAbout);
            this.Controls.Add(this.lblCount);
            this.Controls.Add(this.btnStart);
            this.Controls.Add(this.btnStop);
            this.Controls.Add(this.groupBoxImageProcTime);
            this.Controls.Add(this.stillPanel);
            this.Controls.Add(this.videoPanel);
            this.Controls.Add(this.toolBar);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Fixed3D;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "MainForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "WebCam";
            this.Activated += new System.EventHandler(this.MainForm_Activated);
            this.Closing += new System.ComponentModel.CancelEventHandler(this.MainForm_Closing);
            this.Load += new System.EventHandler(this.MainForm_Load);
            this.stillPanel.ResumeLayout(false);
            this.stillPanel.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.trckBarTimer)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.trckBarSaveValue)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox)).EndInit();
            this.groupBoxImageProcTime.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }
#endregion

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.Run(new MainForm());
        }

        private void MainForm_Closing(object sender, CancelEventArgs e)
        {
            Hide();
            CloseInterfaces();
        }

        /// <summary> detect first form appearance, start grabber. </summary>
        private void MainForm_Activated(object sender, EventArgs e)
        {
            if (firstActive) return;
            firstActive = true;
            if (!DsUtils.IsCorrectDirectXVersion())
            {
                MessageBox.Show(this, "DirectX 8.1 NOT installed!", "DirectShow.NET", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                Close(); return;
            }
            if (!DsDev.GetDevicesOfCat(FilterCategory.VideoInputDevice, out capDevices))
            {
                MessageBox.Show(this, "No video capture devices found!", "DirectShow.NET", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                Close(); return;
            }
            DsDevice dev = null;
            if (capDevices.Count == 1) dev = capDevices[0] as DsDevice;
            else
            {
                DeviceSelector selector = new DeviceSelector(capDevices);
                selector.ShowDialog(this);
                dev = selector.SelectedDevice;
            }
            if (dev == null)
            {
                Close(); return;
            }
            if (!StartupVideo(dev.Mon)) Close();
            ToolBarButtonClickEventArgs ee = new ToolBarButtonClickEventArgs(toolBarBtnGrab);
            toolBar_ButtonClick(toolBarBtnGrab, ee);
        }

        private void videoPanel_Resize(object sender, EventArgs e)
        {
            ResizeVideoWindow();
        }

        /// <summary> handler for toolbar button clicks. </summary>
        private void toolBar_ButtonClick(object sender, ToolBarButtonClickEventArgs e)
        {
#if DEBUG
            Console.WriteLine("!!BTN: toolBar_ButtonClick");
#endif
            int hr;
            if (sampGrabber == null) return;
            if (e.Button == toolBarBtnGrab)
            {
#if DEBUG
                Console.WriteLine("!!BTN: toolBarBtnGrab");
#endif
                if (savedArray == null)
                {
                    int size = videoInfoHeader.BmiHeader.ImageSize;
                    if (size < 1000 || size > 16000000) return;
                    savedArray = new byte[size + 64000];
                }
                toolBarBtnSave.Enabled = true;
                toolBarBtnView.Enabled = true;
                toolBarBtnConfig.Enabled = true;
                Image old = pictureBox.Image;
                pictureBox.Image = null;
                if (old != null) old.Dispose();
                toolBarBtnGrab.Enabled = false;
                captured = false;
                hr = sampGrabber.SetCallback(this, 1);
            }
            else if (e.Button == toolBarBtnSave)
            {
#if DEBUG
                Console.WriteLine("!!BTN: toolBarBtnSave");
#endif
                try
                {
                    if (savedArray == null)
                    {
                        int size = videoInfoHeader.BmiHeader.ImageSize;
                        if (size < 1000 || size > 16000000) return;
                        savedArray = new byte[size + 64000];
                    }
                    toolBarBtnSave.Enabled = true;
                    toolBarBtnView.Enabled = true;
                    toolBarBtnConfig.Enabled = true;
                    Image old = pictureBox.Image;
                    if (old != null) old.Dispose();
                    toolBarBtnGrab.Enabled = true;
                    captured = false;
                    hr = sampGrabber.SetCallback(this, 1);
                    SaveFileDialog sd = new SaveFileDialog();
                    sd.FileName = @"WebCam-Wanted.jpg";
                    sd.Title = "Save Image as...";
                    sd.Filter = "JPEG file (*.jpg)|*.jpeg";
                    sd.FilterIndex = 1;
                    if (sd.ShowDialog() != DialogResult.OK) return;
                    pictureBox1.Image.Save(sd.FileName, ImageFormat.Jpeg);
                }
                catch { }
            }
            else if (e.Button == toolBarBtnTune)
            {
                if (capGraph != null) DsUtils.ShowTunerPinDialog(capGraph, capFilter, Handle);
            }
            else if (e.Button == toolBarBtnView)
            {
                try
                {
                    Process.Start(path);
                }
                catch (Exception ep)
                {
#if DEBUG
                    Console.WriteLine("!!VIEW: ", ep.ToString());
#endif
                }
            }
            else if (e.Button == toolBarBtnConfig)
            {
                OpenFileDialog folderBrowser = new OpenFileDialog();
                folderBrowser.InitialDirectory = path;
                folderBrowser.ValidateNames = false;
                folderBrowser.CheckFileExists = false;
                folderBrowser.CheckPathExists = true;
                folderBrowser.RestoreDirectory = true;
                folderBrowser.Title = "Folder Selection";
                folderBrowser.FileName = "Folder Selection";
                if (folderBrowser.ShowDialog() == DialogResult.OK)
                {
                    try
                    {
                        string folderPath = Path.GetDirectoryName(folderBrowser.FileName);
#if DEBUG
                        Console.WriteLine("!!CONF: folderPath: " + folderPath);
#endif
                        // Create a custom configuration section.
                        CustomSection customSection = new CustomSection();
                        // Add an entry to appSettings section.
                        string key = "path";
                        string newValue = folderPath;
                        config.AppSettings.Settings.Remove(key);
                        config.AppSettings.Settings.Add(key, newValue);
                        // Save the configuration file.
                        customSection.SectionInformation.ForceSave = true;
                        config.Save(ConfigurationSaveMode.Full);
#if DEBUG
                        Console.WriteLine("!!CONF: Created configuration file: {0}", config.FilePath);
#endif
                        path = newValue;
                        BootWatcher(path);
                    }
                    catch (ConfigurationErrorsException err)
                    {
#if DEBUG
                        Console.WriteLine("!!CONF: CreateConfigurationFile: {0}", err.ToString());
#endif
                    }
                }
            }
        }

        /// <summary> capture event, triggered by buffer callback. </summary>
        void OnCaptureDone()
        {
#if DEBUG
            Console.WriteLine("!!DLG: OnCaptureDone");
#endif
            try
            {
                toolBarBtnGrab.Enabled = true;
                int hr;
                if (sampGrabber == null) return;
                hr = sampGrabber.SetCallback(null, 0);
                int w = videoInfoHeader.BmiHeader.Width;
                int h = videoInfoHeader.BmiHeader.Height;
                if ((w & 0x03) != 0 || w < 32 || w > 4096 || h < 32 || h > 4096) return;
                //get Image
                int stride = w * 3;
                GCHandle handle = GCHandle.Alloc(savedArray, GCHandleType.Pinned);
                int scan0 = (int)handle.AddrOfPinnedObject();
                scan0 += (h - 1) * stride;
                b = new Bitmap(w, h, -stride, PixelFormat.Format24bppRgb, (IntPtr)scan0);
                handle.Free();
                pictureBox1.Image = b;
                b = new Bitmap(pictureBox1.Image);
                if (firstRun)
                {
                    firstRun = false;
                    pictureBox.Image = b;
                    a = new Bitmap(b);
                }
                else
                {
                    count++;
                    wanted = new Bitmap(b.Width, b.Height);
                    int percent = 0;
                    counter.Start();
                    ImageProcessing imageProcessing = new ImageProcessing(a, b, wanted);
                    imageProcessing.CompareUnsafeFaster(out percent);
                    percent = percent * 100 / (b.Width * b.Height);
                    counter.Stop();
                    pictureBox2.Image = wanted;
                    if (percent >= cPercent && !dontSaveNext)
                    {
                        DateTime currentDateTime = DateTime.Now;
                        saveTime = count + "-" + currentDateTime.ToString("MM_dd_yyyy_HH_mm_sss") + ".jpg";
                        b.Save(path + @"\img_" + saveTime, ImageFormat.Jpeg);
                        a.Save(path + @"\img_" + saveTime, ImageFormat.Jpeg);
                        count2++;
                        Console.Beep(1760, 100);
                        dontSaveNext = true;
                        lblSaveOrNot.Text = "No SaveNext";
                    }
                    else
                    {
                        dontSaveNext = false;
                        lblSaveOrNot.Text = "SaveNext";
                    }
                    lblTime.Text = counter.ToString();
                    sec += counter.Seconds;
                    lblTotalTime.Text = sec.ToString() + " Seconds.";
                    counter.Clear();
                    lblCount.Text = count2 + "/" + count;
                    lblPercent.Text = percent + "%";
                    //the below code calculates the minimum save percent
                    //because the difference between webcams & daylight
                    if (percent - minSave > 5)
                    {
                        try
                        {
                            trckBarSaveValue.Value = percent + 5;
                            minSave = percent;
                        }
                        catch { }
                    }
                    //else if(percent<minSave)
                    try
                    {
                        minSave = (minSave + percent) / 2;
                        trckBarSaveValue.Value = minSave + 5;
                    }
                    catch { }
                }
                pictureBox.Image = b;
                a = new Bitmap(pictureBox.Image);
                savedArray = null;
            }
            catch { }
        }

        /// <summary> start all the interfaces, graphs and preview window. </summary>
#pragma warning disable CS0618 // 'UCOMIMoniker' is obsolete: 'Use System.Runtime.InteropServices.ComTypes.IMoniker instead. http://go.microsoft.com/fwlink/?linkid=14202'
        bool StartupVideo(UCOMIMoniker mon)
#pragma warning restore CS0618 // 'UCOMIMoniker' is obsolete: 'Use System.Runtime.InteropServices.ComTypes.IMoniker instead. http://go.microsoft.com/fwlink/?linkid=14202'
        {
            int hr;
            try
            {
                if (!CreateCaptureDevice(mon)) return false;
                if (!GetInterfaces()) return false;
                if (!SetupGraph()) return false;
                if (!SetupVideoWindow()) return false;
#if DEBUG
				DsROT.AddGraphToRot( graphBuilder, out rotCookie );		// graphBuilder capGraph
#endif
                hr = mediaCtrl.Run();
                if (hr < 0) Marshal.ThrowExceptionForHR(hr);
                bool hasTuner = DsUtils.ShowTunerPinDialog(capGraph, capFilter, Handle);
                toolBarBtnTune.Enabled = hasTuner;
                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <summary> make the video preview window to show in videoPanel. </summary>
        bool SetupVideoWindow()
        {
            int hr;
            try
            {
                // Set the video window to be a child of the main window
                hr = videoWin.put_Owner(videoPanel.Handle);
                if (hr < 0) Marshal.ThrowExceptionForHR(hr);
                // Set video window style
                hr = videoWin.put_WindowStyle(WS_CHILD | WS_CLIPCHILDREN);
                if (hr < 0) Marshal.ThrowExceptionForHR(hr);
                // Use helper function to position video window in client rect of owner window
                ResizeVideoWindow();
                // Make the video window visible, now that it is properly positioned
                hr = videoWin.put_Visible(DsHlp.OATRUE);
                if (hr < 0) Marshal.ThrowExceptionForHR(hr);
                hr = mediaEvt.SetNotifyWindow(Handle, WM_GRAPHNOTIFY, IntPtr.Zero);
                if (hr < 0) Marshal.ThrowExceptionForHR(hr);
                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <summary> build the capture graph for grabber. </summary>
        bool SetupGraph()
        {
            int hr;
            try
            {
                hr = capGraph.SetFiltergraph(graphBuilder);
                if (hr < 0) Marshal.ThrowExceptionForHR(hr);
                hr = graphBuilder.AddFilter(capFilter, "Ds.NET Video Capture Device");
                if (hr < 0) Marshal.ThrowExceptionForHR(hr);
                //DsUtils.ShowCapPinDialog(capGraph, capFilter, Handle);
                AMMediaType media = new AMMediaType();
                media.majorType = MediaType.Video;
                media.subType = MediaSubType.RGB24;
                media.formatType = FormatType.VideoInfo;
                hr = sampGrabber.SetMediaType(media);
                if (hr < 0) Marshal.ThrowExceptionForHR(hr);
                hr = graphBuilder.AddFilter(baseGrabFlt, "Ds.NET Grabber");
                if (hr < 0) Marshal.ThrowExceptionForHR(hr);
                Guid cat = PinCategory.Preview;
                Guid med = MediaType.Video;
                hr = capGraph.RenderStream(ref cat, ref med, capFilter, null, null); // baseGrabFlt 
                if (hr < 0) Marshal.ThrowExceptionForHR(hr);
                cat = PinCategory.Capture;
                med = MediaType.Video;
                hr = capGraph.RenderStream(ref cat, ref med, capFilter, null, baseGrabFlt); // baseGrabFlt 
                if (hr < 0) Marshal.ThrowExceptionForHR(hr);
                media = new AMMediaType();
                hr = sampGrabber.GetConnectedMediaType(media);
                if (hr < 0) Marshal.ThrowExceptionForHR(hr);
                if (media.formatType != FormatType.VideoInfo || media.formatPtr == IntPtr.Zero)
                    throw new NotSupportedException("Unknown Grabber Media Format");
                videoInfoHeader = (VideoInfoHeader)Marshal.PtrToStructure(media.formatPtr, typeof(VideoInfoHeader));
                Marshal.FreeCoTaskMem(media.formatPtr); media.formatPtr = IntPtr.Zero;
                hr = sampGrabber.SetBufferSamples(false);
                if (hr == 0) hr = sampGrabber.SetOneShot(false);
                if (hr == 0) hr = sampGrabber.SetCallback(null, 0);
                if (hr < 0) Marshal.ThrowExceptionForHR(hr);
                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <summary> create the used COM components and get the interfaces. </summary>
        bool GetInterfaces()
        {
            Type comType = null;
            object comObj = null;
            try
            {
                comType = Type.GetTypeFromCLSID(Clsid.FilterGraph);
                if (comType == null)
                    throw new NotImplementedException(@"DirectShow FilterGraph not installed/registered!");
                comObj = Activator.CreateInstance(comType);
                graphBuilder = (IGraphBuilder)comObj; comObj = null;
                Guid clsid = Clsid.CaptureGraphBuilder2;
                Guid riid = typeof(ICaptureGraphBuilder2).GUID;
                comObj = DsBugWO.CreateDsInstance(ref clsid, ref riid);
                capGraph = (ICaptureGraphBuilder2)comObj; comObj = null;
                comType = Type.GetTypeFromCLSID(Clsid.SampleGrabber);
                if (comType == null)
                    throw new NotImplementedException(@"DirectShow SampleGrabber not installed/registered!");
                comObj = Activator.CreateInstance(comType);
                sampGrabber = (ISampleGrabber)comObj; comObj = null;
                mediaCtrl = (IMediaControl)graphBuilder;
                videoWin = (IVideoWindow)graphBuilder;
                mediaEvt = (IMediaEventEx)graphBuilder;
                baseGrabFlt = (IBaseFilter)sampGrabber;
                return true;
            }
            catch
            {
                return false;
            }
            finally
            {
                if (comObj != null) Marshal.ReleaseComObject(comObj); comObj = null;
            }
        }
        /// <summary> create the user selected capture device. </summary>
#pragma warning disable CS0618 // 'UCOMIMoniker' is obsolete: 'Use System.Runtime.InteropServices.ComTypes.IMoniker instead. http://go.microsoft.com/fwlink/?linkid=14202'
        bool CreateCaptureDevice(UCOMIMoniker mon)
#pragma warning restore CS0618 // 'UCOMIMoniker' is obsolete: 'Use System.Runtime.InteropServices.ComTypes.IMoniker instead. http://go.microsoft.com/fwlink/?linkid=14202'
        {
            object capObj = null;
            try
            {
                Guid gbf = typeof(IBaseFilter).GUID;
                mon.BindToObject(null, null, ref gbf, out capObj);
                capFilter = (IBaseFilter)capObj; capObj = null;
                return true;
            }
            catch
            {
                return false;
            }
            finally
            {
                if (capObj != null) Marshal.ReleaseComObject(capObj); capObj = null;
            }
        }

        /// <summary> do cleanup and release DirectShow. </summary>
        void CloseInterfaces()
        {
            int hr;
            try
            {
#if DEBUG
				if( rotCookie != 0 )
					DsROT.RemoveGraphFromRot( ref rotCookie );
#endif
                if (mediaCtrl != null)
                {
                    hr = mediaCtrl.Stop();
                    mediaCtrl = null;
                }
                if (mediaEvt != null)
                {
                    hr = mediaEvt.SetNotifyWindow(IntPtr.Zero, WM_GRAPHNOTIFY, IntPtr.Zero);
                    mediaEvt = null;
                }
                if (videoWin != null)
                {
                    hr = videoWin.put_Visible(DsHlp.OAFALSE);
                    hr = videoWin.put_Owner(IntPtr.Zero);
                    videoWin = null;
                }
                baseGrabFlt = null;
                if (sampGrabber != null) Marshal.ReleaseComObject(sampGrabber); sampGrabber = null;
                if (capGraph != null) Marshal.ReleaseComObject(capGraph); capGraph = null;
                if (graphBuilder != null) Marshal.ReleaseComObject(graphBuilder); graphBuilder = null;
                if (capFilter != null) Marshal.ReleaseComObject(capFilter); capFilter = null;
                if (capDevices != null)
                {
                    foreach (DsDevice d in capDevices) d.Dispose();
                    capDevices = null;
                }
            }
            catch { }
        }

        /// <summary> resize preview video window to fill client area. </summary>
        void ResizeVideoWindow()
        {
            if (videoWin != null)
            {
                Rectangle rc = videoPanel.ClientRectangle;
                videoWin.SetWindowPosition(0, 0, rc.Right, rc.Bottom);
            }
        }

        /// <summary> override window fn to handle graph events. </summary>
        protected override void WndProc(ref Message m)
        {
            if (m.Msg == WM_GRAPHNOTIFY)
            {
                if (mediaEvt != null) OnGraphNotify();
                return;
            }
            base.WndProc(ref m);
        }

        /// <summary> graph event (WM_GRAPHNOTIFY) handler. </summary>
        void OnGraphNotify()
        {
            DsEvCode code;
            int p1, p2, hr = 0;
            try
            {
                do
                {
                    hr = mediaEvt.GetEvent(out code, out p1, out p2, 0);
                    if (hr < 0) break;
                    hr = mediaEvt.FreeEventParams(code, p1, p2);
                }
                while (hr == 0);
            }
            catch { }
        }

        /// <summary> sample callback, NOT USED. </summary>
        int ISampleGrabberCB.SampleCB(double SampleTime, IMediaSample pSample)
        {
#if DEBUG
            Console.WriteLine("!!CB: ISampleGrabberCB.SampleCB");
#endif
            return 0;
        }

        /// <summary> buffer callback, COULD BE FROM FOREIGN THREAD. </summary>
        int ISampleGrabberCB.BufferCB(double SampleTime, IntPtr pBuffer, int BufferLen)
        {
            if (captured || savedArray == null)
            {
#if DEBUG
                Console.WriteLine("!!CB: ISampleGrabberCB.BufferCB");
#endif
                return 0;
            }
            captured = true;
            bufferedSize = BufferLen;
#if DEBUG
            Console.WriteLine("!!CB: ISampleGrabberCB.BufferCB  !GRAB! size = " + BufferLen.ToString());
#endif
            if (pBuffer != IntPtr.Zero && BufferLen > 1000 && BufferLen <= savedArray.Length)
                Marshal.Copy(pBuffer, savedArray, 0, BufferLen);
            //else Console.WriteLine("!!GRAB: failed ");
            BeginInvoke(new CaptureDone(OnCaptureDone));
            return 0;
        }

        /// <summary> flag to detect first Form appearance </summary>
        private bool firstActive;
        /// <summary> base filter of the actually used video devices. </summary>
        private IBaseFilter capFilter;
        /// <summary> graph builder interface. </summary>
        private IGraphBuilder graphBuilder;
        /// <summary> capture graph builder interface. </summary>
        private ICaptureGraphBuilder2 capGraph;
        private ISampleGrabber sampGrabber;
        /// <summary> control interface. </summary>
        private IMediaControl mediaCtrl;
        /// <summary> event interface. </summary>
        private IMediaEventEx mediaEvt;
        /// <summary> video window interface. </summary>
        private IVideoWindow videoWin;
        /// <summary> grabber filter interface. </summary>
        private IBaseFilter baseGrabFlt;
        /// <summary> structure describing the bitmap to grab. </summary>
        private VideoInfoHeader videoInfoHeader;
        private bool captured = true;
        private int bufferedSize;
        /// <summary> buffer for bitmap data. </summary>
        private byte[] savedArray;
        /// <summary> list of installed video devices. </summary>
        private ArrayList capDevices;
        private const int WM_GRAPHNOTIFY = 0x00008001;  // message from graph
        private const int WS_CHILD = 0x40000000;    // attributes for video window
        private const int WS_CLIPCHILDREN = 0x02000000;
        private const int WS_CLIPSIBLINGS = 0x04000000;
        /// <summary> event when callback has finished (ISampleGrabberCB.BufferCB). </summary>
        private delegate void CaptureDone();
#if DEBUG
		private int		rotCookie = 0;
#endif
        private void timer1_Tick(object sender, EventArgs e)
        {
            ToolBarButtonClickEventArgs ee = new ToolBarButtonClickEventArgs(toolBarBtnGrab);
            toolBar_ButtonClick(toolBarBtnGrab, ee);
        }

        private void btnStart_Click(object sender, EventArgs e)
        {
            btnStart.Enabled = false;
            btnStop.Enabled = true;
            timer1.Enabled = true;
        }

        private void btnGrab_Click(object sender, EventArgs e)
        {
            ToolBarButtonClickEventArgs ee = new ToolBarButtonClickEventArgs(toolBarBtnGrab);
            toolBar_ButtonClick(toolBarBtnGrab, ee);
        }

        private void btnView_Click(object sender, EventArgs e)
        {
            ToolBarButtonClickEventArgs ee = new ToolBarButtonClickEventArgs(toolBarBtnView);
            toolBar_ButtonClick(toolBarBtnView, ee);
        }

        private void btnPath_Click(object sender, EventArgs e)
        {
            ToolBarButtonClickEventArgs ee = new ToolBarButtonClickEventArgs(toolBarBtnConfig);
            toolBar_ButtonClick(toolBarBtnConfig, ee);
        }

        private void pictureBox_Click(object sender, EventArgs e)
        {
        }

        private void videoPanel_Paint(object sender, PaintEventArgs e)
        {
        }

        private void trckBarSaveValue_Scroll(object sender, EventArgs e)
        {
        }

        private void lblPercent_Click(object sender, EventArgs e)
        {
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
        }

        private void splitter1_SplitterMoved(object sender, SplitterEventArgs e)
        {

        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {
        }

        private void trckBarSaveValue_ValueChanged(object sender, EventArgs e)
        {
            cPercent = trckBarSaveValue.Value;
            lblSavePercent.Text = cPercent + "%";
        }

        private void btnStop_Click(object sender, EventArgs e)
        {
            btnStart.Enabled = true;
            btnStop.Enabled = false;
            timer1.Enabled = false;
            firstRun = true;
        }

        private void btnAbout_Click(object sender, EventArgs e)
        {
            About about = new About();
            about.ShowDialog(this);
        }

        private void trckBarTimer_ValueChanged(object sender, EventArgs e)
        {
            timer1.Interval = trckBarTimer.Value;
            lblTimerValue.Text = trckBarTimer.Value.ToString();
        }
    }

    internal enum PlayState
    {
        Init, Stopped, Paused, Running
    }
}
