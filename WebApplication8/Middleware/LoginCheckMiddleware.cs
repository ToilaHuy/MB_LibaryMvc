namespace WebApplication8.Middleware
{
    public class LoginCheckMiddleware
    {
        private readonly RequestDelegate _next;

        public LoginCheckMiddleware(RequestDelegate next)
        {
            _next = next;
        }
        public async Task InvokeAsync(HttpContext context)
        {
            if (context.Request.Path.StartsWithSegments("/Home/Login"))
            {
                await _next(context);
            }
            else
            {
                if (context.Session.GetString("userName") == null)
                {
                    context.Response.Redirect("/Home/Login");
                    return;
                }
                else
                {
                    await _next(context);
                }
            }

           
        }
    }

}
