using System;
using LabApi.Loader.Features.Plugins;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using Interactables.Interobjects.DoorUtils;
using Mirror;
using LabApi.Events.Arguments.PlayerEvents;
using LabApi.Events.Handlers;
using LabApi.Features.Wrappers;
using MapGeneration;
using PlayerRoles;
using UnityEngine;
using Version = System.Version;
// using Log = LabApi.Features.Console.Logger;
using BreakableDoor = Interactables.Interobjects.BreakableDoor;
using Object = UnityEngine.Object;

namespace EscapePlan
{
    public class EscapePlan : Plugin<Config>
    {
        public static Vector3 SurfacePosition { get; private set; } 
        public static List<Player> MilitantEscapes { get; } = new(); //Broken PlayerChangedRoleEventArgs bandaid
        
        public override string Name => "Escape Plan";
        public override string Description => "Adds extra functionality to the escaping mechanics";
        public override string Author => "Goodman";
        public override Version Version => new(1, 0, 0, 0);
        public override Version RequiredApiVersion => new(0, 0, 0);

        public override void Enable()
        {
            PlayerEvents.ChangedRole += OnPlayerEscape;
            ServerEvents.RoundStarted += OnRoundStarted;

            LabApi.Features.Console.Logger.Debug("EscapePlan Plugin Loaded");
        }

        public override void Disable()
        {
            MilitantEscapes.Clear();
            PlayerEvents.ChangedRole -= OnPlayerEscape;
            ServerEvents.RoundStarted -= OnRoundStarted;
        }
        
        private static void OnRoundStarted()
        {
            SurfacePosition = RoomIdentifier.AllRoomIdentifiers.First(x => x.Name == RoomName.Outside).transform.position;
            
            //If Escapees spawn at the gate they escape at or detained militants are allowed to escape, spawn a primitive at Gate B escape area to handle Gate B escapes
            if (Config.DetainedMilitantsEscapees.Any() || Config.EscapeesSpawnAtEscapeGate)
            {
                GameObject primitive = GameObject.CreatePrimitive(PrimitiveType.Cube);
                primitive.transform.position = SurfacePosition + new Vector3(124, -10, 20);
                primitive.transform.localScale = new Vector3(5,1,1);
                primitive.isStatic = true;

                BoxCollider primitiveCollider = primitive.gameObject.GetComponent<BoxCollider>();
                primitiveCollider.isTrigger = true;
                primitiveCollider.gameObject.AddComponent<EscapeComponent>().spawnPosition = new Vector3(120, -4, -44.3f);
            }
            
            //If the Gate A escape door is enabled, spawn it in to handle Gate A escapes
            if (Config.EscapeDoor == null) return;
            var escapeDoor = Config.EscapeDoor;

            BreakableDoor toy = Object.Instantiate(
                (from gameObject in NetworkClient.prefabs.Values
                    where gameObject.name == "LCZ BreakableDoor"
                    select gameObject.GetComponent<BreakableDoor>()).First(), //Find the LCZ Door Prefab
                //Set world position relative to the room
                RoomIdentifier.AllRoomIdentifiers.First(x => x.Name == escapeDoor.EscapeRoom).transform.position + escapeDoor.PositionOffset,
                Quaternion.Euler(escapeDoor.Rotation)
            );
            toy.RemainingHealth = int.MaxValue;
            toy.ServerChangeLock(DoorLockReason.SpecialDoorFeature, true);

            BoxCollider collider = toy.gameObject.AddComponent<BoxCollider>();
            collider.isTrigger = true;
            collider.gameObject.AddComponent<EscapeComponent>().spawnPosition = escapeDoor.SpawnLocation;
            NetworkServer.Spawn(toy.gameObject);
        }

        private static void OnPlayerEscape(PlayerChangedRoleEventArgs args) //PlayerEscapeEvent doesn't include escapes made by the Collider Components
        {
            if (args.ChangeReason != RoleChangeReason.Escaped) return;
            Player player = args.Player;

            //--------------PlayerChangedRoleEventArgs.OldRole is currently broken---------------
            // if (args.OldRole.RoleTypeId != RoleTypeId.Scientist && args.OldRole.RoleTypeId != RoleTypeId.ClassD) return;
            
            if (MilitantEscapes.Contains(player)) {MilitantEscapes.Remove(player); return;} //Broken PlayerChangedRoleEventArgs; Check the militant escape list to see if the player should get rewards for escaping
            
            void GiveItemsFromList(List<ItemType> itemList) {foreach (ItemType item in itemList) {player.AddItem(item);}} //Define function to only be used with this specific player
            
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
