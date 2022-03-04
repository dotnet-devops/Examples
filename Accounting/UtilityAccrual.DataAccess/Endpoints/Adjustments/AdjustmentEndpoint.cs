using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using Dapper;
using UtilityAccrual.DataAccess.Handlers;
using UtilityAccrual.Shared.Definitions;
using UtilityAccrual.Shared.Models;

namespace UtilityAccrual.DataAccess.Endpoints.Adjustments
{
    public static class AdjustmentEndpoint
    {
        private static readonly string _select = string.Empty; // Redacted

        #region Create

        public static async Task InsertAdjustment(this IDbConnection db, AdjustmentModel adjustment)
        {
            string sql = "Redacted";
            await db.ExecuteAsync(sql, new
            {
                // Redacted
            });
        }

        #endregion

        #region Read

        public static async Task<AdjustmentModel> GetAdjustment(this IDbConnection db, int id)
        {
            string sql = "Redacted";
                            
            var output = await db.QueryFirstOrDefaultAsync<AdjustmentModel>(sql, new { }); // Redacted
            return output;
        }

        public static async Task<IEnumerable<int>> GetAdjustmentMonthsByYear(this IDbConnection db, int year)
        {
            string sql = string.Empty; // Redacted

            var output = await db.QueryAsync<int>(sql, new { }); // Redacted
            return output;
        }

        public static async Task<IEnumerable<int>> GetAdjustmentYears(this IDbConnection db)
        {
            string sql = string.Empty; // Redacted

            var output = await db.QueryAsync<int>(sql, new DynamicParameters());
            return output;
        }

        public static async Task<IEnumerable<AdjustmentModel>> GetAdjustmentsByPeriod(this IDbConnection db, int month, int year)
        {
            string sql = string.Empty;
            var output = await db.QueryAsync<AdjustmentModel>(sql, new { }); // Redacted
            return output;
        }

        public static async Task<IEnumerable<AdjustmentModel>> GetAdjustments(this IDbConnection db)
        {
            string sql = string.Empty; 
            var output = await db.QueryAsync<AdjustmentModel>(sql, new DynamicParameters()); // Redacted
            return output;
        }

        public static async Task<AdjustmentModel> GetLatestAdjustment(this IDbConnection db, int utility, int month, int year)
        {
            string sql = "Redacted";

            var output = await db.QueryFirstOrDefaultAsync<AdjustmentModel>(sql, new { Redacted = "Redacted" });
            return output;
        }

        #endregion

        #region Update

        public static async Task UpdateAdjustment(this IDbConnection db, AdjustmentModel adjustment)
        {
            string sql = string.Empty;
            await db.ExecuteAsync(sql, new
            {
                // Redacted
            });
        }

        #endregion

        #region Delete
        public static async Task DeleteAdjustment(this IDbConnection db, int id)
        { 
            string sql = "DELETE FROM redacted " +
                         "WHERE redacted = @redacted";
            await db.ExecuteAsync(sql, new { }); // Redacted
        }
        #endregion
    }
}
