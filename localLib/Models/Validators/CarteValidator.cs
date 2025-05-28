using FluentValidation;
using localLib.Models;

namespace localLib.Validators
{
    public class CarteValidator : AbstractValidator<Carte>
    {
        public CarteValidator()
        {
            RuleFor(x => x.Titlu)
                .NotEmpty().WithMessage("Titlul este obligatoriu")
                .MaximumLength(200).WithMessage("Titlul nu poate depăși 200 caractere");

            RuleFor(x => x.ISBN)
                .NotEmpty().WithMessage("ISBN este obligatoriu")
                .MaximumLength(20).WithMessage("ISBN nu poate depăși 20 caractere");

            RuleFor(x => x.Cota)
                .NotEmpty().WithMessage("Cota este obligatorie")
                .MaximumLength(50).WithMessage("Cota nu poate depăși 50 caractere");

            RuleFor(x => x.TitluInfo)
                .NotEmpty().WithMessage("Informația titlului este obligatorie")
                .MaximumLength(200).WithMessage("Informația titlului nu poate depăși 200 caractere");

            RuleFor(x => x.MentiuniResponsabilitate)
                .NotEmpty().WithMessage("Mențiunile de responsabilitate sunt obligatorii");

            RuleFor(x => x.Editie)
                .NotEmpty().WithMessage("Ediția este obligatorie")
                .MaximumLength(50).WithMessage("Ediția nu poate depăși 50 caractere");

            RuleFor(x => x.EdituraId)
                .NotEmpty().WithMessage("Editura este obligatorie");

            RuleFor(x => x.DataPublicarii)
                .NotEmpty().WithMessage("Data publicării este obligatorie")
                .LessThanOrEqualTo(DateTime.Now).WithMessage("Data publicării nu poate fi în viitor");

            RuleFor(x => x.LoculPublicarii)
                .NotEmpty().WithMessage("Locul publicării este obligatoriu")
                .MaximumLength(100).WithMessage("Locul publicării nu poate depăși 100 caractere");

            RuleFor(x => x.Bibliografie)
                .NotEmpty().WithMessage("Bibliografia este obligatorie");

            RuleFor(x => x.Descriere)
                .NotEmpty().WithMessage("Descrierea este obligatorie");

            RuleFor(x => x.NrPagini)
                .NotEmpty().WithMessage("Numărul de pagini este obligatoriu")
                .GreaterThan(0).WithMessage("Numărul de pagini trebuie să fie mai mare de 0");

            RuleFor(x => x.Pret)
                .NotEmpty().WithMessage("Prețul este obligatoriu")
                .GreaterThanOrEqualTo(0).WithMessage("Prețul nu poate fi negativ");

            RuleFor(x => x.ZonaColectieId)
                .NotEmpty().WithMessage("Zona colecției este obligatorie");

            RuleFor(x => x.LimbaId)
                .NotEmpty().WithMessage("Limba este obligatorie");

            RuleFor(x => x.TaraId)
                .NotEmpty().WithMessage("Țara este obligatorie");

            RuleFor(x => x.NumarInventar)
                .NotEmpty().WithMessage("Numărul de inventar este obligatoriu")
                .GreaterThan(0).WithMessage("Numărul de inventar trebuie să fie mai mare de 0");

            RuleFor(x => x.Paginatie)
                .NotEmpty().WithMessage("Paginația este obligatorie")
                .MaximumLength(50).WithMessage("Paginația nu poate depăși 50 caractere");

            RuleFor(x => x.Ilustratii)
                .NotEmpty().WithMessage("Informația despre ilustrații este obligatorie")
                .Must(x => x == "Da" || x == "Nu").WithMessage("Valoarea pentru ilustrații trebuie să fie 'Da' sau 'Nu'");

            RuleFor(x => x.CopertaURL)
                .NotEmpty().WithMessage("URL-ul coperții este obligatoriu")
                .MaximumLength(255).WithMessage("URL-ul coperții nu poate depăși 255 caractere")
                .Must(uri => Uri.TryCreate(uri, UriKind.Absolute, out _))
                .WithMessage("URL-ul coperții trebuie să fie valid");
        }
    }
}