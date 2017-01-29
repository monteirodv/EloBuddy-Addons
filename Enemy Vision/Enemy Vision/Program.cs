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


            RootMenu = MainMenu.AddMenu("Enemy Vision", "Enemy Vision");
            RootMenu.Add("Enabled", new KeyBind("Enabled", false, KeyBind.BindTypes.PressToggle, "I".ToCharArray()[0]));


            Game.OnTick += Game_OnTick;
            Drawing.OnDraw += Drawing_OnDraw;

        }

        private static void Game_OnTick(EventArgs args)
        {

            if (RootMenu.Get<KeyBind>("Enabled").CurrentValue)
            {
 
            }
        }


        private static void Drawing_OnDraw(EventArgs args)
        {
            foreach (var enemy in EntityManager.Heroes.Enemies)
            {

                enemy.DrawCircle(1200, System.Drawing.Color.Red, 5);
            }

        }
    }


}

