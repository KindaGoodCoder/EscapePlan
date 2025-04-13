using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using MapGeneration;
using PlayerRoles;
using UnityEngine;

namespace EscapePlan
{
    public class Config
    {
        //Detained militant escapes
        [Description("Allow which Foundation militants classes are able to be detained and convert to the other team. Leave list empty to disable detained NTF escapes")]
        public List<RoleTypeId> DetainedNtfEscapes { get; set; } = new()
        {
            // RoleTypeId.NtfCaptain,
            // RoleTypeId.NtfPrivate,
            // RoleTypeId.NtfSergeant,
            // RoleTypeId.NtfSpecialist
        };
        
        [Description("Allow which Insurgent militants classes are able to be detained and convert to the other team. Leave list empty to disable detained CI escapes")]
        public List<RoleTypeId> DetainedCiEscapes { get; set; } = new()
        {
            // RoleTypeId.ChaosConscript,
            // RoleTypeId.ChaosMarauder,
            // RoleTypeId.ChaosRepressor,
            // RoleTypeId.ChaosRifleman
        };
        
        //EscapeDoor configs
        [Description("Set the room the EscapeDoor will spawn. Set to RoomName.None or 0 to disable secondary escape route")]
        public RoomName EscapeDoorRoom { get; set; } = RoomName.Outside;

        [Description("If EscapeDoor is enabled, set the position offset in the configured Room")]
        public Vector3 EscapeDoorPositionOffset { get; set; } = new (-41.3f, -9, -36.1f);

        [Description("If EscapeDoor is enabled, set the Eular Rotation")]
        public Vector3 EscapeDoorRotation { get; set; } = new (0,90,0);
        
        //---------------Escapee Reward items
        //Shared
        [Description("List the items you'll like all CI and NTF escapees to receive while escaping.")]
        public List<ItemType> rewardItems { get; set; } = new()
        {
            ItemType.Adrenaline
        };

        //CI Conscript
        [Description("Set how much 7.62 rounds CI Conscripts spawn with. Default is the maximum amount combat armour can carry")]
        public byte ciAmmo { get; set; } = 120;

        [Description("Exclusive items only for CI Conscripts. This can include sidearms and ammunition")]
        public List<ItemType> ciItems { get; set; } = new()
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
        public List<ItemType> ntfItems { get; set; } = new()
        {
            ItemType.GunCOM18
        };
    }
}