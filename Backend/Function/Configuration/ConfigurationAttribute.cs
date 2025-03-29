namespace Function.Configuration;

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct, AllowMultiple = true, Inherited = false)] 
public class ConfigurationAttribute : Attribute {
    public ConfigurationAttribute(string configKey) {
        ConfigKey = configKey;
    }

    public string ConfigKey {get; private init;}
}