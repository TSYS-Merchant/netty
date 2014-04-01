namespace SampleWebsite
{

    using System.Web.Services;

    /// <summary>
    ///     A simple web service that adds two numbers together.
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    public class AddTwoNumbers : System.Web.Services.WebService
    {

        [WebMethod]
        public string HelloWorld()
        {
            return "Hello World";
        }

        [WebMethod]
        public int AddNumbers(int number1, int number2)
        {
            int returnValue = number1 + number2;
            return returnValue;
        }

    }

}