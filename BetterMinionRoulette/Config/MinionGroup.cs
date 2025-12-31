namespace BetterMinionRoulette.Config;

using BetterRouletteBase.Config;

using System.Collections.Generic;

internal sealed class MinionGroup : IItemGroup
{
    public string Name { get; set; } = "";

    public HashSet<uint> IncludedItems { get; set; } = [];

    public bool IncludedMeansActive { get; set; }
}
