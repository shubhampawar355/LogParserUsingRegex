using logParser;
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using Xunit;

namespace moqLogParser
{
    public class LogTest
    {
        [Fact]
        public void TestIfDelimChangeThenToStringUpdateAccordingly()
        {
            Log.Delimeter = ',';
            Assert.True((",1,INFO,22 Mar " + ((int)DateTime.Now.Year).ToString() + ",08:51 AM,this is for testing,").Equals(new Log(1, "03/22", "08:51:02", "INFO", "this is for testing").ToString()));
        }

        [Fact]
        public void TestDelimCantChangeIfAssignedToDefaultCharTypeValue()
        {
            Log.Delimeter = '\0';
           // Assert.Equal(",1,INFO,22 Mar " + ((int)DateTime.Now.Year).ToString() + ",08:51 AM,this is for testing something else,", new Log(1, "03/22", "08:51:02", "INFO", "this is for testing something else").ToString());
            Assert.True(("|1|INFO|22 Mar " + ((int) DateTime.Now.Year).ToString() + "|08:51 AM|this is for testing|").Equals(new Log(1, "03/22", "08:51:02", "INFO", "this is for testing").ToString()));
        }

        [Fact]
        public void TestIsLog()
        {
            HashSet<string> userGivenLevels = new HashSet<string>();
            userGivenLevels.Add("info");
            userGivenLevels.Add("Debug");
            Log.SetLevelsToConsider(userGivenLevels);
            Assert.True(Log.IsLog("03/22 08:51:01 INFO   :...locate_configFile: Specified configuration file: /u/user10/rsvpd1.conf"));
        }

        [Fact]
        public void TestIsLogForInvalidDate()
        {
            HashSet<string> userGivenLevels = new HashSet<string>();
            userGivenLevels.Add("info");
            userGivenLevels.Add("Debug");
            Log.SetLevelsToConsider(userGivenLevels);
            Assert.False(Log.IsLog("99/99 08:51:01 INFO   :...locate_configFile: Specified configuration file: /u/user10/rsvpd1.conf"));
        }

        [Fact]
        public void TestIsLogForInvalidLevel()
        {
            HashSet<string> userGivenLevels = new HashSet<string>();
            userGivenLevels.Add("info");
            userGivenLevels.Add("Debug");
            Log.SetLevelsToConsider(userGivenLevels);
            Assert.False(Log.IsLog("03/23 08:51:01 shubham   :...locate_configFile: Specified configuration file: /u/user10/rsvpd1.conf"));
        }

        [Fact]
        public void TestIsLogForInvalidTime()
        {
            HashSet<string> userGivenLevels = new HashSet<string>();
            userGivenLevels.Add("info");
            userGivenLevels.Add("Debug");
            Log.SetLevelsToConsider(userGivenLevels);
            Assert.False(Log.IsLog("03/23 13:13:13 INFO   :...locate_configFile: Specified configuration file: /u/user10/rsvpd1.conf"));
        }

        [Fact]
        public void TestSetLogFormat()
        {
            HashSet<string> userGivenLevels = new HashSet<string>();
            userGivenLevels.Add("info");
            userGivenLevels.Add("debug");
            Log.SetLevelsToConsider(userGivenLevels);
            Assert.True(new Regex(@"(0[1-9]|1[012])[- /.](0[1-9]|[12][0-9]|3[01])\s(0[1-9]|1[012])[:](0[1-9]|[12345][0-9])[:](0[1-9]|[12345][0-9])\s(info|debug)", RegexOptions.IgnoreCase).ToString().Equals(Log.LogFormat.ToString()));
        }
    }
}
