// Add this to a new file named JsonControllers.cs
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Serialization;
using PROJET_PIIA.Model;
using ErrorEventArgs = Newtonsoft.Json.Serialization.ErrorEventArgs;

namespace PROJET_PIIA.Helpers {
   
    public static class JsonHelper {
        
        public static JsonSerializerSettings GetSerializerSettings() {
            return new JsonSerializerSettings {
                TypeNameHandling = TypeNameHandling.All,
                MetadataPropertyHandling = MetadataPropertyHandling.ReadAhead,
                NullValueHandling = NullValueHandling.Ignore,
                MissingMemberHandling = MissingMemberHandling.Ignore,

               
                ContractResolver = new PlanContractResolver(),

                Error = HandleDeserializationError
            };
        }

       
        public static JsonSerializerSettings GetDeserializerSettings() {
            return new JsonSerializerSettings {
                TypeNameHandling = TypeNameHandling.All,
                NullValueHandling = NullValueHandling.Ignore,
                MissingMemberHandling = MissingMemberHandling.Ignore,
                Error = HandleDeserializationError
            };
        }

        private static void HandleDeserializationError(object sender, ErrorEventArgs errorArgs) {
            // Log the error but don't throw exceptions
            System.Diagnostics.Debug.WriteLine($"JSON Deserialization error: {errorArgs.ErrorContext.Error.Message}");
            errorArgs.ErrorContext.Handled = true;
        }

        
        public static string SerializeObject(object obj) {
            return JsonConvert.SerializeObject(obj, GetSerializerSettings());
        }

        
        public static T DeserializeObject<T>(string json) {
            return JsonConvert.DeserializeObject<T>(json, GetDeserializerSettings());
        }

       
        public static string SavePlanToJson(Plan plan, string directoryPath, string fileName = null) {
            if (plan == null)
                throw new ArgumentNullException(nameof(plan), "Le plan ne peut pas être null.");

            if (!Directory.Exists(directoryPath))
                Directory.CreateDirectory(directoryPath);

            if (string.IsNullOrEmpty(fileName)) {
                string sanitizedPlanName = string.Join("_", plan.Nom.Split(Path.GetInvalidFileNameChars()));
                fileName = $"{sanitizedPlanName}_{DateTime.Now:yyyyMMdd_HHmmss}.json";
            }

            string filePath = Path.Combine(directoryPath, fileName);

            string json = SerializeObject(plan);

            File.WriteAllText(filePath, json);

            return filePath;
        }

        
        public static Plan LoadPlanFromJson(string filePath) {
            if (!File.Exists(filePath))
                throw new FileNotFoundException("Le fichier plan n'existe pas.", filePath);

            string json = File.ReadAllText(filePath);

            try {
                Plan loadedPlan = DeserializeObject<Plan>(json);

                if (loadedPlan == null)
                    throw new InvalidOperationException("Le plan n'a pas pu être chargé correctement.");

                return loadedPlan;
            } catch (Exception ex) {
                System.Diagnostics.Debug.WriteLine($"Error loading plan: {ex.Message}");
                throw new InvalidOperationException($"Erreur lors du chargement du plan: {ex.Message}", ex);
            }
        }
    }

    
    public class PlanContractResolver : DefaultContractResolver {
        protected override JsonProperty CreateProperty(System.Reflection.MemberInfo member, MemberSerialization memberSerialization) {
            JsonProperty property = base.CreateProperty(member, memberSerialization);

            if (property.PropertyName == "Meubles" && property.DeclaringType == typeof(Plan)) {
                property.Writable = true;
            }

            return property;
        }
    }

   
    public class PlanJsonManager {
        private readonly string _baseSavePath;
        private readonly string _userName;

        public PlanJsonManager(string basePath, string userName) {
            _baseSavePath = basePath ?? Application.StartupPath;
            _userName = userName ?? "DefaultUser";
        }

       
        public string UserSaveDirectory => Path.Combine(_baseSavePath, "SavedPlans", _userName);

       
        public string SavePlan(Plan plan) {
            try {
                return JsonHelper.SavePlanToJson(plan, UserSaveDirectory);
            } catch (Exception ex) {
                throw new InvalidOperationException($"Erreur lors de la sauvegarde du plan: {ex.Message}", ex);
            }
        }

        
        public List<string> GetAllPlanFiles() {
            if (!Directory.Exists(UserSaveDirectory))
                return new List<string>();

            return new List<string>(Directory.GetFiles(UserSaveDirectory, "*.json"));
        }

        
        public List<Plan> LoadAllPlans() {
            List<Plan> plans = new List<Plan>();

            foreach (string filePath in GetAllPlanFiles()) {
                try {
                    Plan plan = JsonHelper.LoadPlanFromJson(filePath);
                    if (plan != null)
                        plans.Add(plan);
                } catch (Exception ex) {
                    System.Diagnostics.Debug.WriteLine($"Failed to load plan {filePath}: {ex.Message}");
                    // Continue with next file
                }
            }

            return plans;
        }

       
        public bool DeletePlanFile(string fileName) {
            string filePath = Path.Combine(UserSaveDirectory, fileName);

            if (!File.Exists(filePath))
                return false;

            try {
                File.Delete(filePath);
                return true;
            } catch {
                return false;
            }
        }
    }

    
    public class PointFConverter : JsonConverter<PointF> {
        public override PointF ReadJson(JsonReader reader, Type objectType, PointF existingValue, bool hasExistingValue, JsonSerializer serializer) {
            JObject jObject = JObject.Load(reader);
            float x = jObject["X"].Value<float>();
            float y = jObject["Y"].Value<float>();
            return new PointF(x, y);
        }

        public override void WriteJson(JsonWriter writer, PointF value, JsonSerializer serializer) {
            writer.WriteStartObject();
            writer.WritePropertyName("X");
            writer.WriteValue(value.X);
            writer.WritePropertyName("Y");
            writer.WriteValue(value.Y);
            writer.WriteEndObject();
        }
    }



   

public class TagsEnumConverter : JsonConverter<List<Tag>> {
        public override List<Tag> ReadJson(JsonReader reader, Type objectType,
            List<Tag> existingValue, bool hasExistingValue, JsonSerializer serializer) {
            var jObj = JObject.Load(reader);
            var arr = jObj["$values"] as JArray;
            var ids = arr?.ToObject<List<int>>() ?? new List<int>();

            // Cast each int to your Tag enum
            return ids.Select(i => (Tag)i).ToList();
        }

        public override void WriteJson(JsonWriter writer, List<Tag> value, JsonSerializer serializer) {
            
            serializer.Serialize(writer, value.Select(t => (int)t));
        }
    }

}