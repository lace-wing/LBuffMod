using Microsoft.Xna.Framework;
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
        public bool sourceIsNotNull;
        public NPC npc;
        public Player player;
        public Item sourceItem;
        public override void OnSpawn(Projectile projectile, IEntitySource source)
        {
            if (source is EntitySource_Parent)
            {
                EntitySource_Parent source_Parent = source as EntitySource_Parent;
                if (source_Parent.Entity is NPC)
                {
                    npc = (NPC)source_Parent.Entity;
                    sourceIsNotNull = true;
                }
            }
            if (source is EntitySource_ItemUse)
            {
                EntitySource_ItemUse source_ItemUse = source as EntitySource_ItemUse;
                if (source_ItemUse.Item is Item)
                {
                    sourceItem = (Item)source_ItemUse.Item;
                    sourceIsNotNull = true;
                }
            }
        }
        public override void ModifyHitPlayer(Projectile projectile, Player target, ref int damage, ref bool crit)
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
        public override void PostAI(Projectile projectile)
        {
            //挥发明胶射弹修改
            if (projectile.type == ProjectileID.VolatileGelatinBall && projectile.friendly && (sourceIsNotNull ? sourceItem.type == ItemID.VolatileGelatin : false))
            {
                if (projectile.scale > 1.5f)
                {
                    projectile.scale -= 0.003f;
                }
                projectile.position.X += projectile.velocity.X * 0.4f;
                projectile.position.Y -= projectile.velocity.Y * 0.1f;
                NPC targetNPC = projectile.FindTargetWithinRange(480);
                if (targetNPC != null)
                {
                    int hX = (int)(targetNPC.Center.X - projectile.Center.X);
                    int hY = (int)(targetNPC.Center.Y - projectile.Center.Y);
                    if (hX < 120 && hX > -120)
                    {
                        hX = hX > 0 ? 120 : -120;
                    }
                    if (hY < 120 && hY > -120)
                    {
                        hY = hY > 0 ? 120 : -120;
                    }
                    Vector2 homingVelocity = new Vector2(hX, hY) * MathHelper.Lerp(0.0003f, 0.0036f, new Vector2(hX, hY).Length() / 480f);
                    projectile.velocity += homingVelocity;
                    if (targetNPC.Center.Y < projectile.Center.Y)
                    {
                        projectile.velocity += homingVelocity * 0.0012f;
                    }
                }
            }
        }
    }
}
