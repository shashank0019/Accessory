using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using AccessoryCreation.DataAccess;
using AccessoryCreation.Models;

namespace AccessoryCreation.BusinessLogic
{
    public class AccessoryBusinessLogic
    {
        private readonly AccessoryDataAccess _dataAccess;

        public AccessoryBusinessLogic(string connectionString)
        {
            _dataAccess = new AccessoryDataAccess(connectionString);
        }

        // -------------------------------
        // Catalog
        // -------------------------------
        public List<AccessoryCatalogDto> GetAccessories()
        {
            return _dataAccess.GetAccessories();
        }

        public int AddAccessory(AccessoryCreateDto model)
        {
            if (model == null) throw new ArgumentNullException(nameof(model));
            if (string.IsNullOrWhiteSpace(model.AccessoryName)) throw new Exception("AccessoryName required");
            if (string.IsNullOrWhiteSpace(model.Make)) throw new Exception("Make required");
            if (model.AccessoryCount <= 0) throw new Exception("AccessoryCount > 0 required");
            if (string.IsNullOrWhiteSpace(model.Remarks)) throw new Exception("Remarks required");

            return _dataAccess.InsertAccessory(model);
        }

        // -------------------------------
        // Request
        // -------------------------------
        public int InsertRequestMaster(int initiatorEmpId)
        {
            using var conn = new SqlConnection(_dataAccess.ConnectionString);
            conn.Open();
            using var trans = conn.BeginTransaction();
            try
            {
                int masterId = _dataAccess.InsertAccessoryMaster(initiatorEmpId, conn, trans);
                trans.Commit();
                return masterId;
            }
            catch
            {
                trans.Rollback();
                throw;
            }
        }

        public void InsertRequestDetails(int masterId, List<AccessoryRequestDetailDto> accessories)
        {
            if (accessories == null || accessories.Count == 0)
                throw new ArgumentException("Accessories cannot be empty");

            using var conn = new SqlConnection(_dataAccess.ConnectionString);
            conn.Open();
            using var trans = conn.BeginTransaction();
            try
            {
                foreach (var acc in accessories)
                {
                    _dataAccess.InsertAccessoryRequest(masterId, acc, conn, trans);
                }
                trans.Commit();
            }
            catch
            {
                trans.Rollback();
                throw;
            }
        }

        public void InsertIntoInventory(int masterId, List<AccessoryInventoryDto> accessories)
        {
            if (accessories == null || accessories.Count == 0)
                throw new ArgumentException("Accessories cannot be empty");

            using var conn = new SqlConnection(_dataAccess.ConnectionString);
            conn.Open();
            using var trans = conn.BeginTransaction();
            try
            {
                foreach (var acc in accessories)
                {
                    _dataAccess.InsertUpdateInventory(masterId, acc, conn, trans);
                }
                trans.Commit();
            }
            catch
            {
                trans.Rollback();
                throw;
            }
        }

        public List<AccessoryRequestDetailResultDto> GetAccessoryDetails(int requestId)
        {
            return _dataAccess.GetAccessoryDetails(requestId);
        }
    }
}
