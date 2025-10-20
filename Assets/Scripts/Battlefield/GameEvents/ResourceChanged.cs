using Units.Resources;

namespace System.Collections.Generic
{
    public sealed class ResourceChanged : IGameEvent
    {
        public readonly ResourceId ResourceId;
        public readonly int OldValue;
        public readonly int NewValue;

     
        public ResourceChanged(ResourceId id, int oldV, int newV)
        {
            ResourceId = id; OldValue = oldV; NewValue = newV;;
        }
    }
}