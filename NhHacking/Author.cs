using System.Collections.Generic;

namespace NhHacking {
    public class Author {
        public virtual string Id { get; set; }
        public virtual string FirstName { get; set; }
        public virtual string LastName { get; set; }
    }

    public class Book {
        public virtual string Id { get; set; }
        public virtual string Title { get; set; }
        public virtual Publisher Publisher { get; set; }
    }

    public class Publisher {
        public virtual string Id { get; set; }
        public virtual string Name { get; set; }
    }

    public class Alert {
        public Alert() {
            Messages = new Dictionary<CxChannel, string>();
        }
        public virtual int Id { get; set; }
        public virtual string Name { get; set; }
        public virtual IDictionary<CxChannel, string> Messages { get; set; }
    }

    public enum CxChannel {
        Message,
        Email
    }
}