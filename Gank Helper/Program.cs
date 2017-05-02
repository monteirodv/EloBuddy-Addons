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

        private static Menu RootMenu;

        static void Main(string[] args)
        {

            Loading.OnLoadingComplete += Loading_OnLoadingComplete;

        }

        private static void Loading_OnLoadingComplete(EventArgs args)
        {


            RootMenu = MainMenu.AddMenu("Gank Helper", "GankHelper");
            RootMenu.Add("healthpercent", new Slider("Health percent to show line", 35, 1, 100));


            Game.OnTick += Game_OnTick;
            Drawing.OnDraw += Drawing_OnDraw;

        }

        private static void Game_OnTick(EventArgs args)
        {


        }

        private static void Drawing_OnDraw(EventArgs args)
        {
            int extra = 0;
            foreach(var enemy in EntityManager.Heroes.Enemies.Where(e => e.IsVisible && e.HealthPercent <= RootMenu["healthpercent"].Cast<Slider>().CurrentValue))
            {
                extra -= 30;
                {
                    if (enemy.IsValidTarget() && !enemy.IsDead)
                    {
                        Drawing.DrawLine(ObjectManager.Player.Position.WorldToScreen(), enemy.Position.WorldToScreen(), 5, System.Drawing.Color.Green);
                        var mypos = Drawing.WorldToScreen(ObjectManager.Player.Position);
                        Drawing.DrawText(mypos.X - 10, mypos.Y - extra, System.Drawing.Color.Red, "Can Gank:" + enemy.ChampionName.ToString() + " HP:" + enemy.HealthPercent.ToString() + "%");
                        
                    }
                    } //oi
                }
        }
    }
}
