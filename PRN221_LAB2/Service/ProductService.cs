using Microsoft.EntityFrameworkCore;
using ServerApp.Entity;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ServerApp.Service
{
    public interface IProductService
    {
        List<Product> GetProducts();
        bool OrderProduct(Order order);
    }

    public class ProductService : IProductService
    {
        private readonly northwindContext _dbContext;

        public ProductService(northwindContext dbContext)
        {
            _dbContext = dbContext;
        }

        public List<Product> GetProducts()
        {
            return _dbContext.Products.ToList();
        }

        public bool OrderProduct(Order order)
        {
            using (var transaction = _dbContext.Database.BeginTransaction())
            {
                try
                {
                    foreach (var orderDetail in order.OrderDetails)
                    {
                        PurchaseProduct(orderDetail);
                    }
                    transaction.Commit();
                    return true;
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    Console.WriteLine($"Error processing order: {ex.Message}");
                    return false;
                }
            }
        }

        private void PurchaseProduct(OrderDetail orderDetail)
        {
            const int maxRetryCount = 3;
            int retryCount = 0;
            bool success = false;

            while (!success && retryCount < maxRetryCount)
            {
                try
                {
                    var product = _dbContext.Products.FirstOrDefault(x => x.ProductId == orderDetail.ProductId)
                                  ?? throw new Exception("Product not found");

                    if (product.UnitsInStock < orderDetail.Quantity)
                    {
                        throw new Exception("Insufficient stock available");
                    }

                    product.UnitsInStock -= orderDetail.Quantity;
                    product.UpdatedTime = DateTime.UtcNow;

                    _dbContext.SaveChanges();
                    success = true;
                }
                catch (DbUpdateConcurrencyException ex)
                {
                    retryCount++;
                    foreach (var entry in ex.Entries)
                    {
                        if (entry.Entity is Product)
                        {
                            var databaseEntry = entry.GetDatabaseValues();
                            if (databaseEntry == null)
                            {
                                throw new Exception("Product has been deleted");
                            }

                            entry.OriginalValues.SetValues(databaseEntry);
                            Console.WriteLine("Concurrency conflict occurred, retrying...");
                        }
                        else
                        {
                            throw new NotSupportedException("Concurrency conflict for entity type not supported");
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error purchasing product: {ex.Message}");
                    throw;
                }
            }

            if (!success)
            {
                throw new Exception("Failed to purchase product after multiple attempts");
            }
        }
    }
}

