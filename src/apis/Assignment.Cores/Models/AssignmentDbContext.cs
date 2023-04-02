using Assignment.Cores.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace Assignment.Cores.Models
{
    public class AssignmentDbContext : DbContext
    {
        #region Properties

        public virtual DbSet<Player>? Players { get; set; }

        #endregion

        #region Constructor

        protected AssignmentDbContext()
        {
        }

        public AssignmentDbContext(DbContextOptions<AssignmentDbContext> options) : base(options)
        {
        }

        #endregion

        #region Methods

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            var player = modelBuilder.Entity<Player>();
            player.HasKey(x => x.Id);
        }

        #endregion
    }
}
