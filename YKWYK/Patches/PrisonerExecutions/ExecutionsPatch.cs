//using HarmonyLib;

//using Helpers;

//using Microsoft.Extensions.Logging;

//using System;
//using System.Reflection;

//using TaleWorlds.CampaignSystem;
//using TaleWorlds.CampaignSystem.Party;
//using TaleWorlds.CampaignSystem.ViewModelCollection.Party;
//using TaleWorlds.Core;
//using TaleWorlds.Library;
//using Utility;

//using YouKeepWhatYouKill.Models;

//namespace YouKeepWhatYouKill.Patches
//{
//    public class ExecutionsPatch : PatchBase<ExecutionsPatch>
//    {
//        public override bool Applied { get; protected set; }

//        private static readonly MethodInfo TargetMethodInfo = typeof(PartyScreenLogic).GetMethod("IsExecutable", BindingFlags.Public | BindingFlags.Instance);
//        private static readonly MethodInfo PatchMethodInfo = typeof(ExecutionsPatch).GetMethod(nameof(PartyScreenLogicTroopIsExecutablePatchPostfix), BindingFlags.NonPublic | BindingFlags.Static);

//        private static readonly MethodInfo TargetMethodInfo2 = typeof(PartyScreenLogic).GetMethod("GetExecutableReasonText", BindingFlags.Public | BindingFlags.Instance);
//        private static readonly MethodInfo PatchMethodInfo2 = typeof(ExecutionsPatch).GetMethod(nameof(PartyScreenLogicTroopExecuteReasonPatchPostfix), BindingFlags.NonPublic | BindingFlags.Static);

//        private static readonly MethodInfo TargetMethodInfo3 = typeof(PartyScreenLogic).GetMethod("ExecuteTroop", BindingFlags.NonPublic | BindingFlags.Instance);
//        private static readonly MethodInfo PatchMethodInfo3 = typeof(ExecutionsPatch).GetMethod(nameof(PartyScreenLogicExecuteTroopPatchPostfix), BindingFlags.NonPublic | BindingFlags.Static);

//        private static readonly MethodInfo TargetMethodInfo4 = typeof(PartyCharacterVM).GetMethod("get_IsHeroPrisonerOfPlayer", BindingFlags.Public | BindingFlags.Instance);
//        private static readonly MethodInfo PatchMethodInfo4 = typeof(ExecutionsPatch).GetMethod(nameof(IsHeroPrisonerOfPlayerPostfix), BindingFlags.NonPublic | BindingFlags.Static);

//        private static ILogger Log { get; set; } = LogFactory.Get<SubModule>();

//        public override bool IsApplicable(Game game)
//        {
//            return true;
//        }
//        public override bool IsEarlyPatch { get; protected set; } = false;
//        public override void Apply(Game game)
//        {
//            if (Applied)
//            {
//                return;
//            }

//            SubModule.Harmony.Patch(TargetMethodInfo, postfix: new HarmonyMethod(PatchMethodInfo) { priority = Priority.Last });
//            SubModule.Harmony.Patch(TargetMethodInfo2, postfix: new HarmonyMethod(PatchMethodInfo2) { priority = Priority.Last });
//            SubModule.Harmony.Patch(TargetMethodInfo3, postfix: new HarmonyMethod(PatchMethodInfo3) { priority = Priority.Last });
//            SubModule.Harmony.Patch(TargetMethodInfo4, postfix: new HarmonyMethod(PatchMethodInfo4) { priority = Priority.Last });

//            Applied = true;
//        }

//        public override void Reset() { }



//        private static void PartyScreenLogicTroopIsExecutablePatchPostfix(ref bool __result, PartyScreenLogic.TroopType troopType, CharacterObject character, PartyScreenLogic.PartyRosterSide side)
//        {
//            if (troopType == PartyScreenLogic.TroopType.Prisoner)
//            {
//                __result = true;
//            }
//        }
//        private static void PartyScreenLogicTroopExecuteReasonPatchPostfix(ref string __result, CharacterObject character)
//        {
//            if (!character.IsHero)
//            {
//                //    return GameTexts.FindText("str_cannot_execute_nonhero", null).ToString();
//                __result = "Execute Troop";
//            }
//            //return GameTexts.FindText("str_execute_prisoner", null).ToString();
//        }

//        private static void PartyScreenLogicExecuteTroopPatchPostfix(PartyScreenLogic.PartyCommand command)
//        {
//            try
//            {
//                CharacterObject character = command.Character;
//                for (int i = 0; i < 12; i++)
//                {
//                    if (character.HeroObject.BattleEquipment[i].Item != null)
//                    {

//                        EquipmentElement item = character.HeroObject.BattleEquipment[i];
//                        PartyBase.MainParty.ItemRoster.AddToCounts(item.Item, 1);
//                        // item = character.HeroObject.BattleEquipment[i];
//                        InformationManager.DisplayMessage(new InformationMessage(string.Concat(item.Item.Name.ToString(), " Added to inventory")));
//                    }
//                }
//            }
//            catch(Exception ex) {
//                Log.LogError(ex, "Execute Troop");
//            }
//        }

//        private static void IsHeroPrisonerOfPlayerPostfix(PartyCharacterVM __instance, ref bool __result)
//        {
//            __result = __instance.IsPrisonerOfPlayer;
//        }

       

//    }
//}
