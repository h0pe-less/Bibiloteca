using FluentValidation;
using localLib.Models;
using localLib.Data;
using Microsoft.EntityFrameworkCore;

namespace localLib.Validators
{
    public class ZonaColectieValidator : AbstractValidator<ZonaColectie>
    {
        private readonly BibliotecaContext _context;

        public ZonaColectieValidator(BibliotecaContext context)
        {
            _context = context;

            RuleFor(x => x.DenumireZona)
                .NotEmpty().WithMessage("Denumirea zonei este obligatorie")
                .MinimumLength(2).WithMessage("Denumirea zonei trebuie să aibă cel puțin 2 caractere")
                .MaximumLength(100).WithMessage("Denumirea zonei nu poate depăși 100 caractere")
                .Matches(@"^[a-zA-Z0-9ăâîșțĂÂÎȘȚ\s\-\.\,\(\)]+$").WithMessage("Denumirea poate conține doar litere, cifre, spații și semne de punctuație obișnuite")
                .Must(BeUniqueName).WithMessage("Există deja o zonă de colecție cu această denumire");
        }

        private bool BeUniqueName(ZonaColectie zonaColectie, string denumireZona)
        {
            return !_context.ZoneColectie.Any(z => z.DenumireZona.ToLower() == denumireZona.ToLower() && z.ZonaColectieId != zonaColectie.ZonaColectieId);
        }
    }
}