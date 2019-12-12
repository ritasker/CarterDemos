using System.Collections.Generic;

namespace CarterDemos
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