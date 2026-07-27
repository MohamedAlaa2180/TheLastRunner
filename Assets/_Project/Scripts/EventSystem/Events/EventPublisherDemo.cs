using Reflex.Attributes;
using UnityEngine;

namespace Genesis.Core.Events
{
    public class EventPublisherDemo : MonoBehaviour
    {
        [Inject] private IEventBus _bus;

        [ContextMenu("Test Event")]
        public void TestEvent()
        {
            if (_bus == null) return;
            _bus.Publish(new TestEvent(Time.timeAsDouble));
        }
    }
}