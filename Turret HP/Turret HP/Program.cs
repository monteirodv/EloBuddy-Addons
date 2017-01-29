using System;
using System.Collections.Generic;
using System.Linq;
using EloBuddy;
using EloBuddy.SDK;
using EloBuddy.SDK.Enumerations;
using EloBuddy.SDK.Events;
using EloBuddy.SDK.Menu;
using EloBuddy.SDK.Menu.Values;
using EloBuddy.SDK.Rendering;
using SharpDX;


namespace AddonTemplate
{

    class Program
    {
        private static AIHeroClient Player { get { return ObjectManager.Player; } }

        private static Menu RootMenu;

        static void Main(string[] args)
        {

            Loading.OnLoadingComplete += Loading_OnLoadingComplete;

        }

        private static void Loading_OnLoadingComplete(EventArgs args)
        {


            RootMenu = MainMenu.AddMenu("Turret HP%", "Turret HP%");
            RootMenu.Add("Enabled", new KeyBind("Enabled", false, KeyBind.BindTypes.PressToggle, "T".ToCharArray()[0]));


            Game.OnTick += Game_OnTick;
            Drawing.OnDraw += Drawing_OnDraw;
            Drawing.OnEndScene += Drawing_OnEndScene;
        }

        private static void Game_OnTick(EventArgs args)
        {

            if (RootMenu.Get<KeyBind>("Enabled").CurrentValue)
            {

            }
        }

        static void Drawing_OnEndScene(EventArgs args)
        {
            foreach (var turrets in EntityManager.Turrets.AllTurrets)
            {
                var turretshp = Math.Round(turrets.HealthPercent);

                var turretsmap = turrets.Position.WorldToMinimap();
                Drawing.DrawText(turretsmap.X, turretsmap.Y, System.Drawing.Color.LightGreen, turretshp.ToString() + "%");
            }
        }
        private static void Drawing_OnDraw(EventArgs args)
        {


        }
    }


}

