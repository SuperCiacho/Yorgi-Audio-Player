using System;
using System.IO;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AudioPlayer2.Tests
{
    [TestClass]
    public class Playground
    {
        [TestMethod]
        public void TestMethod1()
        {
            var a = @"D:\Music\Playlist.m3u";
            var b = @"D:\Music\My-Way\Song.mp3";
            int index = 0;

            var ppSegments = a.Split(Path.AltDirectorySeparatorChar, Path.DirectorySeparatorChar);
            var tpSegments = b.Split(Path.AltDirectorySeparatorChar, Path.DirectorySeparatorChar);
            int n = Math.Min(ppSegments.Length, tpSegments.Length);

            for (int i = 0; i < n; i++)
            {
                if (ppSegments[i] == tpSegments[i]) index++;
                else break;
            }

            var r = tpSegments.Skip(index);
            var result = string.Join(Path.DirectorySeparatorChar.ToString(), r);

            Assert.IsNotNull(result);
        }
    }
}
