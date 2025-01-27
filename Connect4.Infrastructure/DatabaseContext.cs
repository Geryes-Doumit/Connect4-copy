using Connect4.Infrastructure.Model;
using Microsoft.EntityFrameworkCore;

namespace Connect4.Infrastructure
{
    /// <summary>
    /// The primary database context for the Connect4 application, configuring 
    /// the entity sets and providing connection settings for SQLite.
    /// </summary>
    public class DatabaseContext : DbContext
    {
        /// <summary>
        /// Gets or sets the table of users.
        /// </summary>
        public DbSet<UserSqlite> Users { get; set; } = null!;

        /// <summary>
        /// Gets or sets the table of games.
        /// </summary>
        public DbSet<GameSqlite> Games { get; set; } = null!;

        /// <summary>
        /// Gets or sets the table of boards.
        /// </summary>
        public DbSet<BoardSqlite> Boards { get; set; } = null!;

        /// <summary>
        /// Parameterless constructor for design-time support.
        /// </summary>
        public DatabaseContext() { }

        /// <summary>
        /// Initializes a new instance of the <see cref="DatabaseContext"/> class with specific options.
        /// </summary>
        /// <param name="options">The options for configuring the context.</param>
        public DatabaseContext(DbContextOptions<DatabaseContext> options) : base(options) { }

