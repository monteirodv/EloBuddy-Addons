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

namespace Diana___Scorn_of_the_Moon
{
    class Program
    {
        private static AIHeroClient Player { get { return ObjectManager.Player; } }

        private static Menu RootMenu, ComboMenu, HarassMenu, FarmingMenu, ksMenu, DrawingsMenu;

        public static Spell.Skillshot Q;
        public static Spell.Active W;
        public static Spell.Active E;
        public static Spell.Targeted R;
        static void Main(string[] args)
        {

            Loading.OnLoadingComplete += Loading_OnLoadingComplete;

        }

        private static void Loading_OnLoadingComplete(EventArgs args)
        {
            Q = new Spell.Skillshot(SpellSlot.Q, 830, SkillShotType.Linear, 250, 1400, 50);
            Q.AllowedCollisionCount = int.MaxValue;
            W = new Spell.Active(SpellSlot.W, 250);
            E = new Spell.Active(SpellSlot.E, 350);
            R = new Spell.Targeted(SpellSlot.R, 825);

            RootMenu = MainMenu.AddMenu("Diana - Scorn of the moon", "Diana - Scorn of the moon");

            ComboMenu = RootMenu.AddSubMenu("Combo", "Combo");

            ComboMenu.Add("UseQ", new CheckBox("Use Q"));
            ComboMenu.Add("UseW", new CheckBox("Use W"));
            ComboMenu.Add("UseE", new CheckBox("Use E "));
            ComboMenu.Add("UseR", new CheckBox("Use R "));

            HarassMenu = RootMenu.AddSubMenu("Harass", "Harass");

            HarassMenu.Add("UseQ", new CheckBox("Use Q"));
            HarassMenu.Add("UseW", new CheckBox("Use W when in range"));


            FarmingMenu = RootMenu.AddSubMenu("Farming", "farming");

            FarmingMenu.Add("Qclear", new CheckBox("Use Q to clear wave"));
            FarmingMenu.Add("Wclear", new CheckBox("Use W to clear wave"));
            FarmingMenu.Add("Qclearmana", new Slider("W mana to clear %", 30, 0, 100));

            FarmingMenu.Add("Wclearmana", new Slider("W mana to clear %", 30, 0, 100));

            FarmingMenu.Add("Qclearjg", new CheckBox("Use Q to clear jungle"));
            FarmingMenu.Add("Wclearjg", new CheckBox("Use W to clear jungle"));
            FarmingMenu.Add("Rclearjg", new CheckBox("Use R to clear jungle"));


            DrawingsMenu = RootMenu.AddSubMenu("Drawings", "Drawings");

            DrawingsMenu.Add("DrawQ", new CheckBox("Draw Q range"));
            DrawingsMenu.Add("DrawW", new CheckBox("Draw W range"));
            DrawingsMenu.Add("DrawE", new CheckBox("Draw E range"));
            DrawingsMenu.Add("DrawWpred", new CheckBox("Draw Q prediction"));

            Game.OnTick += Game_OnTick;
            Drawing.OnDraw += Drawing_OnDraw;

        }

