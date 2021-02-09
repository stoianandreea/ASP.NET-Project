using System.Collections.Generic;
using System.Data.Entity;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace SkinCareShop.Models
{
    // You can add profile data for the user by adding more properties to your ApplicationUser class, please visit https://go.microsoft.com/fwlink/?LinkID=317594 to learn more.
    public class ApplicationUser : IdentityUser
    {
        // many to many relationship with Product 
        public virtual ICollection<Product> Products { get; set; }

        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser> manager)
        {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            // Add custom user claims here
            return userIdentity;
        }
    }

    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext()
            : base("DefaultConnection", throwIfV1Schema: false)
        {
            /*Database.SetInitializer<ApplicationDbContext>(new Initp());*/
            /*Database.SetInitializer<ApplicationDbContext>(new DropCreateDatabaseAlways<ApplicationDbContext>());*/
        }

        public DbSet<Product> Products { get; set; }
        public DbSet<Manufacturer> Manufacturers { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Address> Addresses { get; set; }

        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }
    }

    /*public class Initp : DropCreateDatabaseAlways<ApplicationDbContext> {
        protected override void Seed(ApplicationDbContext ctx) {
            Manufacturer manufacturer1 = new Manufacturer {
                ManufacturerId = 1,
                Name = "The Ordinary",
                PhoneNumber = "0755555555",
                Email = "theOrdinary5@gmail.com"
            };

            ctx.Manufacturers.Add(manufacturer1);

            Category category1 = new Category { CategoryId = 1, Name = "Direct Acids" };
            Category category2 = new Category { CategoryId = 2, Name = "Hydrators and Oils" };

            ctx.Categories.Add(category1);
            ctx.Categories.Add(category2);

            Product product1 = new Product {
                Name = "Niacinamide",
                Description = "Niacinamide (Vitamin B3) is indicated to reduce the appearance of skin blemishes and congestion. A high 10% concentration of this vitamin is supported in the formula by zinc salt of pyrrolidone carboxylic acid to balance visible aspects of sebum activity.",
                Quantity = 30,
                ManufacturerId = manufacturer1.ManufacturerId,
                CategoryId = category1.CategoryId
            };
            Product product2 = new Product {
                Name = "Hyaluronic Acid",
                Description = "Hyaluronic Acid (HA) can attract up to 1,000 times its weight in water. The molecular size of HA determines its depth of delivery in the skin. This formulation combines low-, medium- and high-molecular weight HA, as well as a next-generation HA crosspolymer at a combined concentration of 2% for multi-depth hydration. This system is supported with the addition of Vitamin B5 which also enhances surface hydration. A more advanced HA formulation with 15 forms of HA, including precursors of HA, is offered by our brand NIOD in Multi-Molecular Hyaluronic Complex.",
                Quantity = 25,
                ManufacturerId = manufacturer1.ManufacturerId,
                CategoryId = category2.CategoryId
            };
            Product product3 = new Product {
                Name = "Test",
                Description = "TestTestTest",
                Quantity = 25,
                Manufacturer = new Manufacturer {
                    Name = "ManufTest",
                    PhoneNumber = "0755555555",
                    Email = "test@gmail.com"
                },
                Category = new Category { Name = "test" }
            };
            ctx.Products.Add(product1);
            ctx.Products.Add(product2);
            ctx.Products.Add(product3);
            ctx.SaveChanges();
            base.Seed(ctx);
        }
    }*/
}