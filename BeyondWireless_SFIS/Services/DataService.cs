using BeyondWireless_SFIS.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Deployment.Application;
using System.Threading.Tasks;
using static BeyondWireless_SFIS.Shared.ConnectionStrings;
using static BeyondWireless_SFIS.Shared.Logging;



namespace BeyondWireless_SFIS.Services
{
    public class DataService

    {

        public async Task<List<BlockedReason>> GetBlockedReasonsAsync(bool? isActive = null)
        {
            try
            {
                using (var connection = new SqlConnection(bwDb))
                using (var command = new SqlCommand("GetBlockedReasons", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.Add("@IsActive", SqlDbType.Bit).Value = (object)isActive ?? DBNull.Value;
                    await connection.OpenAsync();
                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        var reasons = new List<BlockedReason>();
                        while (await reader.ReadAsync())
                        {
                            reasons.Add(new BlockedReason
                            {
                                BlockedReasonId = reader.GetInt32(reader.GetOrdinal("BlockedReasonId")),
                                Reason = reader.GetString(reader.GetOrdinal("Reason")),
                                IsActive = reader.GetBoolean(reader.GetOrdinal("IsActive"))
                            });
                        }
                        return reasons;
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.Error($"Exception occurred in GetBlockedReasonsAsync, Error: {ex.Message}");
                throw;
            }
        }

        public void LogAppVersion()
        {
            var appVersion = string.Empty;
            if (ApplicationDeployment.IsNetworkDeployed)
            {
                appVersion = ApplicationDeployment.CurrentDeployment.CurrentVersion.ToString();
            }
            else
            {
                appVersion = "Not Deployed";
            }
            var hostName = Environment.MachineName;
            var appName = "BeyondWireless_SFIS";

            try
            {
                using (var connection = new SqlConnection(referenceDb))
                {
                    using (var command = new SqlCommand("LogAppVersion", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.Add("@HostName", SqlDbType.VarChar, 100).Value = hostName;
                        command.Parameters.Add("@AppVersion", SqlDbType.VarChar, 50).Value = appVersion;
                        command.Parameters.Add("@AppName", SqlDbType.VarChar, 100).Value = appName;
                        connection.Open();
                        command.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.Error($"Failed to log application version. Error: {ex.Message}");
            }
        }

        public bool DbConnectionOk()
        {
            try
            {
                using (var connection = new SqlConnection(bwDb))
                {
                    connection.Open();
                    connection.Close();
                    return true; // Connection successful
                }
            }
            catch (Exception)
            {
                return false;
            }
        }

        public async Task<List<Repair>> GetRepairsByUnitIdAsync(int unitId)
        {
            try
            {
                using (var connection = new SqlConnection(bwDb))
                {
                    await connection.OpenAsync();
                    using (var command = new SqlCommand("GetRepairsOfUnit", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.Add("@UnitId", SqlDbType.Int).Value = unitId;

                        using (var reader = await command.ExecuteReaderAsync())
                        {
                            var repairTypes = new List<Repair>();
                            while (await reader.ReadAsync())
                            {
                                repairTypes.Add(new Repair
                                {
                                    RepairId = reader.GetInt32(reader.GetOrdinal("RepairId")),
                                    RepairTypeId = reader.GetInt32(reader.GetOrdinal("RepairTypeId")),
                                    TestResultId = reader.GetInt32(reader.GetOrdinal("TestResultId")),
                                    RepairDescription = reader.GetString(reader.GetOrdinal("RepairDescription")),
                                    Comment = reader.GetString(reader.GetOrdinal("Comment")),
                                    RepairDate = reader.GetDateTime(reader.GetOrdinal("RepairDate")),
                                    UserId = reader.GetInt32(reader.GetOrdinal("UserId")),
                                    UnitId = reader.GetInt32(reader.GetOrdinal("UnitId"))


                                });
                            }
                            return repairTypes;
                        }
                    }
                }
            }
            catch (SqlException sx)
            {
                Logger.Error($"Exception happened in GetRepairsByUnitId. Error: {sx.Message}");
                throw;
            }
            catch (Exception ex)
            {
                Logger.Error($"Exception happened in GetRepairsByUnitId, Error: {ex.Message}");
                throw;
            }
        }

        public DbResponse AddTestResult(TestResult testResult)
        {
            try
            {
                using (var connection = new SqlConnection(bwDb))
                {
                    connection.Open();
                    using (var command = new SqlCommand("AddTestResultJson", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        var jsonResult = Newtonsoft.Json.JsonConvert.SerializeObject(testResult);
                        command.Parameters.Add("@jsonTestResult", SqlDbType.NVarChar).Value = jsonResult;
                        using (var reader = command.ExecuteReader())
                        {
                            if (reader.HasRows)
                            {
                                reader.Read();
                                return new DbResponse
                                {
                                    StatusCode = reader.GetInt32(reader.GetOrdinal("StatusCode")),
                                    Message = reader.GetString(reader.GetOrdinal("Message"))
                                };
                            }
                            else
                            {
                                return new DbResponse
                                {
                                    StatusCode = 500,
                                    Message = "No response from stored procedure."
                                };
                            }
                        }
                    }
                }
            }
            catch (SqlException sx)
            {
                Logger.Error($"SQL Exception occurred in AddTestResult, Error: {sx.Message}");
                throw;
            }
            catch (Exception ex)
            {
                Logger.Error($"Exception occurred in AddTestResult, Error: {ex.Message}");
                throw;
            }
        }

        public async Task LogReportUsageAsync(Report report, User user)
        {
            try
            {
                using (var sqlConnection = new SqlConnection(bwDb))
                {
                    await sqlConnection.OpenAsync();
                    using (var sqlCommand = new SqlCommand("LogReportUsage", sqlConnection))
                    {
                        sqlCommand.CommandType = CommandType.StoredProcedure;
                        sqlCommand.Parameters.Add("@ReportId", SqlDbType.Int).Value = report.ReportId;
                        sqlCommand.Parameters.Add("@UserId", SqlDbType.Int).Value = user.UserId;
                        await sqlCommand.ExecuteNonQueryAsync();
                    }
                }
            }
            catch (SqlException sx)
            {
                Logger.Error($"SQL Exception occurred in LogReportUsageAsync, ReportId: {report.ReportId}, Error: {sx.Message}");
            }
            catch (Exception ex)
            {
                Logger.Error($"Exception occurred in LogReportUsageAsync, ReportId: {report.ReportId}, Error: {ex.Message}");
            }
        }



        //public DbResponse AddTestResult(TestResult testResult)
        //{
        //    try
        //    {
        //        using (var connection = new SqlConnection(bwDb))
        //        {
        //            connection.Open();
        //            using (var command = new SqlCommand("AddTestResult", connection))
        //            {
        //                command.CommandType = CommandType.StoredProcedure;
        //                command.Parameters.Add("@UnitId", SqlDbType.Int).Value = testResult.UnitId;
        //                //command.Parameters.AddWithValue("@TestDate", testResult.TestDate); Use Sql Server Time
        //                command.Parameters.Add("@StageId", SqlDbType.Int).Value = testResult.StageId;
        //                command.Parameters.Add("@IsPass", SqlDbType.Bit).Value = testResult.IsPass;
        //                command.Parameters.Add("@DefectDescription", SqlDbType.VarChar, 200).Value = testResult.DefectDescription;
        //                command.Parameters.Add("DefectId", SqlDbType.Int).Value = testResult.DefectId;
        //                command.Parameters.Add("TestTypeId", SqlDbType.TinyInt).Value = testResult.TestTypeId;
        //                command.Parameters.Add("@Position", SqlDbType.VarChar, 200).Value = testResult.Position;
        //                command.Parameters.Add("@UserId", SqlDbType.Int).Value = testResult.UserId;
        //                using (var reader = command.ExecuteReader())
        //                {
        //                    if (reader.HasRows)
        //                    {
        //                        reader.Read();
        //                        return new DbResponse
        //                        {
        //                            StatusCode = reader.GetInt32(reader.GetOrdinal("StatusCode")),
        //                            Message = reader.GetString(reader.GetOrdinal("Message"))
        //                        };
        //                    }
        //                    else
        //                    {
        //                        return new DbResponse
        //                        {
        //                            StatusCode = 500,
        //                            Message = "No response from stored procedure."
        //                        };
        //                    }
        //                }
        //            }
        //        }
        //    }
        //    catch (SqlException sx)
        //    {
        //        Logger.Error($"SQL Exception occurred in AddTestResult, Error: {sx.Message}");
        //        throw;
        //    }
        //    catch (Exception ex)
        //    {
        //        Logger.Error($"Exception occurred in AddTestResult, Error: {ex.Message}");
        //        throw;
        //    }
        //}


        public DbResponse UpdateUnitStage(Unit unit, User user)
        {
            try
            {
                using (var connection = new SqlConnection(bwDb))
                {
                    connection.Open();
                    using (var command = new SqlCommand("UpdateUnitStage", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("@UnitId", unit.UnitId);
                        command.Parameters.AddWithValue("@StageId", unit.StageId);
                        command.Parameters.AddWithValue("@ModifiedBy", user.UserId);
                        using (var reader = command.ExecuteReader())
                        {
                            if (reader.HasRows)
                            {
                                reader.Read();
                                return new DbResponse
                                {
                                    StatusCode = reader.GetInt32(reader.GetOrdinal("StatusCode")),
                                    Message = reader.GetString(reader.GetOrdinal("Message"))
                                };
                            }
                            else
                            {
                                return new DbResponse
                                {
                                    StatusCode = 500,
                                    Message = "No response from stored procedure."
                                };
                            }
                        }
                    }
                }
            }
            catch (SqlException sx)
            {
                Logger.Error($"SQL Exception occurred in UpdateUnitStage, Error: {sx.Message}");
                throw;
            }
            catch (Exception ex)
            {
                Logger.Error($"Exception occurred in UpdateUnitStage, Error: {ex.Message}");
                throw;
            }
        }

        //Register a New unit and add a UnitHistory Record, 
        //If the unit exists, return the existing unit , else return the unit that was added
        public Unit AddUnit(Unit unit, User user)
        {
            try
            {
                using (var connection = new SqlConnection(bwDb))
                {
                    connection.Open();
                    using (var command = new SqlCommand("AddUnit", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("@SerialNumber", unit.SerialNumber);
                        command.Parameters.AddWithValue("@CustomerOrderId", unit.CustomerOrderId);
                        command.Parameters.AddWithValue("@IsFaulty", unit.IsFaulty);
                        command.Parameters.AddWithValue("@IsScrapped", unit.IsScrapped);
                        command.Parameters.AddWithValue("@ModifiedBy", user.UserId);

                        using (var reader = command.ExecuteReader())
                        {
                            if (reader.HasRows)
                            {
                                reader.Read();
                                return MapUnit(reader);
                            }
                            return null;
                        }
                    }
                }
            }
            catch (SqlException sx)
            {
                Logger.Error($"SQL Exception occurred in AddUnit, Error: {sx.Message}");
                throw;
            }
            catch (Exception ex)
            {
                Logger.Error($"Exception occurred in AddUnit, Error: {ex.Message}");
                throw;
            }
        }

        public DbResponse AddCustomerOrder(CustomerOrder customerOrder)
        {
            try
            {
                using (var connection = new SqlConnection(bwDb))
                {
                    connection.Open();
                    using (var command = new SqlCommand("AddCustomerOrder", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.Add("@CustomerOrderNumber", SqlDbType.VarChar, 50).Value = customerOrder.CustomerOrderNumber;
                        command.Parameters.Add("@ProductId", SqlDbType.Int).Value = customerOrder.ProductId;
                        command.Parameters.Add("@BuildTypeId", SqlDbType.Int).Value = customerOrder.BuildTypeId;
                        command.Parameters.Add("@Odf", SqlDbType.VarChar, 20).Value = customerOrder.Odf;
                        command.Parameters.Add("@PanelOdf", SqlDbType.VarChar, 20).Value = customerOrder.PanelOdf;
                        command.Parameters.Add("@QuantityOrdered", SqlDbType.Int).Value = customerOrder.QuantityOrdered;
                        command.Parameters.Add("@QuantityDespatched", SqlDbType.Int).Value = customerOrder.QuantityDespatched;
                        //command.Parameters.Add("@DateLoaded", SqlDbType.DateTime2, 7).Value = customerOrder.DateLoaded;
                        command.Parameters.Add("@BCAssemblyOrder", SqlDbType.VarChar, 50).Value = customerOrder.BCAssemblyOrder;
                        command.Parameters.Add("@ModifiedBy", SqlDbType.Int).Value = customerOrder.ModifiedBy;
                        command.Parameters.Add("@IsActive", SqlDbType.Bit).Value = customerOrder.IsActive;
                        command.Parameters.Add("@ExpectedKitArrivalDate", SqlDbType.DateTime2).Value = customerOrder.ExpectedKitArrivalDate.Date;
                        command.Parameters.Add("@ExpectedPanelArrivalDate", SqlDbType.DateTime2).Value = customerOrder.ExpectedPanelArrivalDate.Date;

                        using (var reader = command.ExecuteReader())
                        {
                            if (reader.HasRows)
                            {
                                reader.Read();
                                return new DbResponse
                                {
                                    StatusCode = reader.GetInt32(reader.GetOrdinal("StatusCode")),
                                    Message = reader.GetString(reader.GetOrdinal("Message"))
                                };
                            }
                            else
                            {
                                return new DbResponse
                                {
                                    StatusCode = 500,
                                    Message = "No response from stored procedure."
                                };
                            }
                        }
                    }
                }
            }
            catch (SqlException sx)
            {
                Logger.Error($"SQL Exception occurred in AddCustomerOrder, Error: {sx.Message}");
                throw;
            }
            catch (Exception ex)
            {
                Logger.Error($"Exception occurred in AddCustomerOrder, Error: {ex.Message}");
                throw;
            }
        }

        public DbResponse AddProduct(Product product)
        {
            try
            {
                using (var connection = new SqlConnection(bwDb))
                {
                    connection.Open();
                    using (var command = new SqlCommand("AddProduct", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.Add("@PartNumber", SqlDbType.VarChar, 50).Value = product.PartNumber;
                        command.Parameters.Add("@ProductName", SqlDbType.VarChar, 150).Value = product.ProductName;
                        command.Parameters.Add("@ProductShortCode", SqlDbType.VarChar, 50).Value = product.ProductShortCode;
                        command.Parameters.Add("@InspectionLevel", SqlDbType.Int).Value = product.InspectionLevel;
                        command.Parameters.Add("@BuildTypeId", SqlDbType.Int).Value = product.BuildTypeId;
                        command.Parameters.Add("@IsActive", SqlDbType.Bit).Value = product.IsActive;

                        using (var reader = command.ExecuteReader())
                        {
                            if (reader.HasRows)
                            {
                                reader.Read();
                                return new DbResponse
                                {
                                    StatusCode = reader.GetInt32(reader.GetOrdinal("StatusCode")),
                                    Message = reader.GetString(reader.GetOrdinal("Message"))
                                };
                            }
                            else
                            {
                                return new DbResponse
                                {
                                    StatusCode = 500,
                                    Message = "No response from stored procedure."
                                };
                            }
                        }
                    }
                }
            }
            catch (SqlException sx)
            {
                Logger.Error($"SQL Exception occurred in AddProduct, Error: {sx.Message}");
                throw;
            }
            catch (Exception ex)
            {
                Logger.Error($"Exception occurred in AddProduct, Error: {ex.Message}");
                throw;
            }
        }

        public DbResponse UpdateProduct(Product product)
        {
            try
            {
                using (var connection = new SqlConnection(bwDb))
                {
                    connection.Open();
                    using (var command = new SqlCommand("UpdateProduct", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.Add("@ProductId", SqlDbType.Int).Value = product.ProductId;
                        command.Parameters.Add("@PartNumber", SqlDbType.VarChar, 50).Value = product.PartNumber;
                        command.Parameters.Add("@ProductName", SqlDbType.VarChar, 150).Value = product.ProductName;
                        command.Parameters.Add("@ProductShortCode", SqlDbType.VarChar, 50).Value = product.ProductShortCode;
                        command.Parameters.Add("@InspectionLevel", SqlDbType.Int).Value = product.InspectionLevel;
                        command.Parameters.Add("@BuildTypeId", SqlDbType.Int).Value = product.BuildTypeId;
                        command.Parameters.Add("@IsActive", SqlDbType.Bit).Value = product.IsActive;


                        using (var reader = command.ExecuteReader())
                        {
                            if (reader.HasRows)
                            {
                                reader.Read();
                                return new DbResponse
                                {
                                    StatusCode = reader.GetInt32(reader.GetOrdinal("StatusCode")),
                                    Message = reader.GetString(reader.GetOrdinal("Message"))
                                };
                            }
                            else
                            {
                                return new DbResponse
                                {
                                    StatusCode = 500,
                                    Message = "No response from stored procedure."
                                };
                            }
                        }
                    }
                }
            }
            catch (SqlException sx)
            {
                Logger.Error($"SQL Exception occurred in UpdateProduct, Error: {sx.Message}");
                throw;
            }
            catch (Exception ex)
            {
                Logger.Error($"Exception occurred in UpdateProduct, Error: {ex.Message}");
                throw;
            }
        }


        public DbResponse UpdateCustomerOrder(CustomerOrder customerOrder)
        {
            try
            {
                using (var connection = new SqlConnection(bwDb))
                {
                    connection.Open();
                    using (var command = new SqlCommand("UpdateCustomerOrder", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.Add("@CustomerOrderId", SqlDbType.Int).Value = customerOrder.CustomerOrderId;
                        command.Parameters.Add("@CustomerOrderNumber", SqlDbType.VarChar, 50).Value = customerOrder.CustomerOrderNumber;
                        command.Parameters.Add("@ProductId", SqlDbType.Int).Value = customerOrder.ProductId;
                        command.Parameters.Add("@BuildTypeId", SqlDbType.Int).Value = customerOrder.BuildTypeId;
                        command.Parameters.Add("@Odf", SqlDbType.VarChar, 20).Value = customerOrder.Odf;
                        command.Parameters.Add("@PanelOdf", SqlDbType.VarChar, 20).Value = customerOrder.PanelOdf;
                        //command.Parameters.AddWithValue("@PartNumber", customerOrder.PartNumber);
                        //command.Parameters.AddWithValue("@ProductName", customerOrder.ProductName);
                        command.Parameters.Add("@QuantityOrdered", SqlDbType.Int).Value = customerOrder.QuantityOrdered;
                        //command.Parameters.AddWithValue("@QuantityDespatched", customerOrder.QuantityDespatched);
                        //command.Parameters.AddWithValue("@DateLoaded", customerOrder.DateLoaded);
                        command.Parameters.Add("@BCAssemblyOrder", SqlDbType.VarChar, 50).Value = customerOrder.BCAssemblyOrder;
                        command.Parameters.Add("@ModifiedBy", SqlDbType.Int).Value = customerOrder.ModifiedBy;
                        command.Parameters.Add("@IsActive", SqlDbType.Bit).Value = customerOrder.IsActive;
                        command.Parameters.Add("@LabelPrintingActive", SqlDbType.Bit).Value = customerOrder.LabelPrintingActive;
                        command.Parameters.Add("@ExpectedKitArrivalDate", SqlDbType.DateTime2).Value = customerOrder.ExpectedKitArrivalDate.Date;
                        command.Parameters.Add("@ExpectedPanelArrivalDate", SqlDbType.DateTime2).Value = customerOrder.ExpectedPanelArrivalDate.Date;

                        using (var reader = command.ExecuteReader())
                        {
                            if (reader.HasRows)
                            {
                                reader.Read();
                                return new DbResponse
                                {
                                    StatusCode = reader.GetInt32(reader.GetOrdinal("StatusCode")),
                                    Message = reader.GetString(reader.GetOrdinal("Message"))
                                };
                            }
                            else
                            {
                                return new DbResponse
                                {
                                    StatusCode = 500,
                                    Message = "No response from stored procedure."
                                };
                            }
                        }
                    }
                }
            }
            catch (SqlException sx)
            {
                Logger.Error($"SQL Exception occurred in UpdateCustomerOrder, Error: {sx.Message}");
                throw;
            }
            catch (Exception ex)
            {
                Logger.Error($"Exception occurred in UpdateCustomerOrder, Error: {ex.Message}");
                throw;
            }
        }

        public async Task<List<CustomerOrder>> GetCustomerOrdersAsync(bool? isActive = null)
        {
            try
            {
                using (var connection = new SqlConnection(bwDb))
                {
                    await connection.OpenAsync();
                    using (var command = new SqlCommand("GetCustomerOrders", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        if (isActive.HasValue)
                        {
                            command.Parameters.Add("@IsActive", SqlDbType.Bit).Value = isActive.HasValue ? (object)isActive.Value : DBNull.Value;
                        }
                        else
                        {
                            command.Parameters.Add("@IsActive", SqlDbType.Bit).Value = DBNull.Value; // Handle null case
                        }

                        using (var reader = await command.ExecuteReaderAsync())
                        {
                            var customerOrders = new List<CustomerOrder>();
                            while (await reader.ReadAsync())
                            {
                                customerOrders.Add(MapCustomerOrder(reader));
                            }
                            return customerOrders;
                        }
                    }
                }
            }
            catch (SqlException sx)
            {
                Logger.Error($"SQL Exception occurred in GetCustomerOrdersAsync Error: {sx.Message}");
                throw;
            }
            catch (Exception ex)
            {
                Logger.Error($"Exception occurred in GetCustomerOrdersAsync, Error: {ex.Message}");
                throw;
            }
        }

        public async Task<List<CustomerOrder>> GetCustomerOrdersOfProductAsync(int ProductId, bool? IsActive = null)
        {
            try
            {
                using (var connection = new SqlConnection(bwDb))
                {
                    await connection.OpenAsync();
                    using (var command = new SqlCommand("GetCustomerOrdersForProduct", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;

                        command.Parameters.Add("@ProductId", SqlDbType.Int).Value = ProductId;

                        if (IsActive.HasValue)
                        {
                            command.Parameters.Add("@IsActive", SqlDbType.Bit).Value = IsActive.HasValue ? (object)IsActive.Value : DBNull.Value;
                        }
                        else
                        {
                            command.Parameters.AddWithValue("@IsActive", DBNull.Value);
                        }

                        using (var reader = await command.ExecuteReaderAsync())
                        {
                            var customerOrders = new List<CustomerOrder>();
                            while (await reader.ReadAsync())
                            {
                                customerOrders.Add(MapCustomerOrder(reader));
                            }
                            return customerOrders;
                        }
                    }
                }
            }
            catch (SqlException sx)
            {
                Logger.Error($"SQL Exception occurred in GetCustomerOrdersOfProductAsync Error: {sx.Message}");
                throw;
            }
            catch (Exception ex)
            {
                Logger.Error($"Exception occurred in GetCustomerOrdersOfProductAsync, Error: {ex.Message}");
                throw;
            }
        }

        //public async Task<List<CustomerOrderSummary>> GetCustomerOrderSummaryAsync(bool? isActive = null)
        //{
        //    try
        //    {
        //        using (var connection = new SqlConnection(bwDb))
        //        {
        //            await connection.OpenAsync();
        //            using (var command = new SqlCommand("GetCustomerOrderSummary", connection))
        //            {
        //                command.CommandType = CommandType.StoredProcedure;
        //                if (isActive.HasValue)
        //                {
        //                    command.Parameters.Add("@IsActive", SqlDbType.Bit).Value = isActive.Value ? 1 : 0;
        //                }
        //                else
        //                {
        //                    command.Parameters.Add("@IsActive", SqlDbType.Bit).Value = DBNull.Value;
        //                }
        //                using (var reader = await command.ExecuteReaderAsync())
        //                {
        //                    var summaries = new List<CustomerOrderSummary>();
        //                    while (await reader.ReadAsync())
        //                    {
        //                        summaries.Add(new CustomerOrderSummary
        //                        {
        //                            CustomerOrderId = reader.GetInt32(reader.GetOrdinal("CustomerOrderId")),
        //                            Odf = reader.GetString(reader.GetOrdinal("Odf")),
        //                            ProductId = reader.GetInt32(reader.GetOrdinal("ProductId")),
        //                            Model = reader.GetString(reader.GetOrdinal("Model")),
        //                            BuildTypeName = reader.GetString(reader.GetOrdinal("BuildTypeName")),
        //                            OrderQuantity = reader.GetInt32(reader.GetOrdinal("OrderQuantity")),
        //                            RegisteredUnits = reader.GetInt32(reader.GetOrdinal("RegisteredUnits")),
        //                            WarehouseUnits = reader.GetInt32(reader.GetOrdinal("WarehouseUnits")),
        //                            DespatchedUnits = reader.GetInt32(reader.GetOrdinal("DespatchedUnits")),
        //                            ScrappedUnits = reader.GetInt32(reader.GetOrdinal("ScrappedUnits")),
        //                            FaultyUnits = reader.GetInt32(reader.GetOrdinal("FaultyUnits")),
        //                            BlockedUnits = reader.GetInt32(reader.GetOrdinal("BlockedUnits")),
        //                            IsActive = reader.GetBoolean(reader.GetOrdinal("IsActive"))
        //                        });
        //                    }
        //                    return summaries;
        //                }
        //            }
        //        }
        //    }
        //    catch (SqlException sx)
        //    {
        //        Logger.Error($"SQL Exception occurred in GetCustomerOrderSummaryAsync, Error: {sx.Message}");
        //        throw;
        //    }
        //    catch (Exception ex)
        //    {
        //        Logger.Error($"Exception occurred in GetCustomerOrderSummaryAsync, Error: {ex.Message}");
        //        throw;
        //    }
        //}

        public CustomerOrder GetCustomerOrderByODF(string odf)
        {
            try
            {
                using (var connection = new SqlConnection(bwDb))
                {
                    connection.Open();
                    using (var command = new SqlCommand("GetCustomerOrderByODF", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("@odf", odf);
                        using (var reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                return MapCustomerOrder(reader);
                            }
                        }
                    }
                }
            }
            catch (SqlException sx)
            {
                Logger.Error($"SQL Exception occurred in GetCustomerOrderByODF, ODF: {odf}, Error: {sx.Message}");
                throw;
            }
            catch (Exception ex)
            {
                Logger.Error($"Exception occurred in GetCustomerOrderByODF, ODF: {odf}, Error: {ex.Message}");
                throw;
            }
            return null;
        }

        private static CustomerOrder MapCustomerOrder(SqlDataReader reader)
        {
            return new CustomerOrder
            {
                CustomerOrderId = reader.GetInt32(reader.GetOrdinal("CustomerOrderId")),
                CustomerOrderNumber = reader.GetString(reader.GetOrdinal("CustomerOrderNumber")),
                CustomerOrderStatusId = reader.GetInt32(reader.GetOrdinal("CustomerOrderStatusId")),
                StatusName = reader.GetString(reader.GetOrdinal("StatusName")),
                ProductId = reader.GetInt32(reader.GetOrdinal("ProductId")),
                BuildTypeId = reader.GetInt32(reader.GetOrdinal("BuildTypeId")),
                Odf = reader.GetString(reader.GetOrdinal("ODF")),
                PanelOdf = reader.IsDBNull(reader.GetOrdinal("PanelOdf")) ? null : reader.GetString(reader.GetOrdinal("PanelOdf")),
                PartNumber = reader.GetString(reader.GetOrdinal("PartNumber")),
                ProductName = reader.GetString(reader.GetOrdinal("ProductName")),
                QuantityOrdered = reader.GetInt32(reader.GetOrdinal("QuantityOrdered")),
                QuantityDespatched = reader.GetInt32(reader.GetOrdinal("QuantityDespatched")),
                PcbasDespatched = reader.IsDBNull(reader.GetOrdinal("PcbasDespatched")) ? 0 : reader.GetInt32(reader.GetOrdinal("PcbasDespatched")),
                DateLoaded = reader.GetDateTime(reader.GetOrdinal("DateLoaded")),
                BCAssemblyOrder = reader.GetString(reader.GetOrdinal("BCAssemblyOrder")),
                DateModified = reader.GetDateTime(reader.GetOrdinal("DateModified")),
                ModifiedBy = reader.GetInt32(reader.GetOrdinal("ModifiedBy")),
                IsActive = reader.GetBoolean(reader.GetOrdinal("IsActive")),
                BuildType = reader.GetString(reader.GetOrdinal("BuildType")),
                Model = reader.GetString(reader.GetOrdinal("Model")),
                LabelPrintingActive = reader.GetBoolean(reader.GetOrdinal("LabelPrintingActive")),
                ExpectedKitArrivalDate = reader.GetDateTime(reader.GetOrdinal("ExpectedKitArrivalDate")),
                ExpectedPanelArrivalDate = reader.GetDateTime(reader.GetOrdinal("ExpectedPanelArrivalDate")),
                PID = reader.IsDBNull(reader.GetOrdinal("PID")) ? string.Empty : reader.GetString(reader.GetOrdinal("PID"))
            };
        }

        public async Task<DataTable> GetDespatchesByCustomerOrderIdAsync(int customerOrderId)
        {
            DataTable dt = new DataTable();
            try
            {
                using (var connection = new SqlConnection(bwDb))
                {
                    await connection.OpenAsync();
                    using (var command = new SqlCommand("GetDespatchesByCustomerOrderId", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.Add("@CustomerOrderId", SqlDbType.Int).Value = customerOrderId;

                        using (var adapter = new SqlDataAdapter(command))
                        {
                            adapter.Fill(dt);
                        }

                        return dt;
                    }
                }
            }
            catch (SqlException sx)
            {
                Logger.Error($"SQL Exception occurred in GetDespatchesByCustomerOrderIdAsync, Error: {sx.Message}");
                throw;
            }
            catch (Exception ex)
            {
                Logger.Error($"Exception occurred in GetDespatchesByCustomerOrderIdAsync, Error: {ex.Message}");
                throw;
            }
        }

        public Unit GetUnitBySerialNumber(string serialNumber)
        {
            try
            {
                using (var connection = new SqlConnection(bwDb))
                {
                    connection.Open();

                    using (var command = new SqlCommand("GetUnitBySerialNumber", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("@serialNumber", serialNumber);

                        using (var reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                return MapUnit(reader);
                            }
                        }
                    }
                }
            }
            catch (SqlException sx)
            {
                Logger.Error($"SQL Exception occurred in GetUnitBySerialNumber, serialNumber: {serialNumber}, Error: {sx.Message}");
                throw;
            }
            catch (Exception ex)
            {
                Logger.Error($"Exception occurred in GetUnitBySerialNumber, serialNumber: {serialNumber}, Error:; {ex.Message}");
                throw;
            }

            return null;
        }

        public async Task<Unit> GetUnitBySerialNumberAsync(string serialNumber)
        {
            try
            {
                using (var connection = new SqlConnection(bwDb))
                {
                    await connection.OpenAsync();

                    using (var command = new SqlCommand("GetUnitBySerialNumber", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("@serialNumber", serialNumber);

                        using (var reader = await command.ExecuteReaderAsync())
                        {
                            if (await reader.ReadAsync())
                                return MapUnit(reader);

                            return null; // Or handle no results appropriately
                        }
                    }
                }
            }
            catch (SqlException sx)
            {
                Logger.Error($"SQL Exception in GetUnitBySerialNumberAsync. Serial: {serialNumber}, Error: {sx.Message}");
                throw;
            }
            catch (Exception ex)
            {
                Logger.Error($"Exception in GetUnitBySerialNumberAsync. Serial: {serialNumber}, Error: {ex.Message}");
                throw;
            }
        }

        public Unit GetUnitBySetSerialNumber(string setSerialNumber)
        {
            try
            {
                using (var connection = new SqlConnection(bwDb))
                {
                    connection.Open();

                    using (var command = new SqlCommand("GetUnitBySetSerialNumber", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.Add("@SetSerialNumber", SqlDbType.VarChar, 50).Value = setSerialNumber;

                        using (var reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                return MapUnit(reader);
                            }
                        }
                    }
                }
            }
            catch (SqlException sx)
            {
                Logger.Error($"SQL Exception occurred in GetUnitBySetSerialNumber, serialNumber: {setSerialNumber}, Error: {sx.Message}");
                throw;
            }
            catch (Exception ex)
            {
                Logger.Error($"Exception occurred in GetUnitBySetSerialNumber, serialNumber: {setSerialNumber}, Error:; {ex.Message}");
                throw;
            }

            return null;
        }

        public async Task<Unit> GetUnitBySetSerialNumberAsync(string serialNumber)
        {
            try
            {
                using (var connection = new SqlConnection(bwDb))
                {
                    await connection.OpenAsync();

                    using (var command = new SqlCommand("GetUnitBySerialNumber", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("@SerialNumber", serialNumber);

                        using (var reader = await command.ExecuteReaderAsync())
                        {
                            await reader.ReadAsync();

                            if (reader.HasRows)
                            {

                                if (reader.Read())
                                    return MapUnit(reader);
                            }
                            return null;
                        }
                    }
                }
            }
            catch (SqlException sx)
            {
                Logger.Error($"SQL Exception occurred in GetUnitBySetSerialNumberAsync, serialNumber: {serialNumber}, Error: {sx.Message}");
                throw;
            }
            catch (Exception ex)
            {
                Logger.Error($"Exception occurred in GetUnitBySetSerialNumberAsync, serialNumber: {serialNumber}, Error:; {ex.Message}");
                throw;
            }
        }


        private static Unit MapUnit(SqlDataReader reader)
        {
            //return new Unit
            //{
            //    UnitId = reader.GetInt32(reader.GetOrdinal("UnitId")),
            //    SerialNumber = reader.GetString(reader.GetOrdinal("SerialNumber")),
            //    CustomerOrderId = reader.GetInt32(reader.GetOrdinal("CustomerOrderId")),
            //    StageId = reader.GetInt32(reader.GetOrdinal("StageId")),
            //    IsFaulty = reader.GetBoolean(reader.GetOrdinal("IsFaulty")),
            //    IsScrapped = reader.GetBoolean(reader.GetOrdinal("IsScrapped")),
            //    DateModified = reader.GetDateTime(reader.GetOrdinal("DateModified")),

            //    StageName = reader.GetString(reader.GetOrdinal("StageName")),
            //    NextStageId = reader.GetInt32(reader.GetOrdinal("NextStageId")),
            //    NextStageName = reader.GetString(reader.GetOrdinal("NextStageName")),
            //    NextRequiredStageId = reader.GetInt32(reader.GetOrdinal("NextRequiredStageId")),
            //    NextRequiredStageName = reader.GetString(reader.GetOrdinal("NextRequiredStageName")),
            //    ODF = reader.GetString(reader.GetOrdinal("ODF"))

            //};
            var unit = new Unit();
            unit.UnitId = reader.GetInt32(reader.GetOrdinal("UnitId"));
            unit.SerialNumber = reader.GetString(reader.GetOrdinal("SerialNumber"));
            unit.CustomerOrderId = reader.GetInt32(reader.GetOrdinal("CustomerOrderId"));
            unit.StageId = reader.GetInt32(reader.GetOrdinal("StageId"));
            unit.IsFaulty = reader.GetBoolean(reader.GetOrdinal("IsFaulty"));
            unit.IsScrapped = reader.GetBoolean(reader.GetOrdinal("IsScrapped"));
            unit.DateModified = reader.GetDateTime(reader.GetOrdinal("DateModified"));
            unit.ProductId = reader.GetInt32(reader.GetOrdinal("ProductId"));
            unit.StageName = reader.GetString(reader.GetOrdinal("StageName"));
            unit.NextStageId = reader.GetInt32(reader.GetOrdinal("NextStageId"));
            unit.NextStageName = reader.GetString(reader.GetOrdinal("NextStageName"));
            unit.NextRequiredStageId = reader.GetInt32(reader.GetOrdinal("NextRequiredStageId"));
            unit.NextRequiredStageName = reader.GetString(reader.GetOrdinal("NextRequiredStageName"));
            unit.Odf = reader.GetString(reader.GetOrdinal("Odf"));
            unit.SetSerialNumber = reader.GetString(reader.GetOrdinal("SetSerialNumber"));
            unit.Model = reader.GetString(reader.GetOrdinal("Model"));
            unit.CanRetest = reader.GetBoolean(reader.GetOrdinal("CanRetest"));
            unit.IsBlocked = reader.GetBoolean(reader.GetOrdinal("IsBlocked"));
            unit.IsStolen = reader.GetBoolean(reader.GetOrdinal("IsStolen"));
            unit.ReasonBlocked = reader.IsDBNull(reader.GetOrdinal("ReasonBlocked")) ? null : reader.GetString(reader.GetOrdinal("ReasonBlocked"));
            return unit;
        }

        public async Task<List<Product>> GetProductsAsync(bool? isActive = null)
        {
            List<Product> products = new List<Product>();
            try
            {
                using (var connection = new SqlConnection(bwDb))
                {
                    await connection.OpenAsync();
                    using (var command = new SqlCommand("GetProducts", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        if (isActive.HasValue)
                        {
                            command.Parameters.AddWithValue("@IsActive", isActive.Value ? 1 : 0);
                        }
                        else
                        {
                            command.Parameters.AddWithValue("@IsActive", DBNull.Value);
                        }

                        using (var reader = await command.ExecuteReaderAsync())
                        {

                            while (await reader.ReadAsync())
                            {
                                products.Add(MapProduct(reader));
                            }


                            return products;
                        }
                    }
                }
            }
            catch (SqlException sx)
            {
                Logger.Error($"SQL Exception occurred in GetProducts, Error:; {sx.Message}");
                throw;
            }
            catch (Exception ex)
            {
                Logger.Error($"Exception occurred in GetProducts, Error:; {ex.Message}");
                throw;
            }
        }

        private static Product MapProduct(SqlDataReader reader)
        {
            return new Product
            {
                ProductId = reader.GetInt32(reader.GetOrdinal("ProductId")),
                PartNumber = reader.GetString(reader.GetOrdinal("PartNumber")),
                ProductName = reader.GetString(reader.GetOrdinal("ProductName")),
                ProductShortCode = reader.GetString(reader.GetOrdinal("ProductShortCode")),
                InspectionLevel = reader.GetInt32(reader.GetOrdinal("InspectionLevel")),
                BuildTypeId = reader.GetInt32(reader.GetOrdinal("BuildTypeId")),
                BuildTypeName = reader.GetString(reader.GetOrdinal("BuildTypeName")),
                HasSerialsPrinted = reader.GetBoolean(reader.GetOrdinal("HasSerialsPrinted")),
                IsActive = reader.GetBoolean(reader.GetOrdinal("IsActive"))
            };
        }


        public User GetUserByRfidSerial(string rfidSerial)
        {
            try
            {
                using (var connection = new SqlConnection(bwDb))
                {
                    connection.Open();

                    var query = @"GetUserByRfidSerial";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@RfidSerial", rfidSerial);
                        command.CommandType = CommandType.StoredProcedure;

                        var reader = command.ExecuteReader();

                        if (reader.Read())
                        {
                            return new User
                            {
                                UserId = reader.GetInt32(reader.GetOrdinal("UserId")),
                                FullName = reader.GetString(reader.GetOrdinal("FullName")),
                                RFIDSerial = reader.GetString(reader.GetOrdinal("RFIDSerial")),
                                AuthorityLevel = reader.GetInt32(reader.GetOrdinal("AuthorityLevel")),
                                IsActive = reader.GetBoolean(reader.GetOrdinal("IsActive"))
                            };
                        }
                    }
                }
            }
            catch (SqlException sx)
            {
                Logger.Error($"SQL Exception occurred in GetUserByRfidSerial, RFID Serial: {rfidSerial}, Error: {sx.Message}");
                throw sx;
            }
            catch (Exception ex)
            {
                Logger.Error($"Exception occurred in GetUserByRfidSerial, RFID Serial: {rfidSerial}, Error: {ex.Message}");
                throw ex;
            }

            return null;
        }

        public async Task<List<Stage>> GetStagesAsync(bool? IsActive = null)
        {
            string sql = "GetStages";
            try
            {
                using (var connection = new SqlConnection(bwDb))
                {
                    await connection.OpenAsync();
                    using (var command = new SqlCommand(sql, connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;

                        if (IsActive.HasValue)
                        {
                            command.Parameters.AddWithValue("@IsActive", IsActive.Value ? 1 : 0);
                        }
                        else
                        {
                            command.Parameters.AddWithValue("@IsActive", DBNull.Value);
                        }

                        using (var reader = await command.ExecuteReaderAsync())
                        {
                            var stages = new List<Stage>();
                            while (await reader.ReadAsync())
                            {
                                stages.Add(MapStage(reader));
                            }
                            return stages;
                        }
                    }
                }
            }
            catch (SqlException sx)
            {
                Logger.Error($"SQL Exception occurred in GetStagesAsync, Error: {sx.Message}");
                throw;
            }
            catch (Exception ex)
            {
                Logger.Error($"Exception occurred in GetStagesAsync, Error: {ex.Message}");
                throw;
            }

        }

        public async Task<List<Stage>> GetStagesByBuildTypeIdAsync(int buildTypeId)
        {
            string sql = "GetStagesByBuildTypeId";
            try
            {
                using (var connection = new SqlConnection(bwDb))
                {
                    await connection.OpenAsync();
                    using (var command = new SqlCommand(sql, connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("@BuildTypeId", buildTypeId);

                        using (var reader = await command.ExecuteReaderAsync())
                        {
                            var stages = new List<Stage>();
                            while (await reader.ReadAsync())
                            {
                                stages.Add(MapStage(reader));
                            }
                            return stages;
                        }
                    }
                }
            }
            catch (SqlException sx)
            {
                Logger.Error($"SQL Exception occurred in GetStagesByBuildTypeIdAsync, Error: {sx.Message}");
                throw;
            }
            catch (Exception ex)
            {
                Logger.Error($"Exception occurred in GetStagesByBuildTypeIdAsync, Error: {ex.Message}");
                throw;
            }
        }

        private static Stage MapStage(SqlDataReader reader)
        {
            return new Stage
            {
                StageId = reader.GetInt32(reader.GetOrdinal("StageId")),
                BuildTypeId = reader.GetInt32(reader.GetOrdinal("BuildTypeId")),
                SequenceNumber = reader.GetInt32(reader.GetOrdinal("SequenceNumber")),
                StageName = reader.GetString(reader.GetOrdinal("StageName")),
                IsActive = reader.GetBoolean(reader.GetOrdinal("IsActive")),
                TestTypeId = reader.GetInt32(reader.GetOrdinal("TestTypeId"))

            };
        }


        public DbResponse AddOrUpdateLinkModule(LinkModule linkModule, int userId)
        {
            try
            {
                using (var connection = new SqlConnection(bwDb))
                {
                    connection.Open();
                    using (var command = new SqlCommand("AddOrUpdateLinkModule", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.Add("@LinkModuleId", SqlDbType.Int).Value = linkModule.LinkModuleId;
                        command.Parameters.Add("@PartNumber", SqlDbType.VarChar, 50).Value = linkModule.PartNumber;
                        command.Parameters.Add("@ModuleName", SqlDbType.VarChar, 150).Value = linkModule.ModuleName;
                        command.Parameters.Add("@ProductId", SqlDbType.Int).Value = linkModule.ProductId;
                        command.Parameters.Add("@IsActive", SqlDbType.Bit).Value = linkModule.IsActive;
                        command.Parameters.Add("@UserId", SqlDbType.Int).Value = userId;

                        using (var reader = command.ExecuteReader())
                        {
                            if (reader.HasRows)
                            {
                                reader.Read();
                                return new DbResponse
                                {
                                    StatusCode = reader.GetInt32(reader.GetOrdinal("StatusCode")),
                                    Message = reader.GetString(reader.GetOrdinal("Message"))
                                };
                            }
                            else
                            {
                                return new DbResponse
                                {
                                    StatusCode = 500,
                                    Message = "No response from stored procedure."
                                };
                            }
                        }
                    }
                }
            }
            catch (SqlException sx)
            {
                Logger.Error($"SQL Exception occurred in AddOrUpdateLinkModule, Error: {sx.Message}");
                throw;
            }
            catch (Exception ex)
            {
                Logger.Error($"Exception occurred in AddOrUpdateLinkModule, Error: {ex.Message}");
                throw;
            }
        }



        //public DbResponse AddOrUpdateLinkAccessory(LinkAccessory linkAccessory, int userId)
        //{
        //    try
        //    {
        //        using (var connection = new SqlConnection(bwDb))
        //        {
        //            connection.Open();
        //            using (var command = new SqlCommand("AddOrUpdateLinkAccessory", connection))
        //            {
        //                command.CommandType = CommandType.StoredProcedure;
        //                command.Parameters.Add("@LinkAccessoryId", SqlDbType.Int).Value = linkAccessory.LinkAccessoryId;
        //                command.Parameters.Add("@PartNumber", SqlDbType.VarChar, 50).Value = linkAccessory.PartNumber;
        //                command.Parameters.Add("@PartName", SqlDbType.VarChar, 30).Value = linkAccessory.PartName;
        //                command.Parameters.Add("@ProductId", SqlDbType.Int).Value = linkAccessory.ProductId;
        //                command.Parameters.Add("@IsActive", SqlDbType.Bit).Value = linkAccessory.IsActive;
        //                command.Parameters.Add("@UserId", SqlDbType.Int).Value = userId;

        //                using (var reader = command.ExecuteReader())
        //                {
        //                    if (reader.HasRows)
        //                    {
        //                        reader.Read();
        //                        return new DbResponse
        //                        {
        //                            StatusCode = reader.GetInt32(reader.GetOrdinal("StatusCode")),
        //                            Message = reader.GetString(reader.GetOrdinal("Message"))
        //                        };
        //                    }
        //                    else
        //                    {
        //                        return new DbResponse
        //                        {
        //                            StatusCode = 500,
        //                            Message = "No response from stored procedure."
        //                        };
        //                    }
        //                }
        //            }
        //        }
        //    }
        //    catch (SqlException sx)
        //    {
        //        Logger.Error($"SQL Exception occurred in AddOrUpdateLinkAcccessory, Error: {sx.Message}");
        //        throw;
        //    }
        //    catch (Exception ex)
        //    {
        //        Logger.Error($"Exception occurred in AddOrUpdateLinkAcccessory, Error: {ex.Message}");
        //        throw;
        //    }
        //}

        public async Task<List<LinkModule>> GetModulesToLinkAsync(int customerOrderId)
        {
            try
            {
                using (var connection = new SqlConnection(bwDb))
                {
                    await connection.OpenAsync();
                    using (var command = new SqlCommand("GetModulesToLinkByOrderId", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("@CustomerOrderId", customerOrderId);

                        using (var reader = await command.ExecuteReaderAsync())
                        {
                            var modules = new List<LinkModule>();
                            while (await reader.ReadAsync())
                            {
                                modules.Add(new LinkModule
                                {
                                    LinkModuleId = reader.GetInt32(reader.GetOrdinal("LinkModuleId")),
                                    ProductId = reader.GetInt32(reader.GetOrdinal("ProductId")),
                                    ModuleName = reader.GetString(reader.GetOrdinal("ModuleName")),
                                    PartNumber = reader.GetString(reader.GetOrdinal("PartNumber")),
                                    IsActive = reader.GetBoolean(reader.GetOrdinal("IsActive"))
                                });
                            }
                            return modules;
                        }
                    }
                }
            }
            catch (SqlException sx)
            {
                Logger.Error($"SQL Exception occurred in GetModulesToLinkAsync, Error: {sx.Message}");
                throw;
            }
            catch (Exception ex)
            {
                Logger.Error($"Exception occurred in GetModulesToLinkAsync, Error: {ex.Message}");
                throw;
            }

        }

        public async Task<List<LinkModule>> GetLinkModulesOfProductAsync(int productId)
        {
            try
            {
                using (var connection = new SqlConnection(bwDb))
                {
                    await connection.OpenAsync();
                    using (var command = new SqlCommand("GetLinkModulesOfProduct", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("@ProductId", productId);

                        using (var reader = await command.ExecuteReaderAsync())
                        {
                            var modules = new List<LinkModule>();
                            while (await reader.ReadAsync())
                            {
                                modules.Add(new LinkModule
                                {
                                    LinkModuleId = reader.GetInt32(reader.GetOrdinal("LinkModuleId")),
                                    ProductId = reader.GetInt32(reader.GetOrdinal("ProductId")),
                                    ModuleName = reader.GetString(reader.GetOrdinal("ModuleName")),
                                    PartNumber = reader.GetString(reader.GetOrdinal("PartNumber")),
                                    IsActive = reader.GetBoolean(reader.GetOrdinal("IsActive"))
                                });
                            }
                            return modules;
                        }
                    }
                }
            }
            catch (SqlException sx)
            {
                Logger.Error($"SQL Exception occurred in GetLinkModulesOfProductAsync, Error: {sx.Message}");
                throw;
            }
            catch (Exception ex)
            {
                Logger.Error($"Exception occurred in GetLinkModulesOfProductAsync, Error: {ex.Message}");
                throw;
            }

        }

        //public async Task<List<LinkAccessory>> GetAccessoriesToLinkAsync(int customerOrderId)
        //{
        //    try
        //    {
        //        using (var connection = new SqlConnection(bwDb))
        //        {
        //            await connection.OpenAsync();
        //            using (var command = new SqlCommand("GetAccessoriesToLinkByOrderId", connection))
        //            {
        //                command.CommandType = CommandType.StoredProcedure;
        //                command.Parameters.AddWithValue("@CustomerOrderId", customerOrderId);

        //                using (var reader = await command.ExecuteReaderAsync())
        //                {
        //                    var accessories = new List<LinkAccessory>();
        //                    while (await reader.ReadAsync())
        //                    {
        //                        accessories.Add(new LinkAccessory
        //                        {
        //                            LinkAccessoryId = reader.GetInt32(reader.GetOrdinal("LinkAccessoryId")),
        //                            ProductId = reader.GetInt32(reader.GetOrdinal("ProductId")),
        //                            PartNumber = reader.GetString(reader.GetOrdinal("PartNumber")).Trim(),//Trim() There was /r/n being appended - possibly a bug with how the accessorty was svaed
        //                            PartName = reader.GetString(reader.GetOrdinal("PartName")),
        //                            IsActive = reader.GetBoolean(reader.GetOrdinal("IsActive"))
        //                        });
        //                    }
        //                    return accessories;
        //                }
        //            }
        //        }
        //    }
        //    catch (SqlException sx)
        //    {
        //        Logger.Error($"SQL Exception occurred in GetAccessoriesToLinkAsync, Error: {sx.Message}");
        //        throw;
        //    }
        //    catch (Exception ex)
        //    {
        //        Logger.Error($"Exception occurred in GetAccessoriesToLinkAsync, Error: {ex.Message}");
        //        throw;
        //    }

        //}

        //public async Task<List<LinkAccessory>> GetLinkAccessoriesOfProductAsync(int productId)
        //{
        //    try
        //    {
        //        using (var connection = new SqlConnection(bwDb))
        //        {
        //            await connection.OpenAsync();
        //            using (var command = new SqlCommand("GetLinkAccessoriesOfProduct", connection))
        //            {
        //                command.CommandType = CommandType.StoredProcedure;
        //                command.Parameters.AddWithValue("@ProductId", productId);

        //                using (var reader = await command.ExecuteReaderAsync())
        //                {
        //                    var accessories = new List<LinkAccessory>();
        //                    while (await reader.ReadAsync())
        //                    {
        //                        accessories.Add(new LinkAccessory
        //                        {
        //                            LinkAccessoryId = reader.GetInt32(reader.GetOrdinal("LinkAccessoryId")),
        //                            ProductId = reader.GetInt32(reader.GetOrdinal("ProductId")),
        //                            PartNumber = reader.GetString(reader.GetOrdinal("PartNumber")),
        //                            PartName = reader.GetString(reader.GetOrdinal("PartName")),
        //                            IsActive = reader.GetBoolean(reader.GetOrdinal("IsActive"))
        //                        });
        //                    }
        //                    return accessories;
        //                }
        //            }
        //        }
        //    }
        //    catch (SqlException sx)
        //    {
        //        Logger.Error($"SQL Exception occurred in GetLinkAccessoriesOfProductAsync, Error: {sx.Message}");
        //        throw;
        //    }
        //    catch (Exception ex)
        //    {
        //        Logger.Error($"Exception occurred in GetLinkAccessoriesOfProductAsync, Error: {ex.Message}");
        //        throw;
        //    }

        //}

        public DbResponse AddLinkedModules(string jsonModulesToSave, int unitId, int userId, int stageId)
        {
            string sql = "AddLinkedModules";
            try
            {
                using (var connection = new SqlConnection(bwDb))
                {
                    connection.Open();
                    using (var command = new SqlCommand(sql, connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("@JsonData", jsonModulesToSave);
                        command.Parameters.AddWithValue("@UnitId", unitId);
                        command.Parameters.AddWithValue("@UserId", userId);
                        command.Parameters.AddWithValue("@StageId", stageId);

                        using (var reader = command.ExecuteReader())
                        {
                            if (reader.HasRows)
                            {
                                reader.Read();
                                return new DbResponse
                                {
                                    StatusCode = reader.GetInt32(reader.GetOrdinal("StatusCode")),
                                    Message = reader.GetString(reader.GetOrdinal("Message"))
                                };
                            }
                            else
                            {
                                return new DbResponse
                                {
                                    StatusCode = 500,
                                    Message = "No response from stored procedure."
                                };
                            }
                        }
                    }
                }
            }
            catch (SqlException sx)
            {
                Logger.Error($"SQL Exception occurred in AddLinkedModules, Error: {sx.Message}");
                throw;
            }
            catch (Exception ex)
            {
                Logger.Error($"Exception occurred in AddLinkedModules, Error: {ex.Message}");
                throw;
            }
        }

        public DbResponse AddReLinkedModule(LinkModule linkModule, int unitId, int userId, int stageId)
        {
            string sql = "AddReLinkedModule";
            try
            {
                using (var connection = new SqlConnection(bwDb))
                {
                    connection.Open();
                    using (var command = new SqlCommand(sql, connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;

                        command.Parameters.Add("@UnitId", SqlDbType.Int).Value = unitId;
                        command.Parameters.Add("@UserId", SqlDbType.Int).Value = userId;
                        command.Parameters.Add("@StageId", SqlDbType.Int).Value = stageId;
                        command.Parameters.Add("@LinkModuleId", SqlDbType.Int).Value = linkModule.LinkModuleId;
                        command.Parameters.Add("@ModuleName", SqlDbType.Int).Value = linkModule.ModuleName;
                        command.Parameters.Add("@ModuleSerialNumber", SqlDbType.Int).Value = linkModule.ScannedModuleSerial;
                        command.Parameters.Add("@SetSerialNumber", SqlDbType.Int).Value = linkModule.SetSerialNumber;

                        using (var reader = command.ExecuteReader())
                        {
                            if (reader.HasRows)
                            {
                                reader.Read();
                                return new DbResponse
                                {
                                    StatusCode = reader.GetInt32(reader.GetOrdinal("StatusCode")),
                                    Message = reader.GetString(reader.GetOrdinal("Message"))
                                };
                            }
                            else
                            {
                                return new DbResponse
                                {
                                    StatusCode = 500,
                                    Message = "No response from stored procedure."
                                };
                            }
                        }
                    }
                }
            }
            catch (SqlException sx)
            {
                Logger.Error($"SQL Exception occurred in AddReLinkedModule, Error: {sx.Message}");
                throw;
            }
            catch (Exception ex)
            {
                Logger.Error($"Exception occurred in AddReLinkedModule, Error: {ex.Message}");
                throw;
            }
        }

        public DbResponse AddDespatchedUnits(string jsonUnitsToSave, int userId, int stageId, string shippingReference)
        {
            string sql = "AddDespatchedUnits";
            try
            {
                using (var connection = new SqlConnection(bwDb))
                {
                    connection.Open();
                    using (var command = new SqlCommand(sql, connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.Add("@JsonData", SqlDbType.NVarChar).Value = jsonUnitsToSave;
                        command.Parameters.Add("@UserId", SqlDbType.Int).Value = userId;
                        command.Parameters.Add("@StageId", SqlDbType.Int).Value = stageId;
                        command.Parameters.Add("@ShippingReference", SqlDbType.VarChar, 255).Value = shippingReference;


                        using (var reader = command.ExecuteReader())
                        {
                            if (reader.HasRows)
                            {
                                reader.Read();
                                return new DbResponse
                                {
                                    StatusCode = reader.GetInt32(reader.GetOrdinal("StatusCode")),
                                    Message = reader.GetString(reader.GetOrdinal("Message"))
                                };
                            }
                            else
                            {
                                return new DbResponse
                                {
                                    StatusCode = 500,
                                    Message = "No response from stored procedure."
                                };
                            }
                        }
                    }
                }
            }
            catch (SqlException sx)
            {
                Logger.Error($"SQL Exception occurred in AddDespatchedUnits, Error: {sx.Message}");
                throw;
            }
            catch (Exception ex)
            {
                Logger.Error($"Exception occurred in AddDespatchedUnits, Error: {ex.Message}");
                throw;
            }
        }

        public DbResponse AddDespatchedPCBAs(string jsonUnitsToSave, int userId, int stageId)
        {
            string sql = "AddDespatchedPCBAs";
            try
            {
                using (var connection = new SqlConnection(bwDb))
                {
                    connection.Open();
                    using (var command = new SqlCommand(sql, connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.Add("@JsonData", SqlDbType.NVarChar).Value = jsonUnitsToSave;
                        command.Parameters.Add("@UserId", SqlDbType.Int).Value = userId;
                        command.Parameters.Add("@StageId", SqlDbType.Int).Value = stageId;


                        using (var reader = command.ExecuteReader())
                        {
                            if (reader.HasRows)
                            {
                                reader.Read();
                                return new DbResponse
                                {
                                    StatusCode = reader.GetInt32(reader.GetOrdinal("StatusCode")),
                                    Message = reader.GetString(reader.GetOrdinal("Message"))
                                };
                            }
                            else
                            {
                                return new DbResponse
                                {
                                    StatusCode = 500,
                                    Message = "No response from stored procedure."
                                };
                            }
                        }
                    }
                }
            }
            catch (SqlException sx)
            {
                Logger.Error($"SQL Exception occurred in AddDespatchedPCBAs, Error: {sx.Message}");
                throw;
            }
            catch (Exception ex)
            {
                Logger.Error($"Exception occurred in AddDespatchedPCBAs, Error: {ex.Message}");
                throw;
            }
        }


        public async Task<CustomerOrder> GetCustomerOrderByOdfAsync(string odf)
        {
            try
            {
                using (var connection = new SqlConnection(bwDb))
                {
                    await connection.OpenAsync();
                    using (var command = new SqlCommand("GetCustomerOrderByODF", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("@odf", odf);

                        using (var reader = await command.ExecuteReaderAsync())
                        {

                            if (await reader.ReadAsync())  // Returns false if no rows
                            {
                                return MapCustomerOrder(reader);
                            }
                            return null;
                        }
                    }
                }
            }
            catch (SqlException sx)
            {
                Logger.Error($"SQL Exception occurred in GetCustomerOrderByOdfAsync, Error: {sx.Message}");
                throw;
            }
            catch (Exception ex)
            {
                Logger.Error($"Exception occurred in GetCustomerOrderByOdfAsync, Error: {ex.Message}");
                throw;
            }
        }

        public async Task<List<BomItem>> GetBomByProductIdAsync(int productId, bool? isActive = null)
        {
            try
            {
                using (var connection = new SqlConnection(bwDb))
                {
                    await connection.OpenAsync();
                    using (var command = new SqlCommand("GetBomByProductId", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("@productId", productId);
                        if (isActive.HasValue)
                        {
                            command.Parameters.Add("@IsActive", SqlDbType.Bit).Value = isActive.HasValue ? (object)isActive.Value : DBNull.Value;
                        }
                        else
                        {
                            command.Parameters.AddWithValue("@IsActive", DBNull.Value);
                        }

                        using (var reader = await command.ExecuteReaderAsync())
                        {

                            var bomItems = new List<BomItem>();
                            while (await reader.ReadAsync())
                            {
                                bomItems.Add(MapBomItem(reader));
                            }
                            return bomItems;
                        }
                    }
                }
            }
            catch (SqlException sx)
            {
                Logger.Error($"SQL Exception occurred in GetCustomerOrderByOdfAsync, Error: {sx.Message}");
                throw;
            }
            catch (Exception ex)
            {
                Logger.Error($"Exception occurred in GetCustomerOrderByOdfAsync, Error: {ex.Message}");
                throw;
            }
        }

        private BomItem MapBomItem(SqlDataReader reader)
        {
            return new BomItem
            {
                ProductId = reader.GetInt32(reader.GetOrdinal("ProductId")),
                ComponentPartNumber = reader.GetString(reader.GetOrdinal("ComponentPartNumber")),
                ComponentDescription = reader.GetString(reader.GetOrdinal("ComponentDescription")),
                //UsageQuantity = reader.GetInt32(reader.GetOrdinal("UsageQuantity")),
                Position = reader.GetString(reader.GetOrdinal("position")),
                IsActive = reader.GetBoolean(reader.GetOrdinal("IsActive"))
            };
        }
         
        public DbResponse AddLinkedAccessories(string jsonAccessoriesToSave, int unitId, int userId, int stageId)
        {
            string sql = "AddLinkedAccessories";
            try
            {
                using (var connection = new SqlConnection(bwDb))
                {
                    connection.Open();
                    using (var command = new SqlCommand(sql, connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("@JsonData", jsonAccessoriesToSave);
                        command.Parameters.AddWithValue("@UnitId", unitId);
                        command.Parameters.AddWithValue("@UserId", userId);
                        command.Parameters.AddWithValue("@StageId", stageId);

                        using (var reader = command.ExecuteReader())
                        {
                            if (reader.HasRows)
                            {
                                reader.Read();

                                return new DbResponse
                                {
                                    StatusCode = reader.GetInt32(reader.GetOrdinal("StatusCode")),
                                    Message = reader.GetString(reader.GetOrdinal("Message"))
                                };
                            }
                            else
                            {
                                return new DbResponse
                                {
                                    StatusCode = 500,
                                    Message = "No response from stored procedure."
                                };
                            }
                        }
                    }
                }
            }
            catch (SqlException sx)
            {
                Logger.Error($"SQL Exception occurred in AddLinkedAccessories, Error: {sx.Message}");
                throw;
            }
            catch (Exception ex)
            {
                Logger.Error($"Exception occurred in SaveLinkedAccessories, Error: {ex.Message}");
                throw;
            }
        }

        public async Task<List<RepairType>> GetRepairTypesAsync()
        {
            try
            {
                using (var connection = new SqlConnection(bwDb))
                {
                    await connection.OpenAsync();
                    using (var command = new SqlCommand("GetRepairTypes", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;

                        using (var reader = await command.ExecuteReaderAsync())
                        {
                            var repairTypes = new List<RepairType>();
                            while (await reader.ReadAsync())
                            {
                                repairTypes.Add(new RepairType
                                {
                                    RepairTypeId = reader.GetInt32(reader.GetOrdinal("RepairTypeId")),
                                    TestTypeId = reader.GetInt32(reader.GetOrdinal("TestTypeId")),
                                    RepairDescription = reader.GetString(reader.GetOrdinal("RepairDescription")),
                                    IsActive = reader.GetBoolean(reader.GetOrdinal("IsActive"))
                                });
                            }
                            return repairTypes;
                        }
                    }
                }
            }
            catch (SqlException sx)
            {
                Logger.Error($"Exception happened in GetRepairTypesAsync. Error: {sx.Message}");
                throw;
            }
            catch (Exception ex)
            {
                Logger.Error($"Exception happened in GetRepairTypesAsync, Error: {ex.Message}");
                throw;
            }
        }

        public async Task<DataTable> GetUnitHistoryBySerialAsync(string serialNumber)
        {
            var parameters = new SqlParameter[]
            {
                new SqlParameter("@SerialNumber", serialNumber),
            };
            return await ExecuteStoredProcedureAsync("GetUnitHistoryBySerial", parameters);
        }

        public async Task<DataTable> GetAoiTestHistoryBySerialAsync(string serialNumber)
        {
            var parameters = new SqlParameter[]
            {
                new SqlParameter("@SerialNumber", serialNumber),
            };
            return await ExecuteStoredProcedureAsync("GetAoiTestHistoryBySerial", parameters);
        }

        public async Task<DataTable> GetPcbaTestHistoryBySerialAsync(string serialNumber)
        {
            var parameters = new SqlParameter[]
            {
                new SqlParameter("@SerialNumber", serialNumber),
            };
            return await ExecuteStoredProcedureAsync("GetPcbaTestHistoryBySerial", parameters);
        }

        public async Task<DataTable> GetLinkedModulesBySerialAsync(string serialNumber)
        {
            var parameters = new SqlParameter[]
            {
                new SqlParameter("@SerialNumber", serialNumber),
            };
            return await ExecuteStoredProcedureAsync("GetLinkedModulesBySerial", parameters);
        }

        //public async Task<List<LinkModule>> GetLinkedModuleListBySerialAsync(string serialNumber)
        //{
        //    List<LinkModule> modules = new List<LinkModule>();
        //    try
        //    {
        //        using (var connection = new SqlConnection(bwDb))
        //        {
        //            await connection.OpenAsync();
        //            using (var command = new SqlCommand("GetLinkedModulesBySerial", connection))
        //            {
        //                command.CommandType = CommandType.StoredProcedure;
        //                command.Parameters.Add("@SerialNumber", SqlDbType.VarChar, 60).Value = serialNumber;

        //                using (var reader = await command.ExecuteReaderAsync())
        //                {
        //                    if (reader.HasRows)
        //                    {
        //                        while (reader.Read())
        //                        {
        //                            var linkedModule = new LinkModule
        //                            {
        //                                Id = reader.GetInt32(reader.GetOrdinal("Id")),
        //                                LinkModuleId = reader.GetInt32(reader.GetOrdinal("LinkModuleId")),
        //                                ProductId = reader.GetInt32(reader.GetOrdinal("ProductId")),
        //                                ModuleName = reader.GetString(reader.GetOrdinal("ModuleName")),
        //                                PartNumber = reader.GetString(reader.GetOrdinal("PartNumber")),
        //                                IsActive = reader.GetBoolean(reader.GetOrdinal("IsActive")),
        //                                ScannedModuleSerial = reader.GetString(reader.GetOrdinal("ModuleSerialNumber"))
        //                            };
        //                            modules.Add(linkedModule);
        //                        }
        //                        return modules;
        //                    }
        //                    return null;
        //                }
        //            }
        //        }
        //    }
        //    catch (SqlException sx)
        //    {
        //        Logger.Error($"SQL Exception occurred in GetLinkedModuleListBySerialAsync, Error: {sx.Message}");
        //        throw;
        //    }
        //    catch (Exception ex)
        //    {
        //        Logger.Error($"Exception occurred in GetLinkedModuleListBySerialAsync, Error: {ex.Message}");
        //        throw;
        //    }
        //}

        //public async Task<List<LinkAccessory>> GetActiveLinkedAccessoryListBySerialAsync(string serialNumber)
        //{
        //    List<LinkAccessory> accessories = new List<LinkAccessory>();
        //    try
        //    {
        //        using (var connection = new SqlConnection(bwDb))
        //        {
        //            await connection.OpenAsync();
        //            using (var command = new SqlCommand("GetLinkedAccessoriesBySerial", connection))
        //            {
        //                command.CommandType = CommandType.StoredProcedure;
        //                command.Parameters.Add("@SerialNumber", SqlDbType.VarChar, 60).Value = serialNumber;

        //                using (var reader = await command.ExecuteReaderAsync())
        //                {
        //                    if (reader.HasRows)
        //                    {
        //                        while (reader.Read())
        //                        {
        //                            var linkedModule = new LinkAccessory
        //                            {
        //                                //LinkAccessoryId = reader.GetInt32(reader.GetOrdinal("LinkAccessoryId")),
        //                                PartName = reader.GetString(reader.GetOrdinal("PartName")),
        //                                //ProductId = reader.GetInt32(reader.GetOrdinal("ProductId")),
        //                                SerialNumber = reader.GetString(reader.GetOrdinal("SerialNumber")),
        //                                PartNumber = reader.GetString(reader.GetOrdinal("PartNumber")),
        //                                IsActive = reader.GetBoolean(reader.GetOrdinal("IsActive"))
        //                            };
        //                            accessories.Add(linkedModule);
        //                        }
        //                        return accessories;
        //                    }
        //                    return null;
        //                }
        //            }
        //        }
        //    }
        //    catch (SqlException sx)
        //    {
        //        Logger.Error($"SQL Exception occurred in GetLinkedModuleListBySerialAsync, Error: {sx.Message}");
        //        throw;
        //    }
        //    catch (Exception ex)
        //    {
        //        Logger.Error($"Exception occurred in GetLinkedModuleListBySerialAsync, Error: {ex.Message}");
        //        throw;
        //    }
        //}

        //public DbResponse UpdateLinkedModule(LinkModule linkModule, Unit unit, int userId)
        //{

        //    try
        //    {
        //        using (var connection = new SqlConnection(bwDb))
        //        {
        //            connection.Open();
        //            using (var command = new SqlCommand("UpdateLinkedModule", connection))
        //            {
        //                command.CommandType = CommandType.StoredProcedure;
        //                command.Parameters.Add("@Id", SqlDbType.Int).Value = linkModule.Id;
        //                command.Parameters.Add("@ModuleSerialNumber", SqlDbType.VarChar, 50).Value = linkModule.ScannedModuleSerial;
        //                command.Parameters.Add("@UnitId", SqlDbType.Int).Value = unit.UnitId;
        //                command.Parameters.Add("@UserId", SqlDbType.Int).Value = userId;
        //                command.Parameters.Add("@StageId", SqlDbType.Int).Value = unit.StageId;


        //                using (var reader = command.ExecuteReader())
        //                {
        //                    if (reader.HasRows)
        //                    {
        //                        reader.Read();
        //                        return new DbResponse
        //                        {
        //                            StatusCode = reader.GetInt32(reader.GetOrdinal("StatusCode")),
        //                            Message = reader.GetString(reader.GetOrdinal("Message"))
        //                        };
        //                    }
        //                    else
        //                    {
        //                        return new DbResponse
        //                        {
        //                            StatusCode = 500,
        //                            Message = "No response from stored procedure."
        //                        };
        //                    }
        //                }
        //            }
        //        }
        //    }
        //    catch (SqlException sx)
        //    {
        //        Logger.Error($"SQL Exception occurred in AddDespatchedUnits, Error: {sx.Message}");
        //        throw;
        //    }
        //    catch (Exception ex)
        //    {
        //        Logger.Error($"Exception occurred in AddDespatchedUnits, Error: {ex.Message}");
        //        throw;
        //    }
        //}

        public async Task<DataTable> GetLinkedAccessoriesBySerialAsync(string serialNumber)
        {
            var parameters = new SqlParameter[]
            {
                new SqlParameter("@SerialNumber", serialNumber),
            };
            return await ExecuteStoredProcedureAsync("GetLinkedAccessoriesBySerial", parameters);
        }

        public async Task<DataTable> GetFaTestHistoryBySerialAsync(string serialNumber)
        {
            var parameters = new SqlParameter[]
            {
                new SqlParameter("@SerialNumber", serialNumber),
            };
            return await ExecuteStoredProcedureAsync("GetFaTestHistoryBySerial", parameters);
        }

        public async Task<DataTable> GetOqcHistoryBySerialAsync(string serialNumber)
        {
            var parameters = new SqlParameter[]
            {
                new SqlParameter("@SerialNumber", serialNumber),
            };
            return await ExecuteStoredProcedureAsync("GetOqcHistoryBySerial", parameters);
        }

        public async Task<DataTable> GetRepairHistoryBySerialAsync(string serialNumber)
        {
            var parameters = new SqlParameter[]
            {
                new SqlParameter("@SerialNumber", serialNumber),
            };
            return await ExecuteStoredProcedureAsync("GetRepairHistoryBySerial", parameters);
        }


        private async Task<DataTable> ExecuteStoredProcedureAsync(string procedureName, params SqlParameter[] parameters)
        {
            try
            {
                using (var connection = new SqlConnection(bwDb))
                {
                    await connection.OpenAsync();

                    using (var command = new SqlCommand(procedureName, connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddRange(parameters);

                        using (var reader = await command.ExecuteReaderAsync())
                        {
                            var dataTable = new DataTable();
                            dataTable.Load(reader);
                            return dataTable;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.Error($"Exception occurred in ExecuteStoredProcedureAsync, Procedure: {procedureName}, Error: {ex.Message}");
                throw;
            }

        }

        public async Task<List<Defect>> GetDefectsByTestTypeIdAsync(int? testTypeId = null, bool? isActive = null)
        {
            List<Defect> defects = new List<Defect>();
            try
            {
                using (var connection = new SqlConnection(bwDb))
                {
                    await connection.OpenAsync();
                    using (var command = new SqlCommand("GetDefectsByTestTypeId", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;

                        if (testTypeId.HasValue)
                        {
                            command.Parameters.AddWithValue("@TestTypeId", testTypeId.Value);
                        }
                        else
                        {
                            command.Parameters.AddWithValue("@TestTypeId", DBNull.Value);
                        }

                        if (isActive.HasValue)
                        {
                            command.Parameters.AddWithValue("@IsActive", isActive.Value ? 1 : 0);
                        }
                        else
                        {
                            command.Parameters.AddWithValue("@IsActive", DBNull.Value);
                        }

                        using (var reader = await command.ExecuteReaderAsync())
                        {
                            if (reader.HasRows)
                            {
                                while (await reader.ReadAsync())
                                {
                                    //defects.Add(new Defect
                                    //{
                                    //    DefectId = reader.GetInt32(reader.GetOrdinal("DefectId")),
                                    //    DefectGroupId = reader.GetInt32(reader.GetOrdinal("DefectGroupId")),
                                    //    DefectName = reader.GetString(reader.GetOrdinal("DefectName")),
                                    //    IsActive = reader.GetBoolean(reader.GetOrdinal("IsActive"))
                                    //});
                                    var defect = new Defect();
                                    defect.DefectId = reader.GetInt32(reader.GetOrdinal("DefectId"));
                                    defect.TestTypeId = reader.GetInt32(reader.GetOrdinal("TestTypeId"));
                                    defect.DefectName = reader.GetString(reader.GetOrdinal("DefectName"));
                                    defect.IsActive = reader.GetBoolean(reader.GetOrdinal("IsActive"));
                                    defects.Add(defect);
                                }
                                return defects;

                            }
                        }
                    }
                }
            }
            catch (SqlException sx)
            {
                Logger.Error($"SQL Exception occurred in GetDefectsByDefectGroupIdAsync, Error: {sx.Message}");
                throw;
            }
            catch (Exception ex)
            {
                Logger.Error($"Exception occurred in GetDefectsByDefectGroupIdAsync, Error: {ex.Message}");
                throw;
            }
            return defects;
        }

        public List<Repair> GetRepairDataByUnitIdAndStage(Unit unit)
        {
            List<Repair> repairs = new List<Repair>();
            try
            {
                using (var connection = new SqlConnection(bwDb))
                {
                    connection.Open();

                    var query = @"GetRepairDataByUnitIdAndStage";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@UnitId", unit.UnitId);
                        command.Parameters.AddWithValue("@StageId", unit.StageId);
                        command.CommandType = CommandType.StoredProcedure;

                        var reader = command.ExecuteReader();

                        while (reader.Read())
                        {
                            repairs.Add(
                                new Repair()
                                {
                                    RepairId = reader.GetInt32(reader.GetOrdinal("RepairId")),
                                    UnitId = reader.GetInt32(reader.GetOrdinal("UnitId")),
                                    TestResultId = reader.GetInt32(reader.GetOrdinal("TestResultId")),
                                    RepairDescription = reader.GetString(reader.GetOrdinal("RepairDescription")),
                                    Comment = reader.GetString(reader.GetOrdinal("Comment")),
                                    RepairDate = reader.GetDateTime(reader.GetOrdinal("RepairDate")),
                                    UserId = reader.GetInt32(reader.GetOrdinal("UserId"))
                                });
                        }
                        return repairs;
                    }
                }
            }
            catch (SqlException sx)
            {
                Logger.Error($"SQL Exception occurred in GetRepairDataByUnitIdAndStage, UnitId: {unit.UnitId}, Error: {sx.Message}");
                throw sx;
            }
            catch (Exception ex)
            {
                Logger.Error($"Exception occurred in GetRepairDataByUnitIdAndStage, UnitId: {unit.UnitId}, Error: {ex.Message}");
                throw ex;
            }
        }

        public List<TestResult> GetTestResultsByUnitId(int unitId)
        {
            try
            {
                using (var connection = new SqlConnection(bwDb))
                {
                    connection.Open();

                    using (SqlCommand command = new SqlCommand("GetTestResultsByUnitIdJson", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("@UnitId", unitId);

                        // Use ExecuteReader for large JSON results
                        using (var reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                var jsonResult = reader[0].ToString();

                                if (!string.IsNullOrEmpty(jsonResult))
                                {
                                    return JsonConvert.DeserializeObject<List<TestResult>>(jsonResult);
                                }
                            }
                        }

                        return new List<TestResult>();
                    }
                }
            }
            catch (SqlException sx)
            {
                Logger.Error($"SQL Exception occurred in GetTestResultsByUnitId, UnitId: {unitId}, Error: {sx.Message}");
                throw;
            }
            catch (Exception ex)
            {
                Logger.Error($"Exception occurred in GetTestResultsByUnitId, UnitId: {unitId}, Error: {ex.Message}");
                throw;
            }
        }

        //public List<TestResult> GetTestResultsByUnitId(int unitId)
        //{
        //    List<TestResult> repairs = new List<TestResult>();
        //    try
        //    {
        //        using (var connection = new SqlConnection(bwDb))
        //        {
        //            connection.Open();

        //            var query = @"GetTestResultsByUnitId";

        //            using (SqlCommand command = new SqlCommand(query, connection))
        //            {
        //                command.CommandType = CommandType.StoredProcedure;
        //                command.Parameters.AddWithValue("@UnitId", unitId);


        //                var reader = command.ExecuteReader();

        //                while (reader.Read())
        //                {
        //                    repairs.Add(
        //                        new TestResult()
        //                        {
        //                            TestResultId = reader.GetInt32(reader.GetOrdinal("TestResultId")),
        //                            TestTypeId = reader.GetInt32(reader.GetOrdinal("TestTypeId")),
        //                            UnitId = reader.GetInt32(reader.GetOrdinal("UnitId")),
        //                            TestDate = reader.GetDateTime(reader.GetOrdinal("TestDate")),
        //                            StageId = reader.GetInt32(reader.GetOrdinal("StageId")),
        //                            IsPass = reader.GetBoolean(reader.GetOrdinal("IsPass")),
        //                            DefectDescription = reader.GetString(reader.GetOrdinal("DefectDescription")),
        //                            DefectId = reader.GetInt32(reader.GetOrdinal("DefectId")),
        //                            Position = reader.GetString(reader.GetOrdinal("Position")),
        //                            IsRepaired = reader.GetBoolean(reader.GetOrdinal("IsRepaired")),
        //                            UserId = reader.GetInt32(reader.GetOrdinal("UserId"))
        //                        });
        //                }
        //                return repairs;
        //            }
        //        }
        //    }
        //    catch (SqlException sx)
        //    {
        //        Logger.Error($"SQL Exception occurred in GetRepairDataByUnitIdAndStage, UnitId: {unitId}, Error: {sx.Message}");
        //        throw sx;
        //    }
        //    catch (Exception ex)
        //    {
        //        Logger.Error($"Exception occurred in GetRepairDataByUnitIdAndStage, UnitId: {unitId}, Error: {ex.Message}");
        //        throw ex;
        //    }
        //}


        public async Task<QcInspectionBatchStatus> GetQcInspectionStatusOfBatchAsync(int cutomerOrderId)
        {
            try
            {
                using (var connection = new SqlConnection(bwDb))
                {
                    await connection.OpenAsync();
                    using (var command = new SqlCommand("GetQcInspectionStatusOfBatch", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.Add("@CustomerOrderId", SqlDbType.Int).Value = cutomerOrderId;



                        using (var reader = await command.ExecuteReaderAsync())
                        {
                            if (reader.HasRows)
                            {
                                await reader.ReadAsync();
                                var status = new QcInspectionBatchStatus();

                                status.CustomerOrderId = reader.GetInt32(reader.GetOrdinal("CustomerOrderId"));
                                status.Odf = reader.GetString(reader.GetOrdinal("ODF"));
                                status.InspectionLevel = reader.GetInt32(reader.GetOrdinal("InspectionLevel"));
                                status.BatchSize = reader.GetInt32(reader.GetOrdinal("BatchSize"));
                                status.SampleSize = reader.GetInt32(reader.GetOrdinal("SampleSize"));
                                status.SampledCount = reader.GetInt32(reader.GetOrdinal("SampledCount"));
                                status.DefectCount = reader.GetInt32(reader.GetOrdinal("DefectCount"));
                                status.MajorDefectLimit = reader.GetInt32(reader.GetOrdinal("MajorDefectLimit"));
                                status.MinorDefectLimit = reader.GetInt32(reader.GetOrdinal("MinorDefectLimit"));
                                status.CriticalDefectLimit = reader.GetInt32(reader.GetOrdinal("CriticalDefectLimit"));
                                return status;

                            }
                        }
                    }
                }
            }
            catch (SqlException sx)
            {
                Logger.Error($"SQL Exception occurred in GetDefectsByDefectGroupIdAsync, Error: {sx.Message}");
                throw;
            }
            catch (Exception ex)
            {
                Logger.Error($"Exception occurred in GetDefectsByDefectGroupIdAsync, Error: {ex.Message}");
                throw;
            }
            return null;

        }

        public async Task<List<QcCheckListItem>> GetQcCheckListByInspectionTypeIdAsync(int inspectionTypeId, bool? isActive = null)
        {
            List<QcCheckListItem> checkListItems = new List<QcCheckListItem>();
            try
            {
                using (var connection = new SqlConnection(bwDb))
                {
                    await connection.OpenAsync();
                    using (var command = new SqlCommand("GetQcCheckListByInspectionTypeId", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.Add("@QcInspectionTypeId", SqlDbType.Int).Value = inspectionTypeId;
                        if (isActive.HasValue)
                        {
                            command.Parameters.Add("@IsActive", SqlDbType.Bit).Value = isActive.Value ? 1 : 0;
                        }
                        else
                        {
                            command.Parameters.Add("@IsActive", SqlDbType.Bit).Value = DBNull.Value;
                        }

                        using (var reader = await command.ExecuteReaderAsync())
                        {
                            while (await reader.ReadAsync())
                            {
                                checkListItems.Add(new QcCheckListItem
                                {
                                    QcCheckListItemId = reader.GetInt32(reader.GetOrdinal("QcCheckListItemId")),
                                    QcInspectionTypeId = reader.GetInt32(reader.GetOrdinal("QcInspectionTypeId")),
                                    InspectionTypeName = reader.GetString(reader.GetOrdinal("InspectionTypeName")),
                                    SequenceNumber = reader.GetInt32(reader.GetOrdinal("SequenceNumber")),
                                    ItemName = reader.GetString(reader.GetOrdinal("ItemName")),
                                    AcceptanceCriteria = reader.GetString(reader.GetOrdinal("AcceptanceCriteria")),
                                    DateCreated = reader.GetDateTime(reader.GetOrdinal("DateCreated")),
                                    DateModified = reader.GetDateTime(reader.GetOrdinal("DateModified")),
                                    ModifiedBy = reader.GetInt32(reader.GetOrdinal("ModifiedBy")),
                                    ModifiedByName = reader.GetString(reader.GetOrdinal("ModifiedByName")),
                                    IsActive = reader.GetBoolean(reader.GetOrdinal("IsActive"))

                                });
                            }
                            return checkListItems;
                        }
                    }
                }
            }
            catch (SqlException sx)
            {
                Logger.Error($"SQL Exception occurred in GetQcCheckListItems, Error: {sx.Message}");
                throw;
            }
            catch (Exception ex)
            {
                Logger.Error($"Exception occurred in GetQcCheckListItems, Error: {ex.Message}");
                throw;
            }

        }

        public DbResponse AddQcInspectionResults(string jsonResults, int stageId, int userId)
        {
            try
            {
                using (var connection = new SqlConnection(bwDb))
                {
                    connection.Open();
                    using (var command = new SqlCommand("AddQcInspectionResults", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.Add("@jsonResults", SqlDbType.NVarChar).Value = jsonResults;
                        command.Parameters.Add("@StageId", SqlDbType.Int).Value = stageId;
                        command.Parameters.Add("@UserId", SqlDbType.Int).Value = userId;

                        using (var reader = command.ExecuteReader())
                        {
                            if (reader.HasRows)
                            {
                                reader.Read();
                                return new DbResponse
                                {
                                    StatusCode = reader.GetInt32(reader.GetOrdinal("StatusCode")),
                                    Message = reader.GetString(reader.GetOrdinal("Message"))
                                };
                            }
                            else
                            {
                                return new DbResponse
                                {
                                    StatusCode = 500,
                                    Message = "No response from stored procedure."
                                };
                            }
                        }
                    }
                }
            }
            catch (SqlException sx)
            {
                Logger.Error($"SQL Exception occurred in AddQcInspectionResults, Error: {sx.Message}");
                throw;
            }
            catch (Exception ex)
            {
                Logger.Error($"Exception occurred in AddQcInspectionResults, Error: {ex.Message}");
                throw;
            }
        }

        public async Task<List<Report>> GetReportsAsync(bool? isActive = null)
        {
            try
            {
                using (var connection = new SqlConnection(bwDb))
                {
                    await connection.OpenAsync();

                    using (var cmd = new SqlCommand("GetReports", connection))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        if (isActive.HasValue)
                            cmd.Parameters.Add("@IsActive", SqlDbType.Bit).Value = isActive.Value;
                        else
                            cmd.Parameters.Add("@IsActive", SqlDbType.Bit).Value = DBNull.Value;

                        var items = new List<Report>();

                        using (var reader = await cmd.ExecuteReaderAsync())
                        {
                            while (await reader.ReadAsync())
                            {
                                items.Add(MapReport(reader));
                            }
                        }

                        return items;
                    }
                }
            }
            catch (SqlException sx)
            {
                Logger.Error($"Exception occurred in GetReportsAsync, Error: {sx.Message}");
                throw sx;
            }
            catch (Exception ex)
            {
                Logger.Error($"Exception occurred in GetReportsAsync, Error: {ex.Message}");
                throw ex;
            }
        }


        private Report MapReport(SqlDataReader reader)
        {
            return new Report()
            {
                ReportId = reader.GetInt32(reader.GetOrdinal("ReportId")),
                ReportName = reader.GetString(reader.GetOrdinal("ReportName")),
                SPName = reader.GetString(reader.GetOrdinal("SPName")),
                RequiresProductId = reader.GetBoolean(reader.GetOrdinal("RequiresProductId")),
                RequiresCustomerOrderId = reader.GetBoolean(reader.GetOrdinal("RequiresCustomerOrderId")),
                RequiresDateRange = reader.GetBoolean(reader.GetOrdinal("RequiresDateRange")),
                RequiresStageId = reader.GetBoolean(reader.GetOrdinal("RequiresStageId")),
                IsActive = reader.GetBoolean(reader.GetOrdinal("IsActive"))

            };
        }

        public async Task<DataTable> GetReportAsync(ReportParams reportParams)
        {
            try
            {
                using (var connection = new SqlConnection(bwDb))
                {
                    await connection.OpenAsync();

                    using (var cmd = new SqlCommand(reportParams.SPName, connection))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;


                        cmd.Parameters.Add("@StartDate", SqlDbType.DateTime2, 7).Value = reportParams.StartDate;
                        cmd.Parameters.Add("@EndDate", SqlDbType.DateTime2, 7).Value = reportParams.EndDate;
                        cmd.Parameters.Add("@ProductId", SqlDbType.Int).Value = reportParams.ProductId.HasValue ? (object)reportParams.ProductId.Value : DBNull.Value;
                        cmd.Parameters.Add("@CustomerOrderId", SqlDbType.Int).Value = reportParams.CustomerOrderId.HasValue ? (object)reportParams.CustomerOrderId.Value : DBNull.Value;
                        cmd.Parameters.Add("@StageId", SqlDbType.Int).Value = reportParams.StageId.HasValue ? (object)reportParams.StageId.Value : DBNull.Value;


                        var dt = new DataTable();


                        using (var adapter = new SqlDataAdapter(cmd))
                        {
                            adapter.Fill(dt);
                        }

                        return dt;
                    }
                }
            }
            catch (SqlException sx)
            {
                Logger.Error($"Exception occurred in GetReportAsync, Error: {sx.Message}");
                throw sx;
            }
            catch (Exception ex)
            {
                Logger.Error($"Exception occurred in GetReportAsync, Error: {ex.Message}");
                throw ex;
            }
        }

        public async Task<List<SerialPrintOrder>> GetOrdersForPrintAsync()
        {
            try
            {
                using (var connection = new SqlConnection(bwDb))
                {
                    await connection.OpenAsync();

                    using (var cmd = new SqlCommand("GetCustomerOrdersForPrinting", connection))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        var items = new List<SerialPrintOrder>();

                        using (var reader = await cmd.ExecuteReaderAsync())
                        {
                            while (await reader.ReadAsync())
                            {
                                items.Add(new SerialPrintOrder()
                                {
                                    CustomerOrderId = reader.GetInt32(reader.GetOrdinal("CustomerOrderId")),
                                    Odf = reader.GetString(reader.GetOrdinal("ODF")),
                                    Model = reader.GetString(reader.GetOrdinal("Model")),
                                    QuantityToPrint = reader.GetInt32(reader.GetOrdinal("QuantityToPrint"))
                                });
                            }
                        }

                        return items;
                    }
                }
            }
            catch (SqlException sx)
            {
                Logger.Error($"Exception occurred in GetOrdersForPrintAsync, Error: {sx.Message}");
                throw;
            }
            catch (Exception ex)
            {
                Logger.Error($"Exception occurred in GetOrdersForPrintAsync, Error: {ex.Message}");
                throw;
            }
        }

        public SerialPrintResponse AddPrintRange(int customerOrderId, int printQuantity, int userId)
        {
            const string methodName = nameof(AddPrintRange);

            try
            {
                using (var connection = new SqlConnection(bwDb))
                using (var command = new SqlCommand("AddPrintRangeForCustomerOrder", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@CustomerOrderId", customerOrderId);
                    command.Parameters.AddWithValue("@PrintQuantity", printQuantity);
                    command.Parameters.AddWithValue("@UserId", userId);

                    connection.Open();


                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            var statusCode = reader.GetInt32(reader.GetOrdinal("StatusCode"));
                            var message = reader.GetString(reader.GetOrdinal("Message"));

                            if (statusCode != 0)
                            {
                                Logger.Error($"Print range failed: {message}");
                                return new SerialPrintResponse
                                {
                                    StatusCode = statusCode,
                                    Message = message,
                                    PrintRange = null // or new SerialPrintRange() with all defaults if preferred
                                };
                            }

                            // Only read PrintRange if StatusCode is 0 (success)
                            var printRange = new SerialPrintRange
                            {
                                PrintID = reader.GetInt32(reader.GetOrdinal("PrintId")),
                                CustomerOrderId = reader.IsDBNull(reader.GetOrdinal("CustomerOrderId")) ? -1 : reader.GetInt32(reader.GetOrdinal("CustomerOrderId")),
                                Odf = reader.IsDBNull(reader.GetOrdinal("Odf")) ? null : reader.GetString(reader.GetOrdinal("Odf")),
                                Pid = reader.IsDBNull(reader.GetOrdinal("Pid")) ? null : reader.GetString(reader.GetOrdinal("Pid")),
                                Model = reader.IsDBNull(reader.GetOrdinal("Model")) ? null : reader.GetString(reader.GetOrdinal("Model")),
                                DateCode = reader.IsDBNull(reader.GetOrdinal("DateCode")) ? null : reader.GetString(reader.GetOrdinal("DateCode")),
                                FirstSerial = reader.GetInt32(reader.GetOrdinal("FirstSerial")),
                                LastSerial = reader.GetInt32(reader.GetOrdinal("LastSerial")),
                                PcbaPartNumber = reader.IsDBNull(reader.GetOrdinal("PcbaPartNumber")) ? null : reader.GetString(reader.GetOrdinal("PcbaPartNumber")),
                                FirstFullSerialNumber = reader.GetString(reader.GetOrdinal("FirstFullSerialNumber")),
                                LastFullSerialNumber = reader.GetString(reader.GetOrdinal("LastFullSerialNumber")),
                                DatePrinted = reader.GetDateTime(reader.GetOrdinal("DatePrinted")),
                                UserID = reader.GetInt32(reader.GetOrdinal("UserId")),
                                PrintQuantity = reader.GetInt32(reader.GetOrdinal("PrintQuantity"))
                            };



                            return new SerialPrintResponse
                            {
                                StatusCode = statusCode,
                                Message = message,
                                PrintRange = printRange
                            };
                        }
                    }

                }

                Logger.Error("Stored procedure returned no results");
                return new SerialPrintResponse
                {
                    StatusCode = -1,
                    Message = "No data returned from database",
                    PrintRange = new SerialPrintRange()
                };
            }
            catch (SqlException sqlEx)
            {
                Logger.Error($"Database error in {methodName}: {sqlEx.Number} - {sqlEx.Message}");
                return new SerialPrintResponse
                {
                    StatusCode = -2,
                    Message = $"Database error: {sqlEx.Message}",
                    PrintRange = new SerialPrintRange()
                };
            }
            catch (Exception ex)
            {
                Logger.Error($"Unexpected error in {methodName}. {ex.Message}");
                return new SerialPrintResponse
                {
                    StatusCode = -3,
                    Message = $"Unexpected error: {ex.Message}",
                    PrintRange = new SerialPrintRange()
                };
            }
        }

        public DbResponse AddRepair(Repair repair)
        {
            try
            {
                using (var connection = new SqlConnection(bwDb))
                {
                    connection.Open();
                    using (var command = new SqlCommand("AddRepair", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.Add("@UnitId", SqlDbType.Int).Value = repair.UnitId;
                        command.Parameters.Add("@TestResultId", SqlDbType.Int).Value = repair.TestResultId;
                        command.Parameters.Add("@TestResultDetailId", SqlDbType.Int).Value = repair.TestResultDetailId;
                        command.Parameters.Add("@DefectId", SqlDbType.Int).Value = repair.DefectId;
                        command.Parameters.Add("@RepairDescription", SqlDbType.VarChar, 200).Value = repair.RepairDescription;
                        command.Parameters.Add("@RepairTypeId", SqlDbType.Int).Value = repair.RepairTypeId;
                        command.Parameters.Add("@Comment", SqlDbType.VarChar, 255).Value = repair.Comment;
                        command.Parameters.Add("@UserId", SqlDbType.Int).Value = repair.UserId;
                        command.Parameters.Add("@Position", SqlDbType.VarChar, 20).Value = repair.Position;
                        //Repair date uses sql server time

                        using (var reader = command.ExecuteReader())
                        {
                            if (reader.HasRows)
                            {
                                reader.Read();
                                return new DbResponse
                                {
                                    StatusCode = reader.GetInt32(reader.GetOrdinal("StatusCode")),
                                    Message = reader.GetString(reader.GetOrdinal("Message"))
                                };
                            }
                            else
                            {
                                return new DbResponse
                                {
                                    StatusCode = 500,
                                    Message = "No response from stored procedure."
                                };
                            }
                        }
                    }
                }
            }
            catch (SqlException sx)
            {
                Logger.Error($"SQL Exception occurred in AddRepair, Error: {sx.Message}");
                throw;
            }
            catch (Exception ex)
            {
                Logger.Error($"Exception occurred in AddRepair, Error: {ex.Message}");
                throw;
            }

        }

        public int GetUnitsToBuildByCustomerOrderId(int customerOrderId)
        {
            try
            {
                using (var SqlConnection = new SqlConnection(bwDb))
                {
                    SqlConnection.Open();
                    using (var command = new SqlCommand("GetUnitsToBuildByCustomerOrderId", SqlConnection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("@CustomerOrderId", customerOrderId);

                        var result = command.ExecuteScalar();
                        return result != null ? Convert.ToInt32(result) : 0;
                    }
                }
            }
            catch (SqlException sx)
            {
                Logger.Error($"SQL Exception occurred in GetUnitsToBuildByCustomerOrderId, Error: {sx.Message}");
                throw;
            }
            catch (Exception ex)
            {
                Logger.Error($"Exception occurred in GetUnitsToBuildByCustomerOrderId, Error: {ex.Message}");
                throw;
            }
        }

        public int GetPCBAsToBuildByCustomerOrderId(int customerOrderId)
        {
            try
            {
                using (var SqlConnection = new SqlConnection(bwDb))
                {
                    SqlConnection.Open();
                    using (var command = new SqlCommand("GetPCBAsToBuildByCustomerOrderId", SqlConnection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("@CustomerOrderId", customerOrderId);

                        var result = command.ExecuteScalar();
                        return result != null ? Convert.ToInt32(result) : 0;
                    }
                }
            }
            catch (SqlException sx)
            {
                Logger.Error($"SQL Exception occurred in GetPCBAsToBuildByCustomerOrderId, Error: {sx.Message}");
                throw;
            }
            catch (Exception ex)
            {
                Logger.Error($"Exception occurred in GetPCBAsToBuildByCustomerOrderId, Error: {ex.Message}");
                throw;
            }
        }

        public DbResponse AddScrapUnit(Unit unit, int userId, string reason)
        {
            try
            {
                using (var connection = new SqlConnection(bwDb))
                {
                    connection.Open();
                    using (var command = new SqlCommand("AddScrapUnit", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.Add("@UnitId", SqlDbType.Int).Value = unit.UnitId;
                        command.Parameters.Add("@UserId", SqlDbType.Int).Value = userId;
                        command.Parameters.Add("@StageId", SqlDbType.Int).Value = unit.StageId;
                        command.Parameters.Add("@ScrapReason", SqlDbType.VarChar, 80).Value = reason;


                        using (var reader = command.ExecuteReader())
                        {
                            if (reader.HasRows)
                            {
                                reader.Read();
                                return new DbResponse
                                {
                                    StatusCode = reader.GetInt32(reader.GetOrdinal("StatusCode")),
                                    Message = reader.GetString(reader.GetOrdinal("Message"))
                                };
                            }
                            else
                            {
                                return new DbResponse
                                {
                                    StatusCode = 500,
                                    Message = "No response from stored procedure."
                                };
                            }
                        }
                    }
                }
            }
            catch (SqlException sx)
            {
                Logger.Error($"SQL Exception occurred in AddScrapUnit, Error: {sx.Message}");
                throw;
            }
            catch (Exception ex)
            {
                Logger.Error($"Exception occurred in AddScrapUnit, Error: {ex.Message}");
                throw;
            }

        }

        public DbResponse AddBomItems(List<BomItem> bomItems)
        {
            try
            {
                using (var connection = new SqlConnection(bwDb))
                {
                    connection.Open();
                    using (var command = new SqlCommand("AddBomItems", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        var jsonItems = JsonConvert.SerializeObject(bomItems);
                        command.Parameters.Add("@JsonData", SqlDbType.NVarChar).Value = jsonItems;
                        using (var reader = command.ExecuteReader())
                        {
                            if (reader.HasRows)
                            {
                                reader.Read();
                                return new DbResponse
                                {
                                    StatusCode = reader.GetInt32(reader.GetOrdinal("StatusCode")),
                                    Message = reader.GetString(reader.GetOrdinal("Message"))
                                };
                            }
                            else
                            {
                                return new DbResponse
                                {
                                    StatusCode = 500,
                                    Message = "No response from stored procedure."
                                };
                            }
                        }
                    }
                }
            }
            catch (SqlException sx)
            {
                Logger.Error($"SQL Exception occurred in AddBomItems, Error: {sx.Message}");
                throw;
            }
            catch (Exception ex)
            {
                Logger.Error($"Exception occurred in AddBomItems, Error: {ex.Message}");
                throw;
            }

        }

        public async Task<int> GetLabePrintBalanceForOrder(int CutomerOrderId)
        {
            try
            {
                using (var connection = new SqlConnection(bwDb))
                {
                    await connection.OpenAsync();
                    using (var command = new SqlCommand("GetLabelPrintBalanceForOrder", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.Add("@CustomerOrderId", SqlDbType.Int).Value = CutomerOrderId;
                        var result = await command.ExecuteScalarAsync();
                        return result != null ? Convert.ToInt32(result) : -1;
                    }
                }
            }
            catch (SqlException sx)
            {
                Logger.Error($"SQL Exception occurred in GetLabePrintBalanceForOrder, Error: {sx.Message}");
                throw;
            }
            catch (Exception ex)
            {
                Logger.Error($"Exception occurred in GetLabePrintBalanceForOrder, Error: {ex.Message}");
                throw;
            }
        }

        public DbResponse SetBlockedStateOfUnit(string serialNumber, bool isBlocked, string reasonBlocked, int userId)
        {
            try
            {
                using (var connection = new SqlConnection(bwDb))
                {
                    connection.Open();
                    using (var command = new SqlCommand("SetBlockedStateOfUnit", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.Add("@SerialNumber", SqlDbType.VarChar, 60).Value = serialNumber;
                        command.Parameters.Add("@IsBlocked", SqlDbType.Bit).Value = isBlocked ? 1 : 0;
                        command.Parameters.Add("@UserId", SqlDbType.Int).Value = userId;
                        var reasonParam = command.Parameters.Add("@ReasonBlocked", SqlDbType.VarChar, 50);
                        reasonParam.Value = string.IsNullOrEmpty(reasonBlocked) ? (object)DBNull.Value : reasonBlocked;

                        using (var reader = command.ExecuteReader())
                        {
                            if (reader.HasRows)
                            {
                                reader.Read();
                                return new DbResponse
                                {
                                    StatusCode = reader.GetInt32(reader.GetOrdinal("StatusCode")),
                                    Message = reader.GetString(reader.GetOrdinal("Message"))
                                };
                            }
                            else
                            {
                                return new DbResponse
                                {
                                    StatusCode = 500,
                                    Message = "No response from stored procedure."
                                };
                            }
                        }
                    }
                }
            }
            catch (SqlException sx)
            {
                Logger.Error($"SQL Exception occurred in SetBlockedStateOfUnit, Error: {sx.Message}");
                throw;
            }
            catch (Exception ex)
            {
                Logger.Error($"Exception occurred in SetBlockedStateOfUnit, Error: {ex.Message}");
                throw;
            }
        }

        public async Task<DbResponse> GetBlockedStateOfUnitAsync(string serialNumber)
        {
            try
            {
                using (var connection = new SqlConnection(bwDb))
                {
                    await connection.OpenAsync();
                    using (var command = new SqlCommand("GetBlockedStateOfUnit", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.Add("@SerialNumber", SqlDbType.VarChar, 60).Value = serialNumber;
                        using (var reader = await command.ExecuteReaderAsync())
                        {
                            if (reader.HasRows)
                            {
                                return await reader.ReadAsync()
                                    ? new DbResponse
                                    {
                                        StatusCode = reader.GetInt32(reader.GetOrdinal("StatusCode")),
                                        Message = reader.GetString(reader.GetOrdinal("Message"))
                                    }
                                    : new DbResponse
                                    {
                                        StatusCode = 500,
                                        Message = "No response from stored procedure."
                                    };
                            }
                            else
                            {
                                throw new Exception("No response from stored procedure.");
                            }
                        }
                    }
                }
            }
            catch (SqlException sx)
            {
                Logger.Error($"SQL Exception occurred in GetBlockedStateOfUnitAsync, Error: {sx.Message}");
                throw;
            }
            catch (Exception ex)
            {
                Logger.Error($"Exception occurred in GetBlockedStateOfUnitAsync, Error: {ex.Message}");
                throw;
            }
        }

        //public async Task<List<CartonLabel>> GetCartonLabelsAsync()
        //{
        //    try
        //    {
        //        using (var connection = new SqlConnection(bwDb))
        //        {
        //            await connection.OpenAsync();

        //            using (var cmd = new SqlCommand("GetCartonLabels", connection))
        //            {
        //                cmd.CommandType = CommandType.StoredProcedure;

        //                var items = new List<CartonLabel>();

        //                using (var reader = await cmd.ExecuteReaderAsync())
        //                {
        //                    while (await reader.ReadAsync())
        //                    {
        //                        items.Add(new CartonLabel()
        //                        {
        //                            ProductId = reader.GetInt32(reader.GetOrdinal("ProductId")),
        //                            Model = reader.GetString(reader.GetOrdinal("Model")),
        //                            EAN = reader.GetString(reader.GetOrdinal("EAN")),
        //                            TA = reader.GetString(reader.GetOrdinal("TA")),
        //                            Dimensions = reader.GetString(reader.GetOrdinal("Dimensions")),
        //                            Weight = reader.GetString(reader.GetOrdinal("Weight")),
        //                            IsActive = reader.GetBoolean(reader.GetOrdinal("IsActive")),
        //                            HasIcasaTypeApproval = reader.GetBoolean(reader.GetOrdinal("HasIcasaTypeApproval"))
        //                        });
        //                    }
        //                }

        //                return items;
        //            }
        //        }
        //    }
        //    catch (SqlException sx)
        //    {
        //        Logger.Error($"Exception occurred in GetSetSerialLabelsAsync, Error: {sx.Message}");
        //        throw;
        //    }
        //    catch (Exception ex)
        //    {
        //        Logger.Error($"Exception occurred in GetSetSerialLabelsAsync, Error: {ex.Message}");
        //        throw;
        //    }
        //}

        //public DbResponse LogCartonLabelPrint(CartonLabel setSerialLabel)
        //{
        //    try
        //    {
        //        using (var connection = new SqlConnection(bwDb))
        //        {
        //            connection.Open();
        //            using (var command = new SqlCommand("LogCartonLabelPrint", connection))
        //            {
        //                command.CommandType = CommandType.StoredProcedure;
        //                command.Parameters.Add("@ProductId", SqlDbType.Int).Value = setSerialLabel.ProductId;
        //                command.Parameters.Add("@Model", SqlDbType.VarChar, 20).Value = setSerialLabel.Model;
        //                command.Parameters.Add("@SetSerialNumber", SqlDbType.VarChar, 60).Value = setSerialLabel.SetSerialNumber;
        //                command.Parameters.Add("@EAN", SqlDbType.VarChar, 50).Value = setSerialLabel.EAN;
        //                command.Parameters.Add("@TA", SqlDbType.VarChar, 50).Value = setSerialLabel.TA;
        //                command.Parameters.Add("@Dimensions", SqlDbType.VarChar, 50).Value = setSerialLabel.Dimensions;
        //                command.Parameters.Add("@Weight", SqlDbType.VarChar, 50).Value = setSerialLabel.Weight;
        //                command.Parameters.Add("@UserId", SqlDbType.Int).Value = setSerialLabel.UserId;


        //                using (var reader = command.ExecuteReader())
        //                {
        //                    if (reader.HasRows)
        //                    {
        //                        reader.Read();
        //                        return new DbResponse
        //                        {
        //                            StatusCode = reader.GetInt32(reader.GetOrdinal("StatusCode")),
        //                            Message = reader.GetString(reader.GetOrdinal("Message"))
        //                        };
        //                    }
        //                    else
        //                    {
        //                        return new DbResponse
        //                        {
        //                            StatusCode = 500,
        //                            Message = "No response from stored procedure."
        //                        };
        //                    }
        //                }
        //            }
        //        }
        //    }
        //    catch (SqlException sx)
        //    {
        //        Logger.Error($"Exception occurred in LogCartonLabelPrint, Error: {sx.Message}");
        //        throw;
        //    }
        //    catch (Exception ex)
        //    {
        //        Logger.Error($"Exception occurred in LogCartonLabelPrint, Error: {ex.Message}");
        //        throw;
        //    }
        //}

        //public DbResponse LogTclReplacementBoard(TclReplacementBoard tclReplacementBoard, User user)
        //{
        //    try
        //    {
        //        using (var connection = new SqlConnection(bwDb))
        //        {
        //            connection.Open();
        //            using (var command = new SqlCommand("LogTclReplacementBoard", connection))
        //            {
        //                command.CommandType = CommandType.StoredProcedure;
        //                command.Parameters.Add("@UnitId", SqlDbType.Int).Value = tclReplacementBoard.UnitId;
        //                command.Parameters.Add("@TclSerialNumber", SqlDbType.VarChar, 50).Value = tclReplacementBoard.TclSerialNumber;
        //                command.Parameters.Add("@UserId", SqlDbType.Int).Value = user.UserId;



        //                using (var reader = command.ExecuteReader())
        //                {
        //                    if (reader.HasRows)
        //                    {
        //                        reader.Read();
        //                        return new DbResponse
        //                        {
        //                            StatusCode = reader.GetInt32(reader.GetOrdinal("StatusCode")),
        //                            Message = reader.GetString(reader.GetOrdinal("Message"))
        //                        };
        //                    }
        //                    else
        //                    {
        //                        return new DbResponse
        //                        {
        //                            StatusCode = 500,
        //                            Message = "No response from stored procedure."
        //                        };
        //                    }
        //                }
        //            }
        //        }
        //    }
        //    catch (SqlException sx)
        //    {
        //        Logger.Error($"Exception occurred in LogTclReplacementBoard, Error: {sx.Message}");
        //        throw;
        //    }
        //    catch (Exception ex)
        //    {
        //        Logger.Error($"Exception occurred in LogTclReplacementBoard, Error: {ex.Message}");
        //        throw;
        //    }
        //}

        //public async Task<List<ContainerDto>> GetContainersDueInAsync()
        //{
        //    try
        //    {
        //        using (var connection = new SqlConnection(bwDb))
        //        {
        //            await connection.OpenAsync();
        //            using (var cmd = new SqlCommand("GetContainersDueIn", connection))
        //            {
        //                cmd.CommandType = CommandType.StoredProcedure;

        //                using (var reader = await cmd.ExecuteReaderAsync())
        //                {
        //                    var containerList = new List<ContainerDto>();
        //                    while (await reader.ReadAsync())
        //                    {
        //                        containerList.Add(MapContainerDto(reader));
        //                    }
        //                    return containerList;
        //                }
        //            }
        //        }
        //    }
        //    catch (SqlException sx)
        //    {
        //        Logger.Error($"Exception occurred in GetContainerInfoByCustomerOrderIdAsync, Error: {sx.Message}");
        //        throw;
        //    }
        //    catch (Exception ex)
        //    {
        //        Logger.Error($"Exception occurred in GetContainerInfoByCustomerOrderIdAsync, Error: {ex.Message}");
        //        throw;
        //    }
        //}
        //private ContainerDto MapContainerDto(SqlDataReader reader)
        //{
        //    return new ContainerDto()
        //    {
        //        BOMNumber = reader.GetString(reader.GetOrdinal("BOMNumber")),
        //        OrderQuantity = reader.GetInt32(reader.GetOrdinal("OrderQuantity")),
        //        Odf = reader.GetString(reader.GetOrdinal("ODF")),
        //        KitFormatName = reader.GetString(reader.GetOrdinal("KitFormatName")),
        //        DeliveryTypeName = reader.GetString(reader.GetOrdinal("DeliveryTypeName")),
        //        StatusName = reader.GetString(reader.GetOrdinal("StatusName")),
        //        VoyageNumber = reader.GetString(reader.GetOrdinal("VoyageNumber")),
        //        ContainerNumber = reader.GetString(reader.GetOrdinal("ContainerNumber")),
        //        ExpectedArrivalDate = reader.GetString(reader.GetOrdinal("ExpectedArrivalDate")),
        //        RevisedETA = reader.GetString(reader.GetOrdinal("RevisedETA")),
        //        HasArrived = reader.GetBoolean(reader.GetOrdinal("HasArrived"))
        //    };
        //}

        public Product GetProductBySetSerialNumber(string setSerialNumber)
        {
            try
            {
                using (var connection = new SqlConnection(bwDb))
                {
                    connection.Open();
                    using (var command = new SqlCommand("GetProductBySetSerialNumber", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.Add("@SetSerialNumber", SqlDbType.VarChar, 60).Value = setSerialNumber;
                        using (var reader = command.ExecuteReader())
                        {
                            if (reader.HasRows)
                            {
                                reader.Read();
                                return MapProduct(reader);

                            }
                            else
                            {
                                return null; // or throw an exception if preferred
                            }
                        }
                    }
                }
            }
            catch (SqlException sx)
            {
                Logger.Error($"Exception occurred in GetProductBySetSerialNumber, Error: {sx.Message}");
                throw;
            }
            catch (Exception ex)
            {
                Logger.Error($"Exception occurred in GetProductBySetSerialNumber, Error: {ex.Message}");
                throw;
            }
        }

        //public Product GetProductByOdf(string odf)
        //{
        //    try
        //    {
        //        using (var connection = new SqlConnection(bwDb))
        //        {
        //            connection.Open();
        //            using (var command = new SqlCommand("GetProductByOdf", connection))
        //            {
        //                command.CommandType = CommandType.StoredProcedure;
        //                command.Parameters.Add("@Odf", SqlDbType.VarChar, 60).Value = odf;
        //                using (var reader = command.ExecuteReader())
        //                {
        //                    if (reader.HasRows)
        //                    {
        //                        reader.Read();
        //                        return MapProduct(reader);

        //                    }
        //                    else
        //                    {
        //                        return null; // or throw an exception if preferred
        //                    }
        //                }
        //            }
        //        }
        //    }
        //    catch (SqlException sx)
        //    {
        //        Logger.Error($"Exception occurred in GetProductByOdf, Error: {sx.Message}");
        //        throw;
        //    }
        //    catch (Exception ex)
        //    {
        //        Logger.Error($"Exception occurred in GetProductByOdf, Error: {ex.Message}");
        //        throw;
        //    }
        //}

        //public async Task<DbResponse> UpdateInboundShipmentArrivalsAsync(List<InboundShipment> updatedShipments)
        //{
        //    try
        //    {
        //        using (var connection = new SqlConnection(bwDb))
        //        {
        //            using (var command = new SqlCommand("UpdateInboundShipmentArrivals", connection))
        //            {
        //                command.CommandType = CommandType.StoredProcedure;

        //                string jsonContainers = JsonConvert.SerializeObject(updatedShipments);

        //                command.Parameters.AddWithValue("@json", jsonContainers);

        //                await connection.OpenAsync();
        //                using (var reader = await command.ExecuteReaderAsync())
        //                {
        //                    await reader.ReadAsync();

        //                    if (reader.HasRows)
        //                    {
        //                        return new DbResponse
        //                        {
        //                            StatusCode = reader.GetInt32(reader.GetOrdinal("StatusCode")),
        //                            Message = reader.GetString(reader.GetOrdinal("Message"))
        //                        };
        //                    }
        //                    else
        //                    {
        //                        return new DbResponse
        //                        {
        //                            StatusCode = 500,
        //                            Message = "No response from stored procedure."
        //                        };

        //                    }
        //                }
        //            }
        //        }
        //    }
        //    catch (SqlException sx)
        //    {
        //        Logger.Error($"Exception occurred in UpdateContainerArrivalsAsync, Error: {sx.Message}");
        //        throw;
        //    }
        //    catch (Exception ex)
        //    {
        //        Logger.Error($"Exception occurred in UpdateContainerArrivalsAsync, Error: {ex.Message}");
        //        throw;
        //    }
        //}


        public DbResponse UnlinkModulesFromUnit(string serialNumnber, int userId)
        {
            try
            {
                using (var connection = new SqlConnection(bwDb))
                {
                    connection.Open();
                    using (var command = new SqlCommand("UnlinkModules", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.Add("@PcbaSerialNumber", SqlDbType.VarChar, 60).Value = serialNumnber;
                        command.Parameters.Add("@UserId", SqlDbType.Int).Value = userId;


                        using (var reader = command.ExecuteReader())
                        {
                            if (reader.HasRows)
                            {
                                reader.Read();
                                return new DbResponse
                                {
                                    StatusCode = reader.GetInt32(reader.GetOrdinal("StatusCode")),
                                    Message = reader.GetString(reader.GetOrdinal("Message"))
                                };
                            }
                            else
                            {
                                return new DbResponse
                                {
                                    StatusCode = 500,
                                    Message = "No response from stored procedure."
                                };
                            }
                        }
                    }
                }
            }
            catch (SqlException sx)
            {
                Logger.Error($"Exception occurred in UnlinkModulesFromUnit, Error: {sx.Message}");
                throw;
            }
            catch (Exception ex)
            {
                Logger.Error($"Exception occurred in UnlinkModulesFromUnit, Error: {ex.Message}");
                throw;
            }
        }

        public DbResponse RevertUnitToStage(string pcbaSerialNumber, int newStageId, int userId, bool isFaulty)
        {
            try
            {
                using (var connection = new SqlConnection(bwDb))
                {
                    connection.Open();
                    using (var command = new SqlCommand("RevertUnitToStage", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.Add("@PcbaSerialNumber", SqlDbType.VarChar, 60).Value = pcbaSerialNumber;
                        command.Parameters.Add("@NewStageId", SqlDbType.Int).Value = newStageId;
                        command.Parameters.Add("@UserId", SqlDbType.Int).Value = userId;
                        command.Parameters.Add("@IsFaulty", SqlDbType.Bit).Value = isFaulty;
                        using (var reader = command.ExecuteReader())
                        {
                            if (reader.HasRows)
                            {
                                reader.Read();
                                return new DbResponse
                                {
                                    StatusCode = reader.GetInt32(reader.GetOrdinal("StatusCode")),
                                    Message = reader.GetString(reader.GetOrdinal("Message"))
                                };
                            }
                            else
                            {
                                return new DbResponse
                                {
                                    StatusCode = 500,
                                    Message = "No response from stored procedure."
                                };
                            }
                        }
                    }
                }
            }
            catch (SqlException sx)
            {
                Logger.Error($"Exception occurred in RevertUnitToStage, Error: {sx.Message}");
                throw;
            }
            catch (Exception ex)
            {
                Logger.Error($"Exception occurred in RevertUnitToStage, Error: {ex.Message}");
                throw;
            }

        }

        public DbResponse SwapSerialNumbers(string oldSerialNumber, string newSerialNumber, string newOdf, int userId)
        {
            try
            {
                using (var connection = new SqlConnection(bwDb))
                {
                    connection.Open();
                    using (var command = new SqlCommand("SwapSerial", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.Add("@OldSerialNumber", SqlDbType.VarChar, 60).Value = oldSerialNumber;
                        command.Parameters.Add("@NewSerialNumber", SqlDbType.VarChar, 60).Value = newSerialNumber;
                        command.Parameters.Add("@UserId", SqlDbType.Int).Value = userId;
                        command.Parameters.Add("@NewOdf", SqlDbType.VarChar, 20).Value = newOdf;
                        using (var reader = command.ExecuteReader())
                        {
                            if (reader.HasRows)
                            {
                                reader.Read();
                                return new DbResponse
                                {
                                    StatusCode = reader.GetInt32(reader.GetOrdinal("StatusCode")),
                                    Message = reader.GetString(reader.GetOrdinal("Message"))
                                };
                            }
                            else
                            {
                                return new DbResponse
                                {
                                    StatusCode = 500,
                                    Message = "No response from stored procedure."
                                };
                            }
                        }
                    }
                }
            }
            catch (SqlException sx)
            {
                Logger.Error($"Exception occurred in SwapSerialNumbers, Error: {sx.Message}");
                throw;
            }
            catch (Exception ex)
            {
                Logger.Error($"Exception occurred in SwapSerialNumbers, Error: {ex.Message}");
                throw;
            }
        }

        //public async Task<List<InboundShipment>> GetInboundShipmentsAsync()
        //{

        //    try
        //    {
        //        using (var connection = new SqlConnection(bwDb))
        //        {
        //            await connection.OpenAsync();
        //            using (var cmd = new SqlCommand("GetInboundShipmentsDueIn", connection))
        //            {
        //                cmd.CommandType = CommandType.StoredProcedure;

        //                using (var reader = await cmd.ExecuteReaderAsync())
        //                {
        //                    var shipments = new List<InboundShipment>();
        //                    while (await reader.ReadAsync())
        //                    {
        //                        shipments.Add(MapInboudShipment(reader));
        //                    }
        //                    return shipments;
        //                }
        //            }
        //        }
        //    }
        //    catch (SqlException sx)
        //    {
        //        Logger.Error($"Exception occurred in GetInboundShipmentsAsync, Error: {sx.Message}");
        //        throw;
        //    }
        //    catch (Exception ex)
        //    {
        //        Logger.Error($"Exception occurred in GetInboundShipmentsAsync, Error: {ex.Message}");
        //        throw;
        //    }

        //}

        //private InboundShipment MapInboudShipment(SqlDataReader reader)
        //{
        //    return new InboundShipment()
        //    {
        //        InboundShipmentId = reader.GetInt32(reader.GetOrdinal("InboundShipmentId")),
        //        FactoryCode = reader.GetString(reader.GetOrdinal("FactoryCode")),
        //        FactoryName = reader.GetString(reader.GetOrdinal("FactoryName")),
        //        SalesOrder = reader.GetString(reader.GetOrdinal("SalesOrder")),
        //        ContainerNumber = reader.GetString(reader.GetOrdinal("ContainerNumber")),
        //        CustomerModel = reader.GetString(reader.GetOrdinal("CustomerModel")),
        //        MaterialNumber = reader.GetString(reader.GetOrdinal("MaterialNumber")),
        //        Odf = reader.GetString(reader.GetOrdinal("Odf")),
        //        OdfQuantity = reader.GetInt32(reader.GetOrdinal("OdfQuantity")),
        //        ReceiptQuantity = reader.GetInt32(reader.GetOrdinal("ReceiptQuantity")),
        //        ExpectedArrivalTime = reader.GetDateTime(reader.GetOrdinal("ExpectedArrivalTime")),
        //        RevisedETA = reader.GetDateTime(reader.GetOrdinal("RevisedETA")),
        //        ReceiptDate = reader.IsDBNull(reader.GetOrdinal("ReceiptDate")) ? (DateTime?)null : reader.GetDateTime(reader.GetOrdinal("ReceiptDate")),
        //        VoyageNumber = reader.IsDBNull(reader.GetOrdinal("VoyageNumber")) ? null : reader.GetString(reader.GetOrdinal("VoyageNumber")),
        //        Remarks = reader.IsDBNull(reader.GetOrdinal("Remarks")) ? null : reader.GetString(reader.GetOrdinal("Remarks")),
        //        //BuildTypeId = reader.IsDBNull(reader.GetOrdinal("BuildTypeId")) ? 0 : reader.GetInt32(reader.GetOrdinal("BuildTypeId")),
        //        PanelCode = reader.IsDBNull(reader.GetOrdinal("PanelCode")) ? null : reader.GetString(reader.GetOrdinal("PanelCode")),
        //        ContentType = reader.IsDBNull(reader.GetOrdinal("ContentType")) ? null : reader.GetString(reader.GetOrdinal("ContentType")),
        //        HasArrived = reader.IsDBNull(reader.GetOrdinal("HasArrived")) ? false : reader.GetBoolean(reader.GetOrdinal("HasArrived"))
        //    };
        //}

        //public async Task<DbResponse> UpdateVoyageArrivalDateAsync(List<InboundShipment> shipments, DateTime newArrivaldate)
        //{
        //    try
        //    {
        //        using (var connection = new SqlConnection(bwDb))
        //        {
        //            await connection.OpenAsync();
        //            using (var command = new SqlCommand("UpdateVoyageArrivalDate", connection))
        //            {
        //                command.CommandType = CommandType.StoredProcedure;


        //                var json = JsonConvert.SerializeObject(shipments);

        //                command.Parameters.Add("@json", SqlDbType.NVarChar, -1).Value = json;
        //                command.Parameters.Add("@NewArrivalDate", SqlDbType.DateTime).Value = newArrivaldate;
        //                using (var reader = await command.ExecuteReaderAsync())
        //                {
        //                    if (reader.HasRows)
        //                    {
        //                        await reader.ReadAsync();
        //                        return new DbResponse
        //                        {
        //                            StatusCode = reader.GetInt32(reader.GetOrdinal("StatusCode")),
        //                            Message = reader.GetString(reader.GetOrdinal("Message"))
        //                        };
        //                    }
        //                    else
        //                    {
        //                        return new DbResponse
        //                        {
        //                            StatusCode = 500,
        //                            Message = "No response from stored procedure."
        //                        };
        //                    }
        //                }
        //            }
        //        }
        //    }
        //    catch (SqlException sx)
        //    {
        //        Logger.Error($"Exception occurred in UpdateVoyageArrivalDateAsync, Error: {sx.Message}");
        //        throw;
        //    }
        //    catch (Exception ex)
        //    {
        //        Logger.Error($"Exception occurred in UpdateVoyageArrivalDateAsync, Error: {ex.Message}");
        //        throw;
        //    }
        //}

        //public async Task<DbResponse> UpdateContainerArrivalDateAsync(int inboundShipmentId, DateTime newArrivalDate)
        //{
        //    try
        //    {
        //        using (var connection = new SqlConnection(bwDb))
        //        {
        //            await connection.OpenAsync();
        //            using (var command = new SqlCommand("UpdateContainerArrivalDate", connection))
        //            {
        //                command.CommandType = CommandType.StoredProcedure;
        //                command.Parameters.Add("@InboundShipmentId", SqlDbType.Int).Value = inboundShipmentId;
        //                command.Parameters.Add("@RevisedETA", SqlDbType.DateTime).Value = newArrivalDate;
        //                using (var reader = await command.ExecuteReaderAsync())
        //                {
        //                    if (reader.HasRows)
        //                    {
        //                        await reader.ReadAsync();
        //                        return new DbResponse
        //                        {
        //                            StatusCode = reader.GetInt32(reader.GetOrdinal("StatusCode")),
        //                            Message = reader.GetString(reader.GetOrdinal("Message"))
        //                        };
        //                    }
        //                    else
        //                    {
        //                        return new DbResponse
        //                        {
        //                            StatusCode = 500,
        //                            Message = "No response from stored procedure."
        //                        };
        //                    }
        //                }
        //            }
        //        }
        //    }
        //    catch (SqlException sx)
        //    {
        //        Logger.Error($"Exception occurred in UpdateContainerArrivalDateAsync, Error: {sx.Message}");
        //        throw;
        //    }
        //    catch (Exception ex)
        //    {
        //        Logger.Error($"Exception occurred in UpdateContainerArrivalDateAsync, Error: {ex.Message}");
        //        throw;
        //    }
        //}

        public async Task<ReportDefinition> GetReportDefinitionByIdAsync(int reportId)
        {
            try
            {
                using (var connection = new SqlConnection(bwDb))
                {
                    await connection.OpenAsync();
                    using (var command = new SqlCommand("GetReportDefinitionById", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.Add("@ReportId", SqlDbType.Int).Value = reportId;

                        using (var reader = await command.ExecuteReaderAsync())
                        {
                            if (reader.HasRows)
                            {
                                await reader.ReadAsync();
                                return MapReportDefinition(reader);
                            }
                            else
                            {
                                return null;
                            }
                        }
                    }
                }
            }
            catch (SqlException sx)
            {
                Logger.Error($"Exception occurred in GetReportDefinitionByIdAsync, Error: {sx.Message}");
                throw;
            }
            catch (Exception ex)
            {
                Logger.Error($"Exception occurred in GetReportDefinitionByIdAsync, Error: {ex.Message}");
                throw;
            }
        }

        private ReportDefinition MapReportDefinition(SqlDataReader reader)
        {
            return new ReportDefinition()
            {
                ReportId = reader.GetInt32(reader.GetOrdinal("ReportId")),
                ReportName = reader.GetString(reader.GetOrdinal("ReportName")),
                DefinitionText = reader.GetString(reader.GetOrdinal("DefinitionText"))
            };
        }

        //public async Task<List<StorageLocation>> GetStorageLocationsAsync()
        //{

        //    try
        //    {
        //        using (var connection = new SqlConnection(bwDb))
        //        {
        //            await connection.OpenAsync();
        //            using (var command = new SqlCommand("GetStorageLocations", connection))
        //            {
        //                command.CommandType = CommandType.StoredProcedure;

        //                using (var reader = await command.ExecuteReaderAsync())
        //                {
        //                    var storageLocations = new List<StorageLocation>();
        //                    while (await reader.ReadAsync())
        //                    {
        //                        storageLocations.Add(MapStorageLocation(reader));
        //                    }
        //                    return storageLocations;
        //                }
        //            }
        //        }
        //    }
        //    catch (SqlException sx)
        //    {
        //        Logger.Error($"Exception occurred in UpdateContainerArrivalDateAsync, Error: {sx.Message}");
        //        throw;
        //    }
        //    catch (Exception ex)
        //    {
        //        Logger.Error($"Exception occurred in UpdateContainerArrivalDateAsync, Error: {ex.Message}");
        //        throw;
        //    }
        //}

        //private StorageLocation MapStorageLocation(SqlDataReader reader)
        //{
        //    return new StorageLocation()
        //    {
        //        StorageLocationId = reader.GetInt32(reader.GetOrdinal("StorageLocationId")),
        //        LocationName = reader.GetString(reader.GetOrdinal("LocationName")),
        //        IsActive = reader.GetBoolean(reader.GetOrdinal("IsActive"))
        //    };
        //}

        public DbResponse AddUnitToStorageLocation(Unit unit, User user, int storageLocationId)
        {
            try
            {
                using (var connection = new SqlConnection(bwDb))
                {
                    connection.Open();
                    using (var command = new SqlCommand("AddUnitToStorageLocation", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.Add("@UnitId", SqlDbType.Int).Value = unit.UnitId;
                        command.Parameters.Add("@UserId", SqlDbType.Int).Value = user.UserId;
                        command.Parameters.Add("@StageId", SqlDbType.Int).Value = unit.StageId;
                        command.Parameters.Add("@StorageLocationId", SqlDbType.Int).Value = storageLocationId;

                        using (var reader = command.ExecuteReader())
                        {
                            if (reader.HasRows)
                            {
                                reader.Read();

                                return new DbResponse
                                {
                                    StatusCode = reader.GetInt32(reader.GetOrdinal("StatusCode")),
                                    Message = reader.GetString(reader.GetOrdinal("Message"))
                                };
                            }
                            else
                            {
                                return new DbResponse
                                {
                                    StatusCode = 500,
                                    Message = "No response from stored procedure."
                                };
                            }
                        }
                    }
                }
            }
            catch (SqlException sx)
            {
                Logger.Error($"Exception occurred in AddUnitToStorageLocation, Error: {sx.Message}");
                throw;
            }
            catch (Exception ex)
            {
                Logger.Error($"Exception occurred in AddUnitToStorageLocation, Error: {ex.Message}");
                throw;
            }
        }

        public async  Task<List<BuildType>> GetBuildTypesAsync(bool? isActive = null)
        {
            try
            {
                using (var connection = new SqlConnection(bwDb))
                {
                    await connection.OpenAsync();
                    using (var command = new SqlCommand("GetBuildTypes", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;

                        if (isActive.HasValue)
                            command.Parameters.Add("@IsActive", SqlDbType.Bit).Value = isActive.Value;
                        else
                            command.Parameters.Add("@IsActive", SqlDbType.Bit).Value = DBNull.Value;


                        using (var reader = await command.ExecuteReaderAsync())
                        {
                            if (reader.HasRows)
                            {
                                var types = new List<BuildType>();  
                                while (await reader.ReadAsync())
                                {
                                    types.Add(MapBuildType(reader));
                                }
                                return types;
                            }
                            else
                            {
                                return null;
                            }
                        }
                    }
                }
            }
            catch (SqlException sx)
            {
                Logger.Error($"Exception occurred in GetBuildTypesAsync, Error: {sx.Message}");
                throw;
            }
            catch (Exception ex)
            {
                Logger.Error($"Exception occurred in GetBuildTypesAsync, Error: {ex.Message}");
                throw;
            }
        }
        private BuildType MapBuildType(SqlDataReader reader)
        {
            return new BuildType()
            {
                BuildTypeId = reader.GetInt32(reader.GetOrdinal("BuildTypeId")),
                BuildTypeName = reader.GetString(reader.GetOrdinal("BuildTypeName")),
                IsActive = reader.GetBoolean(reader.GetOrdinal("IsActive"))
            };
        }   
    }
}
