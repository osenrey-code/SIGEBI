using Microsoft.EntityFrameworkCore;
using SIGEBI.Infrastructure.Persistencia;

namespace SIGEBI.Api
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            //Controladores
            builder.Services.AddControllers();
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            //Conexion
            builder.Services.AddDbContext<SIGEBIDbContext>(options =>
               options.UseSqlServer(builder.Configuration.GetConnectionString("ConexionSQL")));

            

            builder.Services.AddControllers();
           
            builder.Services.AddOpenApi();

            var app = builder.Build();
            // Encender la intefaz grafica de Swagger
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();

            //Mapear Rutas
            app.MapControllers();

            app.Run();
        }
    }
}
