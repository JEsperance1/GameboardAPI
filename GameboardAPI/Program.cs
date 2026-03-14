
namespace GameboardAPI
{
    public class Program
    {
        public static void Main(string[] args)
        {

            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddCors(options =>
            {
                options.AddPolicy("AllowAngular",
                    policy =>
                    {
                        policy.WithOrigins("http://localhost:4200", "https://localhost:4200")
                              .AllowAnyHeader()
                              .AllowAnyMethod();
                    });
            });




            builder.Services.AddControllers();

            builder.Services.AddOpenApi();

            var app = builder.Build();
         
            app.MapOpenApi();
          
            app.UseHttpsRedirection();

            app.UseCors("AllowAngular");

            app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }
    }
}
