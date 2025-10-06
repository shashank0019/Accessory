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
            if (string.IsNullOrWhiteSpace(connectionString))
                throw new ArgumentException("Connection string cannot be null or empty.", nameof(connectionString));

            _dataAccess = new AccessoryDataAccess(connectionString);
        }


        // -------------------------------
        // Catalog
        // -------------------------------
        public List<AccessoryCatalogDto> GetAccessories() => _dataAccess.GetAccessories();

        public int AddNewAccessory(string accessoryName)
        {
            if (string.IsNullOrWhiteSpace(accessoryName))
                throw new ArgumentException("Accessory name cannot be empty.");

            return _dataAccess.AddNewAccessory(accessoryName);
        }

        // -------------------------------
        // Requests
        // -------------------------------
        public int InsertRequestMaster(int initiatorEmpId)
        {
            if (initiatorEmpId <= 0)
                throw new ArgumentException("Invalid initiator employee ID.");

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

        public void InsertAccessoryRequest(AccessoryRequestDetailDto accessory)
        {
            if (accessory == null)
                throw new ArgumentNullException(nameof(accessory), "Accessory data is required.");

            if (accessory.AccessoryWFMID <= 0)
                throw new ArgumentException("AccessoryWFMID (master ID) must be greater than zero.");

            if (accessory.AccessoryMID <= 0)
                throw new ArgumentException("AccessoryMID must be greater than zero.");

            if (accessory.AccessoryCount <= 0)
                throw new ArgumentException("Accessory count must be greater than zero.");

            _dataAccess.InsertAccessoryRequest(accessory);
        }

        public void InsertOrUpdateAccessoryQuantity(AccessoryQuantityDto accessory)
        {
            if (accessory == null)
                throw new ArgumentNullException(nameof(accessory), "Accessory data is required.");

            if (string.IsNullOrWhiteSpace(accessory.AccessoryName))
                throw new ArgumentException("Accessory name is required.");

            if (accessory.AccessoryCount <= 0)
                throw new ArgumentException("Accessory count must be greater than zero.");

            _dataAccess.InsertOrUpdateAccessoryQuantity(accessory);
        }

        public List<AccessoryRequestDetailResultDto> GetAccessoryDetails(int requestId)
        {
            if (requestId <= 0)
                throw new ArgumentException("Invalid request ID.");

            return _dataAccess.GetAccessoryDetails(requestId);
        }
    }
}