        private static void Game_OnTick(EventArgs args)
        {
            switch (Orbwalker.ActiveModesFlags)
            {
                case Orbwalker.ActiveModes.Combo:
                    Combo();
                    break;
                case Orbwalker.ActiveModes.Harass:
                    Harass();
                    break;
                case Orbwalker.ActiveModes.LaneClear:
                    LaneClear();
                    break;
                case Orbwalker.ActiveModes.JungleClear:
                    JungleClear();
                    break;
                case Orbwalker.ActiveModes.LastHit:
                 //   LastHit();
                    break;
                case Orbwalker.ActiveModes.None:
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private static void Drawing_OnDraw(EventArgs args)
        {
            var target = TargetSelector.GetTarget(Q.Range, DamageType.Physical);
            if (DrawingsMenu["DrawQ"].Cast<CheckBox>().CurrentValue)
            {
                Circle.Draw(Color.Aqua, Q.Range, Player);
            }
            if (DrawingsMenu["DrawW"].Cast<CheckBox>().CurrentValue)
            {
                Circle.Draw(Color.Aqua, W.Range, Player);
            }
            if (DrawingsMenu["DrawE"].Cast<CheckBox>().CurrentValue)
            {
                Circle.Draw(Color.Aqua, E.Range, Player);
            }
            if (DrawingsMenu["DrawQpred"].Cast<CheckBox>().CurrentValue)
            {
                if (target == null)
                    return;
                Drawing.DrawCircle(Q.GetPrediction(target).CastPosition, 150, System.Drawing.Color.Red);

            }
        }

        private static void Combo()
        {
            var target = TargetSelector.GetTarget(Q.Range, DamageType.Magical);

            if (target == null)
                return;
            if (ComboMenu["UseQ"].Cast<CheckBox>().CurrentValue)
            {
                if (target.IsInRange(Player, Q.Range) && Q.IsReady())
                {

                    if (Q.GetPrediction(target).HitChance >= HitChance.High)
                    {
                        Q.Cast(target);
                    }


                }
            }
            if (ComboMenu["UseW"].Cast<CheckBox>().CurrentValue)
            {
                if (target.IsInRange(Player, W.Range) && W.IsReady())
                {

                    W.Cast();



                }
            }
            if (ComboMenu["UseE"].Cast<CheckBox>().CurrentValue)
            {
                if (target.IsInRange(Player, E.Range) && E.IsReady())
                {

                    E.Cast();


                }
            }
            if (ComboMenu["UseR"].Cast<CheckBox>().CurrentValue)
            {
                if (target.IsInRange(Player, R.Range) && !Q.IsReady()) 
                {

                    R.Cast(target);



                }
            }


        }
        private static void Harass()
        {
            var target = TargetSelector.GetTarget(Q.Range, DamageType.Magical);

            if (target == null)
                return;
            if (HarassMenu["UseQ"].Cast<CheckBox>().CurrentValue)
            {
                if (target.IsInRange(Player, Q.Range) && Q.IsReady())
                {

                    if (Q.GetPrediction(target).HitChance >= HitChance.High)
                    {
                        Q.Cast(target);
                    }


                }
            }
            if (HarassMenu["UseW"].Cast<CheckBox>().CurrentValue)
            {
                if (target.IsInRange(Player, W.Range) && W.IsReady())
                {

                    W.Cast();



                }
            }



        }

        private static void LaneClear()
        {
            if (FarmingMenu["Qclearmana"].Cast<Slider>().CurrentValue <= Player.ManaPercent)
            {
                var minion1 = EntityManager.MinionsAndMonsters.EnemyMinions.FirstOrDefault(m => m.IsValidTarget(Q.Range));

                Q.Cast(minion1);

            }
            if (FarmingMenu["Wclearmana"].Cast<Slider>().CurrentValue <= Player.ManaPercent)
            {
                var minion1 = EntityManager.MinionsAndMonsters.EnemyMinions.FirstOrDefault(m => m.IsValidTarget(W.Range));

                W.Cast();

            }

        }
        private static void JungleClear()
        {

            if (FarmingMenu["Qclearjg"].Cast<CheckBox>().CurrentValue)
            {
                var monster = EntityManager.MinionsAndMonsters.GetJungleMonsters(Player.Position, Q.Range);
                Q.Cast(monster.First());
            }
            if (FarmingMenu["Wclearjg"].Cast<CheckBox>().CurrentValue)
            {
                var monster = EntityManager.MinionsAndMonsters.GetJungleMonsters(Player.Position, W.Range);
                W.Cast();
            }
            if (FarmingMenu["Rclearjg"].Cast<CheckBox>().CurrentValue)
            {
                var monster = EntityManager.MinionsAndMonsters.Monsters.FirstOrDefault(m => m.IsValidTarget(R.Range));
                R.Cast(monster);
            }

        }
    }
}