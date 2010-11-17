using FluentNHibernate.Mapping;

namespace FluentNhHacking {
    public sealed class FooMap : ClassMap<Foo> {
        public FooMap() {
            Id(x => x.Id).GeneratedBy.GuidComb();
            Map(x => x.Name);
            HasManyToMany(x => x.Bars)
                .Cascade.AllDeleteOrphan()
                .Not.LazyLoad()
                .Fetch.Join();
        }
    }
}