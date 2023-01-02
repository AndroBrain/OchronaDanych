using OchronaDanychAPI.data.entities;
using ShopManagmentAPI.data.entities;
using ShopManagmentAPI.domain;
using SQLite;
using SQLiteNetExtensions.Extensions;
using System.Diagnostics;

namespace ShopManagmentAPI.data.db.user;

public class UserDb : IUserDao
{
    public UserDb()
    {
        using (SQLiteConnection conn = new SQLiteConnection(DbSettings.dbPath))
        {
            conn.CreateTable<UserEntity>();
            conn.CreateTable<NoteEntity>();
        }
    }
    public UserEntity? Create(UserEntity user)
    {
        using (SQLiteConnection conn = new SQLiteConnection(DbSettings.dbPath))
        {
            try
            {
                conn.Insert(user);
                conn.UpdateWithChildren(user);
                return user;
            }  catch(SQLiteException e)
            {
                return null;
            }
        }
    }
    public UserEntity? Get(int id)
    {
        using (SQLiteConnection conn = new SQLiteConnection(DbSettings.dbPath))
        {
            try
            {
                var users = conn.GetAllWithChildren<UserEntity>(filter: u => u.Id == id);
                return users.FirstOrDefault(defaultValue: null);
            }
            catch (InvalidOperationException e)
            {
                return null;
            }
        }
    }
    public UserEntity? GetByEmail(string email)
    {
        using (SQLiteConnection conn = new SQLiteConnection(DbSettings.dbPath))
        {
            try
            {
                var users = conn.GetAllWithChildren<UserEntity>(filter: u => u.Email == email);
                return users.FirstOrDefault(defaultValue: null);
            }
            catch (InvalidOperationException e)
            {
                return null;
            }
        }
    }
}
