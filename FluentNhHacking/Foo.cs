using System;
using System.Collections.Generic;

namespace FluentNhHacking {
    public class Foo {
        public virtual Guid Id { get; private set; }
        public virtual string Name { get; set; }
        public virtual IEnumerable<Bar> Bars { get; set; }
        public virtual NameComponent FullName { get; set; }
    }

    public class NameComponent {
        public string Salutation { get; set; }
        public string Forename { get; set; }
        public string Surname { get; set; }
    }
}