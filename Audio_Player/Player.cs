using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;
using Audio_Player.Properties;
using FMOD;

namespace Audio_Player
{
    public partial class Player : Form
    {
        #region Funkcje, zmienne, deklaracje
        #region Zmienne i deklaracje
        private List<string> Lista = new List<string>();

        RESULT result;
        FMOD.System system = new FMOD.System();
        Sound player = new Sound();
        Channel channel = new Channel();

        private int currentSong;
        public int Iterator;
        private int tb_tmp_v, vb_tmp_v;
        private uint pos, length;
        private bool del_state, state;
        Random shuffle;

        #endregion
        #region Funkcje
        private void SongLength(string path)
        {
            try
            {
                system.createSound(path, (MODE.SOFTWARE | MODE._2D | MODE.CREATESTREAM | MODE.OPENONLY), ref player);
                player.getLength(ref length, TIMEUNIT.MS);

                var totalSeconds = TimeSpan.FromMilliseconds(length).TotalSeconds;
                TrackBar.Maximum = totalSeconds < 1 ? 1 : (int)totalSeconds;
            }
            catch { }
            player = null;
            TrackBar.Value = 0;
        }

        private void ReadTags(List<string> paths)
        {
            TAG tag_title = new TAG();
            TAG tag_artist = new TAG();

            foreach (string p in paths)
            {
                try
                {
                    string fileName;
                    bool isWaveFile = p.EndsWith(".wav");

                    if (isWaveFile)
                    {
                        fileName = new DirectoryInfo(p).Name;
                        fileName = fileName.Remove(fileName.Length - 4);
                    }
                    else
                    {
                        fileName = $"{Marshal.PtrToStringAnsi(tag_artist.data)} - {Marshal.PtrToStringAnsi(tag_title.data)}";
                    }

                    PlaylistManager.GetTags(ref tag_artist, ref tag_title, ref this.length, p, ref this.system, isWaveFile);
                    this.Playlist.Rows.Add(this.Iterator + 1, fileName, PlaylistManager.NormalizeTime(this.length));
                    this.Iterator++;
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Błąd", MessageBoxButtons.OK);
                }
            }
        }

        private void AddItem(IEnumerable<string> fileNames)
        {
            List<string> audioFiles = fileNames.Where(s => s.EndsWith(".mp3") || s.EndsWith(".flac") || s.EndsWith(".ogg") || s.EndsWith(".wav")).ToList();
            this.Lista.AddRange(audioFiles.Select(Path.GetFileNameWithoutExtension));
            this.ReadTags(audioFiles);
        }
        private void DeleteItem()
        {
            if (Playlist.RowCount > 0)
            {
                int i = Playlist.CurrentRow.Index;
                Lista.RemoveAt(i);
                Playlist.Rows.Remove(Playlist.CurrentRow);
                Iterator--;
                del_state = false;
                for (; i < Playlist.RowCount; i++)
                {
                    Playlist[0, i].Value = i + 1;
                    if (Playlist.Rows[i].DefaultCellStyle.BackColor == Color.AliceBlue)
                        this.currentSong = (int)Playlist[0, i].Value - 1;
                }
            }
        }
        private void PlayItem(object sender, EventArgs e)
        {
            Playlist.Rows[this.currentSong].DefaultCellStyle.BackColor = Color.White;
            this.currentSong = (int)Playlist.CurrentRow.Cells[0].Value - 1;
            Playlist.Rows[this.currentSong].DefaultCellStyle.BackColor = Color.AliceBlue;
            this.OnPlayButtonClick(sender, e);
        }
        private static bool IsDirectory(string path)
        {
            FileAttributes fa = File.GetAttributes(path);
            return (fa & FileAttributes.Directory) != 0;
        }
        private static List<string> GetFilesList(string path)
        {
            List<string> result = new List<string>();
            Stack<string> stack = new Stack<string>();
            stack.Push(path);
            while (stack.Count > 0)
            {
                string dir = stack.Pop();

                try
                {
                    result.AddRange(Directory.GetFiles(dir, "*.*"));
                    foreach (string dn in Directory.GetDirectories(dir))
                    {
                        stack.Push(dn);
                    }
                }
                catch { }
            }
            return result;
        }
        private void Shufflin()
        {
            Playlist.Rows[this.currentSong].DefaultCellStyle.BackColor = Color.White;

            int newTrackIndex;
            do
            {
                newTrackIndex = shuffle.Next(0, Lista.Count - 1);
            }
            while (this.currentSong == newTrackIndex);
            this.currentSong = newTrackIndex;
            this.TitleLabel.Text = this.TitleLabel.Text = Playlist[1, Playlist.CurrentRow.Index].Value.ToString();
            Playlist.Rows[this.currentSong].DefaultCellStyle.BackColor = Color.AliceBlue;
        }
        #endregion
        #endregion

