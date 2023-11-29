using SerializationTaskConsole.Model;
using System;
using System.Data;
using System.Runtime.CompilerServices;
using System.Text;

namespace SerializationTaskConsole
{
    public class ModelGenerator
    {
        private static Random rnd = new Random();
        private static string[] maleNames = { "Егор", "Иван", "Дмитрий", "Владимир", "Алексей", "Александр", "Андрей" };
        private static string[] femaleNames = { "Елена", "Ирина", "Светлана", "Анастасия", "Анна", "Кристина", "Татьяна" };
        private static string[] lastNames = { "Коклюев", "Морозов", "Малкин", "Голубев", "Свечников", "Тарасов"};
        private static int createdChildrenCount = 0;
        /// <summary>
        /// Мужской возраст ухода на пенсию.
        /// </summary>
        private const int PERSON_MALE_MAX_AGE = 62;
        /// <summary>
        /// Женский возраст ухода на пенсию.
        /// </summary>
        private const int PERSON_FEMALE_MAX_AGE = 57;
        private const int PERSON_MIN_AGE = 18;
        /// <summary>
        /// МРОТ.
        /// </summary>
        private const int MIN_SALARY = 16242;
        /// <summary>
        /// Максимальный размер оплаты труда в условном регионе.
        /// </summary>
        private const int MAX_SALARY = 1000000;
        /// <summary>
        /// Возраст совершеннолетия.
        /// </summary>
        private const int MAX_CHILD_AGE = 18;
        /// <summary>
        /// Количество работающих банков в стране.
        /// </summary>
        private const int BANKS_COUNT = 324;
        public static Person[] GetRandomPersons(int count)
        {
            var result = new Person[count];
            for (int i = 0; i < count; i++)
            {
                var randomGender = GetRandomGender();
                var birthDate = GetRandomDate(DateTime.Now.Year -
                    (randomGender == Gender.Male ? PERSON_MALE_MAX_AGE : PERSON_FEMALE_MAX_AGE), DateTime.Now.Year - PERSON_MIN_AGE);
                var lastName = randomGender == Gender.Male ? lastNames[rnd.Next(0, lastNames.Length)]
                    : lastNames[rnd.Next(0, lastNames.Length)] + 'а';
                result[i] = new Person()
                {
                    Id = i,
                    SequenceId = i,
                    TransportId = Guid.NewGuid(),
                    Gender = randomGender,
                    BirthDate = birthDate,
                    Age = DateTime.Now.Year - DateUnixConverter.UnixTimeStampToDateTime(birthDate).Year,
                    FirstName = randomGender == Gender.Male ? 
                    maleNames[rnd.Next(0, maleNames.Length)] : 
                    femaleNames[rnd.Next(0, femaleNames.Length)],
                    LastName = lastName,
                    CreditCardNumbers = CreateCreditCards(rnd.Next(1, BANKS_COUNT)),
                    Phones = CreatePhones(rnd.Next(1, 10)),
                    Salary = GetRamdomSalary(MIN_SALARY, MAX_SALARY),
                    IsMarred = Convert.ToBoolean(rnd.Next(0, 2)),
                    Children = GetRandomChilds(rnd.Next(1, 10), lastName, randomGender)
                };
            }
            return result;
        }
        private static string[] CreateCreditCards(int count)
        {
            string[] result = new string[count];
            int minCardNumber = 1000;
            int maxCardNumber = 10000;
            for(int i = 0; i < count; i++) 
            {
                result[i] = $"{rnd.Next(minCardNumber, maxCardNumber)} {rnd.Next(minCardNumber, maxCardNumber)} {rnd.Next(minCardNumber, maxCardNumber)} {rnd.Next(minCardNumber, maxCardNumber)}";
            }
            return result;
        }

        private static string[] CreatePhones(int count) 
        {
            string[] result = new string[count];
            int phoneMaxSize = 10;
            StringBuilder sb = new StringBuilder();
            int[] phoneNumbers;

            for(int i = 0; i < count; i++) 
            {
                sb.Append("8");
                phoneNumbers = GetRandomNumbers();
                Array.ForEach(phoneNumbers, x => sb.Append(x));
                result[i] = sb.ToString();
                sb.Clear();
            }
            
            int[] GetRandomNumbers() 
            {
                int[] randomPhoneNumers = new int[phoneMaxSize];
                for (int i = 0; i < randomPhoneNumers.Length; i++)
                {
                    randomPhoneNumers[i] = rnd.Next(1, 10);
                }
                return randomPhoneNumers;
            }
            return result;
        }

        private static Int64 GetRandomDate(int minDateValue, int maxDateValue)
        {
            DateTime minDate = new DateTime(minDateValue, 1, 1);
            DateTime maxDate = new DateTime(maxDateValue, 1, 1);
            int range = (maxDate - minDate).Days;
            Int64 result = ((DateTimeOffset)minDate.
                AddDays(rnd.Next(range))).
                ToUnixTimeSeconds();

            return result;
        }

        private static Double GetRamdomSalary(int minValue, int maxValue)
        {
            string randomValue = $"{rnd.Next(minValue, maxValue + 1)},{rnd.Next(100)}";
            if(Double.TryParse(randomValue, out Double result)) 
            {
                return result;
            }
            else 
            {
                return minValue;
            }
        }

        private static Gender GetRandomGender() 
        {
            Array values = Enum.GetValues(typeof(Gender));
            return (Gender)values.GetValue(rnd.Next(values.Length));
        }

        private static Child[] GetRandomChilds(int count, string parentLastName, Gender parentGender) 
        {
            var result = new Child[count];
            for(int i = 0; i < count; i++) 
            {
                var childGender = GetRandomGender();
                string childLastName;

                if(parentGender == Gender.Female && childGender == Gender.Male) 
                {
                    childLastName = parentLastName.TrimEnd('а');
                }
                else if(parentGender == Gender.Male && childGender == Gender.Female) 
                {
                    childLastName = $"{parentLastName}а";
                }
                else 
                {
                    childLastName = parentLastName;
                }
                result[i] = new Child() 
                {
                    Id = createdChildrenCount,
                    Gender = childGender,
                    FirstName = childGender == Gender.Male ? maleNames[rnd.Next(0, maleNames.Length)]
                    : femaleNames[rnd.Next(0, femaleNames.Length)],
                    LastName = childLastName,
                    BirthDate = GetRandomDate(DateTime.Now.Year - MAX_CHILD_AGE, DateTime.Now.Year - 1)
                };
                createdChildrenCount++;
            }
            return result;
        }
    }


}
