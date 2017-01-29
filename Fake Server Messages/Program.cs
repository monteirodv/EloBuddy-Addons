using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.IO;
using System.Reflection;
using EloBuddy;
using EloBuddy.SDK;
using EloBuddy.SDK.Events;
using EloBuddy.SDK.Menu;
using EloBuddy.SDK.Menu.Values;
using SharpDX;
using Version = System.Version;


namespace FakeServerMessages
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
    
            RootMenu = MainMenu.AddMenu("Server Messages", "Server Messages");

            RootMenu.AddLabel("Use .msg to send the server message, it will display as [Server Message]");
            RootMenu.Add("all", new CheckBox("Display in All chat"));
            Game.OnTick += Game_OnTick;
            Chat.OnInput += OnInput;

        }

        private static void OnInput(ChatInputEventArgs args)
        {
            if (RootMenu["all"].Cast<CheckBox>().CurrentValue)
            {
                if (args.Input.StartsWith(".msg"))
                {
                    var message = args.Input.ToString().Substring(4);
                    args.Process = false;
                    Chat.Say("/all" + " " +  new string('_', 57 + Player.Name.Length) + "[Server Message]" + message);
                }
            }
            else
            {
                if (args.Input.StartsWith(".msg"))
                {
                    var message = args.Input.ToString().Substring(4);
                    args.Process = false;
                    Chat.Say(new string('_', 60 + Player.Name.Length) + "[Server Message]" + message);
                }
            }
        }

  


        private static void Game_OnStart(EventArgs args)
        {
        }
        private static void Game_OnTick(EventArgs args)
        {

        
        }



        

    }
}