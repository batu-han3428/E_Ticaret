using System.ComponentModel.DataAnnotations.Schema;

namespace E_Ticaret.Domains.Entities.Base.Interfaces
{
    public interface IBaseEntity
    {
        Int64 Id { get; set; }
        bool IsActive { get; set; }
        bool IsDeleted { get; set; }

        [Column(TypeName = "datetime")]
        DateTimeOffset CreatedAt { get; set; }

        [Column(TypeName = "datetime")]
        DateTimeOffset? UpdatedAt { get; set; }
    }
}
