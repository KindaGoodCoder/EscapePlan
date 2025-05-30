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
        [Description("Data for the Escape Door. Set Room, position offset from the room, rotation and where the door will spawn escapees. Set to null to disable")]
        public EscapeDoorData EscapeDoor => new();
        public class EscapeDoorData
        {
            [Description("Set which room the EscapeDoor will spawn in")]
            public RoomName EscapeRoom => RoomName.Outside;

            [Description("Set the Door's position offset from the Spawn Room")]
            public Vector3 PositionOffset => new(-41.3f, -9, -36.1f);
            public Vector3 Rotation => new(0, 90, 0);
            [Description("Set where escapees will spawn after using the door")]
            public Vector3 SpawnLocation => new(20, -9, -46);
        }
        
        [Description("Set whether escaping at a specific gate will spawn you in at that location, or simply use the default escape spawn")]
        public bool EscapeesSpawnAtEscapeGate => true;
        
        //Detained militant escapes
        [Description("Allow which militant classes are able to be detained and convert to the other team. Leave list empty to disable detained militant escapes")]
        public RoleTypeId[] DetainedMilitantsEscapees => new RoleTypeId[]
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
        public RoleTypeId DetainedChaosEscapeRole => RoleTypeId.NtfPrivate;
        
        [Description("Set which class will spawn if a Foundation militant is detained and escapes")]
        public RoleTypeId DetainedFoundationEscapeRole => RoleTypeId.ChaosConscript;
        
        //Escapee Reward items//
            //Shared
        [Description("List the items you'll like all CI and NTF escapees to receive while escaping.")]
        public ItemType[] rewardItems => new[] 
        {
            ItemType.Adrenaline
        };

            //CI Conscript
        [Description("Set how much 7.62 rounds CI Conscripts spawn with. Default is the maximum amount combat armour can carry")]
        public byte ciAmmo => 120;

        [Description("Exclusive items only for CI Conscripts. This can include sidearms and ammunition")]
        public ItemType[] ciItems => new[]
        {
            ItemType.GunRevolver,
            ItemType.Ammo44cal, //Each ItemType.Ammo44cal only gives 6 bullets. Do it twice for 12 total spare bullets
            ItemType.Ammo44cal
        };

            //NTF 
        [Description("Set how much 9mm rounds NTF escapees spawn with. Default is the maximum amount combat armour can carry")]
        public byte ntf_9mmAmmo => 170;

        [Description("Set how much 5.56 ammo NTF escapees spawn with. Default is the maximum amount combat armour can carry")]
        public byte ntf_556Ammo => 120;
        
        [Description("Exclusive items only for NTF Escapees. This can include sidearms and ammunition")]
        public ItemType[] ntfItems => new[]
        {
            ItemType.GunCOM18
        };
    }
}