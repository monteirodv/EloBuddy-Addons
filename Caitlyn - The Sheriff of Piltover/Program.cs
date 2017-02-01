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

namespace Cait
{
    class Program
    {
        private static AIHeroClient Player { get { return ObjectManager.Player; } }

        private static Menu RootMenu, ComboMenu, HarassMenu, FarmingMenu, ksMenu, DrawingsMenu;

        public static Spell.Skillshot Q;
        public static Spell.Skillshot W;
        public static Spell.Skillshot E;
        public static Spell.Targeted R;
        static void Main(string[] args)
        {

            Loading.OnLoadingComplete += Loading_OnLoadingComplete;

        }

        private static void Loading_OnLoadingComplete(EventArgs args)
        {
            Q = new Spell.Skillshot(SpellSlot.Q, 1200, SkillShotType.Linear, 625, 2200, 90);
            Q.AllowedCollisionCount = int.MaxValue;
            W = new Spell.Skillshot(SpellSlot.W, 800, SkillShotType.Circular, 500, int.MaxValue, 20);
            E = new Spell.Skillshot(SpellSlot.E, 800, SkillShotType.Linear, 150, 1600, 80);
            R = new Spell.Targeted(SpellSlot.R, 825);

            RootMenu = MainMenu.AddMenu("Caitlyn - The Sheriff of Piltover", "Caitlyn - The Sheriff of Piltover");

            ComboMenu = RootMenu.AddSubMenu("Combo", "Combo");

            ComboMenu.Add("UseQ", new CheckBox("Use Q"));
            ComboMenu.Add("UseW", new CheckBox("Use W", false));
            ComboMenu.Add("UseE", new CheckBox("Use E "));
            ComboMenu.Add("UseR", new CheckBox("Use R if killable"));

            HarassMenu = RootMenu.AddSubMenu("Harass", "Harass");

            HarassMenu.Add("UseQ", new CheckBox("Use Q"));
            HarassMenu.Add("UseQ", new CheckBox("Use Q when player is processing spell or cc'd"));
            HarassMenu.Add("QHarassmana", new Slider("Min mana to harass with Q %", 30, 0, 100));


            FarmingMenu = RootMenu.AddSubMenu("Farming", "farming");

            FarmingMenu.Add("Qclear", new CheckBox("Use Q to clear wave"));
            FarmingMenu.Add("Qclearmana", new Slider("Q mana to clear %", 30, 0, 100));

            FarmingMenu.Add("Qclearjg", new CheckBox("Use Q to clear jungle"));


            DrawingsMenu = RootMenu.AddSubMenu("Drawings", "Drawings");

            DrawingsMenu.Add("DrawQ", new CheckBox("Draw Q range"));
            DrawingsMenu.Add("DrawW", new CheckBox("Draw W range"));
            DrawingsMenu.Add("DrawE", new CheckBox("Draw E range"));
            DrawingsMenu.Add("DrawWpred", new CheckBox("Draw Q prediction"));

            Game.OnTick += Game_OnTick;
            Drawing.OnDraw += Drawing_OnDraw;
            Obj_AI_Base.OnProcessSpellCast += OnProcessSpellCast;
            Gapcloser.OnGapcloser += Gapcloser_OnGapCloser;

        }

        private static void Game_OnTick(EventArgs args)
        {
            var target = TargetSelector.GetTarget(W.Range, DamageType.Physical);
            if ( target.IsRooted || target.IsStunned || target.IsTaunted )
            {
                W.Cast(target.ServerPosition);
            }
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
         private static void OnProcessSpellCast(Obj_AI_Base sender, GameObjectProcessSpellCastEventArgs args)
        {
             if (sender.IsInRange(Player, W.Range) && sender.IsEnemy)
             {
                 W.Cast(sender.ServerPosition);
                 Q.Cast(sender.ServerPosition);
             }
        }
         private static void Gapcloser_OnGapCloser(AIHeroClient sender, Gapcloser.GapcloserEventArgs gapcloser)
         {
             if (sender.IsAlly)
             {
                 return;
             }
             E.Cast(sender.ServerPosition);

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
                        Q.Cast(Q.GetPrediction(target).CastPosition);
                    }


                }
            }
            if (ComboMenu["UseW"].Cast<CheckBox>().CurrentValue)
            {
                if (target.IsInRange(Player, W.Range) && W.IsReady())
                {

                    W.Cast(target.ServerPosition);



                }
            }
            if (ComboMenu["UseE"].Cast<CheckBox>().CurrentValue)
            {
                if (target.IsInRange(Player, E.Range) && E.IsReady())
                {

                    E.Cast(E.GetPrediction(target).CastPosition);


                }
            }
            if (ComboMenu["UseR"].Cast<CheckBox>().CurrentValue)
            {
                if (target.IsInRange(Player, R.Range) && !Q.IsReady() && R.GetSpellDamage(target) >= target.Health)
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
            if (HarassMenu["UseQ"].Cast<CheckBox>().CurrentValue && HarassMenu["QHarassmana"].Cast<Slider>().CurrentValue <= Player.ManaPercent)
            {
                if (target.IsInRange(Player, Q.Range) && Q.IsReady())
                {

                    if (Q.GetPrediction(target).HitChance >= HitChance.High)
                    {
                        Q.Cast(Q.GetPrediction(target).CastPosition);
                    }


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


        }
        private static void JungleClear()
        {

            if (FarmingMenu["Qclearjg"].Cast<CheckBox>().CurrentValue)
            {
                var monster = EntityManager.MinionsAndMonsters.GetJungleMonsters(Player.Position, Q.Range);
                Q.Cast(monster.First());
            }

        }
    }
}