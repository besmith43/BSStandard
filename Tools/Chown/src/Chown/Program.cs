using System;
using System.IO;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Security.AccessControl;
using System.Security.Principal;
using CommandLine;

namespace Chown
{
    class Program
    {
        static void Main(string[] args)
        {
            var results = Parser.Default.ParseArguments<Options>(args)
                .WithParsed(Run);
        }

        public static void Run(Options opts)
        {
            if (opts.Domain == null)
            {
                opts.Domain = Environment.UserDomainName;
            }

            if (opts.User == null)
            {
                opts.User = Environment.UserName;
            }

            Parallel.ForEach(opts.files, (file) => {
                if (!File.Exists(file) && !Directory.Exists(file))
                {
                    file = Path.GetFullPath(file);
                }

                if (File.Exists(file))
                {
                    SetFileOwnerShip(file, opts.Domain, opts.User);
                }
                else if (Directory.Exists(file))
                {
                    SetFolderOwnerShip(file, opts.Domain, opts.User);
                }
                else
                {
                    Console.WriteLine($"Given Input is neither File or Directory: { file }");
                }
            });
        }

        public static void SetFileOwnerShip(string filename, string Domain, string User)
        {
            var fileSec = File.GetAccessControl(filename);

            try
            {
                var newOwner = new System.Security.Principal.NTAccount(Domain, User);
                fileSec.SetOwner(newOwner);
                fileSec.SetAccessRule(new FileSystemAccessRule(newOwner, FileSystemRights.FullControl, AccessControlType.Allow));

                File.SetAccessControl(filename, fileSec);
            }
            catch
            {
                Console.WriteLine($"Ownership of { filename } couldn't be modified");
            }
        }

        public static void SetFolderOwnerShip(string foldername, string Domain, string User)
        {
            string[] files = Directory.GetFiles(foldername);
            string[] folders = Directory.GetDirectories(foldername);

            var folderSec = Directory.GetAccessControl(foldername);

            try
            {
                var newOwner = new System.Security.Principal.NTAccount(Domain, User);
                folderSec.SetOwner(newOwner);
                folderSec.SetAccessRule(new FileSystemAccessRule(newOwner, FileSystemRights.FullControl, AccessControlType.Allow));

                Directory.SetAccessControl(foldername, folderSec);
            }
            catch
            {
                Console.WriteLine($"Ownership of { foldername } couldn't be modified");
            }

            Parallel.ForEach(files, (file) => {
                SetFileOwnerShip(file, Domain, User);
            });

            foreach (string folder in folders)
            {
                SetFolderOwnerShip(folder, Domain, User);
            }
        }

        public class Options
        {
            [Option('d', "Domain", Required=false, HelpText="Domain of the User Account.  If it is a local account, use the hostname of the computer")]
            public string Domain { get; set; }

            [Option('u', "user", Required=false, HelpText="User Account")]
            public string User { get; set; }

            [Value(0, MetaName="Files", Required=true, HelpText="Files to have Ownership modified")]
            public IEnumerable<string> files { get; set; }
        }
    }
}
