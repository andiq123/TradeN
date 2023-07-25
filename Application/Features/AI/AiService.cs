using System.Text.Json;
using System.Text.RegularExpressions;
using Application.Features.AI.DTOs;
using Application.Features.AI.Responses;
using Microsoft.Extensions.Configuration;
using OpenAI_API;
using OpenAI_API.Completions;

namespace Application.Features.AI;

public class AiService
{
    private readonly OpenAIAPI openai;

    public AiService(IConfiguration configuration)
    {
        var apiKey = configuration["OpenAI:ApiKey"];
        openai = new OpenAIAPI(apiKey);
    }

    public async Task<ResumeResponse> ResumeContent(string query)
    {
        const string preText =
            "Uita contextul trecut, Rezuma conținutul cu ajutorul câtorva cuvinte cheie (7-8 cuvinte formulate bine): \n";
        query = preText + query;

        var completionRequest = CompletionRequest(query);

        var completions = await openai.Completions.CreateCompletionAsync(completionRequest);

        var result = completions.Completions.Aggregate("", (current, completion) => current + completion.Text);
        var pattern = "[^a-zA-Z0-9 ,]";
        var cleanString = Regex.Replace(result, pattern, "");

        return new ResumeResponse() { Result = cleanString };
    }

    public async Task<string> ReorderOffers(ReorderOffersDTO request)
    {
        using var stream = new MemoryStream();
        await JsonSerializer.SerializeAsync(stream, request.Offers);
        stream.Position = 0;
        using var reader = new StreamReader(stream);
        var jsonString = await reader.ReadToEndAsync();


        var query =
            "Reordonează următoarea listă de obiecte conform propunerii mele, selectând cea mai avantajoasă opțiune." +
            $"Descrierea anunțului este: '{request.Content}'." +
            $"Autorul dorește: '{request.DesiredItem}'." +
            $"Te rog, în lista de obiecte JSON reprezentând ofertele: {jsonString}, reordonează lista și returnează obiectele JSON care conțin proprietățile 'offerId' (string) și 'rank' (int). Înlocuiește valoarea 'rank' cu un număr între 0 și numărul total de obiecte, în funcție de cât de avantajoasă este oferta de calitate pentru autor. Te rog, întoarce-mi lista de obiecte JSON reordonată conform acestor criterii și returnează-o.";

        var completionRequest = CompletionRequest(query);

        var completions = await openai.Completions.CreateCompletionAsync(completionRequest);
        return completions.Completions.Aggregate("", (current, completion) => current + completion.Text);
    }

    private static CompletionRequest CompletionRequest(string query)
    {
        var completionRequest = new CompletionRequest
        {
            Model = OpenAI_API.Models.Model.DavinciText,
            MaxTokens = 1024,
            Temperature = 1.0,
            Prompt = query,
        };
        return completionRequest;
    }
}