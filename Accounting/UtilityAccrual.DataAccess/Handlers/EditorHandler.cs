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
    public class ApproverArrayHandler : SqlMapper.TypeHandler<IEnumerable<Editor>>
    {
        public override void SetValue(IDbDataParameter parameter, IEnumerable<Editor> value)
        {
            parameter.Value = JsonSerializer.Serialize(value);
        }

        public override IEnumerable<Editor> Parse(object value)
        {
            var approvers = JsonSerializer.Deserialize<IEnumerable<Editor>>(value.ToString());
            return approvers;
        }
    }
}
