namespace BetterMinionRoulette.Config;

using BetterRouletteBase.Config;
using BetterRouletteBase.Util.Memory;

using Newtonsoft.Json;

using System;
using System.Collections.Generic;
using System.Linq;

internal sealed class CharacterConfig : ICharacterConfig<MinionGroup>
{
    public List<MinionGroup> Groups { get; set; } = [];

    [JsonIgnore]
    public bool HasNonDefaultGroups { get; set; }

    public string? RouletteGroup { get; set; }

    public bool SuppressChatErrors { get; set; }

    public void AddGroup(string name)
    {
        Groups.Add(new MinionGroup { Name = name });
    }

    public MinionGroup? GetGroupByName(StringView name)
    {
        return Groups.FirstOrDefault(x => x.Name.Equals(name, StringComparison.InvariantCultureIgnoreCase));
    }

    public void ResetSelection(string from, string? to)
    {
        if (RouletteGroup == from)
        {
            RouletteGroup = to;
        }
    }

    public void CopyFrom(CharacterConfig other)
    {
        Groups = other.Groups;
        HasNonDefaultGroups = other.HasNonDefaultGroups;
        RouletteGroup = other.RouletteGroup;
    }
}
