using System;
using System.Collections.Generic;
using Microsoft.Extensions.Caching.Memory;

using esdc_simulation_base.Src.Storage;

namespace maternity_benefits.Storage.Cache
{
    public class MaternityBenefitsPersonCacheStore : IStorePersons<MaternityBenefitsPerson>
    {
        private readonly static string cacheKeyBase = "maternity_benefits";
        private readonly static string cacheKeyPersons = $"{cacheKeyBase}_persons";

        private readonly IMemoryCache _cache;

        public MaternityBenefitsPersonCacheStore(IMemoryCache cache) {
            _cache = cache;
            var curr = GetAllPersons();
            if (curr == null) {
                _cache.Set(cacheKeyPersons, new List<MaternityBenefitsPerson>());
            }
        }

        public IEnumerable<MaternityBenefitsPerson> GetAllPersons() {
            return (List<MaternityBenefitsPerson>)_cache.Get(cacheKeyPersons);
        }

        public void AddPersons(IEnumerable<MaternityBenefitsPerson> persons) {
            List<MaternityBenefitsPerson> currList = (List<MaternityBenefitsPerson>)_cache.Get(cacheKeyPersons);
            currList.AddRange(persons);
            _cache.Set(cacheKeyPersons, currList);
        }

        public void Clear() {
            _cache.Set(cacheKeyPersons, new List<MaternityBenefitsPerson>());
        }

        public void DeletePerson(Guid id) {
            // TODO: Implement this if needed?
            throw new NotImplementedException();
        }
    }
}
