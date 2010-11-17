using System;
using FluentNHibernate.Cfg;
using FluentNHibernate.Cfg.Db;
using FluentNHibernate.Conventions.Helpers;
using HibernatingRhinos.Profiler.Appender.NHibernate;
using NHibernate.Tool.hbm2ddl;

namespace FluentNhHacking {
    internal class Program {
        private static void Main(string[] args) {
            NHibernateProfiler.Initialize();
            var cfg = Fluently.Configure()
                              .Database(MsSqlConfiguration.MsSql2008.ConnectionString("Server=(local);Database=scratch;Integrated Security=SSPI").UseOuterJoin())
                              .Mappings(x => x.FluentMappings.AddFromAssemblyOf<Foo>()
                                              .Conventions.Add(DefaultLazy.Always()))
                              .BuildConfiguration();
//            new SchemaExport(cfg).Execute(script: false, export: true, justDrop: false);
            var sessionFactory = cfg.BuildSessionFactory();
            using(var session = sessionFactory.OpenSession()) {
                using(var tx = session.BeginTransaction()) {
                    var foo1 = new Foo {Name = "Foo", Bars = new[] {new Bar {Name = "Bar"}}};
                    session.Save(foo1);

                    var allFoos = session.CreateCriteria<Foo>().List<Foo>();
                    foreach(var foo in allFoos) {
                        Console.WriteLine("{0} - {1}", foo.Id, foo.Name);
                    }
                    tx.Commit();
                }
            }
            Console.WriteLine("Press <ENTER> to exit...");
            Console.ReadLine();
        }
    }
}