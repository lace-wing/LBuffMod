using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LBuffMod.Common.ModPlayers;

namespace LBuffMod.Content.Buffs
{
    public class Madness : ModBuff
    {
        public override void SetStaticDefaults()
        {
            Main.debuff[Type] = true;
            Main.pvpBuff[Type] = true;
            Main.buffNoSave[Type] = true;
        }
        public override void Update(Player player, ref int buffIndex)
        {
            player.GetModPlayer<LDebuffPlayer>().madnessDebuff = true;
        }
    }
}
