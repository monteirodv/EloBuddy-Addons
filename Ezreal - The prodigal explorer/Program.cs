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

        private static Menu RootMenu, ComboMenu, HarassMenu, FarmingMenu, ksMenu, DrawingsMenu;

        public static Spell.Skillshot Q;
        public static Spell.Skillshot W;
        public static Spell.Skillshot E;
        public static Spell.Skillshot R;
        static void Main(string[] args)
        {

            Loading.OnLoadingComplete += Loading_OnLoadingComplete;

        }

        private static void Loading_OnLoadingComplete(EventArgs args)
        {
            Q = new Spell.Skillshot(SpellSlot.Q, 1150, SkillShotType.Linear, 250, 2000, 60);
            W = new Spell.Skillshot(SpellSlot.W, 1000, SkillShotType.Linear, 250, 1600, 80);
            W.AllowedCollisionCount = int.MaxValue;
            E = new Spell.Skillshot(SpellSlot.E, 475, SkillShotType.Linear);
            E.AllowedCollisionCount = int.MaxValue;
            R = new Spell.Skillshot(SpellSlot.R, 3000, SkillShotType.Linear, 1000, 2000, 160);
            R.AllowedCollisionCount = int.MaxValue;

            RootMenu = MainMenu.AddMenu("Ezreal - The Prodigal Explorer", "Ezreal - The Prodigal Explorer");

            ComboMenu = RootMenu.AddSubMenu("Combo", "Combo");

            ComboMenu.Add("UseQ", new CheckBox("Use Q"));
            ComboMenu.Add("UseW", new CheckBox("Use W"));
            ComboMenu.Add("UseE", new CheckBox("Use E Offensively"));
            ComboMenu.Add("UseR", new CheckBox("Use R in combo"));

            HarassMenu = RootMenu.AddSubMenu("Harass", "Harass");

            HarassMenu.Add("UseQ", new CheckBox("Use Q"));
            HarassMenu.Add("UseW", new CheckBox("Use W"));


            FarmingMenu = RootMenu.AddSubMenu("Farming", "farming");

            FarmingMenu.Add("Qlast", new CheckBox("Use Q to last hit"));
            FarmingMenu.Add("Qclear", new CheckBox("Use Q to clear wave"));
            FarmingMenu.Add("Qclearmana", new Slider("Q mana to clear %", 30, 0, 100));
            FarmingMenu.Add("Qlastmana", new Slider("Q mana to last hit %", 30, 0, 100));


            DrawingsMenu = RootMenu.AddSubMenu("Drawings", "Drawings");

            DrawingsMenu.Add("DrawQ", new CheckBox("Draw Q range"));
            DrawingsMenu.Add("DrawW", new CheckBox("Draw W range"));
            DrawingsMenu.Add("DrawE", new CheckBox("Draw E range"));
            DrawingsMenu.Add("DrawWpred", new CheckBox("Draw W prediction"));

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
                case Orbwalker.ActiveModes.LastHit:
                    LastHit();
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
            if (DrawingsMenu["DrawWpred"].Cast<CheckBox>().CurrentValue)
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
                if (target.Distance(ObjectManager.Player) <= Q.Range && Q.IsReady())
                {
                    
                    if (Q.GetPrediction(target).HitChance >= HitChance.High)
                    {
                        Q.Cast(Q.GetPrediction(target).CastPosition);
                    }


                }
            }
            if (ComboMenu["UseW"].Cast<CheckBox>().CurrentValue)
            {
                if (target.Distance(ObjectManager.Player) <= W.Range && W.IsReady())
                {

                    W.Cast(W.GetPrediction(target).CastPosition);
           


                }
            }
            if (ComboMenu["UseE"].Cast<CheckBox>().CurrentValue)
            {
                if (target.Distance(ObjectManager.Player) <= Q.Range && E.IsReady())
                {

                        E.Cast(Game.CursorPos);
                    

                }
            }
            if (ComboMenu["UseR"].Cast<CheckBox>().CurrentValue)
            {
                if (target.Distance(ObjectManager.Player) <= 5000 && R.IsReady() && Player.GetSpellDamage(target, SpellSlot.R) >= target.Health) ;
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
                if (target.Distance(ObjectManager.Player) <= Q.Range && Q.IsReady())
                {

                    if (Q.GetPrediction(target).HitChance >= HitChance.High)
                    {
                        Q.Cast(Q.GetPrediction(target).CastPosition);
                    }


                }
            }
            if (HarassMenu["UseW"].Cast<CheckBox>().CurrentValue)
            {
                if (target.Distance(ObjectManager.Player) <= W.Range && W.IsReady())
                {

                    W.Cast(W.GetPrediction(target).CastPosition);



                }
            }



        }

        private static void LaneClear()
        {
            if(FarmingMenu["Qclearmana"].Cast<Slider>().CurrentValue <= Player.ManaPercent)
            {
                var minion1 = EntityManager.MinionsAndMonsters.EnemyMinions.FirstOrDefault(m => m.IsValidTarget(Q.Range));

                    Q.Cast(minion1);

            }

        }
        private static void LastHit()
        {

            if (FarmingMenu["Qlastmana"].Cast<Slider>().CurrentValue <= Player.ManaPercent)
            {
                var minion = EntityManager.MinionsAndMonsters.EnemyMinions.FirstOrDefault(m => m.IsValidTarget(Q.Range) && Player.GetSpellDamage(m, SpellSlot.Q) >= m.Health);
                Q.Cast(minion);
            }

        }
    }
}