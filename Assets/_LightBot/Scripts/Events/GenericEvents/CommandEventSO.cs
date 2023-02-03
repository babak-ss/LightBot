using LightBot.Commands;
using UnityEngine;

namespace LightBot.Events
{
    [CreateAssetMenu(menuName = "Events/Command Event")]
    public class CommandEventSO : GenericEventSO<BaseCommand> { }
}