# EscapePlan
**Escape with a Plan**

Plugin that adds on to the current escape system and introduces some new features such as:
- Adds a new escape door with a configurable location where you can choose where it spawns; Maybe try putting it at Gate A
- Allows for the full and easy customisability of the CI Conscript and NTF Specialist loadouts
  - Escapees that escape as NTF Privates get extra items in addition to their default loadouts.
- Configuration option to allow players to spawn at the gate they escaped from. For example, a Class-D who escaped from Gate B can spawn on the Gate B road as a Chaos Insurgency Conscript.
- Allows specific militant roles to be detained and defect to the enemy team (Disabled by default)

There's plenty of customisable configuration options to suit whatever your server needs, so feel free to only use the features you like.

This is a remake of my old EscapePlan plugin for SCP: Containment Breach Multiplayer but designed for SL using the new LabAPI framework.

### How to Install?
1. Download the appropiate `.dll` release file
2. Find the SL Server LabAPI plugins folder at `%APPDATA%\SCP Secret Laboratory\LabAPI\plugins` and move the `.dll` file to the desired port. 
3. Launch the server once to load the Configs and edit the `.yml` file in `\LabAPI\Configs\(port)` to your heart's content.

## Config file

```yml
# Config options for the custom escape area
escape_area:
  enabled: true
  position_offset: #Offset from surface position
    x: -41.3
    y: -9
    z: -36.1
  bounds_size: #Size of the Escape Bound. Must be within range of the Bounds to escape
    x: 4
    y: 4
    z: 4
  door_rotation_offset: #Door will spawn where the positionOffset is declared. Adjust rotation
    x: 0
    y: 90
    z: 0
# Set whether escaping at a specific gate will spawn you in at that Gate, or simply use the default escape spawn for the role
escapees_spawn_at_escape_gate: true

# Set whether or not Escapees will get the configured loadouts or the base-game default loadouts. Setting to false will use the loadouts defined below
use_default_escapee_loadout: false

# Configure CI Conscript full loadout. Can include sidearms and additional ammunition
ci_conscript_loadout:
- KeycardChaosInsurgency
- GunAK
- GunRevolver
- ArmorCombat
- Medkit
- Painkillers
- Adrenaline
- Ammo44cal
- Ammo12gauge

# Set how much 7.62 rounds CI Conscripts spawn with. Includes ammunition in guns
ci762_ammo: 150

# Set NTF Specialists full loadout. This can include sidearms and ammunition
ntf_specialist_loadout:
- KeycardMTFOperative
- GunE11SR
- GunCOM18
- ArmorCombat
- Medkit
- Adrenaline
- GrenadeHE

# Set how much 9mm rounds NTF Specialists spawn with. Includes ammunition in guns
ntf_specialist9mm_ammo: 191

# Set how much 5.56 ammo NTF Specialists spawn with. Includes ammunition in guns
ntf_specialist556_ammo: 160

# Exclusive items only for Class-D who escape as NTF Private. This is in addition to the NTF Private loadout
ntf_private_escapee_additional_items:
- Adrenaline
- GunCOM18

# Set how much 9mm ammo NTF Privates Escapees spawn with. Does NOT includes ammunition in firearms
ntf_private9mm_ammo: 170

# Set how much 5.56 ammo NTF Privates Escapees spawn with. Does NOT includes ammunition in firearms
ntf_private556_ammo: 120

# Allow which militant classes are able to be detained and convert to the other team. Leave list empty to disable detained militant escapes
detained_militants_escapees:
- NtfCaptain
- NtfPrivate
- NtfSergeant
- NtfSpecialist
- ChaosConscript
- ChaosMarauder
- ChaosRepressor
- ChaosRifleman

# Set which class will spawn if a Chaos Insurgent is detained and escapes
detained_chaos_escape_role: NtfPrivate

# Set which class will spawn if a Foundation militant is detained and escapes
detained_foundation_escape_role: ChaosConscript
