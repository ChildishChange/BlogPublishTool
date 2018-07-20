using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security;
using System.Text;
using System.Threading.Tasks;
using MetaWeblogClient;

/// <summary>
/// 
/// </summary>
namespace BlogPublishTool
{
    public class BlogHandler
    {
        private static BlogConnectionInfo connectionInfo;
        

        private const string blogUrl = "https://www.cnblogs.com/";
        private const string metaWeblogUrl = "https://rpc.cnblogs.com/metaweblog/";



        public BlogHandler()
        {
            Console.WriteLine("Please input your blog ID:");
            string BlogId = Console.ReadLine();

            Console.WriteLine("Please input your user name:");
            string UserName = Console.ReadLine();

            Console.WriteLine("Please input your password:");
            string PassWord = GetPassword();


            //用securestring好像有点多此一举。。先用着吧
            connectionInfo = new BlogConnectionInfo(
                blogUrl + BlogId,
                metaWeblogUrl + BlogId,
                BlogId,
                UserName,
                PassWord);
        }

        private static string GetPassword()
        {
            var password = new StringBuilder();
            while (true)
            {
                ConsoleKeyInfo i = Console.ReadKey(true);
                if (i.Key == ConsoleKey.Enter)
                {
                    Console.Write('\n');
                    break;
                }
                else if (i.Key == ConsoleKey.Backspace)
                {
                    if (password.Length > 0)
                    {
                        password.Remove(password.Length - 1,1);
                        Console.Write("\b \b");
                    }
                }
                else
                {
                    password.Append(i.KeyChar);
                    Console.Write("*");
                }
            }
            return password.ToString();
        }




        /// <summary>
        /// 
        /// </summary>
        /// <param name="filePath"></param>
        public void UploadPicture(string filePath)
        {

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="blogFilePath"></param>
        /// <param name="jsonFilePath"></param>
        public void ReplaceBlogUrl(string blogFilePath, string jsonFilePath)
        {

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="blogFilePath"></param>
        public void PublishBlog(string blogFilePath)
        {
            Client blogClient = new Client(connectionInfo);
            Console.WriteLine("======>START PUBLISH BLOG<======");

            Console.WriteLine("Please input title of this blog:");
            string blogTitle = Console.ReadLine();
            
            string blogContent = File.ReadAllText(blogFilePath);

            var postID = blogClient.NewPost(blogTitle, blogContent, new List<string>(){"[Markdown]"}, true, DateTime.Now);

            Console.WriteLine("Blog published here: "+ connectionInfo.BlogURL+"/p/"+postID+".html");
            Console.WriteLine("======>END PUBLISH BLOG<======");
        }

    }
}
