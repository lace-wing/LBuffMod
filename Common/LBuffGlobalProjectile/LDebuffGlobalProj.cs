using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.DataStructures;

namespace LBuffMod.Common.LBuffGlobalProjectile
{
    public class LDebuffGlobalProj : GlobalProjectile
    {
        public override bool InstancePerEntity => true;
        public NPC npc;
        public Item item;
        public override void OnSpawn(Projectile projectile, IEntitySource source)
        {
            if (source is EntitySource_Parent)
            {
                EntitySource_Parent source_Parent = source as EntitySource_Parent;
                if (source_Parent.Entity is NPC)
                {
                    npc = (NPC)source_Parent.Entity;
                }
            }
        }
        public override void OnHitPlayer(Projectile projectile, Player target, int damage, bool crit)
        {
            if (npc == null)
            {
                return;
            }
            #region Pre-hard Mode projs inflicting damaging debuffs
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
