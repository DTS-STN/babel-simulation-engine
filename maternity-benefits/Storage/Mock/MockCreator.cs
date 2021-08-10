using System;
using System.Collections.Generic;

namespace maternity_benefits.Storage.Mock
{
    public static class MockCreator
    {
        private static readonly int MINIMUM_AGE = 16;
        private static readonly int MAXIMUM_AGE = 36;

        public static List<MaternityBenefitsPerson> GetMockPersons(int amount = 100) {
            var result = new List<MaternityBenefitsPerson>();

            for (int i = 0; i < amount; i++) {
                var nextPerson = GenerateMockPerson();
                result.Add(nextPerson);
            }
            return result;
        }

        private static MaternityBenefitsPerson GenerateMockPerson() {
            return new MaternityBenefitsPerson() {
                Id = Guid.NewGuid(),
                Age = GenerateAge(),
                SpokenLanguage = GenerateSpokenLanguage(),
                EducationLevel = GenerateEducationLevel(),
                Province = GenerateProvince(),
                AverageIncome = GenerateIncome()
            };
        }

        private static int GenerateAge() {
            var rnd = new Random();
            return rnd.Next(MINIMUM_AGE, MAXIMUM_AGE);
        }

        private static string GenerateSpokenLanguage() {
            var rnd = new Random();
            var roll = rnd.Next(20);
            if (roll < 3) {
                return "Other";
            } else if (roll < 10) { 
                return "French";
            } else {
                return "English";
            }
        }

        private static string GenerateProvince() {
            var rnd = new Random();
            var roll = rnd.Next(25);
            if (roll < 1) {
                return "SK";
            } else if (roll < 2) { 
                return "MB";
            } else if (roll < 5) { 
                return "BC";
            } else if (roll < 8) { 
                return "AB";
            } else if (roll < 16) { 
                return "ON";
            } else if (roll < 17) { 
                return "NL";
            } else if (roll < 18) { 
                return "PEI";
            } else if (roll < 19) { 
                return "NS";
            } else if (roll < 20) { 
                return "NB";
            } else if (roll < 21) { 
                return "YK";
            } else if (roll < 22) { 
                return "NWT";
            } else if (roll < 23) { 
                return "NT";
            } else {
                return "QC";
            }
        }

        private static string GenerateEducationLevel() {
            var rnd = new Random();
            var roll = rnd.Next(20);
            if (roll < 6) {
                return "University";
            } else if (roll < 15) { 
                return "High School";
            } else {
                return "Other";
            }
        }

        private static decimal GenerateIncome() {
            var rnd = new Random();
            decimal baseAmount = rnd.Next(300, 1300);

            var roll = rnd.Next(1, 6);
            if (roll < 2) {
                baseAmount += 200;
            }
            else if (roll < 4) {
                baseAmount -= 100;
            }
            return baseAmount;
        }
    }
}