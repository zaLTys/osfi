using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using FluentValidation;
using NHibernate;
using NHibernate.Linq;
using StatistinesAtaskaitos.Models;
using Vic.ZubStatistika.Entities;

namespace StatistinesAtaskaitos.Validators
{
    public class UserCreateValidator : AbstractValidator<UserCreateModel>
    {
        public UserCreateValidator(ISessionFactory sessionFactory)
        {
            RuleFor(x => x.Pavarde).NotEmpty().WithMessage("Pavardė privalo būti nurodyta");
            RuleFor(x => x.Vardas).NotEmpty().WithMessage("Vardas privalo būti nurodytas");
            RuleFor(x => x.Password)
                .NotEmpty().WithMessage("Slaptažodis privalo būti nurodytas")
                .Equal(x => x.RepeatPassword).WithMessage("Slaptažodžiai nesutampa");

            RuleFor(x => x.Username).NotEmpty().WithMessage("Vartotojo vardas privalo būti nurodytas")
                .Must(x =>
                {
                    var usernameLower = (x == null ? null : x.ToLower());
                    using (var session = sessionFactory.OpenSession())
                    using (var transaction = session.BeginTransaction())
                    {
                        var existingUser = session.Query<User>()
                            .FirstOrDefault(u => u.Username.ToLower() == usernameLower);

                        return existingUser == null;
                    }
                }).WithMessage("Toks vartotojas jau egzistuoja sistemoje"); ;
        }
    }
}