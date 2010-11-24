using NHibernate.Event;
using NHibernate.Event.Default;

namespace NhHacking {
    public class CustomLoadListener : DefaultLoadEventListener {
        public override void OnLoad(LoadEvent @event, LoadType loadType) {
            if(@event.EntityId is MD5Hash) {
                var id = (MD5Hash) @event.EntityId;
                if(id == MD5Hash.Empty) {
                    @event.Result = new Referenced { Id = MD5Hash.Empty };
                    return;
                }
            }
            base.OnLoad(@event, loadType);
        }
    }

    public class CustomSaveOrUpdateListener : DefaultSaveOrUpdateEventListener {
        public override void OnSaveOrUpdate(SaveOrUpdateEvent @event) {
            var entity = @event.Entity as Referenced;
            if(entity != null && entity.Id == MD5Hash.Empty) {
                return;
            }
            base.OnSaveOrUpdate(@event);
        }
    }
}