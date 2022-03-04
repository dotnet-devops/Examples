using Dapper;
using System.Collections.Generic;
using System.Data;
using System.Text.Json;
using UtilityAccrual.Shared.Models;

namespace UtilityAccrual.DataAccess.Handlers
{
    public class AdjustmentHandler : SqlMapper.TypeHandler<IEnumerable<AdjustmentModel>>
    {
        public override void SetValue(IDbDataParameter parameter, IEnumerable<AdjustmentModel> value)
        {
            parameter.Value = JsonSerializer.Serialize(value);
        }

        public override IEnumerable<AdjustmentModel> Parse(object value)
        {
            return JsonSerializer.Deserialize<IEnumerable<AdjustmentModel>>((string)value);
        }
    }
}
