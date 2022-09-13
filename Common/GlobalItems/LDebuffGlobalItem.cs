using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LBuffMod.Common.ModPlayers;
using Terraria.Localization;

namespace LBuffMod.Common.GlobalItems
{
    public class LDebuffGlobalItem : GlobalItem
    {
        public override void UpdateAccessory(Item item, Player player, bool hideVisual)
        {
            if (item.type == ItemID.RoyalGel)
            {
                player.GetDamage(DamageClass.Generic) -= 0.1f;
                player.GetModPlayer<LDebuffPlayer>().royalGelOnFire = true;
            }
            if (item.type == ItemID.SharkToothNecklace)
            {
                player.GetModPlayer<LDebuffPlayer>().sharkToothNecklaceBleeding = true;
                player.GetDamage(DamageClass.Generic).Base -= 1;
            }
            if (item.type == ItemID.StingerNecklace)
            {
                player.GetModPlayer<LDebuffPlayer>().stingerNecklaceBleedingAndPoison = true;
                player.GetDamage(DamageClass.Generic).Base -= 1;
            }
            if (item.type == ItemID.VolatileGelatin)
            {
                player.GetDamage(DamageClass.Generic) -= 0.15f;
                player.GetModPlayer<LDebuffPlayer>().volatileGelatinFire = true;
            }
        }
        public override void UpdateInventory(Item item, Player player)
        {
            //毁灭刃
            if (item.type == ItemID.BreakerBlade)
            {
                if (item.scale < 1.6f)
                {
                    item.scale = 1.6f;
                } 
                if (item.scale > 1.6f)
                {
                    item.scale -= 0.002f;
                }
            }
        }
        public override void ModifyTooltips(Item item, List<TooltipLine> tooltips)
        {
            //Accessories
            if (item.type == ItemID.RoyalGel)
            {
                tooltips.Add(new(Mod, "RoyalGelFireTooltip", Language.GetTextValue("Mods.LBuffMod.TooltipLine.RoyalGelFireTooltip")));
            }
            if (item.type == ItemID.VolatileGelatin)
            {
                tooltips.Add(new(Mod, "VolatileGelatinFireNOil", Language.GetTextValue("Mods.LBuffMod.TooltipLine.VolatileGelatinFireNOil")));
            }
            if (item.type == ItemID.SharkToothNecklace)
            {
                tooltips.Add(new(Mod, "SharkToothNecklaceBleedingTooltip", Language.GetTextValue("Mods.LBuffMod.TooltipLine.SharkToothNecklaceBleedingTooltip")));
            }
            if (item.type == ItemID.StingerNecklace)
            {
                tooltips.Add(new(Mod, "StingerNecklaceBleedingAndPoisonTooltip", Language.GetTextValue("Mods.LBuffMod.TooltipLine.StingerNecklaceBleedingAndPoisonTooltip")));
            }
            //Weapons
            if (item.type == ItemID.BreakerBlade)
            {
                tooltips.Add(new(Mod, "BreakerBladeBuffsTooltip", Language.GetTextValue("Mods.LBuffMod.TooltipLine.BreakerBladeBuffsTooltip")));
            }
            if (item.type == ItemID.VampireFrogStaff || item.type == ItemID.BloodRainBow || item.type == ItemID.SanguineStaff || item.type == ItemID.DripplerFlail)
            {
                tooltips.Add(new(Mod, "BloodWeaponsTooltip", Language.GetTextValue("Mods.LBuffMod.TooltipLine.BloodWeaponsTooltip")));
            }
            //Armors
            if (item.type == ItemID.WoodHelmet)
            {
                tooltips.Add(new(Mod, "WoodHelmetTooltip", Language.GetTextValue("Mods.LBuffMod.TooltipLine.WoodHelmetTooltip")));
            }
            if (item.type == ItemID.RichMahoganyHelmet)
            {
                tooltips.Add(new(Mod, "RichMahoganyHelmetTooltip", Language.GetTextValue("Mods.LBuffMod.TooltipLine.RichMahoganyHelmetTooltip")));
            }
            if (item.type == ItemID.BorealWoodHelmet)
            {
                tooltips.Add(new(Mod, "BorealWoodHelmetTooltip", Language.GetTextValue("Mods.LBuffMod.TooltipLine.BorealWoodHelmetTooltip")));
            }
            if (item.type == ItemID.PalmWoodHelmet)
            {
                tooltips.Add(new(Mod, "PalmWoodHelmetTooltip", Language.GetTextValue("Mods.LBuffMod.TooltipLine.PalmWoodHelmetTooltip")));
            }
            if (item.type == ItemID.EbonwoodHelmet)
            {
                tooltips.Add(new(Mod, "EbonwoodHelmetTooltip", Language.GetTextValue("Mods.LBuffMod.TooltipLine.EbonwoodHelmetTooltip")));
            }
            if (item.type == ItemID.ShadewoodHelmet)
            {
                tooltips.Add(new(Mod, "ShadewoodHelmetTooltip", Language.GetTextValue("Mods.LBuffMod.TooltipLine.ShadewoodHelmetTooltip")));
            }
            if (item.type == ItemID.PearlwoodHelmet)
            {
                tooltips.Add(new(Mod, "PearlwoodHelmetTooltip", Language.GetTextValue("Mods.LBuffMod.TooltipLine.PearlwoodHelmetTooltip")));
            }
            if (item.type == ItemID.SpookyHelmet)
            {
                tooltips.Add(new(Mod, "SpookyHelmetTooltip", Language.GetTextValue("Mods.LBuffMod.TooltipLine.SpookyHelmetTooltip")));
            }
        }
    }
}
