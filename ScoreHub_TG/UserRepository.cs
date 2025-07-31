using Microsoft.EntityFrameworkCore;

namespace ScoreHub_TG;

public class UserRepository
{
    private readonly TgBotDbContext _dbContext;

    public UserRepository(TgBotDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task AddUser(User user)
    {
        await _dbContext.Users.AddAsync(user);
        await _dbContext.SaveChangesAsync();
    }

    public async Task<string> GetUserToken(long chatId)
    {
        return await _dbContext.Users.Where(u => u.UserId == chatId).Select(u=>u.Token).FirstOrDefaultAsync();
    }
}