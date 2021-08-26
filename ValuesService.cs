using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace swaggerAzFunc1
{
    public interface IValuesService
    {
        Task<ValueModel> Add(ValueModel model);

        Task<IEnumerable<ValueModel>> Get();

        Task<ValueModel> Get(Guid id);

        Task<ValueModel> New(string name, int value);
        Task<bool> Remove(Guid id);
    }

    public class ValuesService : IValuesService
    {
        private readonly ICollection<ValueModel> values;

        public ValuesService(ILogger<ValuesService> logger)
        {
            values = new List<ValueModel>()
            {
                new ValueModel() { Name = "one", Value = 1, },
                new ValueModel() { Name = "two", Value = 2, },
                new ValueModel() { Name = "three", Value = 3, },
            };
        }

        public Task<ValueModel> Add(ValueModel model)
        {
            values.Add(model);
            return Task.FromResult(model);
        }

        public Task<IEnumerable<ValueModel>> Get()
        {
            return Task.FromResult<IEnumerable<ValueModel>>(values);
        }

        public Task<ValueModel> Get(Guid id)
        {
            ValueModel model = values.SingleOrDefault(model => model.Id == id);
            return Task.FromResult(model);
        }

        public Task<ValueModel> New(string name, int value)
        {
            ValueModel model = new ValueModel() {
                Name = name,
                Value = value,
            };
            values.Add(model);
            return Task.FromResult(model);
        }

        public Task<bool> Remove(Guid id)
        {
            ValueModel model = values.SingleOrDefault(model => model.Id == id);
            if(model != null)
            {
                values.Remove(model);
            }
            return Task.FromResult(model != null);
        }
    }
}