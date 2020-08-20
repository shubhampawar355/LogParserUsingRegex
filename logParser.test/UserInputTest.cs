namespace logParser.test
{
    using logParser;
    using System;
    using Xunit;

    public class UserInputTest
    {
        public string[] ArgsArray;

        public UserInputTest()
        {
            string[] textArray1 = new string[9];
            textArray1[0] = "--log-dir";
            textArray1[1] = @"..\..\..\SampleInputOutput\Input\logs";
            textArray1[2] = "--csv";
            textArray1[3] = @"..\..\..\SampleInputOutput\Output\prorigo.csv";
            textArray1[4] = "--log-level";
            textArray1[5] = "Error";
            textArray1[6] = "info";
            textArray1[7] = "debug";
            textArray1[8] = "something";
            this.ArgsArray = textArray1;
        }

        [Fact]
        public void TestArgsGivenMappedWithRightFields()
        {
            UserInput instance = UserInput.GetInstance(this.ArgsArray);
            Assert.True(instance.Source.Equals(@"..\..\..\SampleInputOutput\Input\logs"));
            Assert.True(instance.Destination.Equals(@"..\..\..\SampleInputOutput\Output\prorigo.csv"));
            Assert.True(instance.UserGivenLevels.Contains("Error"));
            Assert.True(instance.UserGivenLevels.Contains("info"));
            Assert.True(instance.UserGivenLevels.Contains("debug"));
            Assert.False(instance.UserGivenLevels.Contains("something"));
        }

        [Fact]
        public void TestUserInputIgnoreWrongLevels()
        {
            Assert.False(UserInput.GetInstance(this.ArgsArray).UserGivenLevels.Contains("something"));
        }

        [Fact]
        public void TestUserInputIsSingleton()
        {
            string[] args = new string[] { "source", "level", "destination" };
            Assert.Equal<UserInput>(UserInput.GetInstance(this.ArgsArray), UserInput.GetInstance(args));
        }
    }
}

