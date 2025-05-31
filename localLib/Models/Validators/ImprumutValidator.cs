using FluentValidation;
using localLib.Models;

namespace localLib.Validators
{
    public class ImprumutValidator : AbstractValidator<Imprumut>
    {
        public ImprumutValidator()
        {
            RuleFor(x => x.Nume)
                .NotEmpty().WithMessage("Numele este obligatoriu")
                .MaximumLength(100).WithMessage("Numele nu poate depăși 100 caractere")
                .Matches(@"^[a-zA-ZăâîșțĂÂÎȘȚ\s\-']+$").WithMessage("Numele poate conține doar litere, spații, cratimi și apostrofuri");

            RuleFor(x => x.Prenume)
                .NotEmpty().WithMessage("Prenumele este obligatoriu")
                .MaximumLength(100).WithMessage("Prenumele nu poate depăși 100 caractere")
                .Matches(@"^[a-zA-ZăâîșțĂÂÎȘȚ\s\-']+$").WithMessage("Prenumele poate conține doar litere, spații, cratimi și apostrofuri");

            RuleFor(x => x.Grupa)
                .NotEmpty().WithMessage("Grupa este obligatorie")
                .MaximumLength(20).WithMessage("Grupa nu poate depăși 20 caractere")
                .Matches(@"^[a-zA-Z0-9\-_]+$").WithMessage("Grupa poate conține doar litere, cifre, cratimi și underscore");

            RuleFor(x => x.DataImprumut)
                .NotEmpty().WithMessage("Data împrumutului este obligatorie")
                .LessThanOrEqualTo(DateTime.Now).WithMessage("Data împrumutului nu poate fi în viitor")
                .GreaterThan(new DateTime(2000, 1, 1)).WithMessage("Data împrumutului trebuie să fie după anul 2000");

            RuleFor(x => x.DataReturnare)
                .GreaterThanOrEqualTo(x => x.DataImprumut)
                .WithMessage("Data returnării trebuie să fie după sau egală cu data împrumutului")
                .When(x => x.DataReturnare.HasValue);

            RuleFor(x => x.CarteId)
                .NotEmpty().WithMessage("Cartea este obligatorie")
                .GreaterThan(0).WithMessage("Trebuie să selectați o carte validă");

            RuleFor(x => x.DataReturnare)
                .NotNull().WithMessage("Data returnării este obligatorie când cartea este marcată ca returnată")
                .When(x => x.EsteReturnat);

            RuleFor(x => x.EsteReturnat)
                .Equal(true).WithMessage("Cartea trebuie să fie marcată ca returnată când data returnării este setată")
                .When(x => x.DataReturnare.HasValue);

            RuleFor(x => x.DataReturnare)
                .LessThanOrEqualTo(x => x.DataImprumut.AddYears(1))
                .WithMessage("Perioada de împrumut nu poate depăși un an")
                .When(x => x.DataReturnare.HasValue);

            RuleFor(x => x.DataReturnare)
                .LessThanOrEqualTo(DateTime.Now)
                .WithMessage("Data returnării nu poate fi în viitor")
                .When(x => x.DataReturnare.HasValue);
        }
    }
}