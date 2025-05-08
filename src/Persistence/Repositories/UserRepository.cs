using Domain.Entities;

namespace Persistence.Repositories;

/// <summary>
/// Repository for managing <see cref="User"/> entities in the Database.
/// </summary>
/// <param name="context"></param>
public class UserRepository(ApplicationDbContext context) 
    : BaseRepository<User>(context);