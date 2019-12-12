using System;
using System.Collections.Generic;
using System.Linq;
using CarterDemos.ActorDemo;

namespace CarterDemos.Tests
{
    public class TestingActorProvider : ActorProvider
    {

        public TestingActorProvider(Dictionary<int, Actor> actors = null)
        {
            base.actors = actors ?? new Dictionary<int, Actor>();
        }

        public bool HasMatch(Func<Actor, bool> predicate)
        {
            return actors.Values.Count(predicate) > 0;
        }

        public bool HasNoMatch(Func<Actor, bool> predicate = null)
        {
            if (predicate == null)
            {
                return actors.Values.Count == 0;
            }
            return actors.Values.Count(predicate) == 0;
        }
    }
}