        public Player()
        {
            InitializeComponent();

            VolumeBar.Value = Settings.Default.Volume;
            channel.setVolume(Settings.Default.Volume / 100.0f);
            result = Factory.System_Create(ref system);
            result = system.init(2, INITFLAGS.NORMAL, IntPtr.Zero);

            if (File.Exists(@"current.pls") && new FileInfo(@"current.pls").Length > 0)
            {
                PlaylistManager.Load(@"current.pls", Lista, Playlist, Encoding.UTF8);
                this.currentSong = Settings.Default.Last_Played_Song_ID;
                Playlist.Rows[this.currentSong].Selected = true;
                Playlist.CurrentCell = Playlist.Rows[this.currentSong].Cells[0];
                this.TitleLabel.Text = Playlist[1, this.currentSong].Value.ToString();
                Iterator = Lista.Count;
            }
        }

        #region Przyciski
        private void OnPlayButtonClick(object sender, EventArgs e)
        {
            if (Lista.Count > 0)
            {
                if (state)
                    this.OnPauseButtonClick(sender, e);
                else
                {
                    player?.release();
                    channel?.stop();

                    string path = Lista[this.currentSong];
                    if (!File.Exists(path))
                    {
                        Playlist.Rows[this.currentSong].DefaultCellStyle.BackColor = Color.Red;
                        Playlist[1, this.currentSong].Value = path;
                        this.currentSong = (this.currentSong == this.Lista.Count - 1) ? 0 : this.currentSong + 1;
                        path = Lista[this.currentSong];
                        Playlist.Rows[this.currentSong].DefaultCellStyle.BackColor = Color.AliceBlue;
                    }

                    this.SongLength(path);
                    this.TitleLabel.Text = Playlist[1, this.currentSong].Value.ToString();

                    result = system.createStream(path, MODE.HARDWARE, ref player);
                    result = system.playSound(CHANNELINDEX.FREE, player, false, ref channel);

                    channel.setVolume(VolumeBar.Value / 100f);

                    this.Clock.Start();
                    Zegar.Text = Resources.ResetClock;
                    this.PlayButton.BackColor = Color.Black;
                    this.PlayButton.Image = Resources.media_play_inv;
                }
            }
        }

        private void OnStopButtonClick(object sender, EventArgs e)
        {
            this.result = channel.stop();
            this.Clock.Stop();
            this.TrackBar.Value = 0;
            this.Zegar.Text = Resources.EmptyClock;
            this.PlayButton.UseVisualStyleBackColor = true;
            this.PauseButton.UseVisualStyleBackColor = true;
            this.PlayButton.Image = Resources.media_play;
            this.PauseButton.Image = Resources.media_pause;
        }

