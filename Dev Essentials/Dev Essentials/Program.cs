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
           

            RootMenu = MainMenu.AddMenu("Developer Essentials", "Developer Essentials");
            RootMenu.Add("Enable", new CheckBox("Enabled"));

       
            Game.OnTick += Game_OnTick;
            Drawing.OnDraw += Drawing_OnDraw;

        }

        private static void Game_OnTick(EventArgs args)
        {


        }
        

        private static void Drawing_OnDraw(EventArgs args)
        {
            if(RootMenu["Enable"].Cast<CheckBox>().CurrentValue)
            {
                SpellDataInst spellQ = Player.Spellbook.GetSpell(SpellSlot.Q);
                SpellData dataQ = Player.Spellbook.GetSpell(SpellSlot.Q).SData;
                SpellDataInst spellW = Player.Spellbook.GetSpell(SpellSlot.W);
                SpellData dataW = Player.Spellbook.GetSpell(SpellSlot.W).SData;
                SpellDataInst spellE = Player.Spellbook.GetSpell(SpellSlot.E);
                SpellData dataE = Player.Spellbook.GetSpell(SpellSlot.E).SData;
                SpellDataInst spellR = Player.Spellbook.GetSpell(SpellSlot.R);
                SpellData dataR = Player.Spellbook.GetSpell(SpellSlot.R).SData;
                var PlayerCrit = Player.Crit.ToString();
                var PlayerCrit1 = PlayerCrit.Replace("0,", "") + "%";
                string temp = "";
                foreach (var buff in Player.Buffs)
                {
                    temp += (buff.DisplayName + "(" + buff.Count + ")" + ", ");
                }
                Drawing.DrawText(10, 0, System.Drawing.Color.Red, "Dev Essentials by swipedbrain");
                Drawing.DrawText(10, 10, System.Drawing.Color.White, "Coordinates:");
                Drawing.DrawText(10, 25, System.Drawing.Color.White, Player.Position.ToString());
                Drawing.DrawText(10, 55, System.Drawing.Color.White, "General Info:");
                Drawing.DrawText(10, 70, System.Drawing.Color.White, "Gold Earned: " + Player.GoldTotal.ToString());
                Drawing.DrawText(10, 85, System.Drawing.Color.White, "Attack Delay: " + Player.AttackDelay.ToString());
                Drawing.DrawText(10, 100, System.Drawing.Color.White, "Chance of Critical: " + PlayerCrit);
                Drawing.DrawText(10, 130, System.Drawing.Color.White, "Wards:");
                Drawing.DrawText(10, 145, System.Drawing.Color.White, "Wards Destroyed: " + Player.WardsKilled.ToString());
                Drawing.DrawText(10, 160, System.Drawing.Color.White, "Wards Placed: " + Player.WardsPlaced.ToString());
                Drawing.DrawText(10, 225, System.Drawing.Color.White, "Player Direction:");
                Drawing.DrawText(10, 240, System.Drawing.Color.White, Player.Direction.ToString());
                Drawing.DrawText(10, 265, System.Drawing.Color.White, "Base AD: " + Player.BaseAttackDamage.ToString());
                Drawing.DrawText(10, 280, System.Drawing.Color.White, "Base AP: " + Player.BaseAbilityDamage.ToString());
                Drawing.DrawText(10, 325, System.Drawing.Color.White, "Cursor Position: " + Game.CursorPos.ToString());
                Drawing.DrawText(10, 355, System.Drawing.Color.White, "Buffs: ");
                Drawing.DrawText(10, 370, System.Drawing.Color.White, temp.ToString());

                Drawing.DrawText(400, 0, System.Drawing.Color.White, "Skill Info:");
                Drawing.DrawText(400, 25, System.Drawing.Color.White, "Q: ");
                Drawing.DrawText(400, 40, System.Drawing.Color.White, "--------");
                Drawing.DrawText(400, 50, System.Drawing.Color.White, "Name: " + spellQ.Name.ToString());
                Drawing.DrawText(400, 65, System.Drawing.Color.White, "Level: " + spellQ.Level.ToString());
                Drawing.DrawText(400, 80, System.Drawing.Color.White, "Range: " + spellQ.SData.CastRange.ToString());
                Drawing.DrawText(400, 100, System.Drawing.Color.White, "W: ");
                Drawing.DrawText(400, 115, System.Drawing.Color.White, "--------");
                Drawing.DrawText(400, 130, System.Drawing.Color.White, "Name: " + spellW.Name.ToString());
                Drawing.DrawText(400, 145, System.Drawing.Color.White, "Level: " + spellW.Level.ToString());
                Drawing.DrawText(400, 160, System.Drawing.Color.White, "Range: " + spellW.SData.CastRange.ToString());
                Drawing.DrawText(400, 180, System.Drawing.Color.White, "E: ");
                Drawing.DrawText(400, 195, System.Drawing.Color.White, "--------");
                Drawing.DrawText(400, 210, System.Drawing.Color.White, "Name: " + spellE.Name.ToString());
                Drawing.DrawText(400, 225, System.Drawing.Color.White, "Level: " + spellE.Level.ToString());
                Drawing.DrawText(400, 240, System.Drawing.Color.White, "Range: " + spellE.SData.CastRange.ToString());
                Drawing.DrawText(400, 280, System.Drawing.Color.White, "R: ");
                Drawing.DrawText(400, 295, System.Drawing.Color.White, "--------");
                Drawing.DrawText(400, 310, System.Drawing.Color.White, "Name: " + spellR.Name.ToString());
                Drawing.DrawText(400, 325, System.Drawing.Color.White, "Level: " + spellR.Level.ToString());
                Drawing.DrawText(400, 340, System.Drawing.Color.White, "Range: " + spellR.SData.CastRange.ToString());
                Drawing.DrawText(0, 10, System.Drawing.Color.Red, "|");
                Drawing.DrawText(0, 20, System.Drawing.Color.Red, "|");
                Drawing.DrawText(0, 30, System.Drawing.Color.Red, "|");
                Drawing.DrawText(0, 40, System.Drawing.Color.Red, "|");
                Drawing.DrawText(0, 50, System.Drawing.Color.Red, "|");
                Drawing.DrawText(0, 60, System.Drawing.Color.Red, "|");
                Drawing.DrawText(0, 70, System.Drawing.Color.Red, "|");
                Drawing.DrawText(0, 80, System.Drawing.Color.Red, "|");
                Drawing.DrawText(0, 90, System.Drawing.Color.Red, "|");
                Drawing.DrawText(0, 100, System.Drawing.Color.Red, "|");
                Drawing.DrawText(0, 110, System.Drawing.Color.Red, "|");
                Drawing.DrawText(0, 120, System.Drawing.Color.Red, "|");
                Drawing.DrawText(0, 130, System.Drawing.Color.Red, "|");
                Drawing.DrawText(0, 140, System.Drawing.Color.Red, "|");
                Drawing.DrawText(0, 150, System.Drawing.Color.Red, "|");
                Drawing.DrawText(0, 160, System.Drawing.Color.Red, "|");
                Drawing.DrawText(0, 170, System.Drawing.Color.Red, "|");
                Drawing.DrawText(0, 180, System.Drawing.Color.Red, "|");
                Drawing.DrawText(0, 190, System.Drawing.Color.Red, "|");
                Drawing.DrawText(0, 200, System.Drawing.Color.Red, "|");
                Drawing.DrawText(0, 210, System.Drawing.Color.Red, "|");
                Drawing.DrawText(0, 220, System.Drawing.Color.Red, "|");
                Drawing.DrawText(0, 230, System.Drawing.Color.Red, "|");
                Drawing.DrawText(0, 240, System.Drawing.Color.Red, "|");
                Drawing.DrawText(0, 250, System.Drawing.Color.Red, "|");
                Drawing.DrawText(0, 260, System.Drawing.Color.Red, "|");
                Drawing.DrawText(0, 270, System.Drawing.Color.Red, "|");
                Drawing.DrawText(0, 280, System.Drawing.Color.Red, "|");
                Drawing.DrawText(0, 290, System.Drawing.Color.Red, "|");
                Drawing.DrawText(0, 300, System.Drawing.Color.Red, "|");
                Drawing.DrawText(0, 310, System.Drawing.Color.Red, "|");
                Drawing.DrawText(0, 320, System.Drawing.Color.Red, "|");
                Drawing.DrawText(0, 330, System.Drawing.Color.Red, "|");
                Drawing.DrawText(0, 340, System.Drawing.Color.Red, "|");
                Drawing.DrawText(0, 350, System.Drawing.Color.Red, "|");
                Drawing.DrawText(0, 360, System.Drawing.Color.Red, "|");
                Drawing.DrawText(0, 370, System.Drawing.Color.Red, "|");

                //
                Drawing.DrawText(390, 0, System.Drawing.Color.Red, "|");
                Drawing.DrawText(390, 10, System.Drawing.Color.Red, "|");
                Drawing.DrawText(390, 20, System.Drawing.Color.Red, "|");
                Drawing.DrawText(390, 30, System.Drawing.Color.Red, "|");
                Drawing.DrawText(390, 40, System.Drawing.Color.Red, "|");
                Drawing.DrawText(390, 50, System.Drawing.Color.Red, "|");
                Drawing.DrawText(390, 60, System.Drawing.Color.Red, "|");
                Drawing.DrawText(390, 70, System.Drawing.Color.Red, "|");
                Drawing.DrawText(390, 80, System.Drawing.Color.Red, "|");
                Drawing.DrawText(390, 90, System.Drawing.Color.Red, "|");
                Drawing.DrawText(390, 100, System.Drawing.Color.Red, "|");
                Drawing.DrawText(390, 110, System.Drawing.Color.Red, "|");
                Drawing.DrawText(390, 120, System.Drawing.Color.Red, "|");
                Drawing.DrawText(390, 130, System.Drawing.Color.Red, "|");
                Drawing.DrawText(390, 140, System.Drawing.Color.Red, "|");
                Drawing.DrawText(390, 150, System.Drawing.Color.Red, "|");
                Drawing.DrawText(390, 160, System.Drawing.Color.Red, "|");
                Drawing.DrawText(390, 170, System.Drawing.Color.Red, "|");
                Drawing.DrawText(390, 180, System.Drawing.Color.Red, "|");
                Drawing.DrawText(390, 190, System.Drawing.Color.Red, "|");
                Drawing.DrawText(390, 200, System.Drawing.Color.Red, "|");
                Drawing.DrawText(390, 210, System.Drawing.Color.Red, "|");
                Drawing.DrawText(390, 220, System.Drawing.Color.Red, "|");
                Drawing.DrawText(390, 230, System.Drawing.Color.Red, "|");
                Drawing.DrawText(390, 240, System.Drawing.Color.Red, "|");
                Drawing.DrawText(390, 250, System.Drawing.Color.Red, "|");
                Drawing.DrawText(390, 260, System.Drawing.Color.Red, "|");
                Drawing.DrawText(390, 270, System.Drawing.Color.Red, "|");
                Drawing.DrawText(390, 280, System.Drawing.Color.Red, "|");
                Drawing.DrawText(390, 290, System.Drawing.Color.Red, "|");
                Drawing.DrawText(390, 300, System.Drawing.Color.Red, "|");
                Drawing.DrawText(390, 310, System.Drawing.Color.Red, "|");
                Drawing.DrawText(390, 320, System.Drawing.Color.Red, "|");
                Drawing.DrawText(390, 330, System.Drawing.Color.Red, "|");
                Drawing.DrawText(390, 335, System.Drawing.Color.Red, "|");
                Drawing.DrawText(390, 340, System.Drawing.Color.Red, "|");
            }

            }
        }


    }


