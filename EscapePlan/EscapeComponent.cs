using System;
using System.Runtime.Remoting.Messaging;
using LabApi.Events.Arguments.PlayerEvents;
using LabApi.Events.Handlers;
using LabApi.Features.Wrappers;
using PlayerRoles;
using UnityEngine;
using Random = UnityEngine.Random;

// using Log = LabApi.Features.Console.Logger;

namespace EscapePlan
{
    public class EscapeComponent : MonoBehaviour
    //Escape handler for the Gate A Escape Door
    {
        public Vector3 spawnPosition;
    
        private void OnTriggerEnter(Collider collider)
        {
            if (!Player.TryGet(collider.gameObject, out Player player)) return;
            
            (RoleTypeId escapeRole, Escape.EscapeScenarioType escapeScenario) = player.Role switch
            {
                RoleTypeId.Scientist => player.IsDisarmed ?
                    (RoleTypeId.ChaosConscript, Escape.EscapeScenarioType.CuffedScientist)
                    :
                    (RoleTypeId.NtfSpecialist, Escape.EscapeScenarioType.Scientist),
                
                RoleTypeId.ClassD => player.IsDisarmed ?
                    (RoleTypeId.NtfPrivate, Escape.EscapeScenarioType.CuffedClassD)
                    :
                    (RoleTypeId.ChaosConscript, Escape.EscapeScenarioType.ClassD),
                
                _ when player.IsDisarmed && EscapePlan.config.DetainedMilitantsEscapees.Contains(player.Role) =>
                    (
                        player.Team == Team.ChaosInsurgency ? EscapePlan.config.DetainedChaosEscapeRole : EscapePlan.config.DetainedFoundationEscapeRole,
                        Escape.EscapeScenarioType.Custom
                    ),
                
                _ => (RoleTypeId.None, Escape.EscapeScenarioType.None)
            };
            if (escapeRole == RoleTypeId.None) return;

            player.SetRole(escapeRole, RoleChangeReason.Escaped);
            
            if (EscapePlan.config.EscapeesSpawnAtEscapeGate) player.Position = spawnPosition + EscapePlan.SurfacePosition + new Vector3(0, 0, Random.Range(0,5));
        }
    }
}