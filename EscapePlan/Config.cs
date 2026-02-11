using System.ComponentModel;
using PlayerRoles;
using UnityEngine;
// ReSharper disable InconsistentNaming

// ReSharper disable MemberCanBeMadeStatic.Global

namespace EscapePlan
{
    public class Config
    {
        public EscapeAreaData EscapeArea { get; set; } = new()
        {
            Enabled = true,
            PositionOffset = new Vector3(-41.3f, -9, -36.1f),
            BoundsSize = Vector3.one * 4f,
            DoorRotationOffset = new Vector3(0f, 0f, 0f)
        };
        
        [Description("Set whether escaping at a specific gate will spawn you in at that Gate, or simply use the default escape spawn for the role")]
        public bool EscapeesSpawnAtEscapeGate { get; set; } = true;

        [Description("Set whether or not Escapees will get the configured loadouts or the base-game default loadouts. Setting to false will use the loadouts defined below")]
        public bool UseDefaultEscapeeLoadout { get; set; } = false;

        //CI Conscripts
        [Description("Configure CI Conscript full loadout. Can include sidearms and additional ammunition")]
        public ItemType[] CiConscriptLoadout { get; set; } = new[] 
        {
            ItemType.KeycardChaosInsurgency,
            ItemType.GunAK,
            ItemType.GunRevolver,
            ItemType.ArmorCombat,
            ItemType.Medkit,
            ItemType.Painkillers,
            ItemType.Adrenaline,
            ItemType.Ammo44cal,
            ItemType.Ammo12gauge
        };
        
        [Description("Set how much 7.62 rounds CI Conscripts spawn with. Includes ammunition in guns")]
        public ushort Ci762Ammo { get; set; } = 150;
        
        //NTF Specialists
        [Description("Set NTF Specialists full loadout. This can include sidearms and ammunition")]
        public ItemType[] NtfSpecialistLoadout { get; set; } = new[]
        {
            ItemType.KeycardMTFOperative,
            ItemType.GunE11SR,
            ItemType.GunCOM18,
            ItemType.ArmorCombat,
            ItemType.Medkit,
            ItemType.Adrenaline,
            ItemType.GrenadeHE,
        };
        
        [Description("Set how much 9mm rounds NTF Specialists spawn with. Includes ammunition in guns")]
        public ushort NtfSpecialist9mmAmmo { get; set; } = 191;

        [Description("Set how much 5.56 ammo NTF Specialists spawn with. Includes ammunition in guns")]
        public ushort NtfSpecialist556Ammo { get; set; } = 160;
        
        //NTF Privates
        [Description("Exclusive items only for Class-D who escape as NTF Private. THIS IS IN ADDITION to the NTF Private loadout")]
        public ItemType[] NtfPrivateEscapeeAdditionalItems { get; set; } = new[]
        {
            ItemType.Adrenaline,
            ItemType.GunCOM18
        };

        [Description("Set how much 9mm ammo NTF Privates Escapees spawn with. Does NOT includes ammunition in firearms")]
        public ushort NtfPrivate9mmAmmo { get; set; } = 170;
        
        [Description("Set how much 5.56 ammo NTF Privates Escapees spawn with. Does NOT includes ammunition in firearms")]
        public ushort NtfPrivate556Ammo { get; set; } = 120;

        //Detained militant escapes
        [Description("Allow which militant classes are able to be detained and convert to the other team. Leave list empty to disable detained militant escapes")]
        public RoleTypeId[] DetainedMilitantsEscapees { get; set; } = new RoleTypeId[]
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
    }
    
    public struct EscapeAreaData
    {
        public bool Enabled { get; set; }
        public Vector3 PositionOffset { get; set; }
        
        public Vector3 BoundsSize { get; set; }
        
        public Vector3 DoorRotationOffset { get; set; }
    }
}