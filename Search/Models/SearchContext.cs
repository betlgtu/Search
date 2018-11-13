namespace Search.Models
{
    using System.Data.Entity;

    public partial class SearchContext : DbContext
    {
        public SearchContext()
            : this("name=SearchContextConnection")
        { }

        public SearchContext(string nameOrConnectionString)
            : base(nameOrConnectionString)
        { }

        public virtual DbSet<SearchEngine> SearchEngines { get; set; }
        public virtual DbSet<SearchQuery> SearchQueries { get; set; }
        public virtual DbSet<SearchResult> SearchResults { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<SearchEngine>()
                .HasMany(e => e.SearchQueries)
                .WithRequired(e => e.SearchEngine)
                .WillCascadeOnDelete(true);

            modelBuilder.Entity<SearchQuery>()
                .HasMany(e => e.SearchResults)
                .WithRequired(e => e.SearchQuery)
                .WillCascadeOnDelete(true);
        }
    }
}
