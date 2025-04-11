using System.Collections.Generic;
using System.ComponentModel;
using LabApi.Features.Wrappers;
using UnityEngine;

namespace EscapePlan
{
    public class Config
    {
        //EscapeDoor configs
        [Description("Set whether or not if you want a second Escape route on surface. Perhaps in Gate A")]
        public bool EscapeDoor { get; set; } = true;

        [Description("If GateAEscape is enabled, get the position and rotation. Default location replaces the door next to the gate where the CI car spawns")]
        public Vector3 EscapeDoorPosition { get; set;  } = new Vector3(-41f, 991.8f, -36);
        
        public Vector3 EscapeDoorRotation { get; set; } = new Vector3(0,90,0);
        
        //---------------Escapee Reward items
        //Shared
        [Description("List the items you'll like all CI and NTF escapees to receive while escaping. Ammunition can be added but unless its not an option, you should use set ammo for the specific class. Armour is currently not supported (Trying so will spawn the player with two armour items)")]
        public List<ItemType> rewardItems { get; set; } = new List<ItemType>
        {
            ItemType.Adrenaline
        };

        //CI Conscript
        [Description("Set how much 7.62 rounds CI Conscripts spawn with. Default is the maximum amount combat armour can carry")]
        public byte ciAmmo { get; set; } = 120;

        [Description("Exclusive items only for CI Conscripts. This can include sidearms and ammunition")]
        public List<ItemType> ciItems { get; set; } = new List<ItemType>
        {
            ItemType.GunRevolver,
            ItemType.Ammo44cal, //Each ItemType.Ammo44cal only gives 6 bullets. Do it twice for 12 total spare bullets
            ItemType.Ammo44cal
        };

        //NTF 
        [Description("Set how much 9mm rounds NTF escapees spawn with. Default is the maximum amount combat armour can carry")]
        public byte ntf_9mmAmmo { get; set; } = 170;

        [Description("Set how much 5.56 ammo NTF escapees spawn with. Default is the maximum amount combat armour can carry")]
        public byte ntf_556Ammo { get; set; } = 120;
        
        [Description("Exclusive items only for NTF Escapees. This can include sidearms and ammunition")]
        public List<ItemType> ntfItems { get; set; } = new List<ItemType>
        {
            ItemType.GunCOM18
        };
    }
}