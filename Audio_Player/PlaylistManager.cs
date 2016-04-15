using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using FMOD;

namespace Audio_Player
{
    public class PlaylistManager
    {
        private static string fileName;
        private static FMOD.System paSystem;
        private static TAG title;
        private static TAG artist;
        private static uint length;

        public static void Load(string path, List<string> list, DataGridView table, Encoding encode)
        {
            switch (Path.GetExtension(path))
            {
                case ".pls":
                    foreach(string s in File.ReadAllLines(path, encode))
                    {
                        if (Regex.IsMatch(s, "File\\d+"))
                            list.Add(Regex.Replace(s, "File\\d+=", ""));

                        if (Regex.IsMatch(s, "Title\\d+"))
                            table.Rows.Add(list.Count, Regex.Replace(s, "Title\\d+=", ""));

                        if (Regex.IsMatch(s, "Length\\d+")) 
                            table[2, list.Count - 1].Value = TimeSpan.FromSeconds(Convert.ToDouble((Regex.Replace(s, "Length\\d+=", "")))).ToString().TrimStart('0', ':');
                    }
                    break;

                case ".m3u":
                    foreach (string s in File.ReadAllLines(path, encode))
                    {
                        if (!Regex.IsMatch(s, "#EXT"))
                            list.Add(s);
                        if (Regex.IsMatch(s, "#EXTINF"))
                        {
                            string inputString = Regex.Replace(s, "#EXTINF:", "");
                            table.Rows.Add(list.Count + 1, inputString.Remove(0, inputString.IndexOf(',') + 1), TimeSpan.FromSeconds(Convert.ToDouble((inputString.Remove(inputString.IndexOf(','))))).ToString().TrimStart('0', ':'));
                        }

                    }
                    break;
            }
        }

        public static void Save(string path, List<string> list)
        {
            using (var sw = new StreamWriter(path, false))
            {
                Factory.System_Create(ref paSystem);
                paSystem.init(100, INITFLAGS.NORMAL, (IntPtr) null);

                switch (Path.GetExtension(path))
                {
                    case ".pls":
                        sw.WriteLine("[playlist]");
                        for (int i = 0; i < list.Count; i++)
                        {
                            if (list[i].EndsWith(".wav"))
                            {
                                fileName = new DirectoryInfo(list[i]).Name;
                                fileName = fileName.Remove(fileName.Length - 4);
                                sw.WriteLine("File{0}={1}", Convert.ToString(i + 1), list[i]);
                                sw.WriteLine("Title{0}={1}", Convert.ToString(i + 1), fileName);
                                sw.WriteLine("Length{0}={1}", Convert.ToString(i + 1), (int) TimeSpan.FromMilliseconds(length).TotalSeconds);
                            }
                            else
                            {
                                GetTags(ref artist, ref title, ref length, list[i], ref paSystem, false);
                                fileName = new DirectoryInfo(list[i]).Name;
                                sw.WriteLine("File{0}={1}", Convert.ToString(i + 1), list[i]);
                                sw.WriteLine("Title{0}={1} - {2}", Convert.ToString(i + 1), Marshal.PtrToStringAnsi(artist.data), Marshal.PtrToStringAnsi(title.data));
                                sw.WriteLine("Length{0}={1}", Convert.ToString(i + 1), (int) TimeSpan.FromMilliseconds(length).TotalSeconds);
                            }
                        }
                        sw.WriteLine("NumberOfEntries=" + list.Count);
                        sw.WriteLine("Version=2");
                        break;

                    case ".m3u":
                        sw.WriteLine("#EXTM3U");
                        foreach (string s in list)
                        {
                            GetTags(ref artist, ref title, ref length, s, ref paSystem, true);
                            fileName = new DirectoryInfo(s).Name;
                            sw.WriteLine("#EXTINF:" + (int) TimeSpan.FromMilliseconds(length).TotalSeconds + ',' + fileName.Remove(fileName.Length - 4));
                            sw.WriteLine(s);
                        }
                        break;

                }

                sw.Flush();
                sw.Close();
            }
        }

        public static string NormalizeTime(uint time)
        {
            string output = TimeSpan.FromSeconds(Convert.ToInt32(TimeSpan.FromMilliseconds(time).TotalSeconds)).ToString();
            output = output.TrimStart('0', ':');
            return output;
        }

        public static void GetTags(ref TAG artistTag,ref TAG titleTag,ref uint songLength, string path, ref FMOD.System sys, bool onlyLength)
        {
            Sound sound = null;
            sys.createSound(path, (MODE.SOFTWARE | MODE._2D | MODE.CREATESTREAM | MODE.OPENONLY), ref sound);

            sound.getLength(ref songLength, TIMEUNIT.MS);
            if (!onlyLength)
            {
                sound.getTag("ARTIST", 0, ref artistTag);
                sound.getTag("TITLE", 0, ref titleTag);

                if (artistTag.data.ToString() == "0" || titleTag.data.ToString() == "0")
                {
                    sound.getTag("TPE1", 0, ref artistTag);
                    sound.getTag("TIT2", 0, ref titleTag);
                }
            }
            sound.release();
        }
    }
}
