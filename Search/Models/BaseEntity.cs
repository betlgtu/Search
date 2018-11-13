using System.ComponentModel.DataAnnotations;

namespace Search.Models
{
    public abstract class BaseEntity
    {
        [Key]
        public int Id { get; set; }
    }
}