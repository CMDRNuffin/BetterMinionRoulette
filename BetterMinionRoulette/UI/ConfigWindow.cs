namespace BetterMinionRoulette.UI;

using BetterMinionRoulette.Config;
using BetterMinionRoulette.Util;

using BetterRouletteBase.UI;
using BetterRouletteBase.Util;

using Dalamud.Bindings.ImGui;

using System;
using System.Numerics;

internal sealed class ConfigWindow : ConfigWindowBase<CharacterConfig, MinionGroup, MinionData, MinionRegistry, Configuration>
{
    private readonly BetterMinionRoulettePlugin _plugin;

    public ConfigWindow(BetterMinionRoulettePlugin plugin) : base("Better Minion Roulette", ImGuiWindowFlags.AlwaysAutoResize)
    {
        _plugin = plugin;
    }

    protected override WindowManagerBase WindowManager => _plugin.WindowManager;

    protected override MinionRegistry ItemRegistry => _plugin.MinionRegistry;

    protected override CharacterConfig? CharacterConfig => _plugin.CharacterConfig;

    protected override ReadOnlySpan<byte> ItemGroupsTabName => "Minion Groups"u8;

    protected override CharacterManagementRendererBase<Configuration> CreateCharacterManagementRenderer()
    {
        return new CharacterManagementRenderer(
            _plugin.PlayerState,
            _plugin.DalamudPluginInterface,
            _plugin.WindowManager,
            _plugin.CharacterManager,
            _plugin.Configuration);
    }

    protected override ItemGroupPage<MinionData, MinionGroup, MinionRegistry> CreateItemGroupPage()
    {
        return new MinionGroupPage(
            ItemRegistry,
            _plugin.TextureProvider,
            WindowManager);
    }

    protected override void GeneralConfigTab(CharacterConfig characterConfig)
    {
        string? rouletteGroupName = characterConfig.RouletteGroup;

        RouletteGroup(characterConfig, ref rouletteGroupName);

        ImGui.Text("For the override to take effect, the selected group has to enable at least one minion."u8);

        bool suppressChatErrors = characterConfig.SuppressChatErrors;
        _ = ImGui.Checkbox("Suppress error messages in chat"u8, ref suppressChatErrors);

        characterConfig.RouletteGroup = rouletteGroupName;
        characterConfig.SuppressChatErrors = suppressChatErrors;
    }

    private void RouletteGroup(CharacterConfig characterConfig, ref string? groupName)
    {
        ImGuiStylePtr style = ImGui.GetStyle();

        const int ROWS = 2;
        float spacing = style.ItemSpacing.Y * (ROWS - 1);
        float checkboxHeight = ImGui.GetFrameHeight();
        float contentHeight = spacing + (checkboxHeight * ROWS);
        float totalHeight = contentHeight + (style.FramePadding.Y * 2) + style.ItemSpacing.Y;

        if (ImGui.BeginChildFrame(1u, new Vector2(0, totalHeight)))
        {
            ReadOnlySpan<byte> rouletteGroupId = "##roulettegroup"u8;
            if (ImGui.BeginTable(rouletteGroupId, 2))
            {
                ImGui.TableSetupColumn("##icon"u8, ImGuiTableColumnFlags.WidthFixed, contentHeight);
                ImGui.TableSetupColumn("##settings"u8, ImGuiTableColumnFlags.WidthStretch);

                _ = ImGui.TableNextColumn();

                ImGui.Image(_plugin.TextureProvider.LoadIconTexture(117u), new Vector2(contentHeight));

                _ = ImGui.TableNextColumn();

                SelectRouletteGroup(characterConfig, ref groupName, rouletteGroupId);

                ImGui.EndTable();
            }

            ImGui.EndChildFrame();
        }
    }

    protected override void Save()
    {
        _plugin.SaveConfig();
    }
}
