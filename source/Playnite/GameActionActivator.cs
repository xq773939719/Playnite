﻿using Playnite.SDK;
using Playnite.SDK.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Playnite
{
    public class GameActionActivator
    {
        private static ILogger logger = LogManager.GetLogger();

        public static Process ActivateAction(GameAction action)
        {
            logger.Info($"Activating game action {action}");
            switch (action.Type)
            {
                case GameActionType.File:
                    return ProcessStarter.StartProcess(action.Path, action.Arguments, action.WorkingDir);
                case GameActionType.URL:
                    return ProcessStarter.StartUrl(action.Path);
                case GameActionType.Emulator:
                    throw new Exception("Cannot start emulated game without emulator.");
            }

            return null;
        }

        public static Process ActivateAction(GameAction action, Game gameData)
        {
            logger.Info($"Activating game task {action}");
            switch (action.Type)
            {
                case GameActionType.File:
                    var path = action.Path;
                    var arguments = action.Arguments;
                    var workdir = action.WorkingDir;
                    return ProcessStarter.StartProcess(path, arguments, workdir);
                case GameActionType.URL:
                    var url = action.Path;
                    return ProcessStarter.StartUrl(url);
                case GameActionType.Emulator:
                    throw new Exception("Cannot start emulated game without emulator.");
            }

            return null;
        }

        public static Process ActivateAction(GameAction action, Game gameData, EmulatorProfile config)
        {
            logger.Info($"Activating game task {action}");
            switch (action.Type)
            {
                case GameActionType.File:
                case GameActionType.URL:
                    return ActivateAction(action, gameData);
                case GameActionType.Emulator:
                    if (config == null)
                    {
                        throw new Exception("Cannot start emulated game without emulator.");
                    }

                    var path = config.Executable;
                    var arguments = config.Arguments;
                    if (!string.IsNullOrEmpty(action.AdditionalArguments))
                    {
                        arguments += " " + action.AdditionalArguments;
                    }

                    if (action.OverrideDefaultArgs)
                    {
                        arguments = action.Arguments;
                    }

                    var workdir = config.WorkingDirectory;
                    return ProcessStarter.StartProcess(path, arguments, workdir);
            }

            return null;
        }

        public static Process ActivateAction(GameAction action, Game gameData, List<Emulator> emulators)
        {
            return ActivateAction(action, gameData, GetGameActionEmulatorConfig(action, emulators));
        }

        public static EmulatorProfile GetGameActionEmulatorConfig(GameAction action, List<Emulator> emulators)
        {
            return null;

            // TODO
            //if (action.EmulatorId == null || emulators == null)
            //{
            //    return null;
            //}

            //return emulators.FirstOrDefault(a => a.Id == action.EmulatorId)?.Profiles.FirstOrDefault(a => a.Id == action.EmulatorProfileId);
        }
    }
}
