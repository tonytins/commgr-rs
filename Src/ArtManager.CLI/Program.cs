// Copyright (c) Anthony Wilcox and contributors. All rights reserved.
// Licensed under the GNU GPL v3 license. See LICENSE file in the project
// root for full license information.
using System;
using System.Diagnostics;
using ArtManager.Common;
using ArtManager.CLI.Commands;
using ArtManager.CLI.Interface;
using Sixam.Logging;
using CommandLine;

namespace ArtManager.CLI
{
    class Program
    {
        static int Main(string[] args)
        {
            try
            {
                if (Debugger.IsAttached)
                    SerilogRunner.InitLogToDebug();
                else
                    SerilogRunner.InitLogToDirectory(ArtmConsts.AppDataPath, "artm");

                ICommand command;

                return Parser.Default.ParseArguments<SelfOpt, RequestOpt, ComOpt, YchOpt, ListOpt>(args)
                    .MapResult((SelfOpt opts) =>
                    {
                        command = new SelfOpt();
                        return command.RunCommand(opts);
                    }, (RequestOpt opts) =>
                    {
                        command = new RequestOpt();
                        return command.RunCommand(opts);
                    }, (ComOpt opts) =>
                    {
                        command = new ComOpt();
                        return command.RunCommand(opts);
                    }, (YchOpt opts) =>
                    {
                        command = new YchOpt();
                        return command.RunCommand(opts);
                    }, (ListOpt opts) =>
                    {
                        command = new ListOpt();
                        return command.RunCommand(opts);
                    }, errs => 1);
            }
            catch (Exception err)
            {
                SerilogHelper.LogException(err, true);
                throw new Exception(err.Message);
            }
        }
    }
}