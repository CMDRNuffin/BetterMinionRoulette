namespace BetterMinionRoulette.Util;

using BetterMinionRoulette.Config;

using BetterRouletteBase.Util;

using Dalamud.Plugin.Services;

using FFXIVClientStructs.FFXIV.Client.Game;
using FFXIVClientStructs.FFXIV.Client.Game.Character;

using System;
using System.Runtime.InteropServices;

internal sealed class ActionHandler : ActionHandlerBase
{
    private readonly IObjectTable _objectTable;
    private readonly MinionRegistry _registry;
    private readonly Func<CharacterConfig?> _getCharConfig;

    public ActionHandler(
        IGameInteropProvider gameInteropProvider,
        IPluginLog pluginLog,
        IObjectTable objectTable,
        MinionRegistry registry,
        Func<CharacterConfig?> getCharConfig) : base(gameInteropProvider, pluginLog)
    {
        _objectTable = objectTable;
        _registry = registry;
        _getCharConfig = getCharConfig;
    }

    public unsafe void Summon(MinionGroup group)
    {
        uint minionId = _registry.GetRandom(ActionManager.Instance(), group);
        if (minionId != 0)
        {
            _ = ActionManager.Instance()->UseAction(ActionType.Companion, minionId);
        }
    }

    protected override unsafe bool OnUseAction(UseActionArgs args)
    {
        if ((args.ActionType, args.ActionID) is not (ActionType.GeneralAction, 10)
            || _getCharConfig() is not { } charConfig
            || charConfig.GetGroupByName(charConfig.RouletteGroup) is not { } group)
        {
            return args.Original();
        }

        uint newActionId = _registry.GetRandom(args.ActionManager, group, except: GetCurrentMinionID());
        if (newActionId != 0)
        {
            args.ActionType = ActionType.Companion;
            args.ActionID = newActionId;
        }

        return args.Original();
    }

    public unsafe uint GetCurrentMinionID()
    {
        uint currentMinion = 0;
        if (_objectTable.LocalPlayer is { Address: nint playerAddr })
        {
            var companion = (CompanionEx*)((Character*)playerAddr)->CompanionData.CompanionObject;
            if (companion is not null && companion->Unk0x2370 == 1)
            {
                currentMinion = companion->Companion.BaseId;
            }
        }

        return currentMinion;
    }

    [StructLayout(LayoutKind.Explicit, Size = 9328)]
    private struct CompanionEx
    {
        [FieldOffset(0)]
        public Companion Companion;

        [FieldOffset(0x2370)]
        public int Unk0x2370;
    }
}
