using System.Collections.Generic;
using System.ComponentModel;
using MapGeneration;
using PlayerRoles;
using UnityEngine;
// ReSharper disable MemberCanBeMadeStatic.Global

namespace EscapePlan
{
    public class Config
    {
        //EscapeDoor configs
        [Description(
            "Data for the Escape Door. Set Room, position offset from the room, rotation and where the door will spawn escapees. Set to null to disable")]
        public EscapeAreaData EscapeArea { get; set; } =
            new()
            {
                EscapeRoom = RoomName.Outside,
                PositionOffset = new(-41.3f, -9, -36.1f),
            };
        
        [Description("Set whether escaping at a specific gate will spawn you in at that location, or simply use the default escape spawn")]
        public bool EscapeesSpawnAtEscapeGate { get; set; } = true;
        
        //Detained militant escapes
        [Description("Allow which militant classes are able to be detained and convert to the other team. Leave list empty to disable detained militant escapes")]
        public RoleTypeId[] DetainedMilitantsEscapees { get; set; } = 
        {
            // RoleTypeId.NtfCaptain,
            // RoleTypeId.NtfPrivate,
            // RoleTypeId.NtfSergeant,
            // RoleTypeId.NtfSpecialist,
            // RoleTypeId.ChaosConscript,
            // RoleTypeId.ChaosMarauder,
            // RoleTypeId.ChaosRepressor,
            // RoleTypeId.ChaosRifleman
        };
        
        [Description("Set which class will spawn if a Chaos Insurgent is detained and escapes")]
        public RoleTypeId DetainedChaosEscapeRole { get; set; } = RoleTypeId.NtfPrivate;
        
        [Description("Set which class will spawn if a Foundation militant is detained and escapes")]
        public RoleTypeId DetainedFoundationEscapeRole { get; set; } = RoleTypeId.ChaosConscript;
        
        [Description("List the items you'll like all CI and NTF escapees to receive while escaping.")]
        public ItemType[] RewardItems { get; set; } = 
        {
            ItemType.Adrenaline
        };

            //CI Conscript
        [Description("Set how much 7.62 rounds CI Conscripts spawn with. Default is the maximum amount combat armour can carry")]
        public byte CiAmmo { get; set; } = 120;

        [Description("Exclusive items only for CI Conscripts. This can include sidearms and ammunition")]
        public ItemType[] CiItems { get; set; } = 
        {
            ItemType.GunRevolver,
            ItemType.Ammo44cal, //Each ItemType.Ammo44cal only gives 6 bullets. Do it twice for 12 total spare bullets
            ItemType.Ammo44cal
        };
        
        [Description("Set how much 9mm rounds NTF escapees spawn with. Default is the maximum amount combat armour can carry")]
        // ReSharper disable once InconsistentNaming
        public byte Ntf9mmAmmo { get; set; } = 170;

        [Description("Set how much 5.56 ammo NTF escapees spawn with. Default is the maximum amount combat armour can carry")]
        public byte Ntf556Ammo { get; set; } = 120;
        
        [Description("Exclusive items only for NTF Escapees. This can include sidearms and ammunition")]
        public ItemType[] NtfItems { get; set; } = 
        {
            ItemType.GunCOM18
        };
    }
    
    public class EscapeAreaData
    {
        [Description("Set which room the EscapeDoor will spawn in")]
        public RoomName EscapeRoom { get; set; }

        [Description("Set the Door's position offset from the Spawn Room")]
        public Vector3 PositionOffset { get; set; }
        
        public Vector3 Rotation { get; set; }
    }
}