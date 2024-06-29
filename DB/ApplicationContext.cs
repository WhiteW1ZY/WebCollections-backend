using backend.Models;
using backend.Models.Fields;
using Microsoft.EntityFrameworkCore;

namespace backend.DB
{
    public class ApplicationContext: DbContext
    {
        public ApplicationContext(DbContextOptions<ApplicationContext> options)
            :base(options)
        {
            Database.EnsureCreated();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Comment>()
                .HasOne(u => u.Owner)
                .WithMany(c => c.Comments)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Reaction>()
                .HasOne(u => u.Owner)
                .WithMany(c => c.reactions)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Tag>()
                .HasMany(i => i.Items)
                .WithMany(t => t.Tags);

            modelBuilder.Entity<Item>()
                .HasOne(c => c.collection)
                .WithMany(i => i.Items)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Collection>()
                .HasOne(u => u.Owner)
                .WithMany(c => c.Collections)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<BooleanField>()
                .HasOne(i => i.item)
                .WithMany(f => f.BooleanFields)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<IntegerField>()
                .HasOne(i => i.item)
                .WithMany(f => f.IntegerFields)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<StringField>()
                .HasOne(i => i.item)
                .WithMany(f => f.StringFields)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<DateField>()
                .HasOne(i => i.item)
                .WithMany(f => f.DateFields)
                .OnDelete(DeleteBehavior.Cascade);


            modelBuilder.Entity<User>().HasData(
                    new User { id = 1, Login = "admin", Password = "admin", Role = "admin", isBanned=false}
                    );
        }

        public DbSet<User> Users { get; set; } = null!;

        public DbSet<Item> Items { get; set; } = null!;
        public DbSet<Tag> tags { get; set; } = null!;

        public DbSet<Comment> Comments { get; set; } = null!;
        public DbSet<Reaction> Reactions { get; set; } = null!;

        public DbSet<Collection> Collections { get; set; } = null!;

        public DbSet<BooleanField> BooleanFields { get; set; } = null!;
        public DbSet<DateField> DateFields { get; set; } = null!;
        public DbSet<IntegerField> IntegerFields { get; set; } = null!;
        public DbSet<StringField> StringFields { get; set; } = null!;

    }
}
