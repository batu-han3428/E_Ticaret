using E_Ticaret.Domains.Entities.Base.Interfaces;

namespace E_Ticaret.Domains.Entities.Base.Models
{
    public class BaseEntity : IBaseEntity
    {
        public Int64 Id { get ; set ; }
        public bool IsActive { get ; set ; }
        public bool IsDeleted { get ; set ; }
        public DateTimeOffset CreatedAt { get ; set ; }
        public DateTimeOffset? UpdatedAt { get ; set ; }
    }
}
