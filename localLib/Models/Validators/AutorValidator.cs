using FluentValidation;
using localLib.Models;
using localLib.Data;
using Microsoft.EntityFrameworkCore;

namespace localLib.Validators
{
    public class AutorValidator : AbstractValidator<Autor>
    {
        private readonly BibliotecaContext _context;

        public AutorValidator(BibliotecaContext context)
        {
            _context = context;

            RuleFor(x => x.NumePrenume)
                .NotEmpty().WithMessage("Numele și prenumele sunt obligatorii")
                .MinimumLength(3).WithMessage("Numele și prenumele trebuie să aibă cel puțin 3 caractere")
                .MaximumLength(100).WithMessage("Numele și prenumele nu pot depăși 100 caractere")
                .Matches(@"^[a-zA-ZăâîșțĂÂÎȘȚ\s\-'\.]+$").WithMessage("Numele poate conține doar litere, spații, cratimi, apostrofuri și puncte")
                .Must(BeUniqueName).WithMessage("Există deja un autor cu acest nume");

            RuleFor(x => x.DataNasterii)
                .NotEmpty().WithMessage("Data nașterii este obligatorie")
                .LessThan(DateTime.Now).WithMessage("Data nașterii trebuie să fie în trecut")
                .GreaterThan(new DateTime(1800, 1, 1)).WithMessage("Data nașterii trebuie să fie după anul 1800")
                .Must(BeReasonableAge).WithMessage("Autorul trebuie să aibă între 5 și 150 de ani");

            RuleFor(x => x.Biografie)
                .NotEmpty().WithMessage("Biografia este obligatorie")
                .MinimumLength(10).WithMessage("Biografia trebuie să aibă cel puțin 10 caractere")
                .MaximumLength(2000).WithMessage("Biografia nu poate depăși 2000 caractere");
        }

        private bool BeUniqueName(Autor autor, string numePrenume)
        {
            return !_context.Autori.Any(a => a.NumePrenume.ToLower() == numePrenume.ToLower() && a.AutorId != autor.AutorId);
        }

        private bool BeReasonableAge(DateTime dataNasterii)
        {
            var age = DateTime.Now.Year - dataNasterii.Year;
            return age >= 5 && age <= 150;
        }
    }
}