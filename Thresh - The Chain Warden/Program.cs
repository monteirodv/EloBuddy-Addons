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


namespace Thresh___The_Chain_Warden
{

    class Program
    {
        private static AIHeroClient Player
        {
            get
            {
                return ObjectManager.Player;
            }
        }

        private static Menu RootMenu, ComboMenu, HarassMenu, FlayMenu, FlashHook, DrawingsMenu, LanternMenu;


        private static float _lastCheck = Environment.TickCount;
        public static Spell.Targeted Flash
        {
            get;
            private set;
        }
        public static Spell.Skillshot Q;
        public static Spell.Skillshot Q2;
        public static Spell.Skillshot W;
        public static Spell.Skillshot E;
        public static Spell.Active R;
        static void Main(string[] args)
        {

            Loading.OnLoadingComplete += Loading_OnLoadingComplete;

        }

        private static void Loading_OnLoadingComplete(EventArgs args)
        {
            Q = new Spell.Skillshot(SpellSlot.Q, 1040, SkillShotType.Linear, 500, 1900, 60)
            {
                AllowedCollisionCount = 0
            };
            Q2 = new Spell.Skillshot(SpellSlot.Q, 1300, SkillShotType.Linear, 500, 1900, 60);
            W = new Spell.Skillshot(SpellSlot.W, 950, SkillShotType.Circular, 250, 1800, 300)
            {
                AllowedCollisionCount = int.MaxValue
            };
            E = new Spell.Skillshot(SpellSlot.E, 480, SkillShotType.Linear, 0, 2000, 110)
            {
                AllowedCollisionCount = int.MaxValue
            };
            R = new Spell.Active(SpellSlot.R, 450);
            var slot = Player.GetSpellSlotFromName("summonerflash");

            switch (slot)
            {
                case SpellSlot.Summoner1:
                case SpellSlot.Summoner2:
                    Flash = new Spell.Targeted(slot, 425);
                    break;
            }
            RootMenu = MainMenu.AddMenu("Thresh - The Chain Warden", "Thresh - The Chain Warden");

            ComboMenu = RootMenu.AddSubMenu("Combo", "Combo");

            ComboMenu.Add("UseQ", new CheckBox("Use Q"));
            ComboMenu.Add("UseE", new CheckBox("Use E"));
            ComboMenu.Add("UseR", new CheckBox("Use R"));
            ComboMenu.Add("EPush", new CheckBox("E Push/Pull(on/off)", true));


            HarassMenu = RootMenu.AddSubMenu("Harass", "Harass");

            HarassMenu.Add("UseQ", new CheckBox("Use Q"));
            HarassMenu.Add("UseE", new CheckBox("Use E"));



            FlayMenu = RootMenu.AddSubMenu("Flay", "Flay");

            FlayMenu.Add("Push", new KeyBind("Push", false, KeyBind.BindTypes.HoldActive, "I".ToCharArray()[0]));
            FlayMenu.Add("Pull", new KeyBind("Pull", false, KeyBind.BindTypes.HoldActive, "U".ToCharArray()[0]));

            FlashHook = RootMenu.AddSubMenu("Flash Hook", "Flash Hook");
            FlashHook.Add("Fhook", new KeyBind("Flash Q Combo", false, KeyBind.BindTypes.HoldActive, "G".ToCharArray()[0]));

            LanternMenu = RootMenu.AddSubMenu("Lantern", "Lantern");

            LanternMenu.Add("Throwlantern", new KeyBind("Throw Lantern to ally", false, KeyBind.BindTypes.HoldActive, "T".ToCharArray()[0]));
            LanternMenu.Add("LanternNear", new CheckBox("Prioritize Nearest Ally"));
            LanternMenu.Add("LanternLow", new CheckBox("Prioritize Lowest Ally", true));

            DrawingsMenu = RootMenu.AddSubMenu("Drawings", "Drawings");

            DrawingsMenu.Add("DrawQ", new CheckBox("Draw Q range"));
            DrawingsMenu.Add("DrawW", new CheckBox("Draw W range"));
            DrawingsMenu.Add("DrawE", new CheckBox("Draw E range"));
            DrawingsMenu.Add("DrawE", new CheckBox("Draw R range"));
            DrawingsMenu.Add("DrawQpred", new CheckBox("Draw Q prediction"));

            Game.OnTick += Game_OnTick;
            Drawing.OnDraw += Drawing_OnDraw;
            Gapcloser.OnGapcloser += Gapcloser_OnGapCloser;
            Interrupter.OnInterruptableSpell += Interrupter_OnInterruptableSpell;

        }

