namespace Search.Models
{
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.ComponentModel.DataAnnotations;
    using System.Diagnostics.CodeAnalysis;

    public partial class SearchEngine : BaseEntity
    {
        [SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public SearchEngine()
        {
            SearchQueries = new HashSet<SearchQuery>();
        }

        [Required]
        [StringLength(50)]
        [DisplayName("Название")]
        public string Name { get; set; }

        [Required]
        [StringLength(250)]
        [DisplayName("Адрес")]
        public string URL { get; set; }

        [Required]
        [StringLength(250)]
        [DisplayName("Доменный адрес")]
        public string Domain { get; set; }

        [SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<SearchQuery> SearchQueries { get; set; }
    }
}
