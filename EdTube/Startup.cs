using EdTube.Data;
using EdTube.Services;
using ElectronNET.API;
using ElectronNET.API.Entities;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using Microsoft.EntityFrameworkCore;

namespace EdTube;

public class Startup
	{
		public Startup(IConfiguration configuration)
		{
			Configuration = configuration;
		}

		public IConfiguration Configuration { get; }

		// This method gets called by the runtime. Use this method to add services to the container.
		// For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
		public void ConfigureServices(IServiceCollection services)
		{
			// Add services to the container.
			var connectionString = Configuration.GetConnectionString("DefaultConnection");
			
			services.AddDbContext<ApplicationDbContext>(options =>
				options.UseNpgsql(connectionString));
			services.AddDatabaseDeveloperPageExceptionFilter();

			services.AddDefaultIdentity<IdentityUser>(options =>
				{            
					options.SignIn.RequireConfirmedEmail = false;
					options.SignIn.RequireConfirmedPhoneNumber = false;
					options.SignIn.RequireConfirmedAccount = false;
					options.User.AllowedUserNameCharacters = string.Empty;
					options.Password = new PasswordOptions
					{
						RequireDigit = false,
						RequiredLength = 0,
						RequireLowercase = false,
						RequireUppercase = false,
						RequiredUniqueChars = 0,
						RequireNonAlphanumeric = false
					};
				})
				.AddRoles<IdentityRole>()
				.AddEntityFrameworkStores<ApplicationDbContext>();
			services.AddControllersWithViews();

			services.Configure<FormOptions>(x =>
			{
				x.ValueLengthLimit = int.MaxValue;
				x.MultipartBodyLengthLimit = int.MaxValue;
				x.MultipartHeadersLengthLimit = int.MaxValue;
			});

			services.Configure<KestrelServerOptions>(x =>
			{
				x.Limits.MaxRequestBodySize = int.MaxValue;
			});

			services.AddSingleton<IVideoProvider, VideoProvider>();
			services.AddSingleton<IThumbnailProvider, ThumbnailProvider>();
		}

		// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
		public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
		{
			// Configure the HTTP request pipeline.
			if (env.IsDevelopment()) 
			{
				app.UseMigrationsEndPoint();
			}
			else
			{
				app.UseExceptionHandler("/Home/Error");
				// The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
				app.UseHsts();
			}

			app.UseHttpsRedirection();
			app.UseStaticFiles();

			app.UseRouting();

			app.UseAuthentication();
			app.UseAuthorization();

			app.UseEndpoints(endpoints =>
			{
				endpoints.MapControllerRoute(
					name: "default",
					pattern: "{controller=Home}/{action=Index}/{id?}");
				endpoints.MapRazorPages();
			});
			
			if (HybridSupport.IsElectronActive)
			{
				ElectronCreateWindow();
			}
		}

		public async void ElectronCreateWindow()
		{
			var browserWindowOptions = new BrowserWindowOptions
			{
				Width          = 1024,
				Height         = 768,
				Show           = false, // wait to open it
				WebPreferences = new WebPreferences
				{
					WebSecurity = false
				}
			};

			var browserWindow = await Electron.WindowManager.CreateWindowAsync(browserWindowOptions);
			await browserWindow.WebContents.Session.ClearCacheAsync();

			// Handler to show when it is ready
			browserWindow.OnReadyToShow += () =>
			{
				browserWindow.Show();
			};

			// Close Handler
			browserWindow.OnClose += () => Environment.Exit(0);
		}
	}