using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Web;
using FluentValidation;
using NHibernate;
using NHibernate.Linq;
using StatistinesAtaskaitos.Models;
using StatistinesAtaskaitos.Security;
using Vic.ZubStatistika.Entities;

namespace StatistinesAtaskaitos.Validators
{
    public class PasswordChangeValidator : AbstractValidator<SlaptazodzioKeitimasModel>
    {
        public PasswordChangeValidator(ISessionFactory sessionFactory, [LoggedIn] UserInformation loggedInUser, HashAlgorithm hashAlgorithm)
        {
            RuleFor(x => x.DabartinisSlaptazodis)
                .NotEmpty().WithMessage("Privalote nurodyti buvusį slaptažodį")
                .Must(slaptazodis =>
                      {
                            using (var session = sessionFactory.OpenSession())
                            using (var transaction = session.BeginTransaction())
                            {
                                var user = session.Query<User>().First(x => x.Id == loggedInUser.Id);

                                var hashedPassword = hashAlgorithm.GetHashedString(slaptazodis);

                                transaction.Commit();

                                return hashedPassword == user.Password;
                            }
                      })
                      .WithMessage("Neteisingas slaptažodis");

            RuleFor(x => x.NaujasSlaptazodis)
                .NotEmpty().WithMessage("Privalote nurodyti naują slaptažodį")
                .Length(5, 500).WithMessage("Slaptažodžio ilgis turi būti ne mažesnis kaip 5 simboliai")
                .Equal(model => model.PakartotasSlaptazodis).WithMessage("Slaptažodžiai nesutampa");
        }
    }
}