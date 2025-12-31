namespace BetterMinionRoulette.Config;

using BetterRouletteBase.Config;

using Dalamud.Plugin;
using Dalamud.Plugin.Services;

internal sealed class CharacterManager : CharacterManagerBase<Configuration, CharacterConfig>
{
    public CharacterManager(
        IPluginLog pluginLog,
        IDalamudPluginInterface dalamudPluginInterface,
        IPlayerState playerState,
        Configuration configuration
    )
        : base(pluginLog, dalamudPluginInterface, playerState, configuration)
    {
    }

    protected override CharacterConfig CreateCharacterConfig()
    {
        return new CharacterConfig { Groups = [new() { Name = "Default" }] };
    }

    protected override void ImportFromConfig(CharacterConfig current, CharacterConfig toImport)
    {
        current.CopyFrom(toImport);
    }
}