        /// <summary>
        /// Configures the context to use SQLite if not already configured.
        /// </summary>
        /// <param name="optionsBuilder">A builder used to create or modify options for this context.</param>
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlite(@"Data Source=connect4.db");
            }
        }

        /// <summary>
        /// Called by EF Core to further configure the model that was discovered by convention.
        /// Seeds initial data (users) in the database.
        /// </summary>
        /// <param name="modelBuilder">The builder being used to construct the model for this context.</param>
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Seeding example users
            modelBuilder.Entity<UserSqlite>().HasData(
                new UserSqlite
                {
                    Id = 1,
                    Username = "Marc",
                    HashPwd = "Ik1okgeDOdeQq4j8bYCbSL+jd4sAOynYIueTDhwmsZsTXhw/2UyzDJPfy447m1JC\r\n"
                },
                new UserSqlite
                {
                    Id = 2,
                    Username = "Geryes",
                    HashPwd = "vhIPqqrRkHjjATeu0OzQpW5nSgSP53rt6G+xxzHoyLcmlJT8Jod7+ogE6j7BFelX\r\n"
                },
                new UserSqlite
                {
                    Id = 3,
                    Username = "Matthias",
                    HashPwd = "HqQJMbZAb4f6KF5xmUhBfBShyxlNGmFdyI0E6QKg8LC3uOg3or4Zqhees7KTOjXC\r\n"
                },
                new UserSqlite
                {
                    Id = 4,
                    Username = "Alice",
                    HashPwd = "sMqnLsFfiNA78VQBL4c6H34gt3UOlDXY/IRs+G6BmNubZB8LBo/Tm03A3AyTFEWQ\r\n"
                },
                new UserSqlite
                {
                    Id = 5,
                    Username = "Bob",
                    HashPwd = "Y74bnkEsTuJNs+n0KqY/JdEh5xhFJKi7XA6H8f1b2UiQzf4XZwCTIEHTXdjnU5fj\r\n"
                },
                new UserSqlite
                {
                    Id = 6,
                    Username = "John",
                    HashPwd = "9dDM3cyozqNc+Zvs7JO5g8w60m2noWP6WhDQ13MDcbKyb3NTDuXlNi0rPMuU6DWM\r\n"
                },
                new UserSqlite
                {
                    Id = 7,
                    Username = "Gabin",
                    HashPwd = "IN7TOsgmFXU/F9O7j0UHcN4SsdhvBhN9cPszS35zf3HIC4JT4W7mHMVt2pV8z2kG\r\n"
                }
            );

            modelBuilder.Entity<BoardSqlite>().HasData(
                new BoardSqlite
                {
                    Id = 1,
                    CurrentPlayerId = 2,
                    State = "0000000;0000000;0000000;0000000;0000000;0000000"
                },
                new BoardSqlite
                {
                    Id = 2,
                    CurrentPlayerId = 2,
                    State = "0000000;0000000;0000000;0000000;0000000;0000000"
                },
                new BoardSqlite
                {
                    Id = 3,
                    CurrentPlayerId = 3,
                    State = "0000000;0000000;0000000;0000000;0000000;0000000"
                },
                new BoardSqlite
                {
                    Id = 4,
                    CurrentPlayerId = 4,
                    State = "0000000;0000000;0000000;0000000;0000000;0000000"
                },
                new BoardSqlite
                {
                    Id = 5,
                    CurrentPlayerId = 5,
                    State = "0000000;0000000;0000000;0000000;0000000;0000000"
                },
                new BoardSqlite
                {
                    Id = 6,
                    CurrentPlayerId = 2,
                    State = "0000000;0000000;0000000;0000000;0000000;0000000"
                },
                new BoardSqlite
                {
                    Id = 7,
                    CurrentPlayerId = 2,
                    State = "0000000;0000000;0000000;0000000;0000000;0000000"
                },
                new BoardSqlite
                {
                    Id = 8,
                    CurrentPlayerId = 3,
                    State = "0000000;0000000;0000000;0000000;0000000;0000000"
                },
                new BoardSqlite
                {
                    Id = 9,
                    CurrentPlayerId = 1,
                    State = "0000000;0000000;0000000;0000000;0000000;1111000"
                },
                new BoardSqlite
                {
                    Id = 10,
                    CurrentPlayerId = 1,
                    State = "0001000;0001000;0001000;0001000;0000000;0000000"
                },

                new BoardSqlite
                {
                    Id = 11,
                    CurrentPlayerId = 1,
                    State = "0000000;0000000;1000000;0100000;0010000;0001000"
                },
                new BoardSqlite
                {
                    Id = 12,
                    CurrentPlayerId = 1,
                    State = "1111000;0001000;0000100;0000010;0000000;0000000"
                },
                new BoardSqlite
                {
                    Id = 13,
                    CurrentPlayerId = 2,
                    State = "0000000;0000000;0000000;0000000;1111000;0000000"
                },
                new BoardSqlite
                {
                    Id = 14,
                    CurrentPlayerId = 2,
                    State = "0000000;0000000;0000010;0000010;0000010;0000010"
                },
                new BoardSqlite
                {
                    Id = 15,
                    CurrentPlayerId = 2,
                    State = "0000000;0000000;1000000;0100000;0010000;0001000"
                },
                new BoardSqlite
                {
                    Id = 16,
                    CurrentPlayerId = 3,
                    State = "0000000;0000000;0000000;0000010;0000100;0001000"
                },
                new BoardSqlite
                {
                    Id = 17,
                    CurrentPlayerId = 6,
                    State = "0000000;0000000;0000000;0000000;0000000;0000000"
                },
                new BoardSqlite
                {
                    Id = 18,
                    CurrentPlayerId = 6,
                    State = "0000000;0000000;0000000;0000000;0000000;0000000"
                },
                new BoardSqlite
                {
                    Id = 19,
                    CurrentPlayerId = 5,
                    State = "0000000;0000000;0000000;0000000;0000000;0000000"
                },
                new BoardSqlite
                {
                    Id = 20,
                    CurrentPlayerId = 6,
                    State = "0000000;0000000;0000000;0000000;0000000;0000000"
                }
             );

            // Seeding example waiting games
            modelBuilder.Entity<GameSqlite>().HasData(
                new GameSqlite
                {
                    Id = 1,
                    BoardId = 1,
                    Name = "Game 1",
                    HostId = 2,
                    GuestId = 3,
                    Status = GameStatusSqlite.Finished,
                    WinnerId = 2
                },
                new GameSqlite
                {
                    Id = 2,
                    BoardId = 2,
                    Name = "Game 2",
                    HostId = 2,
                    GuestId = 4,
                    WinnerId = 2,
                    Status = GameStatusSqlite.Finished
                },
                new GameSqlite
                {
                    Id = 3,
                    BoardId = 3,
                    Name = "Game 3",
                    HostId = 3,
                    GuestId = 5,
                    WinnerId = 3,
                    Status = GameStatusSqlite.Finished
                },
                new GameSqlite
                {
                    Id = 4,
                    BoardId = 4,
                    Name = "Game 4",
                    HostId = 3,
                    GuestId = 6,
                    WinnerId = 3,
                    Status = GameStatusSqlite.Finished
                },
                new GameSqlite
                {
                    Id = 5,
                    BoardId = 5,
                    Name = "Game 5",
                    HostId = 4,
                    GuestId = 2,
                    WinnerId = 4,
                    Status = GameStatusSqlite.Finished
                },
                new GameSqlite
                {
                    Id = 6,
                    BoardId = 6,
                    Name = "Game 6",
                    HostId = 4,
                    GuestId = 3,
                    WinnerId = 4,
                    Status = GameStatusSqlite.Finished
                },
                new GameSqlite
                {
                    Id = 7,
                    BoardId = 7,
                    Name = "Game 7",
                    HostId = 5,
                    GuestId = 2,
                    WinnerId = 5,
                    Status = GameStatusSqlite.Finished
                },
                new GameSqlite
                {
                    Id = 8,
                    BoardId = 8,
                    Name = "Game 8",
                    HostId = 5,
                    GuestId = 3,
                    WinnerId = 5,
                    Status = GameStatusSqlite.Finished
                },
                new GameSqlite
                {
                    Id = 9,
                    BoardId = 9,
                    Name = "Game 9",
                    HostId = 1,
                    GuestId = 2,
                    WinnerId = 1,
                    Status = GameStatusSqlite.Finished
                },
                new GameSqlite
                {
                    Id = 10,
                    BoardId = 10,
                    Name = "Game 10",
                    HostId = 1,
                    GuestId = 3,
                    WinnerId = 1,
                    Status = GameStatusSqlite.Finished
                },
                new GameSqlite
                {
                    Id = 11,
                    BoardId = 11,
                    Name = "Game 11",
                    HostId = 1,
                    GuestId = 4,
                    WinnerId = 1,
                    Status = GameStatusSqlite.Finished
                },
                new GameSqlite
                {
                    Id = 12,
                    BoardId = 12,
                    Name = "Game 12",
                    HostId = 1,
                    GuestId = 5,
                    WinnerId = 1,
                    Status = GameStatusSqlite.Finished
                },
                new GameSqlite
                {
                    Id = 13,
                    BoardId = 13,
                    Name = "Game 13",
                    HostId = 2,
                    GuestId = 3,
                    WinnerId = 2,
                    Status = GameStatusSqlite.Finished
                },
                new GameSqlite
                {
                    Id = 14,
                    BoardId = 14,
                    Name = "Game 14",
                    HostId = 2,
                    GuestId = 4,
                    WinnerId = 2,
                    Status = GameStatusSqlite.Finished
                },
                new GameSqlite
                {
                    Id = 15,
                    BoardId = 15,
                    Name = "Game 15",
                    HostId = 5,
                    GuestId = 4,
                    WinnerId = 5,
                    Status = GameStatusSqlite.Finished
                },
                new GameSqlite
                {
                    Id = 16,
                    BoardId = 16,
                    Name = "Game 16",
                    HostId = 3,
                    GuestId = 4,
                    WinnerId = 3,
                    Status = GameStatusSqlite.Finished
                },
                new GameSqlite
                {
                    Id = 17,
                    BoardId = 17,
                    Name = "Game 17",
                    HostId = 6,
                    GuestId = 7,
                    WinnerId = 6,
                    Status = GameStatusSqlite.Finished
                },
                new GameSqlite
                {
                    Id = 18,
                    BoardId = 18,
                    Name = "Game 18",
                    HostId = 6,
                    GuestId = 4,
                    WinnerId = 6,
                    Status = GameStatusSqlite.Finished
                },
                new GameSqlite
                {
                    Id = 19,
                    BoardId = 19,
                    Name = "Game 19",
                    HostId = 5,
                    Status = GameStatusSqlite.WaitingForPlayers
                },
                new GameSqlite
                {
                    Id = 20,
                    BoardId = 20,
                    Name = "Game 20",
                    HostId = 6,
                    Status = GameStatusSqlite.WaitingForPlayers
                }
            );
        }
    }
}
