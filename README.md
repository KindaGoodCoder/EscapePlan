# EscapePlan
**Escape with a Plan**

Plugin that adds on to the current escape system and introduces some new features such as:
- Gives civilian escapees additional items as a reward for escaping (Can be configured)
- Adds a new escape route with a configurable location; Maybe try putting it at Gate A
- Allows certain detained militant roles to be captured and defect to the enemy team (Disabled by default)

Features are almost completely configurable so customise it for whatever your server needs.

This is a remake of my old EscapePlan plugin for SCP: Containment Breach Multiplayer but designed for SL using the new LabAPI framework.

### How to Install?
1. Download the appropiate `.dll` release file
2. Find the SL Server config folder at `%APPDATA%\SCP Secret Laboratory\LabAPI\plugins` and move the `.dll` file to the appropiate port. 
3. Launch the server once to load the Configs and edit the `.yml` file in `\LabAPI\Configs\(port)` to your heart's content.

## Config file

```yml
# Which militant classes can be detained and converted to the other team. Leave empty for none (Disabled by default)
detained_militant_escapes:
- NtfCaptain
- NtfPrivate
- NtfSergeant
- NtfSpecialist
- ChaosConscript
- ChaosMarauder
- ChaosRepressor
- ChaosRifleman

# Set the room the EscapeDoor will spawn. Set to RoomName.Unnamed or 0 to disable secondary escape route
escape_door_room: RoonName.Outside
# If EscapeDoor is enabled, set the position offset in the configured Room
escape_door_position:
  x: -41.25
  y: -9
  z: -36.1
# If EscapeDoor is enabled, set the Eular Rotation
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
The current code was tested to work for 14.0.3 but since in 14.1, surface has been moved from y=1000 to y=300, there may be some errors. However, the program should be able to automatically account for this.
But since there is currently no avaliable 14.1 server tool, it is currently not possible to test it.
