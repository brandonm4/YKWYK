using HarmonyLib;

using System.Collections.Generic;
using System.Reflection;

using TaleWorlds.CampaignSystem.Inventory;
using TaleWorlds.CampaignSystem.Party;
using TaleWorlds.CampaignSystem.Roster;
using TaleWorlds.Core;

using Utility;

namespace YouKeepWhatYouKill.Patches
{
    public class OpenScreenAsLootPatch : PatchBase<OpenScreenAsLootPatch>
    {
        public override bool Applied { get; protected set; }

        private static readonly MethodInfo TargetMethodInfo = typeof(InventoryManager).GetMethod(
            "OpenScreenAsLoot",
            BindingFlags.Public | BindingFlags.Static | BindingFlags.DeclaredOnly
        );

        private static readonly MethodInfo PatchMethodInfo =
            typeof(OpenScreenAsLootPatch).GetMethod(
                nameof(Prefix),
                BindingFlags.NonPublic | BindingFlags.Static | BindingFlags.DeclaredOnly
            );

        public override bool IsApplicable(Game game)
        {
            return true;
        }

        public override void Apply(Game game)
        {
            if (Applied)
            {
                return;
            }

            SubModule.Harmony.Patch(
                TargetMethodInfo,
                prefix: new HarmonyMethod(PatchMethodInfo)
                {
                    priority = Priority.First,
                    //before = new[] { "that.other.harmony.user" }
                }
            );

            Applied = true;
        }

        public override bool IsEarlyPatch { get; protected set; } = false;

        public override void Reset() { }

        protected static bool Prefix(InventoryManager __instance, Dictionary<PartyBase, ItemRoster> itemRostersToLoot)
        {
            LootController.Instance.TransferLoot(itemRostersToLoot);
            return true;
        }


    }
}
