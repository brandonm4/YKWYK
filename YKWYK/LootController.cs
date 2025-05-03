using Bannerlord.ButterLib.Logger.Extensions;

using Helpers;

using Microsoft.Extensions.Logging;

using System;
using System.Collections.Generic;
using System.Linq;

using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.Party;
using TaleWorlds.CampaignSystem.Roster;
using TaleWorlds.Core;
using TaleWorlds.Library;

using Utility;

using YouKeepWhatYouKill.Models;

using static TaleWorlds.InputSystem.HotKey;

namespace YouKeepWhatYouKill
{
    public class LootController
    {
        public List<EquipmentElement> TroopEquipment { get; set; } = new List<EquipmentElement>();
        public Dictionary<Hero, List<EquipmentElement>> HeroEquipment { get; set; } = new();
        static LootController instance = new();

        public static LootController Instance
        {
            get
            {
                return instance;
            }
        }

        static ILogger Log = LogFactory.Get<SubModule>();

        public void Initialize()
        {
            Instance.TroopEquipment = new();
            Instance.HeroEquipment = new();
             Log.LogDebug(($"Loot pools initialized"));

        }
        public void AddLoot(CharacterObject character)
        {
            ExplainedNumber explainedNumber = new ExplainedNumber(Settings.Instance.LootChance, false, null);
            CharacterObject effectivePartyLeaderForSkill = SkillHelper.GetEffectivePartyLeaderForSkill(Hero.MainHero.PartyBelongedTo.Party);
            if (effectivePartyLeaderForSkill != null)
            {
                SkillHelper.AddSkillBonusForCharacter(DefaultSkills.Roguery, DefaultSkillEffects.RogueryLootBonus, effectivePartyLeaderForSkill, ref explainedNumber, -1, true, 0);
            }

            for (int index = 0; index < 12; index++)
            {
                EquipmentElement equipmentFromSlot = character.Equipment.GetEquipmentFromSlot((EquipmentIndex)index);


                if (equipmentFromSlot.Item != null)
                {
                    ItemModifier? modifier = equipmentFromSlot.ItemModifier;
                    if (MBRandom.RandomFloatRanged(.01f, 100f) <= explainedNumber.ResultNumber)
                    {
                        if (modifier == null)
                        {
                            if (MBRandom.RandomFloatRanged(.001f, 100f) <= Settings.Instance.ItemWorseChance)
                            {
                               //  Log.LogDebug(("Item is in poor condition."));
                                modifier = GetRandomModifierWeighted(GetModifierGroup(equipmentFromSlot.Item), 1.05f, false, true);
                            }
                            else if (MBRandom.RandomFloatRanged(.001f, 100f) <= Settings.Instance.ItemBetterChance)
                            {
                               //  Log.LogDebug(("Item is in better condition."));
                                modifier = GetRandomModifierWeighted(GetModifierGroup(equipmentFromSlot.Item), 1.05f, true, false);
                            }
                        }
                        //Log.LogDebug($"{equipmentFromSlot.Item.Name} added to Loot Pool.");
                        if (modifier != null)
                        {
                            equipmentFromSlot.SetModifier(modifier);
                        }
                       //  Log.LogDebug(($" {equipmentFromSlot.GetModifiedItemName()} added to Equipment pool."));
                        Instance.TroopEquipment.Add(equipmentFromSlot);
                    }
                    //else
                    //{
                    //   //  Log.LogDebug(($" {equipmentFromSlot.GetModifiedItemName()} failed loot roll."));
                    //}
                }
            }
        }
        public void AddHeroLoot(Hero hero, float lootChance, bool IsCompanion)
        {
            List<EquipmentElement> heroEquipment = new();
            ExplainedNumber explainedNumber = new ExplainedNumber(lootChance, false, null);
            CharacterObject effectivePartyLeaderForSkill = SkillHelper.GetEffectivePartyLeaderForSkill(Hero.MainHero.PartyBelongedTo.Party);
            if (effectivePartyLeaderForSkill != null)
            {
                SkillHelper.AddSkillBonusForCharacter(DefaultSkills.Roguery, DefaultSkillEffects.RogueryLootBonus, effectivePartyLeaderForSkill, ref explainedNumber, -1, true, 0);
            }

            for (int index = 0; index < 12; index++)
            {
                EquipmentElement equipmentFromSlot = hero.CharacterObject.Equipment.GetEquipmentFromSlot((EquipmentIndex)index);
                ItemModifier? modifier = equipmentFromSlot.ItemModifier;
                if (equipmentFromSlot.Item != null)
                {
                    if (MBRandom.RandomFloatRanged(.01f, 100f) <= explainedNumber.ResultNumber)
                    {

                        if (modifier == null)
                        {
                            if (MBRandom.RandomFloatRanged(.001f, 100f) <= Settings.Instance.ItemWorseChance)
                            {                                 
                                modifier = GetRandomModifierWeighted(GetModifierGroup(equipmentFromSlot.Item), 1.05f, false, true);
                            }
                            else if (MBRandom.RandomFloatRanged(.001f, 100f) <= Settings.Instance.ItemBetterChance)
                            {                             
                                modifier = GetRandomModifierWeighted(GetModifierGroup(equipmentFromSlot.Item), 1.05f, true, false);
                            }
                        }
                        //Log.LogDebug($"{equipmentFromSlot.Item.Name} added to Loot Pool.");
                        if (modifier != null)
                        {
                            equipmentFromSlot.SetModifier(modifier);
                        }

                        //ssageHelper.ShowDebugMessage($"{equipmentFromSlot.Item.Name} added to Loot Pool.");
                        if (Settings.Instance.DebugMode)
                        {
                             Log.LogDebug(($" {equipmentFromSlot.GetModifiedItemName()} added to Hero Equipment pool."));
                        }
                        else
                            Log.LogDebug(($" {equipmentFromSlot.GetModifiedItemName()} added to Hero Equipment pool."));

                        heroEquipment.Add(equipmentFromSlot);
                    }
                }
            }
            Instance.HeroEquipment[hero] = heroEquipment;
        }

