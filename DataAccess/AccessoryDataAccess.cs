using System;
using System.Data;
using System.Data.SqlClient;
using AccessoryCreation.Models;

namespace AccessoryCreation.DataAccess
{
    public class AccessoryDataAccess
    {
        private readonly string _connectionString;

        public AccessoryDataAccess(string connectionString)
        {
            _connectionString = connectionString ?? throw new ArgumentNullException(nameof(connectionString));
        }

        // 🔹 Expose connection string for BLL if needed
        public string ConnectionString => _connectionString;

        #region Catalog Methods
        public DataSet GetAccessories()
        {
            var ds = new DataSet();
            using var conn = new SqlConnection(_connectionString);
            using var cmd = new SqlCommand("ITHW_AMT_AccessoryCreation_GetAccessories", conn)
            {
                CommandType = CommandType.StoredProcedure
            };
            using var da = new SqlDataAdapter(cmd);
            da.Fill(ds);
            return ds;
        }

        public int InsertAccessory(Accessory model)
        {
            using var conn = new SqlConnection(_connectionString);
            using var cmd = new SqlCommand("IJTM_AML_AccessoryCreation_AddNewAccessory", conn)
            {
                CommandType = CommandType.StoredProcedure
            };
            cmd.Parameters.AddWithValue("@AccessoryName", model.AccessoryName ?? (object)DBNull.Value);
            cmd.Parameters.AddWithValue("@AccessoryType", model.AccessoryType ?? (object)DBNull.Value);
            cmd.Parameters.AddWithValue("@Make", model.Make ?? (object)DBNull.Value);
            cmd.Parameters.AddWithValue("@AcqDate", model.AcqDate == default(DateTime) ? (object)DBNull.Value : model.AcqDate);
            cmd.Parameters.AddWithValue("@AccessoryCount", model.AccessoryCount);
            cmd.Parameters.AddWithValue("@ProductStatus", model.ProductStatus ?? (object)DBNull.Value);
            cmd.Parameters.AddWithValue("@Remarks", model.Remarks ?? (object)DBNull.Value);

            conn.Open();
            var result = cmd.ExecuteScalar();
            return result == null ? 0 : Convert.ToInt32(result);
        }
        #endregion

        #region Request Methods
        public int InsertAccessoryMaster(int empId, SqlConnection conn, SqlTransaction trans)
        {
            using var cmd = new SqlCommand("ITHW_AMT_AccessoryCreation_Master_Insert", conn, trans)
            {
                CommandType = CommandType.StoredProcedure
            };
            cmd.Parameters.AddWithValue("@MEmpId", empId);
            var result = cmd.ExecuteScalar();
            return result == null ? 0 : Convert.ToInt32(result);
        }

        public void InsertAccessoryRequest(int masterId, Accessory acc, SqlConnection conn, SqlTransaction trans)
        {
            using var cmd = new SqlCommand("ITHW_AMT_AccessoryCreation_Insert", conn, trans)
            {
                CommandType = CommandType.StoredProcedure
            };
            cmd.Parameters.AddWithValue("@AccessoryWFMID", masterId);
            cmd.Parameters.AddWithValue("@AccessoryMID", acc.AccessoryMID);
            cmd.Parameters.AddWithValue("@AccessoryType", acc.AccessoryType ?? (object)DBNull.Value);
            cmd.Parameters.AddWithValue("@AccessoryName", acc.AccessoryName ?? (object)DBNull.Value);
            cmd.Parameters.AddWithValue("@Make", acc.Make ?? (object)DBNull.Value);
            cmd.Parameters.AddWithValue("@AcqDate", acc.AcqDate == default(DateTime) ? (object)DBNull.Value : acc.AcqDate);
            cmd.Parameters.AddWithValue("@AccessoryCount", acc.AccessoryCount);
            cmd.Parameters.AddWithValue("@ProductStatus", acc.ProductStatus ?? (object)DBNull.Value);
            cmd.Parameters.AddWithValue("@Remarks", acc.Remarks ?? (object)DBNull.Value);

            cmd.ExecuteNonQuery();
        }

        public void InsertUpdateInventory(int masterId, Accessory acc, SqlConnection conn, SqlTransaction trans)
        {
            using var cmd = new SqlCommand("ITHW_AMT_AccessoryQuantity_InsertUpdate", conn, trans)
            {
                CommandType = CommandType.StoredProcedure
            };
            cmd.Parameters.AddWithValue("@AccessoryWFMID", masterId);
            cmd.Parameters.AddWithValue("@AccessoryMID", acc.AccessoryMID);
            cmd.Parameters.AddWithValue("@AccessoryType", acc.AccessoryType ?? (object)DBNull.Value);
            cmd.Parameters.AddWithValue("@AccessoryName", acc.AccessoryName ?? (object)DBNull.Value);
            cmd.Parameters.AddWithValue("@ProductStatus", acc.ProductStatus ?? (object)DBNull.Value);
            cmd.Parameters.AddWithValue("@AccessoryCount", acc.AccessoryCount);

            cmd.ExecuteNonQuery();
        }

        public DataSet GetAccessoryDetails(int requestId)
        {
            var ds = new DataSet();
            using var conn = new SqlConnection(_connectionString);
            using var cmd = new SqlCommand("ITHW_AMT_AccessoryCreation_GetRequestedAccessoryDetails", conn)
            {
                CommandType = CommandType.StoredProcedure
            };
            cmd.Parameters.AddWithValue("@AccessoryWFMID", requestId);
            using var da = new SqlDataAdapter(cmd);
            da.Fill(ds);
            return ds;
        }
        #endregion
    }
}
