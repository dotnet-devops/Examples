using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UtilityAccrual.Shared.Models;

namespace UtilityAccrual.DataAccess.Endpoints.Adjustments
{
    public static class AdjustmentRevisionEndpoint
    {
        // Redacted
        private static readonly string _select;
        #region Create

        public static async Task InsertRevision(this IDbConnection db, AdjustmentRevision revision)
        {
            string sql = "Redacted";
            await db.ExecuteAsync(sql, new 
            {
                /****************
                 *              *
                 *   Redacted   *
                 *              *
                 ****************/
            });
        }

        #endregion

        #region Read

        public static async Task<AdjustmentRevision> GetAdjustmentRevision(this IDbConnection db, int id)
        {
            string sql = string.Empty; // Redacted
            var output = await db.QueryFirstOrDefaultAsync<AdjustmentRevision>(sql, new { }); // Redacted
            return output;
        }

        public static async Task<int> GetAdjustmentRevisionCount(this IDbConnection db, int month, int year)
        {
            string sql = string.Empty; // Redacted
            var output = await db.QueryFirstAsync<int>(sql, new { }); // Redacted
            return output;
        }

        public static async Task<IEnumerable<AdjustmentRevision>> GetLastFiftyRevisions(this IDbConnection db)
        {
            string sql = string.Empty; // Redacted
            var output = await db.QueryAsync<AdjustmentRevision>(sql, new DynamicParameters());
            return output;
        }

        public static async Task<IEnumerable<AdjustmentRevision>> GetAdjustmentRevisions(this IDbConnection db)
        {
            string sql = string.Empty; // Redacted
            var output = await db.QueryAsync<AdjustmentRevision>(sql, new DynamicParameters());
            return output;
        }

        public static async Task<IEnumerable<AdjustmentRevision>> GetAdjustmentRevisionsByPeriod(this IDbConnection db, int month, int year)
        {
            string sql = string.Empty; // Redacted
            var output = await db.QueryAsync<AdjustmentRevision>(sql, new { }); // Redacted
            return output;
        }

        public static async Task<IEnumerable<AdjustmentRevision>> GetAdjustmentRevisionsByStatus(this IDbConnection db, int status)
        {
            string sql = string.Empty; // Redacted
            var output = await db.QueryAsync<AdjustmentRevision>(sql, new { }); // Redacted
            return output;
        }

        public static async Task<IEnumerable<AdjustmentRevision>> GetAdjustmentRevisionsByRowData(this IDbConnection db, int status, int utility, int budget)
        {
            string sql = string.Empty; // Redacted
            var output = await db.QueryAsync<AdjustmentRevision>(sql, new { }); // Redacted
            return output;
        }

        #endregion

        #region Update

        public static async Task UpdateRevision(this IDbConnection db, AdjustmentRevision revision)
        {
            string sql = "Redacted";
            await db.ExecuteAsync(sql, new
            {
                Redacted
            });
        }

        public static async Task UpdateRevisionStatus(this IDbConnection db, int revision, int status)
        {
            string sql = "Redacted";
            await db.ExecuteAsync(sql, new { Redacted });
        }

        #endregion

        #region Delete

        public static async Task DeleteRevision(this IDbConnection db, int id)
        { 
            string sql = "Redacted";
            await db.ExecuteAsync(sql, new { Redacted });
        }

        #endregion
    }
}
