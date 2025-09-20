using Content.Shared.DoAfter;
using Robust.Shared.Serialization;

namespace Content.Shared.Disarmable;

[Serializable, NetSerializable]
public sealed partial class DisarmDoAfterEvent : DoAfterEvent
{
    public override DoAfterEvent Clone() => this;
}
