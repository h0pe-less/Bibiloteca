using FluentValidation;
using localLib.Models;
using localLib.Data;
using Microsoft.EntityFrameworkCore;

namespace localLib.Validators
{
    public class EdituraValidator : AbstractValidator<Editura>
    {
        private readonly BibliotecaContext _context;

        public EdituraValidator(BibliotecaContext context)
        {
            _context = context;

            RuleFor(x => x.Denumire)
                .NotEmpty().WithMessage("Denumirea editurii este obligatorie")
                .MinimumLength(2).WithMessage("Denumirea editurii trebuie să aibă cel puțin 2 caractere")
                .MaximumLength(150).WithMessage("Denumirea editurii nu poate depăși 150 caractere")
                .Matches(@"^[a-zA-Z0-9ăâîșțĂÂÎȘȚ\s\-\.\,\(\)&]+$").WithMessage("Denumirea poate conține doar litere, cifre, spații și semne de punctuație obișnuite")
                .Must(BeUniqueName).WithMessage("Există deja o editură cu această denumire");
        }

        private bool BeUniqueName(Editura editura, string denumire)
        {
            return !_context.Edituri.Any(e => e.Denumire.ToLower() == denumire.ToLower() && e.EdituraId != editura.EdituraId);
        }
    }
}