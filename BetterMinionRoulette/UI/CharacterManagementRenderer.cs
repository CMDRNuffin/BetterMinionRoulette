namespace BetterMinionRoulette.UI;

using BetterMinionRoulette.Config;

using BetterRouletteBase.Config;
using BetterRouletteBase.UI;

using Dalamud.Plugin;
using Dalamud.Plugin.Services;

internal sealed class CharacterManagementRenderer(
    IPlayerState playerState,
    IDalamudPluginInterface dalamudPluginInterface,
    WindowManagerBase windowManager,
    ICharacterManager characterManager,
    Configuration configuration
) : CharacterManagementRendererBase<Configuration>(
    playerState,
    dalamudPluginInterface,
    windowManager,
    characterManager,
    configuration
)
{
}
