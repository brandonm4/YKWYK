using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.ComponentInterfaces;
using TaleWorlds.CampaignSystem.Party;
using TaleWorlds.CampaignSystem.ViewModelCollection.CharacterDeveloper;
using TaleWorlds.Core;

namespace YouKeepWhatYouKill.Models
{
    public class YKWYKPartyHealingModel : PartyHealingModel
    {
        public YKWYKPartyHealingModel(PartyHealingModel model) : base()
        {
            _model = model;
        }

        private readonly PartyHealingModel _model;

        //public override float GetSurvivalChance(PartyBase party, CharacterObject character, DamageTypes damageType, PartyBase enemyParty = null)
        //{
        //    if (damageType == DamageTypes.Blunt || character.IsHero && CampaignOptions.BattleDeath == CampaignOptions.Difficulty.VeryEasy || character.IsPlayerCharacter && CampaignOptions.BattleDeath == CampaignOptions.Difficulty.Easy)
        //    {
        //        return 1f;
        //    }

        //    var chance = _model.GetSurvivalChance(party, character, damageType, enemyParty);
        //    if (character.IsHero)
        //    {
        //        if (party != null && party == MobileParty.MainParty.Party)
        //        {
        //            chance = chance / Settings.Instance.CompanionChanceMultiplyer;
        //        }
        //        else
        //        {
        //            chance = chance / Settings.Instance.HeroDeathChanceMultiplyer;
        //        }
        //    }
        //    else
        //    {
        //        chance = chance / Settings.Instance.DeathChanceMultiplyer;
        //    }

        //    return chance;
        //}

        //public override int GetBattleEndHealingAmount(MobileParty party, CharacterObject character) => _model.GetBattleEndHealingAmount(party, character);

        public override ExplainedNumber GetDailyHealingForRegulars(MobileParty party, bool includeDescriptions = false) => _model.GetDailyHealingForRegulars(party, includeDescriptions);

        public override ExplainedNumber GetDailyHealingHpForHeroes(MobileParty party, bool includeDescriptions = false) => _model.GetDailyHealingHpForHeroes(party, includeDescriptions);

        public override int GetHeroesEffectedHealingAmount(Hero hero, float healingRate) => _model.GetHeroesEffectedHealingAmount(hero, healingRate);

        public override float GetSiegeBombardmentHitSurgeryChance(PartyBase party) => _model.GetSiegeBombardmentHitSurgeryChance(party);

        public override int GetSkillXpFromHealingTroop(PartyBase party) => _model.GetSkillXpFromHealingTroop(party);

        //public override float GetSurgeryChance(PartyBase party, CharacterObject character) => _model.GetSurgeryChance(party, character);

        public override float GetSurvivalChance(PartyBase party, CharacterObject agentCharacter, DamageTypes damageType, bool canDamageKillEvenIfBlunt, PartyBase enemyParty)
        {
            if (damageType == DamageTypes.Blunt || agentCharacter.IsHero && CampaignOptions.BattleDeath == CampaignOptions.Difficulty.VeryEasy || agentCharacter.IsPlayerCharacter && CampaignOptions.BattleDeath == CampaignOptions.Difficulty.Easy)
            {
                return 1f;
            }

            var chance = _model.GetSurvivalChance(party, agentCharacter, damageType, canDamageKillEvenIfBlunt, enemyParty);
            if (agentCharacter.IsHero)
            {
                if (party != null && party == MobileParty.MainParty.Party)
                {
                    chance = chance / Settings.Instance.CompanionChanceMultiplyer;
                }
                else
                {
                    chance = chance / Settings.Instance.HeroDeathChanceMultiplyer;
                }
            }
            else
            {
                chance = chance / Settings.Instance.DeathChanceMultiplyer;
            }

            return chance;
        }

        public override float GetSurgeryChance(PartyBase party) => _model.GetSurgeryChance(party);


        public override int GetBattleEndHealingAmount(MobileParty party, Hero hero) => _model.GetBattleEndHealingAmount(party, hero);

    }
}
