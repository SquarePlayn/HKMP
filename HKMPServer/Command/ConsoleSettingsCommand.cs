using Hkmp.Api.Command.Server;
using Hkmp.Game.Command.Server;
using Hkmp.Game.Server;
using Hkmp.Game.Settings;

namespace HkmpServer.Command {
    /// <summary>
    /// The settings command for the console program.
    /// </summary>
    internal class ConsoleSettingsCommand : SettingsCommand {
        public ConsoleSettingsCommand(
            ServerManager serverManager, 
            GameSettings gameSettings
        ) : base(serverManager, gameSettings) {
        }

        /// <inheritdoc />
        public override void Execute(ICommandSender commandSender, string[] args) {
            base.Execute(commandSender, args);

            ConfigManager.SaveGameSettings(GameSettings);
        }
        
        
    }
}