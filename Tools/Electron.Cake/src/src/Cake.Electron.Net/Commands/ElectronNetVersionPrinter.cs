﻿using Cake.Core;
using Cake.Core.Annotations;
using Cake.Electron.Net.Commands.Settings;
using Cake.Electron.Net.Utils;

using System;

namespace Cake.Electron.Net.Commands
{
    public static class ElectronNetVersionPrinter
    {
        private const string CmdBase = "electronize version";

        [CakeMethodAlias]
        public static int ElectronNetVersion(this ICakeContext context, ElectronNetVersionSettings settings)
        {
            if (settings == null)
            {
                throw new ArgumentNullException(nameof(settings));
            }

            return ElectronNetVersion(context, settings.WorkingDirectory);
        }

        [CakeMethodAlias]
        public static int ElectronNetVersion(this ICakeContext context, string workingDirectory)
        {
            if (workingDirectory == null)
            {
                throw new ArgumentNullException(nameof(workingDirectory));
            }

            return ElectronCakeContext.Current.ProcessHelper.CmdExecute(CmdBase, workingDirectory);
        }
    }
}