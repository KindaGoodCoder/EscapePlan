﻿using LabApi.Loader.Features.Plugins;
using System.Collections.Generic;
using System.Linq;
using Interactables.Interobjects.DoorUtils;
using Mirror;
using LabApi.Events.Arguments.PlayerEvents;
using LabApi.Events.Handlers;
using LabApi.Features.Wrappers;
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
        public static List<Player> MilitantEscapes { get; set; } = new(); //Broken PlayerChangedRoleEventArgs
        public override string Name { get; } = "Escape Plan";
        public override string Description { get; } = "Escape with a plan";
        public override string Author { get; } = "Escapist";
        public override Version Version { get; } = new Version(1, 0, 0, 0);
        public override Version RequiredApiVersion { get; } = new Version(0, 0, 0);

        public override void Enable()
        {
            Instance = this;
            PlayerEvents.ChangedRole += OnPlayerEscape;
            ServerEvents.RoundStarted += OnRoundStarted;

            Log.Debug("EscapePlan successfully loaded");
        }

        public override void Disable()
        {
            Instance = null;
            PlayerEvents.ChangedRole -= OnPlayerEscape;
            ServerEvents.RoundStarted -= OnRoundStarted;
        }
        
        private void OnRoundStarted()
        {
            if (Config.DetainedNtfEscapes.Any() || Config.DetainedCiEscapes.Any())
            {
                //If detained militant escapes are allowed, spawn a primitive object at Gate B to catch non-civilian escapes
                var primitive = GameObject.CreatePrimitive(PrimitiveType.Cube);
                primitive.transform.position = new Vector3(124, 989, 18);
                primitive.transform.localScale = new Vector3(5,1,1);
                primitive.isStatic = true;

                var primitiveCollider = primitive.gameObject.GetComponent<BoxCollider>();
                primitiveCollider.isTrigger = true;
                primitiveCollider.gameObject.AddComponent<Components.DefaultEscapeComponent>();
            }

            if (!Config.EscapeDoorEnabled) return;
            
            //If the Gate A escape door is enabled, spawn it in
            var toy = Object.Instantiate(
                (from gameObject in NetworkClient.prefabs.Values where gameObject.name == "LCZ BreakableDoor" select gameObject.GetComponent<BreakableDoor>()).FirstOrDefault(), //LCZ Breakable Door Prefab 
                Config.EscapeDoorPosition, //Position
                Quaternion.Euler(Config.EscapeDoorRotation)); //Rotation
            toy.RemainingHealth = int.MaxValue;
            toy.ServerChangeLock(DoorLockReason.SpecialDoorFeature, true);

            var collider = toy.gameObject.AddComponent<BoxCollider>();
            collider.isTrigger = true;
            collider.gameObject.AddComponent<Components.DoorEscapeComponent>();

            NetworkServer.Spawn(toy.gameObject);
        }

        private void OnPlayerEscape(PlayerChangedRoleEventArgs args) //Better catch-all for any escape, including ones done by the EscapeComponent colliders
        {
            if (args.ChangeReason != RoleChangeReason.Escaped) return;

            Player player = args.Player;

            // if (args.OldRole.RoleTypeId != RoleTypeId.Scientist && args.OldRole.RoleTypeId != RoleTypeId.ClassD) return; //--------------PlayerChangedRoleEventArgs.OldRole is currently broken---------------
            
            //Only civs get reward
            if (MilitantEscapes.Contains(player)) {MilitantEscapes.Remove(player); return;} //Broken PlayerChangedRoleEventArgs; Check the militant escape list to see if the player should get rewards for escaping
            
            void GiveItemsFromList(List<ItemType> itemList) { foreach (ItemType item in itemList) {player.AddItem(item);} } //Define function to only be used with this specific player
            
            //-----Give extra items and ammunition to escapees
            
            GiveItemsFromList(Config.rewardItems); //For every item in the config shared reward list, give to the player

            if (args.NewRole == RoleTypeId.ChaosConscript) {
                player.SetAmmo(ItemType.Ammo762x39, Config.ciAmmo);
                GiveItemsFromList(Config.ciItems);
                return;
            }
            
            player.SetAmmo(ItemType.Ammo9x19, Config.ntf_9mmAmmo);
            player.SetAmmo(ItemType.Ammo556x45, Config.ntf_556Ammo);

            GiveItemsFromList(Config.ntfItems);
        }
    }
}