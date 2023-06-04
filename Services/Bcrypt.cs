using System;
using Bcr = BCrypt.Net;

namespace ShimanoTask.Services.BCrypt;

public class PasswordManager
{
    public string HashPassword(string password)
    {
    	// Generate a salt with a work factor of 10
        string salt = Bcr.BCrypt.GenerateSalt(10);
        
        // Hash the password with the generated salt
        string hashedPassword = Bcr.BCrypt.HashPassword(password, salt);
        
        return hashedPassword;
    }

    public bool VerifyPassword(string password, string hashedPassword)
    {
        // Verify the password against the hashed password
        bool passwordMatch = Bcr.BCrypt.Verify(password, hashedPassword);
        
        return passwordMatch;
    }
}


