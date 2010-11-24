using System;

namespace NhHacking {
    public class Person
    {
        public virtual int PersonId { get; set; }
        public virtual string Firstname { get; set; }   
        public virtual string Nickname { get; set; }    
        public virtual string Surname { get; set; } 
        public virtual DateTime BirthDate { get; set; }     
    }
}