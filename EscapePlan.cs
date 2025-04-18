using LabApi.Loader.Features.Plugins;
using LabApi.Features.Wrappers.Items;
using System.Collections.Generic;
using System.Linq;
using Interactables.Interobjects.DoorUtils;
using InventorySystem.Items;
using InventorySystem.Items.Firearms;
using InventorySystem.Items.Usables;
using Mirror;
using LabApi.Events.Arguments.PlayerEvents;
using LabApi.Events.Handlers;
using LabApi.Features.Wrappers;
using MapGeneration;
using PlayerRoles;
using UnityEngine;
using Version = System.Version;
using Log = LabApi.Features.Console.Logger;
using BreakableDoor = Interactables.Interobjects.BreakableDoor;

namespace EscapePlan
{
    public class EscapePlan : Plugin<Config>
    {
        public static EscapePlan Instance { get; private set; }

        public static Vector3 SurfacePosition { get; private set; } 
        public static List<Player> MilitantEscapes { get; set; } = new(); //Broken PlayerChangedRoleEventArgs
        public override string Name { get; } = "Escape Plan";
        public override string Description { get; } = "Adds some extra functionality to the escaping mechanics";
        public override string Author { get; } = "Goodman";
        public override Version Version { get; } = new Version(1, 0, 0, 0);
        public override Version RequiredApiVersion { get; } = new Version(0, 0, 0);

        public override void Enable()
        {
            Instance = this;
            PlayerEvents.ChangedRole += OnPlayerEscape;
            ServerEvents.RoundStarted += OnRoundStarted;

            Log.Debug("EscapePlan Plugin Loaded");
        }

        public override void Disable()
        {
            Instance = null;
            PlayerEvents.ChangedRole -= OnPlayerEscape;
            ServerEvents.RoundStarted -= OnRoundStarted;
        }
        
        private void OnRoundStarted()
        {
            SurfacePosition = RoomIdentifier.AllRoomIdentifiers.First(x => x.Name == RoomName.Outside).transform.position;
            if (Config.DetainedNtfEscapes.Any() || Config.DetainedCiEscapes.Any())
            {
                //If detained militant escapes are allowed, spawn a primitive object at Gate B to catch non-civilian escapes
                var primitive = GameObject.CreatePrimitive(PrimitiveType.Cube);
                primitive.transform.position = SurfacePosition + new Vector3(124, -11, 18);
                primitive.transform.localScale = new Vector3(5,1,1);
                primitive.isStatic = true;

                var primitiveCollider = primitive.gameObject.GetComponent<BoxCollider>();
                primitiveCollider.isTrigger = true;
                primitiveCollider.gameObject.AddComponent<Components.DefaultEscapeComponent>();
            }
            
            if (Config.EscapeDoorRoom == 0) return;
            
            //If the Gate A escape door is enabled, spawn it in
            var toy = Object.Instantiate(
            (from gameObject in NetworkClient.prefabs.Values
                    where gameObject.name == "LCZ BreakableDoor"
                    select gameObject.GetComponent<BreakableDoor>()).First(), //Find the LCZ Door Prefab
            //Set world position relative to the room
            RoomIdentifier.AllRoomIdentifiers.First(x => x.Name == Config.EscapeDoorRoom).transform.position + Config.EscapeDoorPositionOffset, 
            Quaternion.Euler(Config.EscapeDoorRotation)
            );
            toy.RemainingHealth = int.MaxValue;
            toy.ServerChangeLock(DoorLockReason.SpecialDoorFeature, true);
            
            var collider = toy.gameObject.AddComponent<BoxCollider>();
            collider.isTrigger = true;
            collider.gameObject.AddComponent<Components.DoorEscapeComponent>();

            NetworkServer.Spawn(toy.gameObject);
        }

        private void OnPlayerEscape(PlayerChangedRoleEventArgs args) //Better catch-all for any escape compared to OnPlayerEscape; includes escapes done by the EscapeComponent colliders
        {
            if (args.ChangeReason != RoleChangeReason.Escaped) return;

            Player player = args.Player;

            //--------------PlayerChangedRoleEventArgs.OldRole is currently broken---------------
            // if (args.OldRole.RoleTypeId != RoleTypeId.Scientist && args.OldRole.RoleTypeId != RoleTypeId.ClassD) return;
            
            //Only civs get reward
            if (MilitantEscapes.Contains(player)) {MilitantEscapes.Remove(player); return;} //Broken PlayerChangedRoleEventArgs; Check the militant escape list to see if the player should get rewards for escaping
            
            void GiveItemsFromList(List<ItemType> itemList) { foreach (ItemType item in itemList) {player.AddItem(item);} } //Define function to only be used with this specific player
            
            //-----Give extra items and ammunition to escapees
            
            GiveItemsFromList(Config.rewardItems);

            if (args.NewRole == RoleTypeId.ChaosConscript) {
                player.SetAmmo(ItemType.Ammo762x39, Config.ciAmmo);
                GiveItemsFromList(Config.ciItems);
                return;
            }
            
            player.SetAmmo(ItemType.Ammo9x19,   Config.ntf_9mmAmmo);
            player.SetAmmo(ItemType.Ammo556x45, Config.ntf_556Ammo);

            GiveItemsFromList(Config.ntfItems);
        }
    }
}
