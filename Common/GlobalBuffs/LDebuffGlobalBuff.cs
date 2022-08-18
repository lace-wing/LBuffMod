using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LBuffMod.Common.Utilities;

namespace LBuffMod.Common.GlobalBuffs
{
    public class LDebuffGlobalBuff : GlobalBuff
    {
        public override bool ReApply(int type, NPC npc, int time, int buffIndex)
        {
            for (int i = 0; i < LBuffUtils.lDamagingDebuffs.Length; i++)
            {
                if (type == LBuffUtils.lDamagingDebuffs[i])
                {
                    //若不足12分钟则叠加持续时间
                    if (npc.buffTime[buffIndex] < 43200)
                    {
                        npc.buffTime[buffIndex] += time;
                        return true;
                    }
                }
            }
            return base.ReApply(type, npc, time, buffIndex);
        }
    }
}