        private void OnPauseButtonClick(object sender, EventArgs e)
        {
            state = !state;
            result = channel.setPaused(state);

            if (state)
            {
                this.Clock.Stop();
                this.PauseButton.BackColor = Color.Black;
                this.PauseButton.Image = Resources.media_pause_inv;
                this.PlayButton.UseVisualStyleBackColor = true;
                this.PlayButton.Image = Resources.media_play;
            }
            else
            {
                this.Clock.Start();
                this.PauseButton.UseVisualStyleBackColor = true;
                this.PauseButton.Image = Resources.media_pause;
            }

        }
        private void OnPrevButtonClick(object sender, EventArgs e)
        {
            if (this.ShuffleBox.Checked)
            {
                Shufflin();
            }
            else
            {
                Playlist.Rows[this.currentSong].DefaultCellStyle.BackColor = Color.White;

                if (this.currentSong > 0)
                {
                    this.currentSong -= 1;
                }
                else if (this.currentSong == 0)
                {
                    this.currentSong = Lista.Count - 1;
                }

                Playlist.Rows[this.currentSong].DefaultCellStyle.BackColor = Color.AliceBlue;

            }

            this.OnPlayButtonClick(sender, e);
        }
        private void OnNextButtonClick(object sender, EventArgs e)
        {
            if (this.ShuffleBox.Checked)
            {
                Shufflin();
            }
            else
            {
                Playlist.Rows[this.currentSong].DefaultCellStyle.BackColor = Color.White;

                if (this.currentSong < Lista.Count - 1)
                {
                    this.currentSong += 1;
                }
                else if (this.currentSong == Lista.Count - 1)
                {
                    this.currentSong = 0;
                }

                this.TitleLabel.Text = this.TitleLabel.Text = Playlist[1, Playlist.CurrentRow.Index].Value as string;
                Playlist.Rows[this.currentSong].DefaultCellStyle.BackColor = Color.AliceBlue;
            }

            this.OnPlayButtonClick(sender, e);
        }

        private void OnOpenButtonClick(object sender, EventArgs e)
        {
            if (Open_File.ShowDialog() == DialogResult.OK)
                this.AddItem(Open_File.FileNames);

            Open_File.Dispose();
        }

        private void OnMuteButtonClick(object sender, EventArgs e)
        {
            if (VolumeBar.Value == 0)
            {
                VolumeBar.Value = this.vb_tmp_v;
                Mute_button.UseVisualStyleBackColor = true;
                Mute_button.Image = Resources.media_volume_1;
            }
            else
            {
                this.vb_tmp_v = VolumeBar.Value;
                VolumeBar.Value = 0;
                Mute_button.BackColor = Color.Black;
                Mute_button.Image = Resources.media_volume_0;
            }

        }
        #endregion

