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
                        if (LBuffUtils.lDamagingDebuffs[i] == BuffID.Burning)
                        {
                            npc.buffTime[buffIndex] += (int)(time * 0.2f);
                        }
                        else npc.buffTime[buffIndex] += time;
                        return true;
                    }
                }
            }
            return base.ReApply(type, npc, time, buffIndex);
        }
        public override bool ReApply(int type, Player player, int time, int buffIndex)
        {
            if (!player.dead)
            {
                for (int i = 0; i < LBuffUtils.lDamagingDebuffs.Length; i++)
                {
                    if (type == LBuffUtils.lDamagingDebuffs[i])
                    {
                        //若不足12分钟则叠加持续时间
                        if (player.buffTime[buffIndex] < 43200)
                        {
                            if (LBuffUtils.lDamagingDebuffs[i] == BuffID.Burning)
                            {
                                player.buffTime[buffIndex] += (int)(time * 0.2f);
                            }
                            else if (LBuffUtils.lDamagingDebuffs[i] == BuffID.Bleeding && player.buffTime[buffIndex] >= 900)
                            {
                                player.buffTime[buffIndex] += (int)(time * 0.01f);
                            }
                            else player.buffTime[buffIndex] += time;
                            return true;
                        }
                    }
                }
            }
            return base.ReApply(type, player, time, buffIndex);
        }
        public override void Update(int type, NPC npc, ref int buffIndex)
        {
            //带电、灼烧更快流失
            if (type == BuffID.Electrified || type == BuffID.Burning)
            {
                npc.buffTime[buffIndex] -= 1;
            }
        }
        public override void Update(int type, Player player, ref int buffIndex)
        {
            //带电、灼烧更快流失
            if (type == BuffID.Electrified || type == BuffID.Burning)
            {
                player.buffTime[buffIndex] -= 1;
            }
        }
    }
}
