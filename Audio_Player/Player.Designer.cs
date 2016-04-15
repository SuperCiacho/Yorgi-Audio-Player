namespace Audio_Player
{
    partial class Player
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
            this.components = new System.ComponentModel.Container();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Player));
            this.Playlist = new System.Windows.Forms.DataGridView();
            this.LP_column = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Title_column = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Time_column = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.TitleLabel = new System.Windows.Forms.Label();
            this.Zegar = new System.Windows.Forms.TextBox();
            this.Clock = new System.Windows.Forms.Timer(this.components);
            this.Open_File = new System.Windows.Forms.OpenFileDialog();
            this.tooltip = new System.Windows.Forms.ToolTip(this.components);
            this.TrackBar = new System.Windows.Forms.TrackBar();
            this.statusStrip = new System.Windows.Forms.StatusStrip();
            this.toolStrip_Playlist_Button = new System.Windows.Forms.ToolStripSplitButton();
            this.dodajToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.plikiToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.katalogToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.playlistToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.nowaToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.wczytajToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.zapiszToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.Playlist_Context_Menu = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.Context_Play = new System.Windows.Forms.ToolStripMenuItem();
            this.Context_File_Info = new System.Windows.Forms.ToolStripMenuItem();
            this.Context_Delete = new System.Windows.Forms.ToolStripMenuItem();
            this.Mute_button = new System.Windows.Forms.Button();
            this.PauseButton = new System.Windows.Forms.Button();
            this.Next_button = new System.Windows.Forms.Button();
            this.Prev_button = new System.Windows.Forms.Button();
            this.Open_button = new System.Windows.Forms.Button();
            this.Stop_button = new System.Windows.Forms.Button();
            this.PlayButton = new System.Windows.Forms.Button();
            this.pictureBox = new System.Windows.Forms.PictureBox();
            this.ShuffleBox = new System.Windows.Forms.CheckBox();
            this.VolumeBar = new System.Windows.Forms.HScrollBar();
            this.Open_Folder = new System.Windows.Forms.FolderBrowserDialog();
            ((System.ComponentModel.ISupportInitialize)(this.Playlist)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.TrackBar)).BeginInit();
            this.statusStrip.SuspendLayout();
            this.Playlist_Context_Menu.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox)).BeginInit();
            this.SuspendLayout();
            // 
            // Playlist
            // 
            this.Playlist.AllowDrop = true;
            this.Playlist.AllowUserToAddRows = false;
            this.Playlist.AllowUserToDeleteRows = false;
            this.Playlist.AllowUserToResizeColumns = false;
            this.Playlist.AllowUserToResizeRows = false;
            this.Playlist.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.Playlist.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.Playlist.CellBorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.SingleHorizontal;
            this.Playlist.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.Playlist.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.LP_column,
            this.Title_column,
            this.Time_column});
            this.Playlist.EditMode = System.Windows.Forms.DataGridViewEditMode.EditOnKeystroke;
            this.Playlist.Location = new System.Drawing.Point(0, 141);
            this.Playlist.MultiSelect = false;
            this.Playlist.Name = "Playlist";
            this.Playlist.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.Playlist.Size = new System.Drawing.Size(634, 245);
            this.Playlist.TabIndex = 0;
            this.Playlist.CellMouseClick += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.Playlist_CellMouseClick);
            this.Playlist.DragDrop += new System.Windows.Forms.DragEventHandler(this.Playlist_DragDrop);
            this.Playlist.DragEnter += new System.Windows.Forms.DragEventHandler(this.Playlist_DragEnter);
            this.Playlist.KeyDown += new System.Windows.Forms.KeyEventHandler(this.Playlist_KeyDown);
            this.Playlist.KeyUp += new System.Windows.Forms.KeyEventHandler(this.Playlist_KeyUp);
            this.Playlist.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.Playlist_MouseClick);
            // 
            // LP_column
            // 
            this.LP_column.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            this.LP_column.DefaultCellStyle = dataGridViewCellStyle1;
            this.LP_column.FillWeight = 30F;
            this.LP_column.HeaderText = "LP";
            this.LP_column.Name = "LP_column";
            this.LP_column.Width = 45;
            // 
            // Title_column
            // 
            this.Title_column.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.Title_column.HeaderText = "Tytuł";
            this.Title_column.Name = "Title_column";
            this.Title_column.ReadOnly = true;
            this.Title_column.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // Time_column
            // 
            this.Time_column.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            this.Time_column.DefaultCellStyle = dataGridViewCellStyle2;
            this.Time_column.FillWeight = 30F;
            this.Time_column.HeaderText = "Czas";
            this.Time_column.Name = "Time_column";
            this.Time_column.ReadOnly = true;
            this.Time_column.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.Time_column.Width = 36;
            // 
            // TitleLabel
            // 
            this.TitleLabel.AutoSize = true;
            this.TitleLabel.BackColor = System.Drawing.Color.MistyRose;
            this.TitleLabel.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.TitleLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.TitleLabel.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.TitleLabel.Location = new System.Drawing.Point(129, 25);
            this.TitleLabel.Name = "TitleLabel";
            this.TitleLabel.Size = new System.Drawing.Size(122, 18);
            this.TitleLabel.TabIndex = 4;
            this.TitleLabel.Text = "Yorgi Audio Player";
            // 
            // Zegar
            // 
            this.Zegar.BackColor = System.Drawing.SystemColors.GrayText;
            this.Zegar.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.Zegar.ForeColor = System.Drawing.SystemColors.Info;
            this.Zegar.Location = new System.Drawing.Point(13, 19);
            this.Zegar.Name = "Zegar";
            this.Zegar.ReadOnly = true;
            this.Zegar.Size = new System.Drawing.Size(110, 29);
            this.Zegar.TabIndex = 8;
            this.Zegar.Text = ">> Witaj <<";
            this.Zegar.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // Clock
            // 
            this.Clock.Interval = 1000;
            this.Clock.Tick += new System.EventHandler(this.czas_Tick);
            // 
            // Open_File
            // 
            this.Open_File.Filter = "Obsługiwane formaty|*.mp3; *.wav; *.ogg;|MP3 Files|*.mp3|WAV Files|*.wav|OGG File" +
    "s|*.ogg";
            this.Open_File.Multiselect = true;
            // 
            // tooltip
            // 
            this.tooltip.AutomaticDelay = 1;
            this.tooltip.ShowAlways = true;
            this.tooltip.UseFading = false;
            // 
            // TrackBar
            // 
            this.TrackBar.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.TrackBar.Location = new System.Drawing.Point(11, 54);
            this.TrackBar.Name = "TrackBar";
            this.TrackBar.Size = new System.Drawing.Size(459, 45);
            this.TrackBar.TabIndex = 11;
            this.TrackBar.TickStyle = System.Windows.Forms.TickStyle.Both;
            this.TrackBar.ValueChanged += new System.EventHandler(this.progressBar_ValueChanged);
            // 
            // statusStrip
            // 
            this.statusStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStrip_Playlist_Button});
            this.statusStrip.Location = new System.Drawing.Point(0, 389);
            this.statusStrip.Name = "statusStrip";
            this.statusStrip.Size = new System.Drawing.Size(634, 22);
            this.statusStrip.SizingGrip = false;
            this.statusStrip.TabIndex = 14;
            this.statusStrip.Text = "statusStrip1";
            // 
            // toolStrip_Playlist_Button
            // 
            this.toolStrip_Playlist_Button.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStrip_Playlist_Button.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.dodajToolStripMenuItem,
            this.playlistToolStripMenuItem});
            this.toolStrip_Playlist_Button.Image = ((System.Drawing.Image)(resources.GetObject("toolStrip_Playlist_Button.Image")));
            this.toolStrip_Playlist_Button.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStrip_Playlist_Button.Name = "toolStrip_Playlist_Button";
            this.toolStrip_Playlist_Button.Size = new System.Drawing.Size(32, 20);
            this.toolStrip_Playlist_Button.Text = "toolStripSplitButton1";
            this.toolStrip_Playlist_Button.ToolTipText = "toolStrip_Playlist_Button";
            this.toolStrip_Playlist_Button.ButtonClick += new System.EventHandler(this.toolStrip_Playlist_Button_ButtonClick);
            // 
            // dodajToolStripMenuItem
            // 
            this.dodajToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.plikiToolStripMenuItem,
            this.katalogToolStripMenuItem});
            this.dodajToolStripMenuItem.Name = "dodajToolStripMenuItem";
            this.dodajToolStripMenuItem.Size = new System.Drawing.Size(111, 22);
            this.dodajToolStripMenuItem.Text = "Dodaj";
            // 
            // plikiToolStripMenuItem
            // 
            this.plikiToolStripMenuItem.Name = "plikiToolStripMenuItem";
            this.plikiToolStripMenuItem.Size = new System.Drawing.Size(114, 22);
            this.plikiToolStripMenuItem.Text = "Plik(i)";
            this.plikiToolStripMenuItem.Click += new System.EventHandler(this.plikiToolStripMenuItem_Click);
            // 
            // katalogToolStripMenuItem
            // 
            this.katalogToolStripMenuItem.Name = "katalogToolStripMenuItem";
            this.katalogToolStripMenuItem.Size = new System.Drawing.Size(114, 22);
            this.katalogToolStripMenuItem.Text = "Katalog";
            this.katalogToolStripMenuItem.Click += new System.EventHandler(this.katalogToolStripMenuItem_Click);
            // 
            // playlistToolStripMenuItem
            // 
            this.playlistToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.nowaToolStripMenuItem,
            this.wczytajToolStripMenuItem,
            this.zapiszToolStripMenuItem});
            this.playlistToolStripMenuItem.Name = "playlistToolStripMenuItem";
            this.playlistToolStripMenuItem.Size = new System.Drawing.Size(111, 22);
            this.playlistToolStripMenuItem.Text = "Playlist";
            // 
            // nowaToolStripMenuItem
            // 
            this.nowaToolStripMenuItem.Name = "nowaToolStripMenuItem";
            this.nowaToolStripMenuItem.Size = new System.Drawing.Size(115, 22);
            this.nowaToolStripMenuItem.Text = "Nowa";
            this.nowaToolStripMenuItem.Click += new System.EventHandler(this.nowaToolStripMenuItem_Click);
            // 
            // wczytajToolStripMenuItem
            // 
            this.wczytajToolStripMenuItem.Name = "wczytajToolStripMenuItem";
            this.wczytajToolStripMenuItem.Size = new System.Drawing.Size(115, 22);
            this.wczytajToolStripMenuItem.Text = "Load";
            this.wczytajToolStripMenuItem.Click += new System.EventHandler(this.wczytajToolStripMenuItem_Click);
            // 
            // zapiszToolStripMenuItem
            // 
            this.zapiszToolStripMenuItem.Name = "zapiszToolStripMenuItem";
            this.zapiszToolStripMenuItem.Size = new System.Drawing.Size(115, 22);
            this.zapiszToolStripMenuItem.Text = "Save";
            this.zapiszToolStripMenuItem.Click += new System.EventHandler(this.zapiszToolStripMenuItem_Click);
            // 
            // Playlist_Context_Menu
            // 
            this.Playlist_Context_Menu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.Context_Play,
            this.Context_File_Info,
            this.Context_Delete});
            this.Playlist_Context_Menu.Name = "contextMenuStrip1";
            this.Playlist_Context_Menu.Size = new System.Drawing.Size(135, 70);
            // 
            // Context_Play
            // 
            this.Context_Play.Name = "Context_Play";
            this.Context_Play.Size = new System.Drawing.Size(134, 22);
            this.Context_Play.Text = "Odtwórz";
            this.Context_Play.Click += new System.EventHandler(this.Context_Play_Click);
            // 
            // Context_File_Info
            // 
            this.Context_File_Info.Name = "Context_File_Info";
            this.Context_File_Info.Size = new System.Drawing.Size(134, 22);
            this.Context_File_Info.Text = "Info o pliku";
            this.Context_File_Info.Click += new System.EventHandler(this.Context_File_Info_Click);
            // 
            // Context_Delete
            // 
            this.Context_Delete.Name = "Context_Delete";
            this.Context_Delete.Size = new System.Drawing.Size(134, 22);
            this.Context_Delete.Text = "Usuń";
            this.Context_Delete.Click += new System.EventHandler(this.Context_Delete_Click);
            // 
            // Mute_button
            // 
            this.Mute_button.Image = ((System.Drawing.Image)(resources.GetObject("Mute_button.Image")));
            this.Mute_button.Location = new System.Drawing.Point(300, 105);
            this.Mute_button.Name = "Mute_button";
            this.Mute_button.Size = new System.Drawing.Size(24, 23);
            this.Mute_button.TabIndex = 13;
            this.Mute_button.UseVisualStyleBackColor = true;
            this.Mute_button.Click += new System.EventHandler(this.OnMuteButtonClick);
            // 
            // PauseButton
            // 
            this.PauseButton.Image = ((System.Drawing.Image)(resources.GetObject("PauseButton.Image")));
            this.PauseButton.Location = new System.Drawing.Point(104, 105);
            this.PauseButton.Name = "PauseButton";
            this.PauseButton.Size = new System.Drawing.Size(40, 23);
            this.PauseButton.TabIndex = 9;
            this.PauseButton.UseVisualStyleBackColor = true;
            this.PauseButton.Click += new System.EventHandler(this.OnPauseButtonClick);
            // 
            // Next_button
            // 
            this.Next_button.Image = ((System.Drawing.Image)(resources.GetObject("Next_button.Image")));
            this.Next_button.Location = new System.Drawing.Point(196, 105);
            this.Next_button.Name = "Next_button";
            this.Next_button.Size = new System.Drawing.Size(40, 23);
            this.Next_button.TabIndex = 6;
            this.Next_button.UseVisualStyleBackColor = true;
            this.Next_button.Click += new System.EventHandler(this.OnNextButtonClick);
            // 
            // Prev_button
            // 
            this.Prev_button.Image = ((System.Drawing.Image)(resources.GetObject("Prev_button.Image")));
            this.Prev_button.Location = new System.Drawing.Point(12, 105);
            this.Prev_button.Name = "Prev_button";
            this.Prev_button.Size = new System.Drawing.Size(40, 23);
            this.Prev_button.TabIndex = 5;
            this.Prev_button.UseVisualStyleBackColor = true;
            this.Prev_button.Click += new System.EventHandler(this.OnPrevButtonClick);
            // 
            // Open_button
            // 
            this.Open_button.Image = ((System.Drawing.Image)(resources.GetObject("Open_button.Image")));
            this.Open_button.Location = new System.Drawing.Point(242, 105);
            this.Open_button.Name = "Open_button";
            this.Open_button.Size = new System.Drawing.Size(40, 23);
            this.Open_button.TabIndex = 3;
            this.Open_button.UseVisualStyleBackColor = true;
            this.Open_button.Click += new System.EventHandler(this.OnOpenButtonClick);
            // 
            // Stop_button
            // 
            this.Stop_button.Image = ((System.Drawing.Image)(resources.GetObject("Stop_button.Image")));
            this.Stop_button.Location = new System.Drawing.Point(150, 105);
            this.Stop_button.Name = "Stop_button";
            this.Stop_button.Size = new System.Drawing.Size(40, 23);
            this.Stop_button.TabIndex = 2;
            this.Stop_button.Text = " ";
            this.Stop_button.UseVisualStyleBackColor = true;
            this.Stop_button.Click += new System.EventHandler(this.OnStopButtonClick);
            // 
            // PlayButton
            // 
            this.PlayButton.Image = ((System.Drawing.Image)(resources.GetObject("PlayButton.Image")));
            this.PlayButton.Location = new System.Drawing.Point(58, 105);
            this.PlayButton.Name = "PlayButton";
            this.PlayButton.Size = new System.Drawing.Size(40, 23);
            this.PlayButton.TabIndex = 1;
            this.PlayButton.UseVisualStyleBackColor = true;
            this.PlayButton.Click += new System.EventHandler(this.OnPlayButtonClick);
            // 
            // pictureBox
            // 
            this.pictureBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.pictureBox.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("pictureBox.BackgroundImage")));
            this.pictureBox.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.pictureBox.InitialImage = null;
            this.pictureBox.Location = new System.Drawing.Point(476, -2);
            this.pictureBox.Name = "pictureBox";
            this.pictureBox.Size = new System.Drawing.Size(158, 148);
            this.pictureBox.TabIndex = 12;
            this.pictureBox.TabStop = false;
            // 
            // ShuffleBox
            // 
            this.ShuffleBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.ShuffleBox.AutoSize = true;
            this.ShuffleBox.Location = new System.Drawing.Point(411, 7);
            this.ShuffleBox.Name = "ShuffleBox";
            this.ShuffleBox.Size = new System.Drawing.Size(59, 17);
            this.ShuffleBox.TabIndex = 15;
            this.ShuffleBox.Text = "Shuffle";
            this.ShuffleBox.UseVisualStyleBackColor = true;
            this.ShuffleBox.CheckStateChanged += new System.EventHandler(this.Shuffle_Box_CheckStateChanged);
            // 
            // VolumeBar
            // 
            this.VolumeBar.LargeChange = 1;
            this.VolumeBar.Location = new System.Drawing.Point(335, 108);
            this.VolumeBar.Name = "VolumeBar";
            this.VolumeBar.Size = new System.Drawing.Size(134, 19);
            this.VolumeBar.TabIndex = 10;
            this.VolumeBar.Value = global::Audio_Player.Properties.Settings.Default.Volume;
            this.VolumeBar.ValueChanged += new System.EventHandler(this.VolumeBar_ValueChanged);
            this.VolumeBar.MouseHover += new System.EventHandler(this.VolumeBar_MouseHover);
            // 
            // Open_Folder
            // 
            this.Open_Folder.ShowNewFolderButton = false;
            // 
            // Player
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.BackColor = System.Drawing.Color.MistyRose;
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.ClientSize = new System.Drawing.Size(634, 411);
            this.Controls.Add(this.statusStrip);
            this.Controls.Add(this.Mute_button);
            this.Controls.Add(this.ShuffleBox);
            this.Controls.Add(this.TrackBar);
            this.Controls.Add(this.VolumeBar);
            this.Controls.Add(this.PauseButton);
            this.Controls.Add(this.Zegar);
            this.Controls.Add(this.Next_button);
            this.Controls.Add(this.Prev_button);
            this.Controls.Add(this.Open_button);
            this.Controls.Add(this.Stop_button);
            this.Controls.Add(this.PlayButton);
            this.Controls.Add(this.Playlist);
            this.Controls.Add(this.TitleLabel);
            this.Controls.Add(this.pictureBox);
            this.Icon = global::Audio_Player.Properties.Resources.media_headphones;
            this.MaximizeBox = false;
            this.MinimumSize = new System.Drawing.Size(650, 450);
            this.Name = "Player";
            this.Text = "Yorgi Audio Player";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Player_FormClosing);
            ((System.ComponentModel.ISupportInitialize)(this.Playlist)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.TrackBar)).EndInit();
            this.statusStrip.ResumeLayout(false);
            this.statusStrip.PerformLayout();
            this.Playlist_Context_Menu.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button PlayButton;
        private System.Windows.Forms.Button Stop_button;
        private System.Windows.Forms.Button Open_button;
        private System.Windows.Forms.Label TitleLabel;
        private System.Windows.Forms.Button Prev_button;
        private System.Windows.Forms.Button Next_button;
        private System.Windows.Forms.TextBox Zegar;
        private System.Windows.Forms.Timer Clock;
        private System.Windows.Forms.OpenFileDialog Open_File;
        private System.Windows.Forms.Button PauseButton;
        private System.Windows.Forms.HScrollBar VolumeBar;
        private System.Windows.Forms.ToolTip tooltip;
        private System.Windows.Forms.TrackBar TrackBar;
        private System.Windows.Forms.PictureBox pictureBox;
        private System.Windows.Forms.Button Mute_button;
        private System.Windows.Forms.StatusStrip statusStrip;
        private System.Windows.Forms.ToolStripSplitButton toolStrip_Playlist_Button;
        private System.Windows.Forms.ToolStripMenuItem playlistToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem nowaToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem wczytajToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem zapiszToolStripMenuItem;
        private System.Windows.Forms.ContextMenuStrip Playlist_Context_Menu;
        private System.Windows.Forms.ToolStripMenuItem Context_Play;
        private System.Windows.Forms.ToolStripMenuItem Context_Delete;
        private System.Windows.Forms.DataGridView Playlist;
        private System.Windows.Forms.CheckBox ShuffleBox;
        private System.Windows.Forms.DataGridViewTextBoxColumn LP_column;
        private System.Windows.Forms.DataGridViewTextBoxColumn Title_column;
        private System.Windows.Forms.DataGridViewTextBoxColumn Time_column;
        private System.Windows.Forms.ToolStripMenuItem dodajToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem plikiToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem katalogToolStripMenuItem;
        private System.Windows.Forms.FolderBrowserDialog Open_Folder;
        private System.Windows.Forms.ToolStripMenuItem Context_File_Info;
        }
}

