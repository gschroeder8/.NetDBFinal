using System;
using System.Linq;
using NLog;
using Microsoft.EntityFrameworkCore;

string path = Directory.GetCurrentDirectory() + "\\nlog.config";

var logger = LogManager.LoadConfiguration(path).GetCurrentClassLogger();

var db = new ProductContext();

while (true)
{
    Console.WriteLine("\n1. Display all products");
    Console.WriteLine("2. Display specific products");
    Console.WriteLine("3. Add product");
    Console.WriteLine("4. Edit product");
    Console.WriteLine("5. Delete product");
    Console.WriteLine("6. Display all categories");
    Console.WriteLine("7. Display all categories and their products");
    Console.WriteLine("8. Add category");
    Console.WriteLine("9. Edit category");
    Console.WriteLine("10. Delete category");
    Console.WriteLine("11. Search for a product");
    Console.WriteLine("12. Search for a category");
    Console.WriteLine("\nEnter 'x' to exit");
    Console.WriteLine("Enter your option:");

    var userOption = Console.ReadLine();

    if (userOption.ToLower() == "x")
    {
        break;
    }

    try
    {
        switch (userOption)
        {
            case "1":
                var products = db.Products.OrderBy(p => p.ProductName);
                Console.WriteLine("Products:");
                foreach (var p in db.Products)
                {
                    Console.WriteLine($"ID: {p.ProductId}, Name: {p.ProductName}");
                }
                break;
            case "2":
                Console.WriteLine("1. Display all product names");
                Console.WriteLine("2. Display discontinued products");
                Console.WriteLine("3. Display active products");
                var productDisplayOption = Console.ReadLine();

                IQueryable<Product> productsToDisplay = null;
                switch (productDisplayOption)
                {
                    case "1":
                        productsToDisplay = db.Products;
                        break;
                    case "2":
                        productsToDisplay = db.Products.Where(p => p.Discontinued);
                        break;
                    case "3":
                        productsToDisplay = db.Products.Where(p => !p.Discontinued);
                        break;
                    default:
                        logger.Error("Invalid option");
                        break;
                }

                if (productsToDisplay != null)
                {
                    foreach (var prod in productsToDisplay)
                    {
                        var status = prod.Discontinued ? "Discontinued" : "Active";
                        Console.WriteLine($"Name: {prod.ProductName}, Status: {status}");
                    }
                }
                break;
            case "3":
                Console.WriteLine("Enter the product name: ");
                var productName = Console.ReadLine();

                Console.WriteLine("Enter the supplier ID: ");
                var supplierId = int.Parse(Console.ReadLine());

                Console.WriteLine("Enter the category ID: ");
                var categoryId = int.Parse(Console.ReadLine());

                var category = db.Categories.FirstOrDefault(c => c.CategoryId == categoryId);
                if (category == null)
                {
                    Console.WriteLine("Category not found");
                    break;
                }

                Console.WriteLine("Is the product discontinued? (yes/no): ");
                var discontinuedInput = Console.ReadLine();
                var discontinued = discontinuedInput.ToLower() == "yes";

                var newProduct = new Product
                {
                    ProductName = productName,
                    SupplierId = supplierId,
                    CategoryId = categoryId,
                    Category = category,
                    Discontinued = discontinued
                };

                db.Products.Add(newProduct);
                db.SaveChanges();

                Console.WriteLine("Product added successfully");
                break;
            case "4":
                Console.WriteLine("Enter the product ID or name to edit: ");
                var productIdentifier = Console.ReadLine();

                Product productToEdit = null;
                if (int.TryParse(productIdentifier, out int productId))
                {
                    productToEdit = db.Products.FirstOrDefault(p => p.ProductId == productId);
                }
                else
                {
                    productToEdit = db.Products.FirstOrDefault(p => p.ProductName == productIdentifier);
                }

                if (productToEdit != null)
                {
                    Console.Write("Enter the new name: ");
                    var newName = Console.ReadLine();
                    productToEdit.ProductName = newName;
                    db.SaveChanges();
                }
                else
                {
                    logger.Error("Product not found");
                    logger.Info("Trying to edit a product that does not exist");
                }
                break;
            case "5":
                Console.WriteLine("Enter the product ID or name to delete: ");
                var productToDeleteIdentifier = Console.ReadLine();

                Product productToDelete = null;
                if (int.TryParse(productToDeleteIdentifier, out int productToDeleteId))
                {
                    productToDelete = db.Products.FirstOrDefault(p => p.ProductId == productToDeleteId);
                }
                else
                {
                    productToDelete = db.Products.FirstOrDefault(p => p.ProductName == productToDeleteIdentifier);
                }

                if (productToDelete != null)
                {
                    db.Products.Remove(productToDelete);
                    db.SaveChanges();
                }
                else
                {
                    logger.Error("Product not found");
                    logger.Info("Trying to delete a product that does not exist");
                }
                break;
            case "6":
                var categories = db.Categories.OrderBy(c => c.CategoryName);
                Console.WriteLine("Categories:");
                foreach (var cat in db.Categories)
                {
                    var description = cat.Description != null ? cat.Description : "NULL";
                    Console.WriteLine($"ID: {cat.CategoryId}, Name: {cat.CategoryName}, Description: {description}");
                }
                break;
            case "7":
                foreach (var cat in db.Categories.ToList())
                {
                    Console.WriteLine($"Category ID: {cat.CategoryId}, Name: {cat.CategoryName}");

                    var productsInCategory = db.Products.Where(p => p.CategoryId == cat.CategoryId && !p.Discontinued).ToList();
                    foreach (var p in productsInCategory)
                    {
                        var discontinuedStatus = p.Discontinued ? "Discontinued" : "Active";
                        Console.WriteLine($"\tProduct ID: {p.ProductId}, Name: {p.ProductName}, Status: {discontinuedStatus}");
                    }
                }
                break;
            case "8":
                Console.Write("Enter a name for a new category: ");
                var categoryName = Console.ReadLine();

                Console.Write("Enter a description for the new category: ");
                var categoryDescription = Console.ReadLine();

                var newCategory = new Category { CategoryName = categoryName, Description = categoryDescription };
                db.Categories.Add(newCategory);
                db.SaveChanges();

                Console.WriteLine("Category added successfully");
                break;
            case "9":
                Console.WriteLine("Enter the category ID or name to edit: ");
                var categoryIdentifier = Console.ReadLine();

                Category categoryToEdit = null;
                if (int.TryParse(categoryIdentifier, out int catId))
                {
                    categoryToEdit = db.Categories.FirstOrDefault(c => c.CategoryId == catId);
                }
                else
                {
                    categoryToEdit = db.Categories.FirstOrDefault(c => c.CategoryName == categoryIdentifier);
                }

                if (categoryToEdit != null)
                {
                    Console.Write("Enter the new name: ");
                    var newName = Console.ReadLine();
                    categoryToEdit.CategoryName = newName;
                    db.SaveChanges();
                }
                else
                {
                    logger.Error("Category not found");
                    logger.Info("Trying to edit a category that does not exist");
                }
                break;
            case "10":
                Console.WriteLine("Enter the ID of the category to delete: ");
                var categoId = int.Parse(Console.ReadLine());

                var categ = db.Categories.Include(c => c.Products).FirstOrDefault(c => c.CategoryId == categoId);
                if (categ == null)
                {
                    Console.WriteLine("Category not found");
                    break;
                }

                if (categ.Products.Any())
                {
                    Console.WriteLine("Cannot delete this category");
                    logger.Error("Cannot delete this category");
                    logger.Info("Category cannot be deleted with products stored");
                    break;
                }
                else
                {
                    logger.Error("Category not found");
                    logger.Info("Trying to delete a category that does not exist");
                }

                db.Categories.Remove(categ);
                db.SaveChanges();

                Console.WriteLine("Category deleted successfully");

                break;
            case "11":
                Console.WriteLine("Enter the product ID or name to search for: ");
                var productIdent = Console.ReadLine();

                Product productToSearch = null;
                if (int.TryParse(productIdent, out int prodId))
                {
                    productToSearch = db.Products.FirstOrDefault(p => p.ProductId == prodId);
                }
                else
                {
                    productToSearch = db.Products.FirstOrDefault(p => p.ProductName == productIdent);
                }

                if (productToSearch != null)
                {
                    var suppId = productToSearch.SupplierId;
                    var categId = productToSearch.CategoryId;
                    var catName = productToSearch.Category != null ? productToSearch.Category.CategoryName : "NULL";
                    var disc = productToSearch.Discontinued ? "Discontinued" : "Active";

                    Console.WriteLine($"ID: {productToSearch.ProductId}, Name: {productToSearch.ProductName}, Supplier ID: {suppId}, Category ID: {categId}, Category: {catName}, Status: {disc}");
                }
                else
                {
                    logger.Error("Product not found");
                    logger.Info("Trying to search for a product that does not exist");
                }
                break;
            case "12":
                Console.WriteLine("Enter the category ID or name to search for: ");
                var catIdent = Console.ReadLine();

                Category categoryToSearch = null;
                if (int.TryParse(catIdent, out int catgId))
                {
                    categoryToSearch = db.Categories.Include(c => c.Products).FirstOrDefault(c => c.CategoryId == catgId);
                }
                else
                {
                    categoryToSearch = db.Categories.Include(c => c.Products).FirstOrDefault(c => c.CategoryName == catIdent);
                }

                if (categoryToSearch != null)
                {
                    Console.WriteLine($"Category ID: {categoryToSearch.CategoryId}, Name: {categoryToSearch.CategoryName}");
                    foreach (var p in categoryToSearch.Products)
                    {
                        Console.WriteLine($"\tProduct ID: {p.ProductId}, Name: {p.ProductName}");
                    }
                }
                else
                {
                    logger.Error("Category not found");
                    logger.Info("Trying to search for a category that does not exist");
                }
                break;
        }
    }
    catch (Exception ex)
    {
        logger.Error(ex, "An error occurred");
        Console.WriteLine($"An error occurred: {ex.Message}");
    }
}
