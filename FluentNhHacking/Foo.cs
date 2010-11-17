using System;
using System.Collections.Generic;

namespace FluentNhHacking {
    public class Foo {
        public virtual Guid Id { get; private set; }
        public virtual string Name { get; set; }
        public virtual IEnumerable<Bar> Bars { get; set; }
    }
}