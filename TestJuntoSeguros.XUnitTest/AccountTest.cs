using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using TestJuntoSeguros.Api;
using Xunit;

namespace TestJuntoSeguros.XUnitTest
{
  public class AccountTest : IClassFixture<TestFixture<Startup>>
  {
    private HttpClient Client;
    private string _urlBase = "/api/Account";
    public AccountTest(TestFixture<Startup> fixture)
    {
      Client = fixture.Client;
    }

    [Fact]
    public async Task Register200Test()
    {
      var request = new
      {
        Url = $"{_urlBase}/register",
        Body = new
        {
          email = "pedro@email.com.br",
          password = "Pedro12!",
          confirmPassword = "Pedro12!"
        }
      };

      var response = await Client.PostAsync(request.Url, ContentHelper.GetStringContent(request.Body));
      var value = await response.Content.ReadAsStringAsync();
      //response.EnsureSuccessStatusCode();
      Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }

    [Fact]
    public async Task Register400Test()
    {
      var request = new
      {
        Url = $"{_urlBase}/register",
        Body = new
        {
          email = "pedro@email.com.br",
          password = "Pedro12",
          confirmPassword = "Pedro12!"
        }
      };

      var response = await Client.PostAsync(request.Url, ContentHelper.GetStringContent(request.Body));
      var value = await response.Content.ReadAsStringAsync();
      Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }

    [Fact]
    public async Task GetByName200Test()
    {
      var request = $"{_urlBase}/getByName?name=pedro%40email.com.br";
      var response = await Client.GetAsync(request);
      //response.EnsureSuccessStatusCode();
      Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }

    [Fact]
    public async Task GetByName400Test()
    {
      var request = $"{_urlBase}/getByName?name=teste.com";
      var response = await Client.GetAsync(request);
      //response.EnsureSuccessStatusCode();
      Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }

    [Fact]
    public async Task Login200Test()
    {
      var request = new
      {
        Url = $"{_urlBase}/register",
        Body = new
        {
          email = "pedro@email.com.br",
          password = "Pedro12!"
        }
      };

      var response = await Client.PostAsync(request.Url, ContentHelper.GetStringContent(request.Body));
      var value = await response.Content.ReadAsStringAsync();
      //response.EnsureSuccessStatusCode();
      Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }

    [Fact]
    public async Task Login400Test()
    {
      var request = new
      {
        Url = $"{_urlBase}/register",
        Body = new
        {
          email = "user",
          password = "123123"
        }
      };

      var response = await Client.PostAsync(request.Url, ContentHelper.GetStringContent(request.Body));
      var value = await response.Content.ReadAsStringAsync();
      //response.EnsureSuccessStatusCode();
      Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }


    [Fact]
    public async Task Delete200Test()
    {
      var postRequest = new
      {
        Url = $"{_urlBase}/Register",
        Body = new
        {
          email = "maria@email.com.br",
          password = "Maria12!",
          confirmPassword = "Maria12!"
        }
      };

      var postResponse = await Client.PostAsync(postRequest.Url, ContentHelper.GetStringContent(postRequest.Body));
      var jsonFromPostResponse = await postResponse.Content.ReadAsStringAsync();

      Client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", jsonFromPostResponse.Replace("\"", "").Replace(@"\", ""));
      var deleteResponse = await Client.DeleteAsync($"{ _urlBase}/Delete?name={postRequest.Body.email.Replace("@", "%40")}");

      //deleteResponse.EnsureSuccessStatusCode();

      Assert.Equal(HttpStatusCode.OK, deleteResponse.StatusCode);
    }
  }
}
