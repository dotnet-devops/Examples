using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using UtilityAccrual.Shared.Models;

namespace UtilityAccrual.DataAccess.Handlers
{
    public class ApprovalHandler : SqlMapper.TypeHandler<IEnumerable<AdjustmentRevision>>
    {
        public override void SetValue(IDbDataParameter parameter, IEnumerable<AdjustmentRevision> value)
        {
            parameter.Value = JsonSerializer.Serialize(value);
        }

        public override IEnumerable<AdjustmentRevision> Parse(object value)
        {
            return JsonSerializer.Deserialize<IEnumerable<AdjustmentRevision>>((string)value);
        }
    }
}
