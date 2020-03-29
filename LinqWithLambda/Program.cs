using System;
using LinqWithLambda.Entities;
using System.Collections.Generic;
using System.Linq;

namespace LinqWithLambda
{
    class Program
    {
        static void Print <T>(string message, IEnumerable<T> collection)
        {
            Console.WriteLine(message);
            foreach (T obj in collection)
            {
                Console.WriteLine(obj);                
            }
            Console.WriteLine();
        }

        static void Main(string[] args)
        {
            Category c1 = new Category() { Id = 1, Name = "Tools", Tier = 2 };
            Category c2 = new Category() { Id = 2, Name = "Computers", Tier = 1 };
            Category c3 = new Category() { Id = 3, Name = "Electronics", Tier = 1 };

            List<Product> products = new List<Product>()
            {
                new Product() {Id = 1, Name = "Computer", Price = 1100.0, Category = c2},
                new Product() { Id = 2, Name = "Hammer", Price = 90.0, Category = c1 },
                new Product() { Id = 3, Name = "TV", Price = 1700.0, Category = c3 },
                new Product() { Id = 4, Name = "Notebook", Price = 1300.0, Category = c2 },
                new Product() { Id = 5, Name = "Saw", Price = 80.0, Category = c1 },
                new Product() { Id = 6, Name = "Tablet", Price = 700.0, Category = c2 },
                new Product() { Id = 7, Name = "Camera", Price = 700.0, Category = c3 },
                new Product() { Id = 8, Name = "Printer", Price = 350.0, Category = c3 },
                new Product() { Id = 9, Name = "MacBook", Price = 1800.0, Category = c2 },
                new Product() { Id = 10, Name = "Sound Bar", Price = 700.0, Category = c3 },
                new Product() { Id = 11, Name = "Level", Price = 70.0, Category = c1 }
            };

            var r1 = products.Where(x => x.Price < 900.00 && x.Category.Tier == 1); //retorna os produtos com valor < 900 e que são do grupo Tier = 1
            Print("TIER 1 AND PRICE < 900: ", r1);

            var r2 = products.Where(x => x.Category.Name == "Tools").Select(p => p.Name); //exibe apenas o nome, para os produtos da categoria 'Tools'            
            Print("NAMES OF PRODUCTS FROM TOOLS: ", r2);

            var r3 = products.Where(x => x.Name[0] == 'C').Select(x => new { x.Name, x.Price, CategoryName = x.Category.Name }); //seleciona os produtos q iniciam com 'C',
            
            Print("NAMES STARTED WITH 'C' AND ANONYMOUS OBJECT: ", r3);                                                         // e cria um obj anonimo exibindo determinados campos

            var r4 = products.Where(x => x.Category.Tier == 1).OrderBy(x => x.Price).ThenBy(x => x.Name); //ordena pelo preço, e em seguida pelo nome
            Print("TIER 1 ORDER BY PRICE THEN BY NAME", r4);

            var r5 = r4.Skip(2).Take(4); //pula 2 elementos, e depois pega 4 elementos
            Print("TIER 1 ORDER BY PRICE THEN BY NAME SKIP 2 TAKE 4", r5);

            var r6 = products.FirstOrDefault();
            Console.WriteLine("First or default test 1: " + r6);

            var r7 = products.Where(p => p.Price > 3000.0).FirstOrDefault();
            Console.WriteLine("First or default test 2: " + r7);
            Console.WriteLine();

            var r8 = products.Where(p => p.Id == 3).SingleOrDefault(); //para retornar um único resultado
            Console.WriteLine("Single or default test 3: " + r8);

            var r9 = products.Where(p => p.Id == 30).SingleOrDefault(); //para retornar um único resultado
            Console.WriteLine("Single or default test 3: " + r9);       //neste caso n há ngm com Id = 30, então n deve retornar resultado em r9

            var r10 = products.Max(p => p.Price); 
            Console.WriteLine("Max price: " + r10);

            var r11 = products.Min(p => p.Price);
            Console.WriteLine("Min price: " + r11);

            var r12 = products.Where(p => p.Category.Id == 1).Sum(p => p.Price); //retorna a soma dos produtos da categoria 1
            Console.WriteLine("Category 1 Sum prices: " + r12);

            var r13 = products.Where(p => p.Category.Id == 1).Average(p => p.Price); //retorna a media de valor dos produtos da categoria 1
            Console.WriteLine("Category 1 Average prices: " + r13);

            var r14 = products.Where(p => p.Category.Id == 5).Select(p => p.Price).DefaultIfEmpty(0.0).Average(); //tratativa para realizar a média, sem que a exceção seja lançada
            Console.WriteLine("Category 5 Average prices: " + r14);                                              // pois não há produtos na categoria 5

            var r15 = products.Where(p => p.Category.Id == 1).Select(p => p.Price).Aggregate(0.0, (x, y) => x + y); //Map reduce ou Select and Agregate
            Console.WriteLine("Category 1 aggregate sum " + r15);                    // Esse 0.0 aqui define um valor caso o select nao retorne resultados

            var r16 = products.GroupBy(p => p.Category);
            Console.WriteLine();
            foreach (IGrouping<Category, Product> group in r16)
            {
                Console.WriteLine("Category " + group.Key.Name + ":");
                foreach (Product p in group)
                {
                    Console.WriteLine(p);
                }
                Console.WriteLine();
            }

            Console.WriteLine("-----------------------------");
            Console.WriteLine("Usando uma sintaxe alternativa semelhante ao SQL");

            var r17 =
                from p in products
                where p.Category.Tier == 1 && p.Price < 900.0
                select p;
            Print("TIER 1 AND PRICE < 900: ", r17);
                        
            var r18 =
                from p in products
                where p.Category.Name == "Tools"
                select p.Name;

            Print("NAMES OF PRODUCTS FROM TOOLS: ", r18);

            var r19 =
                from p in products
                where p.Name[0] == 'C'
                select new { p.Name, p.Price, CategoryName = p.Category.Name };
            Print("NAMES STARTED WITH 'C' AND ANONYMOUS OBJECT: ", r19);

            var r20 =
                from p in products
                where p.Category.Tier == 1
                orderby p.Name
                orderby p.Price
                select p;
            Print("TIER 1 ORDER BY PRICE THEN NAME", r20);

            var r21 =
                (from p in r4
                 select p).Skip(2).Take(4);
            Print("TIER 1 ORDER BY PRICE THEN BY NAME SKIP 2 TAKE 4 ", r21);

            var r22 =
                from p in products
                group p by p.Category;
            Console.WriteLine();
            foreach (IGrouping<Category, Product> group in r22)
            {
                Console.WriteLine("Category " + group.Key.Name + ":");
                foreach (Product p in group)
                {
                    Console.WriteLine(p);
                }
                Console.WriteLine();
            }
        }
    }
}
