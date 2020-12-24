using System;
using System.Threading.Tasks;
using Banking.CommandProcessor.Events.EventStore;

namespace Banking.CommandProcessor.Entities
{
    public class EntityStore : IEntityStore
    {
        private IEventStore db;

        public EntityStore(IEventStore db)
        {
            this.db = db;
        }

        public async Task<T> Load<T>(Guid entityId) where T : Entity, new()
        {
            var events = await db.GetEvents(entityId);

            var entity = new T();

            foreach (var @event in events)
            {
                entity.When(@event);
            }

            return entity;
        }

        public async Task Save<T>(T entity) where T : Entity
        {
            foreach (var @event in entity.Changes)
            {
                await db.Save(@event);
            }
        }
    }
}
