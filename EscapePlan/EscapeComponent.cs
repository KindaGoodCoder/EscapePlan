using LabApi.Events.Arguments.PlayerEvents;
using LabApi.Events.Handlers;
using LabApi.Features.Wrappers;
using PlayerRoles;
using UnityEngine;

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
            RoleTypeId escapeRole;
            Escape.EscapeScenarioType escapeScenario;
            
            switch (player.Role)
            {
                case RoleTypeId.Scientist:
                    if (player.IsDisarmed) {
                        escapeRole = RoleTypeId.ChaosConscript;
                        escapeScenario = Escape.EscapeScenarioType.CuffedScientist;
                    } else {
                        escapeRole = RoleTypeId.NtfSpecialist;
                        escapeScenario = Escape.EscapeScenarioType.Scientist;
                    }
                    break;
                case RoleTypeId.ClassD:
                    if (player.IsDisarmed) {
                        escapeRole = RoleTypeId.NtfPrivate;
                        escapeScenario = Escape.EscapeScenarioType.CuffedClassD;
                    } else {
                        escapeRole = RoleTypeId.ChaosConscript;
                        escapeScenario = Escape.EscapeScenarioType.ClassD;
                    }
                    break;
                case var _ when player.IsDisarmed && EscapePlan.config.DetainedMilitantsEscapees.Contains(player.Role):
                    escapeScenario = Escape.EscapeScenarioType.Custom;
                    escapeRole = player.Team == Team.ChaosInsurgency ? EscapePlan.config.DetainedChaosEscapeRole : EscapePlan.config.DetainedFoundationEscapeRole;
                    
                    break;
                default: return;
            }

            player.SetRole(escapeRole, RoleChangeReason.Escaped);
            PlayerEvents.OnEscaped(new PlayerEscapedEventArgs(player.ReferenceHub, escapeRole, escapeScenario));
            if (EscapePlan.config.EscapeesSpawnAtEscapeGate) player.Position = spawnPosition + EscapePlan.SurfacePosition + new Vector3(0, 0, Random.Range(0,5));
        }
    }
}