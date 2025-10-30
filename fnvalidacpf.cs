using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.WebJobs.Extensions.Logging;
using Microsoft.Azure.webJobs.Extensions.Http;
using Newtonsoft.json;

namespace httpValidaCpf
{
    public static class fnvalidacpf
    {
        [FunctionName("fnvalidacpf")]

        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "post", Route = null)] HttpRequest req,
            ILogger log)
        {
            log.LogInformation("Iniciando a validação");

            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            dynamic data = JsonConvert.DeseralizeObject(requestBody);
            if (data == null)
            {
                return new BadRequestObjectResult("Por favor, informe o cpf");
            }
            string cpf = data?.cpf;

            return new OkObjectResult(responseMessage);
        }
        
        public static bool ValidaCPF(string cpf)
        {
            if (string.IsNullOrEmpty(cpf))
                return false;
        
            // Remove caracteres não numéricos
            cpf = new string(cpf.Where(char.IsDigit).ToArray());
        
            // Verifica se o CPF tem 11 dígitos
            if (cpf.Length != 11)
                return false;
        
            // Verifica se todos os dígitos são iguais (ex: 111.111.111-11)
            if (cpf.Distinct().Count() == 1)
                return false;
        
            // Calcula o primeiro dígito verificador
            int[] multiplicador1 = { 10, 9, 8, 7, 6, 5, 4, 3, 2 };
            int soma = 0;
            for (int i = 0; i < 9; i++)
                soma += int.Parse(cpf[i].ToString()) * multiplicador1[i];
        
            int resto = soma % 11;
            int digito1 = resto < 2 ? 0 : 11 - resto;
        
            // Calcula o segundo dígito verificador
            int[] multiplicador2 = { 11, 10, 9, 8, 7, 6, 5, 4, 3, 2 };
            soma = 0;
            for (int i = 0; i < 10; i++)
                soma += int.Parse(cpf[i].ToString()) * multiplicador2[i];
        
            resto = soma % 11;
            int digito2 = resto < 2 ? 0 : 11 - resto;
        
            // Verifica se os dígitos verificadores são válidos
            return cpf[9].ToString() == digito1.ToString() && cpf[10].ToString() == digito2.ToString();
        } 

    }

}
