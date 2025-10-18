using System.Linq;

namespace System.Collections.Generic
{
    public class TriggerManager
    {
        private readonly List<ITrigger> _triggers = new();

        public void Add(ITrigger trigger)
        {
            if (trigger == null)
                throw new ArgumentNullException(nameof(trigger));

            _triggers.Add(trigger);
            trigger.Enable();
            //_triggers.Sort((a, b) => a.TriggerId.CompareTo(b.TriggerId)); // keep ordered
        }

        public void Remove(ITrigger trigger)
        {
            if (trigger == null)
                return;

            _triggers.Remove(trigger);
        }

        public IEnumerable<ITrigger> All => _triggers;
        
    }
}