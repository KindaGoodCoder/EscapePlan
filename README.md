# EscapePlan
**Escape with a Plan**

Plugin that adds on to the current escape system and introduces some new features such as:
- Gives civilian escapees additional items as a reward for escaping (Can be configured)
- Adds a new escape route with a configurable location; Maybe try putting it at Gate A
- Allows certain detained militant roles to be captured and defect to the enemy team (Disabled by default)

Features are almost completely configurable so customise it for whatever your server needs.

This is a remake of my old EscapePlan plugin for SCP: Containment Breach Multiplayer but designed for SL using the new LabAPI framework.

##How to Install?
1. Download the appropiate `.dll` release file
2. Find the SL Server config folder at `%APPDATA%\SCP Secret Laboratory\LabAPI\plugins` and choose the appropiate port
3. Launch the server once to load the Configs and edit the `.yml` file in `\LabAPI\Configs\(port)` to your heart's content.

##Config file

```C#
//Allow detained militant escapes -- Disabled by default.
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
[Description("Set whether or not if you want a second Escape route on surface. Perhaps in Gate A")]
public bool EscapeDoorEnabled { get; set; } = true;

[Description("If GateAEscape is enabled, get the position and rotation. Default location replaces the door next to the gate where the CI car spawns")]
public Vector3 EscapeDoorPosition { get; set;  } = new (-41.25f, 991, -36.1f);

public Vector3 EscapeDoorRotation { get; set; } = new (0,90,0);

//---------------Escapee Reward items
//Shared
[Description("List the items you'll like all CI and NTF escapees to receive while escaping. Ammunition can be added but unless its not an option, you should use set ammo for the specific class. Armour is currently not supported (Trying so will spawn the player with two armour items)")]
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
```

Note:
The current code is intended to work for 14.0.3 and will not work as intended in 14.1 as that update has moved the Surface zone from y=1000 to y=300.

A EscapePlan-14.1.dll release has been made but could not be tested as there is there is no dedicated server tool avaliable for it. However that version of the code simply subtracts 700 from the y-axis from all Vector3 position uses.
