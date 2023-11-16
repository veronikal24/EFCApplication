using Application.DaoInterfaces;
using Domain.DTOs;
using Domain.Models;
using EFCDataAcces;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;


public class UserEfcDao2 : IUserDao
    {
        public async Task<User> CreateAsync(User user)
        {
            EntityEntry<User> newUser = await context.Users.AddAsync(user);
            await context.SaveChangesAsync();
            return newUser.Entity;
        }

        public async Task<User?> GetByUsernameAsync(string userName)
        {
            User? existing = await context.Users.FirstOrDefaultAsync(u =>
                u.UserName.ToLower().Equals(userName.ToLower())
            );
            return existing;
        }

        public async Task<IEnumerable<User>> GetAsync(SearchUserParametersDto searchParameters)
        {
            IQueryable<User> usersQuery = context.Users.AsQueryable();
            if (searchParameters.UsernameContains != null)
            {
                usersQuery = usersQuery.Where(u => u.UserName.ToLower().Contains(searchParameters.UsernameContains.ToLower()));
            }

            IEnumerable<User> result = await usersQuery.ToListAsync();
            return result;
        }

        public async Task<User?> GetByIdAsync(int id)
        {
            User? user = await context.Users.FindAsync(id);
            return user;
        }
        private readonly TodoContext context;

        public UserEfcDao2(TodoContext context)
        {
            this.context = context;
        }
    }
