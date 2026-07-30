namespace cldv_act_2._1
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Added services to the container.
            builder.Services.AddSingleton<cldv_act_2._1.Services.TableStorageService>();
            builder.Services.AddSingleton<cldv_act_2._1.Services.QueueService>();
            builder.Services.AddControllersWithViews();

            var app = builder.Build();


            // Configured the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
              
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseRouting();

            app.UseAuthorization();

            app.MapStaticAssets();
            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}")
                .WithStaticAssets();

            app.Run();
        }
    }
}
