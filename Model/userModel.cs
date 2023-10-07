using System;

namespace BackendAPI.Model
{
    // Basic neccesity for my application, where I added bio for real-time data purposes
    public class UserModel // Use uppercase for class names
    {
        public string username { get; set; } 
        public string password { get; set; }
        public string bio { get; set; }
    }

    // Values you need to login
    public class LoginModel
    {
        public string username { get; set; } 
        public string password { get; set; }
    }

    // Using to for users/{username} end point, not including password because not needed
    public class IndividualModel
    {
        public string username { get; set; }
        public string bio { get; set; }
    }
}
