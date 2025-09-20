using Content.Shared.DoAfter;
using Content.Shared.Verbs;
using Content.Shared.Disarmable; //берём ивент отсюда

namespace Content.Server.Disarmable;

public sealed class DisarmableSystem : EntitySystem
{
    [Dependency] private readonly SharedDoAfterSystem _doAfter = default!;
    [Dependency] private readonly IEntityManager _entMan = default!;

    public override void Initialize()
    {
        base.Initialize();
        SubscribeLocalEvent<DisarmableComponent, GetVerbsEvent<Verb>>(OnGetVerbs);
        SubscribeLocalEvent<DisarmableComponent, DisarmDoAfterEvent>(OnDoAfter);
    }

    private void OnGetVerbs(EntityUid uid, DisarmableComponent comp, GetVerbsEvent<Verb> args)
    {
        if (!args.CanInteract || !args.CanAccess)
            return;

        var verb = new Verb
        {
            Text = "Обезвредить",
            Act = () =>
            {
                var ev = new DisarmDoAfterEvent();
                var doAfterArgs = new DoAfterArgs(EntityManager, args.User, TimeSpan.FromSeconds(comp.DisarmTime), ev, uid, target: uid)
                {
                    BreakOnDamage = true,
                    BreakOnMove = true,
                    NeedHand = true,
                };

                _doAfter.TryStartDoAfter(doAfterArgs);
            },
            Priority = 2
        };

        args.Verbs.Add(verb);
    }

    private void OnDoAfter(EntityUid uid, DisarmableComponent comp, DisarmDoAfterEvent args)
    {
        if (args.Cancelled || args.Handled)
            return;

        var xform = Transform(uid);
        _entMan.SpawnEntity(comp.ResultPrototype, xform.Coordinates);
        _entMan.DeleteEntity(uid);

        args.Handled = true;
    }
}
