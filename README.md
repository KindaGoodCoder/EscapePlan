# EscapePlan
**Escape with a Plan**

Plugin that adds on to the current escape system and introduces some new features such as:
- Gives civilian escapees additional items as a reward for escaping (Can be configured)
- Adds a new escape route with a configurable location; Maybe try putting it at Gate A
- Allows certain detained militant roles to be captured and defect to the enemy team (Disabled by default)

Features are almost completely configurable so customise it for whatever your server needs.

This is a remake of my old EscapePlan plugin for SCP: Containment Breach Multiplayer but designed for SL using the new LabAPI framework.

## How to Install?
1. Download the appropiate `.dll` release file
2. Find the SL Server config folder at `%APPDATA%\SCP Secret Laboratory\LabAPI\plugins` and choose the appropiate port
3. Launch the server once to load the Configs and edit the `.yml` file in `\LabAPI\Configs\(port)` to your heart's content.

## Config file

```yml
# Which Foundation militant classes can be detained and converted to the other team. Leave empty for none
detained_ntf_escapes:
#- NtfCaptain
#- NtfPrivate
#- NtfSergeant
#- NtfSpecialist
# # Which Insurgent militant classes can be detained and converted to the other team. Leave empty for none
detained_ci_escapes:
#- ChaosConscript
#- ChaosMarauder
#- ChaosRepressor
#- ChaosRifleman
# Set whether or not if you want a second Escape route on surface. Perhaps at Gate A
escape_door_enabled: true
# Set the position and rotation. Default location replaces the door next to the gate where the CI car spawns
escape_door_position:
  x: -41.25
  y: 991
  z: -36.1
escape_door_rotation:
  x: 0
  y: 90
  z: 0
# List the items you'll like all CI and NTF escapees to receive while escaping. Ammunition can be added but unless its not an option, you should use set ammo for the specific class. Armour is currently not supported (Trying so will spawn the player with two armour items)
reward_items:
- Adrenaline
# Set how much 7.62 rounds CI Conscripts spawn with. Default is the maximum amount combat armour can carry
ci_ammo: 120
# Exclusive items only for CI Conscripts. This can include sidearms and ammunition
ci_items:
- GunRevolver
- Ammo44cal
- Ammo44cal
# Set how much 9mm rounds NTF escapees spawn with. Default is the maximum amount combat armour can carry
ntf_9mm_ammo: 170
# Set how much 5.56 ammo NTF escapees spawn with. Default is the maximum amount combat armour can carry
ntf_556_ammo: 120
# Exclusive items only for NTF Escapees. This can include sidearms and ammunition
ntf_items:
- GunCOM18
```

Note:
The current code is intended to work for 14.0.3 and will not work as intended in 14.1 as that update has moved the Surface zone from y=1000 to y=300.

A EscapePlan-14.1.dll release has been made but could not be tested as there is there is no dedicated server tool avaliable for it. However that version of the code simply subtracts 700 from the y-axis from all Vector3 position uses.
