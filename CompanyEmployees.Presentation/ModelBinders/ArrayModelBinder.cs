using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.ComponentModel;
using System.Reflection;

namespace CompanyEmployees.Presentation.ModelBinders;

public class ArrayModelBinder : IModelBinder
{
    // A model binder for IEnumerable type
    public Task BindModelAsync(ModelBindingContext bindingContext)
    {
        // Check if parameter is an enumerable type.
        if (!bindingContext.ModelMetadata.IsEnumerableType)
        {
            bindingContext.Result = ModelBindingResult.Failed();
            return Task.CompletedTask;
        }

        // Extract the value and convert it to string.
        var providedValue = bindingContext.ValueProvider
            .GetValue(bindingContext.ModelName)
            .ToString();

        // Check if the string is invalid.
        if (string.IsNullOrEmpty(providedValue))
        {
            bindingContext.Result = ModelBindingResult.Success(null);
            return Task.CompletedTask;
        }

        // get type of parameter.
        var genericType = bindingContext.ModelType.GetTypeInfo().GenericTypeArguments[0];
        // create a converter to a GUID type.
        var converter = TypeDescriptor.GetConverter(genericType);

        // createa an array of type object.
        var objectArray = providedValue.Split(new[] { "," }, StringSplitOptions.RemoveEmptyEntries)
            .Select(x => converter.ConvertFromString(x.Trim()))
            .ToArray();

        // createa an array of GUID type.
        var guidArray = Array.CreateInstance(genericType, objectArray.Length);
        // copy values from object array to guid array.
        objectArray.CopyTo(guidArray, 0);
        // assign the guid array to the binding context.
        bindingContext.Model = guidArray;

        bindingContext.Result = ModelBindingResult.Success(bindingContext.Model);
        return Task.CompletedTask;
    }
}
