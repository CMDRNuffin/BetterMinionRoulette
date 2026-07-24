namespace BetterMinionRoulette.UI;

using BetterMinionRoulette.Config;

using BetterRouletteBase.Config;
using BetterRouletteBase.UI;

using Dalamud.Plugin;
using Dalamud.Plugin.Services;

internal sealed class CharacterManagementRenderer : CharacterManagementRendererBase<Configuration>
{
    public CharacterManagementRenderer(
        IPlayerState playerState,
        IDalamudPluginInterface dalamudPluginInterface,
        WindowManagerBase windowManager,
        ICharacterManager characterManager,
        Configuration configuration)
        : base(playerState, dalamudPluginInterface, windowManager, characterManager, configuration)
    {
    }
}
