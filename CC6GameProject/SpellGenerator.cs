using CC6GameProject.Spells;
using System;

namespace CC6GameProject
{
    static class SpellGenerator
    {
        public static SpellEffect[] GenerateMultiple(ref Random ran)
        {
            byte count = 1;

            // ?????????? Rarity?
            int chance = 999;
            int less = 7;
            while (ran.Next(0, 999) < chance)
            {
                less *= 9;
                less -= 7 * count;
                chance -= less;
                count++;
            }


            SpellEffect[] effects = new SpellEffect[count];

            for (int i = 0; i < count; i++)
            {
                effects[i] = Generate(ref ran);
            }

            return effects;
        }

        /// Generates fully random spell effects including rarity.
        public static SpellEffect Generate(ref Random ran)
        {
            int n = ran.Next(0, 1000);
            if (n < 30)
            {
                return GenerateRareSpell(ref ran);
            }
            else if (n < 222)
            {
                return GenerateUncommonSpell(ref ran);
            }
            else
            {
                return GenerateCommonSpell(ref ran);
            }
        }

        public static SpellEffect GenerateCommonSpell(ref Random ran)
        {
            switch (ran.Next(0, 2))
            {
                default:
                    return new Heal(ran.Next(3, 8));
                case 1:
                    return new StrengthBoost(ran.Next(2, 5));
            }
        }

        public static SpellEffect GenerateUncommonSpell(ref Random ran)
        {
            switch (ran.Next(0, 1))
            {
                default:
                    return new MaxHealthBoost(ran.Next(10, 34) / 10);
            }
        }

        public static SpellEffect GenerateRareSpell(ref Random ran)
        {
            switch (ran.Next(0, 1))
            {
                default:
                    return new HitPenalty(ran.Next(15, 21));
            }
        }

        public static SpellEffect[] GenerateMultipleCommon(ref Random ran, int n = 2)
        {
            SpellEffect[] eff = new SpellEffect[n];

            for (int i = 0; i < n; i++)
            {
                eff[i] = GenerateCommonSpell(ref ran);
            }

            return eff;
        }

        public static SpellEffect[] GenerateMultipleUncommon(ref Random ran, int n = 2)
        {
            SpellEffect[] eff = new SpellEffect[n];

            for (int i = 0; i < n; i++)
            {
                eff[i] = GenerateUncommonSpell(ref ran);
            }

            return eff;
        }

        public static SpellEffect[] GenerateMultipleRare(ref Random ran, int n = 2)
        {
            SpellEffect[] eff = new SpellEffect[n];

            for (int i = 0; i < n; i++)
            {
                eff[i] = GenerateRareSpell(ref ran);
            }

            return eff;
        }

        public static SpellEffect[] GenerateRandomSpellEffects(ref Random ran, int n = 2)
        {
            SpellEffect[] eff = new SpellEffect[n];

            for (int i = 0; i < n; i++)
            {
                eff[i] = Generate(ref ran);
            }

            return eff;
        }
    }
}
