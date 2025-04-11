using LabApi.Loader.Features.Plugins;
using System.Collections.Generic;
using System.Linq;
using Interactables.Interobjects.DoorUtils;
using Mirror;
using LabApi.Events.Arguments.PlayerEvents;
using LabApi.Events.Arguments.ServerEvents;
using LabApi.Events.Handlers;
using LabApi.Features.Wrappers;
using LabApi.Features.Enums;
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

            Log.Debug("Plan to escape");
        }

        public override void Disable()
        {
            Instance = null;
            PlayerEvents.ChangedRole -= OnPlayerEscape;
            ServerEvents.RoundStarted -= OnRoundStarted;
        }

        private void OnRoundStarted()
        {
            if (!Config.EscapeDoor) return;
            
            var toy = Object.Instantiate(ToyPrefab, Config.EscapeDoorPosition, Quaternion.Euler(Config.EscapeDoorRotation));
            toy.RemainingHealth = int.MaxValue;
            toy.ServerChangeLock(DoorLockReason.SpecialDoorFeature, true);
            
            var collider = toy.gameObject.AddComponent<BoxCollider>();
            collider.isTrigger = true;
            collider.gameObject.AddComponent<EscapeComponent>();
            
            NetworkServer.Spawn(toy.gameObject);
        }

        private static BreakableDoor ToyPrefab => (from gameObject in NetworkClient.prefabs.Values where gameObject.name == "LCZ BreakableDoor" select gameObject.GetComponent<BreakableDoor>()).FirstOrDefault();

        private void OnPlayerEscape(PlayerChangedRoleEventArgs args)
        {
            if (args.ChangeReason != RoleChangeReason.Escaped) return;
            
            Player player = args.Player;
            
            void GiveItemsFromList(List<ItemType> itemList) { foreach (ItemType item in itemList) {player.AddItem(item);} } //Define function to only be used with this specific player
            
            //-----Give extra items and ammunition to escapees
            GiveItemsFromList(Config.rewardItems); //For every item in the config shared reward list, give to the player

            if (args.NewRole == RoleTypeId.ChaosConscript) { //If player escaped as a Class-D or detained Scientist
                player.SetAmmo(ItemType.Ammo762x39, Config.ciAmmo);
                GiveItemsFromList(Config.ciItems);
                return; //End the function, avoiding the code handling NTF escapesSingle
            }
            
            //If the Chaos Conscript code is not triggered, that means the escapee became an NTF and this part will run
            player.SetAmmo(ItemType.Ammo9x19, Config.ntf_9mmAmmo);
            player.SetAmmo(ItemType.Ammo556x45, Config.ntf_556Ammo);

            GiveItemsFromList(Config.ntfItems);

            //-------TBC
        }
    }
}