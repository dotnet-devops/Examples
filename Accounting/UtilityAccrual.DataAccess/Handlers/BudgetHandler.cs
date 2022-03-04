using Dapper;
using System.Collections.Generic;
using System.Data;
using System.Text.Json;
using UtilityAccrual.Shared.Models;

namespace UtilityAccrual.DataAccess.Handlers
{
    public class BudgetHandler : SqlMapper.TypeHandler<IEnumerable<Budget>>
    {
        public override void SetValue(IDbDataParameter parameter, IEnumerable<Budget> value)
        {
            parameter.Value = JsonSerializer.Serialize(value);
        }

        public override IEnumerable<Budget> Parse(object value)
        {
            return JsonSerializer.Deserialize<IEnumerable<Budget>>((string)value);
        }
    }
}
