using System.ComponentModel.DataAnnotations;

namespace Garmetix.Core.Interfaces
{
    public interface IEntity
    {
        [Key]
        Guid Id { get; set; }
    }
}