using HarmonyLib;

using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.Party;
using TaleWorlds.Core;
using TaleWorlds.Library;

namespace ExecuteTroops
{
    [HarmonyPatch(typeof(PartyScreenLogic), "IsExecutable")]
    public class PartyScreenLogicTroopIsExecutablePatch
    {
        public static void Postfix(ref bool __result, PartyScreenLogic.TroopType troopType, CharacterObject character, PartyScreenLogic.PartyRosterSide side)
        {
            if (troopType == PartyScreenLogic.TroopType.Prisoner)
            {
                __result = true;
            }
        }
    }

    [HarmonyPatch(typeof(PartyScreenLogic), "GetExecutableReasonText")]
    public class PartyScreenLogicTroopExecuteReasonPatch
    {
        public static void PartyScreenLogicTroopExecuteReasonPatchPostfix(ref string __result, CharacterObject character)
        {
            if (!character.IsHero)
            {
                //    return GameTexts.FindText("str_cannot_execute_nonhero", null).ToString();
                __result = "Execute Troop";
            }
            //return GameTexts.FindText("str_execute_prisoner", null).ToString();
        }
    }



    [HarmonyPatch(typeof(PartyScreenLogic), "ExecuteTroop")]
    public class PartyScreenLogicExecuteTroopPatch
    {
        public static void PartyScreenLogicExecuteTroopPatchPostfix(PartyScreenLogic.PartyCommand command)
        {
            CharacterObject character = command.Character;
            for (int i = 0; i < 12; i++)
            {
                if (character.HeroObject.BattleEquipment[i].Item != null)
                {

                    EquipmentElement item = character.HeroObject.BattleEquipment[i];
                    PartyBase.MainParty.ItemRoster.AddToCounts(item.Item, 1);
                    item = character.HeroObject.BattleEquipment[i];
                    InformationManager.DisplayMessage(new InformationMessage(string.Concat(item.Item.Name.ToString(), " Added to inventory")));
                }
            }
        }
    }
}