        #region Obsługa playlisty
        private void Playlist_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Delete) del_state = true;
        }

        private void Playlist_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Delete && del_state)
            {
                this.DeleteItem();
            }
        }

        private void Playlist_MouseClick(object sender, MouseEventArgs e)
        {
            if (Playlist[0, Playlist.CurrentRow.Index].Value != null)
            {
                this.PlayItem(sender, e);
            }
        }

        private void Playlist_CellMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.RowIndex >= 0 && e.ColumnIndex >= 0 && e.Button == MouseButtons.Right)
            {
                if (e.Button == MouseButtons.Right)
                {
                    Playlist.ClearSelection();
                    Playlist.Rows[e.RowIndex].Selected = true;
                    Playlist.CurrentCell = Playlist.Rows[e.RowIndex].Cells[0];
                    Rectangle r = Playlist.GetCellDisplayRectangle(e.ColumnIndex, e.RowIndex, true);
                    Playlist_Context_Menu.Show((Control)sender, r.Left + e.X, r.Top + e.Y);
                }

            }
        }
        private void toolStrip_Playlist_Button_ButtonClick(object sender, EventArgs e)
        {
            toolStrip_Playlist_Button.ShowDropDown();
        }
        private void nowaToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Iterator = 0;
            Playlist.Rows.Clear();
            Lista.Clear();
            if (File.Exists(@"current.pls")) File.Delete(@"current.pls");
        }
        private void wczytajToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenFileDialog open_playlist = new OpenFileDialog();
            open_playlist.Filter = "Obsługiwane typy list odtwarzań|*.pls;*.m3u|Lista odtwarzania PLS | *.pls|Lista odtwarzania M3U|*.m3u";
            if (open_playlist.ShowDialog() == DialogResult.OK)
            {
                nowaToolStripMenuItem_Click(sender, e);
                PlaylistManager.Load(open_playlist.FileName, Lista, Playlist, Encoding.Default);
                Iterator = Lista.Count;
            }
            open_playlist.Dispose();
        }
        private void zapiszToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SaveFileDialog save_file = new SaveFileDialog();
            save_file.Filter = "PLS Playlist|*.pls|M3U Playlist|*.m3u";
            if (save_file.ShowDialog() == DialogResult.OK)
                PlaylistManager.Save(save_file.FileName, Lista);
            save_file.Dispose();
        }
        private void plikiToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.OnOpenButtonClick(sender, e);
        }
        private void katalogToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (Open_Folder.ShowDialog() == DialogResult.OK)
            {
                this.AddItem(GetFilesList(Open_Folder.SelectedPath).ToArray());
            }
            Open_Folder.Dispose();
        }
        private void Playlist_DragDrop(object sender, DragEventArgs e)
        {
            string[] fileList = (string[])e.Data.GetData(DataFormats.FileDrop, false);
            List<string> tmpList = new List<string>();
            foreach (string s in fileList)
            {
                if (!IsDirectory(s))
                {
                    tmpList.Add(s);
                }
                else
                {
                    tmpList.AddRange(GetFilesList(s));
                }
            }

            this.AddItem(tmpList);
        }
        private void Playlist_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
                e.Effect = DragDropEffects.Copy;
            else
                e.Effect = DragDropEffects.None;
        }
        private void Context_Play_Click(object sender, EventArgs e)
        {
            this.PlayItem(sender, e);
        }
        private void Context_Delete_Click(object sender, EventArgs e)
        {
            this.DeleteItem();
        }

        private void Context_File_Info_Click(object sender, EventArgs e)
        {
            Song_Info si = new Song_Info(Lista[Playlist.CurrentRow.Index], system, player);
            si.Show();
        }
        #endregion

        #region Funkcje Odtwarzacza
        private void czas_Tick(object sender, EventArgs e)
        {
            #region Pasek Postępu + Zegar
            if (TrackBar.Value < TrackBar.Maximum)
            {
                TrackBar.Value += 1;
                tb_tmp_v = TrackBar.Value;
                channel.getPosition(ref pos, TIMEUNIT.MS);
                Zegar.Text = TimeSpan.FromMilliseconds(pos).ToString("mm\\:ss");
            }
            else
            {
                {
                    this.Clock.Stop();
                    this.OnNextButtonClick(sender, e);
                }
            }
            #endregion
        }
        private void VolumeBar_ValueChanged(object sender, EventArgs e)
        {
            if (VolumeBar.Value == 1)
            {
                Mute_button.UseVisualStyleBackColor = true;
                Mute_button.Image = Resources.media_volume_1;
            }
            result = channel.setVolume((float)VolumeBar.Value / 100);
            tooltip.Show(VolumeBar.Value.ToString(), this.VolumeBar, 100, -25, 1000);
        }
        private void VolumeBar_MouseHover(object sender, EventArgs e)
        {
            tooltip.Show(VolumeBar.Value.ToString(), this.VolumeBar, 100, -25, 1000);
        }
        private void progressBar_ValueChanged(object sender, EventArgs e)
        {
            try
            {
                if (TrackBar.Value > 0 && TrackBar.Value - tb_tmp_v != 1)
                    result = channel.setPosition((uint)TrackBar.Value * 1000, TIMEUNIT.MS);
            }
            catch
            {
            }
        }
        private void Shuffle_Box_CheckStateChanged(object sender, EventArgs e)
        {
            this.shuffle = this.ShuffleBox.Checked ? new Random() : null;
        }

        #endregion

        private void Player_FormClosing(object sender, FormClosingEventArgs e)
        {
            File.Delete("current.pls");
            if (Lista.Count > 0)
            {
                PlaylistManager.Save("current.pls", Lista);
                Settings.Default.Last_Played_Song_ID = this.currentSong;
            }
            Settings.Default.Volume = VolumeBar.Value;
            Settings.Default.Save();
            this.Dispose();
        }
    }
}
