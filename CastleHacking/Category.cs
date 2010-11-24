using System.Collections.Generic;
using Castle.ActiveRecord;

namespace CastleHacking {
    [ActiveRecord]
    public class Category : ActiveRecordBase {
        private IList<Category> subcategories = new List<Category>();

        [PrimaryKey]
        private int Id { get; set; }

        [Property]
        public string Name { get; set; }

        [BelongsTo("parent_id")]
        public Category Parent { get; set; }

        [HasMany]
        public IList<Category> SubCategories {
            get { return subcategories; }
            set { subcategories = value; }
        }
    }
}