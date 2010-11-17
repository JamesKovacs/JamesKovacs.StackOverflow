using FluentNHibernate.Mapping;

namespace FluentNhHacking {
    public sealed class BarMap : ClassMap<Bar> {
        public BarMap() {
            Id(x => x.Id).GeneratedBy.GuidComb();
            Map(x => x.Name);
        }
    }
}