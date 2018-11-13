namespace Search.Models
{
    using System;
    using System.ComponentModel;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public partial class SearchResult : BaseEntity
    {
        [Required]
        [StringLength(2048)]
        [DisplayName("Адрес")]
        public string URL { get; set; }

        [Required]
        [StringLength(2048)]
        [DisplayName("Текст")]
        public string Text { get; set; }
        
        [ForeignKey("SearchQuery")]
        [DisplayName("Поисковый запрос")]
        public int SearchQueryId { get; set; }

        public virtual SearchQuery SearchQuery { get; set; }
    }
}
