using System;
using System.Collections.Generic;
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

        public string ConnectionString => _connectionString;

        // -------------------------------
        // Catalog Methods
        // -------------------------------
        public List<AccessoryCatalogDto> GetAccessories()
        {
            var list = new List<AccessoryCatalogDto>();
            using var conn = new SqlConnection(_connectionString);
            using var cmd = new SqlCommand("ITHW_AMT_AccessoryCreation_GetAccessories", conn)
            {
                CommandType = CommandType.StoredProcedure
            };
            conn.Open();
            using var reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                list.Add(new AccessoryCatalogDto
                {
                    AccessoryWFMID = reader.GetInt32(reader.GetOrdinal("AccessoryWFMID")),
                    AccessoryName = reader["AccessoryName"].ToString(),
                    AccessoryType = reader["AccessoryType"].ToString()
                });
            }
            return list;
        }

        public int InsertAccessory(AccessoryCreateDto model)
        {
            using var conn = new SqlConnection(_connectionString);
            using var cmd = new SqlCommand("IJTM_AML_AccessoryCreation_AddNewAccessory", conn)
            {
                CommandType = CommandType.StoredProcedure
            };
            cmd.Parameters.AddWithValue("@AccessoryName", model.AccessoryName);
            cmd.Parameters.AddWithValue("@AccessoryType", model.AccessoryType ?? (object)DBNull.Value);
            cmd.Parameters.AddWithValue("@Make", model.Make);
            cmd.Parameters.AddWithValue("@AcqDate", model.AcqDate ?? (object)DBNull.Value);
            cmd.Parameters.AddWithValue("@AccessoryCount", model.AccessoryCount);
            cmd.Parameters.AddWithValue("@ProductStatus", model.ProductStatus ?? (object)DBNull.Value);
            cmd.Parameters.AddWithValue("@Remarks", model.Remarks);

            conn.Open();
            var result = cmd.ExecuteScalar();
            return result == null ? 0 : Convert.ToInt32(result);
        }

        // -------------------------------
        // Request Methods
        // -------------------------------
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

        public void InsertAccessoryRequest(int masterId, AccessoryRequestDetailDto acc, SqlConnection conn, SqlTransaction trans)
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
            cmd.Parameters.AddWithValue("@AcqDate", acc.AcqDate ?? (object)DBNull.Value);
            cmd.Parameters.AddWithValue("@AccessoryCount", acc.AccessoryCount);
            cmd.Parameters.AddWithValue("@ProductStatus", acc.ProductStatus ?? (object)DBNull.Value);
            cmd.Parameters.AddWithValue("@Remarks", acc.Remarks ?? (object)DBNull.Value);

            cmd.ExecuteNonQuery();
        }

        public void InsertUpdateInventory(int masterId, AccessoryInventoryDto acc, SqlConnection conn, SqlTransaction trans)
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

        public List<AccessoryRequestDetailResultDto> GetAccessoryDetails(int requestId)
        {
            var list = new List<AccessoryRequestDetailResultDto>();
            using var conn = new SqlConnection(_connectionString);
            using var cmd = new SqlCommand("ITHW_AMT_AccessoryCreation_GetRequestedAccessoryDetails", conn)
            {
                CommandType = CommandType.StoredProcedure
            };
            cmd.Parameters.AddWithValue("@AccessoryWFMID", requestId);
            conn.Open();
            using var reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                list.Add(new AccessoryRequestDetailResultDto
                {
                    AccessoryWFMID = Convert.ToInt32(reader["AccessoryWFMID"]),
                    AccessoryName = reader["AccessoryName"].ToString(),
                    AccessoryCount = Convert.ToInt32(reader["AccessoryCount"]),
                    ProductStatus = reader["ProductStatus"].ToString(),
                    Remarks = reader["Remarks"].ToString()
                });
            }
            return list;
        }
    }
}
