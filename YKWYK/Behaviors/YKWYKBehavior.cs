using Bannerlord.ButterLib.Logger.Extensions;


using Microsoft.Extensions.Logging;

using SandBox.Tournaments;

using System;
using System.Linq;

using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.Encounters;
using TaleWorlds.CampaignSystem.Party;
using TaleWorlds.Core;
using TaleWorlds.MountAndBlade;

using Utility;

using YouKeepWhatYouKill.Models;

namespace YouKeepWhatYouKill.Behaviors
{
    public class YKWYKBehavior : CampaignBehaviorBase
    {
        public override void RegisterEvents()
        {
            //CampaignEvents.OnMissionStartedEvent.AddNonSerializedListener(this, new Action<IMission>(this.OnMissionStarted));
            if (Settings.Instance.CompatibilityModes.ServeAsSoldier)
            {
                CampaignEvents.DailyTickEvent.AddNonSerializedListener(this, new Action(TickDaily));
                LogFactory.Get<SubModule>().LogDebug($"Registered Daily Tick Event.");
            }
        }

        public override void SyncData(IDataStore dataStore)
        {

        }

        private void TickDaily()
        {
            bool hadLoot = false;
            if (LootController.Instance.TroopEquipment.Any())
            {
                LootController.Instance.AddToPartyRoster(LootController.Instance.TroopEquipment, MobileParty.MainParty.ItemRoster);
                LogFactory.Get<SubModule>().LogDebug("Transferring Troop Lot.");
                hadLoot = true;
            }
            if (LootController.Instance.HeroEquipment.Keys.Any())
            {
                foreach (var hero in LootController.Instance.HeroEquipment.Keys)
                {
                    if (
                        (hero.IsPlayerCompanion && hero.IsDead)
                        || (!hero.IsPlayerCompanion && (hero.IsDead || !Settings.Instance.RequireKills))
                        )
                    {
                        MessageHelper.ShowDebugMessage(($" Adding {hero.Name} items"));
                        LogFactory.Get<SubModule>().LogDebug($"Transferring Hero {hero.Name} Loot.");
                        //MobileParty.MainParty.ItemRoster.AddToCounts(CompanionEquipment[hero].Select(e => new ItemRosterElement(e.Item)));
                        LootController.Instance.AddToPartyRoster(LootController.Instance.HeroEquipment[hero], MobileParty.MainParty.ItemRoster);
                    }
                }
                hadLoot = true;
            }
            if (hadLoot)
            {
                LootController.Instance.Initialize();
            }
        }

            private void OnMissionStarted(IMission mission)
        {
            if (PlayerEncounter.Current == null)
            {
                return;
            }

            Mission? mission1 = mission as Mission;
            if (mission1 != null
                && CampaignMission.Current != null
                && mission1.Scene != null
                )
            {
                //attempt fix of losing loot between combat sessions
                if (mission1.CombatType == Mission.MissionCombatType.Combat
                    && !mission1.HasMissionBehavior<YKWYKMission>()
                    && mission1.MissionBehaviors.Where(b => b as ITournamentGameBehavior != null).Count() == 0)

                {
                    mission1.AddMissionBehavior(new YKWYKMission());
                    if (Settings.Instance.DebugMode)
                    {
                        LogFactory.Get<SubModule>().LogDebugAndDisplay("YKWYK Logic Added.");
                    }
                    else
                        LogFactory.Get<SubModule>().LogDebug("YKWYK Logic Added.");
                }
                //else
                //{
                //    LootController.Instance.Initialize();
                //}
            }
        }

        


    }
}
