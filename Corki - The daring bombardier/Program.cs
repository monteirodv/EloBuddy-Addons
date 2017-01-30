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
        public static Spell.Active E;
        public static Spell.Skillshot R;
        static void Main(string[] args)
        {

            Loading.OnLoadingComplete += Loading_OnLoadingComplete;

        }

        private static void Loading_OnLoadingComplete(EventArgs args)
        {
            Q = new Spell.Skillshot(SpellSlot.Q, 825, SkillShotType.Circular, 300, 1000, 250);
            Q.AllowedCollisionCount = int.MaxValue;
            W = new Spell.Skillshot(SpellSlot.W, 800, SkillShotType.Linear, 1800, 1500, 200);
            W.AllowedCollisionCount = int.MaxValue;
            E = new Spell.Active(SpellSlot.E, 600);
            R = new Spell.Skillshot(SpellSlot.R, 1300, SkillShotType.Linear, 200, 1950, 40);
            R.AllowedCollisionCount = 0;

            RootMenu = MainMenu.AddMenu("Corki - The Daring bombardier", "Corki - The Daring bombardier");

            ComboMenu = RootMenu.AddSubMenu("Combo", "Combo");

            ComboMenu.Add("UseQ", new CheckBox("Use Q"));
            ComboMenu.Add("UseW", new CheckBox("Use W offensively"));
            ComboMenu.Add("UseE", new CheckBox("Use E"));
            ComboMenu.Add("UseR", new CheckBox("Use R"));

            HarassMenu = RootMenu.AddSubMenu("Harass", "Harass");

            HarassMenu.Add("UseQ", new CheckBox("Use Q"));
            HarassMenu.Add("UseE", new CheckBox("Use E"));
            HarassMenu.Add("UseR", new CheckBox("Use R"));




            FarmingMenu = RootMenu.AddSubMenu("Farming", "farming");

            FarmingMenu.Add("Qclear", new CheckBox("Use Q to clear wave"));
            FarmingMenu.Add("Eclear", new CheckBox("Use E to clear wave"));
            FarmingMenu.Add("Qclearmana", new Slider("Q mana to clear %", 30, 0, 100));
            FarmingMenu.Add("Eclearmana", new Slider("E mana to last hit %", 30, 0, 100));


            DrawingsMenu = RootMenu.AddSubMenu("Drawings", "Drawings");

            DrawingsMenu.Add("DrawQ", new CheckBox("Draw Q range"));
            DrawingsMenu.Add("DrawW", new CheckBox("Draw W range"));
            DrawingsMenu.Add("DrawE", new CheckBox("Draw E range"));
            DrawingsMenu.Add("DrawE", new CheckBox("Draw R range"));
            DrawingsMenu.Add("DrawQpred", new CheckBox("Draw Q prediction"));

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
                Drawing.DrawCircle(Q.GetPrediction(target).CastPosition, 250, System.Drawing.Color.Red);

            }
            if (DrawingsMenu["DrawR"].Cast<CheckBox>().CurrentValue)
            {
                Circle.Draw(Color.Aqua, R.Range, Player);
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
                if (target.Distance(ObjectManager.Player) <= E.Range && E.IsReady())
                {

                    E.Cast();


                }
            }
            if (ComboMenu["UseR"].Cast<CheckBox>().CurrentValue)
            {
                if (target.Distance(ObjectManager.Player) <= R.Range && R.IsReady()) ;
                {

                    R.Cast(R.GetPrediction(target).CastPosition);



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
            if (HarassMenu["UseE"].Cast<CheckBox>().CurrentValue)
            {
                if (target.Distance(ObjectManager.Player) <= E.Range && E.IsReady())
                {

                    E.Cast();

                }
            }
            if (HarassMenu["UseR"].Cast<CheckBox>().CurrentValue)
            {
                if (target.Distance(ObjectManager.Player) <= R.Range && R.IsReady())
                {

                    if (R.GetPrediction(target).HitChance >= HitChance.High)
                    {
                        R.Cast(R.GetPrediction(target).CastPosition);
                    }


                }
            }



        }

        private static void LaneClear()
        {
            if (FarmingMenu["Qclearmana"].Cast<Slider>().CurrentValue <= Player.ManaPercent && FarmingMenu["Qclear"].Cast<CheckBox>().CurrentValue)
            {
                var minions = EntityManager.MinionsAndMonsters.EnemyMinions.Where(t => t.IsEnemy && !t.IsDead && t.IsValid && !t.IsInvulnerable && t.IsInRange(Player.Position, Q.Range));
                foreach (var m in minions)
                {
                    if (Q.GetPrediction(m).CollisionObjects.Where(t => t.IsEnemy && !t.IsDead && t.IsValid && !t.IsInvulnerable).Count() >= 3)
                    {
                        Q.Cast(m);
                        break;
                    }
                }
                }
            if (FarmingMenu["Eclearmana"].Cast<Slider>().CurrentValue <= Player.ManaPercent && FarmingMenu["Eclear"].Cast<CheckBox>().CurrentValue)
            {
                var minions = EntityManager.MinionsAndMonsters.EnemyMinions.Where(t => t.IsEnemy && !t.IsDead && t.IsValid && !t.IsInvulnerable && t.IsInRange(Player.Position, Q.Range));
                foreach (var m in minions)
                {
                    if (m.Distance(Player) <= E.Range)
                    {
                        E.Cast();
                    }
                }
            }

            }

        

        }
    }
