using SIGEBI.Infrastructure;

namespace SIGEBI.Api
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            //Controladores
            builder.Services.AddControllers();

            //Swagger
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            //Conexion
            builder.Services.AddInfrastructureServices(builder.Configuration);
          
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
