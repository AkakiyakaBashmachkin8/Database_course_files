using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab3
{
    class Employee
    {
        public int registrationNumber, salary;
        public long telephoneNumber;
        public string surname, name, patronymic, team, position, additionalDescription;
        public DateTime dateStartWork, dateStopWork;
        public static int lastRegistrationNumber = 0;

        public Employee( int salary, long telephoneNumber, string surname, string name, string patronymic, string team, string position, string additionalDescription, DateTime dateStartWork, DateTime dateStopWork)
        {
            registrationNumber = ++lastRegistrationNumber;
            this.salary = salary;
            this.telephoneNumber = telephoneNumber;
            this.surname = surname;
            this.name = name;
            this.patronymic = patronymic;
            this.team = team;
            this.position = position;
            this.additionalDescription = additionalDescription;
            this.dateStartWork = dateStartWork;
            this.dateStopWork = dateStopWork;
        }

        public string ToShortString()
        {
            return surname + " " + name + " " + patronymic + ", посада: " + position + ", відділ: " + team;
        }

        public override string ToString()
        {
            return surname + " " + name + " " + patronymic + " +" + telephoneNumber + "\nПосада: " + position + ", відділ: " + team + ", зарплата: " + salary + " грн\nПрацював з " + dateStartWork.ToShortDateString() + " по " + dateStopWork.ToShortDateString() + "\nДодатковий опис: " + additionalDescription;

        }
    }
}
