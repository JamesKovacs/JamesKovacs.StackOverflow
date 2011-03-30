using System;
using System.Diagnostics;
using fm.web;
using HibernatingRhinos.Profiler.Appender.NHibernate;
using NHibernate.Cfg;
using NHibernate.Tool.hbm2ddl;
using Utilities;

namespace NhHacking {
    internal class Program {
        private static void Main() {
            NHibernateProfiler.Initialize();
            var stopwatch = new Stopwatch();
            stopwatch.Start();
            var cfg = new Configuration();
            cfg.Configure();
            new SchemaExport(cfg).Execute(script: false, export: true, justDrop: false);
            var sessionFactory = cfg.BuildSessionFactory();
            stopwatch.Stop();
            Console.WriteLine("Startup time was " + stopwatch.ElapsedMilliseconds + "ms");
            using(var session = sessionFactory.OpenSession())
            using(var tx = session.BeginTransaction()) {
                tx.Commit();
            }

            using(var session = sessionFactory.OpenSession())
            using (var tx = session.BeginTransaction()) {
                var user = new User {Firstname = "Sam", Lastname = "Spade", Username = "sspade", Usertype = new UserType {Title = "Admin"}};
                session.Save(user);
                tx.Commit();
            }

            Console.WriteLine("Press <ENTER> to exit...");
            Console.ReadLine();
        }
    }
}