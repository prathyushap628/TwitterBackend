using Twitter.Models;
using Dapper;

using Twitter.Repositories;
using Twitter.Utilities;
using Twitter.DTOs;
//using Twitter.Utilities;

namespace Twitter.Repositories;

public interface IUserRepository
{
    Task<Users> GetByEmail(string Username);
    Task<Users> CreateNewUser(Users Item);
    Task<Users> GetById(int UserId);
    Task Update(Users Item);
}
public class UserRepository : BaseRepository, IUserRepository
{
    public UserRepository(IConfiguration config) : base(config)
    {

    }

    public async Task<Users> CreateNewUser(Users Item)
    {
        var query = $@"INSERT INTO {TableNames.users} ( user_name, email, passwor_d)
	VALUES ( @UserName, @Email, @Password) RETURNING *";

        using (var con = NewConnection)
            return await con.QuerySingleOrDefaultAsync<Users>(query, Item);
    }

    public async Task<Users> GetByEmail(string Email)
    {
        var query = $@"SELECT * FROM ""{TableNames.users}"" 
        WHERE email = @email";

        using (var con = NewConnection)
            return await con.QuerySingleOrDefaultAsync<Users>(query, new { Email });
    }

    public async Task<Users> GetById(int UserId)
    {
        var query = $@"SELECT * FROM {TableNames.users} WHERE user_id = @UserId";

        using (var con = NewConnection)
            return await con.QuerySingleOrDefaultAsync<Users>(query, new { UserId });
    }
    
    public async Task Update(Users Item)
    {
        var query = $@"UPDATE {TableNames.users} SET user_name = @UserName 
         WHERE user_id = @UserId";

        using (var con = NewConnection)
            await con.ExecuteAsync(query, new { UserName = Item.UserName, Userid = Item.UserId });
    }


}
