using System;
using HibernatingRhinos.Profiler.Appender.NHibernate;
using NHibernate.Cfg;
using NHibernate.Tool.hbm2ddl;

namespace NhHacking {
    internal class Program {
        private static void Main() {
            NHibernateProfiler.Initialize();
            var cfg = new Configuration();
            cfg.Configure();
            new SchemaExport(cfg).Execute(script: false, export: true, justDrop: false);
            var sessionFactory = cfg.BuildSessionFactory();
            using(var session = sessionFactory.OpenSession())
            using(var tx = session.BeginTransaction()) {
                var alert = new Alert {Name = "Test", 
                    Messages = {
                                   {CxChannel.Message, "ABCD"},
                                   {CxChannel.Email, "foo@example.com"}
                               }};
                session.Save(alert);
                tx.Commit();
            }
            Console.WriteLine("Press <ENTER> to exit...");
            Console.ReadLine();
        }
    }
}