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
    public class UtilityHandler : SqlMapper.TypeHandler<IEnumerable<Utility>>
    {
        public override void SetValue(IDbDataParameter parameter, IEnumerable<Utility> value)
        {
            parameter.Value = JsonSerializer.Serialize(value);
        }

        public override IEnumerable<Utility> Parse(object value)
        {
            return JsonSerializer.Deserialize<IEnumerable<Utility>>((string)value);
        }
    }
}
