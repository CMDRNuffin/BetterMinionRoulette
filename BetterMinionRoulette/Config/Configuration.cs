namespace BetterMinionRoulette.Config;

using BetterRouletteBase.Config;

internal sealed class Configuration : ConfigurationBase
{
    public static Configuration Init()
    {
        return new();
    }
}
