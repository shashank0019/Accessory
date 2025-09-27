using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
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

        #region Catalog Methods
        public List<Accessory> GetAccessories()
        {
            var list = new List<Accessory>();
            var ds = _dataAccess.GetAccessories();
            if (ds.Tables.Count > 0)
            {
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    list.Add(new Accessory
                    {
                        AccessoryWFMID = Convert.ToInt32(dr["AccessoryWFMID"]),
                        AccessoryName = dr["AccessoryName"].ToString(),
                        AccessoryType = dr["AccessoryType"].ToString()
                    });
                }
            }
            return list;
        }

        public int AddAccessory(AccessoryModel model)
        {
            if (model == null) throw new ArgumentNullException(nameof(model));
            if (string.IsNullOrWhiteSpace(model.AccessoryName)) throw new Exception("AccessoryName required");
            if (string.IsNullOrWhiteSpace(model.Make)) throw new Exception("Make required");
            if (model.AccessoryCount <= 0) throw new Exception("AccessoryCount > 0 required");
            if (string.IsNullOrWhiteSpace(model.Remarks)) throw new Exception("Remarks required");

            var acc = new Accessory
            {
                AccessoryName = model.AccessoryName,
                Make = model.Make,
                AccessoryType = model.AccessoryType,
                AcqDate = model.AcqDate,
                AccessoryCount = model.AccessoryCount,
                ProductStatus = model.ProductStatus,
                Remarks = model.Remarks
            };

            return _dataAccess.InsertAccessory(acc);
        }
        #endregion

        #region Request Methods
        // Insert master request
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

        // Insert accessory details for master
        public void InsertRequestDetails(int masterId, List<Accessory> accessories)
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

        // Approve request and insert into inventory
        public void InsertIntoInventory(int masterId, List<Accessory> accessories)
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

        // Fetch accessory details by request
        public List<Accessory> GetAccessoryDetails(int requestId)
        {
            var list = new List<Accessory>();
            var ds = _dataAccess.GetAccessoryDetails(requestId);
            if (ds.Tables.Count > 0)
            {
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    list.Add(new Accessory
                    {
                        AccessoryWFMID = Convert.ToInt32(dr["AccessoryWFMID"]),
                        AccessoryName = dr["AccessoryName"].ToString(),
                        AccessoryCount = Convert.ToInt32(dr["AccessoryCount"]),
                        ProductStatus = dr["ProductStatus"].ToString(),
                        Remarks = dr["Remarks"].ToString()
                    });
                }
            }
            return list;
        }
        #endregion
    }
}
