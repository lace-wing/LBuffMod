using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LBuffMod.Common.GlobalBuffs
{
    public class LDebuffGlobalBuff : GlobalBuff
    {
        public override bool ReApply(int type, NPC npc, int time, int buffIndex)
        {
            //若不足10分钟则叠加持续时间，包括：着火了！、狱火、霜火、霜噬、诅咒焰、暗影焰、日耀、中毒、毒液
            if (type == BuffID.OnFire || type == BuffID.OnFire3 || type == BuffID.Frostburn || type == BuffID.Frostburn2 || type == BuffID.CursedInferno || type == BuffID.ShadowFlame || type == BuffID.Daybreak || type == BuffID.Poisoned || type == BuffID.Venom)
            {
                if (npc.buffTime[type] < 6000)
                {
                    npc.buffTime[type] += time;
                    return true;
                }
            }
            return base.ReApply(type, npc, time, buffIndex);
        }
        public override void Update(int type, NPC npc, ref int buffIndex)
        {
            //着火了！、狱火、霜火、霜噬、诅咒焰、暗影焰、日耀、中毒、毒液
            if (type == BuffID.OnFire || type == BuffID.OnFire3 || type == BuffID.Frostburn || type == BuffID.Frostburn2 || type == BuffID.CursedInferno || type == BuffID.ShadowFlame || type == BuffID.Daybreak || type == BuffID.Poisoned || type == BuffID.Venom)
            {
                npc.lifeRegen -= Math.Clamp(npc.buffTime[type] / 30, 0, 30);
            }
        }
    }
}
