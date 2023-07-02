﻿namespace API.Utilities;

public class Hashing
{
    private static string GenerateSalt()
    {
        return BCrypt.Net.BCrypt.GenerateSalt(12);
    }

    public static string HashPassword(string password)
    {
        return BCrypt.Net.BCrypt.HashPassword(password, GenerateSalt());
    }

    public static bool ValidatePassword(string password, string hashpassword) 
    {
        return BCrypt.Net.BCrypt.Verify(password, hashpassword);
    }
}