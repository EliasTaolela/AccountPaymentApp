using System;
using System.Collections.Generic;

namespace AccountPaymentApp
{
    public class Account
    {
        public string Name { get; set; }
        public decimal Balance { get; set; }
        public decimal MonthlyPayment { get; set; }

        public Account(string name, decimal balance, decimal monthlyPayment)
        {
            Name = name;
            Balance = balance;
            MonthlyPayment = monthlyPayment;
        }

        public bool MakePayment()
        {
            if (Balance > 0)
            {
                Balance -= MonthlyPayment;
                if (Balance < 0)
                    Balance = 0;
                return true;
            }
            return false;
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            var accounts = new List<Account>();

            Console.WriteLine("Enter the number of accounts:");
            int numberOfAccounts = int.Parse(Console.ReadLine());

            for (int i = 0; i < numberOfAccounts; i++)
            {
                Console.WriteLine($"Enter details for Account {i + 1}:");

                Console.Write("Name: ");
                string name = Console.ReadLine();

                Console.Write("Initial Balance: ");
                decimal balance = decimal.Parse(Console.ReadLine());

                Console.Write("Monthly Payment: ");
                decimal monthlyPayment = decimal.Parse(Console.ReadLine());

                accounts.Add(new Account(name, balance, monthlyPayment));
            }

            Console.WriteLine("Would you like to make manual distribution or automatic distribution when an account is fully paid? (Enter 'manual' or 'automatic')");
            string distributionType = Console.ReadLine().ToLower();

            int month = 0;
            bool allPaidOff = false;

            while (!allPaidOff)
            {
                month++;
                allPaidOff = true;

                foreach (var account in accounts)
                {
                    if (account.MakePayment())
                    {
                        allPaidOff = false;
                    }
                }

                foreach (var account in accounts)
                {
                    if (account.Balance == 0 && account.MonthlyPayment > 0)
                    {
                        if (distributionType == "manual")
                        {
                            Console.WriteLine($"{account.Name} is fully paid. Where would you like to redistribute its payment?");
                            for (int j = 0; j < accounts.Count; j++)
                            {
                                if (accounts[j].Balance > 0)
                                {
                                    Console.WriteLine($"{j + 1}. {accounts[j].Name}");
                                }
                            }
                            int choice = int.Parse(Console.ReadLine());
                            accounts[choice - 1].MonthlyPayment += account.MonthlyPayment;
                        }
                        else if (distributionType == "automatic")
                        {
                            Console.WriteLine($"{account.Name} is fully paid. Redistributing its payment automatically to remaining accounts.");
                            decimal remainingPayment = account.MonthlyPayment;
                            foreach (var remainingAccount in accounts)
                            {
                                if (remainingAccount.Balance > 0)
                                {
                                    remainingAccount.MonthlyPayment += remainingPayment / (numberOfAccounts - 1);
                                }
                            }
                        }
                        account.MonthlyPayment = 0;
                    }
                }

                Console.WriteLine($"Month {month}:");
                foreach (var account in accounts)
                {
                    Console.WriteLine($"{account.Name} - Remaining Balance: {account.Balance:C}");
                }
                Console.WriteLine();
            }

            Console.WriteLine($"All accounts are paid off in {month} months.");
        }
    }
}
