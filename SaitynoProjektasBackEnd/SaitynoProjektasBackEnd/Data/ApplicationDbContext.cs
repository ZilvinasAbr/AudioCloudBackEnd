using Microsoft.EntityFrameworkCore;
using SaitynoProjektasBackEnd.Models;

namespace SaitynoProjektasBackEnd.Data
{
    public class ApplicationDbContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Following> Followings { get; set; }
        public DbSet<Like> Likes { get; set; }
        public DbSet<Playlist> Playlists { get; set; }
        public DbSet<PlaylistSong> PlaylistSongs { get; set; }
        public DbSet<Song> Songs { get; set; }
        public DbSet<Genre> Genres { get; set; }
        public DbSet<Event> Events { get; set; }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder); 

            modelBuilder.Entity<Following>()
                .HasKey(f => new {f.FollowedId, f.FollowerId});

            modelBuilder.Entity<Following>()
                .HasOne(f => f.Follower)
                .WithMany(u => u.Following)
                .HasForeignKey(f => f.FollowerId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Event>()
                .HasOne(e => e.User)
                .WithMany(u => u.Events)
                .HasForeignKey(e => e.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Following>()
                .HasOne(f => f.Followed)
                .WithMany(u => u.Followers)
                .HasForeignKey(f => f.FollowedId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<PlaylistSong>()
                .HasKey(ps => new {ps.PlaylistId, ps.SongId});

            modelBuilder.Entity<PlaylistSong>()
                .HasOne(ps => ps.Playlist)
                .WithMany(p => p.PlaylistSongs)
                .HasForeignKey(ps => ps.PlaylistId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<PlaylistSong>()
                .HasOne(ps => ps.Song)
                .WithMany(p => p.PlaylistSongs)
                .HasForeignKey(ps => ps.SongId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Like>()
                .HasOne(l => l.User)
                .WithMany(u => u.Likes)
                .HasForeignKey(l => l.UserId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
