namespace BetterMinionRoulette.UI;

using BetterRouletteBase.UI;

using Dalamud.Interface.Windowing;

using System.Linq;

internal sealed class WindowManager : WindowManagerBase
{
    private readonly BetterMinionRoulettePlugin _plugin;

    public WindowManager(BetterMinionRoulettePlugin plugin)
        : base(plugin.DalamudPluginInterface)
    {
        _plugin = plugin;
    }

    protected override Window GetOrCreateConfigWindow(out bool isNew)
    {
        ConfigWindow? configWindow = InternalWindows.Windows.OfType<ConfigWindow>().FirstOrDefault();
        if (configWindow is null)
        {
            isNew = true;
            configWindow = new(_plugin);
        }
        else
        {
            isNew = false;
        }

        return configWindow;
    }

    protected override Window? TryGetConfigWindow()
    {
        return InternalWindows.Windows.OfType<ConfigWindow>().FirstOrDefault();
    }
}
