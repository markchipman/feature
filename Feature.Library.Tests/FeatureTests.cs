﻿using System;
using System.Text;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NFluent;
using System.Diagnostics;

namespace Feature.Library.Tests
{
    [TestClass]
    public class FeatureTests
    {        
        [TestMethod]
        public void CheckIsEnabledFor_ValidFlagRulesAndInfo_Correct()
        {
            //  Arrange
            string testUser = "iserra"; /* Note that iserra will return a percentage of 13% (given 1000 buckets) */
            string testGroup = "Browncoats";
            string testUrl = "lassiter";
            bool testAdmin = true;
            bool testInternal = true;

            Dictionary<FlagRule, bool> testRules = new Dictionary<FlagRule, bool>()
            {
                {new FlagRule{ Enabled = true }, true },
                {new FlagRule{ Enabled = false }, false},
                {new FlagRule{ Admin = true}, true},
                {new FlagRule{ Internal = true}, true},
                {new FlagRule{ Users = new List<string>{ "iserra", "MReynolds"} }, true},
                {new FlagRule{ Users = new List<string>{"sometestguy", "someothertestguy"} }, false},
                {new FlagRule{ Groups = new List<string>{"federation", "someothergroup"} }, false},
                {new FlagRule{ Groups = new List<string>{"travelswithjayne", "browncoats" } }, true},
                {new FlagRule{ PercentLoggedIn = 15 }, true}, /* iserra is in a bucket that is included */
                {new FlagRule{ PercentLoggedIn = 5 }, false}, /* iserra is not in a bucket that is included */
            };

            //  For each item in the test table...
            foreach (var item in testRules)
            {
                //  Act
                var retval = Feature.IsEnabledFor(item.Key, testUser, testGroup, testUrl, testInternal, testAdmin);

                //  Assert
                Assert.AreEqual(item.Value, retval);
            }
        }

        [TestMethod]
        public void GetBucket_ValidItem_ReturnsBucketNumber()
        {
            //  Arrange
            Dictionary<string, int> bucketTests = new Dictionary<string, int>()
            {
                {"mreynolds", 787},
                {"zwashburne", 987},
                {"wwashburne", 746},
                {"iserra", 136}, 
                {"jcobb", 33},
                {"kfrye", 912},
                {"stam", 146},
                {"rtam", 147},
                {"dbook", 466},
                {"", 0},
            };

            //  For each item in the test table...
            foreach (var item in bucketTests)
            {
                //  Act
                var retval = Feature.GetBucket(item.Key);

                //  Assert
                Assert.AreEqual(item.Value, retval);
            }
        }

        [TestMethod]
        public void GetBucket_NullItem_ReturnsBucketNumber()
        {
            //  Arrange
            string itemName = null;
            int expectedBucket = 0;

            //  Act
            var retval = Feature.GetBucket(itemName);

            //  Assert
            Assert.AreEqual(expectedBucket, retval);
        }        
    }
}