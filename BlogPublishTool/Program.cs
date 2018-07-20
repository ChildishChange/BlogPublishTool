using System;
using System.Collections.Generic;
using System.Linq;
using System.Security;
using System.Text;
using System.Threading.Tasks;
using CommandLine;

/// <summary>
/// 
/// 
/// </summary>
namespace BlogPublishTool
{
    class Program
    {
        static void Main(string[] args)
        {
            Parser.Default.ParseArguments<Options>(args)
             .WithParsed(opts => RunOptionsAndReturnExitCode(opts))
             .WithNotParsed((errs) => HandleParseError(errs));
        }

        private static void HandleParseError(IEnumerable<Error> errs)
        {

            Console.ReadKey();
        }

        private static void RunOptionsAndReturnExitCode(Options opts)
        {
            //login here
            //除了登录是直接调用bloghandler，其他都是调用mdhandler，mdhandler
            var pass = ReadPassword();

            Console.WriteLine(pass.ToString());

            if(opts.Upload)
            {
                //upload pic here
                //
            }
            
            if(!string.IsNullOrWhiteSpace(opts.JsonFilePath))
            {
                //replace blog urls here
            }

            if(opts.PublishFlag)
            {
                ////publish blog here
            }


            Console.ReadKey();
            
        }
        public static SecureString ReadPassword(string mask = "*")
        {
            var password = new SecureString();
            while (true)
            {
                var i = Console.ReadKey(true);
                if (i.Key == ConsoleKey.Enter)
                {
                    Console.WriteLine(); break;
                }
                if (i.Key == ConsoleKey.Backspace)
                {
                    if (password.Length > 0)
                    {
                        password.RemoveAt(password.Length - 1);
                        Console.Write("\b \b");
                    }
                }
                else
                {
                    password.AppendChar(i.KeyChar);
                    Console.Write(mask);
                }
            }
            return password;
        }

    }
}
