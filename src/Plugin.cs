using System;
using System.Collections.Generic;

using UnityEngine;
using BepInEx;
using Steamworks;
using ImprovedInput;


namespace OnlineMapHotkey
{
    public static class Hotkey
    {
        public static PlayerKeybind Trigger;
    }

    [BepInPlugin("cedaro.online-map-hotkey", "Online Map Hotkey", "1.0")]
    public class Plugin : BaseUnityPlugin
    {
        protected static string BaseURL = "https://rain-world-map.github.io/map.html";

        public static Dictionary<string, string> ModdedSlugcats = new Dictionary<string, string>
        {
            {"the_similar", "yellow"},
            {"the_sporecat", "red"},
            {"the_dronemaster", "artificer"},
            {"the_outsider", "saint"},
            {"mirosslug", "spear"},
        };

        public static List<string> VanillaSlugcats = new List<string> { "white", "yellow", "red", "gourmand", "artificer", "rivulet", "spear", "saint", "sofanthiel" };

        public void OnEnable()
        {
            Hotkey.Trigger = PlayerKeybind.Register("online-map-hotkey:hotkey", "Online Map Hotkey", "Online Map", KeyCode.M, KeyCode.None);
            On.Player.checkInput += Player_CheckInput;
        }

        public static string GetWorldState(Player p)
        {
            string name = p.slugcatStats.name.value.ToLowerInvariant();
            if (ModdedSlugcats.TryGetValue(name, out string name2))
            {
                return name2;
            }
            if (!VanillaSlugcats.Contains(name))
            {
                name = "white";
            }
            return name;
        }

        public static void Player_CheckInput(On.Player.orig_checkInput orig, Player self)
        {
            if (self.IsKeyBound(Hotkey.Trigger) && Hotkey.Trigger.CheckRawPressed(self.playerState.playerNumber) && self.room.game.IsStorySession)
            {
                string url = BaseURL
                    + "?slugcat=" + GetWorldState(self)
                    + "&region=" + self.room.world.region.name
                    + "&room=" + self.room.roomSettings.name;
                SteamFriends.ActivateGameOverlayToWebPage(url);
            }
            orig(self);
        }
    }
}
