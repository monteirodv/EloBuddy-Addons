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

        private static Menu RootMenu, ComboMenu, HarassMenu, FarmingMenu, RMenu, DrawingsMenu;

        public static Spell.Skillshot Q;
        public static Spell.Skillshot W;
        public static Spell.Targeted E;
        public static Spell.Targeted R;
        static void Main(string[] args)
        {

            Loading.OnLoadingComplete += Loading_OnLoadingComplete;

        }

        private static void Loading_OnLoadingComplete(EventArgs args)
        {
            Q = new Spell.Skillshot(SpellSlot.Q, 1050, SkillShotType.Linear, 250, 1600, 60);
            Q.AllowedCollisionCount = 0;
            W = new Spell.Skillshot(SpellSlot.W, 900, SkillShotType.Circular, 850, int.MaxValue, 250);
            W.AllowedCollisionCount = int.MaxValue;
            E = new Spell.Targeted(SpellSlot.E, 630);
            R = new Spell.Targeted(SpellSlot.R, 750);

            RootMenu = MainMenu.AddMenu("Brand - The Burning Vengeance", "Brand - The Burning Vengeance");

            ComboMenu = RootMenu.AddSubMenu("Combo", "Combo");

            ComboMenu.Add("UseQ", new CheckBox("Use Q"));
            ComboMenu.Add("UseW", new CheckBox("Use W"));
            ComboMenu.Add("UseE", new CheckBox("Use E"));
            ComboMenu.Add("UseR", new CheckBox("Use R"));

            HarassMenu = RootMenu.AddSubMenu("Harass", "Harass");

            HarassMenu.Add("UseQ", new CheckBox("Use Q"));
            HarassMenu.Add("UseW", new CheckBox("Use W"));


            RMenu = RootMenu.AddSubMenu("R Options", "Roptions");
            RMenu.Add("REnemies", new Slider("Use R when X enemies can be hit", 2, 0, 5));

            FarmingMenu = RootMenu.AddSubMenu("Farming", "Farming");

            FarmingMenu.Add("Qclear", new CheckBox("Use Q to clear wave"));
            FarmingMenu.Add("Wclear", new CheckBox("Use W to clear wave"));
            FarmingMenu.Add("Qclearmana", new Slider("Q mana to clear %", 30, 0, 100));
            FarmingMenu.Add("Wclearmana", new Slider("W mana to clear %", 30, 0, 100));


            DrawingsMenu = RootMenu.AddSubMenu("Drawings", "Drawings");

            DrawingsMenu.Add("DrawQ", new CheckBox("Draw Q range"));
            DrawingsMenu.Add("DrawW", new CheckBox("Draw W range"));
            DrawingsMenu.Add("DrawE", new CheckBox("Draw E range"));
            DrawingsMenu.Add("DrawE", new CheckBox("Draw R range"));
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
                Drawing.DrawCircle(W.GetPrediction(target).CastPosition, 250, System.Drawing.Color.Red);

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
                    if (W.GetPrediction(target).HitChance >= HitChance.High)
                    {
                        W.Cast(W.GetPrediction(target).CastPosition);

                    }

                }
            }
            if (ComboMenu["UseE"].Cast<CheckBox>().CurrentValue)
            {
                if (target.Distance(ObjectManager.Player) <= E.Range && E.IsReady())
                {

                    E.Cast(target);


                }
            }
            if (ComboMenu["UseR"].Cast<CheckBox>().CurrentValue)
            {
                if (target.Distance(ObjectManager.Player) <= R.Range && R.IsReady()) ;
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
                if (target.Distance(ObjectManager.Player) <= E.Range && E.IsReady())
                {

                    if (W.GetPrediction(target).HitChance >= HitChance.High)
                    {
                        W.Cast(W.GetPrediction(target).CastPosition);
                    }

                }
            }

        }

        private static void LaneClear()
        {

            if (FarmingMenu["Qclearmana"].Cast<Slider>().CurrentValue <= Player.ManaPercent && FarmingMenu["Qclear"].Cast<CheckBox>().CurrentValue)
            {
                var minion1 = EntityManager.MinionsAndMonsters.EnemyMinions.FirstOrDefault(m => m.IsValidTarget(Q.Range));
                Drawing.DrawLine(Player.Position.WorldToScreen(), minion1.Position.WorldToScreen(), 4, System.Drawing.Color.Aqua);
                Q.Cast(minion1);
            }                      
            
            if (FarmingMenu["Wclearmana"].Cast<Slider>().CurrentValue <= Player.ManaPercent && FarmingMenu["Wclear"].Cast<CheckBox>().CurrentValue)
            {
                var minions = EntityManager.MinionsAndMonsters.EnemyMinions.Where(t => t.IsEnemy && !t.IsDead && t.IsValid && !t.IsInvulnerable && t.IsInRange(Player.Position, Q.Range));
                foreach (var m in minions)
                {
                    if (W.GetPrediction(m).CollisionObjects.Where(t => t.IsEnemy && !t.IsDead && t.IsValid && !t.IsInvulnerable).Count() >= 3)
                    {
                        W.Cast(m);
                        break;
                    }
                }   
            
        }
        }



    }
}