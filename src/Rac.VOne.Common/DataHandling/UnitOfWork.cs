using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rac.VOne.Common.DataHandling
{
    public class UnitOfWork<TModel>
        where TModel : class, new()
    {
        public List<TModel> Unchanged { get; private set; } = new List<TModel>();

        public List<TModel> New { get; private set; } = new List<TModel>();

        public List<TModel> Dirty { get; private set; } = new List<TModel>();

        public List<TModel> Removed { get; private set; } = new List<TModel>();

        public UnitOfWork()
        {
        }
    }

    public class UnitOfWorkWithReference<TModel, TIdentity> : UnitOfWork<TModel>
        where TModel : class, new()
    {
        public Dictionary<TIdentity, TModel> All { get; private set; }
        private Func<TModel, TIdentity> createIdentity = null;

        public UnitOfWorkWithReference(
                IEnumerable<TModel> models,
                Func<TModel, TIdentity> identitySelector)
        {
            createIdentity = identitySelector;
            All = models.ToDictionary(identitySelector);
        }

        public void RegisterNew(TModel e)
        {
            TIdentity id = createIdentity(e);
            if (!All.ContainsKey(id))
            {
                All.Add(id, e);
            }

            if (!New.Contains(e))
            {
                New.Add(e);
            }
        }

        public void RegisterDirty(TModel e)
        {
            TIdentity id = createIdentity(e);
            if (!All.ContainsKey(id)) return;

            if (!Removed.Contains(e) && !Dirty.Contains(e))
            {
                Dirty.Add(e);
            }
        }

        //public void RegisterClean(TModel e)
        //{
        //    Dirty.Remove(e);
        //}

        public TModel RegisterRemoved(TModel e)
        {
            if (New.Remove(e))
            {
                return e;
            }

            Dirty.Remove(e);
            if (!Removed.Contains(e))
            {
                Removed.Add(e);
            }

            return e;
        }

        public TIdentity GetIdentity(TModel model)
        {
            return createIdentity(model);
        }

        public TModel GetLocal(TIdentity identity)
        {
            if (All.ContainsKey(identity))
            {
                return All[identity];
            }
            return null;
        }
    }
}
