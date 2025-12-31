namespace BetterMinionRoulette;

using BetterMinionRoulette.Config;
using BetterMinionRoulette.UI;
using BetterMinionRoulette.Util;

using Dalamud.Game.Command;
using Dalamud.Plugin;
using Dalamud.Plugin.Services;

using System;

public sealed class BetterMinionRoulettePlugin : IDalamudPlugin, IDisposable
{
    public const string COMMAND_TEXT = "/pminion";
    public const string COMMAND_HELP_MESSAGE = $"Open the config window.\n"
        + "/pminion <group> → Summon a random minion from the specified group";

    private bool _disposedValue;
    private readonly SessionManager _session;
    private readonly ICommandManager _commandManager;
    private readonly bool _ownsCommand;

    public BetterMinionRoulettePlugin(
        IDalamudPluginInterface dalamudPluginInterface,
        IDataManager dataManager,
        ITextureProvider textureProvider,
        IClientState clientState,
        IPlayerState playerState,
        IPluginLog pluginLog,
        ICommandManager commandManager,
        IFramework framework,
        IGameInteropProvider gameInteropProvider,
        IChatGui chat,
        IObjectTable objectTable)
    {
        ArgumentNullException.ThrowIfNull(dalamudPluginInterface);
        ArgumentNullException.ThrowIfNull(dataManager);
        ArgumentNullException.ThrowIfNull(textureProvider);
        ArgumentNullException.ThrowIfNull(clientState);
        ArgumentNullException.ThrowIfNull(playerState);
        ArgumentNullException.ThrowIfNull(pluginLog);
        ArgumentNullException.ThrowIfNull(commandManager);
        ArgumentNullException.ThrowIfNull(framework);
        ArgumentNullException.ThrowIfNull(gameInteropProvider);
        ArgumentNullException.ThrowIfNull(chat);
        ArgumentNullException.ThrowIfNull(objectTable);

        Configuration = dalamudPluginInterface.GetPluginConfig() as Configuration ?? Configuration.Init();
        dalamudPluginInterface.SavePluginConfig(Configuration);

        PlayerState = playerState;
        PluginLog = pluginLog;
        _commandManager = commandManager;
        Chat = chat;
        DalamudPluginInterface = dalamudPluginInterface;
        TextureProvider = textureProvider;
        MinionRegistry = new(dataManager, textureProvider, clientState);

        WindowManager = new(this);
        CharacterManager = new(pluginLog, dalamudPluginInterface, playerState, Configuration);
        ActionHandler = new(gameInteropProvider, pluginLog, objectTable, MinionRegistry, () => CharacterConfig);

        _ownsCommand = commandManager.AddHandler(COMMAND_TEXT, new CommandInfo(HandleCommand) { HelpMessage = COMMAND_HELP_MESSAGE });

        _session = new(clientState, framework, playerState);
        _session.Login += OnLogin;
        _ = framework.RunOnTick(() =>
        {
            if (PlayerState.IsLoaded)
            {
                OnLogin();
            }
        });
    }

    internal ActionHandler ActionHandler { get; }

    internal WindowManager WindowManager { get; }

    internal MinionRegistry MinionRegistry { get; }

    internal IDalamudPluginInterface DalamudPluginInterface { get; }

    internal IPlayerState PlayerState { get; }

    internal IPluginLog PluginLog { get; }
    public IChatGui Chat { get; }
    internal ITextureProvider TextureProvider { get; }

    internal Configuration Configuration { get; }

    internal CharacterConfig? CharacterConfig { get; private set; }

    internal CharacterManager CharacterManager { get; }

    internal void SaveConfig()
    {
        CharacterManager.SaveCurrentCharacterConfig();
        DalamudPluginInterface.SavePluginConfig(Configuration);
    }

    private void OnLogin()
    {
        if (PlayerState.IsLoaded)
        {
            CharacterConfig = CharacterManager.GetCharacterConfig(PlayerState.ContentId);
        }
    }

    private void HandleCommand(string command, string arguments)
    {
        if (string.IsNullOrWhiteSpace(arguments))
        {
            WindowManager.OpenConfigWindow();
        }
        else if (CharacterConfig is { } charConfig)
        {
            MinionGroup? group = charConfig.GetGroupByName(arguments);
            if (group is null)
            {
                // handle quotes
                if (arguments.StartsWith('"') && arguments.EndsWith('"'))
                {
                    arguments = arguments[1..^1];
                }

                group = charConfig.GetGroupByName(arguments);
                if (group is null)
                {
                    PrintError(this, $"Mount group \"{arguments}\" not found.");
                    return;
                }
            }

            ActionHandler.Summon(group);
        }

        static void PrintError(BetterMinionRoulettePlugin plugin, string message)
        {
            plugin.PluginLog.Error(message);
            if (!(plugin.CharacterConfig?.SuppressChatErrors ?? false))
            {
                plugin.Chat.PrintError(message);
            }
        }
    }

    private void Dispose(bool disposing)
    {
        if (!_disposedValue)
        {
            if (disposing)
            {
                // TODO: dispose managed state (managed objects)
            }

            ActionHandler.Dispose();
            WindowManager.Dispose();
            if (_ownsCommand)
            {
                _ = _commandManager.RemoveHandler(COMMAND_TEXT);
            }

            // TODO: free unmanaged resources (unmanaged objects) and override finalizer
            // TODO: set large fields to null
            _disposedValue = true;
        }
    }

    // // TODO: override finalizer only if 'Dispose(bool disposing)' has code to free unmanaged resources
    // ~BetterMinionRoulettePlugin()
    // {
    //     // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
    //     Dispose(disposing: false);
    // }

    public void Dispose()
    {
        // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
        Dispose(disposing: true);
        GC.SuppressFinalize(this);
    }
}
