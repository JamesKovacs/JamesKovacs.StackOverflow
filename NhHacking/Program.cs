using System;
using System.Data;
using System.Linq;
using HibernatingRhinos.Profiler.Appender.NHibernate;
using NHibernate;
using NHibernate.Cfg;
using NHibernate.Linq;
using NHibernate.Loader.Collection;
using NHibernate.SqlTypes;
using NHibernate.Tool.hbm2ddl;
using NHibernate.UserTypes;
using NHibernate.Util;

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
                var c = new Statement {Code = "FOO", Amount = 42.42};
                session.Save(c);
                tx.Commit();
                var query = from m0 in session.Query<Statement>()
                            where m0.Code == c.Code
                            group m0 by m0.Code
                            into g
                            select new {Expr1 = g.Sum(p => p.Amount)};
                var results = query.ToList();
                tx.Commit();
            }
            Console.WriteLine("Press <ENTER> to exit...");
            Console.ReadLine();
        }
    }

    public class Statement {
        public int Id { get; private set; }
        public string Code { get; set; }
        public double Amount { get; set; } 
    }

    public class Referenced : IEquatable<Referenced> {
        public virtual MD5Hash Id { get; set; }
        public virtual string Name { get; set; } // must NOT be null
        public virtual bool Equals(Referenced other) {
            return Id.Equals(other.Id);
        }
    }

    public class Referencer : IEquatable<Referencer> {
        public virtual MD5Hash Id { get; set; }
        public virtual Referenced Other { get; set; } // may be null
        public virtual bool Equals(Referencer other) {
            return Id.Equals(other.Id);
        }
    }

    public struct MD5Hash : IComparable, IComparable<MD5Hash>, IEquatable<MD5Hash> {
        private readonly byte[] contents;
        public static readonly MD5Hash Empty = new MD5Hash(new byte[]{0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0});

        public MD5Hash(byte[] bytes) {
            contents = bytes;
        }

        public int CompareTo(object obj) {
            return CompareTo((MD5Hash) obj);
        }

        public int CompareTo(MD5Hash other) {
            return contents.Zip(other.contents, (x, y) => x.CompareTo(y)).FirstOrDefault(x => x != 0);
        }

        public bool Equals(MD5Hash other) {
            return CompareTo(other) == 0;
        }

        public byte[] ToByteArray() {
            return contents;
        }

        public static bool operator==(MD5Hash first, MD5Hash second) {
            return first.Equals(second);
        }

        public static bool operator !=(MD5Hash first, MD5Hash second) {
            return !(first == second);
        }
    }

    internal class MD5HashType : IUserType {
        public SqlType[] SqlTypes {
            get { return new[] { new SqlType(DbType.Binary, 16) }; }
        }

        public Type ReturnedType {
            get { return typeof(MD5Hash); }
        }

        public new bool Equals(object x, object y) {
            return Object.Equals(x, y);
        }

        public int GetHashCode(object x) {
            return (null == x) ? 0 : x.GetHashCode();
        }

        public object NullSafeGet(IDataReader rs, string[] names, object owner) {
            var val = NHibernateUtil.Binary.NullSafeGet(rs, names[0]);
            return (null == val || DBNull.Value == val) ? MD5Hash.Empty : new MD5Hash((byte[])val);
        }

        public void NullSafeSet(IDbCommand cmd, object value, int index) {
            var val = (value == null || MD5Hash.Empty == ((MD5Hash)value)) ? null : ((MD5Hash)value).ToByteArray();
            NHibernateUtil.Binary.NullSafeSet(cmd, val, index);
        }

        public object DeepCopy(object value) {
            return value;
        }

        public bool IsMutable {
            get { return false; }
        }

        public object Replace(object original, object target, object owner) {
            return original;
        }

        public object Assemble(object cached, object owner) {
            return cached;
        }

        public object Disassemble(object value) {
            return value;
        }
    }
}