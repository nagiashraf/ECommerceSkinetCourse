using Microsoft.AspNetCore.Mvc.Testing;

namespace API.Tests
{
    public class CustomWebApplicationFactory<TStartup> : WebApplicationFactory<TStartup> where TStartup: class
    {
    }
}