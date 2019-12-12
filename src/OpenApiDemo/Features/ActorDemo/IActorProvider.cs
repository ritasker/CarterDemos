using System.Collections.Generic;

namespace OpenApiDemo.Features.ActorDemo
{
    public interface IActorProvider
    {
        IEnumerable<Actor> Get();
        Actor Get(int id);
        void Delete(int id);
        void Update(Actor actor);
        Actor Create(Actor actor);
    }
}