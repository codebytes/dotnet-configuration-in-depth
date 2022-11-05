using System.Text;
using System.Text.RegularExpressions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;

class ValidateWebHookSettingsOptions : IValidateOptions<WebHookSettings>
{
    public WebHookSettings _settings { get; private set; }

    public ValidateWebHookSettingsOptions(IConfiguration config) =>
        _settings = config.GetSection(WebHookSettings.ConfigurationSectionName)
                          .Get<WebHookSettings>();

    public ValidateOptionsResult Validate(string name, WebHookSettings options)
    {
        StringBuilder failure = new();
        var rx = new Regex(@"^[a-zA-Z''-'\s]{1,20}$");
        Match match = rx.Match(options.DisplayName);
        if (string.IsNullOrEmpty(match.Value))
        {
            failure.AppendLine($"DisplayName: {options.DisplayName} doesn't match RegEx");
        }

        return failure.Length > 0
            ? ValidateOptionsResult.Fail(failure.ToString())
            : ValidateOptionsResult.Success;
    }
}