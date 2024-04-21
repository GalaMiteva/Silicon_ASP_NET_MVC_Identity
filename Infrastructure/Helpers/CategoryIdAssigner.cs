

using Infrastructure.Contexts;
using Infrastructure.Entities;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Silicon_MVC.Models.Views;
using System.Diagnostics;

namespace Infrastructure.Helpers;

public class CategoryIdAssigner
{
    public async Task<CategoryModel> assignIdAsync(HttpResponseMessage category)
    {
        {
            try
            {
                var jsonString = await category.Content.ReadAsStringAsync();
                var model = JsonConvert.DeserializeObject<CategoryModel>(jsonString);
                if (model != null)
                {
                    return model;
                }
            }
            catch (Exception ex) { Debug.WriteLine(ex.Message); }
            return null!;
        }
    }
}