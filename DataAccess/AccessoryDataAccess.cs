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
                    AccessoryWFMID = Convert.ToInt32(reader["AccessoryWFMID"]),
                    AccessoryName = reader["AccessoryName"]?.ToString(),
                    AccessoryType = reader["AccessoryType"]?.ToString()
                });
            }
            return list;
        }

        public int AddNewAccessory(string accessoryName)
        {
            if (string.IsNullOrWhiteSpace(accessoryName))
                throw new ArgumentException("Accessory name cannot be empty.", nameof(accessoryName));

            using var conn = new SqlConnection(_connectionString);
            using var cmd = new SqlCommand("ITHW_AMT_AccessoryCreation_AddNewAccessory", conn)
            {
                CommandType = CommandType.StoredProcedure
            };

            cmd.Parameters.AddWithValue("@AccessoryName", accessoryName);

            conn.Open();
            var result = cmd.ExecuteScalar();
            return Convert.ToInt32(result);
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

        public void InsertAccessoryRequest(AccessoryRequestDetailDto acc)
        {
            if (acc == null)
                throw new ArgumentNullException(nameof(acc));

            using var conn = new SqlConnection(_connectionString);
            using var cmd = new SqlCommand("ITHW_AMT_AccessoryCreation_Insert", conn)
            {
                CommandType = CommandType.StoredProcedure
            };

            cmd.Parameters.AddWithValue("@AccessoryWFMID", acc.AccessoryWFMID);
            cmd.Parameters.AddWithValue("@AccessoryMID", acc.AccessoryMID);
            cmd.Parameters.AddWithValue("@Make", (object?)acc.Make ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@AcqDate", (object?)acc.AcqDate ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@AccessoryCount", acc.AccessoryCount);
            cmd.Parameters.AddWithValue("@ProductStatus", (object?)acc.ProductStatus ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@Remarks", (object?)acc.Remarks ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@AccessoryType", (object?)acc.AccessoryType ?? DBNull.Value);

            conn.Open();
            cmd.ExecuteNonQuery();
        }

        public void InsertOrUpdateAccessoryQuantity(AccessoryQuantityDto accessory)
        {
            if (accessory == null)
                throw new ArgumentNullException(nameof(accessory));

            using var conn = new SqlConnection(_connectionString);
            using var cmd = new SqlCommand("ITHW_AMT_AccessoryQuantity_InsertUpdate", conn)
            {
                CommandType = CommandType.StoredProcedure
            };

            cmd.Parameters.AddWithValue("@AccessoryMID", accessory.AccessoryMID);
            cmd.Parameters.AddWithValue("@AccessoryType", accessory.AccessoryType ?? (object)DBNull.Value);
            cmd.Parameters.AddWithValue("@AccessoryName", accessory.AccessoryName ?? (object)DBNull.Value);
            cmd.Parameters.AddWithValue("@ProductStatus", accessory.ProductStatus ?? (object)DBNull.Value);
            cmd.Parameters.AddWithValue("@AccessoryCount", accessory.AccessoryCount);

            conn.Open();
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
                    AccessoryName = reader["AccessoryName"]?.ToString(),
                    AccessoryCount = reader["AccessoryCount"] != DBNull.Value ? Convert.ToInt32(reader["AccessoryCount"]) : 0,
                    ProductStatus = reader["ProductStatus"]?.ToString(),
                    Remarks = reader["Remarks"]?.ToString()
                });
            }
            return list;
        }
    }
}
