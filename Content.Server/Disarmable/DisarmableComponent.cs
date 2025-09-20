namespace Content.Server.Disarmable;

/// <summary>
/// This is used for...
/// </summary>
[RegisterComponent]
public sealed partial class DisarmableComponent : Component
{
    [DataField("disarmTime"), ViewVariables(VVAccess.ReadWrite)]
    public float DisarmTime = 5f;

    [DataField("resultPrototype", required: true), ViewVariables(VVAccess.ReadWrite)]
    public string ResultPrototype = string.Empty;

}
