using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.ID;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using LBuffMod.Common.ModPlayers;
using LBuffMod.Common.Systems;

namespace LBuffMod.Common.GlobalNPCs
{
    public class LPhysicsGlobalNPC : GlobalNPC
    {
        public override bool InstancePerEntity => true;

        public float npcMass = 16;

        public bool canBeAttractedByMagnet = false;
        public bool hasMagnetism = false;
        public int polarity = 0;
        public float npcMFS = 0;
        public bool magnefiedByMagnetFlower;

        public override void ResetEffects(NPC npc)
        {
            npcMass = 16;

            canBeAttractedByMagnet = false;
            hasMagnetism = false;
            polarity = 0;
            npcMFS = 0;
            magnefiedByMagnetFlower = false;
        }

        internal void CheckIfNPCIsMagnetic(NPC npc)
        {
            if ((npc == null || !npc.active) && LPhysicsSystem.magneticEntity.Contains(npc))
            {
                LPhysicsSystem.magneticEntity.Remove(npc);
            }

            if (!LPhysicsSystem.magneticEntity.Contains(npc) && (hasMagnetism || canBeAttractedByMagnet))
            {
                LPhysicsSystem.magneticEntity.Add(npc);
            }
            if (LPhysicsSystem.magneticEntity.Contains(npc) && !hasMagnetism && !canBeAttractedByMagnet)
            {
                LPhysicsSystem.magneticEntity.Remove(npc);
            }
        }

        internal void SetNPCMagProperty(NPC npc)
        {
            foreach (Player player in Main.player)
            {
                if (player.active)
                {
                    LPhysicsPlayer lbPlr = player.GetModPlayer<LPhysicsPlayer>();
                    if (Vector2.DistanceSquared(npc.Center, player.Center) <= 129600 && lbPlr.equippedMagnetFlower)
                    {
                        magnefiedByMagnetFlower = true;
                    }
                }
            }

            if (magnefiedByMagnetFlower)
            {
                canBeAttractedByMagnet = true;
                npcMFS -= 1.2f;
            }
        }

        public override void PostAI(NPC npc)
        {
            SetNPCMagProperty(npc);

            CheckIfNPCIsMagnetic(npc);
        }
    }
}
