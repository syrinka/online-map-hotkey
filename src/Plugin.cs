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
        protected const string BaseURL = "https://rain-world-map.github.io/map.html";

        public void OnEnable()
        {
            Hotkey.Trigger = PlayerKeybind.Register("online-map-hotkey:hotkey", "Online Map Hotkey", "Online Map", KeyCode.M, KeyCode.None);
            On.Player.checkInput += Player_CheckInput;
        }

        public static void Player_CheckInput(On.Player.orig_checkInput orig, Player self)
        {
            if (self.IsKeyBound(Hotkey.Trigger) && Hotkey.Trigger.CheckRawPressed(self.playerState.playerNumber) && self.room.game.IsStorySession)
            {
                string url = BaseURL
                    + "?slugcat=" + self.slugcatStats.name.ToString().ToLower()
                    + "&region=" + self.room.world.region.name
                    + "&room=" + self.room.roomSettings.name;
                SteamFriends.ActivateGameOverlayToWebPage(url);
            }
            orig(self);
        }
    }
}
