namespace BetterMinionRoulette.Util;

using BetterRouletteBase.Util;

using Dalamud.Plugin.Services;

using FFXIVClientStructs.FFXIV.Client.Game;
using FFXIVClientStructs.Interop;

using Lumina.Text.ReadOnly;

internal sealed class MinionData : ItemData
{
    public MinionData(ITextureProvider textureProvider, ReadOnlySeString name) : base(textureProvider, name)
    {
    }

    public unsafe override bool IsAvailable(Pointer<ActionManager> actionManager)
    {
        return actionManager.Value->GetActionStatus(ActionType.Companion, ID, checkRecastActive: false, checkCastingActive: false) == 0;
    }
}
