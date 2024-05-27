using Microsoft.EntityFrameworkCore;
using OrderApi.DataAccess;
using OrderApi.Models;
using OrderApi.Services;
using OrderApi.Services.Consumers;
using OrderApi.Services.QueueService;

namespace OrderApi
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();
            builder.Services.AddScoped<IOrderService, OrderService>();
            builder.Services.AddScoped<IOrderDataAccess, OrderDataAccess>();
            builder.Services.AddScoped<IClientQueueService, ClientQueueService>();

            builder.Services.AddHostedService<PaymentConsumer>();

            builder.Services.AddDbContext<OrderDbContext>(opt => opt.UseSqlServer("Server=localhost;Database=Order;Trusted_Connection=True;"));

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}