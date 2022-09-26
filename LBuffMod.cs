global using Terraria;
global using Terraria.ModLoader;
global using Terraria.ID;
using Terraria.Localization;
using System.Globalization;
using LBuffMod.Common.Utilities;
using static LBuffMod.Common.Utilities.LLocalizationUtils;

namespace LBuffMod
{
	public class LBuffMod : Mod
	{
        //public override uint ExtraNPCBuffSlots => 9;
        public override void PostSetupContent()
        {
            if (Main.netMode != NetmodeID.Server)
            {
                if (Main.rand.Next(3) >= 0)
                {
                    int yearUTC = System.DateTime.UtcNow.Year;
                    int monthUTC = System.DateTime.UtcNow.Month;
                    int dayUTC = System.DateTime.UtcNow.Day;
                    int hourUTC = System.DateTime.UtcNow.Hour;
                    int minuteUTC = System.DateTime.UtcNow.Minute;
                    int secondUTC = System.DateTime.UtcNow.Second;

                    string titleTime = $"UTC PostSetupContent time: {yearUTC}/{monthUTC}/{dayUTC} {hourUTC}:{minuteUTC}:{secondUTC}";
                    try
                    {
                        Main.instance.Window.Title = titleTime;
                    }
                    catch { }
                }
            }
        }
    }
}