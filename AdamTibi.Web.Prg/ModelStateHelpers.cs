using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace AdamTibi.Web.Prg
{
    internal static class ModelStateHelpers
    {
        public static string SerialiseModelState(ModelStateDictionary modelState)
        {
            IEnumerable<ModelStateTransferValue> errorList = modelState
                .Select(kvp => new ModelStateTransferValue
                {
                    Key = kvp.Key,
                    AttemptedValue = kvp.Value.AttemptedValue,
                    RawValue = kvp.Value.RawValue,
                    ErrorMessages = kvp.Value.Errors.Select(err => err.ErrorMessage).ToList(),
                });

            return JsonConvert.SerializeObject(errorList);
        }

        public static ModelStateDictionary DeserialiseModelState(string serialisedErrorList)
        {
            List<ModelStateTransferValue> errorList = JsonConvert.DeserializeObject<List<ModelStateTransferValue>>(serialisedErrorList);
            ModelStateDictionary modelState = new ModelStateDictionary();

            foreach (ModelStateTransferValue item in errorList)
            {
                // if the checkbox is checked, and if the model is exported, the model 
                // state's raw value is a string[] with both values: { "true", "false" }. This is all fine.
                JArray array = item.RawValue as JArray;
                object value = array?.ToObject<string[]>() ?? item.RawValue;

                modelState.SetModelValue(item.Key, value, item.AttemptedValue);

                foreach (string error in item.ErrorMessages)
                {
                    modelState.AddModelError(item.Key, error);
                }
            }
            return modelState;
        }
    }
}
