using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Rac.VOne.Common.DataHandling;

namespace Rac.VOne.Import
{
    public enum ImportMethod
    {
        /// <summary> 上書 </summary>
        Replace,
        /// <summary> 追加 </summary>
        InsertOnly,
        /// <summary> 更新 </summary>
        InsertAndUpdate,
    }

    public delegate bool ImportMethodDelegate<TModel, TIdentity>(
                UnitOfWorkWithReference<TModel, TIdentity> unitOfWork,
                ImportWorker<TModel> worker)
            where TModel : class, new();

    public static class ImportMethodExtension
    {
        public static bool ValidateDuplicated<TModel, TIdentity>(this ImportMethod method,
                UnitOfWorkWithReference<TModel, TIdentity> unitOfWork,
                ImportWorker<TModel> worker)
            where TModel : class, new()
        {
            if (method != ImportMethod.InsertOnly) return true;

            var valid = true;
            foreach (var pair in worker.Models)
            {
                TModel product = pair.Value;
                TIdentity importedUnique = unitOfWork.GetIdentity(product);

                if (unitOfWork.All.ContainsKey(importedUnique))
                {
                    valid = false;
                    worker.AddKeyDuplicatedError(pair.Key);
                }
            }
            return valid;
        }

        public static bool Import<TModel, TIdentity>(this ImportMethod method,
                UnitOfWorkWithReference<TModel, TIdentity> unitOfWork,
                ImportWorker<TModel> worker)
            where TModel : class, new()
        {
            switch (method)
            {
                case ImportMethod.Replace: return Replace(unitOfWork, worker);
                case ImportMethod.InsertOnly: return InsertOnly(unitOfWork, worker);
                case ImportMethod.InsertAndUpdate: return InsertAndUpdate(unitOfWork, worker);
                default: throw new InvalidOperationException();
            }
        }

        private static bool Replace<TModel, TIdentity>(
                UnitOfWorkWithReference<TModel, TIdentity> unitOfWork,
                ImportWorker<TModel> worker)
            where TModel : class, new()
        {
            foreach (var pair in worker.Models)
            {
                TModel product = pair.Value;
                TIdentity importedUnique = unitOfWork.GetIdentity(product);

                TModel target = null;
                if (unitOfWork.All.TryGetValue(importedUnique, out target))
                {
                    Model.CopyTo(product, target, false);
                    unitOfWork.RegisterDirty(target);
                }
                else
                {
                    unitOfWork.RegisterNew(product);
                }
            }
            IEnumerable<TModel> newAndDirty = unitOfWork.Dirty.Concat(unitOfWork.New);
            Model.SetUpdateBy(newAndDirty, worker.LoginUserId);
            foreach (TModel model in unitOfWork.All.Values.Except(newAndDirty))
            {
                unitOfWork.RegisterRemoved(model);
            }

            if (worker.RecordCount != worker.Models.Count)
            {
                worker.Models.Clear(); // 全件取込不可
            }

            return worker.Models.Any();
        }

        private static bool InsertOnly<TModel, TIdentity>(
                UnitOfWorkWithReference<TModel, TIdentity> unitOfWork,
                ImportWorker<TModel> worker)
            where TModel : class, new()
        {
            foreach (var pair in worker.Models)
            {
                TModel product = pair.Value;
                TIdentity importedUnique = unitOfWork.GetIdentity(product);

                TModel target = null;
                if (!unitOfWork.All.TryGetValue(importedUnique, out target))
                {
                    unitOfWork.RegisterNew(product);
                }
            }
            Model.SetUpdateBy(unitOfWork.New, worker.LoginUserId);

            return worker.Models.Any();
        }

        private static bool InsertAndUpdate<TModel, TIdentity>(
                UnitOfWorkWithReference<TModel, TIdentity> unitOfWork,
                ImportWorker<TModel> worker)
            where TModel : class, new()
        {
            foreach (var pair in worker.Models)
            {
                TModel product = pair.Value;
                TIdentity importedUnique = unitOfWork.GetIdentity(product);

                TModel target = null;
                if (unitOfWork.All.TryGetValue(importedUnique, out target))
                {
                    Model.CopyTo(product, target, false);
                    unitOfWork.RegisterDirty(target);
                }
                else
                {
                    unitOfWork.RegisterNew(product);
                }
            }
            Model.SetUpdateBy(unitOfWork.New.Concat(unitOfWork.Dirty), worker.LoginUserId);

            return worker.Models.Any();
        }
    }
}
