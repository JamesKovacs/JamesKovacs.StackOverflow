using System;
using FluentNHibernate.Automapping;
using FluentNHibernate.Cfg;
using FluentNHibernate.Cfg.Db;
using FluentNHibernate.Conventions;
using FluentNHibernate.Conventions.AcceptanceCriteria;
using FluentNHibernate.Conventions.Inspections;
using FluentNHibernate.Conventions.Instances;
using HibernatingRhinos.Profiler.Appender.NHibernate;
using NHibernate.Tool.hbm2ddl;
using Utilities;

namespace FluentNhHacking {
    internal class Program {
        private static void Main() {
            NHibernateProfiler.Initialize();
            var cfg = Fluently.Configure()
                .Database(MsSqlConfiguration.MsSql2008.ConnectionString("Server=(local);Database=scratch;Integrated Security=SSPI").AdoNetBatchSize(500))
//                .Mappings(x => x.FluentMappings.AddFromAssemblyOf<Organism>())
                .Mappings(m => m.AutoMappings.Add(
                    AutoMap.Assemblies(typeof(Organism).Assembly)
                        .Conventions.AddAssembly(typeof(Program).Assembly)
                        .UseOverridesFromAssembly(typeof(Program).Assembly)))
                .BuildConfiguration();
            new SchemaExport(cfg).Execute(script: false, export: true, justDrop: false);
            var sessionFactory = cfg.BuildSessionFactory();

            using(var session = sessionFactory.OpenSession()) {
                using(var tx = session.BeginTransaction()) {
                    tx.Commit();
                }
            }
            using(var session = sessionFactory.OpenSession()) {
                using(var tx = session.BeginTransaction()) {
                    tx.Commit();
                }
            }

            Console.WriteLine("Press <ENTER> to exit...");
            Console.ReadLine();
        }
    }

//    public sealed class OrganismMappinig : ClassMap<Organism>
//    {
//        public OrganismMappinig()
//        {
//            Table("OrganismTable");
//            Id(x => x.Id).Column("OrganismId");
//        }
//    }
//
//    public sealed class AnimalMapping : SubclassMap<Animal>
//    {
//        public AnimalMapping()
//        {
//            Table("AnimalTable");
//            KeyColumn("AnimalId");
//        }
//    }  
    public class TableNameConvension : IClassConvention, IClassConventionAcceptance {
        public void Apply(IClassInstance instance) {
            instance.Table(instance.EntityType.Name + "Table");
        }

        public void Accept(IAcceptanceCriteria<IClassInspector> criteria) {
            criteria.Expect(x => x.TableName, Is.Not.Set);
        }
    }

    public class PrimaryKeyNameConvention : IIdConvention {
        public void Apply(IIdentityInstance instance) {
            instance.Column(instance.EntityType.Name + "Id");
        }
    }
    public class JoinedSubclassIdConvention : IJoinedSubclassConvention
    {
        public void Apply(IJoinedSubclassInstance instance) {
            instance.Table(instance.EntityType.Name + "Table");
            instance.Key.Column(instance.EntityType.Name + "Id");
        }
    }
//    public class AnimalOverride : IAutoMappingOverride<Animal>
//    {
//        public void Override(AutoMapping<Animal> mapping)
//        {
//            mapping.Table("AnimalTable");
//            mapping.Id(x => x.Id).Column("AnimalId");
//        }
//    }
}