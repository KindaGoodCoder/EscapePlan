using System.Collections.Generic;
using System.ComponentModel;
using MapGeneration;
using PlayerRoles;
using UnityEngine;

namespace EscapePlan
{
    public class Config
    {
        //Detained militant escapes
        [Description("Allow which militant classes are able to be detained and convert to the other team. Leave list empty to disable detained militant escapes")]
        public static List<RoleTypeId> DetainedMilitantsEscapes => new()
        {
            RoleTypeId.NtfCaptain,
            RoleTypeId.NtfPrivate,
            RoleTypeId.NtfSergeant,
            RoleTypeId.NtfSpecialist,
            RoleTypeId.ChaosConscript,
            RoleTypeId.ChaosMarauder,
            RoleTypeId.ChaosRepressor,
            RoleTypeId.ChaosRifleman
        };
        
        [Description("Set which class will spawn if a Chaos Insurgent is detained and escapes")]
        public static RoleTypeId DetainedChaosEscapeRole => RoleTypeId.NtfPrivate;
        
        [Description("Set which class will spawn if a Foundation militant is detained and escapes")]
        public static RoleTypeId DetainedFoundationEscapeRole => RoleTypeId.ChaosConscript;
        
        //EscapeDoor configs
        [Description("Set the room the EscapeDoor will spawn. Set to RoomName.Unnamed or 0 to disable secondary escape route")]
        public static RoomName EscapeDoorRoom => RoomName.Outside;

        [Description("If EscapeDoor is enabled, set the position offset in the configured Room")]
        public static Vector3 EscapeDoorPositionOffset => new (-41.3f, -9, -36.1f);

        [Description("If EscapeDoor is enabled, set the Eular Rotation")]
        public static Vector3 EscapeDoorRotation => new (0,90,0);
        
        //---------------Escapee Reward items
        //Shared
        [Description("List the items you'll like all CI and NTF escapees to receive while escaping.")]
        public static List<ItemType> rewardItems => new()
        {
            ItemType.Adrenaline
        };

        //CI Conscript
        [Description("Set how much 7.62 rounds CI Conscripts spawn with. Default is the maximum amount combat armour can carry")]
        public static byte ciAmmo => 120;

        [Description("Exclusive items only for CI Conscripts. This can include sidearms and ammunition")]
        public static List<ItemType> ciItems => new()
        {
            ItemType.GunRevolver,
            ItemType.Ammo44cal, //Each ItemType.Ammo44cal only gives 6 bullets. Do it twice for 12 total spare bullets
            ItemType.Ammo44cal
        };

        //NTF 
        [Description("Set how much 9mm rounds NTF escapees spawn with. Default is the maximum amount combat armour can carry")]
        public static byte ntf_9mmAmmo => 170;

        [Description("Set how much 5.56 ammo NTF escapees spawn with. Default is the maximum amount combat armour can carry")]
        public static byte ntf_556Ammo => 120;
        
        [Description("Exclusive items only for NTF Escapees. This can include sidearms and ammunition")]
        public static List<ItemType> ntfItems => new()
        {
            ItemType.GunCOM18
        };
    }
}