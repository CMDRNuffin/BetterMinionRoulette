namespace BetterMinionRoulette.UI;

using BetterMinionRoulette.Config;
using BetterMinionRoulette.Util;

using BetterRouletteBase.UI;

using Dalamud.Plugin.Services;

internal sealed class MinionGroupPage(MinionRegistry itemRegistry, ITextureProvider textureProvider, WindowManagerBase windowManager)
    : ItemGroupPage<MinionData, MinionGroup, MinionRegistry>(itemRegistry, textureProvider, windowManager, "minion")
{
    protected override void PluginSpecificSettings(MinionGroup group)
    {
    }
}
