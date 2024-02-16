using Contracts.Domain.Interfaces;

namespace Contracts.Domain
{
    public abstract class EntityBase<TKey> : IEntityBase<TKey>
    {
        public TKey Id { get; set; }
    }
}
