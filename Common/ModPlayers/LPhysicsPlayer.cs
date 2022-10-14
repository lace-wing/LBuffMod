using LBuffMod.Common.Systems;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;

namespace LBuffMod.Common.ModPlayers
{
    public class LPhysicsPlayer : ModPlayer
    {
        public Vector2[] plrVelocity = new Vector2[16];
        public Vector2[] plrAcceleration = new Vector2[16];
        public float plrMass = 16;

        public int buffUpdateTime;

        public bool swiftnessSlipping;
        public bool longerPotionSickness;

        public bool canBeAttractedByMagnet;
        public bool equippedMagnet;
        public int polarity;
        public float plrMFS = 0;
        public bool equippedMagnetFlower;

        public override void ResetEffects()
        {
            plrMass = 16;

            if (Player.CountBuffs() == 0 || buffUpdateTime < 0 || buffUpdateTime >= int.MaxValue)
            {
                buffUpdateTime = 0;
            }

            swiftnessSlipping = false;
            longerPotionSickness = false;

            canBeAttractedByMagnet = false;
            equippedMagnet = false;
            polarity = 0;
            plrMFS = 0;
            equippedMagnetFlower = false;
        }
        public override void PostUpdate()
        {
            #region Set plrVelocity & plrAcceleration
            for (int i = 1; i < plrVelocity.Length - 1; i++)
            {
                plrVelocity[i] = plrVelocity[i - 1];
            }
            plrVelocity[0] = Player.velocity;
            for (int j = 1; j < plrAcceleration.Length - 1; j++)
            {
                plrAcceleration[j] = plrAcceleration[j - 1];
            }
            plrAcceleration[0] = plrVelocity[0] - plrVelocity[1];
            #endregion

            CheckIfPlrIsMagnetic(Player);
        }
        public override void PostUpdateBuffs()
        {
            if (Player.HasBuff(BuffID.Swiftness))
            {
                swiftnessSlipping = true;
            }
            if (Player.HasBuff(BuffID.Regeneration))
            {
                longerPotionSickness = true;
            }
            if (Player.HasBuff(BuffID.Ironskin))
            {
                canBeAttractedByMagnet = true;
                plrMFS += 4;
                Player.moveSpeed -= 0.1f;
                Player.GetAttackSpeed(DamageClass.Melee) -= 0.05f;
                Player.statDefense += 4;
            }
        }

        internal void CheckIfPlrIsMagnetic(Player player)
        {
            if (player == null || !player.active)
            {
                LPhysicsSystem.magneticEntity.Remove(Player);
            }

            if (!LPhysicsSystem.magneticEntity.Contains(Player) && (equippedMagnet || canBeAttractedByMagnet))
            {
                LPhysicsSystem.magneticEntity.Add(Player);
            }
            else if (LPhysicsSystem.magneticEntity.Contains(Player) && !equippedMagnet && !canBeAttractedByMagnet)
            {
                LPhysicsSystem.magneticEntity.Remove(Player);
            }
        }

        public override void Kill(double damage, int hitDirection, bool pvp, PlayerDeathReason damageSource)
        {
            if (LPhysicsSystem.magneticEntity.Contains(Player))
            {
                LPhysicsSystem.magneticEntity.Remove(Player);
            }
        }
    }
}
