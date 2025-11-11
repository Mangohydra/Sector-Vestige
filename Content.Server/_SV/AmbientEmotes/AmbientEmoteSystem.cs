using Content.Server.Chat.Systems;
using Content.Shared._SV.AmbientEmotes;
using Content.Shared.Chat;
using Robust.Shared.Random;
using Robust.Shared.Timing;

namespace Content.Server._SV.AmbientEmotes;

/// <summary>
/// This handles the AmbientEmotes, basicly a copy of the narcolepsy timing code plus a emote for testing.
/// </summary>

public sealed class AmbientEmoteSystem : EntitySystem
{
    [Dependency] private readonly ChatSystem _chat = default!;
    [Dependency] private readonly IRobustRandom _random = default!;
    [Dependency] private readonly IGameTiming _timing = default!;

    public override void Initialize()
    {
        base.Initialize();

        SubscribeLocalEvent<AmbientEmoteComponent, MapInitEvent>(OnMapInit);
    }

    private void OnMapInit(Entity<AmbientEmoteComponent> ent, ref MapInitEvent args)
    {
        ent.Comp.NextIncidentTime = _timing.CurTime + _random.Next(ent.Comp.MinTimeBetweenIncidents, ent.Comp.MaxTimeBetweenIncidents);
        Dirty(ent);
    }

    /// <summary>
    /// Changes the time till next emote.
    /// </summary>
    public void AdjustAmbientEmoteTimer(Entity<AmbientEmoteComponent?> ent, TimeSpan time)
    {
        if (!Resolve(ent, ref ent.Comp, false))
            return;

        ent.Comp.NextIncidentTime = _timing.CurTime + time;
        Dirty(ent);
    }

    public override void Update(float frameTime)
    {
        base.Update(frameTime);

        var query = EntityQueryEnumerator<AmbientEmoteComponent>();

        while (query.MoveNext(out var uid, out var emote))
        {
            if (emote.NextIncidentTime > _timing.CurTime)
                continue;

            // TODO: Replace with RandomPredicted once the engine PR is merged https://github.com/space-wizards/RobustToolbox/pull/5849
            var interval = _random.NextDouble() * (emote.MaxTimeBetweenIncidents - emote.MinTimeBetweenIncidents) + emote.MinDurationOfIncident;

            // Set the new time
            emote.NextIncidentTime = _timing.CurTime + TimeSpan.FromSeconds(timeBetween + duration);

            Dirty(uid, emote);

            _chat.TryEmoteWithChat(uid, "Cough", ChatTransmitRange.Normal, forceEmote: true);
        }
    }
}
