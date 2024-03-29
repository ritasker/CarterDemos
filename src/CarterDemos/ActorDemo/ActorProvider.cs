using System.Collections.Generic;
using System.Linq;

namespace CarterDemos.ActorDemo
{
    public class ActorProvider : IActorProvider
    {
        protected Dictionary<int, Actor> actors;

        public ActorProvider()
        {
            actors = new Dictionary<int, Actor>
            {
                [1] = new Actor {Name = "Johnny Depp", Id = 1, Age = 56},
                [2] = new Actor {Name = "Hugh Jackman", Id = 2, Age = 50},
                [3] = new Actor {Name = "Ewan McGregor", Id = 3, Age = 48},
                [4] = new Actor {Name = "Mark Wahlberg", Id = 4, Age = 48}
            };
        }

        public IEnumerable<Actor> Get()
        {
            return actors.Values.ToList();
        }

        public Actor Get(int id)
        {
            return actors[id];
        }

        public void Delete(int id)
        {
            actors.Remove(id);
        }

        public void Update(Actor actor)
        {
            actors[actor.Id] = actor;
        }

        public Actor Create(Actor actor)
        {
            actor.Id = actors.Keys.Any() ? actors.Keys.Max() + 1 : 1;

            actors.Add(actor.Id, actor);

            return actor;
        }
    }
}