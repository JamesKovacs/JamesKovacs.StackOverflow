using System;

namespace fm.web
{
    public class User
    {
        public static string default_username = "guest";
        public static string default_password = "guest";

        private UserType usertype;

        public virtual int? Id { get; set; }
        public virtual string Username { get; set; }
        public virtual string Password { get; set; }
        public virtual DateTimeOffset Datecreated { get; set; }
        public virtual string Firstname { get; set; }
        public virtual string Lastname { get; set; }
        public virtual string Email { get; set; }
        public virtual UserType Usertype
        {
            get { return usertype; }
            set { usertype = value; }
        }
    }
}

namespace fm.web
{
    public class UserType
    {
        public virtual int? Id { get; set; }
        public virtual string Title { get; set; }
    }
}