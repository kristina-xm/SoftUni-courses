﻿namespace CarDealer
{
    using AutoMapper;
    using CarDealer.Data;
    using CarDealer.DTOs.Import;
    using CarDealer.Models;
    using CarDealer.Utilities;
    using System.IO;

    public class StartUp
    {
        public static void Main()
        {
            CarDealerContext context = new CarDealerContext();

            string inputXml = File.ReadAllText("../../../Datasets/customers.xml");

            string result = ImportCustomers(context, inputXml);

            Console.WriteLine(result);
            
        }

        public static string ImportSuppliers(CarDealerContext context, string inputXml)
        {
            IMapper mapper = InitializeAutoMapper();
            
            XMLHelper helper= new XMLHelper();

            ImportSupplierDto[] suppliersDtos = helper.Deserialize<ImportSupplierDto[]>(inputXml, "Suppliers");

            ICollection<Supplier> validSuppliers = new HashSet<Supplier>();

            foreach (var supplierDto in suppliersDtos)
            {
                if (string.IsNullOrEmpty(supplierDto.Name))
                {
                    continue;
                }

                Supplier supplier = mapper.Map<Supplier>(supplierDto);

                validSuppliers.Add(supplier);

            }

            context.Suppliers.AddRange(validSuppliers);
            context.SaveChanges();

            return $"Successfully imported {validSuppliers.Count}";
        }

        public static string ImportParts(CarDealerContext context, string inputXml)
        {
            IMapper mapper = InitializeAutoMapper();

            XMLHelper helper = new XMLHelper();

            ImportPartDto[] partDtos = helper.Deserialize<ImportPartDto[]>(inputXml, "Parts");

            ICollection<Part> validParts = new HashSet<Part>();

            foreach (var partDto in partDtos)
            {
                if (string.IsNullOrEmpty(partDto.Name))
                {
                    continue;
                }

                if (!partDto.SupplierId.HasValue || !context.Suppliers.Any(s => s.Id == partDto.SupplierId))
                {
                    continue;
                }

                Part part = mapper.Map<Part>(partDto);

                validParts.Add(part);

            }

            context.Parts.AddRange(validParts);
            context.SaveChanges();

            return $"Successfully imported {validParts.Count}";
        }

        public static string ImportCars(CarDealerContext context, string inputXml)
        {
            IMapper mapper = InitializeAutoMapper();

            XMLHelper helper = new XMLHelper();

            ImportCarDto[] carDtos = helper.Deserialize<ImportCarDto[]>(inputXml, "Cars");

            ICollection<Car> validCars = new HashSet<Car>();

            foreach (var carDto in carDtos)
            {
                if (string.IsNullOrEmpty(carDto.Make) || string.IsNullOrEmpty(carDto.Model))
                {
                    continue;
                }

                Car car = mapper.Map<Car>(carDto);

                

                foreach (var partDto in carDto.Parts.DistinctBy(p => p.PartId))
                {
                    if (!context.Parts.Any(p => p.Id == partDto.PartId))
                    {
                        continue;
                    }

                    PartCar carPart = new PartCar()
                    {
                        PartId = partDto.PartId
                    };

                    car.PartsCars.Add(carPart);

                }

                validCars.Add(car);
            }

            context.Cars.AddRange(validCars);
            context.SaveChanges();

            return $"Successfully imported {validCars.Count}";

        }

        public static string ImportCustomers(CarDealerContext context, string inputXml)
        {
            IMapper mapper = InitializeAutoMapper();

            XMLHelper helper = new XMLHelper();

            ImportCustomerDto[] customersDtos = helper.Deserialize<ImportCustomerDto[]>(inputXml, "Customers");

            ICollection<Customer> validCustomers = new HashSet<Customer>();

            foreach (var custDto in customersDtos)
            {
                if (string.IsNullOrEmpty(custDto.Name) || string.IsNullOrEmpty(custDto.BirthDate))
                {
                    continue;
                }

                Customer customer = mapper.Map<Customer>(custDto);  

                validCustomers.Add(customer);
            }
            context.Customers.AddRange(validCustomers);
            context.SaveChanges();

            return $"Successfully imported {validCustomers.Count}";
        }

        private static IMapper InitializeAutoMapper()
            => new Mapper(new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<CarDealerProfile>();
            }));
    }
}