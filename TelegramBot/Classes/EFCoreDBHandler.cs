using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using TelegramBot.Models;

namespace TelegramBot.Classes
{
    class EFCoreDBHandler : IDbHelper
    {
        RecipesContext recipesContext;
        public EFCoreDBHandler()
        {
            this.recipesContext = new RecipesContext();
        }

        public List<List<string>> Search(string keyword)
        {
            List<List<string>> result = new List<List<string>>();
            string resultStr = string.Empty;

            using (recipesContext = new RecipesContext())
            {
                var temp = from rec in recipesContext.Recipes
                           join ingr in recipesContext.Ingredients on rec.RecipeId equals ingr.IngredientId
                           where ingr.Ingr1.Contains(keyword) || ingr.Ingr2.Contains(keyword) || ingr.Ingr3.Contains(keyword) ||
                           ingr.Ingr4.Contains(keyword) || ingr.Ingr5.Contains(keyword) || ingr.Ingr6.Contains(keyword) ||
                           ingr.Ingr7.Contains(keyword) || ingr.Ingr8.Contains(keyword) || ingr.Ingr9.Contains(keyword) ||
                           ingr.Ingr10.Contains(keyword)
                           select new
                           {
                               RecipeName = rec.RecipeName,
                               Ingr1 = ingr.Ingr1,
                               Ingr2 = ingr.Ingr2,
                               Ingr3 = ingr.Ingr3,
                               Ingr4 = ingr.Ingr4,
                               Ingr5 = ingr.Ingr5,
                               Ingr6 = ingr.Ingr6,
                               Ingr7 = ingr.Ingr7,
                               Ingr8 = ingr.Ingr8,
                               Ingr9 = ingr.Ingr9,
                               Ingr10 = ingr.Ingr10,
                               Description = rec.Description
                           };

                foreach (var i in temp)
                {
                    string tmp_RecipeName = $"\n{i.RecipeName}\n";
                    string[] array = { i.Ingr1, i.Ingr2, i.Ingr3, i.Ingr4, i.Ingr5, i.Ingr6, i.Ingr7, i.Ingr8, i.Ingr9, i.Ingr10 };
                    foreach (var s in array)
                    {
                        if (s != "")
                        {
                            tmp_RecipeName += s + ", ";
                        }
                        else
                        {
                            break;
                        }
                    }
                    string tmp2 = $"{i.Description}";
                    resultStr = $"{tmp_RecipeName.TrimEnd(',', ' ')}\n" + tmp2;
                    Console.WriteLine(resultStr);
                    result.Add(new List<string> { resultStr });
                }
            };
            return result;
        }
    }
}