        private static void Game_OnTick(EventArgs args)
        {
            if (LanternMenu.Get<KeyBind>("ThrowLantern").CurrentValue)
            {
                ThrowLantern();
            }

            if (FlayMenu.Get<KeyBind>("Push").CurrentValue)
            {
                Push();
            }
            if (FlayMenu.Get<KeyBind>("Pull").CurrentValue)
            {
                Pull();
            }
            if (FlashHook.Get<KeyBind>("Fhook").CurrentValue)
            {
                Fhook();
            }
            switch (Orbwalker.ActiveModesFlags)
            {
                case Orbwalker.ActiveModes.Combo:
                    Combo();
                    break;
                case Orbwalker.ActiveModes.Harass:
                    Harass();
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
        private static void Interrupter_OnInterruptableSpell(Obj_AI_Base sender, Interrupter.InterruptableSpellEventArgs args)
        {
            if (sender.IsAlly)
            {
                return;
            }

            E.Cast(sender.ServerPosition);
        }
        private static void Gapcloser_OnGapCloser(AIHeroClient sender, Gapcloser.GapcloserEventArgs gapcloser)
        {
            if (sender.IsAlly)
            {
                return;
            }
            E.Cast(sender.ServerPosition);

        }
        private static void ThrowLantern()
        {
            if (W.IsReady())
            {



                foreach (var ally in EntityManager.Heroes.Allies.Where(x => x.IsValidTarget(W.Range) && !x.IsMe && !x.IsDead))
                {
                    W.Cast(ally);
                }
            }
        }
        private static void Pull()
        {
            var target = TargetSelector.GetTarget(E.Range, DamageType.Magical);
            Orbwalker.MoveTo(Game.CursorPos);

            if (E.IsReady() && target.IsInRange(Player, E.Range))
            {
                E.Cast(target.Position.Extend(Player.Position, Vector3.Distance(target.Position, Player.Position) + 400).To3D());
            }
        }
        private static void Push()
        {
            Orbwalker.MoveTo(Game.CursorPos);

            var target = TargetSelector.GetTarget(E.Range, DamageType.Magical);
            if (E.IsReady() && target.IsInRange(Player, E.Range))
            {
                E.Cast(target.Position);
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

            if (ComboMenu["UseE"].Cast<CheckBox>().CurrentValue)
            {
                if (ComboMenu["EPush"].Cast<CheckBox>().CurrentValue)
                {
                    if (E.IsReady() && target.IsInRange(Player, E.Range))
                        E.Cast(target.Position.Extend(Player.Position, Vector3.Distance(target.Position, Player.Position) + 400).To3D());

                }
                else
                {
                    if (E.IsReady() && target.IsInRange(Player, E.Range))
                    {

                        E.Cast(target.Position);
                    }
                }
            }
            if (ComboMenu["UseR"].Cast<CheckBox>().CurrentValue)
            {
                if (target.IsInRange(Player, R.Range) && R.IsReady())
                {

                    R.Cast();



                }
            }


        }
        private static void Fhook()
        {
            Orbwalker.MoveTo(Game.CursorPos);
            var target = TargetSelector.GetTarget(Q2.Range, DamageType.Magical);

            if (Q2.GetPrediction(target).HitChance <= HitChance.High)
            {
                if (Flash.IsReady() && Q.IsReady())
                {
                    var flashpos = Player.ServerPosition.Extend(Game.CursorPos, Flash.Range);
                    Flash.Cast(flashpos.To3D());
                    Q.Cast(Q.GetPrediction(target).CastPosition);
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
                        Q.Cast(target);
                    }


                }
            }
            if (HarassMenu["UseE"].Cast<CheckBox>().CurrentValue)
            {
                if (ComboMenu["EPush"].Cast<CheckBox>().CurrentValue)
                {
                    if (E.IsReady() && target.IsInRange(Player, E.Range))
                        E.Cast(target.Position.Extend(Player.Position, Vector3.Distance(target.Position, Player.Position) + 400).To3D());

                }
                else
                    if (E.IsReady() && target.IsInRange(Player, E.Range))
                    {

                        E.Cast(target.Position);
                    }
            }
        }
    }
}