using MCM.Abstractions.Attributes;
using MCM.Abstractions.Attributes.v2;
using MCM.Abstractions.Base.Global;

namespace YouKeepWhatYouKill.Models
{
    internal class Settings
    {
        public static SettingsMCM Instance
        {
            get
            {
                if (SettingsMCM.Instance != null)
                    return SettingsMCM.Instance;
                return new();
            }
        }
    }

    internal class SettingsMCM : AttributeGlobalSettings<SettingsMCM>
    {
        public override string DisplayName
        {
            get { return "You Keep What You Kill"; }
        }

        public override string FolderName
        {
            get { return "YKWYK"; }
        }

        public override string FormatType
        {
            get { return "json2"; }
        }
        public override string Id
        {
            get { return "YKWYK"; }
        }
        [SettingPropertyGroup("Gameplay", GroupOrder = 1)]
        [SettingPropertyFloatingInteger("Percent Chance to Loot Troop", minValue: 0f, maxValue: 100f, HintText = "Percent chance of looting a kill.",
            RequireRestart = false, Order = 1)]
        public float LootChance { get; set; } = 100f;
        [SettingPropertyGroup("Gameplay", GroupOrder = 1)]
        [SettingPropertyFloatingInteger("Percent Chance to Loot Hero", minValue: 0f, maxValue: 100f, HintText = "Percent chance of looting a kill.",
            RequireRestart = false, Order = 1)]
        public float HeroLootChance { get; set; } = 100f;

        [SettingPropertyGroup("Gameplay", GroupOrder = 1)]
        [SettingPropertyFloatingInteger("Troop Death Chance Multiplier", minValue: .01f, maxValue: 10f, HintText = "Increases or decreases hero chance death - 1.0 is default. Higher = more likely to die.",
            RequireRestart = true, Order = 2)]
        public float DeathChanceMultiplyer { get; set; } = 1f;

        [SettingPropertyGroup("Gameplay", GroupOrder = 1)]
        [SettingPropertyFloatingInteger("Hero/Lord Death Chance Multiplier", minValue: .01f, maxValue: 10f, HintText = "Increases or decreases hero chance death - 1.0 is default. Higher = more likely to die.",
            RequireRestart = false, Order = 3)]
        public float HeroDeathChanceMultiplyer { get; set; } = 1f;

        [SettingPropertyGroup("Gameplay", GroupOrder = 1)]
        [SettingPropertyFloatingInteger("Companion Chance Multiplier", minValue: .01f, maxValue: 10f, HintText = "Increases or decreases hero chance death - 1.0 is default. Higher = more likely to die.",
            RequireRestart = false, Order = 3)]
        public float CompanionChanceMultiplyer { get; set; } = 1f;

        [SettingPropertyGroup("Gameplay", GroupOrder = 1)]
        [SettingPropertyBool("Require Kills", HintText = "Require kills to claim loot.  Off = Knock Outs as well.",
            RequireRestart = true, Order = 4)]
        public bool RequireKills { get; set; } = true;



        [SettingPropertyGroup("Gameplay", GroupOrder = 1)]
        [SettingPropertyFloatingInteger("Claim Companions Gear Chance", minValue: 0f, maxValue: 100f, HintText = "Percent chance of getting companions gear if they die in battle.",
            RequireRestart = false, Order = 5)]
        public float CompanionLootChance { get; set; } = 100f;

        [SettingPropertyGroup("Gameplay", GroupOrder = 1)]
        [SettingPropertyBool("Claim Companion Equipment", HintText = "Get companion gear if they fall in battle.",
            RequireRestart = true, Order = 6)]
        public bool ClaimCompanionGear { get; set; } = true;


        [SettingPropertyGroup("Gameplay", GroupOrder = 1)]
        [SettingPropertyFloatingInteger("Chance of Better", minValue: 0f, maxValue: 100f, HintText = "Percent chance the item is an improved version.",
            RequireRestart = false, Order = 7)]
        public float ItemBetterChance { get; set; } = 10;

        [SettingPropertyGroup("Gameplay", GroupOrder = 1)]
        [SettingPropertyFloatingInteger("Chance of Worse", minValue: 0f, maxValue: 100f, HintText = "Percent chance the item is a worse/broken version.",
            RequireRestart = false, Order = 8)]
        public float ItemWorseChance { get; set; } = 10;
        //[SettingPropertyBool(
        //    "Experimental Features",
        //    HintText = "Settings in here are under development.",
        //    Order = 1,
        //    RequireRestart = false,
        //    IsToggle = true
        //)]
        //[SettingPropertyGroup("Experimental Features", GroupOrder = 50)]
        public bool EnableExperimentalFeatures { get; set; } = false;

        #region Debug
        [SettingPropertyGroup("{=txpg0013}Diagnostics", GroupOrder = 99)]
        [SettingPropertyBool("{=txpd0095}Enable Debug", RequireRestart = false)]
        public bool DebugMode { get; set; } = false;
        #endregion

        public SettingsMCM() { }

        public CompatibilityModes CompatibilityModes { get; set; } = new();

       
    }
    public class CompatibilityModes
    {
        public bool ServeAsSoldier { get; set; }
    }
}
