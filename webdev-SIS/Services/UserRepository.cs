using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;
using System.Text;
using webdev_SIS.DataLayer;
using webdev_SIS.Models;

namespace webdev_SIS.Services
{
    public class UserRepository
    {
        private readonly ApplicationDbContext _db;

        public UserRepository(ApplicationDbContext db)
        {
            _db = db;
        }

        // Improved password hashing with salt
        public static string HashPassword(string password)
        {
            // Create a salt
            byte[] salt = new byte[16];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(salt);
            }

            // Create the hash
            var pbkdf2 = new Rfc2898DeriveBytes(password, salt, 10000, HashAlgorithmName.SHA256);
            byte[] hash = pbkdf2.GetBytes(20);

            // Combine salt and hash
            byte[] hashBytes = new byte[36];
            Array.Copy(salt, 0, hashBytes, 0, 16);
            Array.Copy(hash, 0, hashBytes, 16, 20);

            return Convert.ToBase64String(hashBytes);
        }






        // Verify password
        public static bool VerifyPassword(string password, string storedHash)
        {
            // Convert base64-encoded hash back to bytes
            byte[] hashBytes = Convert.FromBase64String(storedHash);

            // Extract salt (first 16 bytes)
            byte[] salt = new byte[16];
            Array.Copy(hashBytes, 0, salt, 0, 16);

            // Compute hash with the same salt
            var pbkdf2 = new Rfc2898DeriveBytes(password, salt, 10000, HashAlgorithmName.SHA256);
            byte[] hash = pbkdf2.GetBytes(20);

            // Compare computed hash with stored hash
            for (int i = 0; i < 20; i++)
            {
                if (hashBytes[i + 16] != hash[i])
                    return false;
            }
            return true;
        }





        public async Task<int> AddUser(UserEntity user)
        {
            try
            {
                if (!string.IsNullOrEmpty(user.PasswordHash))
                {
                    user.PasswordHash = HashPassword(user.PasswordHash);
                }

                // First insert the user directly
                _db.Users.Add(new UserEntity
                {
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    Email = user.Email,
                    PasswordHash = user.PasswordHash,
                    Role = user.Role,
                    Subject = user.Subject,
                    Position = user.Position,
                    Status = user.Status ?? "Active",
                    CreatedAt = DateTime.Now
                });

                await _db.SaveChangesAsync();

                // Get the last inserted user
                var newUser = await _db.Users
                    .OrderByDescending(u => u.Id)
                    .FirstOrDefaultAsync(u => u.Email == user.Email);

                return newUser?.Id ?? 0;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error creating user: {ex.Message}");
                throw;
            }
        }




        public async Task<UserEntity> LoginUser(string email, string password)
        {
            try
            {
                // Find user by email
                var user = await _db.Users.FirstOrDefaultAsync(u => u.Email == email);

                if (user == null)
                {
                    Console.WriteLine($"No user found with email: {email}");
                    return null;
                }

                // Verify password
                if (VerifyPassword(password, user.PasswordHash))
                {
                    Console.WriteLine("Password verified successfully");
                    return user;
                }
                else
                {
                    Console.WriteLine("Password verification failed");
                    return null;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Login error: {ex.Message}");
                return null;
            }
        }






        // Add this method to your UserRepository class
        public async Task<List<UserEntity>> GetUsersByRole(string role)
        {
            // Make sure this method filters out soft-deleted records
            return await _db.Users
                .Where(u => u.Role == role && u.IsDeleted == false)
                .ToListAsync();
        }






        public async Task<UserEntity> GetUserByEmail(string email)
        {
            return await _db.Users.FirstOrDefaultAsync(u => u.Email == email && u.Status == "Active" && !u.IsDeleted);
        }




        // In your UserRepository class
        public async Task<int> SoftDeleteUser(int id)
        {
            var user = await _db.Users.FindAsync(id);

            if (user == null || user.IsDeleted)
            {
                return 0; // User not found or already deleted
            }

            user.IsDeleted = true;
            user.Status = "Inactive";

            await _db.SaveChangesAsync();
            return 1; // Successfully deleted
        }

        public async Task<bool> ChangePassword(int userId, string newPassword)
        {
            try
            {
                var newHashedPassword = HashPassword(newPassword);

                var user = await _db.Users.FirstOrDefaultAsync(u => u.Id == userId && !u.IsDeleted);

                if (user == null)
                    return false;

                user.PasswordHash = newHashedPassword;
                await _db.SaveChangesAsync();

                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Change password error: " + ex.Message);
                return false;
            }
        }

    }
}