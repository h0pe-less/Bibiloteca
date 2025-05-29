using FluentValidation;
using localLib.Models;
using localLib.Data;
using Microsoft.EntityFrameworkCore;

namespace localLib.Validators
{
    public class CategorieValidator : AbstractValidator<Categorie>
    {
        private readonly BibliotecaContext _context;

        public CategorieValidator(BibliotecaContext context)
        {
            _context = context;

            RuleFor(x => x.Denumire)
                .NotEmpty().WithMessage("Denumirea categoriei este obligatorie")
                .MinimumLength(2).WithMessage("Denumirea categoriei trebuie să aibă cel puțin 2 caractere")
                .MaximumLength(100).WithMessage("Denumirea categoriei nu poate depăși 100 caractere")
                .Matches(@"^[a-zA-Z0-9ăâîșțĂÂÎȘȚ\s\-\.\,\(\)&]+$").WithMessage("Denumirea poate conține doar litere, cifre, spații și semne de punctuație obișnuite")
                .Must(BeUniqueName).WithMessage("Există deja o categorie cu această denumire");

            RuleFor(x => x.Descriere)
                .NotEmpty().WithMessage("Descrierea categoriei este obligatorie")
                .MinimumLength(10).WithMessage("Descrierea trebuie să aibă cel puțin 10 caractere")
                .MaximumLength(500).WithMessage("Descrierea nu poate depăși 500 caractere");
        }

        private bool BeUniqueName(Categorie categorie, string denumire)
        {
            return !_context.Categorii.Any(c => c.Denumire.ToLower() == denumire.ToLower() && c.CategorieId != categorie.CategorieId);
        }
    }
}