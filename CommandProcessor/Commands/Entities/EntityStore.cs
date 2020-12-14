using System.Threading.Tasks;
using CommandProcessor.Entities;
using CommandProcessor.Events.Persistence;

namespace CommandProcessor.Commands.Entities
{
    public class EntityStore : IEntityStore
    {
        private IEventStore db;

        public EntityStore(IEventStore db)
        {
            this.db = db;
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
