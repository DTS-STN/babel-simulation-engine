using System;
using System.Collections.Generic;

namespace maternity_benefits.Storage.Mock
{
    public static class MockCreator
    {
        private static readonly int MINIMUM_AGE = 16;
        private static readonly int MAXIMUM_AGE = 40;

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
            if (roll < 4) { 
                return "French";
            } else {
                return "English";
            }
        }

        private static string GenerateProvince() {
            var rnd = new Random();
            var roll = rnd.Next(25);
            if (roll < 1) {
                return "Saskatchewan";
            } else if (roll < 2) { 
                return "Manitoba";
            } else if (roll < 5) { 
                return "British Columbia";
            } else if (roll < 8) { 
                return "Alberta";
            } else if (roll < 16) { 
                return "Ontario";
            } else if (roll < 17) { 
                return "Newfoundland and Labrador";
            } else if (roll < 18) { 
                return "Prince Edward Island";
            } else if (roll < 19) { 
                return "Nova Scotia";
            } else if (roll < 20) { 
                return "New Brunswick";
            } else if (roll < 21) { 
                return "Yukon";
            } else if (roll < 22) { 
                return "Northwest Territories";
            } else if (roll < 23) { 
                return "Nunavut";
            } else {
                return "Quebec";
            }
        }

        private static string GenerateEducationLevel() {
            var rnd = new Random();
            var roll = rnd.Next(20);
            if (roll < 5) {
                return "Apprenticeship";
            } else if (roll < 9) { 
                return "College";
            } else if (roll < 12) { 
                return "Primary - Elementary";
            } else if (roll < 15) { 
                return "Secondary - High School";
            } else if (roll < 19) { 
                return "University";
            } else {
                return "Other";
            }
        }

        private static decimal GenerateIncome() {
            var rnd = new Random();
            decimal baseAmount = rnd.Next(300, 1400);

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