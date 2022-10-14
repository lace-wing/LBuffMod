using Steamworks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LBuffMod.Common.Utilities
{
    public static class LBuffUtil
    {
        //所有原版伤害性debuff：着火了！、狱火、霜火、霜噬、诅咒焰、暗影焰、日耀、中毒、毒液、带电、穿刺、窒息、树妖祸、狂卷之舌、灼烧
        public static int[] vanillaDamagingDebuffs =
        {
            BuffID.OnFire, BuffID.OnFire3, BuffID.Frostburn, BuffID.Frostburn2, BuffID.CursedInferno,
            BuffID.ShadowFlame, BuffID.Daybreak, BuffID.Poisoned, BuffID.Venom, BuffID.Electrified,
            BuffID.BoneJavelin, BuffID.Suffocation, BuffID.DryadsWardDebuff, BuffID.TheTongue, BuffID.Burning
        };
        //LBuffMod伤害性debuff：着火了！、狱火、霜火、霜噬、诅咒焰、暗影焰、日耀、中毒、毒液、带电、穿刺、窒息、树妖祸、狂卷之舌、灼烧、流血
        public static int[] lDamagingDebuffs =
        {
            BuffID.OnFire, BuffID.OnFire3, BuffID.Frostburn, BuffID.Frostburn2, BuffID.CursedInferno,
            BuffID.ShadowFlame, BuffID.Daybreak, BuffID.Poisoned, BuffID.Venom, BuffID.Electrified,
            BuffID.BoneJavelin, BuffID.Suffocation, BuffID.DryadsWardDebuff, BuffID.TheTongue, BuffID.Burning,
            BuffID.Bleeding
        };
        //总体要加强的debuff：着火了！、狱火、霜火、霜噬、诅咒焰、暗影焰、日耀、中毒、毒液、带电、流血
        public static int[] damagingDebuffsToBuff =
        {
            BuffID.OnFire, BuffID.OnFire3, BuffID.Frostburn, BuffID.Frostburn2, BuffID.CursedInferno,
            BuffID.ShadowFlame, BuffID.Daybreak, BuffID.Poisoned, BuffID.Venom, BuffID.Electrified,
            BuffID.Bleeding
        };
        //热能debuff：着火了！、狱火、霜火、霜噬、诅咒焰、暗影焰、日耀、灼烧
        public static int[] thermalDebuffs =
        {
            BuffID.OnFire, BuffID.OnFire3, BuffID.Frostburn, BuffID.Frostburn2, BuffID.CursedInferno,
            BuffID.ShadowFlame, BuffID.Daybreak, BuffID.Burning
        };
        //常规火焰
        public static int[] normalFireDebuffs =
        {
            BuffID.OnFire, BuffID.OnFire3, BuffID.Burning, BuffID.Daybreak
        };
        //霜冻火焰
        public static int[] frostFireDebuffs =
        {
            BuffID.Frostburn, BuffID.Frostburn2
        };
        //邪恶火焰
        public static int[] evilFires =
        {
            BuffID.CursedInferno, BuffID.ShadowFlame
        };
        //毒性debuff：中毒、毒液
        public static int[] poisonousDebuffs =
        {
            BuffID.Poisoned, BuffID.Venom
        };
        public static bool NPCHasBuffInBuffSet(NPC npc, int[] buffSet)
        {
            for (int i = 0; i < buffSet.Length; i++)
            {
                if (npc.HasBuff(buffSet[i]))
                {
                    return true;
                }
            }
            return false;
        }
        public static bool NPCHasTheBuffInBuffSet(NPC npc, int buffType, int[] buffSet)
        {
            for (int i = 0; i < buffSet.Length; i++)
            {
                if (buffType == buffSet[i])
                {
                    return true;
                }
            }
            return false;
        }
        public static int NPCBuffNumInBuffSet(NPC npc, int[] buffSet)
        {
            int num = 0;
            for (int i = 0; i < buffSet.Length; i++)
            {
                int buffIndex = npc.FindBuffIndex(buffSet[i]);
                if (buffIndex != -1)
                {
                    num++;
                }
            }
            return num;
        }
        public static bool PlayerHasBuffInBuffSet(Player player, int[] buffSet)
        {
            for (int i = 0; i < buffSet.Length; i++)
            {
                if (player.HasBuff(buffSet[i]))
                {
                    return true;
                }
            }
            return false;
        }
        public static int PlayerBuffNumInBuffSet(Player player, int[] buffSet)
        {
            int num = 0;
            for (int j = 0; j < buffSet.Length; j++)
            {
                int buffIndex = player.FindBuffIndex(buffSet[j]);
                if (buffIndex != -1)
                {
                    num++;
                }
            }
            return num;
        }
        public static int BuffIDToLifeRegen(int buffID)
        {
            switch (buffID)
            {
                default: 
                    return 0;
                case BuffID.OnFire:
                    return -8;
                case BuffID.OnFire3:
                    return -30;
                case BuffID.Frostburn:
                    return -16;
                case BuffID.Frostburn2:
                    return -50;
                case BuffID.CursedInferno:
                    return -48;
                case BuffID.ShadowFlame:
                    return -30;
                case BuffID.Daybreak:
                    return -50;
                case BuffID.Burning:
                    return -100;
                case BuffID.Poisoned:
                    return -4;
                case BuffID.Venom:
                    return -60;
                case BuffID.BoneJavelin:
                    return -6;
                case BuffID.Electrified:
                    return -40;
                case BuffID.DryadsWardDebuff:
                    return -14;
                case BuffID.Suffocation:
                    return -40;
                case BuffID.Bleeding:
                    return -6;//Does not give negative lifeRegen in vanilla code
                case BuffID.TheTongue:
                    return -100;
            }
        }
    }
}
