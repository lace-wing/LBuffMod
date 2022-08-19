using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LBuffMod.Common.LBuffGlobalProjectile
{
    public class LDebuffGlobalProj : GlobalProjectile
    {
        public override void OnHitPlayer(Projectile projectile, Player target, int damage, bool crit)
        {
            NPC npc = Main.npc[projectile.owner];
            #region Pre-hard Mode projs inflicting damaging debuffs
            //世吞、大中小噬魂怪、腐化者
            if (npc.type == NPCID.EaterofWorldsBody || npc.type == NPCID.EaterofWorldsHead || npc.type == NPCID.EaterofWorldsTail || npc.type == NPCID.EaterofSouls || npc.type == NPCID.BigEater || npc.type == NPCID.LittleEater || npc.type == NPCID.Corruptor)
            {
                if (!Main.hardMode)
                {
                    target.AddBuff(BuffID.OnFire, 48);//96 in expert world, 120 in master world
                }
                if (Main.hardMode)
                {
                    target.AddBuff(BuffID.CursedInferno, 48);
                }
            }
            //哥布林鲨、血鱿鱼、恐惧鹦鹉螺
            if (npc.type == NPCID.GoblinShark || npc.type == NPCID.BloodSquid || npc.type == NPCID.BloodNautilus)
            {
                target.AddBuff(BuffID.Bleeding, 120);
            }
            //血肉墙、饿鬼、血蛭
            if (npc.type == NPCID.TheHungry || npc.type == NPCID.TheHungryII || npc.type == NPCID.WallofFlesh || npc.type == NPCID.WallofFleshEye || npc.type == NPCID.LeechHead || npc.type == NPCID.LeechBody || npc.type == NPCID.LeechTail)
            {
                if (!Main.hardMode)
                {
                    target.AddBuff(BuffID.OnFire3, 48);
                }
                if (Main.hardMode)
                {
                    target.AddBuff(BuffID.CursedInferno, 48);
                }
            }
            #endregion
            #region Hard mode projs inflicting damaging debuffs
            //机械骷髅王、激光眼、毁灭者身体&尾
            if (npc.type == NPCID.SkeletronPrime || npc.type == NPCID.PrimeCannon || npc.type == NPCID.PrimeLaser || npc.type == NPCID.PrimeSaw || npc.type == NPCID.PrimeVice || npc.type == NPCID.Retinazer || npc.type == NPCID.TheDestroyerBody || npc.type == NPCID.TheDestroyerTail)
            {
                target.AddBuff(BuffID.OnFire, 48);
            }
            //魔焰眼、毁灭者头
            if (npc.type == NPCID.Spazmatism || npc.type == NPCID.TheDestroyer)
            {
                target.AddBuff(BuffID.OnFire, 96);
            }
            #endregion
            //TODO More projs to inflict debuffs!!!
        }
    }
}
