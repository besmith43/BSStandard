using System.Linq;

namespace Sudo.cmd
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
            opts = new Options();

			if (_args.Length < 1)
			{
				opts.helpFlag = true;
			}
			else if (_args.Length == 1)
			{
				opts.command = _args[0];
				opts.commandArgs = "";
			}
			else
			{
				opts.command = _args[0];
				opts.commandArgs = string.Join(" ", _args.Skip(1).ToArray());
			}

            return opts;
        }
    }
}