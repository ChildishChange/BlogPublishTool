using Microsoft.VisualStudio.TestTools.UnitTesting;
using BlogPublishTool;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlogPublishToolTests
{
    [TestClass()]
    public class MdHandlerTests
    {


        [TestMethod()]
        public void RegexParserTest()
        {
            Assert.AreEqual(3, MdHandler.RegexParser("testfile.txt", @"\[.*?\]\((.*?\.md)\)").Count);
            Assert.AreEqual(3, MdHandler.RegexParser("testfile.txt", @"!\[.*?\]\((.*?)\)").Count);
        }

        [TestMethod()]
        public void ReplaceContentWithUrlTest()
        {
            var dic = new Dictionary<string, string>
            {
                {"1.md", "www.cnblogs.com/1"},
                {"2.md", "www.cnblogs.com/2"},
                {"3.md", "www.cnblogs.com/3"},
                {"4.md", "www.cnblogs.com/4"}
            };

            Assert.AreEqual(File.ReadAllText("resultfile.txt"), MdHandler.ReplaceContentWithUrl("testfile.txt", dic));

        }

        [TestMethod()]
        public void WriteFileTest()
        {
            var dic = new Dictionary<string, string>
            {
                {"1.md", "www.cnblogs.com/1"},
                {"2.md", "www.cnblogs.com/2"},
                {"3.md", "www.cnblogs.com/3"},
                {"4.md", "www.cnblogs.com/4"}
            };
            var fileInfo = new FileInfo("writefiletest-cnblogs.txt");
            if (fileInfo.Exists)
            {
                fileInfo.Delete();
            }
            MdHandler.WriteFile("writefiletest.txt",@".\","-cnblogs", MdHandler.ReplaceContentWithUrl("testfile.txt", dic));
            Assert.AreEqual(true,new FileInfo("writefiletest-cnblogs.txt").Exists);
            Assert.AreEqual(File.ReadAllText("resultfile.txt"), File.ReadAllText("writefiletest-cnblogs.txt"));

        }
    }
}