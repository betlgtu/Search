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
        [DisplayName("�����")]
        public string URL { get; set; }

        [Required]
        [StringLength(2048)]
        [DisplayName("�����")]
        public string Text { get; set; }
        
        [ForeignKey("SearchQuery")]
        [DisplayName("��������� ������")]
        public int SearchQueryId { get; set; }

        public virtual SearchQuery SearchQuery { get; set; }
    }
}
