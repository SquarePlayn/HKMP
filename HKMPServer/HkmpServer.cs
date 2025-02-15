﻿using Hkmp;
using Hkmp.Game.Settings;
using Hkmp.Networking.Packet;
using Hkmp.Networking.Server;
using HkmpServer.Command;
using Version = Hkmp.Version;

namespace HkmpServer {
    /// <summary>
    /// The HKMP Server class.
    /// </summary>
    internal class HkmpServer {
        /// <summary>
        /// Initialize the server with the given port, or ask for a port from the command line.
        /// </summary>
        /// <param name="args">The command line arguments.</param>
        public void Initialize(string[] args) {
            var consoleInputManager = new ConsoleInputManager();
            Logger.SetLogger(new ConsoleLogger(consoleInputManager));

            if (args.Length < 1) {
                Logger.Get().Error(this, "Please provide a port in the arguments");
                return;
            }

            if (string.IsNullOrEmpty(args[0]) || !ParsePort(args[0], out var port)) {
                Logger.Get().Error(this, "Invalid port, should be an integer between 0 and 65535");
                return;
            }

            var gameSettings = ConfigManager.LoadGameSettings(out var existed);
            if (!existed) {
                ConfigManager.SaveGameSettings(gameSettings);
            }

            StartServer(port, gameSettings, consoleInputManager);
        }

        /// <summary>
        /// Will start the server with the given port and game settings.
        /// </summary>
        /// <param name="port">The port of the server.</param>
        /// <param name="gameSettings">The game settings for the server.</param>
        /// <param name="consoleInputManager">The input manager for command-line input.</param>
        private void StartServer(
            int port, 
            GameSettings gameSettings,
            ConsoleInputManager consoleInputManager
        ) {
            Logger.Get().Info(this, $"Starting server v{Version.String}");

            var packetManager = new PacketManager();

            var netServer = new NetServer(packetManager);

            var serverManager = new ConsoleServerManager(netServer, gameSettings, packetManager);
            serverManager.Initialize();
            serverManager.Start(port);

            consoleInputManager.ConsoleInputEvent += input => {
                if (!serverManager.TryProcessCommand(new ConsoleCommandSender(), "/" + input)) {
                    Logger.Get().Info(this, $"Unknown command: {input}");
                }
            };
            consoleInputManager.StartReading();
        }

        /// <summary>
        /// Try to parse the given input as a networking port.
        /// </summary>
        /// <param name="input">The string to parse.</param>
        /// <param name="port">Will be set to the parsed port if this method returns true, or 0 if the method
        /// returns false.</param>
        /// <returns>True if the given input was parsed as a valid port, false otherwise.</returns>
        private static bool ParsePort(string input, out int port) {
            if (!int.TryParse(input, out port)) {
                return false;
            }

            if (!IsValidPort(port)) {
                return false;
            }

            return true;
        }

        /// <summary>
        /// Returns true if the given port is a valid networking port.
        /// </summary>
        /// <param name="port">The port to check.</param>
        /// <returns>True if the port is valid, false otherwise.</returns>
        private static bool IsValidPort(int port) {
            return port >= 0 && port <= 65535;
        }
    }
}