using LabApi.Loader.Features.Plugins;
using System.Linq;
using HarmonyLib;
using Interactables.Interobjects.DoorUtils;
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
using Object = UnityEngine.Object;

namespace EscapePlan
{
    public class EscapePlan : Plugin<Config>
    {
        public static Config config {get; private set;}
        public static Vector3 SurfacePosition { get; private set; }
        
        public override string Name => "Escape Plan";
        public override string Description => "Adds extra functionality to the escaping mechanics";
        public override string Author => "Goodman";
        public override Version Version => new(1, 0, 0, 0);
        public override Version RequiredApiVersion => new(0, 0, 0);
        private readonly Harmony _harmony = new Harmony("EscapePlan");

        public override void Enable()
        {
            config = Config;
            PlayerEvents.Escaped += OnPlayerEscape;
            PlayerEvents.Escaping += maybe;
            ServerEvents.RoundStarted += OnRoundStarted;
            _harmony.PatchAll();
            
            Log.Debug("EscapePlan Plugin Loaded");
        }

        public override void Disable()
        {
            PlayerEvents.Escaped -= OnPlayerEscape;
            PlayerEvents.Escaping -= maybe;
            ServerEvents.RoundStarted -= OnRoundStarted;
        }

        private void maybe(PlayerEscapingEventArgs args)
        {
            Log.Info("Perhaps");
            args.EscapeScenario = Escape.EscapeScenarioType.Custom;
            args.NewRole = RoleTypeId.Scp106;
            args.IsAllowed = true;
        }
        
        private void OnRoundStarted()
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

        private void OnPlayerEscape(PlayerEscapedEventArgs args)
        {
            Player player = args.Player;
            
            if (args.EscapeScenarioType == Escape.EscapeScenarioType.Custom) return;
            
            void GiveItemsFromList(ItemType[] itemList) {foreach (ItemType item in itemList) {player.AddItem(item);}} //Define function to only be used with this specific player
            
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
