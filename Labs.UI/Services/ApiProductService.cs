using Labs.Domain.Entities;
using Labs.Domain.Models;

namespace Labs.UI.Services
{
    public class ApiProductService(HttpClient httpClient) : IProductService
    {
        public Task<ResponseData<PetFood>> CreateProductAsync(PetFood product, IFormFile? formFile)
        {
            throw new NotImplementedException();
        }

        public Task DeleteProductAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<ResponseData<PetFood>> GetProductByIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<ResponseData<ProductListModel<PetFood>>> GetProductListAsync(string? categoryNormalizedName, int pageNo = 1)
        {
            // Получение базового URI API-клиента (например: https://localhost:7002/api/petfoods)
            var uri = httpClient.BaseAddress;

            // Создание словаря с параметрами запроса
            var queryData = new Dictionary<string, string>();
            queryData.Add("pageNo", pageNo.ToString()); // номер страницы

            // Если указана категория — добавляем её в параметры
            if (!String.IsNullOrEmpty(categoryNormalizedName))
            {
                queryData.Add("category", categoryNormalizedName); // фильтрация по категории
            }

            // Формирование строки запроса (например: ?pageNo=1&category=cats)
            var query = QueryString.Create(queryData);

            // Отправка GET-запроса к API с параметрами
            var result = await httpClient.GetAsync(uri + query.Value);

            // Если запрос выполнен успешно — читаем тело ответа как ResponseData<ProductListModel<PetFood>>
            if (result.IsSuccessStatusCode)
            {
                return await result.Content.ReadFromJsonAsync<ResponseData<ProductListModel<PetFood>>>();
            }

            // В случае ошибки возвращаем объект с флагом ошибки
            var response = new ResponseData<ProductListModel<PetFood>> 
            { 
                Success = false, 
                ErrorMessage = "Ошибка чтения API" 
            };

            return response;
        }

        public Task UpdateProductAsync(int id, PetFood product, IFormFile? formFile)
        {
            throw new NotImplementedException();
        }
    }
}
