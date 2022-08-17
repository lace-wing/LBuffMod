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
            if (type == LBuffUtilities.GetAllElements(LBuffUtilities.damagingDebuffsToBuff))
            {
                //若不足12分钟则叠加持续时间
                if (npc.buffTime[type] < 43200)
                {
                    Main.NewText(type);
                    npc.buffTime[type] += time;
                    return true;
                }
            }
            return base.ReApply(type, npc, time, buffIndex);
        }
    }
}
