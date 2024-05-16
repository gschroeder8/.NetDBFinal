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
            case "1": // Display all products

                break;
            case "2": // Display specific products

                break;
            case "3": // Add product

                break;
            case "4": // Edit product

                break;
            case "5": // Delete product

                break;
            case "6": // Display all categories

                break;
            case "7": // Display all categories and their products

                break;
            case "8": // Add category

                break;
            case "9": // Edit category

                break;
            case "10": // Delete category

                break;
            case "11": // Search for a product

                break;
            case "12": // Search for a category

                break;
        }
    }
    catch (Exception ex)
    {
        logger.Error(ex, "An error occurred");
        Console.WriteLine($"An error occurred: {ex.Message}");
    }
}