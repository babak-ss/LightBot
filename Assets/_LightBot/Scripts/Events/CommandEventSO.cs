using LightBot.Commands;
using UnityEngine;

namespace LightBot.Core
{
    [CreateAssetMenu(menuName = "Events/Command Event")]
    public class CommandEventSO : GenericEventSO<BaseCommand> { }
}