using System;
using System.IO;

namespace DiffFolder.cmd
{
    public class cmdParser
    {
        private string[] _args;
        private Options opts;

        public cmdParser(string[] Args)
        {
            _args = Args;
        }

        public Options Parse()
        {
            opts = new();

            if (_args != null)
            {
                for (int i = 0; i < _args.Length; i++)
                {
                    if (_args[i] == "-h" || _args[i] == "--help")
                    {
                        opts.helpFlag = true;
                    }
                    else if (_args[i] == "-V" || _args[i] == "--version")
                    {
                        opts.versionFlag = true;
                    }
                    else if (_args[i] == "-v" || _args[i] == "--verbose")
                    {
                        opts.verbose = true;
                    }
                    else
                    {
                        // place any unnamed or positional value checks
                        if (Directory.Exists(_args[i]) && String.IsNullOrEmpty(opts.source))
                        {
                            opts.source = _args[i];
                        }
                        else if (Directory.Exists(_args[i]) && String.IsNullOrEmpty(opts.compFolder))
                        {
                            opts.compFolder = _args[i];
                        }
                    }
                }
            }

            return opts;
        }
    }
}
