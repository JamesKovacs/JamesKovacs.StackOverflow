using FluentNHibernate.Mapping;

namespace FluentNhHacking {
    public sealed class FooMap : ClassMap<Foo> {
        public FooMap() {
            Id(x => x.Id).GeneratedBy.GuidComb();
            Map(x => x.Name);
            Component(x => x.FullName, m => {
                                           m.Map(x => x.Salutation);
                                           m.Map(x => x.Forename);
                                           m.Map(x => x.Surname);
                                       });
            HasMany(x => x.Bars)
                .Cascade.AllDeleteOrphan()
                .Not.LazyLoad()
                .Fetch.Join();
        }
    }
}