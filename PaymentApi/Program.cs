using Microsoft.EntityFrameworkCore;
using PaymentApi.Consumers;
using PaymentApi.Services;

namespace PaymentApi
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

            builder.Services.AddScoped<IPaymentService, PaymentService>();

            builder.Services.AddHostedService<OrderQueueConsumer>();

            builder.Services.AddDbContext<PaymentDbContext>(opt => opt.UseSqlServer("Server=localhost;Database=Payment;Trusted_Connection=True;"));
            
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