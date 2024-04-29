namespace Gauss.TccUnifaat.MVC.Services.Interfaces
{
    public interface IEmailService
    {
        Task SendEmailAsync(string emailDestinatario, string assunto, string mensagemTexto, string mensagemHtml);
    }
}
