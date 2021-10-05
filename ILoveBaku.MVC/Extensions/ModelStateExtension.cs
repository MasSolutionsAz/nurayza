using ILoveBaku.Application.Common.Extension;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ILoveBaku.MVC.Extensions
{
    public static class ModelStateExtension
    {
        public static void FillErrors(this ModelStateDictionary modelState, Dictionary<string, string> errors)
        {
            if (!errors.IsNull())
                foreach (var error in errors) modelState.AddModelError(error.Key, error.Value);
        }

        public static ModelValidationState GetModelValidationState(this ModelStateDictionary modelState, string key)
        {
            foreach (var item in modelState)
            {
                if (item.Key.Contains(key))
                {
                    if (item.Value.ValidationState == ModelValidationState.Invalid)
                    {
                        return ModelValidationState.Invalid;
                    }
                }
            }

            return ModelValidationState.Valid;
        }

        public static List<ModelError> GetErrors(this ModelStateDictionary modelState)
        {
            return modelState.Select(x => x.Value.Errors).Where(y => y.Count > 0).FirstOrDefault().ToList();
        }

        public static Dictionary<string, string> GetErrorsWithKey(this ModelStateDictionary modelState)
        {
            Dictionary<string, string> result = new Dictionary<string, string>();
            foreach (var modelStateKey in modelState.Keys)
            {
                var modelStateVal = modelState[modelStateKey];
                foreach (ModelError error in modelStateVal.Errors)
                {
                    result.TryAdd(modelStateKey, error.ErrorMessage);
                }
            }
            return result;
        }
    }
}
