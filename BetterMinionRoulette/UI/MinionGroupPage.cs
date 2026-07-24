namespace BetterMinionRoulette.UI;

using BetterMinionRoulette.Config;
using BetterMinionRoulette.Util;

using BetterRouletteBase.UI;

using Dalamud.Plugin.Services;

internal sealed class MinionGroupPage : ItemGroupPage<MinionData, MinionGroup, MinionRegistry>
{
    public MinionGroupPage(MinionRegistry itemRegistry, ITextureProvider textureProvider, WindowManagerBase windowManager)
        : base(itemRegistry, textureProvider, windowManager, "minion")
    {
    }

    protected override void PluginSpecificSettings(MinionGroup group)
    {
    }
}
