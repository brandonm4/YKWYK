using Bannerlord.ButterLib.Logger.Extensions;

using Microsoft.Extensions.Logging;

using System;

using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.Party;
using TaleWorlds.Core;
using TaleWorlds.MountAndBlade;

using Utility;

using YouKeepWhatYouKill.Models;

namespace YouKeepWhatYouKill.Behaviors
{
    internal class YKWYKMission : MissionLogic
    {
        static ILogger Log = LogFactory.Get<SubModule>();

        public override void OnAfterMissionCreated()
        {
            base.OnAfterMissionCreated();
        }

        public override void OnAgentRemoved(Agent affectedAgent, Agent affectorAgent, AgentState agentState, KillingBlow killingBlow)
        {
            bool added = false;
            try
            {
                if (affectedAgent != null && affectorAgent != null && affectedAgent.IsHuman)
                {
                    if (affectedAgent.IsHero != null && affectedAgent.IsHero && Settings.Instance.ClaimCompanionGear)
                    {
                        if (affectedAgent.Character != null)
                        {
                            var c = ((CharacterObject)affectedAgent.Character);
                            if (c != null && c.HeroObject != null)
                            {
                                if (c.HeroObject.IsPlayerCompanion)
                                {
                                    try
                                    {
                                        if (Settings.Instance.DebugMode)
                                        {
                                            Log.LogDebugAndDisplay($"{c.Name} killed by {affectorAgent.Name}. Adding Loot.");
                                        }
                                        else
                                        {
                                            Log.LogDebug($"{c.Name} killed by {affectorAgent.Name}. Adding Loot.");
                                        }
                                        LootController.Instance.AddHeroLoot(((CharacterObject)affectedAgent.Character).HeroObject, Settings.Instance.CompanionLootChance, true);
                                    }
                                    catch (Exception ex)
                                    {
                                        Log.LogError(ex.Message);
                                        //MessageHelper.ShowDebugMessage("Error added companion loot.");
                                    }
                                    added = true;

                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Log.LogError(ex, "OnAgentRemoved Troop");
                //  MessageHelper.ShowDebugMessage(ex.Message);
                //  MessageHelper.ShowDebugMessage("Error testing for companion");
            }

            try
            {
                if (!added && affectorAgent != null && affectedAgent != null && affectedAgent.Character != null && affectedAgent.IsHuman
                    && (!Settings.Instance.RequireKills || agentState == AgentState.Killed)
                    && affectorAgent.IsHero
                    && affectorAgent.Character == Hero.MainHero.CharacterObject)
                {
                    if (affectedAgent.IsHero)
                    {
                        try
                        {
                            Log.LogDebug($"{affectedAgent.Name} killed by {affectorAgent.Name}. Adding Loot.");
                            LootController.Instance.AddHeroLoot(((CharacterObject)affectedAgent.Character).HeroObject, Settings.Instance.HeroLootChance, false);
                        }
                        catch (Exception ex)
                        {
                            Log.LogError(ex.Message);
                            //MessageHelper.ShowDebugMessage("Error adding hero loot");
                        }
                    }
                    else
                    {
                        try
                        {
                            Log.LogDebug($"{affectedAgent.Name} killed by {affectorAgent.Name}. Adding Loot.");
                            LootController.Instance.AddLoot((CharacterObject)affectedAgent.Character);
                        }
                        catch (Exception ex)
                        {
                            Log.LogError(ex, "OnAgentRemoved Hero");
                            //MessageHelper.ShowDebugMessage(ex.Message);
                            //MessageHelper.ShowDebugMessage("Error adding troop loot");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Log.LogError(ex, "Error testing for agent.");
            }
        }




        public override void OnMissionResultReady(MissionResult missionResult)
        {
            Log.LogInformation("OnMissionResultReady.");
            try
            {
                if (missionResult.PlayerDefeated)
                {
                    Log.LogInformation("Player Defeated, clearing loot.");
                    LootController.Instance.Initialize();
                }               
            }
            catch (Exception ex)
            {
                Log.LogError(ex, "OnMissionResultsReady");
            }
        }



        public override void OnRetreatMission()
        {
            try
            {
                Log.LogInformation("Player retreated, clearing loot.");
                LootController.Instance.Initialize();
            }
            catch (Exception ex)
            {
                Log.LogError(ex, "OnRetreatMission");
            }
        }


    }
}
