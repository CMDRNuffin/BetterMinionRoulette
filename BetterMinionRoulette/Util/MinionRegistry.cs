namespace BetterMinionRoulette.Util;

using BetterMinionRoulette.Config;

using BetterRouletteBase.Util;

using Dalamud.Plugin.Services;

using FFXIVClientStructs.FFXIV.Client.Game.UI;

using Lumina.Excel.Sheets;

using System.Collections.Generic;
using System.Linq;

internal sealed class MinionRegistry(IDataManager dataManager, ITextureProvider textureProvider, IClientState clientState)
    : ItemRegistry<MinionData, MinionGroup>(clientState)
{
    private readonly IDataManager _dataManager = dataManager;
    private readonly ITextureProvider _textureProvider = textureProvider;

    protected override IEnumerable<MinionData> GetAllItems()
    {
        return from minion in _dataManager.GetExcelSheet<Companion>()
               where minion.Order > 0 && minion.Icon != 0 /* valid minions only */
               orderby minion.Order, minion.RowId
               select new MinionData(_textureProvider, minion.Singular)
               {
                   IconID = minion.Icon,
                   ID = minion.RowId,
                   Unlocked = IsItemUnlocked(minion.RowId),
               };
    }

    protected unsafe override bool IsItemUnlocked(uint id)
    {
        return UIState.Instance()->IsCompanionUnlocked(id);
    }
}
