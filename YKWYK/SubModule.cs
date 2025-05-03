using Bannerlord.ButterLib.Common.Extensions;
using Bannerlord.ButterLib.Logger.Extensions;
using Bannerlord.ButterLib.MBSubModuleBaseExtended;

using Microsoft.Extensions.Logging;

using SandBox.Tournaments;

using Serilog.Events;

using System;
using System.Linq;

using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.ComponentInterfaces;
using TaleWorlds.Core;
using TaleWorlds.Library;
using TaleWorlds.ModuleManager;
using TaleWorlds.MountAndBlade;

using Utility;

using YouKeepWhatYouKill.Behaviors;
using YouKeepWhatYouKill.Models;

namespace YouKeepWhatYouKill
{
    public partial class SubModule : MBSubModuleBaseEx
    {
        public readonly static string Name = "YouKeepWhatYouKill";
        public readonly static string DisplayName = "You Keep What You Kill";
        public readonly static string HarmonyDomain = "com.darkspyre.ykwyk";
        internal readonly static Color StdTextColor = Color.FromUint(15822118);
        internal static SubModule Instance { get; set; } = default!;
        private static ILogger Log { get; set; } = default!;

        public static string Version
        {
            get
            {
                return ($"{ModuleHelper.GetModuleInfo(Name).Version}");
            }
        }


        #region Taleworlds Sub Mod Callbacks       
        protected override void OnSubModuleLoad()
        {
            base.OnSubModuleLoad();
            Instance = this;

            //var extender = new UIExtender(Name);
            //extender.Register(typeof(SubModule).Assembly);
            //extender.Enable();

            LogEventLevel logEventLevel = LogEventLevel.Verbose;
            if (Settings.Instance.DebugMode)
            {
                logEventLevel = LogEventLevel.Verbose;
            }

            this.AddSerilogLoggerProvider($"{Name}.log", new[] { $"{Name}.*", $"{Name}.Behaviors.*", $"{Name}.Patches.*", "YKWYK*" }, config => config.MinimumLevel.Is(logEventLevel));
            Log = LogFactory.Get<SubModule>();
            Log.LogInformation("You Keep What You Kill Loaded");
        }

        public override void OnCampaignStart(Game game, object starterObject)
        {
            base.OnCampaignStart(game, starterObject);
        }

        protected override void OnGameStart(Game game, IGameStarter gameStarterObject)
        {
            base.OnGameStart(game, gameStarterObject);

            if (game.GameType is Campaign)
            {
                ApplyPatches(
                    game,
                    typeof(SubModule),
                    Settings.Instance.DebugMode
                );

                if (gameStarterObject is CampaignGameStarter campaignGameStarter)
                {
                    campaignGameStarter.AddBehavior(new YKWYKBehavior());

                    if (Settings.Instance.DeathChanceMultiplyer != 1f || Settings.Instance.HeroDeathChanceMultiplyer != 1f)
                    {
                        //campaignGameStarter.AddModel(new YKWYKPartyHealingModel());
                        campaignGameStarter.AddModel(new YKWYKPartyHealingModel((PartyHealingModel)campaignGameStarter.Models.ToList().FindLast(model => model is PartyHealingModel)));
                    }
                }

                Log.LogInformation("You Keep What You Kill Game Started");
            }

        }

        protected override void OnBeforeInitialModuleScreenSetAsRoot()
        {
            base.OnBeforeInitialModuleScreenSetAsRoot();

            try
            {
                if (Settings.Instance.DebugMode)
                {
                    MessageHelper.ShowMessage($"You Keep What You Kill {Version}- DEBUG", Colors.Yellow);
                    MessageHelper.ShowMessage(LootController.Instance.TroopEquipment.Count().ToString());

                }
                else
                {
                    MessageHelper.ShowMessage($"You Keep What You Kill {Version} Loaded", Colors.Cyan);
                }

                if (ModuleHelper.GetModules().Where(m => m.Name.Contains("ServeAsSoldier") || m.Name.Contains("Serve As Soldier")).Count() > 0)
                {
                    Log.LogInformation($"Serve as Soldier Detected");
                    var info = ModuleHelper.GetModules().Where(m => m.Name.Contains("ServeAsSoldier") || m.Name.Contains("Serve As Soldier")).First();
                    Settings.Instance.CompatibilityModes.ServeAsSoldier = true;
                    MessageHelper.ShowMessage("Serve as Soldier Detected. Enabling Compatibility Mode.", Colors.Green);
                    Log.LogInformation("Serve as Soldier Detected. Enabling Compatibility Mode.");
                }
            }
            catch (Exception ex)
            {

            }
        }
        public override void OnGameInitializationFinished(Game game)
        {

        }

        public override void OnBeforeMissionBehaviorInitialize(Mission mission)
        {
            //Mission? mission1 = mission as Mission;
            if (mission != null
                && CampaignMission.Current != null
                && mission.Scene != null
                )
            {
                //attempt fix of losing loot between combat sessions
                if (mission.CombatType == Mission.MissionCombatType.Combat
                    && !mission.HasMissionBehavior<YKWYKMission>()
                    && mission.MissionBehaviors.Where(b => b as ITournamentGameBehavior != null).Count() == 0)

                {
                    mission.AddMissionBehavior(new YKWYKMission());
                    Log.LogDebugAndDisplay("YKWYK Logic Added.");
                    MessageHelper.ShowDebugMessage("YKWYK Logic Added");
                    //if (Settings.Instance.DebugMode)
                    //{
                    //    LogFactory.Get<YKWYKBehavior>().LogDebugAndDisplay("YKWYK Logic Added.");
                    //}
                    //else
                    //    LogFactory.Get<YKWYKBehavior>().LogDebug("YKWYK Logic Added.");
                }
                //else
                //{
                //    LootController.Instance.Initialize();
                //}
            }
        }
        protected override void OnSubModuleUnloaded()
        {
            base.OnSubModuleUnloaded();

        }

        #endregion
    }
}