        public void TransferLoot(Dictionary<PartyBase, ItemRoster> itemRostersToLoot)
        {
            Log.LogDebug((" Starting Loot Transfer"));
            Log.LogDebug(($" {Instance.TroopEquipment.Count + Instance.HeroEquipment.Count} items found in loot pools."));
            if (Instance.TroopEquipment.Count > 0 || Instance.HeroEquipment.Count > 0)
            {
                //MessageHelper.ShowMessage("Adding loot to party inventory.");
                int sum = Instance.TroopEquipment.Count;

                //Log.LogDebug($"Roster before loot additions: {MobileParty.MainParty.ItemRoster.Select(r => r.Amount).Sum()}");
                // Log.LogDebug(($"Roster before loot additions: {MobileParty.MainParty.ItemRoster.Select(r => r.Amount).Sum()}");
                Log.LogDebug(($" Roster before loot additions: {itemRostersToLoot[PartyBase.MainParty].Select(r => r.Amount).Sum()}"));

                //MobileParty.MainParty.ItemRoster.Add(equipment.Select(e => new ItemRosterElement(e.Item)));
                AddToPartyRoster(Instance.TroopEquipment, itemRostersToLoot[PartyBase.MainParty]);

                Log.LogDebug(($" Roster after troop loot additions: {itemRostersToLoot[PartyBase.MainParty].Select(r => r.Amount).Sum()}"));

                foreach (var hero in Instance.HeroEquipment.Keys)
                {
                    if (
                        (hero.IsPlayerCompanion && hero.IsDead)
                        || (!hero.IsPlayerCompanion && (hero.IsDead || !Settings.Instance.RequireKills))
                        )
                    {
                         Log.LogDebug(($" Adding {hero.Name} items"));
                        //MobileParty.MainParty.ItemRoster.AddToCounts(CompanionEquipment[hero].Select(e => new ItemRosterElement(e.Item)));
                        AddToPartyRoster(Instance.HeroEquipment[hero], itemRostersToLoot[PartyBase.MainParty]);
                        sum += Instance.HeroEquipment[hero].Count;
                    }
                }
                if (sum > 0)
                {
                    MessageHelper.ShowNotification(new TaleWorlds.Localization.TextObject($"You claimed {sum} items from the battlefield."), charObj: Hero.MainHero.CharacterObject);
                }
                Log.LogDebug(($" Roster after hero additions: {itemRostersToLoot[PartyBase.MainParty].Select(r => r.Amount).Sum()}"));
            }
            Instance.Initialize();
        }

        public void AddToPartyRoster(List<EquipmentElement> equipment, ItemRoster itemRoster)
        {
            foreach (var e in equipment)
            {
                //MobileParty.MainParty.ItemRoster.AddToCounts(e, 1);
                if (e.Item != null && !e.Item.Name.ToStringWithoutClear().StartsWith("DP ") && !e.Item.Name.ToStringWithoutClear().StartsWith("RYB_") && !e.Item.Name.ToStringWithoutClear().StartsWith("RYT_"))
                {
                    itemRoster.AddToCounts(e, 1);
                     Log.LogDebug(($" {e.Item.Name} added to Party Inventory"));
                }
                else
                {
                    try
                    {
                         Log.LogDebug(($" {e.GetModifiedItemName()} is null.  Not added to party."));
                    }
                    catch { }
                }
            }
        }


        public static ItemModifierGroup? GetModifierGroup(ItemObject item)
        {
            if (item.HasArmorComponent)
            {
                return item.ArmorComponent.ItemModifierGroup;
            }
            if (item.HasWeaponComponent)
            {
                return item.WeaponComponent.ItemModifierGroup;
            }
            return null;
        }

        public static ItemModifier? GetRandomModifierWeighted(ItemModifierGroup? instance, float variation = 0f, bool onlyGood = false, bool onlyBad = false)
        {
            try
            {
                if (instance == null || instance.ItemModifiers == null || instance.ItemModifiers.Count() == 0) return null;

                var itemQuery = instance.ItemModifiers.AsQueryable();

                if (onlyGood) itemQuery = itemQuery.Where(im => im.PriceMultiplier > 0);
                else if (onlyBad) itemQuery = itemQuery.Where(im => im.PriceMultiplier < 0);

                if (itemQuery.Count() == 0) return null;

                var maxValue = itemQuery.Max(im => im.PriceMultiplier);
                var minValue = itemQuery.Min(im => im.PriceMultiplier);

                if (minValue * -1 > maxValue) maxValue = (minValue * -1);

                maxValue *= variation;

                List<ValueTuple<ItemModifier, float>> valueTuples = new List<ValueTuple<ItemModifier, float>>();
                foreach (ItemModifier itemModifier in itemQuery)
                {
                    if (itemModifier.PriceMultiplier < 0)
                    {
                        valueTuples.Add(new ValueTuple<ItemModifier, float>(itemModifier, maxValue - (-1f * itemModifier.PriceMultiplier)));
                    }
                    else
                    {
                        valueTuples.Add(new ValueTuple<ItemModifier, float>(itemModifier, maxValue - itemModifier.PriceMultiplier));
                    }

                }
                return MBRandom.ChooseWeighted<ItemModifier>(valueTuples);
            }
            catch(Exception ex)
            {
                Log.LogError(ex, "GetRandomModifierWeighted");
            }
            return null;
        }
    }
}
