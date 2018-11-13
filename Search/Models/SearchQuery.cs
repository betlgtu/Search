namespace Search.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Diagnostics.CodeAnalysis;

    public class SearchQuery : BaseEntity
    {
        [SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public SearchQuery()
        {
            SearchResults = new HashSet<SearchResult>();
        }

        [Required]
        [StringLength(2048)]
        [DisplayName("Запрос")]
        public string Query { get; set; }

        [Required]
        [DisplayName("Дата поиска")]
        public DateTime SearchDate { get; set; }

        [ForeignKey("SearchEngine")]
        [DisplayName("Поисковая система")]
        public int SearchEngineId { get; set; }

        public virtual SearchEngine SearchEngine { get; set; }

        [SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<SearchResult> SearchResults { get; set; }
    }
}