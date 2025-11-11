using Robust.Shared.GameStates;
using Robust.Shared.Prototypes;
using Robust.Shared.Serialization;
using Robust.Shared.Audio;
using Robust.Shared.Toolshed.Syntax;

namespace Content.Shared._SV.AmbientEmotes;

[RegisterComponent, NetworkedComponent]
[AutoGenerateComponentState(fieldDeltas: true), AutoGenerateComponentPause]
public sealed partial class AmbientEmoteComponent : Component
{
    /// <summary>
    /// Maximum time between emote incidents
    /// </summary>
    [DataField(required: true), AutoNetworkedField]
    public TimeSpan MaxTimeBetweenIncidents;

    /// <summary>
    /// Minimum time between emote incidents
    /// </summary>
    [DataField(required: true), AutoNetworkedField]
    public TimeSpan MinTimeBetweenIncidents;

    /// <summary>
    /// When the next emote incident will occur
    /// </summary>
    [DataField, AutoNetworkedField, AutoPausedField]
    public TimeSpan NextIncidentTime;
}
