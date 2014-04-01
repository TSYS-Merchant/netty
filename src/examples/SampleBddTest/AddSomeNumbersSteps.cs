namespace SampleBddTest
{

    using System;
    using NUnit.Framework;
    using TechTalk.SpecFlow;
    using SampleBddTest.AddService;
    using Netty;

    [Binding]
    public class AddSomeNumbersSteps
    {

        private static NettyServer _webServer;

        [BeforeFeature]
        public static void Startup()
        {
            _webServer = new NettyServer(@"..\..\..\SampleWebSite", "/", 9015);
            _webServer.Start();
        }

        [AfterFeature]
        public static void Shutdown()
        {
            _webServer.Stop();
        }

        [Given(@"I have entered the first number (.*) into the calculator")]
        public void GivenIHaveEnteredTheFirstNumberIntoTheCalculator(int p0)
        {
            ScenarioContext.Current.Add("first", p0);
        }

        [Given(@"I have entered the second number (.*) into the calculator")]
        public void GivenIHaveEnteredTheSecondNumberIntoTheCalculator(int p0)
        {
            ScenarioContext.Current.Add("second", p0);
        }

        [Given(@"The AddTwoNumbers web service")]
        public void GivenTheAddTwoNumbersWebService()
        {
        }
        
        [When(@"I call the web service to add the numbers")]
        public void WhenICallTheWebServiceToAddTheNumbers()
        {
            using (var service = new AddTwoNumbers())
            {
                var result = service.AddNumbers(
                    ScenarioContext.Current.Get<int>("first"), ScenarioContext.Current.Get<int>("second"));

                ScenarioContext.Current.Add("result", result);
            }
        }
        
        [When(@"I call the HelloWorld method")]
        public void WhenICallTheHelloWorldMethod()
        {
            using (var service = new AddTwoNumbers())
            {
                var result = service.HelloWorld();
                ScenarioContext.Current.Add("result", result);
            }
        }
        
        [Then(@"the result should be (.*)")]
        public void ThenTheResultShouldBe(int p0)
        {
            Assert.AreEqual(p0, ScenarioContext.Current.Get<int>("result"));
        }
        
        [Then(@"I should receive the message ""(.*)""")]
        public void ThenIShouldReceiveTheMessage(string p0)
        {
            Assert.AreEqual(
                p0, ScenarioContext.Current.Get<string>("result"));
        }

    }

}
