using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MetaWeblogClient;

/// <summary>
/// 
/// </summary>
namespace BlogPublishTool
{
    class BlogHandler
    {
        private static BlogConnectionInfo connectionInfo;
        private const string blogUrl = "https://www.cnblogs.com/";
        private const string metaWeblogUrl = "https://rpc.cnblogs.com/metaweblog/";



        BlogHandler(string BlogId, string UserName, string PassWord)
        {
            connectionInfo = new BlogConnectionInfo(
                blogUrl+BlogId, 
                metaWeblogUrl+BlogId, 
                BlogId, 
                UserName, 
                PassWord);
        }
        //saveinfo


        //loadinfo


        //checkblog


        //uploadpic


    }
}
