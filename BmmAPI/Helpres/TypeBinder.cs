using Microsoft.AspNetCore.Mvc.ModelBinding;
using Newtonsoft.Json;

namespace BmmAPI.Helpres
{
    public class TypeBinder<T> : IModelBinder
    {
        public Task BindModelAsync(ModelBindingContext bindingContext)
        {
            var propertyName= bindingContext.ModelName;
            var value=bindingContext.ValueProvider.GetValue(propertyName);

            if(value == ValueProviderResult.None)
            {
                return Task.CompletedTask;
            }
            else
            {
                try
                {
                    var deserializedValue = JsonConvert.DeserializeObject<T>(value.FirstValue);
                    bindingContext.Result = ModelBindingResult.Success(deserializedValue);
                }
                catch
                {
                    bindingContext.ModelState.TryAddModelError(propertyName, "given value is not if correct type");
                }

                return Task.CompletedTask;
                
            }
        }
    }
}
