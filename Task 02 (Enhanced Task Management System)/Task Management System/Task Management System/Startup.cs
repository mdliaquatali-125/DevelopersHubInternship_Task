//using Microsoft.Owin;
//using Microsoft.Owin.Security.Jwt;
//using Microsoft.Owin.Security;
//using Microsoft.IdentityModel.Tokens;
//using Owin;
//using System.Text;

//[assembly: OwinStartup(typeof(Task_Management_System.Startup))]

//namespace Task_Management_System
//{
//    public class Startup
//    {
//        public void Configuration(IAppBuilder app)
//        {
//            var key = Encoding.UTF8.GetBytes("THIS_IS_MY_SUPER_SECRET_KEY_1234567890");

//            app.UseJwtBearerAuthentication(new JwtBearerAuthenticationOptions
//            {
//                AuthenticationMode = AuthenticationMode.Active,
//                TokenValidationParameters = new TokenValidationParameters
//                {
//                    ValidateIssuer = false,
//                    ValidateAudience = false,
//                    ValidateIssuerSigningKey = true,
//                    ValidateLifetime = true,
//                    IssuerSigningKey = new SymmetricSecurityKey(key)
//                }
//            });

//            // 🔥 FIX SIGNALR BLOCK ISSUE
//            app.Use(async (context, next) =>
//            {
//                if (context.Request.Path.StartsWithSegments(new PathString("/signalr")))
//                {
//                    context.Request.Headers.Remove("Authorization");
//                }

//                await next.Invoke();
//            });

//            app.MapSignalR();
//        }
//    }
//}


using Microsoft.Owin;
using Owin;
using Microsoft.AspNet.SignalR;
using System.Text;
using Microsoft.Owin.Security.Jwt;
using Microsoft.IdentityModel.Tokens;

[assembly: OwinStartup(typeof(Task_Management_System.Startup))]

namespace Task_Management_System
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            var key = Encoding.UTF8.GetBytes("THIS_IS_MY_SUPER_SECRET_KEY_1234567890");

            // 🔐 JWT
            app.UseJwtBearerAuthentication(new JwtBearerAuthenticationOptions
            {
                TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ValidateIssuerSigningKey = true,
                    ValidateLifetime = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key)
                }
            });

            // 🔥 SignalR user mapping
            GlobalHost.DependencyResolver.Register(
                typeof(IUserIdProvider),
                () => new CustomUserIdProvider()
            );

            // 🔥 SignalR allow bypass
            app.Use(async (context, next) =>
            {
                if (context.Request.Path.StartsWithSegments(new PathString("/signalr")))
                {
                    context.Request.Headers.Remove("Authorization");
                }

                await next.Invoke();
            });

            // 🔥 SignalR enable
            app.MapSignalR();
        }
    }
}
