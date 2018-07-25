using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using MetaWeblogClient;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace BlogPublishTool
{
    public class BlogHandler
    {
        private static BlogConnectionInfo _connectionInfo;
        private static Dictionary<string, string> _picFileTable;
        
        private const string BlogUrl = "https://www.cnblogs.com/";
        private const string MetaWeblogUrl = "https://rpc.cnblogs.com/metaweblog/";
        
        /// <summary>
        /// 
        /// </summary>
        public BlogHandler()
        {
            Console.WriteLine("Please input your blog ID:");
            var blogId = Console.ReadLine();

            Console.WriteLine("Please input your user name:");
            var userName = Console.ReadLine();

            Console.WriteLine("Please input your password:");
            var passWord = GetPassword();
            
            _connectionInfo = new BlogConnectionInfo(
                BlogUrl + blogId,
                MetaWeblogUrl + blogId,
                blogId,
                userName,
                passWord);
            InitPicFileTable();
        }

        private void InitPicFileTable()
        {
            _picFileTable = new Dictionary<string, string>
            {
                { ".bmp", "image/bmp" },
                { ".gif", "image/gif" },
                { ".ico", "image/x-icon" },
                { ".jpg", "image/jpeg" },
                { ".jpeg", "image/jpeg" },
                { ".png", "image/png" }
            };
        }
         /// <summary>
         /// 
         /// </summary>
         /// <returns></returns>
        public static string GetPassword()
        {
            var password = new StringBuilder();
            while (true)
            {
                var i = Console.ReadKey(true);
                if (i.Key == ConsoleKey.Enter)
                {
                    Console.Write('\n');
                    break;
                }

                // ReSharper disable once SwitchStatementMissingSomeCases
                switch (i.Key)
                {
                    case ConsoleKey.Backspace when password.Length <= 0:
                        continue;
                    case ConsoleKey.Backspace:
                        password.Remove(password.Length - 1,1);
                        Console.Write("\b \b");
                        break;
                    default:
                        password.Append(i.KeyChar);
                        Console.Write("*");
                        break;
                }
            }
            return password.ToString();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="blogFilePath"></param>
        /// <param name="testFlag"></param>
        public void UploadPicture(string blogFilePath, bool testFlag)
        {
            Console.WriteLine("======>START UPLOAD PICTURE<======");

            const string matchRule = @"!\[.*?\]\((.*?)\)";
            var pictureList = MdHandler.RegexParser(blogFilePath, matchRule);
            var pictureUrlDic = new Dictionary<string, string>();

            Client blogClient = null;
            if (!testFlag)
            {
                blogClient = new Client(_connectionInfo);
            }

            foreach (var picturePath in pictureList)
            {
                if (picturePath.StartsWith("http"))
                {
                    Console.WriteLine($"Jump picture:{picturePath}.");
                    continue;
                }

                //upload picture
                try
                {
                    // ReSharper disable once AssignNullToNotNullAttribute
                    var pictureAbsPath = Path.Combine(new FileInfo(blogFilePath).DirectoryName, picturePath);
                    if (File.Exists(pictureAbsPath))
                    {
                        if(!testFlag)
                        {
                            //这里需要改一下
                            var pictureUrl = blogClient.NewMediaObject(picturePath, _picFileTable[new FileInfo(picturePath).Extension.ToLower()], File.ReadAllBytes(pictureAbsPath));

                            if (!pictureUrlDic.ContainsKey(picturePath))
                            {
                                pictureUrlDic.Add(picturePath, pictureUrl.URL);
                            }
                            Console.WriteLine($"{picturePath} uploaded");
                        }
                        else
                        {
                            Console.WriteLine($"{picturePath} need upload.");
                        }
                    }
                    else
                    {
                        Console.WriteLine($"No such file:{picturePath}");
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }
            }
            if(!testFlag)
            {
                var blogContent = MdHandler.ReplaceContentWithUrl(blogFilePath, pictureUrlDic);
                MdHandler.WriteFile(blogFilePath, new FileInfo(blogFilePath).DirectoryName, "", blogContent);
            }
            
            Console.WriteLine("======>END UPLOAD PICTURE<======");
        }

        /// <summary>
        ///  
        /// </summary>
        /// <param name="blogFilePath"></param>
        /// <param name="outDirPath"></param>
        /// <param name="jsonFilePath"></param>
        /// <param name="blogPlatform"></param>
        public static void ReplaceBlogUrl(string blogFilePath, string outDirPath, string jsonFilePath, string blogPlatform)
        {
            Console.WriteLine("======>START REPLACE BLOG URL<======");
            const string matchRule = @"\[.*?\]\((.*?\.md)\)";
            var linkList = MdHandler.RegexParser(blogFilePath, matchRule);
            var blogUrlDic = new Dictionary<string, string>();
            var blogJson = (JArray)JsonConvert.DeserializeObject(File.ReadAllText(jsonFilePath));
            var blogJsonDic = new Dictionary<string, JObject>();
            foreach (var jToken in blogJson)
            {
                var blog = (JObject) jToken;
                blogJsonDic.Add(blog.Properties().First().Name, blog);
            }

            foreach (var link in linkList)
            {
                if (link.StartsWith("http"))
                {
                    Console.WriteLine($"Jump Link:{link}.");
                    continue;
                }
                try
                {
                    var blogUrl = blogJsonDic[link][link][blogPlatform].ToString();
                    if (!blogUrlDic.ContainsKey(link))
                    {
                        blogUrlDic.Add(link, blogUrl);
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }
            }
            var blogContent = MdHandler.ReplaceContentWithUrl(blogFilePath, blogUrlDic);



            MdHandler.WriteFile(blogFilePath, outDirPath, blogPlatform, blogContent);
            Console.WriteLine("======>END REPLACE BLOG URL<======");
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="blogFilePath"></param>
        public void PublishBlog(string blogFilePath)
        {
            var blogClient = new Client(_connectionInfo);
            Console.WriteLine("======>START PUBLISH BLOG<======");

            Console.WriteLine("Please input title of this blog:{0}", blogFilePath);
            var blogTitle = Console.ReadLine();
            
            var blogContent = File.ReadAllText(blogFilePath);

            var postId = blogClient.NewPost(blogTitle, blogContent, new List<string> {"[Markdown]"}, true, DateTime.Now);

            Console.WriteLine("Blog published here: "+ _connectionInfo.BlogURL+"/p/"+postId+".html");
            Console.WriteLine("======>END PUBLISH BLOG<======");
        }




    }
}
